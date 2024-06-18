using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FiCameraControl : MonoBehaviour {

	public float moveSpeed = 0.001f;
	public float rotateSpeed = 0.5f;

	public static Vector3 kUpDirection=new Vector3(0.0f,1.0f,0.0f);

	//control rotate
	private float m_fLastMousePosX=0.0f;
	private float m_fLastMousePosY=0.0f;
    private bool m_bMouseRightKeyDown = false;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (this.GetComponent<Camera>().enabled) {
            //判断旋转  
            if (Input.GetMouseButtonDown(1)) //鼠标右键刚刚按下了  
            {
                if (m_bMouseRightKeyDown == false)
                {
                    m_bMouseRightKeyDown = true;
                    Vector3 kMousePos = Input.mousePosition;
                    m_fLastMousePosX = kMousePos.x;
                    m_fLastMousePosY = kMousePos.y;
                }
            }
            else if (Input.GetMouseButtonUp(1)) //鼠标右键刚刚抬起了  
            {
                if (m_bMouseRightKeyDown == true)
                {
                    m_bMouseRightKeyDown = false;
                    m_fLastMousePosX = 0;
                    m_fLastMousePosY = 0;
                }
            }
            else if (Input.GetMouseButton(1)) //鼠标右键处于按下状态中  
            {
                if (m_bMouseRightKeyDown)
                {
                    Vector3 kMousePos = Input.mousePosition;
                    float fDeltaX = kMousePos.x - m_fLastMousePosX;
                    float fDeltaY = kMousePos.y - m_fLastMousePosY;
                    m_fLastMousePosX = kMousePos.x;
                    m_fLastMousePosY = kMousePos.y;


                    Vector3 kNewEuler = transform.eulerAngles;
                    kNewEuler.x += (fDeltaY * rotateSpeed);
                    kNewEuler.y += -(fDeltaX * rotateSpeed);
                    transform.eulerAngles = kNewEuler;
                }
            }
        }


        //judge translation
        float fMoveDeltaX = 0.0f;
		float fMoveDeltaZ = 0.0f;
		float fDeltaTime = Time.deltaTime;
		if (Input.GetKey (KeyCode.A)) {
			fMoveDeltaX -= moveSpeed * fDeltaTime;
		}
		if (Input.GetKey (KeyCode.D)) {
			fMoveDeltaX += moveSpeed * fDeltaTime;
		}
		if (Input.GetKey (KeyCode.W)) {
			fMoveDeltaZ += moveSpeed * fDeltaTime;
		}
		if (Input.GetKey (KeyCode.S)) {
			fMoveDeltaZ -= moveSpeed * fDeltaTime;
		}
		if (fMoveDeltaX != 0.0f || fMoveDeltaZ != 0.0f) {
			Vector3 kForward = transform.forward;
			Vector3 kRight = Vector3.Cross (kUpDirection, kForward);
			Vector3 kNewPos = transform.position;
            Vector3 kOldPos = kNewPos;
			kNewPos += kRight * fMoveDeltaX;
			kNewPos += kForward * fMoveDeltaZ;
            //transform.position = kNewPos;
            transform.position = Vector3.Lerp(kOldPos, kNewPos, 0.1f);
		}
	}
}