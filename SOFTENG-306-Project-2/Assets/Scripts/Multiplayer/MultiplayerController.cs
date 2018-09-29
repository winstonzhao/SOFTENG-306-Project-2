using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

namespace Multiplayer
{
    public class MultiplayerController : MonoBehaviour
    {
        public bool Enabled;

        /// <summary>
        /// The time in seconds to wait before syncing local player data to the server 
        /// </summary>
        public float SyncPeriod = 0.3f;

        private bool Connected;

        private WebSocket WebSocket;

        private Player MyPlayer;

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

        public void Register(Player player)
        {
            MyPlayer = player;
        }

        private void Awake()
        {
            PlayerPrefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Multiplayer.prefab");
        }

        private void Start()
        {
            if (!Enabled)
            {
                return;
            }

            if (!Connected)
            {
                Connected = true;

                WebSocket = new WebSocket("wss://ododo.herokuapp.com/play");

                WebSocket.OnMessage += (sender, evt) =>
                {
                    var data = evt.Data;
                    if (data.StartsWith("sync\n"))
                    {
                        var response = data.Substring("sync\n".Length);
                        var sync = JsonUtility.FromJson<GameSync>(response);
                        if (sync != null)
                        {
                            Sync(sync);
                        }
                    }
                };

                WebSocket.OnError += (sender, e) =>
                {
                    Debug.LogError(e);
                    Debug.LogError(e.Exception.StackTrace);
                };

                WebSocket.Connect();
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

        private void Sync(GameSync sync)
        {
            var players = new Dictionary<string, Player>();

            foreach (var player in sync.players)
            {
                players[player.username] = player;
            }

            var prevCount = Players == null ? 0 : Players.Count;
            PlayerCountChange = players.Count - prevCount;
            Players = players;
            IsPlayersDirty = true;
        }

        private void Update()
        {
            if (MyPlayer == null || WebSocket == null)
            {
                return;
            }

            UpdatePlayers();

            var now = Time.time;

            if (now - LastSyncAt > SyncPeriod)
            {
                LastSyncAt = now;

                var json = JsonUtility.ToJson(MyPlayer);
                WebSocket.SendAsync("player-sync\n" + json, (success) => { });
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

                if (player.username == MyPlayer.username)
                {
                    continue;
                }

                var objectName = "Player " + player.username;

                var find = transform.Find(objectName);
                GameObject playerObject = find != null ? find.gameObject : null;

                if (playerObject == null)
                {
                    playerObject = PrefabUtility.InstantiatePrefab(PlayerPrefab) as GameObject;
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
                    var username = child.GetComponent<PlayerController>().Username;
                    if (!players.ContainsKey(username))
                    {
                        DestroyImmediate(child.gameObject);
                    }
                }
            }
        }
    }
}
