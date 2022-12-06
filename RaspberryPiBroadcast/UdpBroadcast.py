from socket import *
import requests

URI = "https://localhost:7234/swagger/index.html"

serverPort = 7234
serverSocket = socket(AF_INET, SOCK_DGRAM)

serverSocket.bind(('', serverPort))
response = requests.get(URI)

print("The server is ready to receive")

while True:
    message, clientAddress = serverSocket.recvfrom(2048)
    print(message.decode())
