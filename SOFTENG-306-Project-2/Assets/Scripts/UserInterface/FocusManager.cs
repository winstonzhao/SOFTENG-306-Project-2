using GameDialog;
using UnityEngine;
using UnityEngine.UI;

namespace UserInterface
{
    public class FocusManager : Singleton<FocusManager>
    {
        protected FocusManager()
        {
        }

        public Selectable Focus;

        public Dialog Dialog;

        public Object Other;

        public bool HasBlocking
        {
            get { return Focus != null || Dialog != null || Other != null; }
        }

        public void Update()
        {
            Selectable focus = null;

            foreach (var selectable in Selectable.allSelectables)
            {
                var inputField = selectable as InputField;

                if (inputField != null && inputField.isFocused)
                {
                    focus = inputField;
                    break;
                }
            }

            Focus = focus;
        }
    }
}
