using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mushroom : MonoBehaviour
{
    public float bounceForce;
    public Animator animator;
    public GameObject bounceSoundObj;

    public void BouncePlayer(PlayerController player)
    {
        player.rigidbody.velocity = Vector3.zero;
        player.rigidbody.AddForce(transform.up * bounceForce);
        animator.Play("MushroomBounce");
        Instantiate(bounceSoundObj, transform.position, Quaternion.identity);
    }
}
