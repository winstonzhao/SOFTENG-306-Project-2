using System;
using UnityEngine;
using UnityEngine.UI;
using WebSocketSharp;

namespace Game.Hiscores.UserInterface
{
    public class HiscoreController : MonoBehaviour
    {
        private Minigames minigame;

        public Minigames Minigame
        {
            get { return minigame; }
            set
            {
                minigame = value;

                OnMinigameChange.Emit(this, EventArgs.Empty);
            }
        }

        public event EventHandler OnMinigameChange;

        private void Start()
        {
            var backButton = transform.Find("Back Button").GetComponent<Button>();
            backButton.onClick.AddListener(() => { Toolbox.Instance.GameManager.ChangeScene("Menu"); });
        }

        public void Open()
        {
            gameObject.SetActive(true);
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}
