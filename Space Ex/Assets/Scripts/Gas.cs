using UnityEngine;
using UnityEngine.Networking;

public class Gas : NetworkBehaviour
{

    public LayerMask Door;// Дверь
    public LayerMask gas;// Смешивается
    public LayerMask Ground;// Не смешивается
    public int N;// Концентрация газа

    bool right = true;
    bool down = true;
    bool LR = false;

    const float ts = 1;
    float t = 0;

    SpriteRenderer sp;

    [SyncVar] float x;
    [SyncVar] float y;

    SV_Vars SVars;

    void Awake()
    {
        SVars = GameObject.FindGameObjectWithTag("SVars").GetComponent<SV_Vars>();

        sp = GetComponentInChildren<SpriteRenderer>();

        if (Random.Range(0, 2) == 0) right = false;
        if (Random.Range(0, 2) == 0) down = false;
        if (Random.Range(0, 2) == 0) LR = true;
    }

    void Update()
    {
        if (sp.enabled != SVars.Oxygen) sp.enabled = SVars.Oxygen;

        if (isServer)
        {
            x = transform.position.x;
            y = transform.position.y;

            if (transform.position.x > 200 || transform.position.y > 200) NetworkServer.Destroy(gameObject);
        }
        else
        {
            transform.position = new Vector2(x, y);
        }

        if (!isServer) return;

        if (t > 0) { t -= Time.fixedDeltaTime * 10; return; }

        if (!LR)
        {
            if (down)
            {
                Collider2D coll = Physics2D.OverlapPoint(new Vector3(transform.position.x, transform.position.y - 1, 0), Door);
                if (((coll && coll.isTrigger) || !coll) && !Physics2D.OverlapPoint(new Vector3(transform.position.x, transform.position.y - 1, 0), Ground) && Physics2D.OverlapPointAll(new Vector3(transform.position.x, transform.position.y - 1, 0), gas).Length < N)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
                    t = ts;
                }
                else down = false;
            }
            else
            {
                Collider2D coll = Physics2D.OverlapPoint(new Vector3(transform.position.x, transform.position.y + 1, 0), Door);
                if (((coll && coll.isTrigger) || !coll) && !Physics2D.OverlapPoint(new Vector3(transform.position.x, transform.position.y + 1, 0), Ground) && Physics2D.OverlapPointAll(new Vector3(transform.position.x, transform.position.y + 1, 0), gas).Length < N)
                {
                    transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
                    t = ts;
                }
                else down = true;
            }

            LR = !LR;
        }
        else
        {
            if (right)
            {
                Collider2D coll = Physics2D.OverlapPoint(new Vector3(transform.position.x + 1, transform.position.y, 0), Door);
                if (((coll && coll.isTrigger) || !coll) && !Physics2D.OverlapPoint(new Vector3(transform.position.x + 1, transform.position.y, 0), Ground) && Physics2D.OverlapPointAll(new Vector3(transform.position.x + 1, transform.position.y, 0), gas).Length < N)
                {
                    transform.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
                    t = ts;
                }
                else right = false;
            }
            else
            {
                Collider2D coll = Physics2D.OverlapPoint(new Vector3(transform.position.x - 1, transform.position.y, 0), Door);
                if (((coll && coll.isTrigger) || !coll) && !Physics2D.OverlapPoint(new Vector3(transform.position.x - 1, transform.position.y, 0), Ground) && Physics2D.OverlapPointAll(new Vector3(transform.position.x - 1, transform.position.y, 0), gas).Length < N)
                {
                    transform.position = new Vector3(transform.position.x - 1, transform.position.y, 0);
                    t = ts;
                }
                else right = true;
            }

            LR = !LR;
        }
    }
}