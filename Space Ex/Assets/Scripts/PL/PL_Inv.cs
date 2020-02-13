using UnityEngine;
using System;

public class PL_Inv : MonoBehaviour
{

    Transform s;
    TextMesh[] items = new TextMesh[7];

    PL_SL plsl;

    [HideInInspector] public int select;
    Transform b;
    SpriteRenderer sp;
    public GameObject[] sps;
    TextMesh count;
    TextMesh Name;

    void Awake()
    {
        b = transform.GetChild(5);
        sp = b.transform.GetChild(1).GetComponent<SpriteRenderer>();
        count = b.transform.GetChild(2).GetComponent<TextMesh>();
        Name = b.transform.GetChild(3).GetComponent<TextMesh>();

        s = transform.GetChild(4);

        for (int i = 0; i < 7; i++)
        {
            items[i] = s.GetChild(i).GetComponent<TextMesh>();
        }

        plsl = GetComponent<PL_SL>();

        while (plsl.blocks[select] == -1) select++;
    }

    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 8));
        s.position = pos;

        items[0].text = "Минералы - " + Convert.ToString(plsl.inv[0]);
        items[1].text = "Металл - " + Convert.ToString(plsl.inv[1]);
        items[2].text = "Песок - " + Convert.ToString(plsl.inv[2]);
        items[3].text = "Нефть - " + Convert.ToString(plsl.inv[3]);
        items[4].text = "Уголь - " + Convert.ToString(plsl.inv[4]);
        items[5].text = "Газ - " + Convert.ToString(plsl.inv[5]);
        items[6].text = "Еда - " + Convert.ToString(plsl.inv[6]);

        pos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10));
        b.position = pos;

        if (Input.GetAxis("Mouse ScrollWheel") < 0) Scroll(1);
        if (Input.GetAxis("Mouse ScrollWheel") > 0) Scroll(-1);

        sp.sprite = sps[select].transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;

        if (select == 0) Name.text = "Земля";
        if (select == 1) Name.text = "Стекло";
        if (select == 2) Name.text = "Металлический блок";
        if (select == 3) Name.text = "Камень";
        if (select == 4) Name.text = "Камень с кислородом";
        if (select == 5) Name.text = "Камень с углеродом";
        if (select == 6) Name.text = "Генератор кислорода";
        if (select == 7) Name.text = "Генератор еды";
        if (select == 8) Name.text = "Непрочная одиночная дверь";
        if (select == 9) Name.text = "Непрочная двойная дверь";
        if (select == 10) Name.text = "Прочная одиночная дверь";
        if (select == 11) Name.text = "Прочная двойная дверь";
        if (select == 12) Name.text = "Техническая станция";
        if (select == 13) Name.text = "Ящик со скафандром";

        count.text = Convert.ToString(plsl.blocks[select]);
    }

    void Scroll(int s)
    {
        int sel = select;
        sel += s;

        if (sel < plsl.blocks.Length && sel > -1)
        {
            while (sel < plsl.blocks.Length && sel > -1 && plsl.blocks[sel] == -1) sel += s;

            if (sel != -1 && sel != plsl.blocks.Length) select = sel;
        }
    }
}