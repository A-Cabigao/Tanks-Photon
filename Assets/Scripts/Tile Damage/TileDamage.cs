using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace QLE
{
    /// <summary>
    /// manages the changing of sprites when tile health is below a certain number
    /// </summary>
    public class TileDamage : MonoBehaviour
    {
        PhotonView view;
        Health health;
        new BoxCollider2D collider2D;
        SpriteRenderer tileSprite;
        SpriteRenderer damageSprite;
        AudioSource source;

        void Awake()
        {
            collider2D = GetComponent<BoxCollider2D>();
            health = GetComponent<Health>();
            view = GetComponent<PhotonView>();
            tileSprite = GetComponent<SpriteRenderer>();
            damageSprite = transform.GetChild(0).GetComponent<SpriteRenderer>();

            health.OnValueChange.AddListener(ChangeTile);
            health.OnDeplete.AddListener(DestroyTile);
        }

        void DestroyTile(BaseAmount args)
        {
            view.RPC("RPCDestroyTile",RpcTarget.AllBufferedViaServer);
        }

        [PunRPC]
        void RPCDestroyTile() => StartCoroutine(DestroyBlock());
        IEnumerator DestroyBlock(){
            // hide from view
            health.OnValueChange.RemoveAllListeners();
            health.OnDeplete.RemoveAllListeners();
            tileSprite.enabled = damageSprite.enabled = collider2D.enabled = false;
            source.Play();
            
            // wait
            yield return new WaitUntil(()=> !source.isPlaying);
            PhotonNetwork.Destroy(gameObject);
        }

        void ChangeTile(BaseAmount args)
        {
            view.RPC("RPCChangeTile",RpcTarget.AllBufferedViaServer);
        }

        void RPCChangeTile()
        {
            damageSprite.sprite = TileDamageManager.Instance.GetSprite(health.Value);
        }
    }
}
