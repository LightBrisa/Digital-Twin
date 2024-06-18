using UnityEngine;
using UnityEngine.UI;

public class ColorEmotion : MonoBehaviour
{
    public RawImage rawImage; // 输入的图像

    public int step = 1;           // 步长，用于处理图像时的采样步长

    const float param_13 = 1.0f / 3.0f;
    const float param_16116 = 16.0f / 116.0f;
    const float Xn = 0.950456f;
    const float Yn = 1.0f;
    const float Zn = 1.088754f;

    public struct ColorXYZ
    {
        public float x;
        public float y;
        public float z;
        public ColorXYZ(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }
    }

    public struct ColorLab
    {
        public float L;
        public float a;
        public float b;
        public ColorLab(float L, float a, float b)
        {
            this.L = L;
            this.a = a;
            this.b = b;
        }
    }

    public float ActivePassive;
    public float WarmCool;
    public float HeavyLight;
    public float HardSoft;

    struct ValueData
    {
        public float _value;
    }

    // 声明一个事件
    public event System.Action EmotionValuesUpdated;

    void Update()
    {
        if (rawImage == null)
        {
            Debug.LogError("请在Inspector中指定输入图像纹理。");
            return;
        }

        // 从输入的图像中计算情感属性
        float activePassive = Active_Passive((Texture2D)rawImage.texture, step);
        float warmCool = Warm_Cool((Texture2D)rawImage.texture, step);
        float heavyLight = Heavy_Light((Texture2D)rawImage.texture, step);
        float hardSoft = Hard_Soft((Texture2D)rawImage.texture, step);

        // 将计算结果赋给属性
        ActivePassive = activePassive;
        WarmCool = warmCool;
        HeavyLight = heavyLight;
        HardSoft = hardSoft;

        // 在值更新后触发事件
        if (EmotionValuesUpdated != null)
        {
            EmotionValuesUpdated.Invoke();
        }
    }

    public Vector4 GetEmotionValues()
    {
        return new Vector4(ActivePassive, WarmCool, HeavyLight, HardSoft);
    }

    public static float gamma(float x)
    {
        return x > 0.04045 ? Mathf.Pow((x + 0.055f) / 1.055f, 2.4f) : x / 12.92f;
    }

    private ColorLab RGBToLab(Color c)
    {
        float B = gamma(c.b);
        float G = gamma(c.g);
        float R = gamma(c.r);
        float X = 0.412453f * R + 0.357580f * G + 0.180423f * B;
        float Y = 0.212671f * R + 0.715160f * G + 0.072169f * B;
        float Z = 0.019334f * R + 0.119193f * G + 0.950227f * B;

        X /= 0.95047f;
        Y /= 1.0f;
        Z /= 1.08883f;

        float FX = X > 0.008856f ? Mathf.Pow(X, 1.0f / 3.0f) : (7.787f * X + 0.137931f);
        float FY = Y > 0.008856f ? Mathf.Pow(Y, 1.0f / 3.0f) : (7.787f * Y + 0.137931f);
        float FZ = Z > 0.008856f ? Mathf.Pow(Z, 1.0f / 3.0f) : (7.787f * Z + 0.137931f);
        float L = Y > 0.008856f ? (116.0f * FY - 16.0f) : (903.3f * Y);
        float a = 500f * (FX - FY);
        float b = 200f * (FY - FZ);

        return new ColorLab(L, a, b);
    }

    public float C(ColorLab c)
    {
        return Mathf.Sqrt(c.a * c.a + c.b * c.b);
    }

    public float h(ColorLab c)
    {
        if (c.a == 0 && c.b == 0)
            return 0;
        if (c.a == 0 && c.b > 0)
            return Mathf.PI / 2;
        if (c.a == 0 && c.b < 0)
            return -Mathf.PI / 2;
        return Mathf.Atan(c.b / c.a);
    }

    // 活动
    public float Active_Passive(ColorLab c)
    {
        float deltaC = C(c);
        float deltaL = c.L - 50;
        return -1.1f + 0.03f * Mathf.Sqrt(Mathf.Pow(deltaC, 2) + Mathf.Pow(deltaL / 1.5f, 2));
    }

    // 热量
    public float Warm_Cool(ColorLab c)
    {
        return -0.5f + 0.02f * Mathf.Pow(C(c), 1.07f) * Mathf.Cos(h(c) - 50 * Mathf.PI / 180);
    }

    // 重量
    public float Heavy_Light(ColorLab c)
    {
        return -2.1f + 0.05f * (100 - c.L);
    }

    public float Hard_Soft(ColorLab c)
    {
        return 11.1f + 0.03f * (100 - c.L) - 11.4f * Mathf.Pow((C(c)), 0.02f);
    }

    // 其他情感属性计算的函数

    public float Active_Passive(Texture2D texture, int step = 1)
    {
        float value = 0;
        for (int i = 0; i < texture.width; i += step)
        {
            for (int j = 0; j < texture.height; j += step)
            {
                Color color = texture.GetPixel(i, j);
                value += Active_Passive(RGBToLab(color));
            }
        }
        return value * step * step / texture.width / texture.width;
    }

    public float Warm_Cool(Texture2D texture, int step = 1)
    {
        float value = 0;
        for (int i = 0; i < texture.width; i += step)
        {
            for (int j = 0; j < texture.height; j += step)
            {
                Color color = texture.GetPixel(i, j);
                value += Warm_Cool(RGBToLab(color));
            }
        }
        return value * step * step / texture.width / texture.width;
    }

    public float Heavy_Light(Texture2D texture, int step = 1)
    {
        float value = 0;
        for (int i = 0; i < texture.width; i += step)
        {
            for (int j = 0; j < texture.height; j += step)
            {
                Color color = texture.GetPixel(i, j);
                value += Heavy_Light(RGBToLab(color));
            }
        }
        return value * step * step / texture.width / texture.width;
    }

    public float Hard_Soft(Texture2D texture, int step = 1)
    {
        float value = 0;
        for (int i = 0; i < texture.width; i += step)
        {
            for (int j = 0; j < texture.height; j += step)
            {
                Color color = texture.GetPixel(i, j);
                value += Hard_Soft(RGBToLab(color));
            }
        }
        return value * step * step / texture.width / texture.width;
    }
}
