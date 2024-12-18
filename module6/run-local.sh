echo "Checking node.js installation"
if command -v node &> /dev/null; then
    echo "node.js installation found, starting infrastructure services"
    docker-compose up -d
    echo "sleeping for 10 seconds to allow infrastructure to start"
    sleep 10
    echo "starting applications"
    docker compose -f docker-compose-services.yml up -d
    sleep 1
    echo "Starting frontend application"
    cd src/frontend;npm i;npm run start
else
    echo "node.js installation not found. Please install NodeJS - https://nodejs.org/en/download/package-manager"
    exit 1
fi