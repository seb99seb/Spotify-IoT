from socket import *
import requests
import urllib3

# Disable warnings
http = urllib3.PoolManager()
urllib3.disable_warnings()

# URI to the REST Service
URI = "http://localhost:5093/api/Moods"

serverPort = 7000
serverSocket = socket(AF_INET, SOCK_DGRAM)
serverSocket.bind(('', serverPort))

# While the program is running, execute code to receive broadcast messages from RaspberryPi and make Post requests to REST Service
print("The server is ready to receive")
while True:
    # Receive message from RaspberryPi
    message, clientAddress = serverSocket.recvfrom(2048)
    print("Message Received: " + message.decode())
    # Post message to REST Service
    url = URI + "?direction=" + message.decode()
    response = requests.put(url=url, verify=False)
    print("Status Code: " + str(response.status_code))
