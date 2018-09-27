using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogCanvasManager : MonoBehaviour
{
    public Text DialogText;
    public Text CharacterNameText;
    public Image Avatar;
    public GameObject TextPanel;
    public Sprite LHSSprite;
    public Sprite RHSSprite;

    private Queue<DialogFrame> _dispatchQueue = new Queue<DialogFrame>();
    private bool _typingText = false;
    private DialogFrame _curShowingFrame;
    private Dialog _dialog;
    

    // Use this for initialization
    void Start()
    {

        DialogFrame[] frames =
        {
            new DialogFrame("Hello, my name is Jeff.", 0),
            new DialogFrame("Well, that's great isn't it?", 1),
            new DialogFrame("Why is it always raining in Auckland?", 0),
            new DialogFrame("That's due to the city having a large amount of dust particles in the air, " +
                            "those particles act as seeds for the clouds that create precipitation!",
                1),
            new DialogFrame("In fact, that's why rain is the city is often more sporadic than in the suburbs!",
                1),
            new DialogFrame("Ooooooo, I see, that's super interesting!", 0),
        };

        Dialog dialog = new Dialog(frames);

        _dialog = dialog;

        ShowDialog(_dialog);
    }

    void ShowDialog(Dialog dialog)
    {
        foreach (var dialogFrame in dialog.DialogFrames)
        {
            this._dispatchQueue.Enqueue(dialogFrame);
        }
        
        NextSentence();
    }

    private IEnumerator RenderFrame(DialogFrame frame)
    {
        _typingText = true;
        _curShowingFrame = frame;
        DialogText.text = "";
        foreach (var c in frame.Text)
        {
            DialogText.text += c;
            yield return null;
        }

        _typingText = false;
    }

    void CloseDialog()
    {
        Destroy(gameObject);
    }

    void NextSentence()
    {
        StopAllCoroutines();
        
        if (_typingText == true)
        {
            _typingText = false;
            DialogText.text = _curShowingFrame.Text;
            return;
        }
        
        if (_dispatchQueue.Count != 0)
        {
            
            DialogFrame currentFrame = _dispatchQueue.Dequeue();

            CheckChangeSides(currentFrame);

            CharacterNameText.text = _dialog.NameMap[currentFrame.Name];
            StartCoroutine(RenderFrame(currentFrame));
        }
        else
        {
            CloseDialog();
        }
    }

    void CheckChangeSides(DialogFrame frame)
    {
        Vector3 curPos = Avatar.GetComponent<RectTransform>().position;
        
        if (_dialog.DirMap[frame.Name] == "RHS" && curPos.x < 0)
        {
            Avatar.sprite = RHSSprite;
            Avatar.GetComponent<RectTransform>().position += Vector3.right * 10;
            TextPanel.GetComponent<RectTransform>().position += Vector3.left * 4;
        }
        if (_dialog.DirMap[frame.Name] == "LHS" && curPos.x > 0)
        {
            Avatar.sprite = LHSSprite;
            Avatar.GetComponent<RectTransform>().position -= Vector3.right * 10;
            TextPanel.GetComponent<RectTransform>().position -= Vector3.left * 4;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log(Avatar.GetComponent<RectTransform>().position);
            NextSentence();
        }
    }
}