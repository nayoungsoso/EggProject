using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{
    public Text timeText;
    public float speed;
    float time;
    float h,jump,jumpPower=0;
    bool parable = false;
    bool die = false;
    int hp = 100;
    int maxHp = 100;
    Rigidbody2D rigid;
    SpriteRenderer spriteRenderer;
    Animator anim;

    float timer = 0;

    // Start is called before the first frame update
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
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
                Burning(collision.transform.position);
            if (collision.gameObject.tag == "Tornado") // 게임오브젝트랑 충돌한 태그가 Storm일 때
                FlyAway(collision.transform.position);
        }

    }

    void Burning(Vector2 targetPos)
    {
        timer += Time.deltaTime;
        // 5초 지속시 사망
        if (timer > 5)
        {
            print("burn");
            OnDie();
            hp = 0;
            timer = 0;
        }
        time = 5;
        gameObject.layer = 11; // 플레이어의 레이어가 11번레이어로 변경됨


        spriteRenderer.color = new Color(1, 1, 1, 0.4f); // 뜨거울 때 투명도조절

    }

    // 충돌이벤트
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!die)
        {
            if (collision.gameObject.tag == "Trap") // 게임오브젝트랑 충돌한 태그가 Trap일 때
                OnTrapDamaged(collision.transform.position);
            if (collision.gameObject.tag == "Flatform") // 게임 오브젝트랑 충돌한 태그가 Flatform일 때
            {
                print(rigid.velocity.magnitude);
                if (rigid.velocity.magnitude > 1f)
                {
                    OnHitDamaged(10);
                }
                else if (rigid.velocity.magnitude > 0.7f)
                {
                    OnHitDamaged(7);
                }
                else if (rigid.velocity.magnitude > 0.4f)
                {
                    OnHitDamaged(4);
                }
            }
            if (collision.gameObject.tag == "Web") // 게임오브젝트랑 충돌한 태그가 Web일 때
                Web(collision.transform.position);
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
        rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);

        Invoke("OffDamaged", 1);// 무적시간

        GameManager.instance.OnDamage(5);
    }

    void FlyAway(Vector2 targetPos)
    {
        gameObject.layer = 11; // 플레이어의 레이어가 11번레이어로 변경됨

        //토네이도를 밟을 때
        int ranx = Random.Range(-1, 1);
        int rany = Random.Range(1, 3);
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
        int dirc = transform.position.x - targetPos.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(dirc, 1) * 3, ForceMode2D.Impulse);

    }

    public void OnDie()
    {
        die = true;
        spriteRenderer.color = new Color(1, 1, 1, 0.4f);
    }

    void para()
    {
        if (parable)
        {
            rigid.gravityScale -= Time.deltaTime * 0.5f;
        }
        else
        {
            rigid.gravityScale = 1;
        }
    }
}
