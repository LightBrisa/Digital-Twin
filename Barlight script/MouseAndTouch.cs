using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseAndTouch : MonoBehaviour
{
    public FlexibleColorPicker fcp;
    private GameObject thisObj; // ��ȡ������ָѡ�е�����
                                // ���漰����϶ཨ������GameManager���ص����У�Ȼ������Tag��Layerȥ����
    private GameObject thisLight;
    private char oldLightnum;
    private char newLightnum;

    void Update()
    {
        
        //����ָ���
        if (Input.GetMouseButtonDown(0))//���е��
        {
            if (thisObj == null)//�����ж�Ŀǰ�Ƿ�������--û��
            {
                Debug.Log("thisObj == null");
                RaycastHit hit = CastRay(); //������Ļ��������

                if (hit.collider != null)
                {
                    if (!hit.collider.CompareTag("highLight"))
                    {
                        return;
                    }
                    thisObj = hit.collider.gameObject;
                    thisObj.GetComponent<Outline>().enabled = true; //ѡ���򽫸����ű�����ΪTrue����״̬
                    
                }
                //�ҵ��ƹ� ���������ֻ��mesh       
                // Debug.Log("thisObj: " + thisObj.name[15]);
                string Lightname = "SpotLight_test_" + thisObj.name[15];
                // Debug.Log("thisLight: " + Lightname);
                thisLight = GameObject.Find(Lightname);
                newLightnum = thisObj.name[15];
                fcp.color = thisLight.GetComponent<Light>().color;
            }
            else//������
            {
                // thisLight.GetComponent<Light>().color = fcp.color;
                // thisObj.GetComponent<Outline>().enabled = false;
                // thisObj = null;
                // Cursor.visible = true; //��ʾ���
                // thisLight=
                // thisObj.GetComponent<Light>().color = fcp.color;
            }
        }
        else//δ���ʱ��
        {
            if (thisObj != null)
            {
                thisLight.GetComponent<Light>().color = fcp.color;
            }
        }

    }

    /// <summary>
    /// ������ײ
    /// </summary>
    /// <returns>RaycastHit hit������ײ��</returns>
    private RaycastHit CastRay()
    {
        Vector3 screenFar = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.farClipPlane);
        Vector3 screenNear = new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane);
        Vector3 worldFar = Camera.main.ScreenToWorldPoint(screenFar); //��Ļ����ײ�������
        Vector3 worldNear = Camera.main.ScreenToWorldPoint(screenNear); //��Ļ����������
        RaycastHit hit;
        Physics.Raycast(worldNear, worldFar - worldNear, out hit);

        return hit;
    }

}
