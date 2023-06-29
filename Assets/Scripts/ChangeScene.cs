using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    public string level;

    private static int lev = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            SceneManager.LoadScene(level);
            if (level == "Level_1")
                lev = 1;
            else if (level == "Level_2")
                lev = 2;
            else
            {
                lev = 3;
            }
        }
               
    }

    public void LoadScene()
    {
        if(lev == 1)
            SceneManager.LoadScene("Level_1");
        else if(lev == 2)
            SceneManager.LoadScene("Level_2");
        else
        {
            SceneManager.LoadScene("Level_3");
        }
    }
}
