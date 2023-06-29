using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockItems : MonoBehaviour
{
    // private bool check = true;

    private Vector2 startPosition; // Vị trí ban đầu của khối block item

    private float speedUp = 4; // Tốc độ nảy lên

    public bool ChuaNam = false; // Item chứa nấm
    public bool ChuaQuai = false; // Item chứa quái
    public bool ChuaSao = false; // Item chứa sao

    GameObject player; // Lấy ra nhân vật
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position; // Thiết lập vị trí ban đầu = vị trí hiện tại của khối block
        player = GameObject.Find("Player"); // Lấy ra gameobject player
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D col) // Hàm xử ly va chạm dùng collision
    {
        if (col.collider.CompareTag("Collision") && col.contacts[0].normal.y > 0) // Kiểm tra nếu như khối block bị tác động từ bên dưới
        {
            UpBlock(); // Gọi hàm xử lý khối block nảy lên
        }
    }

    private void UpBlock() // Hàm xử lý khối block nảy lên
    {
        if (ChuaNam) // Nếu khối block chứa nấm
        {
            StartCoroutine(UpBlocks()); // Gọi hàm xử lý chứa nấm
            NamVaHoa(); // Tạo ra nấm
            ChuaNam = false; // Thiết lập chứa nấm = false
        } else if (ChuaQuai)
        {
            StartCoroutine(UpBlocks());
            NamVaHoa();
            ChuaQuai = false;
        }
        else if(ChuaSao)
        {
            StartCoroutine(UpBlocks());
            NamVaHoa();
            ChuaSao = false;
        }
    }

    IEnumerator UpBlocks() // Hàm xử lý nảy lên
    {
        while (true)
        {
            // Cho khối block nảy lên
            transform.position =
                new Vector2(transform.position.x, transform.position.y + speedUp * Time.deltaTime);
            if (transform.position.y >= startPosition.y + 0.5f) break;
            yield return null;
        }

        while (true)
        {
            // Cho khối block hạ xuống
            transform.position =
                new Vector2(transform.position.x, transform.position.y - speedUp * Time.deltaTime);
            if (transform.position.y <= startPosition.y)
            {
                Destroy(gameObject);
                GameObject emptyBlock = (GameObject)Instantiate(Resources.Load("Prefabs/EmptyBlock"));
                emptyBlock.transform.position = startPosition;
                break;
            }
            yield return null;
        }
    }

    public void NamVaHoa() // Hàm xử lý sinh item
    {
        int thisLevel = player.GetComponent<PlayerController>().level; // Lấy ra level của người chơi
        GameObject obj = null; // Tạo một gameobject
        if (ChuaNam) // Nếu khối chứa nấm thì tạo ra nấm
        {
            obj = (GameObject)Instantiate(Resources.Load("Prefabs/NamItem")); // Tạo ra nấm
            obj.transform.SetParent(this.transform.parent); // thiết lập đối tượng cha là blockitem
            obj.transform.position = new Vector2(startPosition.x, startPosition.y + 1); // Thiết lập vị trí là vị trí trên của khối block
        } else if (ChuaSao)
        {
            obj = (GameObject)Instantiate(Resources.Load("Prefabs/SaoItem"));
            obj.transform.SetParent(this.transform.parent);
            obj.transform.position = new Vector2(startPosition.x, startPosition.y + 1);
        }
        else if(ChuaQuai)
        {
            obj = (GameObject)Instantiate(Resources.Load("Prefabs/RuaXanh"));
            obj.transform.SetParent(this.transform.parent);
            obj.transform.position = new Vector2(startPosition.x, startPosition.y + 1);
        }

    }
}
