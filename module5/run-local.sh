echo "Starting infrastructure"
docker-compose up -d
echo "sleeping for 10 seconds to allow infrastructure to start"
sleep 10
echo "starting applications"
docker compose -f docker-compose-services.yml up -d