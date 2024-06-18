using UnityEngine.UI;
using UnityEngine;
using System;

public class rendertexturereader2 : MonoBehaviour
{
    public RawImage rawImage;
    public Camera mainCamera;

    private RenderTexture renderTexture;
    private Texture2D convertedTexture;

    public static event Action<Texture2D> OnTextureUpdated;

    private void Start()
    {
        // 设置摄像头的目标纹理尺寸
        int targetWidth = 327;
        int targetHeight = 245;
        renderTexture = new RenderTexture(targetWidth, targetHeight, 24);
        mainCamera.targetTexture = renderTexture;

        convertedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGB24, false);
        rawImage.texture = convertedTexture;

        // 在这里订阅事件
        OnTextureUpdated += HandleTextureUpdated;
    }

    private void LateUpdate()
    {
        // 确保摄像机已经渲染完毕后再读取纹理
        mainCamera.Render();

        RenderTexture.active = renderTexture;
        convertedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        convertedTexture.Apply();
        RenderTexture.active = null;

        rawImage.texture = convertedTexture;

        // 触发纹理更新事件
        OnTextureUpdated?.Invoke(convertedTexture);

        // 添加输出语句
        Debug.Log("LateUpdate Executed");
        Debug.Log("Texture Updated");
    }

    private void HandleTextureUpdated(Texture2D updatedTexture)
    {
        // 在这里处理纹理更新事件
        Debug.Log("Handling Texture Updated Event");
    }

    private void OnDestroy()
    {
        // 在对象销毁时取消事件订阅，以防止内存泄漏
        OnTextureUpdated -= HandleTextureUpdated;
    }
}
