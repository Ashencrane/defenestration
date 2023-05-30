using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticles : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine("DeleteAfterTime");
    }
    IEnumerator DeleteAfterTime()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
