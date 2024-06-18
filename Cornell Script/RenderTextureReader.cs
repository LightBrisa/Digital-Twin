using UnityEngine;
using UnityEngine.UI;

public class RenderTextureReader : MonoBehaviour
{
    public RawImage rawImage;
    public Camera mainCamera;
    public Texture2D convertedTexture;
    private void Awake()
    {
        RenderTexture renderTexture = new RenderTexture(Screen.width / 2, Screen.height, 24);
        mainCamera.targetTexture = renderTexture;
        convertedTexture = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        rawImage.texture = convertedTexture;
    }
    private void LateUpdate()
    {
        // 确保摄像机已经渲染完毕后再读取纹理
        mainCamera.Render();
        RenderTexture renderTexture = mainCamera.targetTexture;
        RenderTexture.active = renderTexture;
        convertedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        convertedTexture.Apply();
        RenderTexture.active = null;
        rawImage.texture = convertedTexture;
    }
}
