using UnityEngine;

public class Block : MonoBehaviour
{

    void Awake()
    {
        SpriteRenderer sp = GetComponent<SpriteRenderer>();

        int r = Random.Range(0, 4);
        if (r == 0) sp.flipX = true;
        if (r == 1) sp.flipY = true;
        if (r == 2) { sp.flipX = true; sp.flipY = true; }

        Destroy(this);
    }
}