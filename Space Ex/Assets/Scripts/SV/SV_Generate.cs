using UnityEngine;
using UnityEngine.Networking;

public class SV_Generate : NetworkBehaviour
{
    
    public bool GenCave;
    public bool GenStars;

    public int StarRand;

    public GameObject star;

    [SyncVar] float seed;// Сид

    public float rand;// Сглаживание
    public float smooth;

    public int chunkWidth;// Ширина чанка
    public int chunkHeight;// Высота чанка

    public GameObject oxygen;

    public GameObject dirt;
    public GameObject stone;
    public GameObject stone_1;
    public GameObject obsidian;

    void Start()
    {
        if (isServer) seed = Random.Range(-10000f, 10000f);// Генерация сида

        if (GenCave) GenerateCave(seed);// Генерация пещер
        if (GenStars) GenerateStars();// Генерация звез
    }

    void GenerateCave(float seed)
    {
        // int[,] a = new int[chunkWidth, chunkHeight];

        for (int x = 0; x < chunkWidth; x++)
        {
            for (int y = 0; y < chunkHeight; y++)
            {
                float Noise = Mathf.PerlinNoise(x / smooth + seed, y / smooth + seed);

                // if (Noise < rand) a[x, y] = 1; else a[x, y] = 0;
                if (Noise < rand)
                {
                    GameObject selectB = dirt;

                    if (Random.Range(0, 3) == 0) selectB = stone;
                    if (Random.Range(0, 8) == 0) selectB = stone_1;
                    if (Random.Range(0, 20) == 0) selectB = obsidian;

                    Instantiate(selectB, new Vector3(x, y, 0), Quaternion.identity);
                }
                else
                {
                    if (Random.Range(0, 4) == 0)
                    {
                        Instantiate(oxygen, new Vector3(x, y, 0), Quaternion.identity);
                    }
                }
            }
        }
    }

    void GenerateStars()
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

                    GameObject st = Instantiate(star, new Vector3(10000, 10000, 0), Quaternion.identity);
                    st.transform.parent = BG;
                    st.transform.localScale = new Vector3(r, r, r);
                    st.transform.localPosition = new Vector3(x, y, 0);
                }
            }
        }
    }
}