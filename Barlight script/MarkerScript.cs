using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerScript : MonoBehaviour {

    Vector3 oldPos;
    float radian = 0;  
    float perRadian = 0.03f;
    float radius = 0.1f;

    // Use this for initialization
    void Start () {
        oldPos = transform.position;
    }
	
	// Update is called once per frame
	void Update () {
        radian += perRadian;
        float dy = Mathf.Cos(radian) * radius;
        transform.position = oldPos + new Vector3(0, dy, 0);

        if (Camera.main != null)
        {
            transform.LookAt(Camera.main.transform.position, Vector3.up);
        }
	}

    //点击显示cubemap和色环
    private void OnMouseDown()
    {
        
    }
}
