using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float windForce;
    public ParticleSystem windParticle;
    public BoxCollider windCollider;

    private void Start()
    {
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Rigidbody>().AddForce(transform.right * windForce * Time.deltaTime);
        }
    }
}
