from socket import *
from sense_hat import SenseHat

# Port to broadcast messages
BROADCAST_TO_PORT = 7000

sense = SenseHat()

# Create client UDP socket
s = socket(AF_INET, SOCK_DGRAM)
s.setsockopt(SOL_SOCKET, SO_BROADCAST, 1)

# Define the colors
R = (255, 0, 0)
G = (0, 255, 0)
B = (0, 0, 255)
O = (0, 0, 0)

# Define the lists of pixel values for emotions
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

# Define the functions for setting emotions
def setneutral():
  sense.set_pixels(neutral)
  
def sethappy():
  sense.set_pixels(happy)
  
def setsad():
  sense.set_pixels(sad)

# This keeps the program running to receive joystick events
while True:
  # Executes everytime the stick is pressed in a direction
	for event in sense.stick.get_events():
    # Check for the direction and set emotion
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
    # If the stick is pressed, broadcast direction as type string to port and print the direction
		if event.action == "pressed":
			data = "" + str(event.direction)
			s.sendto(bytes(data, "UTF-8"), ('<broadcast>', BROADCAST_TO_PORT))
			print(data)
