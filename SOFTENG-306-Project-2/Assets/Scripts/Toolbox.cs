using Achievements;
using Game;
using Game.Hiscores;
using Multiplayer;
using Quests;
using UserInterface;
using Utils;

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

    public QuestManager QuestManager
    {
        get { return QuestManager.Instance; }
    }

    public JsonFiles JsonFiles
    {
        get { return JsonFiles.Instance; }
    }

    public AchievementsManager AchievementsManager
    {
        get { return AchievementsManager.Instance; }
    }

    public Notifications Notifications
    {
        get { return Notifications.Instance; }
    }

    public UIManager UIManager
    {
        get { return UIManager.Instance; }
    }
}
