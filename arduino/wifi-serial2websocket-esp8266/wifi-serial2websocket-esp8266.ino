#include <ArduinoWebsockets.h>
#include <ESP8266WiFi.h>

const char* ssid = "*******"; //Enter SSID
const char* password = "*******"; //Enter Password
const char* websockets_server_host = "XXX.XXX.XXX.XXX"; //Enter server adress
const uint16_t websockets_server_port = 1880; // Enter server port

using namespace websockets;

WebsocketsClient client;
void setup() {
    Serial.begin(9600);
    // Connect to wifi
    WiFi.begin(ssid, password);

    // Wait some time to connect to wifi
    for(int i = 0; i < 30 && WiFi.status() != WL_CONNECTED; i++) {
        Serial.print(".");
        delay(1000);
    }

    // Check if connected to wifi
    if(WiFi.status() != WL_CONNECTED) {
        Serial.println("No Wifi!");
        return;
    }

    Serial.println("WiFi connected");
    Serial.println("IP address: ");
    Serial.println(WiFi.localIP());


    Serial.println("Connecting to server.");
    // try to connect to Websockets server
    bool connected = client.connect(websockets_server_host, websockets_server_port, "/");
    if(connected) {
        Serial.println("Connecetd!");
        client.send("Hello Server");
    } else {
        Serial.println("Not Connected!");
    }

    client.onMessage([](WebsocketsMessage msg){
        Serial.println(msg.data());
    });
    
    
}

String c;

void loop() {
    client.poll();
    if (Serial.available() > 0) { // if serial is available
      c = Serial.readStringUntil('\n'); // get the last string from the serial
      client.send(c.substring(0, c.length() - 1)); // trim the new line and send to the websocket
    }
}