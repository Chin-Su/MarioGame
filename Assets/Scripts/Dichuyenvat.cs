using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dichuyenvat : MonoBehaviour
{
    public float VanTocVat;
    public bool DichChuyenTrai = true;

    public void Update()
    {
        if (DichChuyenTrai)
            transform.position += new Vector3(VanTocVat * Time.deltaTime, 0, 0);
        else
            transform.position -= new Vector3(VanTocVat * Time.deltaTime, 0, 0);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(!collision.gameObject.CompareTag("Player"))
        {
            DichChuyenTrai = collision.contacts[0].normal.x > 0;
            if (DichChuyenTrai)
                transform.Rotate(0, 180, 0);
            else
                transform.Rotate(0, 180, 0);
        }
    }
}   

