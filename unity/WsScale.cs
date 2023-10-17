using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WebSocketSharp;

public class WsScale : MonoBehaviour
{
    private WebSocket ws;
    private Vector3 scale;
    
    // Start is called before the first frame update
    void Awake()
    {
        // connect to the websocket
        ws = new WebSocket("ws://localhost:1880");
        // connect to the websocket
        ws.Connect();


        // check if the websocket is connected
        if (ws.ReadyState == WebSocketState.Open)
        {
            // log a message to the console
            Debug.Log("Connected to websocket");
            // send a message
            ws.Send("Hello world");
        }

        // when a message is received parse its argument 
        // as integer, map its range to 0 - 1 and use it to scale the cube
        ws.OnMessage += (sender, e) =>
        {
            Debug.Log("Message received from websocket");
            Debug.Log(e.Data);
            // split the message with spaces, check if the first part is "Scale"
            // and if it is, parse the second part as float
            string[] parts = e.Data.Split(' ');
            Debug.Log(parts[0]);

            if (parts[0] == "/scale")
            {
                
                if (float.TryParse(parts[1], out float scaleValue))
                {   
                    Debug.Log(scaleValue);
                    float mappedScaleValue = scaleValue / 300f;
                    Debug.Log(mappedScaleValue);
                    // scale the cube
                    scale.Set(mappedScaleValue, mappedScaleValue, mappedScaleValue);
                }
            }     
                    
        };
        

        
    }

    // Update is called once per frame
    void Update()
    {
        // linearly interpolate the current scale to the new scale
        transform.localScale = Vector3.Lerp(transform.localScale, scale, Time.deltaTime * 10f);

        // spin the cube slowly
        transform.Rotate(0.05f, 0.04f, 0.03f);

    }

    void OnDestroy()
    {
        if (ws != null){
            ws.Close();
        }
    }


}
