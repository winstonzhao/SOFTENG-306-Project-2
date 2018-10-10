namespace Utils
{
    public delegate void Runnable();

    public class Action
    {
        private Runnable Runnable;

        public Action(Runnable runnable)
        {
            Runnable = runnable;
        }

        public virtual void Run()
        {
            Runnable();
        }
    }
}
