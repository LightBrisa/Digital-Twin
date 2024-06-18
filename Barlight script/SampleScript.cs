using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System;

public class SampleScript : MonoBehaviour
{
    [Serializable]
    public class World
    {
        public string room;
        public float[] R;
        public float[] G;
        public float[] B;
        public World(int maxObject)
        {
            R = new float[maxObject];
            G = new float[maxObject];
            B = new float[maxObject];
        }
    }

    public int index = 0;

    public int i = 0;

    public int maxObject;

    static public string curRoom = "room5-white";

    public System.Random random = new System.Random();

    public ArrayList worlds = new ArrayList();

    public ArrayList groups = GroupController.groups;

    public void read(World w)
    {
        GameObject home = GameObject.Find(w.room);
        maxObject = home.transform.childCount;
        int i = 0;
        foreach (Transform o in home.transform)
        {
            Color c = new Color(w.R[i], w.G[i], w.B[i]);
            o.GetComponent<MeshRenderer>().material.color = c;
            i++;
        }
    }

    public void read1()
    {
        int[] indices = { 78, 120, 142, 153, 176 };
        int index = indices[random.Next(indices.Length)];
        Debug.Log(index);
        FileStream fs = new FileStream("peise/1/" + "groupedsample" + index + ".txt", FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        World w = (World)bf.Deserialize(fs);
        fs.Close();
        read(w);
    }

    public void read2()
    {
        int[] indices = { 7, 43, 73, 127, 162 };
        int index = indices[random.Next(indices.Length)];
        Debug.Log(index);
        FileStream fs = new FileStream("peise/2/" + "groupedsample" + index + ".txt", FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        World w = (World)bf.Deserialize(fs);
        fs.Close();
        read(w);
    }

    public void read3()
    {
        int[] indices = { 23, 27, 54, 109, 130 };
        int index = indices[random.Next(indices.Length)];
        Debug.Log(index);
        FileStream fs = new FileStream("peise/3/" + "groupedsample" + index + ".txt", FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        World w = (World)bf.Deserialize(fs);
        fs.Close();
        read(w);
    }

    public void read4()
    {
        int[] indices = { 19, 61, 99, 124, 180 };
        int index = indices[random.Next(indices.Length)];
        Debug.Log(index);
        FileStream fs = new FileStream("peise/4/" + "groupedsample" + index + ".txt", FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        World w = (World)bf.Deserialize(fs);
        fs.Close();
        read(w);
    }

    public void read()
    {
        FileStream fs = new FileStream("sample" + i + ".txt", FileMode.Open);
        BinaryFormatter bf = new BinaryFormatter();
        World w = (World)bf.Deserialize(fs);
        fs.Close();
        read(w);
        if (++i >= index)
            i = 0;
    }

    public void write(Texture2D t)
    {
        byte[] bytes = t.EncodeToPNG();
        System.IO.File.WriteAllBytes("groupedsample" + index + ".png", bytes);
    }

    public void write(World w)
    {
        FileStream fs = new FileStream("groupedsample" + index + ".txt", FileMode.Create);
        BinaryFormatter bf = new BinaryFormatter();
        bf.Serialize(fs, w);
        fs.Close();
    }

    public void write()
    {
        RandomColor();
        GameObject home = GameObject.Find(curRoom);
        maxObject = home.transform.childCount;

        World w = new World(maxObject);
        int i = 0;
        foreach (Transform o in home.transform)
        {
            Color c = o.GetComponent<MeshRenderer>().material.color;
            w.room = curRoom;
            w.R[i] = c.r;
            w.G[i] = c.g;
            w.B[i] = c.b;
            i++;
        }
        write(w);
        write(CubeForTest(new Vector3(1, 1, 1)));
        index++;
    }

    private Texture2D CubeForTest(Vector3 pos, int pixel = 512)
    {
        Vector3 setCameraPosition = new Vector3(-6.8f, 1.5f, -9.15f);
        //create a cubemap camera
        GameObject go = new GameObject("CubeCamera", typeof(Camera));
        go.transform.position = setCameraPosition;
        go.transform.rotation = Quaternion.identity;

        //remove layer8,9,10 which is layer for marker and line
        go.GetComponent<Camera>().cullingMask = ~((1 << 8) | (1 << 9) | (1 << 10));
        Cubemap cubemap = new Cubemap(pixel, TextureFormat.ARGB32, false);
        go.GetComponent<Camera>().RenderToCubemap(cubemap);//render to cubemap
        Destroy(go);

        Texture2D texture = new Texture2D(cubemap.width, cubemap.height, TextureFormat.ARGB32, false);
        CubemapFace offcialFaces = (CubemapFace)4;

        for (int i = 0; i < cubemap.width; i++)
        {
            for (int j = 0; j < cubemap.height; j++)
            {
                texture.SetPixel(i, j, cubemap.GetPixel(offcialFaces, cubemap.width - i, cubemap.height - j));
            }
        }
        texture.Apply();
        return texture;
    }

    private void RandomColor()
    {
        System.Random Rnd = new System.Random();
        for (int i = 0; i < groups.Count; i++)
        {
            ArrayList group = (ArrayList)groups[i];
            float r = (float)Rnd.Next(1, 256) / 256;
            float g = (float)Rnd.Next(1, 256) / 256;
            float b = (float)Rnd.Next(1, 256) / 256;
            foreach (Transform o in group)
            {
                o.GetComponent<MeshRenderer>().material.color = new Color(r, g, b);
            }
        }
    }

    public void s()
    {
        for (int i = 1; i <= 200; i++)
            write();
    }
}