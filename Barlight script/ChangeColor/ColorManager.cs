using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEditor;

public class ColorManager : MonoBehaviour
{
    public ColorRGB CRGB;
    public ColorPanel CP;
    public ColorCircle CC;

    public Slider sliderCRGB;
    public Image colorShow;

    GameObject cube;
    Color getColor;
    GA script;

    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    public GameObject panel;
    bool panel_flag = false;

    void OnDisable()
    {
        CC.getPos -= CC_getPos;
    }

    void OnEnable()
    {
        CC.getPos += CC_getPos;
    }

    private void CC_getPos(Vector2 pos)
    {
        getColor = CP.GetColorByPosition(pos);
        colorShow.color = getColor;
    }

    void Show()
    {
        panel.SetActive(true);
    }

    // Use this for initialization
    void Start()
    {
        //替换 选中的模型方法   鼠标射线获取物体，存放在数组中，
        script = GameObject.Find("GameObject").GetComponent<GA>();

        GameObject btn = GameObject.Find("Canvas/Panel/ok_Button");
        Button btn1 = (Button)btn.GetComponent<Button>();
        btn1.onClick.AddListener(onClick_1);
        //判断是画笔点击的模型
   

        panel.SetActive(panel_flag);

        //开启绘板
/*
        GameObject PaintButton = GameObject.Find("pencil");
        Button paint = (Button)PaintButton.GetComponent<Button>();
        paint.onClick.AddListener(delegate ()
        {
            panel_flag = !panel_flag;
            panel.SetActive(panel_flag);
            if(panel_flag) CC.getPos += CC_getPos;
        });
*/
        sliderCRGB.onValueChanged.AddListener(OnCRGBValueChanged);
        
    }


    void Update()
    {
    }

    void onClick_1()
    {
        Debug.Log("ok");
        //cube替换
        cube = GameObject.Find("GameObject").GetComponent<GA>().m_selected_object;
        cube.GetComponent<MeshRenderer>().material.color = getColor;
        panel.SetActive(false);

 //     GameObject.Find("MapCamera").GetComponent<MapScript>().Cube();

        /* commented output cube to image --by JuncongLin
        stopwatch.Start();
        string path = Application.dataPath + "/" + "CubeMapTextures/";//save path
        Texture2D final = GameObject.Find("MapCamera").GetComponent<MapScript>().finalImage;
        //transfer to png
        byte[] finalByte = final.EncodeToPNG();
        if (System.IO.File.Exists(path + "Final.png"))
            System.IO.File.Delete(path + "Final.png");
        System.IO.File.WriteAllBytes(path + "Final.png", finalByte);
        Debug.Log("save final image completed!");
        AssetDatabase.Refresh();
        stopwatch.Stop();
        Debug.Log("Time for writing CubeMap to a file: " + stopwatch.Elapsed);
 */       
    }

    void OnCRGBValueChanged(float value)
    {
        Color endColor = CRGB.GetColorBySliderValue(value);
        CP.SetColorPanel(endColor);
        CC.setShowColor();
    }
}
