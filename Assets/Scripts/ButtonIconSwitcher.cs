using UnityEngine;
using UnityEngine.UI;

public class ButtonIconSwitcher : MonoBehaviour
{
    [SerializeField] Sprite disabledSprite;
    [SerializeField] Sprite enabledSprite;
    Image icon;
    void Awake()
    {
        icon = GetComponent<Image>();
    }
    public void Toggle(){
        icon.sprite = icon.sprite == disabledSprite ? enabledSprite : disabledSprite;
    }
    public void Toggle(bool isEnabled){
        icon.sprite = isEnabled ? enabledSprite : disabledSprite;
    }
}
