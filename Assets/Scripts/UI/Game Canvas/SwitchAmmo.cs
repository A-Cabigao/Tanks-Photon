using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace QLE
{
    /// <summary>
    /// for the local player, this handles the updating of ammo amount and sprite in the game canvas
    /// </summary>
    public class SwitchAmmo : MonoBehaviour
    {
        [SerializeField] Image bulletIcon;
        [SerializeField] TextMeshProUGUI amount;
        ShopItemSO current;

        public void ChangeAmmo(int index) {
            if(current != null){
                current.amountUpdate-=UpdateAmount;
            }
            ShopItemSO so = ShopManager.Instance.GetItem(index);
            bulletIcon.sprite = so.sprite;
            amount.text = so.Amount.ToString();
            so.amountUpdate+=UpdateAmount;

            current = so;
        }
        void OnDestroy() {
            if(current != null){
                current.amountUpdate-=UpdateAmount;
            }
        }
        void UpdateAmount(int amount) => this.amount.text = amount.ToString();
    }
}
