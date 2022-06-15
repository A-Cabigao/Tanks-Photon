using UnityEngine;
using UnityEngine.Events;

namespace QLE
{
    public class PlayerCursor : PlayerCursor2D
    {
        [SerializeField] SpriteOutline[] outlines;
        bool notSet;

        [SerializeField] UnityEvent winEvent;
        bool hasNotWon;
        void LateUpdate() {
            if(isMouseInside && !notSet){
                ShowOutline(true);
            }
            else if(!isMouseInside && notSet){
                ShowOutline(false);
            }

            if(IsMouseInside && Input.GetMouseButtonDown(0) && !hasNotWon){
                hasNotWon = true;
                winEvent?.Invoke();
            }
        }
        void ShowOutline(bool willShow){
            for (int i = 0; i < outlines.Length; i++)
            {
                outlines[i].IsOutlineShowing = willShow;
            }
            notSet = willShow;
        }
    }
}
