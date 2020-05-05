docker rm -f $(docker ps -a -q)
docker rmi -f $(docker images | grep '<none>')
docker rmi -f app1
docker rmi -f app2
docker rmi -f auth
docker-compose up -d
sleep 2s
open http://localhost:8437/
