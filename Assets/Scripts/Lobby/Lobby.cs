using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System.Collections.Generic;

namespace QLE
{
    /// <summary>
    /// handles joining and creating a lobby to join
    /// </summary>
    public class Lobby : MonoBehaviourPunCallbacks, IConnectionCallbacks,IInRoomCallbacks
    {
        [SerializeField] string gameVersion = "1";
        [SerializeField] byte maxPlayersPerRoom = 3;
        [SerializeField] GameObject loadingPanel, controlPanel;
        [SerializeField] GameObject waitingCanvas;

        void Start()
        {
            controlPanel.SetActive(true);
            loadingPanel.SetActive(false);
            waitingCanvas.SetActive(false);
            // has to be called by all clients so that they sync to the master client if he loads a different level
            PhotonNetwork.AutomaticallySyncScene = true;
        }
        void OnDestroy() {
            for (int i = 0; i < views.Length; i++)
            {
                if(views[i])
                    Destroy(views[i].gameObject);
            }
        }
        #region MonobehaviourPunCallbacks

        public override void OnConnectedToMaster()
        {
            Log("<color=green>Connected to Master!</color>");
            Debug.Log("Connected to Master");
            //Try joining a random room
            Log("Trying to join a random room");
            PhotonNetwork.JoinRandomRoom();
        }
        public override void OnMasterClientSwitched(Player newMasterClient){
            base.OnMasterClientSwitched(newMasterClient);
            if(PhotonNetwork.IsConnected){
                startGame.gameObject.SetActive(true);
            }
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            if(controlPanel){
                controlPanel.SetActive(true);
                loadingPanel.SetActive(false);
            }
            Debug.Log("Disconnected because " + cause);
        }

        public override void OnJoinedRoom()
        {
            Log("<color=green>Successfully joined a room!</color>");
            Log(string.Format("{0}/{1} Players in {2}",
                PhotonNetwork.CurrentRoom.PlayerCount, 
                PhotonNetwork.CurrentRoom.MaxPlayers,
                PhotonNetwork.CurrentRoom.Name));

            EnableWaitingRoom();
        }

        void EnableWaitingRoom()
        {
            // disable loading
            loadingPanel.SetActive(false);

            //startGame.gameObject.SetActive(PhotonNetwork.IsMasterClient);
            // enable canvas
            waitingCanvas.SetActive(true);
            
            // add existing players into local waiting list
            int index = 0;
            foreach (KeyValuePair<int,Player> connectedPlayer in PhotonNetwork.CurrentRoom.Players)
            {
                GameObject viewGO = Instantiate(viewPrefab,playerList);
                PlayerLobbyView lobbyView = viewGO.GetComponent<PlayerLobbyView>();

                views[index] = lobbyView;
                views[index++].Player = connectedPlayer.Value;
            }
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Log("<color=red>Failed to join a random room: </color>" + message);
            //If no rooms are available or all rooms are at max capacity, simply create a new room
            Log("Creating a room...");
            PhotonNetwork.CreateRoom(PhotonNetwork.NickName + "'s Room", new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }
        #endregion

        [Header("Waiting")]
        [SerializeField] GameObject viewPrefab;
        [SerializeField] string viewPrefabName;
        PlayerLobbyView[] views = new PlayerLobbyView[3];
        
        [SerializeField] Button startGame;
        [SerializeField] Transform playerList;

        #region MonoBehaviourPunCallbacks
        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            base.OnPlayerEnteredRoom(newPlayer);
            Log(newPlayer.NickName + " joined");
            
            // add player view
            GameObject viewGO = Instantiate(viewPrefab,playerList);
            PlayerLobbyView lobbyView = viewGO.GetComponent<PlayerLobbyView>();
            for (int i = 0; i < views.Length; i++)
            {
                if(!views[i]){
                    views[i] = lobbyView;
                    views[i].Player = newPlayer;
                }
            }
            
            // enable start game
            if(PhotonNetwork.CurrentRoom.PlayerCount > 1 && PhotonNetwork.IsMasterClient){
                startGame.interactable = true;
                startGame.gameObject.SetActive(true);
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerLeftRoom(otherPlayer);
            Log(otherPlayer.NickName + " left");

            // remove player view
            for (int i = 0; i < views.Length; i++)
            {
                if(views[i] && views[i].Player == otherPlayer){
                    Destroy(views[i].gameObject);
                    views[i] = null;
                }
            }
            // disable start game
            if(PhotonNetwork.CurrentRoom.PlayerCount == 1){
                startGame.interactable = false;
            }
        }
        #endregion
        /// <summary>
        /// Connection Process:
        /// - Check if already connected to photon's network.
        /// - If connected, attempt to join a random room
        /// - Otherwise, connect to the photon cloud network
        /// </summary>
        public void Connect()
        {
            waitingCanvas.SetActive(true);
            controlPanel.SetActive(false);
            loadingPanel.SetActive(true);
            //Check if connected to photon network
            if (PhotonNetwork.IsConnected)
            {
                Log("Already connected to photon network");
                Log("Trying to join a random room...");
                Debug.Log("Already connected to photon network");
                Debug.Log("Trying to join a random room...");
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                //Connect tot the Photon Online Server
                Log("Not yet connected to photon server.");
                Log("Connecting to photon network...");
                Debug.Log("Not yet connected to photon network.");
                Debug.Log("Trying to connect to photon network...");
                PhotonNetwork.ConnectUsingSettings();
                PhotonNetwork.GameVersion = gameVersion;
            }
        }
        public void StartGame(){
            PhotonNetwork.LoadLevel((int)Level.Gameplay);
        }
        
        void Log(string message) => UILog.Log(message);
    }
}
