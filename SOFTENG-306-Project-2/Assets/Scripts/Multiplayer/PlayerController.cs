using System;
using UltimateIsometricToolkit.physics;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace Multiplayer
{
    /// <summary>
    /// Handles a given player - if it's us then update the player object with our game object position,
    /// otherwise set the game object position to the player object data. 
    /// </summary>
    public class PlayerController : MonoBehaviour
    {
        [NonSerialized]
        public Player Player;

        public bool Self;

        private float LastUpdateAt;

        private float SyncPeriod;

        private Player PrevPlayer;

        private IsoTransform Transform;

        private Animator MovementAnimator;

        private SpeechBubble SpeechBubble;

        private ChatController ChatController;

        private ChatMessage ActiveChatMessage;

        private int LastMessageIdSeen = -1;

        private void Awake()
        {
            Transform = GetComponent<IsoTransform>();

            if (Self)
            {
                Player = Toolbox.Instance.GameManager.Player;

                var activeScene = SceneManager.GetActiveScene();

                // Restore the player position from the previous session
                if (activeScene.name == Player.Scene)
                {
                    Transform.Position = new Vector3(Player.X, Player.Y, Player.Z);

                    // Update the ghost reference too - the player takes the position from the ghost so we need this
                    var ghostReference = GetComponent<GhostReference>();
                    if (ghostReference != null && ghostReference.GhostObject != null)
                    {
                        var ghost = ghostReference.GhostObject;
                        if (ghost != null)
                        {
                            ghost.transform.position = Transform.Position;
                        }
                    }
                }
            }

            MovementAnimator = GetComponent<Animator>();

            var sb = transform.Find("Speech Bubble");
            SpeechBubble = sb == null ? null : sb.GetComponent<SpeechBubble>();

            ChatController = Toolbox.Instance.ChatController;
        }

        private void Start()
        {
            SyncPeriod = Toolbox.Instance.MultiplayerController.SyncPeriod;

            MovementAnimator.SetFloat("hSpeed", 0.0f);
            MovementAnimator.SetFloat("vSpeed", 0.0f);
            MovementAnimator.SetBool("vIdle", true);
            MovementAnimator.SetBool("hIdle", true);
            MovementAnimator.SetBool("walking", false);
        }

        private void Update()
        {
            if (Player == null)
            {
                return;
            }

            UpdateChat();
            UpdatePosition();
        }

        private void UpdateChat()
        {
            if (ChatController == null || SpeechBubble == null)
            {
                return;
            }

            // Try getting a newer message 
            var lastChatMessageId = ChatController.LastChatMessageId;
            var message = ChatController.GetLastMessageBy(Player.Username, LastMessageIdSeen);
            LastMessageIdSeen = lastChatMessageId;

            if (message != null)
            {
                ActiveChatMessage = message;
            }

            var active = ActiveChatMessage;

            if (active != null)
            {
                SpeechBubble.Message = active.Message;

                // Get rid of the message if it's been shown for longer than the active duration
                if (DateTime.Now - active.SentAt > ChatController.ActiveDuration)
                {
                    active = null;
                    ActiveChatMessage = null;
                }
            }

            SpeechBubble.gameObject.SetActive(active != null);
        }

        private void UpdatePosition()
        {
            if (Self)
            {
                var activeScene = SceneManager.GetActiveScene();
                Player.X = Transform.Position.x;
                Player.Y = Transform.Position.y;
                Player.Z = Transform.Position.z;
                Player.Scene = activeScene.name;
            }
            else if (PrevPlayer != null)
            {
                // Interpolate the movement based on the previous location
                var progress = Mathf.Min(1.0f, (Time.time - LastUpdateAt) / SyncPeriod);

                var dx = Player.X - PrevPlayer.X;
                var dy = Player.Y - PrevPlayer.Y;
                var dz = Player.Z - PrevPlayer.Z;

                Transform.Position = new Vector3(
                    PrevPlayer.X + progress * dx,
                    PrevPlayer.Y + progress * dy,
                    PrevPlayer.Z + progress * dz
                );

                // The following code handles player direction/animation
                var hSpeed = dz;
                var vSpeed = dx;

                var hIdle = Mathf.Abs(hSpeed) <= 0.1;
                var vIdle = Mathf.Abs(vSpeed) <= 0.1;
                var walking = !vIdle || !hIdle;

                MovementAnimator.SetFloat("hSpeed", hSpeed);
                MovementAnimator.SetFloat("vSpeed", vSpeed);
                MovementAnimator.SetBool("hIdle", progress < 1.0f && hIdle);
                MovementAnimator.SetBool("vIdle", progress < 1.0f && vIdle);
                MovementAnimator.SetBool("walking", progress < 1.0f && walking);
            }
        }

        public void SetPlayer(Player player)
        {
            PrevPlayer = Player;
            Player = player;

            // Set the position immediately, otherwise interpolate the changes in Update()
            if (PrevPlayer == null)
            {
                Transform.Position = new Vector3(Player.X, Player.Y, Player.Z);
            }

            LastUpdateAt = Time.time;
        }
    }
}
