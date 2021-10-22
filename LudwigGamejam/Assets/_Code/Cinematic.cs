using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cinematic : MonoBehaviour
{
    public PlayerController player;
    public GameObject banana;
    public GameObject blackBG;
    public Options options;
    public CameraController camera;
    public AudioSource audio;
    public AudioClip monkeyFreakClip;
    public Text creditText;
    public Text timeText;

    public void StartCinematic()
    {
        player.isFrozen = true;
        camera.goToPlayer = false;
        StartCoroutine(CutOne(2));
    }

    IEnumerator CutOne(float delay)
    {
        yield return new WaitForSeconds(delay);
        camera.GetComponent<Camera>().orthographicSize = 1.2f;
        camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);
        StartCoroutine(CutTwo(2));
    }

    IEnumerator CutTwo(float delay)
    {
        yield return new WaitForSeconds(delay);
        camera.GetComponent<Camera>().orthographicSize = 1.2f;
        camera.transform.position = new Vector3(banana.transform.position.x, banana.transform.position.y, camera.transform.position.z);
        StartCoroutine(CutThree(2));
    }

    IEnumerator CutThree(float delay)
    {
        yield return new WaitForSeconds(delay);
        camera.GetComponent<Camera>().orthographicSize = 1;
        camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y, camera.transform.position.z);
        StartCoroutine(CutFour(2));
    }

    IEnumerator CutFour(float delay)
    {
        yield return new WaitForSeconds(delay);
        camera.GetComponent<Camera>().orthographicSize = .8f;
        camera.transform.position = new Vector3(banana.transform.position.x, banana.transform.position.y + .3f, camera.transform.position.z);
        StartCoroutine(CutFive(2));
    }

    IEnumerator CutFive(float delay)
    {
        yield return new WaitForSeconds(delay);
        camera.GetComponent<Camera>().orthographicSize = .5f;
        camera.transform.position = new Vector3(player.transform.position.x, player.transform.position.y + .5f, camera.transform.position.z);
        StartCoroutine(CutSix(2));
    }

    IEnumerator CutSix(float delay)
    {
        yield return new WaitForSeconds(delay);
        camera.GetComponent<Camera>().orthographicSize = .5f;
        camera.transform.position = new Vector3(banana.transform.position.x, banana.transform.position.y + .5f, camera.transform.position.z);
        StartCoroutine(CutSeven(3));
    }

    IEnumerator CutSeven(float delay)
    {
        yield return new WaitForSeconds(delay);
        blackBG.SetActive(true);
        audio.Stop();
        audio.clip = monkeyFreakClip;
        audio.Play();
        StartCoroutine(CutEight(5));
    }

    IEnumerator CutEight(float delay)
    {
        yield return new WaitForSeconds(delay);
        creditText.gameObject.SetActive(true);
        StartCoroutine(CutNine(5));
    }

    IEnumerator CutNine(float delay)
    {
        yield return new WaitForSeconds(delay);
        creditText.gameObject.SetActive(false);

        float playTime = options.playTime;
        float roundPlayMins = Mathf.RoundToInt((playTime / 60) - 0.5f);
        float secs = playTime - (roundPlayMins * 60);
        string minString = roundPlayMins.ToString("#");
        string secString = secs.ToString("n3");
        if (roundPlayMins < 1)
        {
            minString = "0";
        }
        else if (secs < 10)
        {
            secString = "0" + secs.ToString("n3");
        }


        timeText.text = "Your Final Time Is: " + minString + ":" + secString;

        timeText.gameObject.SetActive(true);
        StartCoroutine(CutTen(5));
    }

    IEnumerator CutTen(float delay)
    {
        yield return new WaitForSeconds(delay);
        timeText.gameObject.SetActive(false);
        camera.GetComponent<Camera>().orthographicSize = 5;
        camera.StartGoingDown();
        camera.transform.position = new Vector3(0, camera.transform.position.y - 5, camera.transform.position.z);
        StartCoroutine(CutEleven(3));
    }

    IEnumerator CutEleven(float delay)
    {
        yield return new WaitForSeconds(delay);
        blackBG.SetActive(false);
        //StartCoroutine(CutFive(3));
    }
}
