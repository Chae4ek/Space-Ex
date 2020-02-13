using UnityEngine;
using UnityEngine.Networking;

public class PL_Active : NetworkBehaviour
{

    // Ломание блоков
    public GameObject oxygen;
    public GameObject carbon;

    public LayerMask destroyALL;
    public LayerMask desGLASS;
    public LayerMask destroy;
    Collider2D coll2 = null;
    SpriteRenderer s;

    // Door
    public LayerMask door;

    // Oxygen_Gen
    public LayerMask oxy_gen;
    public LayerMask oxy;
    public int N;

    // Station
    public LayerMask stantion;

    // Food_Gen
    public LayerMask food_gen;

    // SS_Crate
    public LayerMask ss_crate;



    TextMesh info;

    Transform AA;
    BoxCollider2D AAcoll;
    Collider2D coll;

    SV_Vars SVars;
    SpriteRenderer[] sp = new SpriteRenderer[9];
    PL_Stat stats;
    PL_Inv inv;
    PL_SL plsl;
    PL_Move move;

    void Awake()
    {
        SVars = GameObject.FindGameObjectWithTag("SVars").GetComponent<SV_Vars>();
        stats = GetComponent<PL_Stat>();
        inv = GetComponent<PL_Inv>();
        plsl = GetComponent<PL_SL>();
        move = GetComponent<PL_Move>();

        info = transform.GetChild(5).GetChild(4).GetComponent<TextMesh>();

        AA = transform.GetChild(1);
        AAcoll = AA.GetComponent<BoxCollider2D>();

        sp[0] = transform.GetChild(1).GetComponent<SpriteRenderer>();
        for (int i = 0; i < 8; i++)
        {
            sp[i + 1] = sp[0].transform.GetChild(i).GetComponent<SpriteRenderer>();
        }
    }

    void Update()
    {
        sp[0].transform.position = new Vector2(Mathf.FloorToInt(transform.position.x) + 0.5f, Mathf.FloorToInt(transform.position.y) + 0.5f);

        if (sp[0].enabled != SVars.AA)
        {
            for (int i = 0; i < 9; i++)
            {
                sp[i].enabled = SVars.AA;
            }
        }

        if (!isLocalPlayer) return;



        if (stats.en < stats.MaxEN && plsl.inv[6] > 0 && Input.GetKeyDown(KeyCode.Q))
        {
            AddInv(6, -1);
            stats.CmdEN(2);
        }

        // Активные объекты
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Door
            Collider2D[] colls = Physics2D.OverlapBoxAll(AA.position, new Vector2(2.8f, 2.8f), 0, door);

            for (int i = 0; i < colls.Length; i++)
            {
                Door door = colls[i].GetComponent<Door>();

                CmdDoor(!door.open, door.gameObject);
            }

            // SS_Crate
            coll = Physics2D.OverlapBox(AA.position, new Vector2(2.8f, 2.8f), 0, ss_crate);

            if (coll)
            {
                SS_Crate ss = coll.GetComponent<SS_Crate>();

                if ((ss.withSS && !move.inSpacesuit) || (!ss.withSS && move.inSpacesuit)) CmdSS(ss.gameObject);
            }
        }

        bool active = false;

        /// Oxygen_Gen
        coll = Physics2D.OverlapBox(AA.position, new Vector2(2.8f, 2.8f), 0, oxy_gen);
        if (coll)
        {
            active = true;
            info.text = "ENERGY - 1\nМинералы - 1";

            if (Input.GetKeyDown(KeyCode.E) && stats.en > 0 && plsl.inv[0] > 0 && Physics2D.OverlapPointAll(coll.transform.position, oxy).Length < N)
            {
                AddInv(0, -1);
                stats.CmdEN(-1);
                CmdOxy(coll.transform.position);
            }
        }

        /// Station
        coll = Physics2D.OverlapBox(AA.position, new Vector2(2.8f, 2.8f), 0, stantion);
        if (coll)
        {
            if (inv.select == 1) { active = true; info.text = "Минералы - 2\nМеталл - 2\nПесок - 3"; }
            if (inv.select == 2) { active = true; info.text = "Минералы - 3\nМеталл - 3"; }
            if (inv.select == 6) { active = true; info.text = "Минералы - 20\nМеталл - 20\nУголь - 15\nГаз - 10"; }
            if (inv.select == 7) { active = true; info.text = "Минералы - 30\nМеталл - 20\nУголь - 20\nГаз - 15"; }
            if (inv.select == 8) { active = true; info.text = "Минералы - 2\nМеталл - 2"; }
            if (inv.select == 9) { active = true; info.text = "Минералы - 4\nМеталл - 4"; }
            if (inv.select == 10) { active = true; info.text = "Минералы - 3\nМеталл - 3"; }
            if (inv.select == 11) { active = true; info.text = "Минералы - 5\nМеталл - 5"; }
            if (inv.select == 12) { active = true; info.text = "Минералы - 40\nМеталл - 30\nПесок - 20\nНефть - 10\nУголь - 10\nГаз - 10"; }
            if (inv.select == 13) { active = true; info.text = "Минералы - 20\nМеталл - 20\nПесок - 30\nГаз - 20"; }

            if (Input.GetKeyDown(KeyCode.E))
            {
                /// Glass
                if (inv.select == 1 && plsl.inv[0] >= 2 && plsl.inv[1] >= 2 && plsl.inv[2] >= 3)
                {
                    AddInv(0, -2);
                    AddInv(1, -2);
                    AddInv(2, -3);
                    AddBlocks(inv.select, 1);
                }
                /// Iron
                if (inv.select == 2 && plsl.inv[0] >= 3 && plsl.inv[1] >= 3)
                {
                    AddInv(0, -3);
                    AddInv(1, -3);
                    AddBlocks(inv.select, 1);
                }
                /// Oxy_GEN
                if (inv.select == 6 && plsl.inv[0] >= 20 && plsl.inv[1] >= 20 && plsl.inv[4] >= 15 && plsl.inv[5] >= 10)
                {
                    AddInv(0, -20);
                    AddInv(1, -20);
                    AddInv(4, -15);
                    AddInv(5, -10);
                    AddBlocks(inv.select, 1);
                }
                /// Food_GEN
                if (inv.select == 7 && plsl.inv[0] >= 30 && plsl.inv[1] >= 20 && plsl.inv[4] >= 20 && plsl.inv[5] >= 15)
                {
                    AddInv(0, -30);
                    AddInv(1, -20);
                    AddInv(4, -20);
                    AddInv(5, -15);
                    AddBlocks(inv.select, 1);
                }
                /// Door_1
                if (inv.select == 8 && plsl.inv[0] >= 2 && plsl.inv[1] >= 2)
                {
                    AddInv(0, -2);
                    AddInv(1, -2);
                    AddBlocks(inv.select, 1);
                }
                /// LDoor_1
                if (inv.select == 9 && plsl.inv[0] >= 4 && plsl.inv[1] >= 4)
                {
                    AddInv(0, -4);
                    AddInv(1, -4);
                    AddBlocks(inv.select, 1);
                }
                /// Door_2
                if (inv.select == 10 && plsl.inv[0] >= 3 && plsl.inv[1] >= 3)
                {
                    AddInv(0, -3);
                    AddInv(1, -3);
                    AddBlocks(inv.select, 1);
                }
                /// LDoor_2
                if (inv.select == 11 && plsl.inv[0] >= 5 && plsl.inv[1] >= 5)
                {
                    AddInv(0, -5);
                    AddInv(1, -5);
                    AddBlocks(inv.select, 1);
                }
                /// Station
                if (inv.select == 12 && plsl.inv[0] >= 40 && plsl.inv[1] >= 30 && plsl.inv[2] >= 20 && plsl.inv[3] >= 10 && plsl.inv[4] >= 10 && plsl.inv[5] >= 10)
                {
                    AddInv(0, -40);
                    AddInv(1, -30);
                    AddInv(2, -20);
                    AddInv(3, -10);
                    AddInv(4, -10);
                    AddInv(5, -10);
                    AddBlocks(inv.select, 1);
                }
                /// SS_Crate
                if (inv.select == 13 && plsl.inv[0] >= 20 && plsl.inv[1] >= 20 && plsl.inv[2] >= 30 && plsl.inv[5] >= 20)
                {
                    AddInv(0, -20);
                    AddInv(1, -20);
                    AddInv(2, -30);
                    AddInv(5, -20);
                    AddBlocks(inv.select, 1);
                }
            }
        }

        /// Food_Gen
        coll = Physics2D.OverlapBox(AA.position, new Vector2(2.8f, 2.8f), 0, food_gen);
        if (coll)
        {
            active = true;
            info.text = "Минералы - 3\nУголь - 1";

            if (Input.GetKeyDown(KeyCode.E) && plsl.inv[0] >= 3 && plsl.inv[4] > 0)
            {
                AddInv(0, -3);
                AddInv(4, -1);
                AddInv(6, 1);
            }
        }

        if (!active) info.text = "";



        // Ломание блоков
        if (stats.en > 0 && Input.GetMouseButton(0))
        {
            coll = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), destroy);
            if (!coll) coll = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition), desGLASS);
            /// coll - Объект для уничтожения

            /// При перескоке на другой блок
            if (coll2 && coll2 != coll)
            {
                coll2.GetComponent<Create_Del>().deleting = false;
                s.color = new Color(1, 1, 1);
                coll2 = null;
            }

            /// Выбор этого блока
            if (coll && !coll.CompareTag("Obsidian"))
            {
                coll.GetComponent<Create_Del>().deleting = true;
                s = coll.transform.GetChild(0).GetComponent<SpriteRenderer>();
                coll2 = coll;
            }

            if (coll2)
            {
                s.color = new Color(Mathf.Lerp(s.color.r, 0, Time.fixedDeltaTime), Mathf.Lerp(s.color.g, 0, Time.fixedDeltaTime), Mathf.Lerp(s.color.b, 0, Time.fixedDeltaTime));

                if (s.color.r < 0.15f && coll2 && !coll2.GetComponent<Create_Del>().deleted)
                {
                    coll2.GetComponent<Create_Del>().deleted = true;
                    stats.CmdEN(-1);

                    if (coll2.CompareTag("Stone")) { AddInv(0, 2); AddInv(1, 1); }
                    if (coll2.CompareTag("Stone_1")) { AddInv(0, 1); AddInv(5, 1); CmdOxy(coll2.transform.position); }
                    if (coll2.CompareTag("Stone_2")) { AddInv(0, 1); AddInv(2, 1); CmdCar(coll2.transform.position); }
                    if (coll2.CompareTag("Iron")) AddInv(1, 2);
                    if (coll2.CompareTag("Dirt")) { AddInv(0, 2); AddInv(2, 2); AddInv(6, 1); }
                    if (coll2.CompareTag("Glass")) { AddInv(1, 1); AddInv(2, 2); }
                    if (coll2.CompareTag("Door_1")) { AddInv(0, 1); AddInv(1, 1); }
                    if (coll2.CompareTag("Door_2")) { AddInv(0, 2); AddInv(1, 2); }
                    if (coll2.CompareTag("LDoor_1")) { AddInv(0, 2); AddInv(1, 2); }
                    if (coll2.CompareTag("LDoor_2")) { AddInv(0, 3); AddInv(1, 3); }
                    if (coll2.CompareTag("Oxygen_Gen")) { AddInv(0, 5); AddInv(1, 5); AddInv(4, 1); AddInv(5, 1); }
                    if (coll2.CompareTag("Food_Gen")) { AddInv(0, 5); AddInv(1, 5); AddInv(4, 2); AddInv(5, 1); }
                    if (coll2.CompareTag("Station")) { AddInv(0, 10); AddInv(1, 8); AddInv(2, 5); AddInv(3, 1); AddInv(4, 1); AddInv(5, 1); }
                    if (coll2.CompareTag("Spacesuit_Crate")) { AddInv(0, 8); AddInv(1, 8); AddInv(2, 5); AddInv(5, 2); }

                    CmdDel(coll2.gameObject);
                }
            }
        }
        else
        {
            if (coll2)
            {
                coll2.GetComponent<Create_Del>().deleting = false;
                s.color = new Color(1, 1, 1);
                coll2 = null;
            }
        }

        // Установка блоков
        if (Input.GetMouseButton(1) && plsl.blocks[inv.select] > 0)
        {
            Vector3 pos = new Vector3(Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x) + 0.5f, Mathf.FloorToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y) + 0.5f, 0);
            int z = 0;
            bool place = true;



            /// Можно ставить блоки только рядом с другими
            if (!Physics2D.OverlapPoint(new Vector3(pos.x - 1, pos.y + 1, 0), destroyALL)
                && !Physics2D.OverlapPoint(new Vector3(pos.x, pos.y + 1, 0), destroyALL)
                && !Physics2D.OverlapPoint(new Vector3(pos.x + 1, pos.y + 1, 0), destroyALL)
                && !Physics2D.OverlapPoint(new Vector3(pos.x - 1, pos.y, 0), destroyALL)
                && !Physics2D.OverlapPoint(new Vector3(pos.x + 1, pos.y, 0), destroyALL)
                && !Physics2D.OverlapPoint(new Vector3(pos.x - 1, pos.y - 1, 0), destroyALL)
                && !Physics2D.OverlapPoint(new Vector3(pos.x, pos.y - 1, 0), destroyALL)
                && !Physics2D.OverlapPoint(new Vector3(pos.x + 1, pos.y - 1, 0), destroyALL)) place = false;



            /// Двойные двери
            if (place && (inv.select == 9 || inv.select == 11))
            {
                bool left = Physics2D.OverlapPoint(new Vector3(pos.x - 1, pos.y, 0), destroy);
                bool right = Physics2D.OverlapPoint(new Vector3(pos.x + 1, pos.y, 0), destroy);
                bool up = Physics2D.OverlapPoint(new Vector3(pos.x, pos.y + 1, 0), destroy);
                bool down = Physics2D.OverlapPoint(new Vector3(pos.x, pos.y - 1, 0), destroy);

                if (left && !right)
                {
                    pos.x += 0.5f;
                    z = 90;
                }
                else
                if (right && !left)
                {
                    pos.x -= 0.5f;
                    z = 90;
                }
                else
                if (up && !down)
                {
                    pos.y -= 0.5f;
                    z = 0;
                }
                else
                if (down && !up)
                {
                    pos.y += 0.5f;
                    z = 0;
                }
                else place = false;
            }

            if (place && (inv.select == 8 || inv.select == 10))
            {
                if (Physics2D.OverlapPoint(new Vector3(pos.x - 1, pos.y, 0), destroy) || Physics2D.OverlapPoint(new Vector3(pos.x + 1, pos.y, 0), destroy))
                {
                    z = 90;
                }
                else
                if (Physics2D.OverlapPoint(new Vector3(pos.x, pos.y + 1, 0), destroy) || Physics2D.OverlapPoint(new Vector3(pos.x, pos.y - 1, 0), destroy))
                {
                    z = 0;
                }
                else place = false;
            }


            
            if (place && AAcoll.OverlapPoint(pos) && ((inv.select != 1 && !Physics2D.OverlapPoint(pos, destroy)) || (inv.select == 1 && !Physics2D.OverlapPoint(pos, desGLASS))))
            {
                AddBlocks(inv.select, -1);
                GameObject b = Instantiate(inv.sps[inv.select], pos, Quaternion.Euler(0, 0, z));
                b.tag = "TEMP";
                CmdSpawn(inv.select, pos, z);
            }
        }
    }

    void AddInv(int select, int i)
    {
        plsl.inv[select] += i;
        CmdUI(gameObject, select, plsl.inv[select]);
        
    }
    void AddBlocks(int select, int i)
    {
        plsl.blocks[select] += i;
        CmdUB(gameObject, select, plsl.blocks[select]);
    }
    [Command]
    void CmdUI(GameObject pl, int select, int i)
    {
        pl.GetComponent<PL_SL>().inv[select] = i;
    }
    [Command]
    void CmdUB(GameObject pl, int select, int i)
    {
        pl.GetComponent<PL_SL>().blocks[select] = i;
    }

    [Command]
    void CmdSS(GameObject s)
    {
        SS_Crate ss = s.GetComponent<SS_Crate>();

        if ((ss.withSS && !move.inSpacesuit) || (!ss.withSS && move.inSpacesuit))
        {
            move.inSpacesuit = !move.inSpacesuit;
            ss.withSS = !ss.withSS;
        }
    }

    [Command]
    void CmdSpawn(int select, Vector3 pos, int z)
    {
        if ((select != 1 && Physics2D.OverlapPoint(transform.position, SVars.groundBLOCK)) || (select == 1 && Physics2D.OverlapPoint(transform.position, desGLASS)))
        {
            TargetSpawnError(connectionToClient, select, 1);
        }
        else
        {
            NetworkServer.Spawn(Instantiate(inv.sps[select], pos, Quaternion.Euler(0, 0, z)));
            TargetSpawnError(connectionToClient, select, 0);
        }
    }
    [TargetRpc]
    void TargetSpawnError(NetworkConnection target, int select, int i)
    {
        GameObject[] b = GameObject.FindGameObjectsWithTag("TEMP");
        foreach (GameObject t in b) Destroy(t);
        AddBlocks(select, i);
    }

    [Command]
    void CmdCar(Vector3 pos)
    {
        NetworkServer.Spawn(Instantiate(carbon, pos, Quaternion.identity));
    }
    [Command]
    void CmdOxy(Vector3 pos)
    {
        NetworkServer.Spawn(Instantiate(oxygen, pos, Quaternion.identity));
    }

    [Command]
    void CmdDel(GameObject obj)
    {
        if (obj) NetworkServer.Destroy(obj);
    }

    [Command]
    void CmdDoor(bool open, GameObject door)
    {
        door.GetComponent<Door>().open = open;
    }
}