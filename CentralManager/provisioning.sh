#!/usr/bin/env bash
# Variables
IOT_AGENT_HOST="http://localhost:4041"
CONTEXT_BROKER_HOST="http://orion:1026"
CONTEXT_BROKER_PROVISION_HOST="http://localhost:1026"
API_KEY="tOiOdxFTpZwezrrpCT"

provisionBed () {
   echo "\nCreating flower bed entity in context broker (orion)\n"
   curl -iX POST \
      --url "${CONTEXT_BROKER_PROVISION_HOST}/v2/entities" \
      --header 'Content-Type: application/json' \
      --data '{
          "id":"urn:ngsi-ld:Bed:001",
          "type":"Bed",
          "name":{"type":"Text", "value":"Flower Bed"}
        }'
}

# Setup service group
provisionServiceGroup () {
    echo "\nCreating service group in IoT Agent\n"
    curl -iXPOST \
        "${IOT_AGENT_HOST}/iot/services" \
        -H 'Content-Type: application/json' \
        -H 'fiware-service: openiot' \
        -H 'fiware-servicepath: /' \
        -d "{
         \"services\": [
           {
             \"apikey\":      \"${API_KEY}\",
             \"cbroker\":     \"${CONTEXT_BROKER_HOST}\",
             \"entity_type\": \"Thing\",
             \"resource\":    \"\"
           }
         ]
        }"
}

# Setup a moisture sensors
provisionSensor() {
    echo "\nCreating sensors in IoT Agent\n"
    curl -iX POST \
          "${IOT_AGENT_HOST}/iot/devices" \
          -H 'Content-Type: application/json' \
          -H 'fiware-service: openiot' \
          -H 'fiware-servicepath: /' \
          -d '{
                "devices": [
                   {
                     "device_id":   "soil-moisture1",
                     "entity_name": "urn:ngsi-ld:SoilMoisture:001",
                     "entity_type": "Moisture",
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
}

# main

provisionBed
provisionServiceGroup
provisionSensor