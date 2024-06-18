using UnityEngine;

public class CanvasControll : MonoBehaviour
{
    private Canvas canvas;

    private void Start()
    {
        // ��ȡCanvas���
        canvas = GetComponent<Canvas>();

        // ��ʼʱ��Canvas����
        canvas.enabled = false;
    }

    private void Update()
    {
        // ��鰴�µļ�������ո��
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // �л�Canvas�Ŀɼ���
            canvas.enabled = !canvas.enabled;
        }
    }
}
