using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Stats")]
    public float minPullForce;
    public float maxPullForce;
    public float pullForceAdd;
    float pullForce;
    public float pullRange;
    public float floorRange;
    [Header("MeshStreching")]
    public Vector3 minScale;
    Vector3 startScale;
    public float stretchBackSpeed;
    [Header("Components")]
    public Rigidbody rigidbody;
    public Camera camera;
    public GameObject pullPointer;
    public GameObject model;
    public GameObject hatSlot;
    public GameObject[] hats;
    public int curHatID;
    [Header("States")]
    bool isHoldingFire1 = false;
    Vector3 mousePos;
    Quaternion modelStartRot;
    public float modelLerpRot;
    public bool isPaused = true;
    public bool isFrozen = false;
    [Header("Effects")]
    public ParticleSystem fartParticle;
    public GameObject wallHitParticleObj;
    public GameObject floorHitParticleObj;
    public GameObject pickUpParticleObj;
    [Header("Sounds")]
    public GameObject fart1SoundObj;
    public GameObject uhOhSoundObj;
    [Range(0f, 1f)]
    public float uhOhSoundChance;
    public float uhOhSoundDelay;
    public GameObject wallHitSoundObj;
    public GameObject floorHitSoundObj;
    public GameObject pickUpSoundObj;

    private void Start()
    {
        pullForce = minPullForce;
        modelStartRot = model.transform.rotation;
        startScale = model.transform.localScale;
    }

    private void Update()
    {
        if (!isPaused && !isFrozen)
        {
            GetMousePos();
            PullManage();
            if (!isHoldingFire1)
            {
                model.transform.localScale = Vector3.Lerp(model.transform.localScale, startScale, stretchBackSpeed * Time.deltaTime);
            }
        }

    }

    void PullManage()
    {
        RaycastHit floorHit;
        if (Physics.Raycast(transform.position, -transform.up, out floorHit, floorRange) || Physics.Raycast(new Vector3(transform.position.x + 1, transform.position.y, transform.position.z), -transform.up, out floorHit, floorRange) || Physics.Raycast(new Vector3(transform.position.x - 1, transform.position.y, transform.position.z), -transform.up, out floorHit, floorRange))
        {
            model.transform.rotation = Quaternion.Lerp(model.transform.rotation, modelStartRot, modelLerpRot * Time.deltaTime);
            
            //start jump prep
            if (Input.GetButtonDown("Fire1"))
            {
                pullForce = minPullForce;
                isHoldingFire1 = true;
                pullPointer.SetActive(true);
            }

            //jump!
            if (Input.GetButtonUp("Fire1"))
            {
                isHoldingFire1 = false;
                pullPointer.SetActive(false);

                Vector3 rayDir =  Vector3.Normalize(new Vector3(mousePos.x, mousePos.y, transform.position.z) - transform.position);
                rigidbody.AddForce(rayDir * pullForce);
                Vector3 rotPos = new Vector3(mousePos.x, mousePos.y, model.transform.position.z);
                RotateTo(rotPos, model.transform, 999);
                model.transform.Rotate(0, 180, 0);

                fartParticle.Play();
                if(fart1SoundObj != null)
                {
                    Instantiate(fart1SoundObj, transform.position, Quaternion.identity);
                }
                if(Random.Range(0f, 1f) < uhOhSoundChance && uhOhSoundObj != null)
                {
                    StartCoroutine(DelayedSpawn(uhOhSoundObj, uhOhSoundDelay));
                }
            }

            //is holding down left mouse
            if (isHoldingFire1)
            {

                Vector3 rotPos = new Vector3(mousePos.x, mousePos.y, pullPointer.transform.position.z);
                RotateTo(rotPos, pullPointer.transform, 99);
                pullPointer.transform.localScale = new Vector3(pullPointer.transform.localScale.x, pullPointer.transform.localScale.y, pullForce / maxPullForce);
                model.transform.localScale = Vector3.Lerp(startScale, minScale, (pullForce - minPullForce) / (maxPullForce - minPullForce));

                pullForce += Time.deltaTime * pullForceAdd;
                if(pullForce > maxPullForce)
                {
                    pullForce = maxPullForce;
                }
            }

        }
    }

    void GetMousePos()
    {
        RaycastHit hit;
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit))
        {
            mousePos = hit.point;
        }
    }

    public void PutOnHat(int hatID)
    {
        if (hatSlot.transform.childCount > 0)
        {
            Destroy(hatSlot.transform.GetChild(0).gameObject);
        }
        if(hats[hatID] != null)
        {
            Instantiate(hats[hatID], hatSlot.transform.position, hatSlot.transform.rotation, hatSlot.transform);
        }
        curHatID = hatID;
    }

    public void RotateTo(Vector3 target, Transform thingToRotate, float rotSpeed)
    {
        // Determine which direction to rotate towards
        Vector3 targetDirection = target - thingToRotate.position;

        // The step size is equal to speed times frame time.
        float singleStep = rotSpeed * Time.deltaTime;

        Vector3 newDirection = Vector3.RotateTowards(thingToRotate.forward, targetDirection, singleStep, 0.0f);

        thingToRotate.rotation = Quaternion.LookRotation(newDirection);
    }

    IEnumerator DelayedSpawn(GameObject spawnObj, float spawnDelay)
    {
        yield return new WaitForSeconds(spawnDelay);
        Instantiate(spawnObj, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Wall")
        {
            Instantiate(wallHitSoundObj, transform.position, Quaternion.identity);
            foreach (ContactPoint contact in collision.contacts)
            {
                if(contact.otherCollider.tag == "Wall")
                {
                    Instantiate(wallHitParticleObj, contact.point, Quaternion.identity);
                }
            }
        }
        if (collision.transform.tag == "Floor")
        {
            Instantiate(floorHitSoundObj, transform.position, Quaternion.identity);
            foreach (ContactPoint contact in collision.contacts)
            {
                if (contact.otherCollider.tag == "Floor")
                {
                    Instantiate(floorHitParticleObj, contact.point, Quaternion.identity);
                }
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Hat")
        {
            if(hatSlot.transform.childCount > 0)
            {
                Destroy(hatSlot.transform.GetChild(0).gameObject);
            }

            Instantiate(pickUpParticleObj, other.transform.position, Quaternion.identity);
            Instantiate(pickUpSoundObj, transform.position, Quaternion.identity);
            PutOnHat(other.GetComponent<Hat>().hatID);
            Destroy(other.gameObject);
        }

        if (!isFrozen)
        {
            if(other.transform.tag == "Barrel")
            {
                other.GetComponent<Barrel>().ShootPrep();
            }
        }

        if (other.transform.tag == "Mushroom")
        {
            other.transform.GetComponent<Mushroom>().BouncePlayer(this);
        }

        if(other.tag == "Cinematic")
        {
            other.GetComponent<Cinematic>().StartCinematic();
        }
    }
}
