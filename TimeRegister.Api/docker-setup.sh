#!/bin/bash

app="timeregister"

echo Publishing .net app
cd src/

if docker ps | awk -v app="$app" 'NR > 1 && $NF == app{ret=1; exit} END{exit !ret}'; then
  docker rm $app -f
fi

if docker container ls -a | awk -v app="$app" 'NR > 1 && $NF == app{ret=1; exit} END{exit !ret}'; then
  docker container rm $app -f
fi

docker build -t $app .

docker run -dP \
-p 5000:5000 \
--name $app $app

cd ..