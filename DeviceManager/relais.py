import RPi.GPIO as GPIO


class Relais:
    _channel = 0

    def __init__(self, gpio_channel: int):
        self._channel = gpio_channel
        print("Setting mode for channel " + str(self._channel))
        GPIO.setmode(GPIO.BCM)
        GPIO.setup(self._channel, GPIO.OUT)

    def __del__(self):
        print("Cleaning up relais")
        GPIO.cleanup()

    def activate(self):
        print("Activating relais " + str(self._channel))
        GPIO.output(self._channel, GPIO.HIGH)

    def deactivate(self):
        print("Deactivating relais " + str(self._channel))
        GPIO.output(self._channel, GPIO.LOW)
