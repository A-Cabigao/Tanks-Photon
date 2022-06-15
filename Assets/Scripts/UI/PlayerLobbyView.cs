using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QLE
{
    /// <summary>
    /// Handles UI of a player in the lobby
    /// </summary>
    [RequireComponent(typeof(PhotonView))]
    public class PlayerLobbyView : MonoBehaviour
    {
        [SerializeField] Image character;
        [SerializeField] TextMeshProUGUI playerName;
        [SerializeField] PhotonView photonView;
        public PhotonView PhotonView => photonView;
        
        Photon.Realtime.Player player;
        public Photon.Realtime.Player Player{
            get=>player;
            set{
                player = value;
                playerName.text = player.NickName;
            }
        }
        public Sprite Character
        {
            get => character.sprite;
            set { character.sprite = value; }
        }
        
        public string PlayerName 
        {
            get => playerName.text;
            set { playerName.text = value; }
        }
    }
}
