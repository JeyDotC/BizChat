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
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            var kernel = new StandardKernel();

            var endpointConfiguration = EndPointConfigurationFactory.Create();

            kernel.Bind<DbContextOptions<BizchatDbContext>>()
                .ToMethod(context => new DbContextOptionsBuilder<BizchatDbContext>()
                            .UseSqlServer(configuration.GetConnectionString("DefaultConnection"))
                            .Options);
            kernel.Bind<BizchatDbContext>()
                .ToSelf();
            kernel.Bind<IChatRoomsRepository, EfChatRoomsRepository>();
            kernel.Bind<IChatMessagesRepository, EfChatMessagesRepository>();
            kernel.Bind<IQueueMessagesService, NServiceBusQueueMessagesService>();
            kernel.Bind<ReceiveMessageVerb>()
                .ToSelf();

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
    }
}
