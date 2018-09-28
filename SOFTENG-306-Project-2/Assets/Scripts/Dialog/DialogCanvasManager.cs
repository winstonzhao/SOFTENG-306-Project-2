using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DialogCanvasManager : MonoBehaviour
{
    
    public Sprite LHSSprite;
    public Sprite RHSSprite;

    private Image _avatar;
    private Text _dialogText;
    private Text _characterNameText;
    private GameObject _textPanel;
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

        ShowDialog(dialog);
    }

    void ShowDialog(Dialog dialog)
    {
        _dialog = dialog;

        var textBoxPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Dialog/Text Panel.prefab");
        var imagePrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Dialog/Image.prefab");
    

        GameObject textBox = Instantiate(textBoxPrefab, gameObject.transform);
        _dialogText = textBox.transform.Find("Dialogue Text").gameObject.GetComponent<Text>();
        _characterNameText = textBox.transform.Find("Character Name Text").gameObject.GetComponent<Text>();
        _textPanel = textBox;

        GameObject image = Instantiate(imagePrefab, gameObject.transform);
        _avatar = image.gameObject.GetComponent<Image>();

        if (dialog.DirMap[dialog.DialogFrames[0].Name] == "RHS")
        {
            SetupRHS();
        } else
        {
            SetupLHS();
        }


        foreach (var dialogFrame in dialog.DialogFrames)
        {
            this._dispatchQueue.Enqueue(dialogFrame);
        }
        
        NextSentence();
    }

    private void SetupLHS()
    {
        _avatar.sprite = LHSSprite;
        Vector3 position = _avatar.GetComponent<RectTransform>().position;
        position[0] = -6;
        _avatar.GetComponent<RectTransform>().position = position;

        position = _textPanel.GetComponent<RectTransform>().position;
        position[0] = -6;
        _textPanel.GetComponent<RectTransform>().position = position;
    }

    private void SetupRHS()
    {
        _avatar.sprite = RHSSprite;
        Vector3 position = _avatar.GetComponent<RectTransform>().position;
        position[0] = 6;
        _avatar.GetComponent<RectTransform>().position = position;

        position = _textPanel.GetComponent<RectTransform>().position;
        position[0] = -10;
        _textPanel.GetComponent<RectTransform>().position = position;
    }

    private IEnumerator RenderFrame(DialogFrame frame)
    {
        _typingText = true;
        _curShowingFrame = frame;
        _dialogText.text = "";
        foreach (var c in frame.Text)
        {
            _dialogText.text += c;
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
            _dialogText.text = _curShowingFrame.Text;
            return;
        }
        
        if (_dispatchQueue.Count != 0)
        {
            
            DialogFrame currentFrame = _dispatchQueue.Dequeue();

            CheckChangeSides(currentFrame);

            _characterNameText.text = _dialog.NameMap[currentFrame.Name];
            StartCoroutine(RenderFrame(currentFrame));
        }
        else
        {
            CloseDialog();
        }
    }

    void CheckChangeSides(DialogFrame frame)
    {
        Vector3 curPos = _avatar.GetComponent<RectTransform>().position;
        
        if (_dialog.DirMap[frame.Name] == "RHS" && curPos.x < 0)
        {
            SetupRHS();
        }
        if (_dialog.DirMap[frame.Name] == "LHS" && curPos.x > 0)
        {
            SetupLHS();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            Debug.Log(_avatar.GetComponent<RectTransform>().position);
            NextSentence();
        }
    }
}