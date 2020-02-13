using UnityEngine;
using UnityEngine.Networking;

public class PL_Move : NetworkBehaviour
{

    Rigidbody2D rb;
    SpriteRenderer sp;

    public LayerMask oxygen;
    public LayerMask carbon;

    public float speed;// Скорость игрока
    public float PowerJump;// Высота прыжка
    float PJ;
    public LayerMask glass;

    public LayerMask LayerGround;// Земля
    bool isGroundS = false;

    bool inSpace = false;// В космосе или нет
    bool isGroundW = false;

    [SyncVar] public bool inSpacesuit;

    void Awake()
    {
        transform.position = new Vector3(12.5f, 17.5f, 0);
        Camera.main.transform.position = new Vector3(12.5f, 17.5f, -10);

        rb = GetComponent<Rigidbody2D>();
        sp = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        Collider2D[] colls = Physics2D.OverlapBoxAll(transform.position + new Vector3(0, -0.47f, 0), new Vector2(0.65f, 0.08f), 0, LayerGround);
        bool isT = true;
        for (int i = 0; i < colls.Length; i++)
        {
            if (!colls[i].GetComponent<Collider2D>().isTrigger)
            {
                isT = false;
                break;
            }
        }
        isGroundS = !isT;

        if (!inSpace) { isGroundW = false; return; }

        colls = Physics2D.OverlapBoxAll(transform.position + new Vector3(0, 0.47f, 0), new Vector2(0.65f, 0.08f), 0, LayerGround);
        isT = true;
        for (int i = 0; i < colls.Length; i++)
        {
            if (!colls[i].GetComponent<Collider2D>().isTrigger)
            {
                isT = false;
                break;
            }
        }
        isGroundW = !isT;
    }

    void Update()
    {
        if (inSpacesuit) sp.enabled = true; else sp.enabled = false;

        if (!isLocalPlayer) return;

        // Управление
        bool W = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Space);
        bool A = Input.GetKey(KeyCode.A);
        bool S = Input.GetKey(KeyCode.S);
        bool D = Input.GetKey(KeyCode.D);

        // inSpace
        inSpace = !Physics2D.OverlapCircle(transform.position, 0.4f, glass);
        if (!inSpace) { rb.gravityScale = 3; PJ = PowerJump; } else { if (inSpacesuit) { rb.gravityScale = 1; PJ = PowerJump; } else { rb.gravityScale = 0; PJ = PowerJump / 1.6f; } }

        // x
        if (!inSpace || isGroundS || isGroundW || inSpacesuit)
        {
            if ((!A && !D) || (A && D)) rb.velocity = new Vector2(0, rb.velocity.y);
            else if (A) rb.velocity = new Vector2(-speed, rb.velocity.y);
            else if (D) rb.velocity = new Vector2(speed, rb.velocity.y);
        }

        // y
        if (isGroundS || isGroundW) rb.velocity = new Vector2(rb.velocity.x, 0);
        if (isGroundS && W) rb.AddForce(new Vector2(0, PJ), ForceMode2D.Impulse);
        if (isGroundW && S) rb.AddForce(new Vector2(0, -PJ), ForceMode2D.Impulse);
    }
}