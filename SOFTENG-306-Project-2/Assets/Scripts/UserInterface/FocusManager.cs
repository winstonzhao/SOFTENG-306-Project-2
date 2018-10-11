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
