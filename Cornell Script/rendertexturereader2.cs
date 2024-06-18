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
        // ��������ͷ��Ŀ������ߴ�
        int targetWidth = 327;
        int targetHeight = 245;
        renderTexture = new RenderTexture(targetWidth, targetHeight, 24);
        mainCamera.targetTexture = renderTexture;

        convertedTexture = new Texture2D(targetWidth, targetHeight, TextureFormat.RGB24, false);
        rawImage.texture = convertedTexture;

        // �����ﶩ���¼�
        OnTextureUpdated += HandleTextureUpdated;
    }

    private void LateUpdate()
    {
        // ȷ��������Ѿ���Ⱦ��Ϻ��ٶ�ȡ����
        mainCamera.Render();

        RenderTexture.active = renderTexture;
        convertedTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        convertedTexture.Apply();
        RenderTexture.active = null;

        rawImage.texture = convertedTexture;

        // ������������¼�
        OnTextureUpdated?.Invoke(convertedTexture);

        // ���������
        Debug.Log("LateUpdate Executed");
        Debug.Log("Texture Updated");
    }

    private void HandleTextureUpdated(Texture2D updatedTexture)
    {
        // �����ﴦ����������¼�
        Debug.Log("Handling Texture Updated Event");
    }

    private void OnDestroy()
    {
        // �ڶ�������ʱȡ���¼����ģ��Է�ֹ�ڴ�й©
        OnTextureUpdated -= HandleTextureUpdated;
    }
}
