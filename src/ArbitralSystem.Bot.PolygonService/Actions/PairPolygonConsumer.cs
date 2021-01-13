using ArbitralSystem.Bot.PairPolygonBotService.Workflow;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Messaging.Messages;
using MassTransit;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ArbitralSystem.Bot.PairPolygonBotService.Actions
{
    internal class PairPolygonConsumer //: IConsumer<IPairPolygonMessage>
    {
        private readonly IPairPolygonBotWorkflow _polygonBotWorkflow;
        private readonly ILogger _logger;

        public PairPolygonConsumer(IPairPolygonBotWorkflow polygonBotWorkflow,
                                   ILogger logger)
        {
            _polygonBotWorkflow = polygonBotWorkflow;
            _logger = logger;
        }

       /* public Task Consume(ConsumeContext<IPairPolygonMessage> context)
        {
            throw new NotImplementedException();
        }*/
    }
}
