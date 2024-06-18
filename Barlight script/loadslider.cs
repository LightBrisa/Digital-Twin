using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class loadslider : MonoBehaviour
{
    public GameObject bangdingslider;
    public float v=0;
    // Use this for initialization
    void Start()
    {
       
    }
    //IEnumerator StartLoading(string str)
    //{
    //    float i = 0;
    //    float j = 0;

    //    AsyncOperation acOp = SceneManager.LoadSceneAsync(str);
    //    acOp.allowSceneActivation = false;
    //    while (i <= 10)
    //    {
    //        i++;
    //        bangdingslider.GetComponent<Slider>().value = i / 10;//GetCompoment <T>()从当前游戏对象获取组件T，只在当前游戏对象中获取，没得到的就返回null，不会去子物体中去寻找。
    //        yield return new WaitForSeconds(1);//每秒
    //        j = i * 10;
    //        baifenbi.text = j.ToString() + "%";//显示的文字
    //    }
    //    acOp.allowSceneActivation = true;
    //}
    private void Update()
    {
        v = bangdingslider.GetComponent<Slider>().value;
        print(v);
    }
}
//在slider里面新建一个text绑定在baifenbi上
//在绑定slider上 绑定slider

