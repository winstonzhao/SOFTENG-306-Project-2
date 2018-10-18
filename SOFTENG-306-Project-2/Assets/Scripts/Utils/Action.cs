namespace Utils
{
    public delegate void Runnable();

    /// <summary>
    /// Some action to be passed around to run - can be inherited from which is where this becomes more
    /// powerful than simply handing around a delegate/lambda.
    /// </summary>
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
