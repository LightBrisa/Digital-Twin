using UnityEngine;
using UnityEngine.UI;

public class ColorDifference : MonoBehaviour
{
    public RawImage sourceImage1;
    public RawImage sourceImage2;
    public RawImage colorDifferenceImage;
    public float scaleFactor = 1.0f;

    private Texture2D texture1;
    private Texture2D texture2;
    private Texture2D differenceTexture;

    private void OnEnable()
    {
        rendertexturereader2.OnTextureUpdated += HandleTextureUpdated;
    }

    private void OnDisable()
    {
        rendertexturereader2.OnTextureUpdated -= HandleTextureUpdated;
    }

    private void Start()
    {
        if (sourceImage1 == null || sourceImage2 == null || colorDifferenceImage == null)
        {
            Debug.LogError("请分配所有 RawImage 组件！");
            return;
        }

        texture1 = GetTextureFromRawImage(sourceImage1);
        texture2 = GetTextureFromRawImage(sourceImage2);

        int targetWidth = 327;
        int targetHeight = 245;
        texture1 = ResizeTexture(texture1, targetWidth, targetHeight);
        texture2 = ResizeTexture(texture2, targetWidth, targetHeight);

        differenceTexture = CalculateColorDifference(texture1, texture2);
        DisplayDifferenceImage(differenceTexture);
    }

    private void HandleTextureUpdated(Texture2D updatedTexture)
    {
        texture2 = updatedTexture;
        differenceTexture = CalculateColorDifference(texture1, texture2);
        DisplayDifferenceImage(differenceTexture);
    }

    private Texture2D ResizeTexture(Texture2D source, int newWidth, int newHeight)
    {
        Texture2D resizedTexture = new Texture2D(newWidth, newHeight);
        Graphics.ConvertTexture(source, resizedTexture);
        return resizedTexture;
    }

    private Texture2D GetTextureFromRawImage(RawImage rawImage)
    {
        Texture sourceTexture = rawImage.texture;
        RenderTexture renderTexture = new RenderTexture(sourceTexture.width, sourceTexture.height, 0);
        Graphics.Blit(sourceTexture, renderTexture);

        Texture2D texture = new Texture2D(renderTexture.width, renderTexture.height);
        RenderTexture.active = renderTexture;
        texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture.Apply();
        RenderTexture.active = null;

        return texture;
    }

    private Texture2D CalculateColorDifference(Texture2D texture1, Texture2D texture2)
    {
        int width = texture1.width;
        int height = texture1.height;
        Texture2D differenceTexture = new Texture2D(width, height);

        float totalDiff = 0; // 用于计算平均色差

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Color color1 = texture1.GetPixel(x, y);
                Color color2 = texture2.GetPixel(x, y);

                float diffMagnitude = Mathf.Sqrt(
                    Mathf.Pow(color1.r - color2.r, 2) +
                    Mathf.Pow(color1.g - color2.g, 2) +
                    Mathf.Pow(color1.b - color2.b, 2));

                float grayScaleValue = diffMagnitude / Mathf.Sqrt(3); // 根据最大可能的色差进行归一化
                totalDiff += grayScaleValue;

                Color differenceColor = new Color(grayScaleValue, grayScaleValue, grayScaleValue, 1.0f);
                differenceTexture.SetPixel(x, y, differenceColor);
            }
        }

        float averageDiff = totalDiff / (width * height); // 计算平均色差
        Debug.Log($"平均色差值: {averageDiff}");

        differenceTexture.Apply();
        return differenceTexture;
    }

    private void DisplayDifferenceImage(Texture2D differenceTexture)
    {
        Sprite sprite = Sprite.Create(differenceTexture, new Rect(0, 0, differenceTexture.width, differenceTexture.height), new Vector2(0.5f, 0.5f));
        colorDifferenceImage.texture = sprite.texture;
        RectTransform rectTransform = colorDifferenceImage.rectTransform;
        rectTransform.sizeDelta = new Vector2(differenceTexture.width * scaleFactor, differenceTexture.height * scaleFactor);
    }
}
