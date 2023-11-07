using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VideoServer
{
    public static class ValueSheet
    {
        public static ServerRoot serverRoot;

        public static ConfigRoot ConfigRoot;

        public static Dictionary<string, ServervideoItem> udp_videoinfo = new Dictionary<string, ServervideoItem>();

        public static State state = State.video;

        public static string getPBUDP()
        {
            foreach (var item in udp_videoinfo)
            {
                if (item.Value.iscreenprotect)
                {
                    return item.Key;
                }
            }
            return "100000";
        }

        public static bool IsTirggerScreenProtect;

        public static bool isIni;
    }


    public enum State
    {
        video,interaction,pb
    }

    public class ServervideoItem
    {
        /// <summary>
        /// 
        /// </summary>
        public int VideoIndex { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string udp { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public int loop { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string url { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool isBackToScreenProtect { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool iscreenprotect { get; set; }
    }

    public class ServerRoot
    {

        public List<ServervideoItem> Servervideo { get; set; }

        public bool isAutoLoop { get; set; }

        public int BGMVloume { get; set; }

        public int OnPlayDelay { get; set; }
        public int InteractionDuration { get; set; }

        public int PbDuration { get; set; }
        public int VideoDuration { get; set; }


        public List<string> clientIP { get; set; }

        public string TriggerInteractionUDP;

        public string TriggerVideoUDP;

        public string TriggerpbUDP;

        public int InteractionUDPport { get; set; }
        public int clientUdpPort { get; set; }

        public int ServerTcpPort { get; set; }

        public string TouchScreenIP { get; set; }

        public string STOPVIDEO { get; set; }

        public string pause { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string volumeup { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string volumedown { get; set; }


        public string lightingServerIP { get; set; }

        public int lightingServerPort { get; set; }

        public string lightOn { get; set; }

        public string lightOff { get; set; }
    }
 

    public class JsonNode
    {


        public string textURL { get; set; }

        public string TextureURL { get; set; }

        public string TitleURL { get; set; }
    }

    public class nodes
    {
        public bool isInfoComplete { get; set; }

        public string introURL { get; set; }
        public string name { get; set; }


        public List<JsonNode> JsonNodes { get; set; }

    }

    public class ConfigRoot
    {
        public nodes left { get; set; }

        public nodes right { get; set; }
    }
}
