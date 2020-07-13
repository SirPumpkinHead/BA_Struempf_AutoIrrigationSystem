#!/bin/bash

set -e

printf "⏳ Creating context data "

#
# Create one Flower Bed Entity
#
curl -s -o /dev/null -X POST \
  'http://localhost:1026/v2/op/update' \
  -H 'Content-Type: application/json' \
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

#
# Add Weather Responses for Flower Bed 1 using random values
#
#curl -s -o /dev/null -X POST \
#  http://orion:1026/v2/registrations \
#  -H 'Content-Type: application/json' \
#  -d '{
#   "description": "Weather Conditions",
#   "dataProvided": {
#     "entities": [
#       {
#         "id" : "urn:ngsi-ld:Bed:001",
#         "type": "FlowerBed"
#       }
#     ],
#     "attrs": [
#      "temperature", "relativeHumidity"
#    ]
#   },
#   "provider": {
#     "http": {
#       "url": "http://context-provider:3000/proxy/v1/random/weatherConditions"
#     },
#     "legacyForwarding": true
#   },
#   "status": "active"
#}'


# Add Weather and Twitter Responses for Store 2 using real sources
#
# curl -s -o /dev/null -X POST \
#   'http://orion:1026/v2/registrations' \
#   -H 'Content-Type: application/json' \
#   -d '{
#   "description": "Store:002 - Real Temperature and Humidity",
#   "dataProvided": {
#     "entities": [
#       {
#         "id": "urn:ngsi-ld:Store:002",
#         "type": "Store"
#       }
#     ],
#     "attrs": [
#       "temperature",
#       "relativeHumidity"
#     ]
#   },
#   "provider": {
#     "http": {
#       "url": "http://context-provider:3000/proxy/weather/number/temperature:temp,relativeHumidity:humidity/berlin%2cde"
#     },
#      "legacyForwarding": true
#   }
# }'

echo -e " \033[1;32mdone\033[0m"
