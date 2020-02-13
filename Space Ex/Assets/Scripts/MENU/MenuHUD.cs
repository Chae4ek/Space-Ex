using UnityEngine;
using UnityEngine.Networking;
using System;
using UnityEngine.SceneManagement;

public class MenuHUD : NetworkManager
{

    Camera cam;
    public BoxCollider2D join;
    public BoxCollider2D play;
    public BoxCollider2D exit;
    public BoxCollider2D cancel;
    public BoxCollider2D ip;
    public BoxCollider2D port;
    public BoxCollider2D Continue;
    public BoxCollider2D disconnect;
    public BoxCollider2D close;
    public BoxCollider2D name1;
    public SpriteRenderer spJoin;
    public SpriteRenderer spPlay;
    public SpriteRenderer spExit;
    public SpriteRenderer spLoad;
    public SpriteRenderer spCreate;
    public SpriteRenderer spCancel;
    public SpriteRenderer spIp;
    public SpriteRenderer spPort;
    public SpriteRenderer spContinue;
    public SpriteRenderer spDisconnect;
    public SpriteRenderer spClose;
    public SpriteRenderer spName;
    public Sprite sp1_0;// join
    public Sprite sp1_1;
    public Sprite sp2_0;// play
    public Sprite sp2_1;
    public Sprite sp3_0;// exit
    public Sprite sp3_1;
    public Sprite sp4_0;// cancel
    public Sprite sp4_1;
    public Sprite sp5_0;// ip
    public Sprite sp5_1;
    public Sprite sp6_0;// port
    public Sprite sp6_1;
    public Sprite sp7_0;// Continue
    public Sprite sp7_1;
    public Sprite sp8_0;// disconnect
    public Sprite sp8_1;
    public Sprite sp10_0;// close
    public Sprite sp10_1;
    public Sprite sp11_0;// name
    public Sprite sp11_1;

    bool Ip = false;
    bool Port = false;
    bool Name = false;
    public TextMesh IP;
    public TextMesh PORT;
    public TextMesh NAME;

    bool connect = false;
    bool loading = false;
    bool Menu = false;
    
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Menu" && Camera.main)
        {
            transform.parent = Camera.main.transform;
            transform.localPosition = Vector3.zero;
        }
        else transform.position = new Vector3(0, 0, -10);
        
        Vector2 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        bool down = Input.GetMouseButton(0);
        bool up = Input.GetMouseButtonUp(0);

        if (connect && !loading && Input.GetKeyDown(KeyCode.Escape)) Menu = !Menu;
        if (NetworkServer.active || NetworkClient.active) connect = true; else connect = false;
        if (connect && SceneManager.GetActiveScene().name == "Menu") loading = true; else loading = false;

        // =====================================================================================================================================

        if (!loading) { spCancel.enabled = false; spCreate.enabled = false; spLoad.enabled = false; }
        if (connect && (!Menu || loading)) { spExit.enabled = false; }
        if (!Menu || loading || !connect) { spContinue.enabled = false; spDisconnect.enabled = false; }

        /// Управление
        if (connect)
        {
            spIp.enabled = false;
            spPort.enabled = false;
            spName.enabled = false;
            spJoin.enabled = false;
            spPlay.enabled = false;
            IP.GetComponent<Renderer>().enabled = false;
            PORT.GetComponent<Renderer>().enabled = false;
            NAME.GetComponent<Renderer>().enabled = false;

            // Меню подключения
            if (loading)
            {
                spCancel.enabled = true;

                /// "Подключение"
                if (NetworkServer.active) { spLoad.enabled = false; spCreate.enabled = true; }
                else { spCreate.enabled = false; spLoad.enabled = true; }
                /// Отмена
                if (down && cancel.OverlapPoint(pos)) spCancel.sprite = sp4_1; else spCancel.sprite = sp4_0;
                if (up && cancel.OverlapPoint(pos)) singleton.StopHost();
            }
            // Игровое меню
            else if (Menu)
            {
                spContinue.enabled = true;
                spDisconnect.enabled = true;
                spExit.enabled = true;

                /// Кнопки
                if (down && Continue.OverlapPoint(pos)) spContinue.sprite = sp7_1; else spContinue.sprite = sp7_0;
                if (up && Continue.OverlapPoint(pos)) Menu = !Menu;
                if (down && disconnect.OverlapPoint(pos)) spDisconnect.sprite = sp8_1; else spDisconnect.sprite = sp8_0;
                if (up && disconnect.OverlapPoint(pos)) Disconnect();
                if (down && exit.OverlapPoint(pos)) spExit.sprite = sp3_1; else spExit.sprite = sp3_0;
                if (up && exit.OverlapPoint(pos)) Application.Quit();
            }
        }
        // Главное меню
        else
        {
            // spClose.enabled = false;
            spIp.enabled = true;
            spPort.enabled = true;
            spName.enabled = true;
            spJoin.enabled = true;
            spPlay.enabled = true;
            spExit.enabled = true;
            IP.GetComponent<Renderer>().enabled = true;
            PORT.GetComponent<Renderer>().enabled = true;
            NAME.GetComponent<Renderer>().enabled = true;

            /// IP и PORT
            if (ip.OverlapPoint(pos)) spIp.sprite = sp5_1; else if (!Ip) spIp.sprite = sp5_0;
            if (port.OverlapPoint(pos)) spPort.sprite = sp6_1; else if (!Port) spPort.sprite = sp6_0;
            if (name1.OverlapPoint(pos)) spName.sprite = sp11_1; else if (!Name) spName.sprite = sp11_0;
            if (down)
            {
                if (ip.OverlapPoint(pos)) { Port = false; Name = false; Ip = true; }
                else
                if (port.OverlapPoint(pos)) { Name = false; Ip = false; Port = true; }
                else
                if (name1.OverlapPoint(pos)) { Ip = false; Port = false; Name = true; }
                else
                { Ip = false; Port = false; Name = false; }
            }
            if (Ip) InputText(IP, 0);
            if (Port) InputText(PORT, 1);
            if (Name) InputText(NAME, 2);

            /// Кнопки
            if (down && join.OverlapPoint(pos)) spJoin.sprite = sp1_1; else spJoin.sprite = sp1_0;
            if (up && join.OverlapPoint(pos)) JoinGame();
            if (down && play.OverlapPoint(pos)) spPlay.sprite = sp2_1; else spPlay.sprite = sp2_0;
            if (up && play.OverlapPoint(pos)) StartupHost();
            if (down && exit.OverlapPoint(pos)) spExit.sprite = sp3_1; else spExit.sprite = sp3_0;
            if (up && exit.OverlapPoint(pos)) Application.Quit();
        }
        // =====================================================================================================================================
    }

    void InputText(TextMesh txt, int select)
    {
        string input = Input.inputString;
        for (int i = 0; i < input.Length; i++)
        {
            /// IP
            if (select == 0 && txt.text.Length < 15 && ((input[i] >= 48 && input[i] <= 57) || input[i] == 46)) txt.text += input[i];
            /// PORT
            if (select == 1 && txt.text.Length < 4 && input[i] >= 48 && input[i] <= 57) txt.text += input[i];
            /// NAME
            if (select == 2 && txt.text.Length < 10 && input[i] >= 32 && input[i] <= 126) txt.text += input[i];

            if (input[i] == 8)
            {
                string st = "";
                for (int j = 0; j < txt.text.Length - 1; j++) st += txt.text[j];
                txt.text = st;
            }
        }
    }

    void StartupHost()
    {
        singleton.networkAddress = IP.text;
        singleton.networkPort = Convert.ToInt16(PORT.text);
        singleton.StartHost();
    }

    void JoinGame()
    {
        singleton.networkAddress = IP.text;
        singleton.networkPort = Convert.ToInt16(PORT.text);
        singleton.StartClient();
    }

    void Disconnect()
    {
        Menu = false;
        singleton.StopHost();
    }

    void OnApplicationQuit()
    {
        if (connect) Disconnect();
    }
}