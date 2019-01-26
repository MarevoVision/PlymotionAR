using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;

public class ImportObjLITE : MonoBehaviour {

    public string ObjModel;
    public string Texture;

    public WWW w;
    public Texture2D tex;

    public bool TextureMirrorX;
    public bool TextureMirrorY;
    static int xx;
    static int yy;

    static int kol;
    static int kol1;

    static string[] lol;

    static string[] sas;

    static string[] kek;

    static Vector3[] vert;
    static int[] tri;

    static List<Vector3> ver = new List<Vector3>();
    static List<int> triang = new List<int>();
    static List<Vector2> uv = new List<Vector2>();
    static List<string> huh = new List<string>();
    static List<string> huh1 = new List<string>();

    static Vector2[] uvn;

    static char[] loh = new char[2];


    // Update is called once per frame
    void Start() {
        

        loh[0] = '/';
        loh[1] = ' ';

        MeshFilter m_f = GetComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        m_f.mesh = mesh;


        StreamReader st = new StreamReader(ObjModel);
        while (!st.EndOfStream)
        {
            var govno = st.ReadLine();

            if (govno.StartsWith("v "))
            {
                lol = govno.Substring(2).Split(' ');
                ver.Add(new Vector3(float.Parse(lol[0]), float.Parse(lol[1]), float.Parse(lol[2])));
            }
            if (govno.StartsWith("vt "))
            {
                kek = govno.Substring(3).Split(' ');

                uv.Add(new Vector2(float.Parse(kek[0]), float.Parse(kek[1])));
            }
            uvn = uv.ToArray();

            if (govno.StartsWith("f "))
            {
                sas = govno.Substring(2).Split(loh);
                huh.Add(sas[0]);
                huh.Add(sas[2]);
                huh.Add(sas[4]);

                huh1.Add(sas[1]);
                huh1.Add(sas[3]);
                huh1.Add(sas[5]);

                triang.Add(Convert.ToInt32(sas[0]) - 1);
                triang.Add(Convert.ToInt32(sas[2]) - 1);
                triang.Add(Convert.ToInt32(sas[4]) - 1);
            }
        }
        kol = huh.Count;

        for (int i = 0; i <= (kol - 1); i++)
        {
            uvn[Convert.ToInt32(huh[i]) - 1] = uv[Convert.ToInt32(huh1[i]) - 1];
        }
        mesh.vertices = ver.ToArray();
        mesh.triangles = triang.ToArray();
        mesh.uv = uvn;

        string k = @"\";
        char[] l = k.ToCharArray();
        string p = Texture.Replace(l[0], '/');
        w = new WWW("file://" + p);
        tex = w.texture;

        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = tex;
    }
    void Update()
    {
        if (TextureMirrorX == true)
        {
            xx = 1;
        }
        else
        {
            xx = -1;
        }
        if (TextureMirrorY == true)
        {
            yy = -1;
        }
        else
        {
            yy = 1;
        }
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTextureScale = new Vector2(xx, yy);
    }

}

