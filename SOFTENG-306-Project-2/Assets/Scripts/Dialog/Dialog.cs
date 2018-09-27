using System.Collections;
using System.Collections.Generic;

public class Dialog
{
    
    private DialogFrame[] _dialogFrames;
    private Dictionary<int, string> _nameMap;
    private Dictionary<int, string> _dirMap;

    public Dialog(DialogFrame[] dialogFrames)
    {
        _dialogFrames = dialogFrames;
        _nameMap = new Dictionary<int, string>();
        _dirMap = new Dictionary<int, string>();
        _nameMap[0] = "Mince Casserole";
        _nameMap[1] = "Kerry Juniper";
        _dirMap[0] = "LHS";
        _dirMap[1] = "RHS";
    }

    public DialogFrame[] DialogFrames
    {
        get { return _dialogFrames; }
    }

    public Dictionary<int, string> NameMap
    {
        get { return _nameMap; }
    }

    public Dictionary<int, string> DirMap
    {
        get { return _dirMap; }
    }
}
