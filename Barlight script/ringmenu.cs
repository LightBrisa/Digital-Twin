using UnityEngine;
using System.Collections;
using UnityEngine.UI;//注意这个不能少
//using UnityEditor.Sprites ;
public class ringmenu : MonoBehaviour
{

    public GameObject moodInput;
    public GameObject fcp;
    //public GameObject Gmenue;
    public GameObject btn_ColorAnalysis;
    public GameObject btn_LightColorAdjust;

    public GameObject sld_Allslider;
    public ColorHarmonizationScript cam_CHS;

    int flo = 0;
    Button btn,btn_left;
    bool isshow = false;
    

    // Use this for initialization
    void Start()
    {
        btn = btn_ColorAnalysis.GetComponent<Button>();
        // onlick在这里
        btn.onClick.AddListener(delegate ()
        {
            if (!isshow)
            { //原菜单出现
                sld_Allslider.SetActive(true);
                isshow = true;
            }
            else
            {
                sld_Allslider.SetActive(false);
                isshow = false;
            }

        });

    }

    public void openMoodInput()
    {
        moodInput.SetActive(!moodInput.active);
    }

    public void openfcp()
    {
        fcp.SetActive(!fcp.active);
    }
    // Update is called once per frame
    void Update()
    {


    }
}


