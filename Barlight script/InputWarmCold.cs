using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//注意这个不能少

public class InputWarmCold : MonoBehaviour
{
    //public GameObject Gmenue;
    public GameObject inputbutton;
    public GameObject button_1;
    public GameObject button_2;
    public GameObject button_3;
   
    int flo = 0;
    Button btn, btn_left;
    bool isshow = false;
//  public ColorHarmonizationScript obj;
    // Use this for initialization
    void Start()
    {
        btn = inputbutton.GetComponent<Button>();
        btn.onClick.AddListener(delegate ()
        {
            if (!isshow)
            { //原菜单出现
                button_1.SetActive(true);
                button_2.SetActive(true);
                button_3.SetActive(true);
               
                isshow = true;
            }
            else
            {
                button_1.SetActive(false);
                button_2.SetActive(false);
                button_3.SetActive(false);
               
                isshow = false;
            }

        });

    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
