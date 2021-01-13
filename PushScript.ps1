docker build -t marketinfo-aggregator -f .\DockerFile-MarketInfoAggregator .
docker tag marketinfo-aggregator {DOCKER_HUB}/marketinfo-aggregator
docker push {DOCKER_HUB}/marketinfo-aggregator

docker build -t marketinfo-storage -f .\DockerFile-MarketInfoStorage .
docker tag marketinfo-storage {DOCKER_HUB}/marketinfo-storage
docker push {DOCKER_HUB}/marketinfo-storage

docker build -t public-market-info -f .\DockerFile-PublicMarketInfoService .
docker tag public-market-info {DOCKER_HUB}/public-market-info
docker push {DOCKER_HUB}/public-market-info

docker build -t mq-manager-service -f .\DockerFile-MqManagerService .
docker tag mq-manager-service {DOCKER_HUB}/mq-manager-service
docker push {DOCKER_HUB}/mq-manager-service

docker build -t mq-orderbook-distributor -f .\DockerFile-MqOrderBookDistributorService .
docker tag mq-orderbook-distributor {DOCKER_HUB}/mq-orderbook-distributor
docker push {DOCKER_HUB}/mq-orderbook-distributor


