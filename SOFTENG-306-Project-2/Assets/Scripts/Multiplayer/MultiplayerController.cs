using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using WebSocketSharp;

namespace Multiplayer
{
    public class MultiplayerController : MonoBehaviour
    {
        public bool Enabled;

        public float SyncPeriod = 0.3f;

        private bool Connected;

        private WebSocket WebSocket;

        private Player MyPlayer;

        private float LastSyncAt;

        private Dictionary<string, Player> Players;

        public void Register(Player player)
        {
            MyPlayer = player;
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

            Players = players;
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
            var players = Players;

            if (players == null)
            {
                return;
            }

            while (transform.childCount != 0)
            {
                foreach (Transform child in transform)
                {
                    DestroyImmediate(child.gameObject);
                }
            }

            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Prefabs/Multiplayer.prefab");

            foreach (var pair in players)
            {
                var player = pair.Value;

                if (player.username == MyPlayer.username)
                {
                    continue;
                }

                var gameObject = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
                gameObject.transform.parent = transform;
                gameObject.GetComponent<PlayerController>().SetPlayer(player);
            }
        }
    }
}
