using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlay : MonoBehaviour
{
    public AudioSource audioSource;
    public float pitchDiff;

    private void Start()
    {
        audioSource.pitch += Random.Range(-pitchDiff, pitchDiff);
    }
}
