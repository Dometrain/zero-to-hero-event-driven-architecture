cd src/PlantBasedPizza.Account && docker build -f application/PlantBasedPizza.Account.Api/Dockerfile -t account-api ../
cd ../../
cd src/PlantBasedPizza.Delivery && docker build -f application/PlantBasedPizza.Delivery.Api/Dockerfile -t delivery-api ../ && docker build -f application/PlantBasedPizza.Delivery.Worker/Dockerfile -t delivery-worker ../
cd ../../
cd src/PlantBasedPizza.Kitchen && docker build -f application/PlantBasedPizza.Kitchen.Api/Dockerfile -t kitchen-api ../ && docker build -f application/PlantBasedPizza.Kitchen.Worker/Dockerfile -t kitchen-worker ../
cd ../../
cd src/PlantBasedPizza.LoyaltyPoints && docker build -f application/PlantBasedPizza.LoyaltyPoints.Api/Dockerfile -t loyalty-api ../ && docker build -f application/PlantBasedPizza.LoyaltyPoints.Worker/Dockerfile -t loyalty-worker ../ && docker build -f application/PlantBasedPizza.LoyaltyPoints.Internal/Dockerfile -t loyalty-internal-api ../
cd ../../
cd src/PlantBasedPizza.Orders && docker build -f application/PlantBasedPizza.Orders.Worker/Dockerfile -t order-worker ../ && docker build -f application/PlantBasedPizza.Orders.Api/Dockerfile -t order-api ../ && docker build -f application/PlantBasedPizza.Orders.Internal/Dockerfile -t order-internal ../
cd ../../
cd src/PlantBasedPizza.Payments && docker build -f application/PlantBasedPizza.Payments/Dockerfile -t payment-api ../
cd ../../
cd src/PlantBasedPizza.Recipes && docker build -f applications/PlantBasedPizza.Recipes.Api/Dockerfile -t recipe-api ../
cd ../../
cd src/frontend && docker build -t frontend .