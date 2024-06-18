using UnityEngine;

public class emotion2graph : MonoBehaviour
{
    public emotion2 emotion2; // 引用 ColorEmotion 脚本
    public LineRenderer emotionLine;

    public int numberOfPoints = 4; // 图上的点数
    public float graphWidth = 10f; // 图的宽度
    public float imageHeight = 227f; // 图像的高度

    void Start()
    {
        if (emotion2 == null)
        {
            Debug.LogError("请分配 ColorEmotion 脚本！");
            return;
        }

        InitializeLineRenderer(emotionLine);
    }

    void Update()
    {
        if (emotion2 == null)
        {
            Debug.LogError("请分配 ColorEmotion 脚本！");
            return;
        }

        // 获取情感属性值
        float[] emotionValues = new float[numberOfPoints];
        emotionValues[0] = emotion2.ActivePassive;
        emotionValues[1] = emotion2.WarmCool;
        emotionValues[2] = emotion2.HeavyLight;
        emotionValues[3] = emotion2.HardSoft;

        // 更新 LineRenderer 的位置
        DrawLineToImage(emotionValues);
    }

    void InitializeLineRenderer(LineRenderer lineRenderer)
    {
        lineRenderer.positionCount = numberOfPoints;
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
    }

    void DrawLineToImage(float[] values)
    {
        for (int i = 0; i < numberOfPoints; i++)
        {
            float t = i / (float)(numberOfPoints - 1);
            float x = Mathf.Lerp(-graphWidth / 2f, graphWidth / 2f, t);
            float y = values[i] * (imageHeight / 2); // 根据情感属性值计算 y 值

            Vector3 position = new Vector3(x, y, 0f);
            emotionLine.SetPosition(i, position);
        }
    }
}
