using System;
using System.Collections.Generic;
using Cinemachine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.Events;

namespace QLE
{
    /// <summary>
    /// manages who will play and who doesn't
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class TurnManager : SingletonPUN<TurnManager>
    {
        public static UnityAction<PlayerTurn> TurnEvent;
        
        [SerializeField] float totalPlayerTimeInTurn = 15f;
        public float TotalPlayerTimeInTurn => totalPlayerTimeInTurn;

        [Header("UI")]
        [Tooltip("Contains the time the player is alloted during his turn")]
        [SerializeField] TimerHand timerHand;

        List<PlayerInfo> players = new List<PlayerInfo>();
        public Transform ActivePlayer => players[activePlayerIndex].transform;
        int activePlayerIndex = 0;

        int initializingIndex = 0;

        /// <summary>
        /// main camera that follows the currently active player or the bullet that has been shot
        /// </summary>
        [SerializeField] CinemachineVirtualCamera cmcamera;
        [SerializeField] float moneyAfterEachRound = 100;

        void Start() {
            TurnEvent += PlayerTurn_Event;
            GameManager.GameEvent += Game_Event;
        }

        void Game_Event(GameEventArgs args)
        {
            if(args.HasGameStarted && PhotonNetwork.IsMasterClient){
                photonView.RPC("StartActivePlayerTurn",RpcTarget.AllBufferedViaServer);
            }
        }
        void OnDestroy() {
            TurnEvent -= PlayerTurn_Event;
            GameManager.GameEvent -= Game_Event;
        }
        [PunRPC]
        void StartActivePlayerTurn(){
            cmcamera.Follow = ActivePlayer;
            timerHand.StartTimer(players[activePlayerIndex].transform);
        }
        void PlayerTurn_Event(PlayerTurn turn){
            if(turn.state == TurnState.Ended){
                // might cause infinite loop
                while(activePlayerIndex + 1 <= players.Count - 1){
                    activePlayerIndex++;
                    if(players[activePlayerIndex].IsAlive) break;
                }

                // check if set complete
                if(activePlayerIndex == players.Count - 1){
                    for (int i = 0; i < players.Count; i++)
                    {
                        if(players[i].IsAlive){
                            players[i].Money.Add(moneyAfterEachRound);
                        }
                    }
                    activePlayerIndex = -1;
                }
                if(PhotonNetwork.IsMasterClient)
                    photonView.RPC("MoveToNextPlayer",RpcTarget.AllBufferedViaServer);
            }
        }
        [PunRPC]
        void MoveToNextPlayer(){
            // move to next player
            activePlayerIndex = (activePlayerIndex + 1) % players.Count;
            cmcamera.Follow = ActivePlayer;

            // reset info
            players[activePlayerIndex].Mana.Reset();

            // start
            timerHand.StartTimer(players[activePlayerIndex].transform);
        }
        public void EndTurnBecausePlayerShot(GameObject projectile){
            photonView.RPC("RPC_PlayerShot",RpcTarget.AllBufferedViaServer,new object[]{projectile.transform});
        }
        [PunRPC]
        void RPC_PlayerShot(object[] projectile){
            cmcamera.Follow = ((Transform)projectile[0]);
            timerHand.StopTimer(true);
        }

        public void EndTurnBecauseNoFuel() => photonView.RPC("RPC_NoFuel", RpcTarget.AllBufferedViaServer);
        [PunRPC]
        void RPC_NoFuel() => timerHand.StopTimer(false);

        /// <summary>
        /// This call is necessary to be called in all clients.
        /// In case the master client disconnects, the next master client will have a complete list of players
        /// </summary>
        public void AddPlayers(PlayerInfo[] info){
            if(GameManager.HasGameStarted){
                // TODO: join spectators
            }
            else{
                players = new List<PlayerInfo>(info);
            }
        }
        /// <summary>
        /// This call is necessary to be called in all clients.
        /// In case the master client disconnects, the next master client will have a complete list of players
        /// </summary>
        // public void AddPlayer(PlayerInfo info){
        //     if(GameManager.HasGameStarted){
        //         // TODO: join spectators
        //     }
        //     else{
        //         players.Add(info);
        //     }
        // }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            base.OnPlayerEnteredRoom(otherPlayer);
            PlayerInfo playerToRemove = null;
            int indexToRemove = 0;
            for (int i = 0; i < players.Count; i++)
            {
                if(players[i].Player == otherPlayer){
                    playerToRemove = players[i];
                    indexToRemove = i;
                    break;
                }
            }
            players.Remove(playerToRemove);

            if(PhotonNetwork.CurrentRoom.PlayerCount == 1){
            // TODO: check if player won because he's the only one left in the room

            }
            else{
            // TODO: otherwise, update the next player because its his turn
                if(activePlayerIndex == indexToRemove){
                    // end turn
                }
                // TODO: check for index out of bounds exception
            }
        }
    }
}
