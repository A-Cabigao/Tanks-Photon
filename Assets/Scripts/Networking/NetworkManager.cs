using System.Collections.Generic;
using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

using Photon.Pun.UtilityScripts;
using System;

namespace QLE
{
    public class SpriteLook{
        /// <summary>
        /// For sprite renderer
        /// </summary>
        public Sprite worldLook;
        public Color color;
    }
    /// <summary>
    /// Handles updating of player Id and player sprite upon player enter and exit
    /// </summary>
    public class NetworkManager : SingletonPUN<NetworkManager>
    {
        [Header("Different player look")]
        [SerializeField] SpriteLook[] playerLooks;

        /// <summary>
        /// contains the current player's id
        /// </summary>
        [SerializeField] int ownerId;

        public static Action OnPlayerNumberingUpdated;

        public override void OnEnable() => PlayerNumbering.OnPlayerNumberingChanged += UpdatePlayerNumbering;
        public override void OnDisable() => PlayerNumbering.OnPlayerNumberingChanged -= UpdatePlayerNumbering;

        void Start()
        {
            //Make sure to load the Lobby Scene first if we are not yet connected to the Photon Services
            if (!PhotonNetwork.IsConnected)
            {
                LevelManager.Instance.LoadMasterScene(Level.Lobby);
                return;
            }

            InitializeOwnerId();
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Log(string.Format("<color=green>{0}</color> has entered the room! #of Players: {1}/{2}", 
                newPlayer.NickName, PhotonNetwork.CurrentRoom.PlayerCount, PhotonNetwork.CurrentRoom.MaxPlayers));
        }
        [ContextMenu("Initialize Player Look")]
        public void InitializePlayerLook(){
            if(playerLooks == null){
                playerLooks = new SpriteLook[3];
                string[] hexCodes = {"#AD1F1F","#1FAD1F","#1F1FAD"};
                for (int i = 0; i < hexCodes.Length; i++)
                    ColorUtility.TryParseHtmlString(hexCodes[i],out playerLooks[i].color);
            }
        }
        void InitializeOwnerId() => ownerId = PhotonNetwork.LocalPlayer.ActorNumber;
        void UpdatePlayerNumbering()
        {
            //Do the behaviours related to updating the player numbering
            OnPlayerNumberingUpdated?.Invoke();
        }

        public void Log(string message) => UILog.Log(message);
        public SpriteLook GetPlayerLook(int index) => playerLooks[index];
    }
}