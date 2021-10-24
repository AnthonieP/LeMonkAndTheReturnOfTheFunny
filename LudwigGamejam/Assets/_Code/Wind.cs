using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float windForce;
    public ParticleSystem windParticle;
    public BoxCollider windCollider;
    PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player" && !player.isInBarrel)
        {
            other.GetComponent<Rigidbody>().AddForce(transform.right * windForce * Time.deltaTime);
        }
    }
}
