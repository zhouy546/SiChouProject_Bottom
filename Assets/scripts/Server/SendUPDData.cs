﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
namespace VideoServer
{
    /// <summary>
    ///发送UDP字符串udpData_str
    /// </summary>
    public class SendUPDData : MonoBehaviour
    {

        public static SendUPDData instance;


        Socket udpserver = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);


        public bool udp_Send(string da, string ip, int port)
        {
            try
            {
                //设置服务IP，设置端口号
                IPEndPoint ipep = new IPEndPoint(IPAddress.Parse(ip), port);
                //发送数据
                byte[] data = new byte[1024];
                data = Encoding.GetEncoding("gb2312").GetBytes(da);
                udpserver.SendTo(data, data.Length, SocketFlags.None, ipep);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Use this for initialization
        void Start()
        {
            EventCenter.AddListener(EventDefine.ini, initialization);

        }

        public void initialization()
        {
            if (instance == null)
            {
                instance = this;
            }

        }

        public void sendMsgtoClient(string s)
        {
            foreach (var item in ValueSheet.serverRoot.clientIP)
            {
                udp_Send(s, item, ValueSheet.serverRoot.ServerTcpPort);
            }
        }

    }
}