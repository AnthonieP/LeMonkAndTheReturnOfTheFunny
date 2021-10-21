using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public Vector3 windForce;


    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<Rigidbody>().AddForce(windForce * Time.deltaTime);
        }
    }
}
