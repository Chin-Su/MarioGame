using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NamDocChet : MonoBehaviour
{
    Vector2 Vitrichet; // Lấy ra vị trí mà nấm độc chết để set animation và không cho di chuyển
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // Hàm xử lý va chạm bằng collision
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu như đối tượng khác va chạm với đối tượng hiện tại(Nấm độc) và va chạm theo chiều từ trên xuống (y < 0)
        if (collision.gameObject.CompareTag("Player") && collision.contacts[0].normal.y < 0)
        {
            Vitrichet = transform.position; // Thiết lập vị trí chết bằng vị trí hiện tại
            // Lấy ra hình ảnh nấm độc sau khi chết
            GameObject HinhBep = (GameObject)Instantiate(Resources.Load("Prefabs/NamDoChet"));
            // Thiết lập vị trí cho hình nấm độc bằng vị trí chết đã set ở trên
            HinhBep.transform.position = Vitrichet;
            Destroy(gameObject); // Phá hủy đối tượng nấm độc sống
        }
    }
}
