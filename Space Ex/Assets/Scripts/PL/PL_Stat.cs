using UnityEngine;
using UnityEngine.Networking;
using System;

public class PL_Stat : NetworkBehaviour
{

    // Здоровье
    Transform HP;
    Transform HP_R;
    Transform HP2;
    Transform HP_R2;
    Bar health;
    public Transform EXP_HP;
    [SyncVar] public float hpEXP = 0;
    [SyncVar] public float hpEXPmax = 10;
    bool hpLU = false;

    [SyncVar] public float hp;
    [SyncVar] public float MaxHP;
    public TextMesh HPText;

    // Кислород
    Transform OX;
    Transform OX_R;
    Transform OX2;
    Transform OX_R2;
    Bar oxygen;
    public Transform EXP_OX;
    [SyncVar] public float oxEXP = 0;
    [SyncVar] public float oxEXPmax = 10;
    bool oxLU = false;

    [SyncVar] public float ox;
    [SyncVar] public float MaxOX;
    public TextMesh OXText;

    // Энергия
    Transform EN;
    Transform EN_R;
    Bar energy;
    public Transform EXP_EN;
    [SyncVar] public float enEXP = 0;
    [SyncVar] public float enEXPmax = 10;
    bool enLU = false;

    [SyncVar] public float en;
    [SyncVar] public float MaxEN;
    public TextMesh ENText;



    public float TOxy;
    public LayerMask oxy;
    public LayerMask carbon;
    float toxy;

    public float THP;
    float thp;

    SpriteRenderer[] sp = new SpriteRenderer[6];
    SV_Vars SVars;
    Transform stats;
    Transform stats2;
    Transform stats3;
    PL_SL plsl;
    PL_Move move;

    void Awake()
    {
        stats = transform.GetChild(3);
        stats2 = transform.GetChild(4);
        stats3 = transform.GetChild(5);

        plsl = GetComponent<PL_SL>();
        move = GetComponent<PL_Move>();

        SVars = GameObject.FindGameObjectWithTag("SVars").GetComponent<SV_Vars>();
        Transform s = transform.GetChild(2);
        for (int i = 0; i < s.childCount; i++)
        {
            sp[i] = s.GetChild(i).GetComponent<SpriteRenderer>();
        }

        toxy = TOxy;
        thp = THP;

        // Здоровье
        HP_R = s.GetChild(1).GetComponent<Transform>();
        HP = s.GetChild(2).GetComponent<Transform>();
        HP_R2 = stats.GetChild(1).GetComponent<Transform>();
        HP2 = stats.GetChild(2).GetComponent<Transform>();

        health = new Bar
        {
            P = hp,
            MaxP = MaxHP,
            PBoost = false
        };

        // Кислород
        OX_R = s.GetChild(4).GetComponent<Transform>();
        OX = s.GetChild(5).GetComponent<Transform>();
        OX_R2 = stats.GetChild(4).GetComponent<Transform>();
        OX2 = stats.GetChild(5).GetComponent<Transform>();

        oxygen = new Bar
        {
            P = ox,
            MaxP = MaxOX,
            PBoost = false
        };

        // Энергия
        EN_R = stats.GetChild(7).GetComponent<Transform>();
        EN = stats.GetChild(8).GetComponent<Transform>();

        energy = new Bar
        {
            P = en,
            MaxP = MaxEN,
            PBoost = false
        };
    }

    void Start()
    {
        if (!isLocalPlayer)
        {
            stats.gameObject.SetActive(false);
            stats2.gameObject.SetActive(false);
            stats3.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        // Здоровье
        if (hp > MaxHP) hp = MaxHP;
        if (hp < 0) hp = 0;

        BarUpdate(HP, HP_R, health, hp, MaxHP, 5);

        // Кислород
        if (ox > MaxOX) ox = MaxOX;
        if (ox < 0) ox = 0;

        BarUpdate(OX, OX_R, oxygen, ox, MaxOX, 5);



        // Энергия
        if (en > MaxEN) en = MaxEN;
        if (en < 0) en = 0;

        if (!isLocalPlayer) return;

        // СМЕРТЬ
        if (hp == 0)
        {
            for (int i = 0; i < plsl.inv.Length; i++) plsl.inv[i] = 0;
            for (int i = 0; i < plsl.blocks.Length; i++) plsl.blocks[i] = 0;
            CmdDeath();
            transform.position = new Vector3(12.5f, 17.5f, 0);
        }

        // Здоровье
        HP2.localScale = new Vector3(HP.localScale.x * 25 / 5, HP2.localScale.y, HP2.localScale.z);
        HP_R2.localScale = new Vector3(HP_R.localScale.x * 25 / 5, HP_R2.localScale.y, HP_R2.localScale.z);

        HPText.text = "HP								   " + Convert.ToString(hp) + " / " + Convert.ToString(MaxHP);

        // Кислород
        OX2.localScale = new Vector3(OX.localScale.x * 17 / 5, OX2.localScale.y, OX2.localScale.z);
        OX_R2.localScale = new Vector3(OX_R.localScale.x * 17 / 5, OX_R2.localScale.y, OX_R2.localScale.z);

        OXText.text = "OX									 " + Convert.ToString(ox) + " / " + Convert.ToString(MaxOX);

        // Энергия
        BarUpdate(EN, EN_R, energy, en, MaxEN, 17);
        ENText.text = "EN									 " + Convert.ToString(en) + " / " + Convert.ToString(MaxEN);

        // EXP_HP
        EXP_HP.localScale = new Vector3(hpEXP * 50 / hpEXPmax, EXP_HP.localScale.y, EXP_HP.localScale.z);

        // EXP_OX
        EXP_OX.localScale = new Vector3(oxEXP * 34 / oxEXPmax, EXP_OX.localScale.y, EXP_OX.localScale.z);

        // EXP_EN
        EXP_EN.localScale = new Vector3(enEXP * 34 / enEXPmax, EXP_EN.localScale.y, EXP_EN.localScale.z);



        if (SVars.Stats != sp[0].enabled)
        {
            for (int i = 0; i < sp.Length; i++)
            {
                sp[i].enabled = SVars.Stats;
            }
        }

        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height, 10));
        pos.x += 0.9f;
        stats.position = pos;
        


        // ox++
        if (ox < MaxOX)
        {
            Collider2D coll = Physics2D.OverlapCircle(transform.position, 0.45f, oxy);

            if (coll)
            {
                CmdDel(coll.gameObject);
                Destroy(coll.gameObject);
                CmdOX(1);
            }
        }

        // ox--
        if (toxy > 0) toxy -= Time.fixedDeltaTime * 10;
        if (toxy <= 0)
        {
            toxy = TOxy;
            CmdOX(-1);
        }
        
        // hp--
        if (ox == 0)
        {
            if (thp > 0) thp -= Time.fixedDeltaTime * 10;
            if (thp <= 0)
            {
                thp = THP;
                CmdHP(-1);
            }
        }
        else thp = THP;

        if (hp > 0)
        {
            Collider2D coll = Physics2D.OverlapCircle(transform.position, 0.45f, carbon);

            if (coll)
            {
                CmdDel(coll.gameObject);
                Destroy(coll.gameObject);
                CmdHP(-1);
            }
        }

        // hpEXP
        if (!hpLU && hpEXP >= hpEXPmax)
        {
            hpLU = true;
            CmdhpEXPmax(5);
            CmdMaxHP(2);
        }
        if (hpLU && hpEXP < hpEXPmax) hpLU = false;

        // oxEXP
        if (!oxLU && oxEXP >= oxEXPmax)
        {
            oxLU = true;
            CmdoxEXPmax(5);
            CmdMaxOX(2);
        }
        if (oxLU && oxEXP < oxEXPmax) oxLU = false;

        // enEXP
        if (!enLU && enEXP >= enEXPmax)
        {
            enLU = true;
            CmdenEXPmax(5);
            CmdMaxEN(2);
        }
        if (enLU && enEXP < enEXPmax) enLU = false;
    }

    [Command]
    void CmdDeath()
    {
        move.inSpacesuit = false;

        hp = 10;
        MaxHP = 10;
        hpEXP = 0;
        hpEXPmax = 10;

        ox = 10;
        MaxOX = 10;
        oxEXP = 0;
        oxEXPmax = 10;

        en = 10;
        MaxEN = 10;
        enEXP = 0;
        enEXPmax = 10;
    }

    [Command]
    void CmdDel(GameObject obj)
    {
        if (obj) NetworkServer.Destroy(obj);
    }


    [Command]
    void CmdhpEXPmax(float i)
    {
        hpEXPmax += i;
        hpEXP = 0;
    }
    [Command]
    void CmdoxEXPmax(float i)
    {
        oxEXPmax += i;
        oxEXP = 0;
    }
    [Command]
    void CmdenEXPmax(float i)
    {
        enEXPmax += i;
        enEXP = 0;
    }


    [Command]
    void CmdMaxHP(float i)
    {
        MaxHP += i;
    }
    [Command]
    void CmdMaxOX(float i)
    {
        MaxOX += i;
    }
    [Command]
    void CmdMaxEN(float i)
    {
        MaxEN += i;
    }


    [Command]
    void CmdHP(float i)
    {
        if (i < 0 && hp > 0) hpEXP += 1;
        hp += i;
    }
    [Command]
    void CmdOX(float i)
    {
        if (i < 0 && ox > 0) oxEXP += 1;
        ox += i;
    }
    [Command]
    public void CmdEN(float i)
    {
        if (i < 0 && en > 0) enEXP += 1;
        en += i;
    }

    class Bar
    {
        public float P;
        public float MaxP;
        public bool PBoost;
    }

    void BarUpdate(Transform L, Transform L_R, Bar b, float P, float MaxP, float Len)
    {
        /// Погрешность сглаживания
        if (L.localScale.x > L_R.localScale.x) L_R.localScale = L.localScale;
        if (L.localScale.x != L_R.localScale.x && (L_R.localScale.x - 0.05f < L.localScale.x || L.localScale.x + 0.05f > L_R.localScale.x))
            if (!b.PBoost) L_R.localScale = L.localScale;
            else L.localScale = L_R.localScale;

        /// Доп. P
        if (MaxP != b.MaxP)
        {
            b.MaxP = MaxP;
            L.localScale = new Vector3(P * Len / MaxP, L.localScale.y, L.localScale.z); L_R.localScale = new Vector3(P * Len / MaxP, L_R.localScale.y, L_R.localScale.z);
        }

        /// Прибавление или убывание P
        if (P < b.P) { b.PBoost = false; L.localScale = new Vector3(P * Len / MaxP, L.localScale.y, L.localScale.z); }
        if (P > b.P) { b.PBoost = true; L_R.localScale = new Vector3(P * Len / MaxP, L_R.localScale.y, L_R.localScale.z); }

        if (P != b.P) b.P = P;

        /// Сглаживание P
        if (!b.PBoost && L.localScale.x < L_R.localScale.x) L_R.localScale = Vector3.Lerp(L_R.localScale, L.localScale, Time.fixedDeltaTime * 2);
        if (b.PBoost && L.localScale.x < L_R.localScale.x) L.localScale = Vector3.Lerp(L.localScale, L_R.localScale, Time.fixedDeltaTime * 2);
    }
}