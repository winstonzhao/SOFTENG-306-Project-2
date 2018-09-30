using System.Security.Cryptography.X509Certificates;
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
}
