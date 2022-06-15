using Photon.Pun;
using UnityEngine;

namespace QLE
{
    public class GameManager : GameManager<Level> {
        [SerializeField] GameObject playerPrefab;
        
        protected override void Awake() {
            base.Awake();
            GameEvent += Game_Event;
        }
        protected override void OnDestroy() {
            base.OnDestroy();
            GameEvent -= Game_Event;
        }
        void Game_Event(GameEventArgs args){
            if(args.State == GameStates.Ended && args.HasWon){
                Debug.Log("Transfer");
                GoToNextScene(true);
            }
        }
    }
}
