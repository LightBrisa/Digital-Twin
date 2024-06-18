using UnityEngine;

public class ColorSystem : MonoBehaviour
{
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

    public static float gamma(float x)
    {
        return x > 0.04045 ? Mathf.Pow((x + 0.055f) / 1.055f, 2.4f) : x / 12.92f;
    }

    public static ColorLab RGBToLab(Color c)
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

    static public float C(ColorLab c)
    {
        return Mathf.Sqrt(c.a * c.a + c.b * c.b);
    }

    static public float h(ColorLab c)
    {
        if (c.a == 0 && c.b == 0)
            return 0;
        if (c.a == 0 && c.b > 0)
            return Mathf.PI / 2;
        if (c.a == 0 && c.b < 0)
            return -Mathf.PI / 2;
        return Mathf.Atan(c.b / c.a);
    }

    //活动
    static public float Active_Passive(ColorLab c)
    {
        float deltaC = C(c);
        float deltaL = c.L - 50;
        return -1.1f + 0.03f * Mathf.Sqrt(Mathf.Pow(deltaC, 2) + Mathf.Pow(deltaL / 1.5f, 2));
    }
    //热量
    static public float Warm_Cool(ColorLab c)
    {
        return -0.5f + 0.02f * Mathf.Pow(C(c), 1.07f) * Mathf.Cos(h(c) - 50 * Mathf.PI / 180);
    }
    //重量
    static public float Heavy_Light(ColorLab c)
    {
        return -2.1f + 0.05f * (100 - c.L);
    }

    

    static public float Hard_Soft(ColorLab c)
    {
        return 11.1f + 0.03f * (100 - c.L) - 11.4f * Mathf.Pow((C(c)), 0.02f);
    }

    static public float Warm_Cool(Texture2D texture, int step = 1)
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

    static public float Heavy_Light(Texture2D texture, int step = 1)
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

    static public float Active_Passive(Texture2D texture, int step = 1)
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

    static public float Hard_Soft(Texture2D texture, int step = 1)
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

    public static ComputeShader moodShader;

    struct ValueData
    {
        public float _value;
    }


    public static bool isWarm = true;
    public static bool isActive = true;
    public static bool isHeavy = true;

    static public float MoodEnergy(Texture2D texture)
    {
        float value = 0;
        int x = texture.width / 32;
        int y = texture.height / 32;
        int kernel = moodShader.FindKernel("MoodKernel");
        ValueData[] output = new ValueData[x * y];
        ComputeBuffer outputbuffer = new ComputeBuffer(output.Length, 4);

        moodShader.SetTexture(kernel, "image", texture);
        moodShader.SetBuffer(kernel, "valueBuffer", outputbuffer);
        moodShader.SetInt("WIDTH", texture.width);
        moodShader.SetInt("HEIGHT", texture.height);
        moodShader.SetBool("isWarm", isWarm);
        moodShader.SetBool("isActive", isActive);
        moodShader.SetBool("isHeavy", isHeavy);

        moodShader.Dispatch(kernel, x, y, 1);
        outputbuffer.GetData(output);


        for (int k = 0; k < output.Length; k++)
            value += output[k]._value;
        outputbuffer.Release();

        return value / x / y;
    }
}