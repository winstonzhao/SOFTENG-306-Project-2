using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogCanvasManager : MonoBehaviour
{
    
    private Sprite _lhsSprite;
    private Sprite _rhsSprite;
    private Image _avatar;
    private Text _dialogText;
    private Text _characterNameText;
    private GameObject _textPanel;
    private DialogFrame _currentFrame;
    private bool _typingText = false;
    private Dialog _dialog;
    private DialogFrame _placeHolder;
    private bool _showing = false;
    

    // Use this for initialization
    void Start()
    {


        
    }

    public void ShowDialog(Dialog dialog, Sprite lhs, Sprite rhs)
    {
        _lhsSprite = lhs;
        _rhsSprite = rhs;
        _showing = true;
        _dialog = dialog;
        DialogFrame head = new DialogFrame(dialog.StartFrame.Text, dialog.StartFrame.Name);
        head.Next = dialog.StartFrame;
        _placeHolder = head;
        _currentFrame = head;

        NextSentence();
    }

    private void SetupLHS()
    {
        float multiplier = GetComponent<RectTransform>().localScale.x;
        float avatarWidth = _avatar.GetComponent<RectTransform>().rect.width;
        Debug.Log(multiplier);
        Debug.Log(avatarWidth);
        _avatar.sprite = _lhsSprite;
        Vector3 position = _avatar.GetComponent<RectTransform>().position;
        position[0] = 100 * multiplier;
        position[1] = 75 * multiplier;
        position[2] = 0;
        _avatar.GetComponent<RectTransform>().position = position;

        position = _textPanel.GetComponent<RectTransform>().position;
        position[0] = 100 * multiplier;
        position[1] = 75 * multiplier;
        position[2] = 0;
        _textPanel.GetComponent<RectTransform>().position = position;
    }

    private void SetupRHS()
    {
        float multiplier = GetComponent<RectTransform>().localScale.x;
        float avatarWidth = _avatar.GetComponent<RectTransform>().rect.width;
        _avatar.sprite = _rhsSprite;
        Vector3 position = _avatar.GetComponent<RectTransform>().position;
        position[0] = 700 * multiplier;
        position[1] = 75 * multiplier;
        position[2] = 0;
        _avatar.GetComponent<RectTransform>().position = position;

        position = _textPanel.GetComponent<RectTransform>().position;
        position[0] = 50 * multiplier;
        position[1] = 75 * multiplier;
        position[2] = 0;
        _textPanel.GetComponent<RectTransform>().position = position;
    }

    private IEnumerator RenderFrame(DialogFrame frame)
    {
        _typingText = true;
        _currentFrame = frame;
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
        DestroyStuff();
        StartCoroutine(WaitForMS());
    }

    /**
     * Wait for 1 second so that we don't keep talking.
     */ 
    IEnumerator WaitForMS()
    {
        yield return new WaitForSeconds(1);
        _showing = false;
    }

    void DestroyStuff()
    {
        Destroy(_textPanel);
        if (_avatar != null)
        {
            Destroy(_avatar.gameObject);
        }
    }

    void RenderTextboxes()
    {
        if (_avatar != null)
        {
            DestroyStuff();
        }

        if (_currentFrame.ButtonFrame == true)
        {
            var buttonBoxPrefab = Resources.Load<GameObject>("Prefabs/Dialog/Button Panel");

            GameObject textBox = Instantiate(buttonBoxPrefab, gameObject.transform);
            _dialogText = textBox.transform.Find("Dialogue Text").gameObject.GetComponent<Text>();
            _characterNameText = textBox.transform.Find("Character Name Text").gameObject.GetComponent<Text>();
            _textPanel = textBox;

            Dictionary<string, DialogFrame> options = _currentFrame.Options;

            int n = 1;

            foreach (KeyValuePair<string, DialogFrame> entry in options)
            {
                Button button = textBox.transform.Find("Button " + n).gameObject.GetComponent<Button>();
                button.transform.Find("Text").gameObject.GetComponent<Text>().text = n + ". " + entry.Key;
                button.onClick.AddListener(delegate {
                    _placeHolder.Next = entry.Value;
                    _currentFrame = _placeHolder;
                    NextSentence();
                });
                n++;
            }

            for (; n <= 4; n++)
            {
                Destroy(textBox.transform.Find("Button " + n).gameObject);
            }
        }
        else
        {
            var textBoxPrefab = Resources.Load<GameObject>("Prefabs/Dialog/Text Panel");

            GameObject textBox = Instantiate(textBoxPrefab, gameObject.transform);
            _dialogText = textBox.transform.Find("Dialogue Text").gameObject.GetComponent<Text>();
            _characterNameText = textBox.transform.Find("Character Name Text").gameObject.GetComponent<Text>();
            _textPanel = textBox;
        }

        var imagePrefab = Resources.Load<GameObject>("Prefabs/Dialog/Image");

        GameObject image = Instantiate(imagePrefab, gameObject.transform);
        _avatar = image.gameObject.GetComponent<Image>();
    }

    void NextSentence()
    {
        StopAllCoroutines();

        if (_currentFrame.TransitionFrame == true)
        {
            GameManager.Instance.ChangeScene(_currentFrame.LevelName);
            return;
        }

        if (_typingText == true)
        {
            _typingText = false;
            _dialogText.text = _currentFrame.Text;
            return;
        }

        if (_currentFrame.ButtonFrame == true)
        {
            return;
        } 
        
        if (_currentFrame.Next != null)
        {
            DialogFrame currentFrame = _currentFrame.Next;
            _currentFrame = _currentFrame.Next;
            RenderTextboxes();
            CheckChangeSides(currentFrame);

            _characterNameText.text = _dialog.NameMap[currentFrame.Name];
            StartCoroutine(RenderFrame(currentFrame));
            return;
        }
        else
        {
            CloseDialog();
        }
    }

    void CheckChangeSides(DialogFrame frame)
    {
        Vector3 curPos = _avatar.GetComponent<RectTransform>().position;
        
        if (_dialog.DirMap[frame.Name] == "RHS")
        {
            SetupRHS();
        }
        if (_dialog.DirMap[frame.Name] == "LHS")
        {
            SetupLHS();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space") && _showing)
        {
            NextSentence();
        }

        for (int i = 1; i <= 4; i++)
        {
            if (Input.GetKeyDown(i.ToString()) && _currentFrame.ButtonFrame)
            {
                Button button = _textPanel.transform.Find("Button " + i).gameObject.GetComponent<Button>();
                button.onClick.Invoke();
                NextSentence();
            }
        }
    }

    public bool Showing
    {
        get { return _showing; }
        set { _showing = value; }
    }
}
