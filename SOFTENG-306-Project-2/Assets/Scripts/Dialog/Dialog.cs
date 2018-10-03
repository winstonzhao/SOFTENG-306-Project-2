using System.Collections;
using System.Collections.Generic;

public class Dialog
{
    
    private DialogFrame _startFrame;
    private Dictionary<int, string> _nameMap;
    private Dictionary<int, string> _dirMap;

    public Dialog(DialogFrame startFrame)
    {
        _startFrame = startFrame;
        _nameMap = new Dictionary<int, string>();
        _dirMap = new Dictionary<int, string>();
        _nameMap[0] = Toolbox.Instance.GameManager.Player.Username;
        _nameMap[1] = "Kerry Juniper";
        _dirMap[0] = "LHS";
        _dirMap[1] = "RHS";
    }

    public DialogFrame StartFrame
    {
        get { return _startFrame; }
    }

    public Dictionary<int, string> NameMap
    {
        get { return _nameMap; }
        set { _nameMap = value; }
    }

    public Dictionary<int, string> DirMap
    {
        get { return _dirMap; }
        set { _dirMap = value;  }
    }
}
