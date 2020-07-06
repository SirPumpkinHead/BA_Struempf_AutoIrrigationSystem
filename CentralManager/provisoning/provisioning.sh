#!/usr/bin/env bash
# Variables
IOT_AGENT_HOST="http://localhost:4041"
CONTEXT_BROKER_HOST="http://orion:1026"
CONTEXT_BROKER_PROVISION_HOST="http://localhost:1026"
API_KEY="tOiOdxFTpZwezrrpCT"
SERVICE_HEADER="fiware-service: garden"
SERVICE_PATH_HEADER="fiware-servicepath: /flowerbed"

provisionContext () {
    printf "⏳ Provisioning context broker\n"

    curl -s -X POST \
      'http://localhost:1026/v2/op/update' \
      -H 'Content-Type: application/json' \
      -H "${SERVICE_HEADER}" \
      -H "${SERVICE_PATH_HEADER}" \
      -g -d '{
      "actionType": "append",
      "entities": [
        {
            "id":"urn:ngsi-ld:Bed:001","type":"FlowerBed",
            "location":{"type":"geo:json","value":{"type":"Point","coordinates":[48.2082,16.3738]}},
            "name":{"type":"Text","value":"Rose Bed"}
        }
      ]
    }'

    echo -e " \033[1;32mdone\033[0m"
}
# Setting up IoT Agent (middleware)
provisionAgent () {
    printf "⏳ Provisioning IoT agent\n"

    # Setup of service group
    curl -X POST \
        "${IOT_AGENT_HOST}/iot/services" \
        -H 'Content-Type: application/json' \
        -H "${SERVICE_HEADER}" \
        -H "${SERVICE_PATH_HEADER}" \
        -d "{
         \"services\": [
           {
             \"apikey\":      \"${API_KEY}\",
             \"entity_type\": \"Thing\",
             \"resource\":    \"\"
           }
         ]
        }"

    # Setup of moisture sensors
    curl -X POST \
          "${IOT_AGENT_HOST}/iot/devices" \
          -H 'Content-Type: application/json' \
          -H "${SERVICE_HEADER}" \
          -H "${SERVICE_PATH_HEADER}" \
          -d '{
                "devices": [
                   {
                     "device_id":   "soil-moisture01",
                     "entity_name": "urn:ngsi-ld:SoilMoisture:001",
                     "entity_type": "Sensor",
                     "protocol":    "PDI-IoTA-UltraLight",
                     "transport":   "MQTT",
                     "timezone":    "Europe/Vienna",
                     "attributes": [
                       { "object_id": "m", "name": "moisture", "type": "Integer" }
                     ],
                     "static_attributes": [
                       { "name":"refBed", "type": "Relationship", "value": "urn:ngsi-ld:Bed:001"}
                     ]
                   },
                   {
                     "device_id":   "soil-moisture02",
                     "entity_name": "urn:ngsi-ld:SoilMoisture:002",
                     "entity_type": "Sensor",
                     "protocol":    "PDI-IoTA-UltraLight",
                     "transport":   "MQTT",
                     "timezone":    "Europe/Vienna",
                     "attributes": [
                       { "object_id": "m", "name": "moisture", "type": "Integer" }
                     ],
                     "static_attributes": [
                       { "name":"refBed", "type": "Relationship", "value": "urn:ngsi-ld:Bed:001"}
                     ]
                   },
                   {
                     "device_id":   "soil-moisture03",
                     "entity_name": "urn:ngsi-ld:SoilMoisture:003",
                     "entity_type": "Sensor",
                     "protocol":    "PDI-IoTA-UltraLight",
                     "transport":   "MQTT",
                     "timezone":    "Europe/Vienna",
                     "attributes": [
                       { "object_id": "m", "name": "moisture", "type": "Integer" }
                     ],
                     "static_attributes": [
                       { "name":"refBed", "type": "Relationship", "value": "urn:ngsi-ld:Bed:001"}
                     ]
                   }
                 ]
              }
        '

    # Setup of actuator (valve)
    curl -iX POST \
          "${IOT_AGENT_HOST}/iot/devices" \
          -H 'Content-Type: application/json' \
          -H "${SERVICE_HEADER}" \
          -H "${SERVICE_PATH_HEADER}" \
          -d '{
          "devices": [
            {
              "device_id": "valve01",
              "entity_name": "urn:ngsi-ld:Valve:001",
              "entity_type": "Valve",
              "transport": "MQTT",
              "commands": [
                { "name": "open", "type": "command" },
                { "name": "close", "type": "command" }
               ],
               "static_attributes": [
                 {"name":"refBed", "type": "Relationship","value": "urn:ngsi-ld:Bed:001"}
              ]
            }
          ]
        }
        '

    echo -e " \033[1;32mdone\033[0m"
}

# main
set -e

echo "Provisioning services..."

provisionContext
provisionAgent

printf "Provisioning \033[1;32mdone\033[0m\n"