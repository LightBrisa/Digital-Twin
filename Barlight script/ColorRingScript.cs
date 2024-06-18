using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class ColorRingScript : MonoBehaviour {

    ColorHarmonizationScript script;
    Camera cam;

    // Use this for initialization
    void Start () {
        cam = GameObject.Find("UICamera").GetComponent<Camera>();
        script = GameObject.Find("Camera").GetComponent<ColorHarmonizationScript>();
    }
	
	// Update is called once per frame
	void Update () {
        
    }

    // 本模型使用URP渲染
    // 在URP下，OnPostRender不会被自动执行 所以需要下列其他方法
    void OnEnable()
    {
        RenderPipelineManager.endCameraRendering += EndCameraRendering;
    }

    void OnDisable()
    {
        RenderPipelineManager.endCameraRendering -= EndCameraRendering;
    }

    void EndCameraRendering(ScriptableRenderContext context, Camera camera)
    {
        OnPostRender();
    }

    void OnPostRender()
    {
        // Debug.Log("ColorRing OnPostRender check");
        if (script.draw_circle && cam.enabled)
        {
            script.Change();
        }
    }


    //public void ColorRender()
    //{
    //    // Debug.Log("ColorRing OnPostRender check");
    //    if (script.draw_circle && cam.enabled)
    //    {
    //        script.Change();

    //    }
    //}
}
