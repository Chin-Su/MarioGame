using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kethu : MonoBehaviour
{
    GameObject Mario;
    
    private void Awake()
    {
        Mario = GameObject.FindGameObjectWithTag("Player");
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && (collision.contacts[0].normal.x>0 || collision.contacts[0].normal.x < 0))
        {
            if (Mario.GetComponent<PlayerController>().level > 1)
            {
                Destroy(gameObject);
            }
            else
            {
                Mario.GetComponent<PlayerController>().MarioChet();
            }
        }
    }
}
