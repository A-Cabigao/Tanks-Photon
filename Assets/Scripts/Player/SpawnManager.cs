using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using UnityEngine;
using UnityEngine.Events;

namespace QLE
{
    /// <summary>
    /// spawns players at selected locations
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class SpawnManager : SingletonPUN<SpawnManager>
    {
        public static UnityAction<PlayerInfo[]> PlayerListUpdate;

        [SerializeField] GameObject playerPrefab;
        [SerializeField] Transform playerContainer;
        [SerializeField] Transform[] spawnPoints;
        public PlayerInfo[] infos;

        static int playerObjectReady = 0;
        static int playersReady = 0;

        void Start() {
            //NetworkManager.OnPlayerNumberingUpdated += PlayerNumberingUpdate;
            if(PhotonNetwork.IsMasterClient){
                infos = new PlayerInfo[PhotonNetwork.CurrentRoom.PlayerCount];
                for (int i = 0; i < infos.Length; i++)
                    infos[i] = PhotonNetwork.Instantiate(playerPrefab.name,spawnPoints[i].position,Quaternion.identity).GetComponent<PlayerInfo>();
                
            }
        }

        void PlayerNumberingUpdate()
        {
            if(infos != null){
                int playerNumber = PhotonNetwork.LocalPlayer.GetPlayerNumber();
                infos[playerNumber].photonView.TransferOwnership(playerNumber);
            }
            // TODO: figure out how to update the list when a player leaves
            List<PlayerInfo> newList = new List<PlayerInfo>();
            for (int i = 0; i < infos.Length; i++)
            {
                for (int j = 0; j < PhotonNetwork.PlayerList.Length; j++)
                {
                    if(infos[i].Player == PhotonNetwork.PlayerList[j]){
                        newList.Add(infos[i]);
                        break;
                    }
                }
            }
            PlayerListUpdate?.Invoke(newList.ToArray());
        }
        public void PlayerObjectReady(){
            playerObjectReady++;

            int totalPlayerCount = PhotonNetwork.PlayerList.Length;
            totalPlayerCount*=totalPlayerCount;
            // check if we've readied all player objects in all clients
            if(playerObjectReady == totalPlayerCount){
                photonView.RPC("SpawnPlayer",RpcTarget.AllBufferedViaServer);
            }
        }
        [PunRPC]
        void SpawnPlayer(){
            // update local list for non-master clients
            if(infos == null || infos.Length == 0)
                infos = FindObjectsOfType<PlayerInfo>();

            // update player references so that each character is owned by their player
            for (int i = 0; i < infos.Length; i++)
                infos[i].Player = PhotonNetwork.PlayerList[i];
Debug.Log("player list: " + infos.Length);
            // sort local players
            for (int i = 0; i < infos.Length; i++)
            {
                for (int j = 0; j < infos.Length; j++)
                {
                    if(infos[i].Player != PhotonNetwork.PlayerList[i]){
                        Swap(ref infos,i,j);
                    }
                }
            }
            //Array.Sort(infos, (PlayerInfo a,PlayerInfo b)=>a.Player.ActorNumber.CompareTo(b.Player.ActorNumber));
            TurnManager.Instance.AddPlayers(infos);
            
            // tell everyone i'm ready
            photonView.RPC("ImReady",RpcTarget.AllBufferedViaServer,PhotonNetwork.NickName);
        }
        void Swap(ref PlayerInfo[] objectArray, int x, int y){
            // swap index x and y
            PlayerInfo buffer = objectArray[x];
            objectArray[x] = objectArray[y];
            objectArray[y] = buffer;
        }

        /// <summary>
        /// tells the all client how many clients are ready
        /// </summary>
        [PunRPC]
        void ImReady(string name){
            playersReady++;
            Debug.Log(name + " is ready");

            if(playersReady == infos.Length){
                GameManager.Instance.StartGame();
            }
        }
    }
}
