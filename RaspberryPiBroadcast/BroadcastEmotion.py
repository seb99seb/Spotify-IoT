BROADCAST_TO_PORT = 7000

from socket import *
from sense_hat import SenseHat

sense = SenseHat()
s = socket(AF_INET, SOCK_DGRAM)
#s.bind(('', 14593))     # (ip, port)
# no explicit bind: will bind to default IP + random port
s.setsockopt(SOL_SOCKET, SO_BROADCAST, 1)

# Define the colors
R = (255, 0, 0)
G = (0, 255, 0)
B = (0, 0, 255)
O = (0, 0, 0)

# Define the lists of pixel values
neutral = [
  O, O, B, B, B, B, O, O,
  O, B, O, O, O, O, B, O,
  B, O, B, O, O, B, O, B,
  B, O, O, O, O, O, O, B,
  B, O, O, O, O, O, O, B,
  B, O, B, B, B, B, O, B,
  O, B, O, O, O, O, B, O,
  O, O, B, B, B, B, O, O,
  ]

happy = [
  O, O, G, G, G, G, O, O,
  O, G, O, O, O, O, G, O,
  G, O, G, O, O, G, O, G,
  G, O, O, O, O, O, O, G,
  G, O, G, O, O, G, O, G,
  G, O, G, G, G, G, O, G,
  O, G, O, O, O, O, G, O,
  O, O, G, G, G, G, O, O,
  ]

sad = [
  O, O, R, R, R, R, O, O,
  O, R, O, O, O, O, R, O,
  R, O, R, O, O, R, O, R,
  R, O, O, O, O, O, O, R,
  R, O, R, R, R, R, O, R,
  R, O, R, O, O, R, O, R,
  O, R, O, O, O, O, R, O,
  O, O, R, R, R, R, O, O,
  ]

# Define the functions
def setneutral():
  sense.set_pixels(neutral)
  
def sethappy():
  sense.set_pixels(happy)
  
def setsad():
  sense.set_pixels(sad)

# This keeps the program running to receive joystick events
while True:
	for event in sense.stick.get_events():
		if event.direction == "up":
			setneutral()
		elif event.direction == "down":
			sense.clear()
		elif event.direction == "left":
			sethappy()
		elif event.direction == "right":
			setsad()
		elif event.direction == "middle":
			sense.clear()
		data = str(event.direction) + " " + str(event.action)
		s.sendto(bytes(data, "UTF-8"), ('<broadcast>', BROADCAST_TO_PORT))
		print(data)
