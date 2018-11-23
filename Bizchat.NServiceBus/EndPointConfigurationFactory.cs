using System;
using System.Collections.Generic;
using System.Text;
using NServiceBus;

namespace Bizchat.NServiceBus
{
    public static class EndPointConfigurationFactory
    {
        public static EndpointConfiguration Create()
        {
            var endpointConfiguration = new EndpointConfiguration("Bizchat");

            endpointConfiguration.UseTransport<LearningTransport>();

            /*endpointConfiguration.UseTransport<RabbitMQTransport>()
                    .UseConventionalRoutingTopology()
                    .ConnectionString("host=localhost");*/

            return endpointConfiguration;
        }
    }
}
