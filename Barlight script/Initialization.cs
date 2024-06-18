using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Initialization : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GroupController.Init();
        ColorSystem.moodShader = Resources.Load<ComputeShader>("MoodShader");
    }

    // Update is called once per frame
    void Update()
    {

    }
}
