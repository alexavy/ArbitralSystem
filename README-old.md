# ArbitralSystem

#Local installation on windows
1) Prepare Infrastructure
	Create Databases:
		-DistributorDb
		-MarketInfoStorageDb
		-PublicMarketInfoDb
		
2) install docker
2.1)Set kubernetes as enabled
	Check
	#kubectl get sc

3)Install Portainer
#docker volume create portainer_data
#docker run -d -p 8000:8000 -p 9000:9000 --name=portainer --restart=always -v /var/run/docker.sock:/var/run/docker.sock -v portainer_data:/data portainer/portainer-ce

4)Install Infra
#docker-compose -f ./_infrastructure.yml up

http://localhost:5050/v2/_catalog
Seq http://localhost:8080/#/events
RabbitMq http://localhost:15672/#/queues

5)Push in local Dockerhub
./PushScript-local.ps1

6)Deploy
#docker-compose -f _arbital-system_fake_0.yml up

#Prod commands======================================================================================================================================================
host.docker.internal

sudo apt-get update
sudo apt-get install ntp

#DOCKER
sudo apt install apt-transport-https ca-certificates curl software-properties-common
curl -fsSL https://download.docker.com/linux/ubuntu/gpg | sudo apt-key add -
sudo add-apt-repository "deb [arch=amd64] https://download.docker.com/linux/ubuntu bionic stable"
sudo apt update && apt-cache policy docker-ce
sudo apt install -y docker-ce
sudo apt-get install docker-compose

#CHECK DOCKER
sudo docker ps
sudo systemctl status docker

#RANCHER
sudo iptables -A INPUT -i lo -j ACCEPT
sudo iptables -A OUTPUT -o lo -j ACCEPT
sudo iptables -A INPUT -m state --state ESTABLISHED,RELATED -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 80 -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 443 -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 2376 -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 6443 -j ACCEPT
sudo iptables -A INPUT -p tcp --dport 22 -j ACCEPT
sudo iptables -A OUTPUT -p tcp --dport 22 -j ACCEPT
sudo iptables -A OUTPUT -p tcp --dport 443 -j ACCEPT
sudo iptables -A OUTPUT -p tcp --dport 2376 -j ACCEPT
sudo iptables -A OUTPUT -p tcp --dport 6443 -j ACCEPT
netstat -anltp | grep "443"

sudo docker run -d --restart=unless-stopped -p 80:80 -p 443:443 rancher/rancher
sudo docker run -d --restart=unless-stopped -p 80:80 -p 443:443 --privileged  rancher/rancher
sudo docker run -d --restart=unless-stopped -p 80:80 -p 443:443 --privileged rancher/rancher:v2.4.8 

sudo docker ps
sudo docker run -d --privileged --restart=unless-stopped --net=host -v /etc/kubernetes:/etc/kubernetes -v /var/run:/var/run rancher/rancher-agent:v2.3.3 --server https://arbitrageur-y.0xcode.in --token mwr9n9t7gwlfc5gbd6b9vrghn6k4tkvf9z2jhm4m8st4pn8cpgw4bt --ca-checksum ce683fd0080f1dfc22fae8ed9ef8f1cbd871aa8a82fd8ac2b1f83e8a45d311cd --etcd --controlplane --worker

#DOCKER REGITRY
sudo docker run -d -p 5050:5000 --restart=always --name registry registry
sudo iptables -A INPUT -p tcp --dport 5050 -j ACCEPT

#RABBITMQ
echo 'deb http://www.rabbitmq.com/debian/ testing main' | sudo tee /etc/apt/sources.list.d/rabbitmq.list
wget -O- https://www.rabbitmq.com/rabbitmq-release-signing-key.asc | sudo apt-key add -
sudo apt-get update
sudo apt-get install rabbitmq-server
sudo update-rc.d rabbitmq-server defaults
sudo service rabbitmq-server start
sudo systemctl enable rabbitmq-server
sudo rabbitmqctl add_user admin 12345678
sudo rabbitmqctl set_user_tags admin administrator
sudo rabbitmqctl set_permissions -p / admin ".*" ".*" ".*"
sudo rabbitmq-plugins enable rabbitmq_management

#CLEAN RABBITMQ

rabbitmqctl stop_app
rabbitmqctl reset  
rabbitmqctl start_app

#CLEAN DOCKER
sudo service docker stop
 cd /var/lib
 sudo rm -rf ./docker
 sudo service docker start
 
====================================================================================================================================
	
Redis install
	sudo apt-get update
	sudo apt install redis-server -y
	#run redis server
	sudo systemctl start redis-server
	#add redis to autolaunch
	sudo systemctl enable redis-server
	
	#check redis
	redis-cli ping //should return "pong"
	
	#To make redis avaliable for remote connections open /etc/redis/redis.conf
		sudo nano /etc/redis/redis.conf
	#and change "bind 127.0.0.1" to bind "0.0.0.0"
	
=====================================================================================
datalust/seq:latest
api 5341 5341
ui 80 4001
ACCEPT_EULA Y

marketinfo-aggregator
localhost:5050/marketinfo-aggregator

marketinfo-storage
localhost:5050/marketinfo-storage

public-market-info
localhost:5050/public-market-info
api 6001 6001
ASPNETCORE_URLS http://+:6001

manager-service
localhost:5050/manager-service
api 7001 7001
ASPNETCORE_URLS http://+:7001

orderbook-distributor
localhost:5050/orderbook-distributor


#SEQ
 sudo docker run --name seq -d --restart unless-stopped -e ACCEPT_EULA=Y -v /path/to/seq/data:/data -p 4001:80 -p 5341:5341 datalust/seq:latest
	

