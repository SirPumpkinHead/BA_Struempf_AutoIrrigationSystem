import time

import RPi.GPIO as GPIO


class Relais:
    _channel = 0

    def __init__(self, gpio_channel: int):
        self._channel = gpio_channel
        print("Setting mode for channel " + str(self._channel))
        GPIO.setmode(GPIO.BCM)
        GPIO.setup(self._channel, GPIO.OUT)
        time.sleep(0.1)
        self.deactivate()

    def __del__(self):
        print("Cleaning up relais")
        GPIO.cleanup()

    def deactivate(self):
        time.sleep(0.1)
        print("Deactivating relais " + str(self._channel))
        GPIO.output(self._channel, GPIO.HIGH)

    def activate(self):
        time.sleep(0.1)
        print("Activating relais " + str(self._channel))
        GPIO.output(self._channel, GPIO.LOW)
