using UnityEngine;

public class SV_Vars : MonoBehaviour
{

    // Спавн блоков
    public LayerMask groundBLOCK;

    // Для игрока
    public bool Oxygen;
    public bool AA;
    public bool Stats;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z)) AA = !AA;
        if (Input.GetKeyDown(KeyCode.X)) Oxygen = !Oxygen;
        if (Input.GetKeyDown(KeyCode.C)) Stats = !Stats;
    }
}