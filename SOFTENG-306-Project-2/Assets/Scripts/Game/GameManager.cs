using Multiplayer;
using UnityEngine;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        public Player Player;

        private void Awake()
        {
            var userId = Mathf.RoundToInt(Random.value * 100000);
            Player = new Player { Username = "Luna Lovegood " + userId };
        }

        public void ChangeScene(string levelName)
        {
            Initiate.Fade(levelName, Color.black, 1.0f);
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
