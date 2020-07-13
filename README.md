# AutoIrrigationSystem

This repo contains scripts for deploying an automatic irrigation system developed as par of a bachelor thesis.

## How to use (Instructions)

Before starting the service a .env file is required inorder to configure the system.
A template is provided in `.env-tmpl`.

For locally running the system on your workstation without deploying sensors run: 
`./scripts.sh start-local`
This will add a [Mosquitto](https://mosquitto.org) message broker and add a service faking sensor data to the system.

For shutting down the system run:
`./scripts.sh stop-local`

The startup scripts requires the following programs: `docker`, `docker-compose`, `curl`

## Water Controller
WaterController was part of this repo, but moved to https://github.com/SirPumpkinHead/BA_Struempf_WaterController

## Weather Controller
WeatherController was part of this repo, but moved to https://github.com/SirPumpkinHead/BA_Struempf_WeatherProvider
