using UnityEngine;

public class M_Cam : MonoBehaviour
{

    public bool cam;

    void Update()
    {
        transform.position = Vector3.zero;
        if (!cam) return;

        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (pos.x - transform.position.x > 20) pos.x = 20 + transform.position.x;
        if (pos.x - transform.position.x < -20) pos.x = -20 + transform.position.x;
        if (pos.y - transform.position.y > 10) pos.y = 10 + transform.position.y;
        if (pos.y - transform.position.y < -10) pos.y = -10 + transform.position.y;

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, pos, Time.fixedDeltaTime * 2);

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(transform.position.x, transform.position.y, 0), Time.fixedDeltaTime * 8);
    }
}