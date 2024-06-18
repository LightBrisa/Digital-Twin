using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Text;
using System.IO;
using UnityEditor;

public class GA : MonoBehaviour
{

    private bool init = false;


    public GameObject go;//物体的统称
    private Camera _camera;//获取相机

    public List<World> al = new List<World>();//使用一个list存储n种配色方案，list模板为World类
    List<World> best_color_every = new List<World>();//存储每一代最优方案
    List<float> best_energy_every = new List<float>();//存储每一代最优方案的能量值以观察收敛性
    List<Color> start = new List<Color>();//用以存储初始状态
    public List<String> object_name = new List<String>();//这个list用于动态获取场景内要进入算法的物体名称
    public List<float> energy = new List<float>();//存储每种配色方案的能量
    public List<float> probability = new List<float>();//用于存储每种配色方案在轮盘法中的概率

    int dai = 0;
    public float e;//能量值
    public int template;//模板索引
    float best = 0;//寻找最终结果
    int bestIndex = 0;//最终结果所对应的索引


    int bestIndex10 = 0, bestIndex20 = 0;
    List<string> name10;

    List<float> R10;

    List<float> G10;

    List<float> B10;

    List<string> name20;

    List<float> R20;

    List<float> G20;

    List<float> B20;

    int generation;
    int ge = 0;

    Color c;

    System.Random Rnd = new System.Random();//生成随机数种子
                                            // Use this for initialization
    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    public GameObject panel;
    public GameObject cube;

    int state;
    GameObject color;
    Button color_btn;
    GameObject _best;
    Button best_btn;

    int draw_num = 0;

    //mouse operation  add by Juncong Lin
    int mi_operation;  //-1 non; 0 grouping, 1 color assignment
    public GameObject m_selected_object;
    public List<GameObject> m_grouping_objects;



    //输入四种情绪
    public GameObject inputbutton;
    public GameObject button_1;
    public GameObject button_2;
    public GameObject button_3;
    public GameObject button_4;
    public GameObject button_5;
    //public GameObject button_6;
    //public GameObject button_7;
    //public GameObject button_8;
    //public GameObject button_9;
    //public GameObject button_10;
   // public GameObject button_11;
    //色彩情緒
    public Dropdown dropdown1;
    public Dropdown dropdown2;
    public Dropdown dropdown3;

    int flo = 0;
    Button btn, btn_left;
    bool isshow = false;
    public ColorHarmonizationScript obj;
    //输入四种情绪

    public ArrayList groups = GroupController.groups;


    void Start()
    {
        ////四种情绪
        //btn = inputbutton.GetComponent<Button>();
        //btn.onClick.AddListener(delegate ()
        //{
        //    if (!isshow)
        //    { //原菜单出现
        //        button_1.SetActive(true);
        //        button_2.SetActive(true);
        //        button_3.SetActive(true);
        //        button_4.SetActive(true);
        //        button_5.SetActive(true);
        //        //button_6.SetActive(true);
        //        //button_7.SetActive(true);
        //        //button_8.SetActive(true);
        //        //button_9.SetActive(true);
        //        //button_10.SetActive(true);
        //      //  button_11.SetActive(true);
        //        isshow = true;
        //    }
        //    else
        //    {
        //        button_1.SetActive(false);
        //        button_2.SetActive(false);
        //        button_3.SetActive(false);
        //        button_4.SetActive(false);
        //        button_5.SetActive(false);
        //        //button_6.SetActive(false);
        //        //button_7.SetActive(false);
        //        //button_8.SetActive(false);
        //        //button_9.SetActive(false);
        //        //button_10.SetActive(false);
        //   //     button_11.SetActive(false);
        //        isshow = false;
        //    }

        //});
        //
        mi_operation = -1;


        /*
        
        color = GameObject.Find("Start");//点击按钮开始遗传
        color_btn = color.GetComponent<Button>();//开始遗传按钮

        GameObject home = GameObject.Find("room5-white");//点击按钮开始遗传
    

        color_btn.onClick.AddListener(delegate ()
        {
            print(groups.Count);
            stopwatch.Start();
            findobject(object_name);
            stopwatch.Stop();
            Debug.Log("共用时：" + stopwatch.Elapsed);
            Debug.Log("object_name.count" + object_name.Count);
            Debug.Log("执行完了");


            
            for (int i = 0; i < object_name.Count; i++)
            {
                go = GameObject.Find(object_name[i]);
                go.GetComponent<MeshRenderer>().material.color = start[i];
            }
            
        });

        */

        // muancao
        //GameObject group = GameObject.Find("group");
        //Button group_btn = group.GetComponent<Button>();
        //group_btn.onClick.AddListener(delegate ()
        //{
        //    mi_operation = 0;
        //    Debug.Log("set as group operation");
        //});

        // muancao
        //GameObject pencil = GameObject.Find("pencil");
        //Button pencil_btn = pencil.GetComponent<Button>();
        //pencil_btn.onClick.AddListener(delegate ()
        //{
        //    mi_operation = 1;
        //    Debug.Log("set as assign operation");
        //});


        _camera = GameObject.Find("Camera").GetComponent<Camera>();

        _best = GameObject.Find("LightColorList");//点击按钮开始遗传
        //_best = GameObject.Find("best");//点击按钮开始遗传
        best_btn = _best.GetComponent<Button>();//开始遗传按钮
        best_btn.onClick.AddListener(delegate ()
        {
            ge++;
            Debug.Log("object_name.count" + object_name.Count);

            if (ge == 1)
            {
                for (int i = 0; i < object_name.Count - 1; i++)
                {
                    go = GameObject.Find(object_name[i]);
                    Debug.Log(bestIndex + "最优索引");
                    Debug.Log(R10[i] + "r" + G10[i] + "g" + B10[i] + "b");
                    go.GetComponent<Light>().color = new Color(R10[i], G10[i], B10[i]);
                }
            }
            else if (ge == 2)
            {
                for (int i = 0; i < object_name.Count - 1; i++)
                {
                    go = GameObject.Find(object_name[i]);
                    //                   Debug.Log(bestIndex + "最优索引");
                    //                   Debug.Log(R20[i] + "r" + G20[i] + "g" + B20[i] + "b");
                    go.GetComponent<Light>().color = new Color(R20[i], G20[i], B20[i]);
                }
            }
            else
            {
                for (int i = 0; i < object_name.Count - 1; i++)
                {
                    go = GameObject.Find(object_name[i]);
                    //                    Debug.Log(bestIndex + "最优索引");
                    //                    Debug.Log(al[bestIndex].R[i] + "r" + al[bestIndex].G[i] + "g" + al[bestIndex].B[i] + "b");
                    go.GetComponent<Light>().color = new Color(al[bestIndex].R[i], al[bestIndex].G[i], al[bestIndex].B[i]);
                }
            }

            GameObject.Find("MapCamera-floor1").GetComponent<MapScript>().Cube();

            /* commented output cube to image --by JuncongLin

                string path = Application.dataPath + "/" + "CubeMapTextures/";//save path
                Texture2D final = GameObject.Find("MapCamera").GetComponent<MapScript>().finalImage;
                //transfer to png
                byte[] finalByte = final.EncodeToPNG();
                if (System.IO.File.Exists(path + "Final.png"))
                    System.IO.File.Delete(path + "Final.png");
                System.IO.File.WriteAllBytes(path + "Final.png", finalByte);
                Debug.Log("save final image completed!");
                AssetDatabase.Refresh();
                */
        });

    }

    public void main()
    {
        if (!init)
        {
            //print(groups.Count);
            stopwatch.Start();
            al.Clear();

            //循环n次，添加n种配色方案进入list
            for (int i = 0; i < 10; i++)//配色方案数
            {
                World world = new World(groups.Count);
                al.Add(world);
            }

            Init();//执行初始化操作

            Debug.Log("GA.main 计算时间: " + stopwatch.Elapsed);

            //start = al;//保存初始状态

            findobject(object_name);
            stopwatch.Stop();
            Debug.Log("GA.main--共用时：" + stopwatch.Elapsed);
            Debug.Log("GA.main--执行完了");
        }
        else
        {
            stopwatch.Start();
            al.Clear();
            findobject(object_name);
            stopwatch.Stop();
            Debug.Log("共用时：" + stopwatch.Elapsed);
            Debug.Log("执行完了");
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (_camera.enabled)
        {
            state = GameObject.Find("color").GetComponent<Button_test>().state;
            if (state == 0)
            {
                object_name.Clear();
            }

            if(Input.GetMouseButtonDown(1))
            {
                //按右键响应事件 
                if(mi_operation==0)
                {
                    for(int i=0; i<m_grouping_objects.Count; i++)
                    {
                        Destroy(m_grouping_objects[i].GetComponent<cakeslice.Outline>());
                    }
                }

            }
        }
        */
    }

    void findobject(List<String> Object_Name)
    {

        //进行循n代
        for (int i = 0; i < 5; i++)//代数
        {
            Debug.Log("代：" + i);
            generation = i;
            //World world = new World(Object_Name.Count);
            Evolve(al, groups.Count);
        }



        best = energy[0];
        bestIndex = 0;
        for (int i = 0; i < energy.Count; i++)
        {
            if (best > energy[i])
            {
                best = energy[i];
                bestIndex = i;
            }
        }

        for(int i = 0; i < groups.Count; i++)
        {
            ArrayList group = (ArrayList)groups[i];
            foreach (Transform o in group)
            {
                // 修改
                // 原来：o.GetComponent<MeshRenderer>().material.color = new Color(al[bestIndex].R[i], al[bestIndex].G[i], al[bestIndex].B[i]);
                o.GetComponent<Light>().color = new Color(al[bestIndex].R[i], al[bestIndex].G[i], al[bestIndex].B[i]);
            }
        }

        StreamWriter log = new StreamWriter("test.txt", true);
        for (int i = 0; i < best_energy_every.Count; i++)
        {

            log.WriteLine("第" + i + "代能量=" + best_energy_every[i]);
        }
        log.Close();

    }

    /// <summary>
    /// 初始化配色方案
    /// </summary>

    void Init()
    {
        StreamWriter logfile = new StreamWriter("test.txt", true);
        StreamWriter Swriter = new StreamWriter("S.txt", false);
        StreamWriter Ywriter = new StreamWriter("Y.txt", false);

        //给当前这种配色方案中的所有物体上色，颜色采用随机函数生成
        for (int n = 0; n < al.Count; n++)
        {
            //            Debug.Log("Initializing world ["+n+"]...");
            Debug.Log("Init()--上色 n：" + n);
            int option_warmorcold = GameObject.Find("color").GetComponent<Button_test>().option_warmorcold;
            //随机情况
            if (option_warmorcold == 0)
            {
                for (int i = 0; i < groups.Count; i++)
                {

                    al[n].R[i] = ((float)Rnd.Next(100, 256) / 256);

                    al[n].G[i] = ((float)Rnd.Next(100, 256) / 256);

                    al[n].B[i] = ((float)Rnd.Next(100, 256) / 256);
                }
            }
            if (option_warmorcold == 1)
            //选择模式为冷色时
            {
                for (int i = 0; i < groups.Count; i++)
                {
                    int option_cold = Rnd.Next(1, 862);
                    if (option_cold <= 194)
                    {
                        al[n].R[i] = ((float)option_cold / 256);

                        al[n].G[i] = ((float)1);

                        al[n].B[i] = ((float)0);
                    }
                    if ((option_cold > 194) && (option_cold <= 450))
                    {
                        al[n].R[i] = ((float)0);

                        al[n].G[i] = ((float)1);

                        al[n].B[i] = ((float)(option_cold - 194) / 256);
                    }
                    if ((option_cold > 451) && (option_cold <= 706))
                    {
                        al[n].R[i] = ((float)0);

                        al[n].G[i] = ((float)(option_cold - 451) / 256);

                        al[n].B[i] = ((float)1);
                    }
                    else
                    {
                        al[n].R[i] = ((float)(option_cold - 760) / 256);

                        al[n].G[i] = ((float)0);

                        al[n].B[i] = ((float)255);
                    }
                }
            }
            if (option_warmorcold == 2)
            //选择模式为冷暖色时
            {
                for (int i = 0; i < groups.Count; i++)
                {

                    int option_cold = Rnd.Next(1, 476);
                    if (option_cold <= 256)
                    {
                        al[n].R[i] = ((float)1);

                        al[n].G[i] = ((float)(option_cold) / 256);

                        al[n].B[i] = ((float)0);
                    }
                    else
                    {
                        al[n].R[i] = ((float)1);

                        al[n].G[i] = ((float)0);

                        al[n].B[i] = ((float)(option_cold - 256) / 256);
                    }

                }

            }


            //将配色方案绑定到场景中
            for (int i = 0; i < groups.Count; i++)
            {
                ArrayList group = (ArrayList)groups[i];
                foreach (Transform o in group)
                {
                    o.GetComponent<Light>().color = new Color(al[n].R[i], al[n].G[i], al[n].B[i]);
                }
                Swriter.Write(al[n].R[i] + " " + al[n].G[i] + " " + al[n].B[i] + " ");
            }
            Swriter.WriteLine();



            //对当前场景的色彩进行和谐分析
            //change to new CubeJ function  by Juncong Lin
            GameObject.Find("MapCamera-floor1").GetComponent<MapScript>().CubeJ();
            //GameObject.Find("MapCamera").GetComponent<MapScript>().Cube();


            ColorHarmonizationScript script = GameObject.Find("Camera").GetComponent<ColorHarmonizationScript>();
            e = script.energy;
            //  print("e"+e);
            template = script.best_template;
            //print("template"+template);
            energy.Add(e);
            //   print("e1" + e);
            Ywriter.WriteLine(e);
            //  print("e2"+e);
            //Debug.Log(e + "||" + template);

            //logfile.WriteLine(e+"能量");


        }//for


        Swriter.Close();
        Ywriter.Close();

        

        Matlab.fit();

        Debug.Log("GA.fit check");

        logfile.WriteLine("初代能量");
        for (int i = 0; i < energy.Count; i++)
        {

            logfile.WriteLine("第" + i + "种方案=" + energy[i]);
        }
        best = energy[0];
        bestIndex = 0;
        for (int i = 0; i < energy.Count; i++)
        {
            if (best > energy[i])
            {

                best = energy[i];
                bestIndex = i;

            }
        }
        best_color_every.Add(al[bestIndex]);

        best_energy_every.Add(best);
        //getProbability()
        getProbability();

        for (int i = 0; i < probability.Count; i++)
        {
            logfile.WriteLine(probability[i] + "gailv");
        }
        logfile.Close();

        
    }

    //加入轮盘法根据其适应性获取变异发生概率，由于能量越小方案越优，故而给能量求倒?求导后再累加使用轮盘法
    void getProbability()
    {

        probability.Clear();

        float sum = 0;
        for (int i = 0; i < energy.Count; i++)
        {
            probability.Add(1 / energy[i]);
        }
        for (int i = 0; i < probability.Count; i++)
        {
            sum += probability[i];
        }
        for (int i = 0; i < probability.Count; i++)
        {
            probability[i] = probability[i] / sum;
        }
    }

    /// <summary>
    /// 进化函数
    /// </summary>
    void Evolve(List<World> al, int maxobject)
    {
        StreamWriter logfile = new StreamWriter("test.txt", true);
        //for (int i = 0; i < al.Count; i++)
        //{
        //    for (int j = 0; j < maxobject; j++)
        //    {
        //        logfile.WriteLine(al[i].R[j] + " " + al[i].G[j] + " " + al[i].B[j] + "初始状态");
        //    }
        //}
        float[] fitness = new float[al.Count];

        float[] fitR = new float[maxobject];

        float[] fitG = new float[maxobject];

        float[] fitB = new float[maxobject];

        string[] fitname = new string[maxobject];

        List<World> temporary = new List<World>();




        //交叉互换，根据轮盘概率随机交叉，两种被选中的配色方案若交叉，则对其中一个物体的rgb进行交叉

        for (int i = 0; i < al.Count / 2; i++)
        {

            int temp1 = 0, temp2 = 0;
            int begin, end;
            int between;
            float r0, g0, b0, R0, G0, B0;
            int r, g, b, R, G, B;
            string r1, b1, g1, R1, G1, B1;
            char[] r2 = new char[8]; char[] g2 = new char[8]; char[] b2 = new char[8]; char[] R2 = new char[8]; char[] G2 = new char[8]; char[] B2 = new char[8];
            int[] r3 = new int[8]; int[] g3 = new int[8]; int[] b3 = new int[8]; int[] R3 = new int[8]; int[] G3 = new int[8]; int[] B3 = new int[8];
            float temp = (float)Rnd.NextDouble();
            //logfile.WriteLine("0到1的随机数1=" + temp);
            for (int n = 0; n < al.Count; n++)
            {
                if ((temp - probability[n]) > 0) { temp = temp - probability[n]; }
                else { temp1 = n; break; }
            }

            temp = (float)Rnd.NextDouble();
            //logfile.WriteLine("0到1的随机数2=" + temp);
            for (int n = 0; n < al.Count; n++)
            {
                if ((temp - probability[n]) > 0) { temp = temp - probability[n]; }
                else { temp2 = n; break; }
            }
            //logfile.WriteLine("第一个方案=" + temp1 + " " + "第二个方案=" + temp2);
            //改变第change个物体的rgb，对其进行二进制交叉

            //for (int j = 0; j < maxobject; j++)
            //{
            //    logfile.WriteLine(al[temp1].R[j] + " " + al[temp1].G[j] + " " + al[temp1].B[j] + "交换前的方案一");
            //    logfile.WriteLine(al[temp2].R[j] + " " + al[temp2].G[j] + " " + al[temp2].B[j] + "交换前的方案二");
            //}

            World x = new World(maxobject);
            for (int f = 0; f < maxobject; f++)
            {
                x.name[f] = al[temp1].name[f];
                x.R[f] = al[temp1].R[f];
                x.G[f] = al[temp1].G[f];
                x.B[f] = al[temp1].B[f];
            }
            World y = new World(maxobject);
            for (int f = 0; f < maxobject; f++)
            {
                y.name[f] = al[temp2].name[f];
                y.R[f] = al[temp2].R[f];
                y.G[f] = al[temp2].G[f];
                y.B[f] = al[temp2].B[f];
            }
            //if (Rnd.Next(1, 20) != 1)
            {
                for (int change = 0; change < maxobject; change++)
                {
                    //change = Rnd.Next(0, maxobject - 1);
                    //logfile.WriteLine(change + " " + "选择方案中的第几个物体交换");
                    r0 = al[temp1].R[change]; g0 = al[temp1].G[change]; b0 = al[temp1].B[change];
                    R0 = al[temp2].R[change]; G0 = al[temp2].G[change]; B0 = al[temp2].B[change];

                    //logfile.WriteLine("r0=" + al[temp1].R[change] + " " + "g0=" + al[temp1].G[change] + " " + "b0=" + al[temp1].B[change]);
                    //logfile.WriteLine("R0=" + al[temp2].R[change] + " " + "G0=" + al[temp2].G[change] + " " + "B0=" + al[temp2].B[change]);

                    //数值型转二进制字符串
                    r1 = Convert.ToString((int)(r0 * 256 + 0.5), 2).PadLeft(8, '0');
                    g1 = Convert.ToString((int)(g0 * 256 + 0.5), 2).PadLeft(8, '0');
                    b1 = Convert.ToString((int)(b0 * 256 + 0.5), 2).PadLeft(8, '0');
                    R1 = Convert.ToString((int)(R0 * 256 + 0.5), 2).PadLeft(8, '0');
                    G1 = Convert.ToString((int)(G0 * 256 + 0.5), 2).PadLeft(8, '0');
                    B1 = Convert.ToString((int)(B0 * 256 + 0.5), 2).PadLeft(8, '0');

                    //logfile.WriteLine("r1=" + r1 + " " + "g1=" + g1 + " " + "b1=" + b1);
                    //logfile.WriteLine("R1=" + R1 + " " + "G1=" + G1 + " " + "B1=" + B1);

                    //二进制字符串转字符数组
                    r2 = r1.ToCharArray();
                    g2 = g1.ToCharArray();
                    b2 = b1.ToCharArray();
                    R2 = R1.ToCharArray();
                    G2 = G1.ToCharArray();
                    B2 = B1.ToCharArray();

                    //logfile.WriteLine("r2=");
                    //for (int n = 0; n < 8; n++)
                    //{
                    //    logfile.Write(r2[n]);
                    //}
                    //logfile.WriteLine("g2=");
                    //for (int n = 0; n < 8; n++)
                    //{
                    //    logfile.Write(g2[n]);
                    //}
                    //logfile.WriteLine("b2=");
                    //for (int n = 0; n < 8; n++)
                    //{
                    //    logfile.Write(b2[n]);
                    //}
                    //logfile.WriteLine("R2=");
                    //for (int n = 0; n < 8; n++)
                    //{
                    //    logfile.Write(R2[n]);
                    //}
                    //logfile.WriteLine("G2=");
                    //for (int n = 0; n < 8; n++)
                    //{
                    //    logfile.Write(G2[n]);
                    //}
                    //logfile.WriteLine("B2=");
                    //for (int n = 0; n < 8; n++)
                    //{
                    //    logfile.Write(B2[n]);
                    //}

                    //字符数组转int数组
                    for (int n = 0; n < 8; n++)
                    {
                        r3[n] = (int)r2[n] - '0';
                        g3[n] = (int)g2[n] - '0';
                        b3[n] = (int)b2[n] - '0';
                        R3[n] = (int)R2[n] - '0';
                        G3[n] = (int)G2[n] - '0';
                        B3[n] = (int)B2[n] - '0';
                    }
                    //logfile.WriteLine("r3=");
                    //for (int n = 0; n < 8; n++)
                    //{
                    //    logfile.Write(r3[n]);
                    //}
                    //logfile.WriteLine("g3=");
                    //for (int n = 0; n < 8; n++)
                    //{
                    //    logfile.Write(g3[n]);
                    //}
                    //logfile.WriteLine("b3=");
                    //for (int n = 0; n < 8; n++)
                    //{
                    //    logfile.Write(b3[n]);
                    //}
                    //logfile.WriteLine("R3=");
                    //for (int n = 0; n < 8; n++)
                    //{
                    //    logfile.Write(R2[n]);
                    //}
                    //logfile.WriteLine("G3=");
                    //for (int n = 0; n < 8; n++)
                    //{
                    //    logfile.Write(G2[n]);
                    //}
                    //logfile.WriteLine("B3=");
                    //for (int n = 0; n < 8; n++)
                    //{
                    //    logfile.Write(B2[n]);
                    //}

                    begin = Rnd.Next(0, 7); end = Rnd.Next(begin, 7);
                    for (int m = begin; m <= end; m++)
                    {
                        between = r3[m];
                        r3[m] = R3[m];
                        R3[m] = between;

                    }

                    begin = Rnd.Next(0, 7); end = Rnd.Next(begin, 7);
                    for (int m = begin; m <= end; m++)
                    {
                        between = g3[m];
                        g3[m] = G3[m];
                        G3[m] = between;

                    }

                    begin = Rnd.Next(0, 7); end = Rnd.Next(begin, 7);
                    for (int m = begin; m <= end; m++)
                    {
                        between = b3[m];
                        b3[m] = B3[m];
                        B3[m] = between;

                    }

                    r0 = 0; g0 = 0; b0 = 0; R0 = 0; G0 = 0; B0 = 0;
                    r = 0; g = 0; b = 0; R = 0; G = 0; B = 0;
                    for (int m = 7; m >= 0; m--)
                    {
                        r += r3[7 - m] * (int)Math.Pow(2, m);
                        g += g3[7 - m] * (int)Math.Pow(2, m);
                        b += b3[7 - m] * (int)Math.Pow(2, m);
                        R += R3[7 - m] * (int)Math.Pow(2, m);
                        G += G3[7 - m] * (int)Math.Pow(2, m);
                        B += B3[7 - m] * (int)Math.Pow(2, m);
                    }
                    //logfile.WriteLine("r=" + r + " " + "g=" + g + " " + "b=" + b);
                    //logfile.WriteLine("R=" + R + " " + "G=" + G + " " + "B=" + B);
                    r0 = r / (float)256; g0 = g / (float)256; b0 = b / (float)256; R0 = R / (float)256; G0 = G / (float)256; B0 = B / (float)256;

                    //logfile.WriteLine("r0=" + r0 + " " + "g0=" + g0 + " " + "b0=" + b0+"交换后的物体结果1");
                    //logfile.WriteLine("R0=" + R0 + " " + "G0=" + G0 + " " + "B0=" + B0+"交换后的物体结果2");


                    x.R[change] = r0; x.G[change] = g0; x.B[change] = b0;
                    y.R[change] = R0; y.G[change] = G0; y.B[change] = B0;
                }
                //logfile.WriteLine("x.R[change]=" + x.R[change] + " " + "x.G[change]=" + x.G[change] + " " + "x.B[change]=" + x.B[change]);
                //logfile.WriteLine("y.R[change]=" + x.R[change] + " " + "y.G[change]=" + x.G[change] + " " + "y.B[change]=" + x.B[change]);

                temporary.Add(x); temporary.Add(y);
            }
            //else
            //{
            //    temporary.Add(x); temporary.Add(y);
            //}
            //for (int j = 0; j < maxobject; j++)
            //{
            //    logfile.WriteLine(x.R[j] + " " +x.G[j] + " " + x.B[j] + "x的值");
            //}
            //for (int j = 0; j < maxobject; j++)
            //{
            //    logfile.WriteLine(y.R[j] + " " + y.G[j] + " " + y.B[j] + "y的值");
            //}


            //al.Clear();
            //for (int i = 0; i < temporary.Count; i++)
            //{
            //    for (int j = 0; j < maxobject; j++)
            //    {
            //        logfile.WriteLine(temporary[i].R[j] + " " + temporary[i].G[j] + " " + temporary[i].B[j] + "temporary的值");
            //    }
            //}
        }
        al.Clear();
        al.AddRange(temporary);

        // Debug.Log("GA.Evolve 交叉互换--check ");

        //for (int i = 0; i < al.Count; i++)
        //{
        //    for (int j = 0; j < maxobject; j++)
        //    {
        //        logfile.WriteLine(temporary[i].R[j] + " " + temporary[i].G[j] + " " + temporary[i].B[j] + "AL的值");
        //    }
        //}

        //变异
        int select, temp_var;
        float var;
        string var1;
        char[] var2 = new char[8];
        int[] var3 = new int[8];
        for (int h = 0; h < al.Count; h++)
        {
            for (int v = 0; v < (maxobject); v++)
            {
                if (Rnd.Next(1, 100) == 1)
                {
                    temp_var = 0;
                    var = al[h].R[v];
                    var1 = Convert.ToString((int)(var * 256 + 0.5), 2).PadLeft(8, '0');
                    var2 = var1.ToCharArray();
                    for (int n = 0; n < 8; n++)
                    {
                        var3[n] = (int)var2[n] - '0';
                    }
                    select = Rnd.Next(0, 7);
                    if (var3[select] == 0) var3[select]++;
                    else if (var3[select] == 1) var3[select]--;
                    for (int m = 7; m >= 0; m--)
                    {
                        temp_var += var3[7 - m] * (int)Math.Pow(2, m);
                    }
                    al[h].R[v] = (temp_var / (float)256);
                }

                if (Rnd.Next(1, 100) == 1)
                {
                    temp_var = 0;
                    var = al[h].G[v];
                    var1 = Convert.ToString((int)(var * 256 + 0.5), 2).PadLeft(8, '0');
                    var2 = var1.ToCharArray();
                    for (int n = 0; n < 8; n++)
                    {
                        var3[n] = (int)var2[n] - '0';
                    }
                    select = Rnd.Next(0, 7);
                    if (var3[select] == 0) var3[select]++;
                    else if (var3[select] == 1) var3[select]--;
                    for (int m = 7; m >= 0; m--)
                    {
                        temp_var += var3[7 - m] * (int)Math.Pow(2, m);
                    }
                    al[h].G[v] = (temp_var / (float)256);
                }

                if (Rnd.Next(1, 100) == 1)
                {
                    temp_var = 0;
                    var = al[h].B[v];
                    var1 = Convert.ToString((int)(var * 256 + 0.5), 2).PadLeft(8, '0');
                    var2 = var1.ToCharArray();
                    for (int n = 0; n < 8; n++)
                    {
                        var3[n] = (int)var2[n] - '0';
                    }
                    select = Rnd.Next(0, 7);
                    if (var3[select] == 0) var3[select]++;
                    else if (var3[select] == 1) var3[select]--;
                    for (int m = 7; m >= 0; m--)
                    {
                        temp_var += var3[7 - m] * (int)Math.Pow(2, m);
                    }
                    al[h].B[v] = (temp_var / (float)256);
                }
                //if (Rnd.Next(1, 20) == 1)

                //    al[Rnd.Next(0, al.Count)].R[v] = ((float)Rnd.Next(1, 256) / 256);

                //if (Rnd.Next(1, 20) == 1)

                //    al[Rnd.Next(0, al.Count)].G[v] = ((float)Rnd.Next(1, 256) / 256);

                //if (Rnd.Next(1, 20) == 1)

                //    al[Rnd.Next(0, al.Count)].B[v] = ((float)Rnd.Next(1, 256) / 256);

            }
        }

        energy.Clear();

        StreamWriter Xwriter = new StreamWriter("X.txt", false);
        StreamWriter REALwriter = new StreamWriter("REAL.txt", false);

        // Debug.Log("GA.Evolve 变异--check "); 

        //执行循环，将当前经过交叉，变异等操作后的n种配色方案画到场景里去，然后执行计算能量存入energy，以便执行下一次进化循环
        for (int i = 0; i < al.Count; i++)
        {
            for (int j = 0; j < maxobject; j++)
            {
                Xwriter.Write(al[i].R[j] + " " + al[i].G[j] + " " + al[i].B[j] + " ");

                //logfile.WriteLine(al[i].R[j]+" "+ al[i].G[j]+" "+ al[i].B[j] + "rgb");
                // logfile.WriteLine("\\\n");
            }

            //将配色方案绑定到场景中
            for (int k = 0; k < groups.Count; k++)
            {
                ArrayList group = (ArrayList)groups[k];
                foreach (Transform o in group)
                {
                    o.GetComponent<Light>().color = new Color(al[i].R[k], al[i].G[k], al[i].B[k]);
                }
            }

            Xwriter.WriteLine();
            //replace with new function CubeJ() by Juncong Lin


 //         GameObject.Find("MapCamera").GetComponent<MapScript>().CubeJ();
 //         GameObject.Find("MapCamera").GetComponent<MapScript>().Cube();
 //           ColorHarmonizationScript script = GameObject.Find("Camera").GetComponent<ColorHarmonizationScript>();
 //           e = script.energy;
 //           REALwriter.WriteLine(e);

 //            energy.Add(e);
            //logfile.WriteLine(e + "能量");


        }

        // Debug.Log("GA.Evolve 变异--check ");
        REALwriter.Close();
        
        Xwriter.Close();

        Matlab.predict();

        //

        StreamReader RESreader = new StreamReader("RES.txt");
        string line;
        while ((line = RESreader.ReadLine()) != null && line.Length != 0)
        {
            energy.Add(Convert.ToSingle(line));
        }
        RESreader.Close();


        //fanhua(maxobject);


        //logfile.WriteLine("/n");
        dai++;
        logfile.WriteLine(dai + "代能量");
        for (int i = 0; i < energy.Count; i++)
        {
            logfile.WriteLine("第" + i + "种方案=" + energy[i]);
        }

        al.Add(best_color_every[best_color_every.Count - 1]);
        energy.Add(best_energy_every[best_energy_every.Count - 1]);

        best = energy[0];
        bestIndex = 0;
        for (int i = 0; i < energy.Count; i++)
        {
            if (best > energy[i])
            {
                best = energy[i];
                bestIndex = i;
            }
        }

        //float l = loss(al[bestIndex], best);

        //Debug.Log("loss: " + l);
        
        best_color_every.Add(al[bestIndex]);

        StreamWriter draw = new StreamWriter("draw.txt", true);

        //draw

        //draw.WriteLine(draw_num.ToString() + " " + best.ToString() + " " + l.ToString() + "\n");
        draw_num++;

        draw.Close();

        // 这里没有输出
        Debug.Log(best);

        if (generation == 9)
        {
            name10 = new List<string>(al[bestIndex].name);
            R10 = new List<float>(al[bestIndex].R);
            G10 = new List<float>(al[bestIndex].G);
            B10 = new List<float>(al[bestIndex].B);
        }

        else if (generation == 19)
        {
            name20 = new List<string>(al[bestIndex].name);
            R20 = new List<float>(al[bestIndex].R);
            G20 = new List<float>(al[bestIndex].G);
            B20 = new List<float>(al[bestIndex].B);
        }
        best_energy_every.Add(best);

        getProbability();
        //for (int i = 0; i < probability.Count; i++)
        //{
        //    logfile.WriteLine(probability[i] + "gailv");
        //}
        logfile.Close();
    }



    public void fanhua(int maxobject)
    {
        StreamWriter Xwriter = new StreamWriter("FANHUAYANGBEN.txt", false);
        StreamWriter REALwriter = new StreamWriter("FANHUAZHENSHIZHI.txt", false);

        List<World> al = new List<World>();

        for (int i = 0; i < 50; i++)
        {
            World world = new World(groups.Count);
            al.Add(world);
        }

        //执行循环，将当前经过交叉，变异等操作后的n种配色方案画到场景里去，然后执行计算能量存入energy，以便执行下一次进化循环
        for (int i = 0; i < 50; i++)
        {
            for (int j = 0; j < groups.Count; j++)
            {
                al[i].R[j] = ((float)Rnd.Next(0, 256) / 256);

                al[i].G[j] = ((float)Rnd.Next(0, 256) / 256);

                al[i].B[j] = ((float)Rnd.Next(0, 256) / 256);

                Xwriter.Write(al[i].R[j] + " " + al[i].G[j] + " " + al[i].B[j] + " ");

                //logfile.WriteLine(al[i].R[j]+" "+ al[i].G[j]+" "+ al[i].B[j] + "rgb");
                // logfile.WriteLine("\\\n");
            }

            //将配色方案绑定到场景中
            for (int k = 0; k < groups.Count; k++)
            {
                ArrayList group = (ArrayList)groups[k];
                foreach (Transform o in group)
                {
                    o.GetComponent<Light>().color = new Color(al[i].R[k], al[i].G[k], al[i].B[k]);
                }
            }

            Xwriter.WriteLine();
            //replace with new function CubeJ() by Juncong Lin


            GameObject.Find("MapCamera-floor1").GetComponent<MapScript>().CubeJ();
            //         GameObject.Find("MapCamera").GetComponent<MapScript>().Cube();
            ColorHarmonizationScript script = GameObject.Find("Camera").GetComponent<ColorHarmonizationScript>();
            e = script.energy;

            REALwriter.WriteLine(e);

            energy.Add(e);
            //logfile.WriteLine(e + "能量");


        }
        REALwriter.Close();

        Xwriter.Close();


        Matlab.predict("FANHUAJIEGUO.txt");

        StreamReader RESreader = new StreamReader("FANHUAJIEGUO.txt");
        string line;
        while ((line = RESreader.ReadLine()) != null && line.Length != 0)
        {
            energy.Add(Convert.ToSingle(line));
        }
        RESreader.Close();
    }


    float loss(World w, float energy)
    {
        for (int k = 0; k < groups.Count; k++)
        {
            ArrayList group = (ArrayList)groups[k];
            foreach (Transform o in group)
            {
                o.GetComponent<Light>().color = new Color(w.R[k], w.G[k], w.B[k]);
            }
        }
        GameObject.Find("MapCamera").GetComponent<MapScript>().CubeJ();
        ColorHarmonizationScript script = GameObject.Find("Camera").GetComponent<ColorHarmonizationScript>();

        return Mathf.Abs(script.energy - energy);
    }



    public void input_emotion1()
    {
        if (dropdown1.value == 0)
            ColorSystem.isActive = true;
        if (dropdown1.value == 1)
            ColorSystem.isActive = false;
    }
    public void input_emotion2()
    {
        if (dropdown2.value == 0)
            ColorSystem.isWarm = true;
        if (dropdown2.value == 1)
            ColorSystem.isWarm = false;
    }
    public void input_emotion3()
    {
        if (dropdown3.value == 0)
            ColorSystem.isHeavy = true;
        if (dropdown3.value == 1)
            ColorSystem.isHeavy = false;
    }


    public class World
    {

        public string[] name;

        public float[] R;

        public float[] G;

        public float[] B;


        public World(int Maxobject)
        {
            name = new string[Maxobject];
            R = new float[Maxobject];
            G = new float[Maxobject];
            B = new float[Maxobject];
        }



    }
}
