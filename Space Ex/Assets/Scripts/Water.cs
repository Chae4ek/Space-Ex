using UnityEngine;

public class Water : MonoBehaviour
{

    public LayerMask water;// Смешивается
    public LayerMask Ground;// Не смешивается
    public int N;// Концентрация воды

    bool left = false;

    const float ts = 1;
    float t = 0;
    
    void Update()
    {
        if (t > 0) { t -= Time.fixedDeltaTime * 10; return; }
        
        if (!Physics2D.OverlapPoint(new Vector3(transform.position.x, transform.position.y - 1, 0), Ground) && Physics2D.OverlapPointAll(new Vector3(transform.position.x, transform.position.y - 1, 0), water).Length < N)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
            t = ts;
        }
        else
        {
            if (left)
            {
                if (!Physics2D.OverlapPoint(new Vector3(transform.position.x - 1, transform.position.y, 0), Ground) && Physics2D.OverlapPointAll(new Vector3(transform.position.x - 1, transform.position.y, 0), water).Length < N)
                {
                    transform.position = new Vector3(transform.position.x - 1, transform.position.y, 0);
                    t = ts;
                }
                else left = false;
            }
            else
            {
                if (!Physics2D.OverlapPoint(new Vector3(transform.position.x + 1, transform.position.y, 0), Ground) && Physics2D.OverlapPointAll(new Vector3(transform.position.x + 1, transform.position.y, 0), water).Length < N)
                {
                    transform.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
                    t = ts;
                }
                else left = true;
            }
        }




        /*
        if (!Physics2D.OverlapPoint(new Vector3(transform.position.x, transform.position.y - 1, 0), Ground) && Physics2D.OverlapPointAll(new Vector3(transform.position.x, transform.position.y - 1, 0), Oxy).Length < S)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 1, 0);
        }
        else
        if (!Physics2D.OverlapPoint(new Vector3(transform.position.x + 1, transform.position.y, 0), Ground) && Physics2D.OverlapPointAll(new Vector3(transform.position.x + 1, transform.position.y, 0), Oxy).Length < S)
        {
            transform.position = new Vector3(transform.position.x + 1, transform.position.y, 0);
        }
        else
        if (!Physics2D.OverlapPoint(new Vector3(transform.position.x - 1, transform.position.y, 0), Ground) && Physics2D.OverlapPointAll(new Vector3(transform.position.x - 1, transform.position.y, 0), Oxy).Length < S)
        {
            transform.position = new Vector3(transform.position.x - 1, transform.position.y, 0);
        }
        else
        if (!Physics2D.OverlapPoint(new Vector3(transform.position.x, transform.position.y + 1, 0), Ground) && Physics2D.OverlapPointAll(new Vector3(transform.position.x, transform.position.y + 1, 0), Oxy).Length < S)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 1, 0);
        }
        */
    }
}