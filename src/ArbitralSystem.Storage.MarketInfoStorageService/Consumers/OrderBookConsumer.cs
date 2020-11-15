using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Messaging.Messages;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Commands;
using ArbitralSystem.Storage.MarketInfoStorageService.Domain.Models;
using AutoMapper;
using JetBrains.Annotations;
using MassTransit;
using MediatR;

namespace ArbitralSystem.Storage.MarketInfoStorageService.Consumers
{
    [UsedImplicitly]
    internal class OrderBookConsumer : IConsumer<IOrderBookPackageMessage>
    {
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly ILogger _logger;

        private TimeSpan TimeOut => new TimeSpan(0, 5, 0);
        
        public OrderBookConsumer(IMediator mediator,IMapper mapper, ILogger logger)
        {
            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<IOrderBookPackageMessage> context)
        {
            
            _logger.Information($"Order book package message received, count: {context.Message.OrderBooks.Count()}, retry attempt: {context.GetRetryAttempt()} ");
            try
            {
                var orderBooks = _mapper.Map<IEnumerable<OrderBook>>(context.Message.OrderBooks);
                var cts = new CancellationTokenSource(TimeOut);
                await _mediator.Send(new BulkSaveOrderBooksCommand(orderBooks), cts.Token);
            }
            catch (Exception e)
            {
                _logger.Error(e,"Error while saving distributor order books");
                throw;
            }

        }
    }
}