using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;

using Photon.Pun.UtilityScripts;
using TMPro;

namespace QLE
{
    public class PlayerStatusUI : MonoBehaviour
    {
        [SerializeField] int playerIndex;

        public TextMeshProUGUI txt_name, txt_health;
        public SpriteRenderer avatar;

        public GameObject connected, waitingForConnection;

        void Start()
        {
            waitingForConnection.SetActive(true);
            connected.SetActive(false);
        }

        void Update() => HandleUI();

        void HandleUI()
        {
            //check if there is an available player with our index
            if (playerIndex <= PhotonNetwork.PlayerList.Length - 1)
            {
                Player player = PhotonNetwork.PlayerList[playerIndex];
                //make sure that the player number is assigned before updating the ui
                if (player.GetPlayerNumber() == -1)
                    return;
                waitingForConnection.SetActive(false);
                connected.SetActive(true);
                UpdateUI(player.NickName, NetworkManager.Instance.GetPlayerLook(playerIndex).worldLook);
            }
            else
            {
                waitingForConnection.SetActive(true);
                connected.SetActive(false);
            }
        }

        void UpdateUI(string name, Sprite icon)
        {
            txt_name.text = name;
            avatar.sprite = icon;
        }
    }
}
