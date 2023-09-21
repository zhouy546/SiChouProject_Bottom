using DemolitionStudios.DemolitionMedia;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace VideoServer
{
    public class Server_MediaCtr : MonoBehaviour
    {
        public Media mediaPlayer;

        public RenderToUGUI renderToUGUI;

        ServervideoItem CurrentServervideoItem;

        public AudioMixer audioMixer;

        public static Server_MediaCtr instance;

        public int currentFrame;

        public int perviousFrame=-1;


        class EventEntry
        {
            public EventEntry(string eventName)
            {
                EventName = eventName;
            }


            public string EventName;
        }
        private List<EventEntry> _events = new List<EventEntry>();


        public void Awake()
        {
            EventCenter.AddListener(EventDefine.ini, ini);
            EventCenter.AddListener(EventDefine.volumdown, VolumeDown); 
            EventCenter.AddListener(EventDefine.volumeup, VolumeUp);

            EventCenter.AddListener(EventDefine.ShowInteraction, Mute);
            EventCenter.AddListener(EventDefine.ShowVideo, UNmute);

        }




        public void FixedUpdate()
        {


            //CheckReachEnd();


            if (mediaPlayer.IsPlaying)
            {

                currentFrame = mediaPlayer.VideoCurrentFrame;

                if (currentFrame != perviousFrame)
                {
                    SendUPDData.instance.sendMsgtoClient(currentFrame.ToString());
                }

                perviousFrame = currentFrame;

            }

            /*

            for (int i = 0; i < _events.Count; ++i)
            {
                var elem = _events[i];

                //Debug.Log(elem.EventName);

                if (elem.EventName == "PlaybackStopped")
                {
                    //    Debug.Log(elem.EventName);
                    if (CurrentServervideoItem.isBackToScreenProtect)
                    {
                       // playVideo(ValueSheet.getPBUDP());
                    }
                }
                else if (elem.EventName == "Opened")
                {
                    if (CurrentServervideoItem.udp != ValueSheet.getPBUDP())
                    {
                        //StartCoroutine(F_lightOff());

                      

                    }
                    else
                    {
                        //StartCoroutine(F_lightOn());

                        //   udp_Send("1200", "192.168.1.254", 29010);
                        
                    }
                }


                _events.Remove(elem);
                i -= 1;
            }

            */
        }

        private IEnumerator F_lightOn()
        {
            yield return new WaitForSeconds(0.5f);

            SendUPDData.instance.udp_Send(ValueSheet.serverRoot.lightOn, ValueSheet.serverRoot.lightingServerIP, ValueSheet.serverRoot.lightingServerPort);
            Debug.Log("开灯");

        }

        private IEnumerator F_lightOff()
        {
            yield return new WaitForSeconds(0.5f);

            SendUPDData.instance.udp_Send(ValueSheet.serverRoot.lightOff, ValueSheet.serverRoot.lightingServerIP, ValueSheet.serverRoot.lightingServerPort);

            Debug.Log("关闭电灯");
        }

        public void CheckReachEnd()
        {
            //Debug.Log(ValueSheet.IsTirggerScreenProtect);   

            if (ValueSheet.isIni)
            {
                if (mediaPlayer.VideoCurrentFrame == mediaPlayer.VideoNumFrames - 1)
                {
                    //Debug.Log("Running");
                    if (ValueSheet.IsTirggerScreenProtect == false)
                    {
                        ValueSheet.IsTirggerScreenProtect = true;

                        SendUPDData.instance.udp_Send(ValueSheet.getPBUDP(), "127.0.0.1", 29010);

                    }

                }
            }

        }



        public void ini()
        {
            instance = this;

            playVideo(ValueSheet.getPBUDP());

            mediaPlayer.Events.AddListener(OnMediaPlayerEvent);
        }


        public async void playVideo(string udp)
        {
            Debug.Log("播放视频UDP:" + udp);

            await PlayVideo(udp);
        }


        private async Task PlayVideo(string udp)
        {
            CurrentServervideoItem = ValueSheet.udp_videoinfo[udp];

            string path = Application.streamingAssetsPath + "/" + CurrentServervideoItem.url;

            //Debug.Log(path);

            mediaPlayer.Open(path);

            mediaPlayer.Loops = CurrentServervideoItem.loop;

            renderToUGUI.color = Color.black;

            await Task.Delay(ValueSheet.serverRoot.OnPlayDelay);

            renderToUGUI.color = Color.white;

            mediaPlayer.Play();

            if (!CurrentServervideoItem.iscreenprotect)
            {
                ValueSheet.IsTirggerScreenProtect = false;
            }

        }


        private void VolumeUp()
        {
            float value;
            audioMixer.GetFloat("Volume", out value);

            if (value < 20)
            {
                value += 5f;

            }
            else
            {
                value = 20;
            }

            audioMixer.SetFloat("Volume", value);

        }

        private void VolumeDown()
        {

            float value;
            audioMixer.GetFloat("Volume", out value);

            if (value > -80)
            {
                value -= 5f;

            }
            else
            {
                value = -80;
            }
            audioMixer.SetFloat("Volume", value);
        }

        private void Mute()
        {
            audioMixer.SetFloat("Volume", -80);
        }

        private void UNmute()
        {
            audioMixer.SetFloat("Volume", -5);
        }

        public void OnMediaPlayerEvent(Media source, MediaEvent.Type type, MediaError error)
        {
            if (error == MediaError.NoError)
            {
                //Debug.Log("[RenderToIMGUIWithControls] Event: " + type.ToString());
                _events.Add(new EventEntry(type.ToString()));
            }
            else
            {
                //Debug.LogError("[RenderToIMGUIWithControls] Error: " + error.ToString());
                _events.Add(new EventEntry(type.ToString() + ": " + error.ToString()));
            }
        }

        public void HideVideo()
        {
            renderToUGUI.color = new Color(1, 1, 1, 0);
        }

        public void ShowVideo()
        {
            renderToUGUI.color = new Color(1, 1, 1, 1);

        }
    }
}
