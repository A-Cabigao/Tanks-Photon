using TMPro;
using UnityEngine;

namespace QLE
{
    public class ShopManager : MonoBehaviourSingleton<ShopManager>
    {
        [SerializeField] TextMeshProUGUI money;
        [SerializeField] ShopItemSO[] items;

        [SerializeField] GameObject shopItemPrefab;
        [SerializeField] RectTransform placeToCreateItems;
        void Awake() {
            for (int i = 0; i < items.Length; i++)
            {
                GameObject go = Instantiate(shopItemPrefab,placeToCreateItems);
                go.GetComponent<ShopItem>().Load(items[i]);
            }
        }
        void Start() => gameObject.SetActive(false);
        public ShopItemSO GetItem(int index)=> items[index % items.Length];
    }
}
