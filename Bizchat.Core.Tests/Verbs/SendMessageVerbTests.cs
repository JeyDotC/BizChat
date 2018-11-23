using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
using Bizchat.Core.Events;
using Bizchat.Core.Services;
using Bizchat.Core.Verbs;
using Moq;
using Shouldly;
using Xunit;

namespace Bizchat.Core.Tests.Verbs
{
    public class SendMessageVerbTests
    {
        private readonly Mock<IQueueMessagesService> _queueMessagesServiceMock = new Mock<IQueueMessagesService>();

        public SendMessageVerbTests()
        {

        }

        [Theory]
        [MemberData(nameof(BasicUseCase))]
        public async Task Run_WhenProvidedRoutersAndMessage_ShouldProduceMessagesToQueue(
            IEnumerable<IRouteMessagesService> routers, 
            ChatMessage message, 
            IEnumerable<ChatMessageSentEvent> expectedMessagesToQueue)
        {
            // Arrange
            var receivedMessagesToQueue = new List<ChatMessageSentEvent>();

            _queueMessagesServiceMock.Setup(q => q.QueueMessage(It.IsAny<ChatMessageSentEvent>()))
                .Callback<ChatMessageSentEvent>(receivedMessagesToQueue.Add)
                .Returns(Task.CompletedTask);

            var queuer = _queueMessagesServiceMock.Object;
            var messageSenderService = new SendMessageVerb(queuer, routers);

            // Act
            await messageSenderService.Run(message);

            // Assert
            this.ShouldSatisfyAllConditions(
                () => expectedMessagesToQueue.Count().ShouldBe(receivedMessagesToQueue.Count),
                () => receivedMessagesToQueue.ShouldAllBe(a => expectedMessagesToQueue.Any(e => e.Contents == a.Contents && e.RoutingKey == a.RoutingKey))
            );
        }

        public static IEnumerable<object[]> BasicUseCase
        {
            get
            {
                var simpleMessage = new ChatMessage
                {
                    Contents = "Hello World",
                    Destination = "World",
                };
                var simpleRouterMock = new Mock<IRouteMessagesService>();
                var simpleMessageToQueue = new ChatMessageSentEvent
                    {
                        Contents = simpleMessage,
                        RoutingKey = "ChatRoom"
                    };

                simpleRouterMock.Setup(r => r.ICanRoute(It.IsAny<ChatMessage>()))
                    .Returns(true);

                simpleRouterMock.Setup(r => r.ExcludeOtherRouters(It.IsAny<ChatMessage>()))
                    .Returns(false);

                simpleRouterMock.Setup(r => r.Route(It.IsAny<ChatMessage>()))
                    .Returns<ChatMessage>(message => new ChatMessageSentEvent
                    {
                        Contents = message,
                        RoutingKey = "ChatRoom"
                    });

                // Simple message.
                yield return new object[] 
                {
                    new IRouteMessagesService[]{ simpleRouterMock.Object },
                    simpleMessage,
                    new ChatMessageSentEvent[]{ simpleMessageToQueue }
                };

                var secondaryRouterMock = new Mock<IRouteMessagesService>();

                secondaryRouterMock.Setup(r => r.ICanRoute(It.IsAny<ChatMessage>()))
                   .Returns(true);

                secondaryRouterMock.Setup(r => r.ExcludeOtherRouters(It.IsAny<ChatMessage>()))
                    .Returns(false);

                secondaryRouterMock.Setup(r => r.Route(It.IsAny<ChatMessage>()))
                    .Returns<ChatMessage>(message => new ChatMessageSentEvent
                    {
                        Contents = message,
                        RoutingKey = "SecondChatRoom"
                    });

                var simpleMessageToQueue2 = new ChatMessageSentEvent
                {
                    Contents = simpleMessage,
                    RoutingKey = "SecondChatRoom"
                };

                // Multiple routers for simple message
                yield return new object[]
                {
                    new IRouteMessagesService[]{ simpleRouterMock.Object, secondaryRouterMock.Object },
                    simpleMessage,
                    new ChatMessageSentEvent[]{ simpleMessageToQueue, simpleMessageToQueue2 }
                };

                var excludingRouterMock = new Mock<IRouteMessagesService>();

                excludingRouterMock.Setup(r => r.ICanRoute(It.IsAny<ChatMessage>()))
                   .Returns(true);

                excludingRouterMock.Setup(r => r.ExcludeOtherRouters(It.IsAny<ChatMessage>()))
                    .Returns(true);

                excludingRouterMock.Setup(r => r.Route(It.IsAny<ChatMessage>()))
                    .Returns<ChatMessage>(message => new ChatMessageSentEvent
                    {
                        Contents = message,
                        RoutingKey = "MrBot"
                    });

                var messageRoutedToExcludingQueue = new ChatMessageSentEvent
                {
                    Contents = simpleMessage,
                    RoutingKey = "MrBot"
                };

                var routerWithUnsupportedMessagesMock = new Mock<IRouteMessagesService>();

                routerWithUnsupportedMessagesMock.Setup(r => r.ICanRoute(It.IsAny<ChatMessage>()))
                   .Returns(false);

                // Router with unsupported messages.
                yield return new object[]
                {
                    new IRouteMessagesService[]
                    {
                        simpleRouterMock.Object,
                        routerWithUnsupportedMessagesMock.Object,
                        secondaryRouterMock.Object
                    },
                    simpleMessage,
                    new ChatMessageSentEvent[]
                    {
                        simpleMessageToQueue,
                        simpleMessageToQueue2
                    }
                };

                // Router that excludes other routers.
                yield return new object[]
                {
                    new IRouteMessagesService[]
                    {
                        simpleRouterMock.Object,
                        excludingRouterMock.Object,
                        secondaryRouterMock.Object
                    },
                    simpleMessage,
                    new ChatMessageSentEvent[]{ messageRoutedToExcludingQueue }
                };
            }
        }
    }
}
