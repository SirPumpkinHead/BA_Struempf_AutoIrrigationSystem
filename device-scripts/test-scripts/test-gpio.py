import RPi.GPIO as GPIO

import time

channel=3

print("Setting mode")
GPIO.setmode(GPIO.BCM)
GPIO.setup(channel, GPIO.OUT)
time.sleep(1)

print("Setting GPIO 0 low")
GPIO.output(channel, GPIO.LOW)
time.sleep(5)

print("Setting GPIO 0 high")
GPIO.output(channel, GPIO.HIGH)
time.sleep(5)

print("Setting GPIO 0 low")
GPIO.output(channel, GPIO.LOW)
time.sleep(5)

print("Cleanup")
GPIO.cleanup()

print("[end of program]")