using UnityEngine;
using UnityEngine.UI;

public class ScrollList : MonoBehaviour
{
    public Scrollbar scrollbar;

    public DraggableScrollList scrollList;

    // Use this for initialization
    private void Start()
    {
        var childrenRectTransform = GetComponentInChildren<Mask>().transform.GetChild(0).GetComponent<RectTransform>();
        var prevValue = 0f;
        scrollList.maxHeight = GetComponent<RectTransform>().sizeDelta.y;
        scrollList.MinSize =
            new Vector2(GetComponent<RectTransform>().sizeDelta.x * GetComponent<RectTransform>().lossyScale.x,
                scrollList.MinSize.y);
        scrollList.layout();
        scrollbar.onValueChanged.AddListener(value =>
        {
            var height = scrollList.GetComponent<RectTransform>().sizeDelta.y;
            height -= scrollList.maxHeight;
            if (height < 0) height = 0;

            // height = height * 2;
            childrenRectTransform.anchoredPosition = new Vector2(0, -height * value);

            prevValue = value;
        });
    }

    // Update is called once per frame
    private void Update()
    {
        var height = GetComponentInChildren<DraggableScrollList>().GetComponent<RectTransform>().sizeDelta.y;
        scrollbar.size = scrollList.maxHeight / height;
        if (height > scrollList.maxHeight && !scrollbar.gameObject.activeInHierarchy)
        {
            scrollbar.gameObject.SetActive(true);
            scrollList.layout();
        }
        else if (height <= scrollList.maxHeight && scrollbar.gameObject.activeInHierarchy)
        {
            var childrenRectTransform =
                GetComponentInChildren<Mask>().transform.GetChild(0).GetComponent<RectTransform>();
            scrollbar.gameObject.SetActive(false);
            childrenRectTransform.anchoredPosition = new Vector2(0, 0);
            scrollbar.value = 0;
            scrollList.layout();
        }
    }
}