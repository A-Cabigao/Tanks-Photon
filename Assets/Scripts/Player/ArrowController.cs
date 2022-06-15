using UnityEngine;
using Photon.Pun;

namespace QLE
{
    public class ArrowController : MonoBehaviour
    {
        // How far the pointer is to the player in radius units.
        [SerializeField] float radiusToPlayer;
        // NOTE: note sure pa how to utilize this
        PlayerTurn playerTurn;
        Camera mainCam;

        [SerializeField] Transform playerTransform;

        private void Awake()
        {
            // DEBUG ONLY
            if (playerTransform == null)
                playerTransform = transform.root;
            mainCam = Camera.main;
        }
        void FixedUpdate()
        {
            MoveArrowController();
        }

        void MoveArrowController()
        {
            var mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
            var direction = new Vector2(mousePosition.x - playerTransform.position.x, 
                mousePosition.y - playerTransform.position.y);
            direction.Normalize();
            transform.up = direction;
            transform.position = playerTransform.position + new Vector3(direction.x, direction.y) * radiusToPlayer;
        }
    }
}
