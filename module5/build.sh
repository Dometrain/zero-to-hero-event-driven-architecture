cd src/PlantBasedPizza.Account && docker build -f application/PlantBasedPizza.Account.Api/Dockerfile-x86 -t account-api ../
cd ../../
cd src/PlantBasedPizza.Delivery && docker build -f application/PlantBasedPizza.Delivery.Api/Dockerfile-x86 -t delivery-api ../ && docker build -f application/PlantBasedPizza.Delivery.Worker/Dockerfile-x86 -t delivery-worker ../
cd ../../
cd src/PlantBasedPizza.Kitchen && docker build -f application/PlantBasedPizza.Kitchen.Api/Dockerfile-x86 -t kitchen-api ../ && docker build -f application/PlantBasedPizza.Kitchen.Worker/Dockerfile-x86 -t kitchen-worker ../
cd ../../
cd src/PlantBasedPizza.LoyaltyPoints && docker build -f application/PlantBasedPizza.LoyaltyPoints.Api/Dockerfile-x86 -t loyalty-api ../ && docker build -f application/PlantBasedPizza.LoyaltyPoints.Worker/Dockerfile-x86 -t loyalty-worker ../ && docker build -f application/PlantBasedPizza.LoyaltyPoints.Internal/Dockerfile-x86 -t loyalty-internal-api ../
cd ../../
cd src/PlantBasedPizza.Orders && docker build -f application/PlantBasedPizza.Orders.Worker/Dockerfile-x86 -t order-worker ../ && docker build -f application/PlantBasedPizza.Orders.Api/Dockerfile-x86 -t order-api ../ && docker build -f application/PlantBasedPizza.Orders.Internal/Dockerfile-x86 -t order-internal ../
cd ../../
cd src/PlantBasedPizza.Payments && docker build -f application/PlantBasedPizza.Payments/Dockerfile-x86 -t payment-api ../
cd ../../
cd src/PlantBasedPizza.Recipes && docker build -f applications/PlantBasedPizza.Recipes.Api/Dockerfile-x86 -t recipe-api ../\
cd ../../
cd src/frontend && docker build -t frontend .