using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [Header("Stats")]
    public Vector3 rotateSpeed;
    public float shootForce;
    public float stopRotTime;
    Quaternion startRot;
    [Header("States")]
    public bool playerIsInside;
    public bool stopsRotAfterTime;
    [Header("Components")]
    public PlayerController player;

    private void Start()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private void Update()
    {
        if (playerIsInside)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                ShootPlayer();
            }

            stopRotTime -= Time.deltaTime;
            if(stopRotTime < 0 && stopsRotAfterTime)
            {
                transform.rotation = startRot;
            }
            else
            {
                transform.Rotate(rotateSpeed * Time.deltaTime);
            }
        }
    }

    public void ShootPrep()
    {
        player.model.SetActive(false);
        player.rigidbody.useGravity = false;
        player.rigidbody.velocity = Vector3.zero;
        playerIsInside = true;
    }

    void ShootPlayer()
    {
        player.model.SetActive(true);
        player.rigidbody.useGravity = true;
        player.rigidbody.AddForce(transform.up * shootForce);
        player.RotateTo(transform.up, player.model.transform, 99);
        playerIsInside = false;
    }

}
