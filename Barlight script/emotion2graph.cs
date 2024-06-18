using UnityEngine;

public class emotion2graph : MonoBehaviour
{
    public emotion2 emotion2; // ���� ColorEmotion �ű�
    public LineRenderer emotionLine;

    public int numberOfPoints = 4; // ͼ�ϵĵ���
    public float graphWidth = 10f; // ͼ�Ŀ��
    public float imageHeight = 227f; // ͼ��ĸ߶�

    void Start()
    {
        if (emotion2 == null)
        {
            Debug.LogError("����� ColorEmotion �ű���");
            return;
        }

        InitializeLineRenderer(emotionLine);
    }

    void Update()
    {
        if (emotion2 == null)
        {
            Debug.LogError("����� ColorEmotion �ű���");
            return;
        }

        // ��ȡ�������ֵ
        float[] emotionValues = new float[numberOfPoints];
        emotionValues[0] = emotion2.ActivePassive;
        emotionValues[1] = emotion2.WarmCool;
        emotionValues[2] = emotion2.HeavyLight;
        emotionValues[3] = emotion2.HardSoft;

        // ���� LineRenderer ��λ��
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
            float y = values[i] * (imageHeight / 2); // �����������ֵ���� y ֵ

            Vector3 position = new Vector3(x, y, 0f);
            emotionLine.SetPosition(i, position);
        }
    }
}
