using System;
using System.IO;
using Multiplayer;
using Quests;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Game
{
    public class GameManager : Singleton<GameManager>
    {
        private GameSettings settings;
        public GameSettings Settings
        {
            get
            {
                if (settings == null)
                {
                    settings = LoadSettings();
                }

                return settings;
            }
        }

        public Player Player
        {
            get { return Settings.Player; }
            set { Settings.Player = value; }
        }

        private readonly DebounceAction DebounceSaveSettings;

        public GameManager()
        {
            var every = TimeSpan.FromSeconds(1);
            DebounceSaveSettings = new DebounceAction(every, SaveSettings);
        }

        private void Awake()
        {
            if (Player == null)
            {
                var userId = Mathf.RoundToInt(Random.value * 100000);
                Player = new Player { Username = "Luna Lovegood " + userId };
            }
        }

        public void Update()
        {
            if (Settings.IsDirty)
            {
                Settings.IsDirty = false;
                DebounceSaveSettings.Run();
            }
        }

        public void ChangeScene(string levelName)
        {
            Initiate.Fade(levelName, Color.black, 1.0f);
        }

        public void QuitGame()
        {
            SaveSettings();
            Application.Quit();
        }

        /// <summary>
        /// To be invoked on wake, do not invoke again unless required
        /// </summary>
        private GameSettings LoadSettings()
        {
            return Toolbox.Instance.JsonFiles.Read<GameSettings>("settings.dat") ?? new GameSettings();
        }

        /// <summary>
        /// Do not invoke directly unless required - use <see cref="DebounceSaveSettings"/>
        /// </summary>
        private void SaveSettings()
        {
            Toolbox.Instance.JsonFiles.Write("settings.dat", Settings);
        }
    }
}
