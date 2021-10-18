using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float lerpSpeed;
    public GameObject player;
    public Vector3 lerpOffset;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(transform.position.x, player.transform.position.y, transform.position.z) + lerpOffset, lerpSpeed * Time.deltaTime);
    }
}
