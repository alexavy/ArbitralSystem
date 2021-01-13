using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Logger;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Connectors.Core.PublicConnectors;
using ArbitralSystem.PublicMarketInfoService.Domain.Commands;
using ArbitralSystem.PublicMarketInfoService.Domain.Interfaces;
using ArbitralSystem.PublicMarketInfoService.Domain.Models;
using ArbitralSystem.PublicMarketInfoService.Domain.Queries;
using ArbitralSystem.PublicMarketInfoService.Domain.Services;
using AutoMapper;
using JetBrains.Annotations;
using MediatR;

namespace ArbitralSystem.PublicMarketInfoService.Domain.Handlers
{
    public class PairsInfoCommandHandler : IRequestHandler<CreateOrDelistPairsForExchangesCommand>
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly IMediator _mediator;
        private readonly IPairInfoRepository _pairInfoRepository;
        private readonly IPublicConnectorFactory _publicConnectorFactory;
        
        public PairsInfoCommandHandler(  [NotNull] ILogger logger,
            [NotNull] IMapper mapper,
            [NotNull] IMediator mediator,
            [NotNull] IPairInfoRepository pairInfoRepository,
            [NotNull] IPublicConnectorFactory publicConnectorFactory)
        {
            Preconditions.CheckNotNull(logger ,mapper, pairInfoRepository, mediator, publicConnectorFactory);

            _logger = logger;
            _mapper = mapper;
            _mediator = mediator;
            _pairInfoRepository = pairInfoRepository;
            _publicConnectorFactory = publicConnectorFactory;
        }
        
        public async Task<Unit> Handle(CreateOrDelistPairsForExchangesCommand request, CancellationToken cancellationToken)
        {
            foreach (var exchange in request.Exchanges)
            {
                _logger.Debug($"Update process for {exchange} started");
                var connector = _publicConnectorFactory.GetInstance(exchange);
                var exchangePairs = await connector.GetPairsInfo(cancellationToken);
            
                var pairs = exchangePairs
                    .Select(o => new PairInfo(o.ExchangePairName, o.BaseCurrency, o.QuoteCurrency, o.UnificatedPairName, o.Exchange)).ToArray();
                var existedPairs = _mapper.Map<PairInfo[]>((await _mediator.Send(new PairInfoByExchangeQuery(exchange, true), cancellationToken)).ToArray());

                var newPairs = pairs.ExceptBy(existedPairs, exchange).ToArray();
                var delistedPairs = existedPairs.ExceptBy(pairs, exchange).ToList();

                delistedPairs.ForEach(o => o.SetPairAsDelisted());

                _logger.Debug($"Update process: exchange: {exchange}, new pairs-{newPairs.Length}, delisted-{delistedPairs.Count}.");
                if (newPairs.Any())
                    await _pairInfoRepository.CreateRangeAsync(newPairs, cancellationToken);

                if (delistedPairs.Any())
                    await _pairInfoRepository.UpdateRangeAsync(delistedPairs.ToArray(), cancellationToken);
                _logger.Debug($"Update process finished for exchange: {exchange}");
            }
            return Unit.Value;
        }
    }
}