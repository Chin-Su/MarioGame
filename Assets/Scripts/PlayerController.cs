using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float velocity = 7; // Tốc độ của người chơi

    private float jumpSpeed = 460; // Lực nhảy

    private float falling = 15; // Tốc độ rơi sau khi nhảy lên

    private float lowJumping = 3; // Lực nhảy thấp

    private float speed = 1; // Chuyển trạng thái animation chạy hay đứng

    private bool isPlatform = true; // Kểm tra xem player có đứng trên nền tảng hay không

    private Animator playerAnim; // Animation

    private Rigidbody2D rgRigidbody2D; // Rigidbody component

    private float horizontalInput; // Nhận đầu vào từ người chơi

    private bool directionPlayer = true; // Hướng của nhận vật nhìn sang phải
    private float timeItems = 6; // Thời gian biến hình

    // Show level and big size of mario
    public int level = 0; // Level hiện tại của Mario

    public bool transfigure = false; // Có thể biến hình
    private bool checkBh = false; // Kiểm tra biến hình
    private bool up = false;
    private bool left = false;
    private bool right = false;
private bool onclickJump  =  false;

    private AudioSource _audioSource; // Audio component để xử lý âm thanh (thêm/ xóa)
    // Location mario dead
    private Vector2 ViTriChet; // Vị trí mà nhân vật chết
    void Start() // Hàm xử lý khi bắt đầu game
    {
        _audioSource = GetComponent<AudioSource>();
        rgRigidbody2D = GetComponent<Rigidbody2D>(); // Get component rigidbody to controller it
        playerAnim = GetComponent<Animator>(); // Get component animator to controller animation
        playerAnim.SetFloat("Speed", 0); // Establish state of player is stand
        playerAnim.SetBool("IsPlatform", true); // Establish state of player is on platform
        
        StartCoroutine(MarioSmall()); // Luôn giữ cho nhân vật ở level 0
        CreateAudio("power_down"); // Thêm hiệu ứng âm thanh giảm level
    }

    // Update is called once per frame
    void Update() // Hàm Update được gọi liên tục mỗi frame
    {
        if(checkBh) // Kiểm tra xem nhân vật có đang biến hình hay không
        {
            timeItems -= Time.deltaTime; // Giảm thời gian biến hình theo các frame
            if (level == 1) // Kiểm tra nếu level = 1 (ăn sao)
            {
                velocity = 10; // Thiết lâp tốc độ của nhân vật bằng 10 (tăng tốc độ)
            }

            if (timeItems < 0) // Nếu như thời gian biến hình < 0 (đ hết thời gian biến hình)
            {
                level = 0; // Thiết lập lại level = 0
                transfigure = true; // Có thể biến hình = true
                checkBh = false; // Set đã biến hình = false
                timeItems = 6; // Thiết lập lại thời gian biến hình = 6s
                velocity = 7; // Thiết lập lại vận tốc của nhân vật về giá trị mặc định
            }
                
        }
        playerAnim.SetFloat("Speed", speed); // Thiết lập animation của nhân vật luôn trong trạng thái đứng (animation)
        playerAnim.SetBool("IsPlatform", isPlatform); // Thiết lập animation của nhân vật luôn trong trạng thái đứng (animation)
        Jump(); // Hàm xử lý nhảy
        if (transfigure) // Kiểm tra nếu có thể biến hình
        {
            switch (level) // Kiểm tra các cấp độ
            {
                case 0:
                    StartCoroutine(MarioSmall()); // Gọi hàm biến hình level 0
                    CreateAudio("power_down"); // Âm thanh hạ level
                    transfigure = false; // Không thể biến hình
                    break;
                case 1:
                    StartCoroutine(MarioEatMushroom()); // Gọi hàm biến hình level 1
                    CreateAudio("power_up"); // Âm thanh tăng level
                    transfigure = false; // Không thể biến hình
                    checkBh = true; // Đã biến hình = true
                    break;
                case 2:
                    StartCoroutine(MarioEatFlower()); // Gọi hàm biến hình level 2
                    transfigure = false; // Không thể biến hình
                    CreateAudio("power_up"); // Âm thanh tăng level
                    checkBh = true; // Đã biến hình = true
                    break;
                default: // Mặc định
                    transfigure = false; // Không thể biến hình
                    break;
            }
        }
        if (gameObject.transform.position.y < -10f) // Kiểm tra nếu như nhân vật có độ cao < -10
        {
            MarioChet(); // Gọi hàm xử lý nhân vật chết
            Destroy(gameObject); // Phá hủy đối tượng nhân vật không cho điều khiển 
        }
    }

    private void FixedUpdate() // 0.2s gọi 1 lần
    {
        Move(); // Gọi hàm di chuyển
    }

    private void Move() // Hàm xử lý di chuyển (trái/ phải)
    {
        horizontalInput = left? -1 : right? 1 : 0; // Nhận đầu vào từ người chơi (A = -1, D = 1, nothing = 0)
        rgRigidbody2D.velocity = new Vector2(velocity * horizontalInput, rgRigidbody2D.velocity.y); // Thiết lập vấn tốc cho nhân vật khi người dùng bấm phím
        speed = MathF.Abs(horizontalInput); // Thiết lập trọng thái animation của nhân vật là trạng thái chạy
        if (horizontalInput > 0 && !directionPlayer) RotateDirection(); // Nếu di chuyển sang phải mà nhân vật nhìn trái -> chuyển nhân vật nhìn phải
        if (horizontalInput < 0 && directionPlayer) RotateDirection(); // Nếu di chuyển sang trái mà nhân vật nhìn phải -> chuyển nhân vật nhìn trái
    }

    private void RotateDirection() // Hàm xử lý chuyển hướng nhân vật
    {
        directionPlayer = !directionPlayer; // Chuyển nhìn phải thành trái và ngược lại
        var localScale = transform.localScale; // Lấy ra component scale của nhân vật (có thể dùng rotate)
        localScale = new Vector3(localScale.x * -1, localScale.y); // Scale âm thì nhân vật sẽ quay ngược lại
        transform.localScale = localScale; // Thiết lập lại scale cho nhân vật
    }

    private void Jump() // Hàm xử lý nhảy
    {
        if (up && isPlatform) // Kiểm tra nếu nhân vật đứng trên nền tảng và người chơi giữ phím space
        {
            isPlatform = false; // Khi nhảy lên thì không đứng trên nền tảng nữa (không cho double jump)
            rgRigidbody2D.AddForce(Vector2.up * jumpSpeed); // Thêm lực để nhân vật nhảy lên
            playerAnim.SetBool("IsPlatform", isPlatform); // Thiết lập animation cho nhân vật trong trạng thái nhảy
            CreateAudio("jump_small"); // Tạo ra âm thanh nhảy
        }

        if (rgRigidbody2D.velocity.y < 0) // Xử lý rơi xuống (sau khi nhảy)
        {
            // Thêm vận tốc để nhân vật rơi xuống nhanh hơn
            rgRigidbody2D.velocity += Vector2.up * (Physics2D.gravity.y * falling * Time.deltaTime);
        }
	else if (rgRigidbody2D.velocity.y > 0 && !up)
        {
            rgRigidbody2D.velocity += Vector2.up * (Physics2D.gravity.y * lowJumping * Time.deltaTime);
onclickJump = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D col) // Hàm xử lý va chạm với nền tảng
    {
        if (col.CompareTag("Platform")) // Nếu đối tượng va chạm là nền tảng
            isPlatform = true; // CHo là nhân vật đang đứng trên nền tảng
    }

    // Change size of Mario
    IEnumerator MarioEatMushroom() // Hàm xử lý biến hình
    {
        float timeDelay = 0.1f; // Thời gian biến hình
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 1);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 1);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);

        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);
    }

    IEnumerator MarioEatFlower()
    {
        float timeDelay = 0.1f;
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 1);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 1);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);

        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 1);
        yield return new WaitForSeconds(timeDelay);
    }

    IEnumerator MarioSmall()
    {
        float timeDelay = 0.1f;
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 1);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 1);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 1);
        yield return new WaitForSeconds(timeDelay);

        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioSmall"), 1);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioBig"), 0);
        playerAnim.SetLayerWeight(playerAnim.GetLayerIndex("MarioEatFlower"), 0);
        yield return new WaitForSeconds(timeDelay);
    }

    void CreateAudio(String name) // Hàm tạo âm thanh
    {
        // Gọi âm thanh 1 lần trong audio folder
        _audioSource.PlayOneShot(Resources.Load<AudioClip>("Audio/" + name));
    }

    public void MarioChet() // Hàm xử lý nhân vt chết
    {
        ViTriChet = transform.position; // Thiết lập vị trí chết bằng vị trí hiện tại
        GameObject Mariochet = (GameObject)Instantiate(Resources.Load("Prefabs/Mariochet")); // Lấy ra hình ảnh player chết
        Mariochet.transform.position = ViTriChet; // Thiết lập vị trí hình ảnh bằng vị trí nhân vật chết
        Destroy(gameObject); // Phá hủy player không cho di chuyển
    }

    public void Up()
    {
        up = true;
    }

    public void LeftUp()
    {
        left = false;
    }

    public void RightUp()
    {
        right = false;
    }
    
    public void UpUp()
    {
        up = false;
    }

    public void Left()
    {
        left = true;
    }

    public void Right()
    {
        right = true;
    }

public void OnclickJump()
{
	onclickJump  = true;
}
}