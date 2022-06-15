using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QLE
{
    /// <summary>
    /// data container for the shop item
    /// </summary>
    public class ShopItem : MonoBehaviourPun
    {
        [SerializeField] Image bulletIcon;
        [SerializeField] TextMeshProUGUI title;
        [SerializeField] TextMeshProUGUI description;
        /// <summary>
        /// refers to how much of the item does the local player have
        /// </summary>
        [SerializeField] TextMeshProUGUI amount;
        [SerializeField] Button[] buyButtons;

        public ShopItemSO item;

        void Awake() {
            buyButtons = transform.Find("Buy Container").GetComponentsInChildren<Button>();
            if(item) Load(item);
            GameManager.GameEvent += Game_Event;
        }
        void OnDestroy() {
            GameManager.GameEvent -= Game_Event;
        }

        void Game_Event(GameEventArgs args)
        {
            if(args.HasGameStarted){
                PlayerInfo.local.Money.OnValueChange.AddListener(CheckIfCanBuy);
            }
        }
        public void Load(ShopItemSO so){
            item = so;

            title.text = so.name;
            description.text = so.description;
            bulletIcon.sprite = so.sprite;
            amount.text = so.Amount.ToString();
            
            SetButtonPrices();
        }
        void SetButtonPrices(){
            int[] multiplier = {1,3,5};
            for (int i = 0; i < buyButtons.Length; i++)
            {
                TextMeshProUGUI text = buyButtons[i].GetComponentInChildren<TextMeshProUGUI>();
                text.text += item.CostFor(multiplier[i]);
            }
        }
        /// <summary>
        /// enables/disables the buttons if money isn't enough
        /// </summary>
        void CheckIfCanBuy(BaseAmount amount){
            int[] multiplier = {1,3,5};
            for (int i = 0; i < multiplier.Length; i++)
                buyButtons[i].interactable = multiplier[i] * item.cost > amount.Value;
        }
        public void Buy(int quantity){
            int totalCost = quantity * item.cost;
            if(PlayerInfo.local.Money.Value >= totalCost){
                PlayerInfo.local.Money.Subtract(totalCost);
            }
        }
        public void Buy1() => Buy(1);
        public void Buy3() => Buy(3);
        public void Buy5() => Buy(5);
    }
}
