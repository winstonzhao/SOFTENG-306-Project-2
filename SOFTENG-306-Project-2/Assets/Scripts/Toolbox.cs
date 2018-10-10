using Game;
using Game.Hiscores;
using Multiplayer;
using UserInterface;

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
}
