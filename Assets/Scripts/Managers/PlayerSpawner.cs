using UnityEngine;

using Photon.Pun;
using Photon.Realtime;

namespace QLE{
    // TODO: spawn players in master client
    public class PlayerSpawner : SingletonPUN<PlayerSpawner>
    {
        [SerializeField] Transform[] spawnPositions;
        bool[] spawnedAlready;

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            NetworkManager.Instance.Log("The new master client is: " + newMasterClient.NickName);
            if(PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
            }
        }


        void Start()
        {
            spawnedAlready = new bool[spawnPositions.Length];
            //Only handle the spawning if you are the master client
            if (!PhotonNetwork.IsMasterClient)
                return;

        }
        void SpawnPlayersInNetwork()
        {
            //Instantiate the Enemy over the network
            PhotonNetwork.InstantiateRoomObject("Player", GetRandomPosition(), Quaternion.identity);
        }

        Vector2 GetRandomPosition()
        {
            //spawn randomly on the four corners of the screen
            for (int i = 0; i < spawnPositions.Length; i++)
            {
                // TODO: keep finding an available spawn position if spawnedAlready is true
                //spawnedAlready[i] = true;
            }
            return Vector2.zero;
        }
    }
}