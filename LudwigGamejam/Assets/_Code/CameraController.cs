using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lerpSpeed;
    public GameObject player;
    public Vector3 lerpOffset;
    public bool goToPlayer = true;

    public float goBackSpeed;
    public bool goBack = false;
    public float yLevelToRestart;

    private void Update()
    {
        if (goToPlayer)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, player.transform.position.y, transform.position.z) + lerpOffset, lerpSpeed * Time.deltaTime);
        }

        if (goBack)
        {
            transform.Translate(0, -goBackSpeed * Time.deltaTime, 0); 
            if(transform.position.y < yLevelToRestart)
            {
                goBack = false;
                FindObjectOfType<Options>().RestartConfirmationButton();
                goToPlayer = true;
                FindObjectOfType<Cinematic>().endSong.Stop();
            }
        }
    }

    public void GoToPlayer()
    {
        if (goToPlayer)
        {
            transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z) + lerpOffset;
        }
    }

    public void StartGoingDown()
    {
        goBack = true;
    }
}
