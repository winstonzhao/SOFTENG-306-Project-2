using Game.Hiscores;
using Multiplayer;
using UserInterface;
using ElectricalScripts.Electrical_Scores;

public class Toolbox : Singleton<Toolbox>
{
    protected Toolbox()
    {
    }

    public FocusManager FocusManager
    {
        get { return FocusManager.Instance; }
    }

    public GameManager GameManager
    {
        get { return GameManager.Instance; }
    }

    public MultiplayerController MultiplayerController
    {
        get { return MultiplayerController.Instance; }
    }

    public Multiplayer.ChatController ChatController
    {
        get { return Multiplayer.ChatController.Instance; }
    }

    public Hiscores Hiscores
    {
        get { return Hiscores.Instance; }
    }

    public Electrical_Scores Electrical_Scores
    {
        get { return Electrical_Scores.Instance;  }
    }

    
}
