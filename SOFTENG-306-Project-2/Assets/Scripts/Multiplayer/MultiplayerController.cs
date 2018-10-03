using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using WebSocketSharp;

namespace Multiplayer
{
    public class MultiplayerController : Singleton<MultiplayerController>
    {
        public string Host = "wss://ododo.herokuapp.com";

        public TimeSpan TimeDrift { get; private set; }

        /// <summary>
        /// The time in seconds to wait before syncing local player data to the server
        /// </summary>
        [NonSerialized]
        public float SyncPeriod = 0.3f;

        private GameManager GameManager;

        private ChatController ChatController;

        private bool Connected;

        private WebSocket WebSocket;

        /// <summary>
        /// Stores the previously sent player position sent to the server. 
        /// Used to determine whether to send the user data in the next update
        /// </summary>
        private float SentX, SentY, SentZ;

        /// <summary>
        /// Similar to <see cref="SentX"/> etc, stores the previously sent player's scene
        /// </summary>
        private string SentScene;

        private string ActiveSceneName;

        private GameObject PlayerPrefab;

        private float LastSyncAt;

        private Dictionary<string, Player> Players;

        /// <summary>
        /// Whether the <see cref="Players"/> object has changed
        /// </summary>
        private bool IsPlayersDirty;

        /// <summary>
        ///  Positive when there are more new players compared to the previous tick
        /// </summary>
        private int PlayerCountChange;

        public void SendAsync(string payload, Action<bool> completed)
        {
            // This is due to Unity hot reload
            if (Connected && WebSocket == null)
            {
                if (Debug.isDebugBuild)
                {
                    Debug.LogWarning("Reconnecting; this is most likely due to Unity hot reload");
                    Connect(false);
                }
                else
                {
                    Debug.LogError("Web socket is null but connection should be open");
                    return;
                }
            }

            if (!Connected || WebSocket == null)
            {
                return;
            }

            WebSocket.SendAsync(payload, completed);
        }

        private void Awake()
        {
            TimeDrift = TimeSpan.Zero;

            PlayerPrefab = Resources.Load<GameObject>("Prefabs/Multiplayer");

            GameManager = Toolbox.Instance.GameManager;

            ChatController = Toolbox.Instance.ChatController;
        }

        private void Start()
        {
            if (!Connected)
            {
                Connect();
            }
        }

        private void OnDestroy()
        {
            if (WebSocket != null)
            {
                WebSocket.Close();
            }

            Connected = false;
        }

        private void Connect(bool async = true)
        {
            // Don't reconnect for no reason
            if (WebSocket != null && WebSocket.IsAlive)
            {
                return;
            }

            WebSocket = new WebSocket(Host + "/play");
            WebSocket.OnOpen += OnSocketOpen;
            WebSocket.OnMessage += OnGameInit;
            WebSocket.OnMessage += OnGameSync;
            WebSocket.OnError += OnSocketError;

            if (ChatController != null)
            {
                WebSocket.OnMessage += OnChatMessages;
            }

            if (async)
            {
                WebSocket.ConnectAsync();
            }
            else
            {
                WebSocket.Connect();
            }
        }

        private void OnSocketOpen(object sender, object arg)
        {
            Connected = true;
        }

        private void OnGameInit(object sender, MessageEventArgs evt)
        {
            const string prefix = "init\n";

            var data = evt.Data;

            if (!data.StartsWith(prefix))
            {
                return;
            }

            var json = data.Substring(prefix.Length);
            var response = JsonUtility.FromJson<GameInitialization>(json);
            if (response != null)
            {
                SyncPeriod = response.tickPeriod / 1000.0f;
                SetTimeDriftFrom(response.currentDateTime);
            }
        }

        private void OnGameSync(object sender, MessageEventArgs evt)
        {
            const string prefix = "sync\n";

            var data = evt.Data;

            if (!data.StartsWith(prefix))
            {
                return;
            }

            var json = data.Substring(prefix.Length);
            var response = JsonUtility.FromJson<GameSync>(json);
            if (response != null)
            {
                Sync(response);
            }
        }

        private void OnChatMessages(object sender, MessageEventArgs evt)
        {
            const string prefix = "get-messages\n";

            var data = evt.Data;

            if (!data.StartsWith(prefix))
            {
                return;
            }

            var json = data.Substring(prefix.Length);
            var response = JsonUtility.FromJson<GetMessages>(json);
            if (response != null)
            {
                ChatController.Sync(response.messages);
            }
        }

        private void OnSocketError(object sender, ErrorEventArgs evt)
        {
            Debug.LogError(evt);
            Debug.LogError(evt.Exception.StackTrace);
        }

        private void Sync(GameSync sync)
        {
            var players = new Dictionary<string, Player>();

            foreach (var player in sync.players)
            {
                if (player.Scene == GameManager.Player.Scene)
                {
                    players[player.Username] = player;
                }
            }

            var prevCount = Players == null ? 0 : Players.Count;
            PlayerCountChange = players.Count - prevCount;
            Players = players;
            IsPlayersDirty = true;

            if (ChatController != null)
            {
                ChatController.Sync(sync.lastChatMessageId);
            }

            SetTimeDriftFrom(sync.currentTime);
        }

        private void SetTimeDriftFrom(string time)
        {
            TimeDrift = DateTime.Now - DateTime.Parse(time);
        }

        private void Update()
        {
            var player = GameManager.Player;

            if (player == null || !Connected)
            {
                return;
            }

            // Delete all the players if we switch scenes
            if (SentScene != player.Scene)
            {
                Players = null;
            }

            UpdatePlayers();

            // Only send player data to server if it changed
            if (SentScene != player.Scene || SentX != player.X || SentY != player.Y || SentZ != player.Z)
            {
                var now = Time.time;

                if (now - LastSyncAt > SyncPeriod)
                {
                    LastSyncAt = now;

                    SentX = player.X;
                    SentY = player.Y;
                    SentZ = player.Z;
                    SentScene = player.Scene;

                    var json = JsonUtility.ToJson(player);
                    SendAsync("player-sync\n" + json, success => { });
                }
            }
        }

        private void UpdatePlayers()
        {
            if (!IsPlayersDirty)
            {
                return;
            }

            IsPlayersDirty = false;

            var players = Players;

            if (players == null)
            {
                return;
            }

            var hasNewPlayer = false;

            foreach (var pair in players)
            {
                var player = pair.Value;

                if (player.Username == GameManager.Player.Username)
                {
                    continue;
                }

                var objectName = "Player " + player.Username;

                var find = transform.Find(objectName);
                GameObject playerObject = find != null ? find.gameObject : null;

                if (playerObject == null)
                {
                    playerObject = Instantiate(PlayerPrefab);
                    playerObject.name = objectName;
                    playerObject.transform.parent = transform;
                    hasNewPlayer = true;
                }

                playerObject.GetComponent<PlayerController>().SetPlayer(player);
            }

            // Remove orphan player objects i.e. players that don't exist on the server anymore
            // Check for new player in case someone joined as someone else left i.e. player count didn't change
            if (PlayerCountChange != 0 || hasNewPlayer)
            {
                foreach (Transform child in transform)
                {
                    var pc = child.GetComponent<PlayerController>();
                    var username = pc.Player.Username;
                    if (!players.ContainsKey(username))
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
        }
    }
}
