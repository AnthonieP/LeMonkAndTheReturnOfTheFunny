using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [System.Serializable]
    public class MusicZone
    {
        public Vector2 musicZone;
        public AudioClip musicClip;
    }

    public bool isMuted = false;
    public MusicZone[] musicZones;
    public AudioSource audioSource;
    public PlayerController player;
    public float audioFadeSpeed;

    private void Update()
    {
        if (isMuted)
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;

            //Check musiczone
            int curMusicZone = 99;
            for (int i = 0; i < musicZones.Length; i++)
            {
                if(player.transform.position.y > musicZones[i].musicZone.x && player.transform.position.y < musicZones[i].musicZone.y)
                {
                    curMusicZone = i;
                }
            }

            //Set clip
            float curVolume = audioSource.volume;
            if(curMusicZone == 99)
            {
                curVolume -= audioFadeSpeed * Time.deltaTime;
            }
            else
            {
                curVolume += audioFadeSpeed * Time.deltaTime;
                if(musicZones[curMusicZone].musicClip != audioSource.clip)
                {
                    audioSource.Stop();
                    audioSource.clip = musicZones[curMusicZone].musicClip;
                    audioSource.Play();
                }
            }

            //Clamp and set volume
            if(curVolume < 0)
            {
                curVolume = 0;
            }
            else if(curVolume > 1)
            {
                curVolume = 1;
            }
            audioSource.volume = curVolume;
        }
    }
}
