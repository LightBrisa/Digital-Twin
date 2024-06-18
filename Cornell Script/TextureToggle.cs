using UnityEngine;
using UnityEngine.UI;

public class TextureToggle : MonoBehaviour
{
    public Button toggleButton; // 用于触发覆盖和取消覆盖操作的按钮
    public RawImage targetImage; // 要被覆盖纹理的目标RawImage
    public RawImage sourceImage; // 源RawImage，其纹理将覆盖到目标RawImage上

    private bool isTextureOverlaid = false; // 标志当前是否已经覆盖了纹理
    private Texture originalTexture; // 用于保存目标RawImage原始纹理的引用

    void Start()
    {
        originalTexture = targetImage.texture; // 保存原始纹理
        toggleButton.onClick.AddListener(ToggleTexture); // 为按钮点击事件注册监听器
    }

    void ToggleTexture()
    {
        if (!isTextureOverlaid)
        {
            // 如果当前未覆盖纹理，则用源RawImage的纹理覆盖到目标RawImage上
            targetImage.texture = sourceImage.texture;
            isTextureOverlaid = true;
        }
        else
        {
            // 如果当前已覆盖纹理，则恢复到原始纹理
            targetImage.texture = originalTexture;
            isTextureOverlaid = false;
        }
    }
}
