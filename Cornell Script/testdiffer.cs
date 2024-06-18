using UnityEngine;
using UnityEngine.UI;

public class testdiffer : MonoBehaviour
{
    public RawImage rawImage1;
    public RawImage rawImage2;

    void Start()
    {
        Texture2D texture1 = ConvertToTexture2D(rawImage1.texture);
        Texture2D texture2 = ConvertToTexture2D(rawImage2.texture);

        if (texture1 != null && texture2 != null)
        {
            int targetWidth = Mathf.Min(texture1.width, texture2.width);
            int targetHeight = Mathf.Min(texture1.height, texture2.height);
            Texture2D resizedTexture1 = ResizeTexture2D(texture1, targetWidth, targetHeight);
            Texture2D resizedTexture2 = ResizeTexture2D(texture2, targetWidth, targetHeight);

            Texture2D differenceTexture = CalculateColorDifference(resizedTexture1, resizedTexture2);
            // 这里可以根据需要对 differenceTexture 进行进一步处理
        }
        else
        {
            Debug.LogError("Unable to convert one of the textures.");
        }
    }

    private Texture2D ConvertToTexture2D(Texture texture)
    {
        if (texture is Texture2D tex2D)
        {
            return tex2D;
        }
        else if (texture is RenderTexture rTex)
        {
            Texture2D tempTexture = new Texture2D(rTex.width, rTex.height, TextureFormat.RGB24, false);
            RenderTexture.active = rTex;
            tempTexture.ReadPixels(new Rect(0, 0, rTex.width, rTex.height), 0, 0);
            tempTexture.Apply();
            RenderTexture.active = null;
            return tempTexture;
        }
        return null;
    }

    private Texture2D ResizeTexture2D(Texture2D source, int width, int height)
    {
        RenderTexture rt = RenderTexture.GetTemporary(width, height);
        RenderTexture.active = rt;
        Graphics.Blit(source, rt);
        Texture2D result = new Texture2D(width, height, TextureFormat.RGB24, false);
        result.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        result.Apply();
        RenderTexture.active = null;
        RenderTexture.ReleaseTemporary(rt);
        return result;
    }

    private Texture2D CalculateColorDifference(Texture2D texture1, Texture2D texture2)
    {
        int width = texture1.width;
        int height = texture1.height;
        Texture2D differenceTexture = new Texture2D(width, height);

        float totalDiff = 0;

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

                float grayScaleValue = diffMagnitude / Mathf.Sqrt(3);
                totalDiff += grayScaleValue;

                Color differenceColor = new Color(grayScaleValue, grayScaleValue, grayScaleValue, 1.0f);
                differenceTexture.SetPixel(x, y, differenceColor);
            }
        }

        differenceTexture.Apply();

        float averageDiff = totalDiff / (width * height);
        Debug.Log($"Average color difference: {averageDiff}");

        return differenceTexture;
    }
}
