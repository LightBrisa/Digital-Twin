using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.IO;

public class Matlab : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    public static void fit()
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        System.Diagnostics.Process process = System.Diagnostics.Process.Start(
            System.Environment.CurrentDirectory + "\\fit.exe",
            "'S.txt' 'Y.txt' 0.01 0.0001 100");
        process.WaitForExit();
        process.Close();

        stopwatch.Stop();
        //Debug.Log("拟合耗时：" + stopwatch.Elapsed);
    }

    public static void predict()
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        System.Diagnostics.Process process = System.Diagnostics.Process.Start(
            System.Environment.CurrentDirectory + "\\predict.exe",
            "'model.mat' 'X.txt' 'RES.txt'");
        process.WaitForExit();
        process.Close();

        stopwatch.Stop();
        //Debug.Log("预测耗时：" + stopwatch.Elapsed);
    }

    public static void predict(string filename)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        System.Diagnostics.Process process = System.Diagnostics.Process.Start(
            System.Environment.CurrentDirectory + "\\predict.exe",
            "model.mat FANHUAYANGBEN.txt " + filename);
        process.WaitForExit();
        process.Close();

        stopwatch.Stop();
        Debug.Log("预测耗时：" + stopwatch.Elapsed);
    }
}