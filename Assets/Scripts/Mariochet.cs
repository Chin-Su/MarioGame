using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mariochet : MonoBehaviour
{
    Vector2 Vitrichet;
    public float DoNayCao = 120f;

    public Rigidbody2D rgPlayer;
    private float timeChange = 3;
        private bool check = true;

    private void Start()
    {
        StartCoroutine(HMarioChet());
    }

    private void Update()
    {
        if (transform.position.y < -20)
            Destroy(gameObject);
    }

    IEnumerator HMarioChet()
    {
        rgPlayer.AddForce(transform.up * DoNayCao, ForceMode2D.Impulse);
        yield return new WaitForSeconds(2);


        SceneManager.LoadScene("GameOver");
        yield return null;
    }
}
