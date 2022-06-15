using UnityEngine;
using UnityEngine.UI;

namespace QLE
{
    /// <summary>
    /// updates local player's mana into the UI
    /// </summary>
    public class ManaUpdater : MonoBehaviour
    {
        [SerializeField] Image manaFill;
        Mana mana;
        void Start()
        {
            GameManager.GameEvent += Game_Event;
        }
        void OnDestroy() {
            GameManager.GameEvent -= Game_Event;
        }

        void Game_Event(GameEventArgs args)
        {
            if(args.HasGameStarted){
                // set local player's mana
            mana = PlayerInfo.local.Mana;
            manaFill.fillAmount = mana.Value;
            mana.OnValueChange.AddListener(UpdateFill);
            }
        }
        void UpdateFill(BaseAmount amount) => manaFill.fillAmount = amount.Value;
    }
}
