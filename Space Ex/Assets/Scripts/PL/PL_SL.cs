using UnityEngine;
using UnityEngine.Networking;

public class PL_SL : NetworkBehaviour
{

    public string Name;
    public int[] inv;
    public int[] blocks;

    void Start()
    {
        if (hasAuthority && !isServer)
        {
            Name = GameObject.FindGameObjectWithTag("MENU").transform.GetChild(14).GetComponent<TextMesh>().text;
            CmdConnect(gameObject, Name);
        }
    }

    [Command]
    void CmdConnect(GameObject pl, string Name)
    {
        pl.GetComponent<PL_SL>().Name = Name;
        GameObject.FindGameObjectWithTag("SaveLoad").GetComponent<Save_Load>().PlayerConnect(pl, Name, connectionToClient);
    }

    public void Pos(int[] INV, int[] BLOCKS, float x, float y, float z)
    {
        transform.position = new Vector3(x, y, 0);
        transform.rotation = Quaternion.Euler(0, 0, z);

        for (int i = 0; i < INV.Length; i++) inv[i] = INV[i];
        for (int i = 0; i < BLOCKS.Length; i++) blocks[i] = BLOCKS[i];
    }
}