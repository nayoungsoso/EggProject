using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public Text timeText;
    public float speed;
    public bool burn = false;

    float time;
    float h,jump,jumpPower=0;
    bool parable = false;
    bool die = false;
    int hp = 100;
    int maxHp = 100;

    public AudioClip jumpSound;
    public AudioClip riseSound;
    public AudioClip breakSound;
    public AudioClip[] hitSound = new AudioClip[2];
    public AudioClip deadSound;
    public AudioClip flapSound;
    public AudioClip webSound;
    public AudioClip burnSound;

    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;
    AudioSource audioSource;

    float timer = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!die)
        {
            if (Input.GetKey(KeyCode.Z))
            {
                Vector2 mPosition = Input.mousePosition; // 마우스 포지션
                Vector2 oPosition = transform.position; // 캐릭터 포지션

                Vector2 target = Camera.main.ScreenToWorldPoint(mPosition); // 마우스의 위치가 바뀔때마다 좌표갱신

                // 각 축의 거리를 계산하여 저장
                float dy = target.y - oPosition.y;
                float dx = target.x - oPosition.x;

                // 축의 세타값을 구해줌
                float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

                // 세타값에 따른 이동방향
                if (rotateDegree < -90 || rotateDegree > 90)
                {
                    h = -0.1f;
                }
                else if ((rotateDegree < 90 && rotateDegree > 0) || (rotateDegree > -90 && rotateDegree < 0))
                {
                    h = 0.1f;
                }
                // 이동
                rigid.AddForce(Vector2.right * h * speed, ForceMode2D.Impulse);
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                if (parable)
                {
                    GameObject.FindWithTag("Parachute").GetComponent<Renderer>().enabled = false;
                    parable = false;
                }
                else
                {
                    GameObject.FindWithTag("Parachute").GetComponent<Renderer>().enabled = true;
                    parable = true;
                }
            }
            para();

            if (time > 0)
            {
                time -= Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Fire") // 게임오브젝트랑 충돌한 태그가 Fire일 때
        {
            audioSource.Stop();
        }

    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!die)
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (jumpPower < 1.5f)
                { jumpPower += 0.05f; }
            }
            else
            {
                Vector2 mPosition = Input.mousePosition; // 마우스 포지션
                Vector2 oPosition = transform.position; // 캐릭터 포지션

                Vector2 target = Camera.main.ScreenToWorldPoint(mPosition); // 마우스의 위치가 바뀔때마다 좌표갱신

                // 각 축의 거리를 계싼하여 저장
                float dy = target.y - oPosition.y;
                float dx = target.x - oPosition.x;

                // 축의 세타값을 구해줌
                float rotateDegree = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

                // 세타값에 따른 이동방향
                if (rotateDegree > 90)
                {
                    jump = (180 - rotateDegree) * 0.1f;
                    h = (rotateDegree - 90) * -0.1f;
                }
                else if (rotateDegree > 0)
                {
                    jump = rotateDegree * 0.1f;
                    h = (90 - rotateDegree) * 0.1f;
                }
                else if (rotateDegree > -90)
                {
                    jump = rotateDegree * 0.1f;
                    h = (90 + rotateDegree) * 0.1f;
                }
                else
                {
                    jump = (-180 - rotateDegree) * 0.1f;
                    h = rotateDegree * 0.1f;
                }
                // 이동
                //PlaySound("JUMP");
                rigid.AddForce(Vector2.right * h * jumpPower, ForceMode2D.Impulse);
                rigid.AddForce(Vector2.up * jump * jumpPower, ForceMode2D.Impulse);
                jumpPower = 0;
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!die)
        {
            if (collision.gameObject.tag == "Fire") // 게임오브젝트랑 충돌한 태그가 Fire일 때
            {
                PlaySound("BURN");
                Burning(collision.transform.position);
            }
            if (collision.gameObject.tag == "Tornado") // 게임오브젝트랑 충돌한 태그가 Storm일 때
            {
                PlaySound("RISE");
                FlyAway(collision.transform.position);
            }
            if (collision.gameObject.tag == "Web") // 게임오브젝트랑 충돌한 태그가 Web일 때
            {
                PlaySound("WEB");
                Web(collision.transform.position);
            }
        }

    }

    void Burning(Vector2 targetPos)
    {
        timer += Time.deltaTime;
        // 5초 지속시 사망
        if (timer > 5)
        {
            burn = true;
            GameManager.instance.OnDamage(100);
            hp = 0;
            timer = 0;
        }
        time = 5;
        gameObject.layer = 11; // 플레이어의 레이어가 11번레이어로 변경됨


        spriteRenderer.color = new Color(1, 0, 0, 0.4f); // 뜨거울 때 투명도조절

    }

    // 충돌이벤트
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!die)
        {
            if (collision.gameObject.tag == "Trap") // 게임오브젝트랑 충돌한 태그가 Trap일 때
            {
                PlaySound("BREAK");
                OnTrapDamaged(collision.transform.position);
            }
            if (collision.gameObject.tag == "Platform") // 게임 오브젝트랑 충돌한 태그가 Flatform일 때
            {
                // 속도에 따른 충돌 데미지
                if (rigid.velocity.magnitude > 3.4f)
                {
                    PlaySound("HIT");
                    OnHitDamaged(7);
                }
                else if (rigid.velocity.magnitude > 2.4f)
                {
                    PlaySound("HIT");
                    OnHitDamaged(6);
                }
                else if (rigid.velocity.magnitude > 1.4f)
                {
                    PlaySound("HIT");
                    OnHitDamaged(5);
                }
            }
            else
            {
                Invoke("OffDamaged", 1);
                time = -1;
            }
        }
    }

    void OnTrapDamaged(Vector2 targetPos)
    {
        gameObject.layer = 11; // 플레이어의 레이어가 11번레이어로 변경됨


        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // 가시를 밟을 때 투명도조절

        //가시 밟을 때
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 2, ForceMode2D.Impulse);

        Invoke("OffDamaged", 1);// 무적시간

        GameManager.instance.OnDamage(5);
    }

    void FlyAway(Vector2 targetPos)
    {
        gameObject.layer = 11; // 플레이어의 레이어가 11번레이어로 변경됨

        

        //토네이도를 밟을 때
        float ranx = Random.Range(-0.1f, 0.1f);
        float rany = Random.Range(0.1f, 0.5f);
        rigid.AddForce(new Vector2(ranx, rany), ForceMode2D.Impulse);

    }

    //무적상태 헤제
    void OffDamaged()
    {
        gameObject.layer = 10;
        spriteRenderer.color = new Color(1, 1, 1, 1);
    }

    // 부딧쳤을 때
    void OnHitDamaged(float damage)
    {
        GameManager.instance.OnDamage(damage);
    }

    void Web(Vector2 targetPos)
    {
        //거미줄에 튕김
        int dirc = transform.position.x - targetPos.x > 0 ? -1 : 1;
        rigid.AddForce(new Vector2(dirc, 1) * 1, ForceMode2D.Impulse);

    }

    public void OnDie()
    {
        PlaySound("DEAD");
        gameObject.SetActive(false);
        die = true;
        burn = false;
    }

    void para()
    {
        if (parable)
        {
            PlaySound("FLAP");
            rigid.gravityScale -= Time.deltaTime * 0.5f;
        }
        else
        {
            rigid.gravityScale = 1;
        }
    }
    
    void PlaySound(string action)
    {
        switch (action)
        {
            case "JUMP":
                audioSource.clip = jumpSound;
                break;
            case "RISE":
                audioSource.clip = riseSound;
                break;
            case "FLAP":
                audioSource.clip = flapSound;
                break;
            case "BURN":
                audioSource.clip = burnSound;
                break;
            case "DEAD":
                audioSource.clip = deadSound;
                break;
            case "WEB":
                audioSource.clip = webSound;
                break;
            case "BREAK":
                audioSource.clip = breakSound;
                break;
            case "HIT":
                int i = Random.Range(0, 3);
                audioSource.clip = hitSound[i];
                break;
        }
        audioSource.Play();

    }
}
