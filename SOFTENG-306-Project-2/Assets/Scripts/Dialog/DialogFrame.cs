using System.Collections;
using System.Collections.Generic;

public class DialogFrame
{
	private string _text;
	private int _speaker;
    private DialogFrame _next;
    private bool _buttonFrame = false;
    private bool _changeLevel = false;
    private string _levelName;
    private Dictionary<string, DialogFrame> _options;

	public DialogFrame(string text, int name)
	{
		_text = text;
		_speaker = name;
	}

    public void MakeTransitionFrame(string levelName)
    {
        _levelName = levelName;
        _changeLevel = true;
    }

    public void MakeOptionFrame(Dictionary<string, DialogFrame> options)
    {
        _options = options;
        _buttonFrame = true;
    }

	public string Text
	{
		get { return _text; }
	}

	public int Name
	{
		get { return _speaker; }
	}

    public DialogFrame Next
    {
        get { return _next; }
        set { _next = value; }
    }

    public Dictionary<string, DialogFrame> Options
    {
        get { return _options; }
    }

    public bool ButtonFrame
    {
        get { return _buttonFrame; }
        set { _buttonFrame = value; }
    }

    public bool TransitionFrame
    {
        get { return _changeLevel;  }
    }

    public string LevelName
    {
        get { return _levelName; }
        set { _levelName = value; }
    }
}
