#!/usr/bin/env bash
# Variables
IOT_AGENT_HOST="http://localhost:4041"
CONTEXT_BROKER_HOST="http://localhost:1026"
CONTEXT_BROKER_PROVISION_HOST="http://localhost:1026"
API_KEY="tOiOdxFTpZwezrrpCT"
SERVICE_HEADER="fiware-service: garden"
SERVICE_PATH_HEADER="fiware-servicepath: /flowerbed"

provisionContext () {
    echo ""
    echo "⏳ Provisioning context broker\n"

    # Creating flower bed entity
    curl  -s -X POST \
      "${CONTEXT_BROKER_HOST}/v2/op/update" \
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

    # Registering weather provider
    curl  -iX POST \
      "${CONTEXT_BROKER_HOST}/v2/registrations" \
      -H 'Content-Type: application/json' \
      -H "${SERVICE_HEADER}" \
      -H "${SERVICE_PATH_HEADER}" \
      -d '{
      "description": "Weather Conditions",
      "dataProvided": {
        "entities": [
          {
            "id": "urn:ngsi-ld:Bed:001",
            "type": "FlowerBed"
          }
        ],
        "attrs": [
          "expRainVolume1h",
          "expRainVolume4h"
        ]
      },
      "provider": {
        "http": {
          "url": "http://weather-provider/weatherConditions/expected/rain"
        }
      }
    }'

    echo ""
    printf "Context broker \033[1;32mdone\033[0m"
    echo ""
}
# Setting up IoT Agent (middleware)
provisionAgent () {
    echo ""
    echo "⏳ Provisioning IoT agent\n"

    # Setup of service group
    curl  -X POST \
        "${IOT_AGENT_HOST}/iot/services" \
        -H 'Content-Type: application/json' \
        -H "${SERVICE_HEADER}" \
        -H "${SERVICE_PATH_HEADER}" \
        -d "{
         \"services\": [
           {
             \"apikey\":      \"${API_KEY}\",
             \"entity_type\": \"Thing\",
             \"resource\":    \"/iot/json\"
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
                     "transport":   "MQTT",
                     "timezone":    "Europe/Vienna",
                     "attributes": [
                       { "object_id": "s", "name": "sufficientMoisture", "type": "String" }
                     ],
                     "static_attributes": [
                       { "name":"refBed", "type": "Relationship", "value": "urn:ngsi-ld:Bed:001"}
                     ]
                   },
                   {
                     "device_id":   "soil-moisture02",
                     "entity_name": "urn:ngsi-ld:SoilMoisture:002",
                     "entity_type": "Sensor",
                     "transport":   "MQTT",
                     "timezone":    "Europe/Vienna",
                     "attributes": [
                       { "object_id": "s", "name": "sufficientMoisture", "type": "String" }
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
          "protocol": "PDI-IoTA-UltraLight",
          "transport": "MQTT",
          "commands": [
            { "name": "open", "type": "command" },
            { "name": "close", "type": "command" }
           ],
           "static_attributes": [
             {"name":"refStore", "type": "Relationship","value": "urn:ngsi-ld:Bed:001"}
          ]
        }
      ]
    }
    '
    echo ""
    printf "IoT Agent \033[1;32mdone\033[0m"
    echo ""
}

# main
set -e

provisionContext
provisionAgent

echo ""
printf "Provisioning \033[1;32mdone\033[0m\n"