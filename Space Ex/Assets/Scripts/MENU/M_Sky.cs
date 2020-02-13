using UnityEngine;

public class M_Sky : MonoBehaviour
{

    public int chunkWidth;
    public int chunkHeight;
    public int StarRand;
    public GameObject star;

    void Awake()
    {
        for (int x = -chunkWidth / 2; x < chunkWidth / 2; x++)
        {
            for (int y = -chunkHeight / 2; y < chunkHeight / 2; y++)
            {
                if (Random.Range(0, StarRand) == 0)
                {
                    // 1 2 3 4 5
                    int rd = Random.Range(0, 5);
                    Transform BG = Camera.main.transform.GetChild(0);
                    for (int i = 0; i < rd; i++)
                    {
                        BG = BG.GetChild(0);
                    }
                    float r = Random.Range(0.5f, 1.9f);

                    GameObject st = Instantiate(star, new Vector3(100, 100, 0), Quaternion.identity);
                    st.transform.parent = BG;
                    st.transform.localScale = new Vector3(r, r, r);
                    st.transform.localPosition = new Vector3(x, y, 0);
                }
            }
        }
        Camera.main.GetComponent<Cam>().cam = true;
        GameObject.FindGameObjectWithTag("Sky").GetComponent<M_Cam>().cam = true;
    }
}