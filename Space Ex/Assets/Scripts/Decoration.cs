using UnityEngine;

public class Decoration : MonoBehaviour
{

    public int rand;

    void Awake()
    {
        if (Random.Range(0, rand) == 0)
        {
            SpriteRenderer sp = GetComponent<SpriteRenderer>();
            if (Random.Range(0, 2) == 0) sp.flipX = true;
            sp.enabled = true;

            Destroy(this);
        }
        else Destroy(gameObject);
    }
}