using System.Security.Cryptography.X509Certificates;
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

    public MultiplayerController MultiplayerController
    {
        get { return MultiplayerController.Instance; }
    }

    public ChatController ChatController
    {
        get { return ChatController.Instance; }
    }
}
