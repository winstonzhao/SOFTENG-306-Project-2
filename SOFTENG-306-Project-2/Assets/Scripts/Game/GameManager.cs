using System;
using System.IO;
using Multiplayer;
using Quests;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Game
{
    /// <summary>
    /// Manages the game, handles settings, current player etc
    /// </summary>
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

        /// <summary>
        /// The player object that represents our in-game player
        /// </summary>
        public Player Player
        {
            get { return Settings.Player; }
            set { Settings.Player = value; }
        }

        /// <summary>
        /// Debounce the save to avoid writing to disk too frequently
        /// </summary>
        private readonly DebounceAction DebounceSaveSettings;

        /// <summary>
        /// Used to periodically save - determines when we last saved i.e. figure out when our next save should be
        /// </summary>
        private float LastSavedAt;

        public GameManager()
        {
            var every = TimeSpan.FromSeconds(1);
            DebounceSaveSettings = new DebounceAction(every, SaveSettings);
        }

        private void Awake()
        {
            // If we don't have a stored player object then create one with no username
            // The user will pick this username granted they go through the main menu
            if (Player == null)
            {
                Player = new Player { Username = null };
            }
        }

        public void Update()
        {
            LastSavedAt += Time.deltaTime;

            // Periodically save every 2.5 seconds - player object changes without triggering the save
            if (LastSavedAt > 2.5f)
            {
                LastSavedAt = 0f;
                DebounceSaveSettings.Run();
            }
        }

        public void ChangeScene(string levelName)
        {
            Initiate.Fade(levelName, Color.white, 3f);
        }

        public void QuitGame()
        {
            SaveSettings();

            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif

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
