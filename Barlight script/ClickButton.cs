using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClickButton : MonoBehaviour
{
    Camera Cam0;
    Camera Cam1;
    Camera Cam2;
    Camera Cam3;
    ColorRingScript Cam2Script;
    bool flag;

    public GameObject grouping;
    public GameObject tooltip;
    public bool tt_flag;

    // Use this for initialization
    void Start()
    {
        //GameObject btnObj1 = GameObject.Find("import");
        //GameObject btnObj3 = GameObject.Find("pencil");
        //GameObject btnObj4 = GameObject.Find("color");
        GameObject btnObj5 = GameObject.Find("map");
        //GameObject btnObj6 = GameObject.Find("cube");
        GameObject btnObj9 = GameObject.Find("LightColorAnalysis");
        //GameObject btnObj10 = GameObject.Find("best");
        //GameObject btnObj11 = GameObject.Find("group");
        GameObject btnObj12 = GameObject.Find("LightColorAdjust");

        //Button btn1 = btnObj1.GetComponent<Button>();
        //Button btn3 = btnObj3.GetComponent<Button>();
        //Button btn4 = btnObj4.GetComponent<Button>();
        Button btn5 = btnObj5.GetComponent<Button>();
        //Button btn6 = btnObj6.GetComponent<Button>();
        Button btn9 = btnObj9.GetComponent<Button>();
        //Button btn10 = btnObj10.GetComponent<Button>();
        //Button btn11 = btnObj11.GetComponent<Button>();
        Button btn12 = btnObj12.GetComponent<Button>();


        //btn1.onClick.AddListener(import);
        //btn3.onClick.AddListener(pencil);
        //btn4.onClick.AddListener(color);
        //btn6.onClick.AddListener(cube);
        btn9.onClick.AddListener(delegate ()
        {
            ColorAnalysis();
        });
        //btn10.onClick.AddListener(best);
        btn12.onClick.AddListener(delegate ()
        {
            LightColorAdjust();
        });

        Cam0 = GameObject.Find("Camera").GetComponent<Camera>();
        Cam1 = GameObject.Find("MapCamera-floor1").GetComponent<Camera>();
        Cam2 = GameObject.Find("UICamera").GetComponent<Camera>();
        Cam3 = GameObject.Find("AdjustCamera_persp").GetComponent<Camera>();

        flag = true;
        Cam0.enabled = flag;
        Cam1.enabled = !flag;
        Cam2.enabled = false;
        Cam3.enabled = false;

        Cam2Script = GameObject.Find("UICamera").GetComponent<ColorRingScript>();


        tt_flag = false;

    }

    public void group()
    {
        grouping.SetActive(true);
    }

    void import()
    {
        Debug.Log("import！");

    }
    void color()
    {
        Debug.Log("color！");

    }
    void pencil()
    {
        Debug.Log("pencil！");

    }
    public void map()
    {
        Debug.Log("map！");

        flag = Cam0.enabled;
        Cam0.enabled = !flag; //Cam0 正视图摄像机
        Cam1.enabled = flag;  //Cam1 俯视图摄像机

        Cam2.enabled = false;

    }

    void cube()
    {
        Debug.Log("cube！");

    }
    void save()
    {
        Debug.Log("save！");

    }
    void ColorAnalysis()
    {
        Debug.Log("circles！");
        //SceneManager.LoadScene(1);
        bool f = Cam2.enabled;
        if (f)
        {
            flag = true;
            Cam0.enabled = true;
            Cam1.enabled = false;
        }
        else
        {
            Cam0.enabled = false;
            Cam1.enabled = false;
        }
        Cam2.enabled = !f;

        Debug.Log("UICamera.State: " + Cam2.enabled);


        // Cam2Script.ColorRender();


    }

    void LightColorAdjust()
    {
        Debug.Log("LightColorAdjust！");
        bool f = Cam3.enabled;
        if (f)
        {
            flag = true;
            Cam0.enabled = true;
            Cam1.enabled = false;
        }
        else
        {
            Cam0.enabled = false;
            Cam1.enabled = false;
        }
        Cam3.enabled = !f;
       
    }

    void best()
    {
        Debug.Log("best！");


    }

    // Update is called once per frame
    void Update()
    {

    }
}