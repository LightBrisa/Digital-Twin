using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MapScript : MonoBehaviour
{

    /*
     * 
     * Variable Part
     * 
     * */

    private Camera _camera;//camera
    private Vector3 mousePosition, setCameraPosition;//mouse position
    private static float view_height = 1.5f;//human's view height

    private Cubemap cubemap;//cubemap
    private CubemapFace offcialFaces;
    public List<Cubemap> cubemaps;

    private Texture2D[] texture = new Texture2D[6];//six-direction texture

    private int count;//no more than 5
    private static int max_count = 6;

    private Texture2D[] maps = new Texture2D[max_count];
     
    private GameObject marker_prefab;//use for marker
    private GameObject marker_map_prefab;
    private GameObject[] marker = new GameObject[max_count];//marker group
    private GameObject[] marker_map = new GameObject[max_count];
    private static float marker_height = 1.0f;//marker properties
    private Vector3 markerPositon;

    private LineRenderer lineRenderer;//draw a line


    private bool draw;
    private List<Vector3> pos;//point group
    private static float width = 0.2f;
    public List<Vector3> sample;//sample point group

    public Texture2D finalImage;

    public List<Vector3> test_samples;

    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();

    /*
     * 
     * Unity Function Part
     * 
     * */
    void Start()
    {
        //get camera and create cubemap
        _camera = GameObject.Find("MapCamera-floor1").GetComponent<Camera>();
        cubemap = new Cubemap(64, TextureFormat.ARGB32, false);
        count = 0;

        // 为什么有两个 
        marker_prefab = (GameObject)Resources.Load("MarkerPrefab");
        marker_map_prefab = (GameObject)Resources.Load("Marker_MapPrefab");

        lineRenderer = GameObject.Find("Line").GetComponent<LineRenderer>();
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
        draw = false;
        pos = new List<Vector3>();
    }

    void Update()
    {
        //when mouse clicks
        //if (Input.GetMouseButtonDown(0))
        //{
        //    mousePosition = Input.mousePosition;

        //    //transfer screen coordinate to world coordinate
        //    setCameraPosition = _camera.ScreenToWorldPoint(mousePosition);
        //    setCameraPosition.y = view_height;

        //    markerPositon = setCameraPosition;
        //    markerPositon.y = marker_height;

        //    //create a cubemap camera
        //    GameObject go = new GameObject("CubeCamera", typeof(Camera));
        //    go.transform.position = setCameraPosition;
        //    go.transform.rotation = Quaternion.identity;

        //    //render to cubemap
        //    go.GetComponent<Camera>().cullingMask = ~((1 << 8) | (1 << 9) | (1 << 10));//remove layer8,9,10 which is layer for marker and line
        //    go.GetComponent<Camera>().RenderToCubemap(cubemap);

        //    //sava 6 faces and merge them
        //    SaveMultiTexture(count);
        //    MergeImage(count);

        //    Destroy(marker[count]);
        //    Destroy(marker_map[count]);
        //    marker[count] = Instantiate(marker_prefab);
        //    marker[count].transform.position = markerPositon;

        //    marker_map[count] = Instantiate(marker_map_prefab);
        //    marker_map[count].transform.position = markerPositon;

        //    if (++count == max_count) count = 0;
        //    //distroy the cubemap camera
        //    DestroyImmediate(go);
        //}


        if (_camera.enabled && (Input.mousePosition.y <= Screen.height - 55) && (Input.mousePosition.x >= 60))
        {
            //start drawing
            if (Input.GetMouseButtonDown(0))
            {
                draw = true;
            }
            //when drawing
            if (draw)
            {
                //get mouse position and add it to list
                Vector3 point = _camera.ScreenToWorldPoint(Input.mousePosition);
                point.y = width;
                pos.Add(point);
                Draw();

            }
            //end drawing
            if (draw && Input.GetMouseButtonUp(0))
            {
                sample = SampleCurve(pos);
                //////////////////////////
                for (int i = 0; i < sample.Count; i++)
                {
                    markerPositon = new Vector3(sample[i].x, marker_height, sample[i].z);
                    Debug.Log(markerPositon);
                    Destroy(marker[i]);
                    Destroy(marker_map[i]);
                    marker[i] = Instantiate(marker_prefab);
                    marker[i].transform.position = markerPositon;

                    marker_map[i] = Instantiate(marker_map_prefab);
                    marker_map[i].transform.position = markerPositon;
                    }
                    ///////////////////////////////


                    //test_samples = MCMC(pos, 70);
                    //int i = 0;
                    //for (i = 0; i < test_samples.Count && i < max_count - 1; i++)
                    //{
                    //    markerPositon = new Vector3(test_samples[i].x, marker_height, test_samples[i].z);
                    //    Debug.Log(markerPositon);
                    //    Destroy(marker[i]);
                    //    Destroy(marker_map[i]);
                    //    marker[i] = Instantiate(marker_prefab);
                    //    marker[i].transform.position = markerPositon;

                    //    marker_map[i] = Instantiate(marker_map_prefab);
                    //    marker_map[i].transform.position = markerPositon;
                    //    print("yeah");
                    //}

                    //markerPositon = new Vector3(test_samples[test_samples.Count - 1].x, marker_height, test_samples[test_samples.Count - 1].z);
                    //Debug.Log(markerPositon);
                    //Destroy(marker[i]);
                    //Destroy(marker_map[i]);
                    //marker[i] = Instantiate(marker_prefab);
                    //marker[i].transform.position = markerPositon;

                    //marker_map[i] = Instantiate(marker_map_prefab);
                    //marker_map[i].transform.position = markerPositon;

                    /////////////

                    draw = false;
                CubeJ();
                Debug.Log("OK");
                pos.Clear();
            }
        }
    }

    public void CubeJ()
    {
        //       stopwatch.Start();

        for (int i = 0; i < sample.Count; i++)
        {
            //set camera position according to sample points
            setCameraPosition = new Vector3(sample[i].x, view_height, sample[i].z);
            //create a cubemap camera
            GameObject go = new GameObject("CubeCamera", typeof(Camera));
            go.transform.position = setCameraPosition;
            go.transform.rotation = Quaternion.identity;

            //remove layer8,9,10 which is layer for marker and line
            go.GetComponent<Camera>().cullingMask = ~((1 << 8) | (1 << 9) | (1 << 10));
            cubemap = new Cubemap(64, TextureFormat.ARGB32, false);
            go.GetComponent<Camera>().RenderToCubemap(cubemap);//render to cubemap

            cubemaps.Add(cubemap);

            SaveMultiTexture(i);
            MergeImage(i);

            Destroy(go);
        }
        //       stopwatch.Stop();
        //       Debug.Log("Render To Cubemap Time: " + stopwatch.Elapsed);

        //        stopwatch.Start();

        MergeFinal();
        ColorHarmonizationScript script = GameObject.Find("Camera").GetComponent<ColorHarmonizationScript>();
        script.InitJ();
        cubemaps.Clear();
        //       stopwatch.Stop();
        //       Debug.Log("InitJ Time: " + stopwatch.Elapsed);

    }
    public void Cube()
    {
        stopwatch.Start();

        for (int i = 0; i < sample.Count; i++)
        {
            //set camera position according to sample points
            setCameraPosition = new Vector3(sample[i].x, view_height, sample[i].z);

            //create a cubemap camera
            GameObject go = new GameObject("CubeCamera", typeof(Camera));
            go.transform.position = setCameraPosition;
            go.transform.rotation = Quaternion.identity;

            //remove layer8,9,10 which is layer for marker and line
            go.GetComponent<Camera>().cullingMask = ~((1 << 8) | (1 << 9) | (1 << 10));
            go.GetComponent<Camera>().RenderToCubemap(cubemap);//render to cubemap

            //sava 6 faces and merge them
            SaveMultiTexture(i);
            MergeImage(i);
            Destroy(go);
        }

        MergeFinal();
        stopwatch.Stop();
        Debug.Log("Time for CubeMap generation: " + stopwatch.Elapsed);

        ColorHarmonizationScript script = GameObject.Find("Camera").GetComponent<ColorHarmonizationScript>();
        script.Init();
    }

    /*
     *
     *Process Image Part
     * 
     * */
    //save 6 cubemap faces
    private void SaveMultiTexture(int n)
    {
        for (int k = 0; k < 6; k++)
        {
            //get 6 faces
            Texture2D screenShot = new Texture2D(cubemap.width, cubemap.height, TextureFormat.ARGB32, false);
            offcialFaces = (CubemapFace)k;

            for (int i = 0; i < cubemap.width; i++)
            {
                for (int j = 0; j < cubemap.height; j++)
                {
                    screenShot.SetPixel(i, j, cubemap.GetPixel(offcialFaces, cubemap.width - i, cubemap.height - j));
                }
            }
            screenShot.Apply();
            texture[k] = screenShot;
        }
    }

    //merge images
    public void MergeImage(int n)
    {
        //存图
        //byte[] bytes = texture[4].EncodeToPNG();
        //System.IO.File.WriteAllBytes("512.png", bytes);

        ////string path = Application.dataPath + "/" + "CubeMapTextures/";//save path
        Texture2D merge = new Texture2D(cubemap.width * 4, cubemap.height * 3, TextureFormat.ARGB32, false);

        TransferPixel(merge, texture[2], 0, 2);//top
        TransferPixel(merge, texture[4], 0, 1);//front
        TransferPixel(merge, texture[3], 0, 0);//bottom
        TransferPixel(merge, texture[1], 1, 1);//left
        TransferPixel(merge, texture[5], 2, 1);//back
        TransferPixel(merge, texture[0], 3, 1);//right

        merge.Apply();
        maps[n] = merge;

        //transfer to png
        //byte[] mergeByte = merge.EncodeToPNG();

        //if (System.IO.File.Exists("Merge" + n + ".png"))
            //System.IO.File.Delete("Merge" + n + ".png");

        //System.IO.File.WriteAllBytes("Merge" + n + ".png", mergeByte);
        //Debug.Log("save merged image completed!");
    }

    //merge all the cubemap together
    public void MergeFinal()
    {
        int w = cubemap.width * 4;
        int h = cubemap.height * 3;
        Texture2D final = new Texture2D(w, h, TextureFormat.ARGB32, false);
        for (int i = 0; i < w; i++)
        {
            int n = i / (cubemap.width * 4);
            for (int j = 0; j < h; j++)
            {
                final.SetPixel(i, j, maps[n].GetPixel(i - n * cubemap.width * 4, j));
            }
        }
        final.Apply();
        finalImage = final;

        //byte[] finalByte = final.EncodeToPNG();
        //System.IO.File.WriteAllBytes("final.png", finalByte);
    }

    //fill merged texture
    public void TransferPixel(Texture2D merge, Texture2D image, int wi, int hi)
    {
        for (int i = image.width * wi; i < image.width * (wi + 1); i++)
        {
            int m = i - image.width * wi;
            for (int j = image.height * hi; j < image.height * (hi + 1); j++)
            {
                int n = j - image.height * hi;
                merge.SetPixel(i, j, image.GetPixel(m, n));
            }
        }
    }


    /*
     *
     *Draw Part
     * 
     * */
    //update line render to draw the curvecub
    private void Draw()
    {
        lineRenderer.positionCount = pos.Count;
        lineRenderer.SetPositions(pos.ToArray());
    }

    //calculate length of the curve
    private float CalLength(List<Vector3> curve)
    {
        float total_length = 0.0f;

        for (int i = 0; i < curve.Count - 1; i++)
        {
            total_length += Vector3.Distance(curve[i], curve[i + 1]);
        }

        return total_length;
    }

    //get sample points from curve(the number of points should above 2)
    private List<Vector3> SampleCurve(List<Vector3> curve)
    {
        Vector3[] sample = new Vector3[max_count];
        float total_length = CalLength(curve);
        float average_length = total_length / (max_count - 1);

        float dist;
        float cur_dist = 0.0f;
        int index = 1;

        sample[0] = curve[0];//head point 
        sample[max_count - 1] = curve[curve.Count - 1];//tail point

        for (int i = 1; i < max_count - 1; i++)
        {
            float cur_length = average_length * i;
            while (true)
            {
                //length of line segment
                dist = Vector3.Distance(curve[index], curve[index + 1]);
                if (cur_dist + dist >= cur_length)
                {
                    float p = (cur_length - cur_dist) / dist;//proportion
                    Vector3 v = (curve[index + 1] - curve[index]) * p;
                    sample[i] = curve[index] + v; //vector addition
                    break;
                }
                else
                {
                    cur_dist += dist;//accumulate distance
                    index++;//next point
                }
            }
        }

        return new List<Vector3>(sample);
    }

    public List<Texture2D> tests = new List<Texture2D>();

    private List<Vector3> MCMC(List<Vector3> curve, int threshold)
    {
        string room = GroupController.roomString;
        GameObject home = GameObject.Find(room);
        ArrayList objects = new ArrayList();
        foreach (Transform child in home.transform)
            objects.Add(child);

        List<Vector3> samples = new List<Vector3>();
        Vector3 pre = curve[0];
        samples.Add(pre);
        foreach (Vector3 pos in curve)
        {
            int d = 0;
            foreach (Transform o in objects)
            {
                Color origin = o.GetComponent<MeshRenderer>().material.color;
                o.GetComponent<MeshRenderer>().material.color = Color.red;
                Texture2D a = CubeForOne(pre);
                Texture2D b = CubeForOne(pos);
                d += difference(a, b);
                o.GetComponent<MeshRenderer>().material.color = origin;
            }

           // Debug.Log(d);
            if (d > threshold)
            {
                samples.Add(pos);
                pre = pos;

                Texture2D t = CubeForOne(pos);
                byte[] bytes = t.EncodeToPNG();
                System.IO.File.WriteAllBytes("" + samples.Count + ".png", bytes);
            }
        }
        return samples;
    }

    private Texture2D CubeForOne(Vector3 pos, int pixel = 64)
    {
        setCameraPosition = new Vector3(pos.x, view_height, pos.z);
        //create a cubemap camera
        GameObject go = new GameObject("CubeCamera", typeof(Camera));
        go.transform.position = setCameraPosition;
        go.transform.rotation = Quaternion.identity;

        //remove layer8,9,10 which is layer for marker and line
        go.GetComponent<Camera>().cullingMask = ~((1 << 8) | (1 << 9) | (1 << 10));
        Cubemap cubemap = new Cubemap(32, TextureFormat.ARGB32, false);
        go.GetComponent<Camera>().RenderToCubemap(cubemap);//render to cubemap
        Destroy(go);

        Texture2D[] texture = new Texture2D[6];
        for (int k = 0; k < 6; k++)
        {
            //get 6 faces
            Texture2D screenShot = new Texture2D(cubemap.width, cubemap.height, TextureFormat.ARGB32, false);
            offcialFaces = (CubemapFace)k;

            for (int i = 0; i < cubemap.width; i++)
            {
                for (int j = 0; j < cubemap.height; j++)
                {
                    screenShot.SetPixel(i, j, cubemap.GetPixel(offcialFaces, cubemap.width - i, cubemap.height - j));
                }
            }
            screenShot.Apply();
            texture[k] = screenShot;
        }

        //string path = Application.dataPath + "/" + "CubeMapTextures/";//save path
        Texture2D merge = new Texture2D(cubemap.width * 4, cubemap.height * 3, TextureFormat.ARGB32, false);

        TransferPixel(merge, texture[2], 0, 2);//top
        TransferPixel(merge, texture[4], 0, 1);//front
        TransferPixel(merge, texture[3], 0, 0);//bottom
        TransferPixel(merge, texture[1], 1, 1);//left
        TransferPixel(merge, texture[5], 2, 1);//back
        TransferPixel(merge, texture[0], 3, 1);//right

        merge.Apply();
        return merge;
    }

    private int difference(Texture2D a, Texture2D b)
    {
        int d = 0;
        for (int i = 0; i < a.width; i++)
        {
            for (int j = 0; i < a.height; i++)
            {
                ColorSystem.ColorLab aLab = ColorSystem.RGBToLab(a.GetPixel(i, j));
                ColorSystem.ColorLab bLab = ColorSystem.RGBToLab(b.GetPixel(i, j));
                if ((aLab.a > 10 && aLab.b > 10) && !(bLab.a > 10 && bLab.b > 10) ||
                    (bLab.a > 10 && bLab.b > 10) && !(aLab.a > 10 && aLab.b > 10))
                    d++;
            }
        }
        return d;
    }
}