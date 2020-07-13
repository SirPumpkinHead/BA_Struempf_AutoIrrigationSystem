import time

import RPi.GPIO as GPIO


class BinarySensor:
    _vcc_channel: int = None
    _read_channel: int = None

    def __init__(self, vcc: int, read: int):
        self._vcc_channel = vcc
        self._read_channel = read
        GPIO.setmode(GPIO.BCM)

        # preparing power for sensor
        GPIO.setup(self._vcc_channel, GPIO.OUT)

        # preparing reading from sensor
        GPIO.setup(self._read_channel, GPIO.IN)

    def __del__(self):
        print("Cleaning up binary sensor")
        GPIO.cleanup()

    def activate(self):
        print("Activating sensor (GPIO " + str(self._vcc_channel) + ")")
        GPIO.output(self._vcc_channel, GPIO.HIGH)
        time.sleep(0.1)

    def deactivate(self):
        print("Deactivating sensor")
        GPIO.output(self._vcc_channel, GPIO.LOW)
        time.sleep(0.1)

    def read(self):
        print("Reading from sensor (GPIO " + str(self._read_channel) + ")")
        return GPIO.input(self._read_channel)
