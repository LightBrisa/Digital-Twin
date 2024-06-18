using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupController : MonoBehaviour
{
    public GameObject room;
    public GameObject aid;
    private Button plus_btn;
    private Button close_btn;

    static public string roomString = "SpotLamp2_onlylight_test";

    static public bool grouping;
    static public ArrayList groups = new ArrayList();
    static public ArrayList grouped = new ArrayList();

    // Start is called before the first frame update
    void Start()
    {
        plus_btn = GameObject.Find("add").GetComponent<Button>();
        close_btn = GameObject.Find("close").GetComponent<Button>();

        close_btn.onClick.AddListener(delegate
        {
            gameObject.SetActive(false);
        });
        plus_btn.onClick.AddListener(delegate
        {
            gameObject.SetActive(false);
            aid.SetActive(true);
            aid.GetComponent<GroupingAidController>().Clear();
            grouping = true;
        });
    }

    static public void Init()
    {
        GameObject home = GameObject.Find(roomString);
        groups.Clear();
        foreach (Transform child in home.transform)
        {
            ArrayList group = new ArrayList();
            group.Add(child);
            groups.Add(group);
        }
        Debug.Log("绑定的物体的数量: "+groups.Count);
    }

    int offset;

    public void AddList(ArrayList group)
    {
        GameObject text = (GameObject)Resources.Load("group");
        string s = "";
        foreach (Transform furniture in group)
        {
            s += furniture.name + "   ";
        }
        text.GetComponent<Text>().text = s;
        GameObject textObject = Instantiate(text, transform);
        textObject.transform.position = new Vector3(textObject.transform.position.x, textObject.transform.position.y - offset);
        offset += 50;
    }

    static public void Add(ArrayList group)
    {
        for (int i = groups.Count - 1; i >= 0; i--)
        {
            ArrayList g = (ArrayList)groups[i];
            foreach (var o in group)
            {
                if (g.Contains(o))
                {
                    groups.Remove(g);
                }
            }
        }
        groups.Add(group);
        Debug.Log(groups.Count);
    }
}
