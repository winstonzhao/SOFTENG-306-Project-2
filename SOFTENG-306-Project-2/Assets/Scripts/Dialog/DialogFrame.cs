using System.Collections;
using System.Collections.Generic;

public class DialogFrame
{
	private string _text;
	private int _speaker;

	public DialogFrame(string text, int name)
	{
		_text = text;
		_speaker = name;
	}

	public string Text
	{
		get { return _text; }
	}

	public int Name
	{
		get { return _speaker; }
	}
}
