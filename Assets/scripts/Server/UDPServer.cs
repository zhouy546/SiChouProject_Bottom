using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;

namespace VideoServer
{
    public class UDPServer : MonoBehaviour
    {

        public int port = 29010;
        UdpClient udpServer;
        string receivedMessage=" ";

        private void Awake()
        {
            EventCenter.AddListener(EventDefine.ini, INI);
        }

        void INI()
        {
            //创建UDP服务器
            udpServer = new UdpClient(port);
            udpServer.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
            //Debug.Log("UDP Server started on port " + port);
        }

        void ReceiveCallback(System.IAsyncResult asyncResult)
        {
            IPEndPoint remoteEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] receivedBytes = udpServer.EndReceive(asyncResult, ref remoteEndPoint);
            receivedMessage = Encoding.GetEncoding("gb2312").GetString(receivedBytes);

            //处理收到的消息
            Debug.Log("Message received from " + remoteEndPoint.Address + ":" + remoteEndPoint.Port + " - " + receivedMessage);

            //开始接收下一个数据包
            udpServer.BeginReceive(new System.AsyncCallback(ReceiveCallback), null);
        }

        private void Update()
        {
            if (receivedMessage != " ")
            {
                dealwithMsg(receivedMessage);
                receivedMessage = " ";
            }
        }

        void dealwithMsg(string s)
        {
            Debug.Log("Running");

            if (ValueSheet.udp_videoinfo.ContainsKey(s))
            {
                Server_MediaCtr.instance.playVideo(s);

                foreach (string item in ValueSheet.serverRoot.clientIP)
                {
                    SendUPDData.instance.udp_Send(s,item, ValueSheet.serverRoot.clientUdpPort);

                }
            }else if (s==ValueSheet.serverRoot.volumedown)
            {
                EventCenter.Broadcast(EventDefine.volumdown);
            }
            else if (s == ValueSheet.serverRoot.volumeup)
            {
                EventCenter.Broadcast(EventDefine.volumeup);

            }else if(s==ValueSheet.serverRoot.TriggerInteractionUDP)
            {

                ValueSheet.state = State.interaction;

                EventCenter.Broadcast(EventDefine.ShowInteraction);
            }
            else if (s == ValueSheet.serverRoot.TriggerVideoUDP)
            {

                ValueSheet.state = State.video;

                EventCenter.Broadcast(EventDefine.ShowVideo);
            }
        }

        void OnApplicationQuit()
        {
            //关闭UDP服务器
            udpServer.Close();
        }
    }


}
