using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Collections.Generic;

public class MultithreadedUDPServer : MonoBehaviour
{
    public ControlUI controlUI;
    private Thread udpServerThread;
    private UdpClient udpClient;
    private IPEndPoint endPoint;
    private bool isRunning = false;
    private string receivedMessage = string.Empty;

    public int port = 12345;

    void Start()
    {
        // 启动UDP服务器线程
        udpServerThread = new Thread(new ThreadStart(StartUDPServer));
        udpServerThread.IsBackground = true;
        isRunning = true;
        udpServerThread.Start();
    }

    void StartUDPServer()
    {
        udpClient = new UdpClient(port);
        endPoint = new IPEndPoint(IPAddress.Any, port);

        while (isRunning)
        {
            try
            {
                byte[] data = udpClient.Receive(ref endPoint);
                receivedMessage = Encoding.UTF8.GetString(data);
                //Debug.Log("Received: " + receivedMessage);
            }
            catch (SocketException ex)
            {
                Debug.Log("SocketException: " + ex.Message);
            }
        }

        udpClient.Close();
    }

    void OnApplicationQuit()
    {
        // 关闭服务器线程
        isRunning = false;
        if (udpClient != null)
        {
            udpClient.Close();
        }
        if (udpServerThread != null && udpServerThread.IsAlive)
        {
            udpServerThread.Abort();
        }
    }

    void Update()
    {
        // 在主线程中处理接收到的消息
        if (!string.IsNullOrEmpty(receivedMessage))
        {
            //Debug.Log("Message processed in Update: " + receivedMessage);
            var str = receivedMessage.Split(';');
            controlUI.ShowBoxInfo(new List<string>(str));
            receivedMessage = string.Empty;
        }
    }
}
