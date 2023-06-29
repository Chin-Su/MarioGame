using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuaiBienMat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(HQuaiBienMat());
    }

    // Update is called once per frame
   
    IEnumerator HQuaiBienMat()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
    }
}
