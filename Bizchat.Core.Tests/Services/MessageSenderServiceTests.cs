using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bizchat.Core.Entities;
using Bizchat.Core.Services;
using Moq;
using Shouldly;
using Xunit;

namespace Bizchat.Core.Tests.Services
{
    public class MessageSenderServiceTests
    {
        private readonly Mock<IQueueMessagesService> _queueMessagesServiceMock = new Mock<IQueueMessagesService>();

        public MessageSenderServiceTests()
        {

        }

        [Theory]
        [MemberData(nameof(BasicUseCase))]
        public async Task Test1(
            IEnumerable<IRouteMessagesService> routers, 
            ChatMessage message, 
            IEnumerable<MessageToQueue> expectedMessagesToQueue)
        {
            // Arrange
            var receivedMessagesToQueue = new List<MessageToQueue>();

            _queueMessagesServiceMock.Setup(q => q.QueueMessage(It.IsAny<MessageToQueue>()))
                .Callback<MessageToQueue>(receivedMessagesToQueue.Add)
                .Returns(Task.CompletedTask);

            var queuer = _queueMessagesServiceMock.Object;
            var messageSenderService = new MessageSenderService(queuer, routers);

            // Act
            await messageSenderService.SendMessage(message);

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
                var simpleMessageToQueue = new MessageToQueue
                    {
                        Contents = simpleMessage,
                        RoutingKey = "ChatRoom"
                    };

                simpleRouterMock.Setup(r => r.ICanRoute(It.IsAny<ChatMessage>()))
                    .Returns(true);

                simpleRouterMock.Setup(r => r.ExcludeOtherRouters(It.IsAny<ChatMessage>()))
                    .Returns(false);

                simpleRouterMock.Setup(r => r.Route(It.IsAny<ChatMessage>()))
                    .Returns<ChatMessage>(message => new MessageToQueue
                    {
                        Contents = message,
                        RoutingKey = "ChatRoom"
                    });

                // Simple message.
                yield return new object[] 
                {
                    new IRouteMessagesService[]{ simpleRouterMock.Object },
                    simpleMessage,
                    new MessageToQueue[]{ simpleMessageToQueue }
                };

                var secondaryRouterMock = new Mock<IRouteMessagesService>();

                secondaryRouterMock.Setup(r => r.ICanRoute(It.IsAny<ChatMessage>()))
                   .Returns(true);

                secondaryRouterMock.Setup(r => r.ExcludeOtherRouters(It.IsAny<ChatMessage>()))
                    .Returns(false);

                secondaryRouterMock.Setup(r => r.Route(It.IsAny<ChatMessage>()))
                    .Returns<ChatMessage>(message => new MessageToQueue
                    {
                        Contents = message,
                        RoutingKey = "SecondChatRoom"
                    });

                var simpleMessageToQueue2 = new MessageToQueue
                {
                    Contents = simpleMessage,
                    RoutingKey = "SecondChatRoom"
                };

                // Multiple routers for simple message
                yield return new object[]
                {
                    new IRouteMessagesService[]{ simpleRouterMock.Object, secondaryRouterMock.Object },
                    simpleMessage,
                    new MessageToQueue[]{ simpleMessageToQueue, simpleMessageToQueue2 }
                };

                var excludingRouterMock = new Mock<IRouteMessagesService>();

                excludingRouterMock.Setup(r => r.ICanRoute(It.IsAny<ChatMessage>()))
                   .Returns(true);

                excludingRouterMock.Setup(r => r.ExcludeOtherRouters(It.IsAny<ChatMessage>()))
                    .Returns(true);

                excludingRouterMock.Setup(r => r.Route(It.IsAny<ChatMessage>()))
                    .Returns<ChatMessage>(message => new MessageToQueue
                    {
                        Contents = message,
                        RoutingKey = "MrBot"
                    });

                var messageRoutedToExcludingQueue = new MessageToQueue
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
                    new MessageToQueue[]
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
                    new MessageToQueue[]{ messageRoutedToExcludingQueue }
                };
            }
        }
    }
}
