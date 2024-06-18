using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;

struct DistanceData
{
    public float _distance;
};


public class ColorHarmonizationScript : MonoBehaviour
{
  
    public bool test = false;
    public Texture2D testTexture;

    public ComputeShader computeShader;
    ComputeBuffer outputbuffer;

    private Camera _camera;//UICamera
    public Texture2D texture;//texture of image
    public Texture2D final;

    //face_weights array add by Juncong Lin to control the contribution of each cubemap face of a camera on harmony
    static float[] face_weights = { 1.0f, 1.0f, 1.0f, 1.0f, 1.0f, 1.0f };

    static int width = 512;//image's properties
    //static int width = 512;
    static int height = 384;

    static float step = 1.0f;//divide into 3600 units
    static float radius = 6.0f;//outer circle
    static float _radius = 4.50f;//inner circle

    static float[] Arcs1 = { 18.0f, 93.6f, 18.0f, 18.0f, 180.0f, 93.6f, 93.6f, 0.0f };//first sector
    static float[] Arcs2 = { 0.0f, 0.0f, 79.2f, 18.0f, 0.0f, 18.0f, 93.6f, 0.0f };//second sector
    static float[] Offset = { 0.0f, 0.0f, 90.0f, 180.0f, 0.0f, 180.0f, 180.0f, 0.0f };//space between two sector 
    static char[] names = { 'i', 'V', 'L', 'I', 'T', 'Y', 'X', 'N' };//names of templates

    List<float> histogram = new List<float>();//hsv information

    Vector3 point1, _point1, point2, _point2;//draw variables
    float x, y, _x, _y;
    private static Material lineMaterial;

    float h, s = 1.0f, v = 1.0f;//hsv

    static Color sector_color = new Color(0.8f, 0.8f, 0.8f, 0.65f);//sector color

    Vector2[] ring = new Vector2[8];//color rings' centre

    float start_x = 758.5f;//changeable variables(x,y)
    float start_y = 433.0f;

    float[] alpha = new float[8];//best fit rotation
    float[] distance = new float[8];//minimum distance

    public float min_distance;
    public float energy; //能量 
    public int best_template;

    public bool draw_circle;

    public int num_camera = 0;
    public int num_facet = 0;
    private int current;
    //指示对应的面


    public List<Texture2D> textures = new List<Texture2D>();

    private void Start()
    {

        _camera = GameObject.Find("UICamera").GetComponent<Camera>();//get camera and image objects
        draw_circle = false;
        for (int i = 0; i < 8; i++)
        {
            //set positions
            if (i < 4)
                ring[i] = new Vector2(start_x - 30 + i * 15.0f, start_y);
            else
                ring[i] = new Vector2(start_x - 30 + (i - 4) * 15.0f, start_y - 14.5f);
        }
    }

    public void InitJ()
    {
        float total_energy = 0.0f;
        float total_camer_weight = 0.0f;

        final = GameObject.Find("MapCamera-floor1").GetComponent<MapScript>().finalImage;


        int count = GameObject.Find("MapCamera-floor1").GetComponent<MapScript>().cubemaps.Count;

        Cubemap cubemap;
        textures.Clear();

        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

        for (int m = 0; m < count; m++)
        {
            float camera_energy = 0.0f;

            //calculate camera harmony weight according to the importance of camera position, by Juncong Lin
            float camera_weight = 1.0f;
            total_camer_weight += camera_weight;

            //calculate camera harmony by summing up the harmony of each cubemap face

            cubemap = GameObject.Find("MapCamera-floor1").GetComponent<MapScript>().cubemaps[m];

            for (int k = 0; k < 6; k++)
            {
                //get 6 faces

                //               stopwatch.Start();

                Texture2D screenShot = new Texture2D(cubemap.width, cubemap.height, TextureFormat.ARGB32, false);

                CubemapFace offcialFaces = (CubemapFace)k;

                for (int i = 0; i < cubemap.width; i++)
                {
                    for (int j = 0; j < cubemap.height; j++)
                    {
                        screenShot.SetPixel(i, j, cubemap.GetPixel(offcialFaces, cubemap.width - i, cubemap.height - j));
                    }
                }
                screenShot.Apply();
                texture = screenShot;
                textures.Add(texture);

                //               stopwatch.Stop();
                //               Debug.Log("Time for CubeMap Face[" +k+"] copy: " + stopwatch.Elapsed);

                stopwatch.Start();
                //CalBestDistanceAndAlpha(1);

                //               stopwatch.Stop();
                // Debug.Log("Time for harmony calculation: " + stopwatch.Elapsed);

                //float face_energy = distance[1];

                //camera_energy += face_weights[k] * face_energy;
            }



            //total_energy += camera_weight * camera_energy;
        }

        CalBestDistanceAndAlphaJ();

        //total_energy /= total_camer_weight;

        draw_circle = true;

    }
    public void Init()
    {

        //set texture
        final = GameObject.Find("MapCamera-floor1").GetComponent<MapScript>().finalImage;
        //texture = ReadImage(Application.dataPath + "/CubeMapTextures/Final.png", width, height);
        //texture = ReadImage(Application.dataPath + "/CubeMapTextures/Merge0.png", width, height);
        CalHistogram(final);

        //for (int i = 0; i < 8; i++)
        //{
        //calculate template and alpha
        //CalBestDistanceAndAlpha(i);

        //CalBestDistanceAndAlpha_Greed(i);
        //Debug.Log(i + "->Alpha:" + alpha[i] + ",Distance:" + distance[i]);
        //}
        CalBestDistanceAndAlpha(1);
        PrintBestTemplate();
        draw_circle = true;
    }

    public void Change()
    {
        Texture2D texture = null;

        if (test)
        {
            texture = testTexture;
            CalBestDistanceAndAlphaS();
        }
        else
        {
            current = num_camera * 6 + num_facet;
            texture = textures[current];
        }
        
        CalHistogram(texture);

        

        GameObject.Find("RingCanvas").GetComponentInChildren<RawImage>().texture = texture;

        GameObject.Find("Slider_Cool__Warm").GetComponent<Slider>().value = (ColorSystem.Warm_Cool(texture) + 2) / 4.0f;
        GameObject.Find("Slider_Light__Heavy").GetComponent<Slider>().value = (ColorSystem.Heavy_Light(texture) + 2) / 4.0f;
        GameObject.Find("Slider_Passive__Active").GetComponent<Slider>().value = (ColorSystem.Active_Passive(texture) + 2) / 4.0f;
        //GameObject.Find("Slider_Soft__Hard").GetComponent<Slider>().value = (ColorSystem.Hard_Soft(texture) + 2) / 4.0f;

        //原来的命名
        //GameObject.Find("Slider Warm_Cool").GetComponent<Slider>().value = (ColorSystem.Warm_Cool(texture) + 2) / 4.0f;
        //GameObject.Find("Slider Heavy_Light").GetComponent<Slider>().value = (ColorSystem.Heavy_Light(texture) + 2) / 4.0f;
        //GameObject.Find("Slider Active_Passive").GetComponent<Slider>().value = (ColorSystem.Active_Passive(texture) + 2) / 4.0f;
        //GameObject.Find("Slider Hard_Soft").GetComponent<Slider>().value = (ColorSystem.Hard_Soft(texture) + 2) / 4.0f;


        //Initial
        CreateLineMaterial();

        //Calculate and Draw
        //2-D draw mode
        GL.LoadOrtho();
        lineMaterial.SetPass(0);

        //draw lines
        GL.Begin(GL.LINES);
        DrawBest();
        //set circles' position(changeable)
        for (int i = 0; i < 8; i++)
        {
            // DrawCircle 没画出来
            DrawCircle(i);
            DrawHistogram(i);
            //print("wokleyi ");
        }
        GL.End();

        // Debug.Log("CHS.GL--check ");

        //Note that separate GL.TRIANGLE and GL.LINES parts
        for (int i = 0; i < 8; i++)
        {
            //print("wokleyiDrawTemplate");
            DrawTemplate(i);
        }
    }

    public void facet_left()
    {
        if (num_facet - 1 >= 0)
            num_facet -= 1;
        CalBestDistanceAndAlphaJ(false);
        GameObject.Find("num_facet").GetComponent<Text>().text = (num_facet + 1).ToString();
    }
    public void facet_right()
    {
        if (num_facet + 1 < 6)
            num_facet += 1;
        CalBestDistanceAndAlphaJ(false);
        GameObject.Find("num_facet").GetComponent<Text>().text = (num_facet + 1).ToString();
    }
    public void camera_left()
    {
        if (num_camera - 1 >= 0)
            num_camera -= 1;
        CalBestDistanceAndAlphaJ(false);
        GameObject.Find("num_camera").GetComponent<Text>().text = (num_camera + 1).ToString();
    }
    public void camera_right()
    {
        if (num_camera + 1 < 6)
            num_camera += 1;
        CalBestDistanceAndAlphaJ(false);
        GameObject.Find("num_camera").GetComponent<Text>().text = (num_camera + 1).ToString();
    }


    /*
     * 
     * Draw Part
     * Mainly use cyclotomic method to draw circle
     * 
     *
     */

    //set material
    public void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            Shader shader = Shader.Find("Sprites/Default");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }


    //draw color ring
    public void DrawCircle(int n)
    {
        //print("wokeyi DrawCircle");
        Vector2 position = ring[n];


        for (float i = 0; i < 360; i += step)
        {
            h = i / 360;

            float cos = Mathf.Cos(i / 180 * Mathf.PI);
            float sin = Mathf.Sin(i / 180 * Mathf.PI);

            x = radius * cos + position.x;
            y = radius * sin + position.y;

            _x = _radius * cos + position.x;
            _y = _radius * sin + position.y;

            point1 = new Vector3(x, y, 100.0f);
            _point1 = _camera.WorldToViewportPoint(point1);

            point2 = new Vector3(_x, _y, 100.0f);
            _point2 = _camera.WorldToViewportPoint(point2);

            Color color = Color.HSVToRGB(h, 1.0f, 1.0f);
            GL.Color(color);

            GL.Vertex(new Vector2(_point1.x, _point1.y));
            GL.Vertex(new Vector2(_point2.x, _point2.y));
        }
    }

    //draw histogram
    public void DrawHistogram(int n)
    {
        //print("wokeyi DrawHistogram");

        Vector2 position = ring[n];

        for (float i = 0; i < 360; i += step)
        {
            float cos = Mathf.Cos(i / 180 * Mathf.PI);
            float sin = Mathf.Sin(i / 180 * Mathf.PI);
            float len = 1 - histogram[(int)(i / step)];

            x = _radius * len * cos + position.x;
            y = _radius * len * sin + position.y;
            _x = _radius * cos + position.x;
            _y = _radius * sin + position.y;

            point1 = new Vector3(x, y, 100.0f);
            _point1 = _camera.WorldToViewportPoint(point1);

            point2 = new Vector3(_x, _y, 100.0f);
            _point2 = _camera.WorldToViewportPoint(point2);

            Color color = Color.HSVToRGB(i / 360, 0.5f, 1.0f);
            GL.Color(color);

            GL.Vertex(new Vector2(_point1.x, _point1.y));
            GL.Vertex(new Vector2(_point2.x, _point2.y));
        }
    }

    //draw single secotor
    public void DrawSector(float start, float end, int n)
    {
        Vector2 position = ring[n];

        Vector3 centre_point = new Vector3(position.x, position.y, 100.0f);
        Vector3 _centre_point = _camera.WorldToViewportPoint(centre_point);

        List<Vector3> v = new List<Vector3>();//store points

        for (float i = start; i < end; i += step)
        {
            x = _radius * Mathf.Cos(i / 180 * Mathf.PI) + position.x;
            y = _radius * Mathf.Sin(i / 180 * Mathf.PI) + position.y;

            v.Add(new Vector3(x, y, 100.0f));
        }

        //Note that GL.TRIANGLES has alpha channel while GL.LINES does not
        GL.Begin(GL.TRIANGLES);
        int count = v.Count - 1;
        for (int i = 0; i < count; i++)
        {
            point1 = v[i];
            _point1 = _camera.WorldToViewportPoint(point1);

            point2 = v[i + 1];
            _point2 = _camera.WorldToViewportPoint(point2);

            GL.Color(sector_color);

            GL.Vertex(new Vector2(_centre_point.x, _centre_point.y));
            GL.Vertex(new Vector2(_point1.x, _point1.y));
            GL.Vertex(new Vector2(_point2.x, _point2.y));
        }
        GL.End();
    }

    //draw template (it is OK if angle > 360)
    public void DrawTemplate(int n)
    {
        if (names[n] == 'N') return;
        float _alpha = alpha[n];
        float arc1 = Arcs1[n];
        float arc2 = Arcs2[n];
        float offset = Offset[n];

        float border1 = _alpha;
        float border2 = _alpha + arc1;

        DrawSector(border1, border2, n);//first sector


        switch (names[n])
        {
            case 'L':
            case 'I':
            case 'Y':
            case 'X':
                border1 = _alpha + arc1 / 2 + offset - arc2 / 2;
                border2 = border1 + arc2;
                DrawSector(border1, border2, n);//second sector
                break;
        }
    }

    public void DrawBest()
    {
        float x = ring[best_template].x;
        float y = ring[best_template].y;
        Vector3 p1 = _camera.WorldToViewportPoint(new Vector3(x - 7, y - 8, 100));
        Vector3 p2 = _camera.WorldToViewportPoint(new Vector3(x + 7, y - 8, 100));
        Vector3 p3 = _camera.WorldToViewportPoint(new Vector3(x + 7, y + 6, 100));
        Vector3 p4 = _camera.WorldToViewportPoint(new Vector3(x - 7, y + 6, 100));
        GL.Color(new Color(0.32f, 0.545f, 0.545f, 1.0f));
        GL.Vertex(new Vector2(p1.x, p1.y));
        GL.Vertex(new Vector2(p2.x, p2.y));

        GL.Vertex(new Vector2(p2.x, p2.y));
        GL.Vertex(new Vector2(p3.x, p3.y));

        GL.Vertex(new Vector2(p3.x, p3.y));
        GL.Vertex(new Vector2(p4.x, p4.y));

        GL.Vertex(new Vector2(p4.x, p4.y));
        GL.Vertex(new Vector2(p1.x, p1.y));
    }

    /*
     * 
     * Algorithm Part
     * Divide hue(from 0 to 1) into 3600 shares and reflect it to different angles
     * 
     * */

    //calculate the histogram
    public void CalHistogram(Texture2D texture)
    {
        for (int i = 0; i < 360; i++)
        {
            histogram.Add(0.0f);
        }

        for (int j = 0; j < texture.height; j++)
        {
            for (int i = 0; i < texture.width; i++)
            {
                //store color information
                Color color = texture.GetPixel(i, j);

                

                Color.RGBToHSV(color, out h, out s, out v);

                int index = (int)(h * 360);//reflect hue to circle angle
                histogram[index] += s;
            }
        }

        //percentage
        float max = histogram.Max();
        //Debug.Log(histogram.IndexOf(max));
        if (max != 0)//not a gray image
        {
            for (int i = 0; i < 360; i++)
            {
                histogram[i] /= max;
            }
        }
    }

    //make angle in [0,360]
    public float AdjustAngle(float angle)
    {
        angle = angle > 360 ? angle - 360 : angle;
        angle = angle < 0 ? 360 + angle : angle;
        return angle;
    }

    public void CalBestDistanceAndAlphaJ(bool isFinal = true)
    {
        System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();

        min_distance = float.MaxValue;
        for (int i = 0; i < 7; i++)
            CalBestDistanceAndAlphaJ(i, isFinal);

        /*       stopwatch.Stop();
               Debug.Log("计算harmony用时" + stopwatch.Elapsed);

               stopwatch.Restart();*/

        energy = 10e6f * min_distance / 180 / final.width / final.height ;

        energy += 10 * ColorSystem.MoodEnergy(final);

        //Debug.Log(energy);

        //energy += ContrastEvaluation.Contrast("room5-white");

        //Debug.Log(energy);

        stopwatch.Stop();
        //        Debug.Log("计算mood用时" + stopwatch.Elapsed);
        Debug.Log("计算fitness用时" + stopwatch.Elapsed);
    }

    //try every angle to find the fittest alpha
    public void CalBestDistanceAndAlphaJ(int n, bool isFinal = true)
    {
        float _distance;
        float cur_min_distance = float.MaxValue;

        int x, y;
        //shader initialization
        DistanceData[] output;
        int kernel = computeShader.FindKernel("CHKernel");

        if (isFinal)
        {
            x = final.width / 32;
            y = final.height / 32;
            output = new DistanceData[x * y];
            computeShader.SetTexture(kernel, "image", final);
            computeShader.SetInt("WIDTH", final.width);
            computeShader.SetInt("HEIGHT", final.height);
        }
        else
        {
            x = textures[num_camera * 6 + num_facet].width / 32;
            y = textures[num_camera * 6 + num_facet].height / 32;
            output = new DistanceData[x * y];
            computeShader.SetTexture(kernel, "image", textures[num_camera * 6 + num_facet]);
            computeShader.SetInt("WIDTH", textures[num_camera * 6 + num_facet].width);
            computeShader.SetInt("HEIGHT", textures[num_camera * 6 + num_facet].height);
        }


        int count = output.Length;
        outputbuffer = new ComputeBuffer(output.Length, 4);
        computeShader.SetBuffer(kernel, "distanceBuffer", outputbuffer);

        for (float i = 0; i < 360; i += step)
        {
            _distance = 0.0f;

            computeShader.SetFloat("arc1", Arcs1[n]);
            computeShader.SetFloat("arc2", Arcs2[n]);
            computeShader.SetFloat("offset", Offset[n]);
            computeShader.SetFloat("_alpha", i);
            computeShader.Dispatch(kernel, x, y, 1);
            outputbuffer.GetData(output);

            for (int k = 0; k < count; k++)
            {
                _distance += output[k]._distance;

            }
            //  print(min_distance);

            if(_distance < cur_min_distance)
            {
                cur_min_distance = _distance;
                alpha[n] = i;
                distance[n] = cur_min_distance;
            }

            if (_distance < min_distance)
            {
                min_distance = _distance;
                alpha[n] = i;
                distance[n] = min_distance;
                best_template = n;

            }
            //   print("kanzheli1 " + distance[0]);

        }
        //print("kanzheli1 "+ min_distance);

        outputbuffer.Release();
    }

    public void CalBestDistanceAndAlpha(int n)
    {
        float min_distance = float.MaxValue;
        float _distance;

        for (float i = 0; i < 360; i += step)
        {
            _distance = CalDistance(n, i);

            if (_distance < min_distance)
            {
                min_distance = _distance;
                alpha[n] = i;
                distance[n] = min_distance;
            }
        }
        print("kanzheli2 ");

    }

    //calculate distance when template(n) has a _alpha rotation
    public float CalDistance(int n, float _alpha)
    {
        float _distance = 0.0f;

        float arc1 = Arcs1[n];
        float arc2 = Arcs2[n];
        float offset = Offset[n];

        DistanceData[] output = new DistanceData[16 * 12];
        //DistanceData[] output = new DistanceData[16 * 12];

        int count = output.Length;

        int kernel = computeShader.FindKernel("CHKernel");

        outputbuffer = new ComputeBuffer(output.Length, 4);
        computeShader.SetTexture(kernel, "image", textures[num_camera * 6 + num_facet]);
        computeShader.SetBuffer(kernel, "distanceBuffer", outputbuffer);
        computeShader.SetFloat("arc1", Arcs1[n]);
        computeShader.SetFloat("arc2", Arcs2[n]);
        computeShader.SetFloat("offset", Offset[n]);
        computeShader.SetFloat("_alpha", _alpha);
        computeShader.Dispatch(kernel, 16, 12, 1);
        outputbuffer.GetData(output);

        for (int i = 0; i < count; i++)
        {
            _distance += output[i]._distance;
        }
        print("_distance " + _distance);
        outputbuffer.Release();

        return _distance;
    }

    public void PrintBestTemplate()
    {
        //min_distance = float.MaxValue;
        //best_template = -1;

        //for (int i = 0; i < 7; i++)
        //{
        //    if (distance[i] < min_distance)
        //    {
        //        min_distance = distance[i];
        //        best_template = i;
        //    }
        //}

        min_distance = distance[1];
        best_template = 1;
        Debug.Log("Best Template:" + names[best_template] + ",Distance:" + min_distance);

        print("kanzheli3 ");

    }

    /*
     * 
     * Other Part
     * 
     * */

    //read image file
    public Texture2D ReadImage(string filePath, int width, int height)
    {
        //file read stream
        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        fileStream.Seek(0, SeekOrigin.Begin);

        //byte array buffer
        byte[] bytes = new byte[fileStream.Length];

        //read file
        fileStream.Read(bytes, 0, (int)fileStream.Length);

        //close stream
        fileStream.Close();
        fileStream.Dispose();
        fileStream = null;

        //create texture
        Texture2D texture = new Texture2D(width, height, TextureFormat.ARGB32, false);
        texture.LoadImage(bytes);
        return texture;
    }

    //calculate the angle which make the template cover the most part of histogram
    //it is a greed method and not accurate
    public void CalBestDistanceAndAlpha_Greed(int n)
    {
        float max_cover = 0;
        float sum_cover = 0;

        float arc1 = Arcs1[n];
        float arc2 = Arcs2[n];
        float offset = Offset[n];
        float current;
        int index;

        //go through a round angles
        for (float i = 0; i < 360; i += step)
        {
            sum_cover = 0;
            float border1 = i;
            float border2 = arc1 + i;
            //area covered by first sector
            for (current = border1; current < border2; current += step)
            {
                index = (int)(AdjustAngle(current) / step);
                sum_cover += histogram[index];
            }
            //area covered by second sector
            if (arc2 != 0)
            {
                float border3 = border1 + arc1 / 2 + offset - arc2 / 2;
                float border4 = border3 + arc2;
                for (current = border3; current < border4; current += step)
                {
                    index = (int)(AdjustAngle(current) / step);
                    sum_cover += histogram[index];
                }
            }

            if (sum_cover > max_cover)
            {
                max_cover = sum_cover;
                alpha[n] = i;
            }
        }
    }

    public void CalBestDistanceAndAlphaS()
    {
        min_distance = float.MaxValue;
        for (int i = 0; i < 7; i++)
            CalBestDistanceAndAlphaS(i);
    }

    //try every angle to find the fittest alpha
    public void CalBestDistanceAndAlphaS(int n)
    {
        float _distance;
        float cur_min_distance = float.MaxValue;

        int x, y;
        //shader initialization
        DistanceData[] output;
        int kernel = computeShader.FindKernel("CHKernel");

        x = testTexture.width / 32;
        y = testTexture.height / 32;
        output = new DistanceData[x * y];
        computeShader.SetTexture(kernel, "image", testTexture);
        computeShader.SetInt("WIDTH", testTexture.width);
        computeShader.SetInt("HEIGHT", testTexture.height);


        int count = output.Length;
        outputbuffer = new ComputeBuffer(output.Length, 4);
        computeShader.SetBuffer(kernel, "distanceBuffer", outputbuffer);

        for (float i = 0; i < 360; i += step)
        {
            _distance = 0.0f;

            computeShader.SetFloat("arc1", Arcs1[n]);
            computeShader.SetFloat("arc2", Arcs2[n]);
            computeShader.SetFloat("offset", Offset[n]);
            computeShader.SetFloat("_alpha", i);
            computeShader.Dispatch(kernel, x, y, 1);
            outputbuffer.GetData(output);

            for (int k = 0; k < count; k++)
            {
                _distance += output[k]._distance;

            }
            //  print(min_distance);

            if (_distance < cur_min_distance)
            {
                cur_min_distance = _distance;
                alpha[n] = i;
                distance[n] = cur_min_distance;
            }

            if (_distance < min_distance)
            {
                min_distance = _distance;
                alpha[n] = i;
                distance[n] = min_distance;
                best_template = n;

            }
            //   print("kanzheli1 " + distance[0]);

        }
        //print("kanzheli1 "+ min_distance);

        outputbuffer.Release();
    }
}