using UnityEngine;
using UnityEngine.Networking;

public class SS_Crate : NetworkBehaviour
{

    [SyncVar] public bool withSS;

    SpriteRenderer sp;
    public Sprite sp0;
    public Sprite sp1;

    void Awake()
    {
        sp = GetComponentInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (withSS && sp.sprite != sp0) sp.sprite = sp0;
        if (!withSS && sp.sprite != sp1) sp.sprite = sp1;
    }
}