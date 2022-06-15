using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace QLE
{
    public class InGameUI : MonoBehaviour
    {
        [SerializeField] ButtonIconSwitcher music;
        [SerializeField] ButtonIconSwitcher sfx;

        void Start() {
            music.Toggle((float)PlayerPreferences.Get(PlayerPreferences.Keys.BACKGROUND_MUSIC_VOLUME,1)==1);
            sfx.Toggle((float)PlayerPreferences.Get(PlayerPreferences.Keys.SOUND_EFFECT_VOLUME,1)==1);
        }
        public void OpenQuitPanel(){
            Action quit = ()=>{
                if(PhotonNetwork.IsConnected){
                    StartCoroutine(Disconnect());
                }
            };
            PopupUI.Instance.ShowYesNo(null,"Do you want to quit?","Yes, I want to quit.",quit, "Nope",null); 
        }
        IEnumerator Disconnect(){
			// wait until reconnection if master just left
			while(!PhotonNetwork.IsConnected){
				yield return null;
			}
			// disconnect from room
			PhotonNetwork.LeaveRoom();

			// wait until fully disconnected from the room
			while(PhotonNetwork.InRoom){
				yield return null;
			}
			
			PhotonNetwork.Disconnect();

			// wait until fully disconnected from the room
			while(PhotonNetwork.IsConnected){
				yield return null;
			}
			
			// load login
			LevelManager.Instance.LoadMasterScene(Level.Lobby);
        }
    }
}
