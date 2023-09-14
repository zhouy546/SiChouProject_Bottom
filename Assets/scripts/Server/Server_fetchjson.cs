using LitJson;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

namespace VideoServer
{
    public class Server_fetchjson : MonoBehaviour
    {

        
        // Start is called before the first frame update
        void Start()
        {
            StartCoroutine(LoadJson());

           // createJson();
        }

        // Update is called once per frame
        void Update()
        {

        }


        void createJson()
        {
            ConfigRoot configRoot = new ConfigRoot();
            nodes leftNodes = new nodes();
            nodes rightNodes = new nodes();
            List<JsonNode> jsonNodes = new List<JsonNode>();
            for (int i = 0; i < 1; i++)
            {
                JsonNode jsonNode = new JsonNode();
                jsonNode.TextureURL = "textureURL";
                jsonNode.textURL = "TEXTURL";
                jsonNode.TitleURL = "TITLEURL";
                jsonNodes.Add(jsonNode);
            }

            leftNodes.JsonNodes = jsonNodes;

            leftNodes.isInfoComplete = true;

            List<JsonNode> _jsonNodes = new List<JsonNode>();

            for (int i = 0; i < 2; i++)
            {
                JsonNode jsonNode = new JsonNode();
                jsonNode.TextureURL = "textureURL";
                jsonNode.textURL = "TEXTURL";
                jsonNode.TitleURL = "TITLEURL";
                _jsonNodes.Add(jsonNode);
            }
            rightNodes.JsonNodes = _jsonNodes;
            rightNodes.isInfoComplete = false;

            configRoot.left = leftNodes;
            configRoot.right = rightNodes;

            string json = JsonMapper.ToJson(configRoot);

            string filepath = Path.Combine(Application.streamingAssetsPath, "config.json");

            File.WriteAllText(filepath, json);
        }

        IEnumerator LoadJson()
        {
            string url = Application.streamingAssetsPath + "/server.json";

            WWW www = new WWW(url);

            yield return www;

            string jsonString = Encoding.UTF8.GetString(www.bytes);

            ValueSheet.serverRoot= JsonMapper.ToObject<ServerRoot>(jsonString.ToString());

            foreach (var item in ValueSheet.serverRoot.Servervideo)
            {
                ValueSheet.udp_videoinfo.Add(item.udp, item);
            }



            string configurl = Application.streamingAssetsPath + "/config.json";

            WWW Configwww = new WWW(configurl);

            yield return Configwww;

            string ConfigjsonString = Encoding.UTF8.GetString(Configwww.bytes);

            ValueSheet.ConfigRoot = JsonMapper.ToObject<ConfigRoot>(ConfigjsonString);

            EventCenter.Broadcast(EventDefine.ini);

            ValueSheet.isIni = true;
        }
    }
}

