using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QLE
{
    [RequireComponent(typeof(PhotonView))]
    public class PlayerWorldUI : MonoBehaviourPunCallbacks
    {
        [SerializeField] TextMeshProUGUI playerName;
        [SerializeField] Image health;
        PlayerInfo info;
        Transform player;

        public void Set(PlayerInfo info) {
            this.info = info;
            playerName.text = info.Player.NickName;
            player = info.transform;
            info.Health.OnValueChange.AddListener(Health_Update);
        }
        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            if(otherPlayer == info.Player && PhotonNetwork.IsMasterClient){
                PhotonNetwork.Destroy(gameObject);
            }
        }

        void LateUpdate() {
            // prevents null when player disconnects
            if(player)
                transform.position = player.position;
        }

        [PunRPC]
        void UpdateHealthToAll(float healthValue){
            health.fillAmount = healthValue;
            if(healthValue == 0)
                gameObject.SetActive(false);
        }

        void Health_Update(BaseAmount args)
        {
            photonView.RPC("UpdateHealthToAll",RpcTarget.AllBufferedViaServer,args.PercentageToMax);
        }
    }
}
