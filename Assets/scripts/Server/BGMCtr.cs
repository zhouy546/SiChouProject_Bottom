using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace VideoServer
{
    public class BGMCtr : MonoBehaviour
    {
        public AudioSource audioSource;
        public AudioClip audioclip_bgm;
        private void Awake()
        {
            EventCenter.AddListener(EventDefine.ShowInteraction, playBGM);
            EventCenter.AddListener(EventDefine.ShowVideo, stopBGM);
            EventCenter.AddListener(EventDefine.ShowVideo, INI);

        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //Debug.Log(audioSource.volume);
        }

        public void INI()
        {
            audioSource.volume = ((float)ValueSheet.serverRoot.BGMVloume / 10F);
        }

        public void playBGM()
        {
            audioSource.mute = false;

            audioSource.PlayOneShot(audioclip_bgm);

            audioSource.volume = ((float)ValueSheet.serverRoot.BGMVloume / 10F);


        }

        public void stopBGM()
        {
            audioSource.Stop();
            audioSource.mute = true;
        }
    }

}
