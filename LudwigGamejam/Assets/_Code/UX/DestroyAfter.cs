using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfter : MonoBehaviour
{
    public float afterSecs;

    private void Update()
    {
        afterSecs -= Time.deltaTime;
        if(afterSecs < 0)
        {
            Destroy(gameObject);
        }
    }
}
