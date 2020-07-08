import signal
import time

try:
    from binarysensor import BinarySensor
except ImportError:
    from .binarysensor import BinarySensor


class Program:
    _running = True
    _sensor1 = None
    _sensor2 = None

    def __init__(self):
        self._sensor1 = BinarySensor(20, 16)
        self._sensor2 = BinarySensor(26, 19)

    def __del__(self):
        del self._sensor1

    def run(self):
        time.sleep(1)
        print("Running...")
        while self._running:
            self._sensor1.activate()
            self._sensor2.activate()
            time.sleep(1)

            value = self._sensor1.read()
            print("Read value " + str(value) + " on sensor 1")
            value = self._sensor2.read()
            print("Read value " + str(value) + " on sensor 2")

            time.sleep(1)

            self._sensor1.deactivate()
            self._sensor2.deactivate()
            time.sleep(4)

    def stop(self):
        self._running = False


# main
program = Program()


def sigterm_handler(_signo, _stack_frame):
    print("\nStopping program")
    program.stop()


signal.signal(signal.SIGTERM, sigterm_handler)
signal.signal(signal.SIGINT, sigterm_handler)

program.run()

del program

print("[end of program]")
