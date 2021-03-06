#!/bin/bash
#
#  Command Line Interface to start all services
#  Based on https://fiware-tutorials.readthedocs.io/en/latest/iot-over-mqtt/index.html
#

set -e

if (( $# != 1 )); then
    echo "Illegal number of parameters"
    echo "usage: services [create|start|stop]"
    exit 1
fi

stoppingContainers () {
	echo "Stopping containers"
	docker-compose --log-level ERROR -p fiware down -v --remove-orphans
}

stoppingContainersLocal () {
	echo "Stopping containers (local)"
	docker-compose -f docker-compose-local.yml --log-level ERROR -p fiware down -v --remove-orphans
}

addDatabaseIndex () {
	printf "Adding appropriate \033[1mMongoDB\033[0m indexes for \033[1;34mOrion\033[0m  ..."
	docker exec  fiware-db-mongo mongo --eval '
	conn = new Mongo();db.createCollection("orion");
	db = conn.getDB("orion");
	db.createCollection("entities");
	db.entities.createIndex({"_id.servicePath": 1, "_id.id": 1, "_id.type": 1}, {unique: true});
	db.entities.createIndex({"_id.type": 1}); 
	db.entities.createIndex({"_id.id": 1});' > /dev/null

	docker exec  fiware-db-mongo mongo --eval '
	conn = new Mongo();db.createCollection("orion-openiot");
	db = conn.getDB("orion-openiot");
	db.createCollection("entities");
	db.entities.createIndex({"_id.servicePath": 1, "_id.id": 1, "_id.type": 1}, {unique: true});
	db.entities.createIndex({"_id.type": 1}); 
	db.entities.createIndex({"_id.id": 1});' > /dev/null
	echo -e " \033[1;32mdone\033[0m"

	printf "Adding appropriate \033[1mMongoDB\033[0m indexes for \033[1;36mIoT-Agent\033[0m  ..."
	docker exec  fiware-db-mongo mongo --eval '
	conn = new Mongo();
	db = conn.getDB("iotagentjson");
	db.createCollection("devices");
	db.devices.createIndex({"_id.service": 1, "_id.id": 1, "_id.type": 1});
	db.devices.createIndex({"_id.type": 1}); 
	db.devices.createIndex({"_id.id": 1});
	db.createCollection("groups");
	db.groups.createIndex({"_id.resource": 1, "_id.apikey": 1, "_id.service": 1});
	db.groups.createIndex({"_id.type": 1});' > /dev/null
	echo -e " \033[1;32mdone\033[0m"
}

waitForOrion () {
	echo -e "\n⏳ Waiting for \033[1;34mOrion\033[0m to be available\n"
	while [ `docker run --network fiware_default --rm curlimages/curl -s -o /dev/null -w %{http_code} 'http://orion:1026/version'` -eq 000 ]
	do 
	  echo -e "Context Broker HTTP state: " `curl -s -o /dev/null -w %http_code 'http://localhost:1026/version'` " (waiting for 200)"
	  sleep 1
	done
}


waitForIoTAgent () {
	echo -e "\n⏳ Waiting for \033[1;36mIoT-Agent\033[0m to be available\n"
	while [ `docker run --network fiware_default --rm curlimages/curl -s -o /dev/null -w %{http_code} 'http://iot-agent:4041/version'` -eq 000 ]
	do 
	  echo -e "IoT Agent HTTP state: " `curl -s -o /dev/null -w %http_code 'http://localhost:4041/version'` " (waiting for 200)"
	  sleep 1
	done
}

displayServices () {
	echo ""
	docker ps --format "table {{.Names}}\t{{.Status}}\t{{.Ports}}" --filter name=fiware-*
	echo ""
}

command="$1"
case "${command}" in
	"help")
        echo "usage: services [create|start|stop]"
        ;;
    "start")
		stoppingContainers
		echo -e "Starting containers \033[1mWaterControllerDB\033[0m,  \033[1mWeatherController\033[0m,  \033[1;34mOrion\033[0m, \033[1;36mIoT-Agent\033[0m, \033[1mMongoDB\033[0m database \033[1mMySQL\033[0m database"
		echo ""
		docker-compose --log-level ERROR -p fiware up --build -d --remove-orphans
		addDatabaseIndex
		waitForOrion
		waitForIoTAgent
		displayServices
		echo -e "All services running"
		./provisoning-scripts/provisioning.sh
		;;
    "start-local")
		stoppingContainersLocal
		echo -e "Starting containers \033[1mWaterControllerDB\033[0m, \033[1mMosquitto\033[0m, \033[1mFakeDevice\033[0m, \033[1mWeatherController\033[0m,  \033[1;34mOrion\033[0m, \033[1;36mIoT-Agent\033[0m, \033[1mMongoDB\033[0m database \033[1mMySQL\033[0m database"
		echo ""
		docker-compose -f docker-compose-local.yml --log-level ERROR -p fiware up --build -d --remove-orphans
		addDatabaseIndex
		waitForOrion
		waitForIoTAgent
		displayServices
		echo -e "All services running"
		echo -e "Provisioning services"
		./provisoning-scripts/provisioning.sh
		echo -e "System ready to use locally"
		;;
	"stop")
		stoppingContainers
		;;
	"stop-local")
		stoppingContainers
		;;
	"create")
		echo "Pulling Docker images"
		docker-compose --log-level ERROR -p fiware pull
		;;
	*)
		echo "Command not Found."
		echo "usage: services [create|start|stop|start-local|stop-local]"
		exit 127;
		;;
esac
