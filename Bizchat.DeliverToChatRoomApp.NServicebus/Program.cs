using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Bizchat.Core.Repositories;
using Bizchat.Core.Services;
using Bizchat.Core.Verbs;
using Bizchat.Ef;
using Bizchat.Ef.Repositories;
using Bizchat.NServiceBus;
using Bizchat.NServiceBus.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Ninject;
using NServiceBus;

namespace Bizchat.DeliverToChatRoomApp.NServicebus
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Group";

            var configuration = GetConfiguration();

            var kernel = ConfigureKernel(configuration.GetConnectionString("DefaultConnection"));

            var endpointConfiguration = EndPointConfigurationFactory.Create();

            endpointConfiguration.UseContainer<NinjectBuilder>(customizations =>
            {
                customizations.ExistingKernel(kernel);
            });

            var endpointInstance = await Endpoint.Start(endpointConfiguration)
                .ConfigureAwait(false);

            Console.WriteLine("Press Enter to exit.");
            Console.ReadLine();

            await endpointInstance.Stop()
                .ConfigureAwait(false);
        }

        private static IKernel ConfigureKernel(string connectionString)
        {
            var kernel = new StandardKernel();

            kernel.Bind<DbContextOptions<BizchatDbContext>>()
                .ToMethod(context => new DbContextOptionsBuilder<BizchatDbContext>()
                            .EnableSensitiveDataLogging()
                            .UseSqlServer(connectionString).Options);

            kernel.Bind<BizchatDbContext>()
                .ToSelf();

            kernel.Bind<IChatRoomsRepository>()
                .To<EfChatRoomsRepository>();

            kernel.Bind<IChatMessagesRepository>()
                .To<EfChatMessagesRepository>();

            kernel.Bind<IChatMessageReceivedByChatRoomEventsRepository>()
                .To<EfChatMessageReceivedByChatRoomEventsRepository>();

            kernel.Bind<IChatMessageSentEventsRepository>()
                .To<EfChatMessageSentEventsRepository>();

            kernel.Bind<IChatUsersRepository>()
                .To<EfChatUsersRepository>();

            kernel.Bind<IQueueMessagesService>()
                .To<NServiceBusQueueMessagesService>();

            kernel.Bind<ReceiveMessageAtChatRoomVerb>()
                .ToSelf();

            return kernel;
        }

        private static IConfiguration GetConfiguration()
            => new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();
    }
}
