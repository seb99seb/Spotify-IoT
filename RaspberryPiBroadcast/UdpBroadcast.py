from socket import *
import requests
import urllib3

http = urllib3.PoolManager()
urllib3.disable_warnings()

URI = "https://localhost:7234/Direction"

serverPort = 7234
serverSocket = socket(AF_INET, SOCK_DGRAM)
serverSocket.bind(('', serverPort))

print("The server is ready to receive")
while True:
    #Receive message from RaspberryPi
    message, clientAddress = serverSocket.recvfrom(2048)
    print("Message Received: " + message.decode())
    #Post message to REST Service
    url = URI + "?message=" + message.decode()
    response = requests.post(url=url, verify=False)
    print("Status Code: " + str(response.status_code))
