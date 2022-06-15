using Photon.Pun;
using UnityEngine;

namespace QLE
{
    public class PlayerWorldUIHandler : MonoBehaviourPun
    {
        [SerializeField] GameObject playerWorldUI;
        PlayerWorldUI[] uis;
        void Awake() {
            GameManager.GameEvent += Game_Event;
        }
        void OnDestroy() {
            GameManager.GameEvent -= Game_Event;
        }

        void Game_Event(GameEventArgs args)
        {
            if(args.HasGameStarted){
                PlayerInfo[] infos = SpawnManager.Instance.infos;
                uis = new PlayerWorldUI[infos.Length];
                Debug.Log("Player length: " + infos.Length);
                for (int i = 0; i < infos.Length; i++)
                {
                    //GameObject go = PhotonNetwork.InstantiateRoomObject("UI/"+playerWorldUI.name,infos[i].transform.position,Quaternion.identity);
                    GameObject go = Instantiate(playerWorldUI,infos[i].transform.position,Quaternion.identity);
                    uis[i] = go.GetComponent<PlayerWorldUI>();
                    uis[i].Set(infos[i]);
                    go.transform.SetParent(transform);
                }
            }
        }
    }
}
