using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class GA1 : MonoBehaviour
{
    public RawImage leftImage;
    public RenderTextureReader renderTextureReader;
    public GameObject targetCube;
    private Material targetMaterial;
    public int populationSize = 100;
    public float mutationRate = 0.2f;
    public int maxGenerations = 100;
    private List<Color> population = new List<Color>();
    private Dictionary<Color, float> fitnessMap = new Dictionary<Color, float>();

    private void Start()
    {
        renderTextureReader = FindObjectOfType<RenderTextureReader>();
        targetMaterial = targetCube.GetComponent<Renderer>().material;
        StartCoroutine(StartOptimization());
    }

    private IEnumerator StartOptimization()
    {
        yield return null;

        InitializePopulation();

        for (int generation = 0; generation < maxGenerations; generation++)
        {
            yield return EvaluatePopulation();
            SelectAndReproduce();

            Color bestColor = GetBestColor();
            ApplyColorToMaterial(bestColor);
            yield return new WaitForEndOfFrame();

            float colorDifference = CalculateColorDifference(bestColor);
            Debug.Log($"Generation {generation + 1} - Best Color: {bestColor}, Color Difference: {colorDifference}");
        }

        Color finalBestColor = GetBestColor();
        ApplyColorToMaterial(finalBestColor);
        float finalColorDifference = CalculateColorDifference(finalBestColor);
        Debug.Log($"Optimization complete. Best Color: {finalBestColor}, Final Color Difference: {finalColorDifference}");
    }

    private void InitializePopulation()
    {
        population.Clear();
        for (int i = 0; i < populationSize; i++)
        {
            population.Add(Random.ColorHSV());
        }
    }

    private IEnumerator EvaluatePopulation()
    {
        fitnessMap.Clear();

        foreach (Color color in population)
        {
            ApplyColorToMaterial(color);
            yield return new WaitForEndOfFrame();

            float colorDifference = CalculateColorDifference(color);
            // 使用色差的倒数的平方作为适应度
            fitnessMap[color] = 1.0f / Mathf.Pow(1.0f + colorDifference, 2);
        }
    }


    private float CalculateColorDifference(Color color)
    {
        Texture2D leftTexture = ResizeTexture2D(leftImage.texture as Texture2D, renderTextureReader.convertedTexture.width, renderTextureReader.convertedTexture.height);
        Texture2D rightTexture = renderTextureReader.convertedTexture;

        return CalculateColorDifference(leftTexture, rightTexture);
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

    private float CalculateColorDifference(Texture2D leftTexture, Texture2D rightTexture)
    {
        float colorDifference = 0f;

        int endX = leftTexture.width / 2; // 修改为只遍历左半部分
        for (int y = 0; y < leftTexture.height; y++)
        {
            for (int x = 0; x < endX; x++) // 修改循环条件为只遍历到一半宽度
            {
                Color leftPixel = leftTexture.GetPixel(x, y);
                Color rightPixel = rightTexture.GetPixel(x, y);

                Vector3 colorDiff = new Vector3(
                    Mathf.Abs(leftPixel.r - rightPixel.r),
                    Mathf.Abs(leftPixel.g - rightPixel.g),
                    Mathf.Abs(leftPixel.b - rightPixel.b));

                colorDifference += colorDiff.magnitude;
            }
        }

        // 注意这里的总像素数也需要相应调整，现在是基于一半宽度计算的
        colorDifference /= (endX * leftTexture.height);

        return colorDifference;
    }



    private void SelectAndReproduce()
    {
        List<Color> newPopulation = new List<Color>();

        for (int i = 0; i < populationSize; i++)
        {
            Color parent1 = RouletteWheelSelection();
            Color parent2 = RouletteWheelSelection();

            Color offspring = SinglePointCrossover(parent1, parent2);
            offspring = Mutate(offspring);

            newPopulation.Add(offspring);
        }

        population = newPopulation;
    }

    private Color RouletteWheelSelection()
    {
        float totalFitness = 0f;
        foreach (var pair in fitnessMap)
        {
            totalFitness += pair.Value;
        }

        float randomValue = Random.Range(0f, totalFitness);
        float sum = 0f;

        foreach (var pair in fitnessMap)
        {
            sum += pair.Value;
            if (sum >= randomValue)
            {
                return pair.Key;
            }
        }

        return population[population.Count - 1]; // Fallback in case of rounding errors
    }

    private Color SinglePointCrossover(Color parent1, Color parent2)
    {
        int crossoverPoint = Random.Range(0, 4); // Assuming RGBA color

        float r = crossoverPoint <= 0 ? parent1.r : parent2.r;
        float g = crossoverPoint <= 1 ? parent1.g : parent2.g;
        float b = crossoverPoint <= 2 ? parent1.b : parent2.b;
        float a = crossoverPoint <= 3 ? parent1.a : parent2.a;

        return new Color(r, g, b, a);
    }

    private Color Mutate(Color color)
    {
        if (Random.value < mutationRate)
        {
            float r = Mathf.Clamp(color.r + Random.Range(-0.1f, 0.1f), 0f, 1f);
            float g = Mathf.Clamp(color.g + Random.Range(-0.1f, 0.1f), 0f, 1f);
            float b = Mathf.Clamp(color.b + Random.Range(-0.1f, 0.1f), 0f, 1f);
            float a = Mathf.Clamp(color.a + Random.Range(-0.1f, 0.1f), 0f, 1f);

            return new Color(r, g, b, a);
        }

        return color;
    }

    private Color GetBestColor()
    {
        Color bestColor = new Color(0f, 120f / 255f, 0f, 1f); // Initialize with a default color, e.g., green
        float bestFitness = 0f;

        foreach (var pair in fitnessMap)
        {
            if (pair.Value > bestFitness)
            {
                bestFitness = pair.Value;
                bestColor = pair.Key;
            }
        }

        return bestColor;
    }

    private void ApplyColorToMaterial(Color color)
    {
        targetMaterial.color = color;
    }
}
