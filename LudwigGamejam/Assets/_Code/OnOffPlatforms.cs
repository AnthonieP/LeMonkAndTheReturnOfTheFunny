using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnOffPlatforms : MonoBehaviour
{
    public GameObject[] platformObjs;
    public float platformSwitchTime;
    float platformSwitchTimeTime = 0;

    private void Update()
    {
        platformSwitchTimeTime -= Time.deltaTime;
        if(platformSwitchTimeTime < 0)
        {
            platformSwitchTimeTime = platformSwitchTime;
            for (int i = 0; i < platformObjs.Length; i++)
            {
                platformObjs[i].SetActive(!platformObjs[i].active);
            }
        }
    }
}
