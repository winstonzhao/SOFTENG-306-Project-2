using Ultimate_Isometric_Toolkit.Scripts.Core;
using UnityEngine;

namespace Multiplayer
{
    public class PlayerController : MonoBehaviour
    {
        public string Username = "meme-man";

        public string Scene = "meme-town";

        public bool Self;

        private Player Player;

        private IsoTransform Transform;

        private void Awake()
        {
            Player = new Player
            {
                username = Username, scene = Scene
            };

            Transform = GetComponent<IsoTransform>();

            if (Self)
            {
                FindObjectOfType<MultiplayerController>().Register(Player);
            }
        }

        private void Update()
        {
            Player.x = Transform.Position.x;
            Player.y = Transform.Position.y;
            Player.z = Transform.Position.z;
        }

        public void SetPlayer(Player player)
        {
            Player = player;
            Username = player.username;
            Transform.Position = new Vector3(player.x, player.y, player.z);
        }
    }
}
