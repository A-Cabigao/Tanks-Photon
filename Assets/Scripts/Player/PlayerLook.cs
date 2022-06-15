using UnityEngine;

using Photon.Pun;
using Photon.Pun.UtilityScripts;
using System;

namespace QLE
{
    /// <summary>
    /// Changes the look of the player according to player number and where he's looking at
    /// </summary>
    public class PlayerLook : MonoBehaviourPun
    {
        SpriteRenderer spriteRenderer;

        void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            NetworkManager.OnPlayerNumberingUpdated += AssignPlayerSprite;
            enabled = false;
        }
        void Update() {
            LookAtWhereTheMouseIs();
        }

        void LookAtWhereTheMouseIs()
        {
            Vector2 mousePos = Input.mousePosition;
            bool isLookingRight = transform.position.x < mousePos.x;
            if((spriteRenderer.flipX && isLookingRight) || (!spriteRenderer.flipX && !isLookingRight)){
                photonView.RPC("FlipSprite",RpcTarget.AllBufferedViaServer,isLookingRight);
            }
        }
        [PunRPC]
        void FlipSprite(bool flipX) => spriteRenderer.flipX = flipX;

        void OnDestroy()
        {
            NetworkManager.OnPlayerNumberingUpdated -= AssignPlayerSprite;
        }
        void AssignPlayerSprite()
        {
            if (photonView.Owner.GetPlayerNumber() == -1)
                return;

            //NetworkManager.Instance.Log(string.Format("Assigning a sprite for <color=Green>{0}</color> with Player Number of <color=Green>{1}</color>",
            //    photonView.Owner.NickName, photonView.Owner.GetPlayerNumber()));

            photonView.RPC("RPCAssignPlayerSprite", RpcTarget.AllBufferedViaServer);
        }

        [PunRPC]
        void RPCAssignPlayerSprite()
        {
            spriteRenderer.sprite = NetworkManager.Instance.GetPlayerLook(photonView.Owner.GetPlayerNumber()).worldLook;
        }
    }
}
