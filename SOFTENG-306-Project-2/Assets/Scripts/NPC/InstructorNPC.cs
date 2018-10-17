using System.Collections.Generic;
using GameDialog;

public abstract class InstructorNPC : NPC
{
    protected abstract string Name { get; }

    protected abstract string QuestId { get; }

    protected abstract DialogFrame GetInstructorDialog(string me);

    // Use this for initialization
    public override Dialog GetDialog()
    {
        var me = Toolbox.Instance.GameManager.Player.Username;

        DialogFrame frame;

        if (Toolbox.Instance.QuestManager.Current.Id == "networking")
        {
            frame = new DialogFrame(Name, "Naomi is looking for you upstairs!");
        }
        else if (Toolbox.Instance.QuestManager.HasFinishedOrIsCurrent(QuestId))
        {
            frame = GetInstructorDialog(me);
        }
        else
        {
            var current = Toolbox.Instance.QuestManager.Current.Title;
            // Capitalize first character & lowercase everything else
            current = current.Substring(0, 1).ToUpper() + current.Substring(1).ToLower();

            frame = new DialogFrame(Name,
                "Sorry it looks like you aren't enrolled for this workshop right now, check your timetable to see " +
                "where you should be going.")
            {
                Next = new DialogFrame(Name, "It looks like you're currently enrolled in the " + current + ".")
            };
        }

        var directions = new Dictionary<string, DialogPosition>
        {
            { me, DialogPosition.Left }, { Name, DialogPosition.Right }
        };

        return new Dialog(frame, directions);
    }
}
