using UnityEngine;
using UnityEngine.UI;

public class TextureDisplay : MonoBehaviour
{
    public RawImage rawImage;   // 在Inspector面板中关联RawImage组件
    public Texture2D displayTexture;  // 在Inspector面板中关联要显示的Texture
    private bool isTextureVisible = false;

    void Start()
    {
        // 确保RawImage组件和Button组件都不为null
        if (rawImage == null)
        {
            Debug.LogError("RawImage component not assigned!");
            return;
        }

        Button button = GetComponent<Button>();
        if (button == null)
        {
            Debug.LogError("Button component not found!");
            return;
        }

        // 添加按钮点击事件监听器
        button.onClick.AddListener(ToggleTextureDisplay);
    }

    void ToggleTextureDisplay()
    {
        // 切换纹理的可见性
        isTextureVisible = !isTextureVisible;

        // 根据可见性设置RawImage的纹理
        if (isTextureVisible)
        {
            rawImage.texture = displayTexture;
        }
        else
        {
            rawImage.texture = null; // 隐藏纹理
        }
    }
}
