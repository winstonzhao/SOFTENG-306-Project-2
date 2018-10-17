using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace GameDialog
{
    public class DialogCanvasManager : MonoBehaviour
    {
        public bool Showing;
        private Sprite LhsSprite;
        private Sprite RhsSprite;
        private Image Avatar;
        private GameObject GameFrame;
        private Text GameFrameDialogueText;
        private Dialog CurrentDialog;
        private DialogFrame CurrentDialogFrame;
        private bool IsTypingText;
        private bool IsFrameSkippable;
        private Button[] Buttons = new Button[0];

        public void ShowDialog(Dialog dialog, Sprite lhs, Sprite rhs)
        {
            Toolbox.Instance.FocusManager.Dialog = dialog;

            LhsSprite = lhs;
            RhsSprite = rhs;
            IsFrameSkippable = false;
            Showing = true;
            CurrentDialog = dialog;

            ShowFrame(dialog.StartFrame);
        }

        /// <summary>
        /// Renders the current game frame e.g. animate the text coming in
        /// </summary>
        /// <param name="frame"> </param>
        /// <returns></returns>
        private IEnumerator RenderFrame(DialogFrame frame)
        {
            IsTypingText = true;
            GameFrameDialogueText.text = "";

            // Show a new character on each frame
            foreach (var c in frame.Text)
            {
                if (GameFrameDialogueText.text.Length > 10)
                {
                    IsFrameSkippable = true;
                }

                GameFrameDialogueText.text += c;
                yield return null;
            }

            IsTypingText = false;
            IsFrameSkippable = true;

            // Move to new scene upon finish
            if (frame.TransitionFrame)
            {
                // Add a slight delay before transitioning
                yield return new WaitForSeconds(0.25f);

                OnCompleteFrame();
                CloseDialog();
                Toolbox.Instance.GameManager.ChangeScene(frame.TransitionToScene);
            }
        }

        private void CloseDialog()
        {
            if (CurrentDialog.OnComplete != null)
            {
                CurrentDialog.OnComplete();
            }

            Toolbox.Instance.FocusManager.Dialog = null;
            DestroyGameFrame();
            StartCoroutine(UnsetShowingAfter(0.25f));
        }

        /// <summary>
        /// Wait a bit so that we don't accidentally open the same dialog
        /// </summary>
        /// <returns>Co-routine to run</returns>
        private IEnumerator UnsetShowingAfter(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            Showing = false;
        }

        private void ShowFrame(DialogFrame frame)
        {
            // Setup user interface
            CurrentDialogFrame = frame;
            SetupGameFrame();
            SetupAvatar();

            // Animate the next frame
            StopCoroutine("RenderFrame");
            StartCoroutine("RenderFrame", frame);
        }

        private void SetupGameFrame()
        {
            if (Avatar != null)
            {
                DestroyGameFrame();
            }

            var gameFrameType = CurrentDialogFrame.ButtonFrame ? "Button" : "Text";
            var gameFramePrefab = Resources.Load<GameObject>("Prefabs/Dialog/" + gameFrameType + " Panel");
            var gameFrame = Instantiate(gameFramePrefab, gameObject.transform);

            var nameText = gameFrame.transform.Find("Character Name Text");
            nameText.GetComponent<Text>().text = CurrentDialogFrame.Name;

            GameFrameDialogueText = gameFrame.transform.Find("Dialogue Text").GetComponent<Text>();
            GameFrame = gameFrame;

            if (CurrentDialogFrame.ButtonFrame)
            {
                SetupButtonFrame();
            }

            var imagePrefab = Resources.Load<GameObject>("Prefabs/Dialog/Image");
            var image = Instantiate(imagePrefab, gameObject.transform);
            Avatar = image.GetComponent<Image>();
        }

        /// <summary>
        /// Setup the buttons in the existing game frame
        ///
        /// Note: there are 4 existing buttons on the frame, we find these and set them up.
        ///
        /// Any unused buttons are destroyed.
        /// </summary>
        private void SetupButtonFrame()
        {
            var buttonNo = 1;

            var options = CurrentDialogFrame.Options;

            var buttons = new Button[options.Count];

            // Setup the buttons to show the options
            foreach (var entry in options)
            {
                var text = entry.Key;
                var nextFrame = entry.Value;

                var button = GameFrame.transform.Find("Button " + buttonNo).GetComponent<Button>();
                button.transform.Find("Text").GetComponent<Text>().text = buttonNo + ". " + text;
                button.onClick.AddListener(() => { ShowFrame(nextFrame); });

                buttons[buttonNo - 1] = button;
                buttonNo++;
            }

            Buttons = buttons;

            // Delete the remaining/unused buttons 
            for (var i = buttonNo; i <= 4; i++)
            {
                var button = GameFrame.transform.Find("Button " + i);
                Destroy(button.gameObject);
            }
        }

        private void SkipFrame()
        {
            if (CurrentDialogFrame.TransitionFrame || !IsFrameSkippable)
            {
                return;
            }

            StopCoroutine("RenderFrame");

            // Skip text animation first
            if (IsTypingText)
            {
                IsTypingText = false;
                GameFrameDialogueText.text = CurrentDialogFrame.Text;
                return;
            }

            // Require the user to pick an option to continue
            if (CurrentDialogFrame.ButtonFrame)
            {
                return;
            }

            OnCompleteFrame();

            // Close dialog if there is none left
            if (CurrentDialogFrame.Next == null)
            {
                CloseDialog();
                return;
            }

            // Move to the next frame
            ShowFrame(CurrentDialogFrame.Next);
        }

        private void OnCompleteFrame()
        {
            if (CurrentDialogFrame.OnComplete != null)
            {
                CurrentDialogFrame.OnComplete();
            }
        }

        private void DestroyGameFrame()
        {
            Destroy(GameFrame);
            Buttons = new Button[0];

            if (Avatar != null)
            {
                Destroy(Avatar.gameObject);
            }

            Avatar = null;
            GameFrame = null;
            GameFrameDialogueText = null;
        }

        private void SetupAvatar()
        {
            switch (CurrentDialog.Directions[CurrentDialogFrame.Name])
            {
                case DialogPosition.Left:
                    SetupLhsAvatar();
                    break;
                case DialogPosition.Right:
                    SetupRhsAvatar();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void SetupLhsAvatar()
        {
            float scale = GetComponent<RectTransform>().localScale.x;
            Avatar.sprite = LhsSprite;

            var avatarTransform = Avatar.GetComponent<RectTransform>();
            var position = avatarTransform.position;
            position[0] = 100 * scale;
            position[1] = 75 * scale;
            position[2] = 0;
            avatarTransform.position = position;

            var gameFrameTransform = GameFrame.GetComponent<RectTransform>();
            position = gameFrameTransform.position;
            position[0] = 100 * scale;
            position[1] = 90 * scale;
            position[2] = 0;
            gameFrameTransform.position = position;
        }

        private void SetupRhsAvatar()
        {
            float scale = GetComponent<RectTransform>().localScale.x;
            Avatar.sprite = RhsSprite;

            var avatarTransform = Avatar.GetComponent<RectTransform>();
            var position = avatarTransform.position;
            position[0] = 700 * scale;
            position[1] = 75 * scale;
            position[2] = 0;
            avatarTransform.position = position;

            var gameFrameTransform = GameFrame.GetComponent<RectTransform>();
            position = gameFrameTransform.position;
            position[0] = 50 * scale;
            position[1] = 90 * scale;
            position[2] = 0;
            gameFrameTransform.position = position;
        }

        private void Start()
        {
            // Place dialog higher than UI
            GetComponent<Canvas>().sortingOrder = 600;
        }

        private void Update()
        {
            if (!Showing)
            {
                return;
            }

            // Space to continue
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SkipFrame();
            }
            else
            {
                // Press number to act on associated button
                for (var index = 0; index < Buttons.Length; index++)
                {
                    var button = Buttons[index];
                    var key = (index + 1).ToString();
                    if (Input.GetKeyDown(key))
                    {
                        button.onClick.Invoke();
                    }
                }
            }
        }

        private void OnDestroy()
        {
            var toolbox = Toolbox.Instance;

            // If the application is being destroyed, the singleton may also be destroyed at this point
            if (toolbox == null)
            {
                return;
            }

            var focusManager = toolbox.FocusManager;

            // Ditto
            if (focusManager == null)
            {
                return;
            }

            // Clear the dialog if we're getting destroyed
            if (focusManager.Dialog == CurrentDialog)
            {
                focusManager.Dialog = null;
            }
        }
    }
}
