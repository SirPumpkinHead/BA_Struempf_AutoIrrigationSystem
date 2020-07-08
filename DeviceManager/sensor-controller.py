import signal
import time

try:
    from binarysensor import BinarySensor
except ImportError:
    from .binarysensor import BinarySensor


class Program:
    _running = True
    _sensor = None

    def __init__(self):
        self._sensor = BinarySensor(26, 19)

    def __del__(self):
        del self._sensor

    def run(self):
        time.sleep(1)
        print("Running...")
        while self._running:
            self._sensor.activate()
            time.sleep(1)

            value = self._sensor.read()
            print("Read value " + str(value))
            time.sleep(1)

            self._sensor.deactivate()
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
