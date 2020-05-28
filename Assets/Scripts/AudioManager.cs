using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[RequireComponent(typeof(AudioSource))]
public class AudioManager : MonoBehaviour
{
    [System.Serializable]
    public struct AudioClipEventPair{
        public string eventName;
        public AudioClip clip;
    }

    public AudioClipEventPair[] clips;
    AudioSource audioSource;
    public bool isMuted {
        get { return audioSource.mute; }
    }
    // Use this for initialization
    void Start()
    {
        GameEventManager.OnMessage += OnMessageHandler;
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnMessageHandler(string msg, object obj) {

        foreach(AudioClipEventPair clip in clips){

            if(clip.eventName == msg){
                audioSource.PlayOneShot(clip.clip);
                break;
            }
        }

    }

    public bool ToggleMute() {
        audioSource.mute = !audioSource.mute;
        return audioSource.mute;
    }

    public void Mute(bool muted=true) {
        audioSource.mute = muted;
    }
}
