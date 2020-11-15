using System;
using System.Threading;
using System.Threading.Tasks;
using ArbitralSystem.Common.Validation;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Commands;
using ArbitralSystem.Distributor.MQDistributor.MQDomain.Interfaces;
using MediatR;

namespace ArbitralSystem.Distributor.MQDistributor.MQDomain.Handlers
{
    public class ServerHandler :  IRequestHandler<CreateServerCommand, Guid>,
        IRequestHandler<DeleteServerCommand>
    {
        private readonly IServerRepository _serverRepository;

        public ServerHandler(IServerRepository serverRepository)
        {
            Preconditions.CheckNotNull(serverRepository);
            _serverRepository = serverRepository;
        }
        
        public async Task<Guid> Handle(CreateServerCommand request, CancellationToken cancellationToken)
        {
            var existedServer = await _serverRepository.GetAsync(request.Server.Id, cancellationToken);
            if (existedServer is null)
            {
                var server = await _serverRepository.CreateAsync(request.Server, cancellationToken);
                return server.Id;
            }
            
            await _serverRepository.UpdateAsync(request.Server, cancellationToken);
            return existedServer.Id;
        }

        public async Task<Unit> Handle(DeleteServerCommand request, CancellationToken cancellationToken)
        {
            var existedServer = await _serverRepository.GetAsync(request.ServerId, cancellationToken);
            if(existedServer is null)
                throw new InvalidOperationException("Can't set server as deleted, not exist.");
            
            await _serverRepository.UpdateAsync(existedServer.SetAsDeleted(), cancellationToken);
            return Unit.Value;
        }
    }
}