using UnityEngine;

public class Cam : MonoBehaviour
{

    public bool cam;
    public float speed;
    public Transform BG1;
    public Transform BG2;
    public Transform BG3;
    public Transform BG4;
    public Transform BG5;

    void Update()
    {
        if (!cam) return;

        BG1.position -= new Vector3(Camera.main.velocity.x * speed, Camera.main.velocity.y * speed, 0);
        BG2.position -= new Vector3(Camera.main.velocity.x * speed, Camera.main.velocity.y * speed, 0);
        BG3.position -= new Vector3(Camera.main.velocity.x * speed, Camera.main.velocity.y * speed, 0);
        BG4.position -= new Vector3(Camera.main.velocity.x * speed, Camera.main.velocity.y * speed, 0);
        BG5.position -= new Vector3(Camera.main.velocity.x * speed, Camera.main.velocity.y * speed, 0);
    }
}