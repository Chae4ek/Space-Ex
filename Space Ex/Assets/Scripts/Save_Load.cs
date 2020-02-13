using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using System;

public class Save_Load : NetworkBehaviour
{

    string path = "/Saves";
    string file = "/save.txt";
    string filePlayers = "/save-P.txt";

    GameObject[] Blocks;
    FileStream save;
    BinaryFormatter form;
    public List<GameObject> BlockForSpawn;
    GameObject[] Players;

    void Update()
    {
        if (!isServer) return;

        if (Input.GetKeyDown(KeyCode.P)) Save();
        if (Input.GetKeyDown(KeyCode.L)) Load();
    }

    [Serializable]
    class Position
    {
        public int count = 0;
        public List<float> x = new List<float>();
        public List<float> y = new List<float>();
        public List<float> rotation = new List<float>();

        public void AddItem(Vector3 pos, Quaternion angle)
        {
            x.Add(pos.x);
            y.Add(pos.y);
            rotation.Add(angle.eulerAngles.z);
        }
    }

    [Serializable]
    class SpecPos : Position
    {
        public List<bool> special = new List<bool>();
    }
    
    [Serializable]
    class PlayerPos : Position
    {
        public List<string> name = new List<string>();
        public List<int[]> inv = new List<int[]>();
        public List<int[]> blocks = new List<int[]>();

        public List<float> hpEXP = new List<float>();
        public List<float> hpEXPmax = new List<float>();
        public List<float> hp = new List<float>();
        public List<float> MaxHP = new List<float>();
        public List<float> oxEXP = new List<float>();
        public List<float> oxEXPmax = new List<float>();
        public List<float> ox = new List<float>();
        public List<float> MaxOX = new List<float>();
        public List<float> enEXP = new List<float>();
        public List<float> enEXPmax = new List<float>();
        public List<float> en = new List<float>();
        public List<float> MaxEN = new List<float>();

        public List<bool> inSS = new List<bool>();

        public void AddPlayer(string Name, Vector3 pos, Quaternion angle, int[] inventory, int[] blocksI, bool SS, float b1, float b2, float b3, float b4, float b5, float b6, float b7, float b8, float b9, float b10, float b11, float b12)
        {
            name.Add(Name);
            AddItem(pos, angle);
            inv.Add(inventory);
            blocks.Add(blocksI);
            ++count;
            hpEXP.Add(b1);
            hpEXPmax.Add(b2);
            hp.Add(b3);
            MaxHP.Add(b4);
            oxEXP.Add(b5);
            oxEXPmax.Add(b6);
            ox.Add(b7);
            MaxOX.Add(b8);
            enEXP.Add(b9);
            enEXPmax.Add(b10);
            en.Add(b11);
            MaxEN.Add(b12);
            inSS.Add(SS);
        }
    }

    void Save()
    {
        if (!Directory.Exists(Application.dataPath + path)) Directory.CreateDirectory(Application.dataPath + path);

        save = new FileStream(Application.dataPath + path + filePlayers, FileMode.Create);
        form = new BinaryFormatter();

        Players = GameObject.FindGameObjectsWithTag("Player");
        PlayerPos posPl;
        try
        {
            posPl = (PlayerPos)form.Deserialize(save);
        }
        catch
        {
            posPl = new PlayerPos();
        }
        bool onServer;
        PL_SL plsl;
        PL_Inv inv;
        PL_Stat stat;
        bool SS;

        /// Если игрок есть в сохраненке, то обновляем, иначе добавляем
        for (int j = 0; j < Players.Length; ++j)
        {
            plsl = Players[j].GetComponent<PL_SL>();
            stat = Players[j].GetComponent<PL_Stat>();
            SS = Players[j].GetComponent<PL_Move>().inSpacesuit;
            onServer = false;

            for (int i = 0; i < posPl.count; ++i)
            {
                if (posPl.name[i] == plsl.Name)
                {
                    posPl.x[i] = Players[j].transform.position.x;
                    posPl.y[i] = Players[j].transform.position.y;
                    posPl.rotation[i] = Players[j].transform.rotation.eulerAngles.z;
                    posPl.inv[i] = plsl.inv;
                    posPl.blocks[i] = plsl.blocks;

                    posPl.hpEXP[i] = stat.hpEXP;
                    posPl.hpEXPmax[i] = stat.hpEXPmax;
                    posPl.hp[i] = stat.hp;
                    posPl.MaxHP[i] = stat.MaxHP;
                    posPl.oxEXP[i] = stat.oxEXP;
                    posPl.oxEXPmax[i] = stat.oxEXPmax;
                    posPl.ox[i] = stat.ox;
                    posPl.MaxOX[i] = stat.MaxOX;
                    posPl.enEXP[i] = stat.enEXP;
                    posPl.enEXPmax[i] = stat.enEXPmax;
                    posPl.en[i] = stat.en;
                    posPl.MaxEN[i] = stat.MaxEN;

                    posPl.inSS[i] = SS;

                    onServer = true;
                    break;
                }
            }

            if (!onServer) posPl.AddPlayer(plsl.Name, Players[j].transform.position, Players[j].transform.rotation, plsl.inv, plsl.blocks, SS, stat.hpEXP, stat.hpEXPmax, stat.hp, stat.MaxHP, stat.oxEXP, stat.oxEXPmax, stat.ox, stat.MaxOX, stat.enEXP, stat.enEXPmax, stat.en, stat.MaxEN);
        }

        form.Serialize(save, posPl);
        save.Close();
        


        save = new FileStream(Application.dataPath + path + file, FileMode.Create);
        form = new BinaryFormatter();

        SaveB("Dirt");
        SaveB("Glass");
        SaveB("Iron");
        SaveB("Obsidian");
        SaveB("Stone");
        SaveB("Stone_1");
        SaveB("Stone_2");
        SaveSpec("Door_1", true);
        SaveSpec("LDoor_1", true);
        SaveSpec("Door_2", true);
        SaveSpec("LDoor_2", true);
        SaveB("Food_Gen");
        SaveB("Oxygen_Gen");
        SaveSpec("Spacesuit_Crate", false);
        SaveB("Station");
        SaveB("Carbon");
        SaveB("Oxygen");

        save.Close();
    }

    void SaveB(string block)
    {
        Blocks = GameObject.FindGameObjectsWithTag(block);
        Position pos = new Position { count = Blocks.Length };
        foreach (GameObject obj in Blocks) pos.AddItem(obj.transform.position, obj.transform.rotation);
        form.Serialize(save, pos);
    }

    void SaveSpec(string block, bool door)
    {
        Blocks = GameObject.FindGameObjectsWithTag(block);
        SpecPos pos = new SpecPos { count = Blocks.Length };
        foreach (GameObject obj in Blocks)
        {
            pos.AddItem(obj.transform.position, obj.transform.rotation);
            if (door) pos.special.Add(obj.GetComponent<Door>().open); else pos.special.Add(obj.GetComponent<SS_Crate>().withSS);
        }
        form.Serialize(save, pos);
    }

    void Load()
    {
        if (!File.Exists(Application.dataPath + path + file)) return;

        if (File.Exists(Application.dataPath + path + filePlayers))
        {
            save = new FileStream(Application.dataPath + path + filePlayers, FileMode.Open);
            form = new BinaryFormatter();

            Players = GameObject.FindGameObjectsWithTag("Player");
            PlayerPos posPl = (PlayerPos)form.Deserialize(save);
            PL_SL plsl;

            for (int j = 0; j < Players.Length; ++j)
            {
                plsl = Players[j].GetComponent<PL_SL>();

                for (int i = 0; i < posPl.count; ++i)
                {
                    if (posPl.name[i] == plsl.Name)
                    {
                        for (int m = 0; m < plsl.inv.Length; m++) plsl.inv[m] = posPl.inv[i][m];
                        for (int m = 0; m < plsl.blocks.Length; m++) plsl.blocks[m] = posPl.blocks[i][m];
                        RpcPlayer(Players[j], posPl.inv[i], posPl.blocks[i], posPl.x[i], posPl.y[i], posPl.rotation[i], posPl.inSS[i], posPl.hpEXP[i], posPl.hpEXPmax[i], posPl.hp[i], posPl.MaxHP[i], posPl.oxEXP[i], posPl.oxEXPmax[i], posPl.ox[i], posPl.MaxOX[i], posPl.enEXP[i], posPl.enEXPmax[i], posPl.en[i], posPl.MaxEN[i]);
                        break;
                    }
                }
            }

            save.Close();
        }

        save = new FileStream(Application.dataPath + path + file, FileMode.Open);
        form = new BinaryFormatter();

        LoadB("Dirt", 0);
        LoadB("Glass", 1);
        LoadB("Iron", 2);
        LoadB("Obsidian", 3);
        LoadB("Stone", 4);
        LoadB("Stone_1", 5);
        LoadB("Stone_2", 6);
        LoadSpec("Door_1", 7, true);
        LoadSpec("LDoor_1", 8, true);
        LoadSpec("Door_2", 9, true);
        LoadSpec("LDoor_2", 10, true);
        LoadB("Food_Gen", 11);
        LoadB("Oxygen_Gen", 12);
        LoadSpec("Spacesuit_Crate", 13, false);
        LoadB("Station", 14);
        LoadB("Carbon", 15);
        LoadB("Oxygen", 16);

        save.Close();
    }

    void LoadB(string block, int blockInd)
    {
        Blocks = GameObject.FindGameObjectsWithTag(block);
        foreach (GameObject obj in Blocks) NetworkServer.Destroy(obj);

        Position pos = (Position)form.Deserialize(save);

        for (int i = 0; i < pos.count; i++)
        {
            GameObject b = Instantiate(BlockForSpawn[blockInd], new Vector3(pos.x[i], pos.y[i], 0), Quaternion.Euler(0, 0, pos.rotation[i]));
            NetworkServer.Spawn(b);
        }
    }

    void LoadSpec(string block, int blockInd, bool door)
    {
        Blocks = GameObject.FindGameObjectsWithTag(block);
        foreach (GameObject obj in Blocks) NetworkServer.Destroy(obj);

        SpecPos pos = (SpecPos)form.Deserialize(save);

        for (int i = 0; i < pos.count; i++)
        {
            GameObject b = Instantiate(BlockForSpawn[blockInd], new Vector3(pos.x[i], pos.y[i], 0), Quaternion.Euler(0, 0, pos.rotation[i]));
            if (door) b.GetComponent<Door>().open = pos.special[i]; else b.GetComponent<SS_Crate>().withSS = pos.special[i];
            NetworkServer.Spawn(b);
        }
    }

    public void PlayerConnect(GameObject pl, string Name, NetworkConnection target)
    {
        Players = GameObject.FindGameObjectsWithTag("Player");

        foreach (GameObject player in Players)
        {
            if (Name == player.GetComponent<PL_SL>().Name && pl != player) { target.Disconnect(); return; }
        }

        // Возобновление игроков и их инвентаря по имени при подключении
        if (File.Exists(Application.dataPath + path + filePlayers))
        {
            save = new FileStream(Application.dataPath + path + filePlayers, FileMode.Open);
            form = new BinaryFormatter();

            PlayerPos posPl = (PlayerPos)form.Deserialize(save);
            PL_SL plsl = pl.GetComponent<PL_SL>();

            int i = 0;
            while (i < posPl.count && posPl.name[i] != plsl.Name) ++i;

            if (i != posPl.count)
            {
                for (int m = 0; m < plsl.inv.Length; m++) plsl.inv[m] = posPl.inv[i][m];
                for (int m = 0; m < plsl.blocks.Length; m++) plsl.blocks[m] = posPl.blocks[i][m];
                RpcPlayer(pl, posPl.inv[i], posPl.blocks[i], posPl.x[i], posPl.y[i], posPl.rotation[i], posPl.inSS[i], posPl.hpEXP[i], posPl.hpEXPmax[i], posPl.hp[i], posPl.MaxHP[i], posPl.oxEXP[i], posPl.oxEXPmax[i], posPl.ox[i], posPl.MaxOX[i], posPl.enEXP[i], posPl.enEXPmax[i], posPl.en[i], posPl.MaxEN[i]);
            }

            save.Close();
        }
    }

    [ClientRpc]
    void RpcPlayer(GameObject pl, int[] inv, int[] blocks, float x, float y, float z, bool SS, float b1, float b2, float b3, float b4, float b5, float b6, float b7, float b8, float b9, float b10, float b11, float b12)
    {
        pl.GetComponent<PL_SL>().Pos(inv, blocks, x, y, z);
        PL_Stat stat = pl.GetComponent<PL_Stat>();
        stat.hpEXP = b1;
        stat.hpEXPmax = b2;
        stat.hp = b3;
        stat.MaxHP = b4;
        stat.oxEXP = b5;
        stat.oxEXPmax = b6;
        stat.ox = b7;
        stat.MaxOX = b8;
        stat.enEXP = b9;
        stat.enEXPmax = b10;
        stat.en = b11;
        stat.MaxEN = b12;
        pl.GetComponent<PL_Move>().inSpacesuit = SS;
    }
}