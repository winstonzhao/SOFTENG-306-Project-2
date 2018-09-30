using System;
using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Multiplayer
{
    public class PlayerController : MonoBehaviour
    {
        public string Username = "meme-man";

        public string Scene = "meme-town";

        public bool Self;

        private float LastUpdateAt;

        private float SyncPeriod;

        private Player PrevPlayer;

        private Player Player;

        private IsoTransform Transform;

        private Animator MovementAnimator;

        private void Awake()
        {
            if (Self)
            {
                var userId = Mathf.RoundToInt(Random.value * 10000);
                Username += "-";
                Username += userId;
                Player = new Player { username = Username, scene = Scene };
            }

            Transform = GetComponent<IsoTransform>();

            MovementAnimator = GetComponent<Animator>();
        }

        private void Start()
        {
            var multiplayer = FindObjectOfType<MultiplayerController>();

            if (Self)
            {
                multiplayer.Register(Player);
            }

            SyncPeriod = multiplayer.SyncPeriod;

            MovementAnimator.SetFloat("hSpeed", 0.0f);
            MovementAnimator.SetFloat("vSpeed", 0.0f);
            MovementAnimator.SetBool("vIdle", true);
            MovementAnimator.SetBool("hIdle", true);
            MovementAnimator.SetBool("walking", true);
        }

        private void Update()
        {
            if (Player == null)
            {
                return;
            }

            if (Self)
            {
                Player.x = Transform.Position.x;
                Player.y = Transform.Position.y;
                Player.z = Transform.Position.z;
            }
            else if (PrevPlayer != null)
            {
                // Interpolate the movement based on the previous location
                var progress = Mathf.Min(1.0f, (Time.time - LastUpdateAt) / SyncPeriod);

                var dx = Player.x - PrevPlayer.x;
                var dy = Player.y - PrevPlayer.y;
                var dz = Player.z - PrevPlayer.z;

                Transform.Position = new Vector3(
                    PrevPlayer.x + progress * dx,
                    PrevPlayer.y + progress * dy,
                    PrevPlayer.z + progress * dz
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
            Username = player.username;

            // Set the position immediately, otherwise interpolate the changes in Update()
            if (PrevPlayer == null)
            {
                Transform.Position = new Vector3(Player.x, Player.y, Player.z);
            }

            LastUpdateAt = Time.time;
        }
    }
}
