from socket import *
import requests
import urllib3

# Disable warnings
http = urllib3.PoolManager()
urllib3.disable_warnings()

# URI to the REST Service
URI = "https://sensifyrest2022.azurewebsites.net//api/Moods"

# Port to broadcast messages
serverPort = 7000

# Create client UDP socket
serverSocket = socket(AF_INET, SOCK_DGRAM)
serverSocket.bind(('', serverPort))

# While the program is running, execute code to receive broadcast messages from RaspberryPi and make Put requests to REST Service
print("The server is ready to receive")
while True:
    # Receive message from RaspberryPi and print it to terminal
    message, clientAddress = serverSocket.recvfrom(2048)
    print("Message Received: " + message.decode())
    # Make a URL with the message as parameter for a Put request
    url = URI + "?direction=" + message.decode()
    # Make a Put request to the REST Service using the URL
    response = requests.put(url=url, verify=False)
    print("Status Code: " + str(response.status_code))
