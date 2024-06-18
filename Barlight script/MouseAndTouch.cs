using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAndTouch : MonoBehaviour
{
    public FlexibleColorPicker fcp;
    private GameObject thisObj; // 获取鼠标或手指选中的物体
                                // 若涉及物体较多建议命名GameManager加载到其中，然后利用Tag或Layer去控制
    private GameObject thisLight;
    private char oldLightnum;
    private char newLightnum;

    void Update()
    {
        
        //鼠标或单指检测
        if (Input.GetMouseButtonDown(0))//进行点击
        {
            if (thisObj == null)//首先判断目前是否有物体--没有
            {
                Debug.Log("thisObj == null");
                RaycastHit hit = CastRay(); //创建屏幕发射射线

                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("highLight"))
                    {
                        return;
                    }
                    thisObj = hit.collider.gameObject;
                    thisObj.GetComponent<Outline>().enabled = true; //选中则将高亮脚本设置为True激活状态
                    
                }
                //找到灯光 这里高亮的只是mesh       
                // Debug.Log("thisObj: " + thisObj.name[15]);
                string Lightname = "SpotLight_test_" + thisObj.name[15];
                // Debug.Log("thisLight: " + Lightname);
                thisLight = GameObject.Find(Lightname);
                newLightnum = thisObj.name[15];
                fcp.color = thisLight.GetComponent<Light>().color;
            }
            else//有物体
            {
                // thisLight.GetComponent<Light>().color = fcp.color;
                // thisObj.GetComponent<Outline>().enabled = false;
                // thisObj = null;
                // Cursor.visible = true; //显示光标
                // thisLight=
                // thisObj.GetComponent<Light>().color = fcp.color;
            }
        }
        else//未点击时刻
        {
            if (thisObj != null)
            {
                thisLight.GetComponent<Light>().color = fcp.color;
            }
        }

    }

    /// <summary>
    /// 射线碰撞
    /// </summary>
    /// <returns>RaycastHit hit射线碰撞体</returns>
    private RaycastHit CastRay()
    {
        Vector3 screenFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldFar = Camera.main.ScreenToWorldPoint(screenFar); //屏幕到碰撞点的向量
        Vector3 worldNear = Camera.main.ScreenToWorldPoint(screenNear); //屏幕发射点的向量
        RaycastHit hit;
        Physics.Raycast(worldNear, worldFar - worldNear, out hit);

        return hit;
    }

}
