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
    [Header("Effects")]
    public ParticleSystem explosionParticle;
    [Header("Sounds")]
    public GameObject explosionSoundObj;
    public GameObject weeeSoundObj;
    [Header("Animation")]
    public Animator animator;
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
        player.isFrozen = true;
        player.model.SetActive(false);
        player.rigidbody.useGravity = false;
        player.rigidbody.velocity = Vector3.zero;
        playerIsInside = true;
        animator.SetBool("Shoot", false);
        animator.SetBool("GotIn", true);
    }

    void ShootPlayer()
    {
        Instantiate(weeeSoundObj, transform.position, Quaternion.identity);
        Instantiate(explosionSoundObj, transform.position, Quaternion.identity);
        if(explosionParticle != null)
        {
            explosionParticle.Play();
        }

        player.transform.position = transform.position;
        player.model.SetActive(true);
        player.rigidbody.useGravity = true;
        player.rigidbody.AddForce(transform.up * shootForce);
        player.RotateTo(transform.up, player.model.transform, 99);
        player.model.transform.Rotate(0, 180, 0);
        playerIsInside = false;
        animator.SetBool("GotIn", false);
        animator.SetBool("Shoot", true);
        StartCoroutine(UnFreezePlayer(.3f));
    }


    IEnumerator UnFreezePlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        player.isFrozen = false;
    }
}
