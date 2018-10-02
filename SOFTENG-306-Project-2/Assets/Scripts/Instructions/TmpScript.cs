using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TmpScript : MonoBehaviour
{
    public int MaxHeight;
    public Scrollbar scrollbar;

    // Use this for initialization
    void Start()
    {
        var childrenRectTransform = GetComponentInChildren<Mask>().transform.GetChild(0).GetComponent<RectTransform>();
        var prevValue = 0f;
        scrollbar.onValueChanged.AddListener((value) => {
            var height = GetComponentInChildren<DraggableScrollList>().GetComponent<RectTransform>().sizeDelta.y;
            height -= MaxHeight;
            if (height < 0)
            {
                height = 0;
            }
            
            // height = height * 2;
            Debug.Log(height * (value - prevValue));
            childrenRectTransform.anchoredPosition += new Vector2(0, -height * (value - prevValue)) ;

            prevValue = value;
        });
    }

    // Update is called once per frame
    void Update()
    {
        var height = GetComponentInChildren<DraggableScrollList>().GetComponent<RectTransform>().sizeDelta.y;
        if (height > MaxHeight)
        {
            scrollbar.gameObject.SetActive(true);
            scrollbar.size = MaxHeight / height;
        }
        else
        {
            scrollbar.gameObject.SetActive(false);
        }

    }
}
