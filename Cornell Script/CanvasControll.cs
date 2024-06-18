using UnityEngine;

public class CanvasControll : MonoBehaviour
{
    private Canvas canvas;

    private void Start()
    {
        // 获取Canvas组件
        canvas = GetComponent<Canvas>();

        // 初始时将Canvas隐藏
        canvas.enabled = false;
    }

    private void Update()
    {
        // 检查按下的键，例如空格键
        if (Input.GetKeyDown(KeyCode.Space))
        {
            // 切换Canvas的可见性
            canvas.enabled = !canvas.enabled;
        }
    }
}
