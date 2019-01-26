#!/bin/bash
docker network create superpagos_network
docker-compose -f docker-compose.test.yml up -d postgres_ci
docker-compose -f docker-compose.test.yml build api_ci 
docker-compose -f docker-compose.test.yml run api_ci 
if [[ $? -ne 0 ]] ; then echo "error al correr tests" ; exit 1 ; fi
docker-compose -f docker-compose.test.yml down -v
docker image prune -f
docker volume rm superpagos_data_test