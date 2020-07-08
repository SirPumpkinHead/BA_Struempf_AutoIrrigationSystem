import signal
import time

try:
    from relais import Relais
except ImportError:
    from .relais import Relais


class Program:
    _running = True
    _relais = None

    def __init__(self):
        self._relais = Relais(14)

    def __del__(self):
        del self._relais

    def run(self):
        time.sleep(1)
        print("Running...")
        while self._running:
            self._relais.activate()
            time.sleep(2.5)
            self._relais.deactivate()
            time.sleep(2.5)

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
