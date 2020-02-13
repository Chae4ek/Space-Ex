using UnityEngine;

public class Star : MonoBehaviour
{

    SpriteRenderer sp;
    bool down = true;
    float r;

    void Awake()
    {
        sp = GetComponent<SpriteRenderer>();
        sp.color = new Color(1, 1, 1, Random.Range(0, 1f));
        if (Random.Range(0, 2) == 0) down = false;
        r = Random.Range(1.5f, 5);
    }

    void Update()
    {
        if (down)
        {
            if (sp.color.a > 0) sp.color -= new Color(0, 0, 0, Time.fixedDeltaTime / r);
            else down = false;
        }
        else
        {
            if (sp.color.a < 1) sp.color += new Color(0, 0, 0, Time.fixedDeltaTime / r);
            else down = true;
        }
    }
}