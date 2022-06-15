using UnityEngine;
using UnityEngine.Events;

namespace QLE
{
    [CreateAssetMenu(fileName = "Item name", menuName = "ScriptableObjects/Shop item")]
    public class ShopItemSO : ScriptableObject
    {
        /// <summary>
        /// Raised when amount is updated
        /// </summary>
        public UnityAction<int> amountUpdate;

        [Tooltip("How the item looks")]
        public Sprite sprite;

        [Tooltip("Refers to how many bullet the player has when starting the game")]
        [SerializeField] int amountInitial;
        
        /// <summary>
        /// Refers to how many bullet the player has in the game
        /// </summary>
        int amount = 0;
        public int Amount
        {
            get => amount;
            set {
                amount = value;
                amountUpdate?.Invoke(value);
            }
        }

        [TextArea] public string description;

        // cost per ammo
        public int cost = 1;

        void Awake() => Reset();
        public void Reset() => Amount = amountInitial;
        public int CostFor(int quantity) => cost * quantity;
    }
}
