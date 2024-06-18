using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GroupingAidController : MonoBehaviour
{
    public GameObject room;
    public GameObject grouping;
    private Button confirm_btn;

    // Start is called before the first frame update
    void Start()
    {
        confirm_btn = GameObject.Find("confirm").GetComponent<Button>();
        confirm_btn.onClick.AddListener(Confirm);
    }

    void Confirm()
    {
        ArrayList group = new ArrayList();
        room = GameObject.Find(GroupController.roomString);
        foreach(Transform furniture in room.transform)
        {
            cakeslice.Outline outline = furniture.GetComponent<cakeslice.Outline>();
            if (outline != null && !outline.eraseRenderer)
            {
                group.Add(furniture);
                outline.eraseRenderer = true;
                GroupController.grouped.Add(furniture);
            }
        }
        GroupController.Add(group);
        grouping.GetComponent<GroupController>().AddList(group);

        grouping.SetActive(true);
        gameObject.SetActive(false);
        GroupController.grouping = false;
    }

    public void Clear()
    {
        foreach(Transform furniture in room.transform)
        {
            cakeslice.Outline outline = furniture.GetComponent<cakeslice.Outline>();
            if (outline != null)
            {
                outline.eraseRenderer = true;
            }
        }
    }
}
