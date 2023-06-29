using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private Transform player; // Lấy ra component tranform của player

    private float minX = 0, maxX = 204; // Thiết lập vị trí x tối thiểu và tối đa của camera

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform; // Lấy transform của player
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null) // Kiểm tra nếu player khác null
        {
            Vector3 thisPosition = transform.position; // Lấy ra vị trí của camera
            thisPosition.x = player.position.x; // Thiết lập vị trí camera = vị trí của player
            if (thisPosition.x < minX) // Kiểm tra nếu vị trí camera < vị trí min
                thisPosition.x = minX; // Gán lại vị trí = vị trí min
            if (thisPosition.x > maxX)
                thisPosition.x = maxX;
            transform.position = thisPosition;
        }
    }
}
