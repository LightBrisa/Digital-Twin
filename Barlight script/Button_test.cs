using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_test : MonoBehaviour
{
    public int option_warmorcold = 0;//0随机，1冷色，2暖色
    public bool choice; //
    public int state = 0;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void Click_test()
    {
        if (state == 0)//判断是否按键
        {
            choice = UnityEditor.EditorUtility.DisplayDialog("功能提示", "系统将自动着色", "随机", "选择模式");
            if (choice)//选择随机
            {
                option_warmorcold = 0;
                state = 1;
            }
            else
            {
                choice = UnityEditor.EditorUtility.DisplayDialog("功能提示", "请选择着色色调", "冷色调", "暖色调");
                if (choice)//选择冷色
                {
                    option_warmorcold = 1;
                    state = 1;
                }
                else option_warmorcold = 2;
            }
        }
        else if (state == 1)
        {
            state = 0;
        }

    }

}
