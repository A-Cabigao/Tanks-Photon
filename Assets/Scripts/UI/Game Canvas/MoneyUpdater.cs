using System;
using TMPro;
using UnityEngine;

namespace QLE
{
    public class MoneyUpdater : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI moneyText;
        Money money;
        void Start()
        {
            if(!moneyText) moneyText = GetComponent<TextMeshProUGUI>();
            GameManager.GameEvent += Game_Event;
        }
        void OnDestroy() {
            GameManager.GameEvent -= Game_Event;
        }

        void Game_Event(GameEventArgs args)
        {
            if(args.HasGameStarted){
                // set local player's money
                money = PlayerInfo.local.Money;
                moneyText.text = money.Value.ToString("000000");
                money.OnValueChange.AddListener(UpdateText);
            }
        }

        void UpdateText(BaseAmount amount) => moneyText.text = amount.Value.ToString("000000");
    }
}
