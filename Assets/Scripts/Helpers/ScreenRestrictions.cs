using UnityEngine;

namespace QLE
{
    public class ScreenRestrictions : MonoBehaviourSingleton<ScreenRestrictions>
    {
        public Camera mainCam { get; private set; }

        Vector2 upperCorner, lowerCorner;

        private void Awake()
        {
            mainCam = Camera.main;
            CalculateScreenRestrictions();
        }
        
        void CalculateScreenRestrictions()
        {
            Vector3 upperScreen = mainCam.ViewportToWorldPoint(new Vector3(1, 1, 0));
            Vector3 lowerScreen = mainCam.ViewportToWorldPoint(Vector3.zero);
            upperCorner = new Vector2(upperScreen.x, upperScreen.y);
            lowerCorner = new Vector2(lowerScreen.x, lowerScreen.y);
        }

        public bool IsObjectOutOfBounds(Transform objectTransform)
        {
            return ((objectTransform.position.x > upperCorner.x || objectTransform.position.x < lowerCorner.x) ||
                (objectTransform.position.y > upperCorner.y || objectTransform.position.y < lowerCorner.y));
        }
    }
}
