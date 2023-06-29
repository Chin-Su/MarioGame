using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RuaXanhChet : MonoBehaviour
{
    Vector2 Vitrichet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       // Vitrichet = transform.localPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < 0)
        {
            Vitrichet = transform.position;
            GameObject HinhBep = (GameObject)Instantiate(Resources.Load("Prefabs/RuaXanhChet"));
            HinhBep.transform.position = Vitrichet;
            Destroy(gameObject);
        }
    }
}
