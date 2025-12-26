using System.Net;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameClient : MonoBehaviour
{
    UdpClient client;
    IPEndPoint serverEndPoint;

    void Start()
    {
        client = new UdpClient();
        serverEndPoint = new IPEndPoint(
            IPAddress.Parse("172.27.120.12"), // server IP
            7777                          // server port
        );

        SendJoin();
        InvokeRepeating(nameof(SendHeartbeat), 1f, 1f);
    }

    void SendJoin()
    {
        byte[] packet = new byte[] { 1 }; // JOIN
        client.Send(packet, packet.Length, serverEndPoint);
        Debug.Log("JOIN sent");
    }

    void SendHeartbeat()
    {
        byte[] packet = new byte[] { 3 }; // HEARTBEAT
        client.Send(packet, packet.Length, serverEndPoint);
    }

    void Update()
    {
        Vector2 move = Vector2.zero;

        var kb = Keyboard.current;
        if (kb == null) return;

        if (kb.wKey.isPressed) move.y += 1;
        if (kb.sKey.isPressed) move.y -= 1;
        if (kb.aKey.isPressed) move.x -= 1;
        if (kb.dKey.isPressed) move.x += 1;

        // send move to server
    }

    void OnApplicationQuit()
    {
        client.Close();
    }
}
