using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using VideoServer;

public class SwitchScenes : MonoBehaviour
{

    public ClickNode[] clickNodes;

    public static SwitchScenes instance;
    // Start is called before the first frame update
    void Start()
    {
        EventCenter.AddListener(EventDefine.ShowVideo, HideInterAction);
        EventCenter.AddListener(EventDefine.ShowInteraction, ShowInterAction);

        EventCenter.AddListener(EventDefine.ini, ini);
        instance = this;
    }

    private void ini()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void HideInterAction()
    {
        SendUPDData.instance.udp_Send(ValueSheet.getPBUDP(), "127.0.0.1", 29010);

        Server_MediaCtr.instance.ShowVideo();

        StartCoroutine(ClientHideInteraction());
    }

    private void ShowInterAction()
    {
        Server_MediaCtr.instance.HideVideo();

        StartCoroutine(ClientShowInteraction());
    } 

    public IEnumerator ClientHideInteraction()
    {
        foreach (var item in ValueSheet.serverRoot.clientIP)
        {
            yield return new WaitForSeconds(0.05f);

            SendUPDData.instance.udp_Send(ValueSheet.serverRoot.TriggerVideoUDP, item, ValueSheet.serverRoot.clientUdpPort);
        }
    }

    public IEnumerator ClientShowInteraction()
    {
        foreach (var item in ValueSheet.serverRoot.clientIP)
        {
            yield return new WaitForSeconds(0.05f);

            SendUPDData.instance.udp_Send(ValueSheet.serverRoot.TriggerInteractionUDP, item, ValueSheet.serverRoot.clientUdpPort);
        }
    }
}
