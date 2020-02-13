using UnityEngine;
using UnityEngine.Networking;

public class Door : NetworkBehaviour
{

    [SyncVar] public bool open = false;

    Create_Del del;
    SpriteRenderer sp;
    BoxCollider2D coll;

    void Awake()
    {
        del = GetComponent<Create_Del>();
        sp = GetComponentInChildren<SpriteRenderer>();
        coll = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (del.deleting) return;

        if (open)
        {
            sp.color = new Color(1, 1, 1, 0.15f);
            coll.isTrigger = true;
        }
        else
        {
            sp.color = new Color(1, 1, 1, 1);
            coll.isTrigger = false;
        }
    }
}