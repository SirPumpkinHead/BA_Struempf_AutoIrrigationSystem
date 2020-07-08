import signal
import time

import paho.mqtt.client as mqtt

try:
    from binarysensor import BinarySensor
except ImportError:
    from .binarysensor import BinarySensor

MQTT_HOST: str = "10.0.0.22"
MQTT_PORT: int = 1883
API_KEY: str = "tOiOdxFTpZwezrrpCT"
READ_EVERY_MINUTES: int = 15


def get_topic(device_id):
    return "/" + API_KEY + "/" + device_id + "/attrs"


def publish_value(_mqtt_client, device_id, value):
    print("Publishing value '" + str(value) + "' for device " + device_id)
    _mqtt_client.publish(get_topic(device_id), "{\"m\":" + str(value) + "}")


class Program:
    _running = True
    _sensor1 = None
    _sensor2 = None
    _mqtt_client = None

    def __init__(self):
        self._sensor1 = BinarySensor(20, 16)
        self._sensor2 = BinarySensor(26, 19)
        self._mqtt_client = mqtt.Client()

    def __del__(self):
        del self._sensor1
        del self._sensor2

    def run(self):
        print("Running...")
        while True:
            print("Connecting to MQTT at " + MQTT_HOST + ":" + str(MQTT_PORT))
            self._mqtt_client.connect(MQTT_HOST, MQTT_PORT, 60)

            print("Activating sensors")
            self._sensor1.activate()
            self._sensor2.activate()
            time.sleep(1)

            value = self._sensor1.read()
            publish_value(self._mqtt_client, "soil-moisture01", value)

            time.sleep(0.5)

            value = self._sensor2.read()
            publish_value(self._mqtt_client, "soil-moisture02", value)

            time.sleep(0.5)

            print("Deactivating sensors")
            self._sensor1.deactivate()
            self._sensor2.deactivate()

            print("Disconnecting from MQTT")
            self._mqtt_client.disconnect()

            seconds_asleep = 0
            while seconds_asleep < READ_EVERY_MINUTES * 60:
                seconds_asleep += 1
                time.sleep(1)
                if not self._running:
                    print("Sleep interrupted")
                    return

    def stop(self):
        self._running = False


# main
program = Program()


def sigterm_handler(_signo, _stack_frame):
    print("\nStopping program")
    program.stop()


print("Registering signal handler")
signal.signal(signal.SIGTERM, sigterm_handler)
signal.signal(signal.SIGINT, sigterm_handler)

program.run()

del program

print("[end of program]")
