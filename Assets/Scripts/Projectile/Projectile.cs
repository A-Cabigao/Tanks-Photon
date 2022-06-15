using Photon.Pun;
using UnityEngine;

namespace QLE
{
    [RequireComponent(typeof(PhotonView))]
    public class Projectile : MonoBehaviourPun
    {
        public ProjectileInfo info;

        [Tooltip("Layers that the projectile will hurt")]
        [SerializeField] LayerMask contact;

        public float damage => info.damage;
        Rigidbody2D rb;

        [SerializeField] Animator animator;
        bool hasImpacted;
        public bool IsAnimatorNotPlaying => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1;

        void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        void Update()
        {
            if(PhotonNetwork.IsMasterClient){
                CheckIfOutOfBounds();
                if(hasImpacted && IsAnimatorNotPlaying){
                    // ensure that this update call will not get triggered again
                    enabled = false;
                    // delete
                    PhotonNetwork.Destroy(gameObject);
                }
            }
        }

        void OnCollisionEnter2D(Collision2D collision) {
            if(info.doesSplashDamage && PhotonNetwork.IsMasterClient)
                photonView.RPC("DamageSurroundingArea",RpcTarget.AllBufferedViaServer);
        }
        [PunRPC]
        void DamageSurroundingArea(){
            //animator.SetTrigger("Impact");

            Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position,info.range,contact);
            for (int i = 0; i < cols.Length; i++)
                cols[i].GetComponent<Health>().Subtract(damage);

            // if(PhotonNetwork.IsMasterClient)
            //     PhotonNetwork.Destroy(gameObject);
            hasImpacted = true;
        }

        void CheckIfOutOfBounds()
        {// TODO: bullet will never be out of bounds if the camera is always following it. find a way to find the new limit and destroy this game object
            if(ScreenRestrictions.Instance.IsObjectOutOfBounds(transform))     
                this.gameObject.SetActive(false);           
        }

        public void SetSpawnInfo(float force, Vector3 direction)
        {
            // TODO set projectile info
            //rb.velocity = transform.up * (projectile.speed - projectile.weight) * force;
            transform.up = direction;
            rb.velocity = transform.up * force;
        }
    }
}
