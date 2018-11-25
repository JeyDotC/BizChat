using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
using Bizchat.Core.Events;
using Bizchat.Core.Verbs;
using RestSharp;

namespace Bizchat.StockChatBot
{
    public partial class SearchStockVerb : IVerb<ChatMessageSentEvent, ChatBotResult>
    {
        private const string RootStockApi = "https://stooq.com/q/l/";

        private static readonly Regex _messageDestinationMatch = new Regex(@"^bot://(\d+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static readonly Regex _messageFormatMatch = new Regex(@"^/stock=([a-z]+)$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly SendMessageVerb _sendMessage;
        private readonly IRestClient _restClient;

        public SearchStockVerb(SendMessageVerb sendMessage)
        {
            _sendMessage = sendMessage;
            _restClient = new RestClient(new Uri(RootStockApi));
        }

        public async Task<ChatBotResult> Run(ChatMessageSentEvent message)
        {
            var chatRoomData = CalculateChatRoomId(message.Contents.Destination);

            if (!chatRoomData.IsCorrect)
            {
                return new ChatBotResult();
            }

            var stockRequest = CalculateRequestData(message.Contents.Contents);

            if (!stockRequest.IsCorrect)
            {
                return new ChatBotResult();
            }

            // https://stooq.com/q/l/?s=aapl.us&f=sd2t2ohlcv&h&e=csv
            var request = new RestRequest(Method.GET)
                .AddQueryParameter("s", $"{stockRequest.Query.ToLower()}.us")
                .AddQueryParameter("f", "sd2t2ohlcv")
                .AddQueryParameter("h", string.Empty)
                .AddQueryParameter("e", "csv");

           var response = await _restClient.ExecuteGetTaskAsync(request);

            if (!response.IsSuccessful)
            {
                await _sendMessage.Run(new ChatMessage
                {
                    Contents = $"An error occurred when trying to get the stock info.",
                    Sender = message.Contents.Sender,
                    Destination = $"chatroom://{chatRoomData.ChatRoomId}"
                });

                return new ChatBotResult();
            }

            var parts = response.Content.Split(Environment.NewLine)[1].Split(',');
            var stockName = parts[0];
            var closingValue = parts[6];
            var contents = $"{stockName} quote is ${closingValue} per share.";

            await _sendMessage.Run(new ChatMessage
            {
                Contents = contents,
                Sender = message.Contents.Sender,
                Destination = $"chatroom://{chatRoomData.ChatRoomId}"
            });

            return new ChatBotResult();
        }

        private static CalculatedRequestData CalculateRequestData(string contents)
        {
            var match = _messageFormatMatch.Match(contents);

            if (!match.Success)
            {
                return new CalculatedRequestData();
            }

            return new CalculatedRequestData
            {
                IsCorrect = true,
                Query = match.Groups[1].Value,
            };
        }

        private static DestinationResult CalculateChatRoomId(string destination)
        {
            var match = _messageDestinationMatch.Match(destination);

            if (!match.Success)
            {
                return new DestinationResult();
            }

            return new DestinationResult
            {
                IsCorrect = true,
                ChatRoomId = int.Parse(match.Groups[1].Value),
            };
        }
    }
}
