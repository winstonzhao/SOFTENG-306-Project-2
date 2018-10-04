using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Use this with the given "Speech Bubble" prefab
/// </summary>
[ExecuteInEditMode]
public class SpeechBubble : MonoBehaviour
{
    public IsoTransform Follow;

    public string Message
    {
        get { return Text.text; }
        set { Text.text = value; }
    }

    public float HorizontalPadding = 16.0f;

    public float VerticalPadding = 16.0f;

    public float VerticalOffset;

    public float MaxWidth = 400.0f;

    public float MaxHeight = 78.0f;

    private IsoTransform ThisTransform;

    private RectTransform CanvasRectTransform;

    private RectTransform ImageRectTransform;

    private Text Text;

    private void Start()
    {
        if (Follow == null)
        {
            var parent = transform.parent;
            Follow = parent == null ? null : parent.GetComponent<IsoTransform>();
        }

        ThisTransform = GetComponent<IsoTransform>();
        CanvasRectTransform = GetComponent<RectTransform>();
        ImageRectTransform = transform.Find("Image").GetComponent<RectTransform>();
        Text = transform.Find("Text").GetComponent<Text>();

        CanvasRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, MaxHeight);
        CanvasRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, MaxWidth);

        FollowTarget();
    }

    private void FollowTarget()
    {
        // Follow the selected game object
        if (Follow != null)
        {
            ThisTransform.Position = new Vector3(
                Follow.Position.x,
                Follow.Position.y + Follow.Size.y + VerticalOffset,
                Follow.Position.z
            );
        }
    }

    private void Update()
    {
        FollowTarget();

        var width = Text.preferredWidth;
        var maxWidth = CanvasRectTransform.rect.width;
        var maxHeight = CanvasRectTransform.rect.height;

        var lines = Mathf.CeilToInt(width / maxWidth);
        if (lines > 1)
        {
            width = maxWidth;
        }

        var height = Text.preferredHeight;
        if (height > maxHeight)
        {
            height = maxHeight;
        }

        ImageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height + VerticalPadding);
        ImageRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width + HorizontalPadding);
    }
}
