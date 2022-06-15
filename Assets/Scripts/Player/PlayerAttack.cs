using Photon.Pun;
using UnityEngine;

namespace QLE
{
    public class PlayerAttack : MonoBehaviourPun
    {
        // TODO: have a script that stores information about prefab name of all bullets
        // TODO: store information of the current player's selected ammo type
        [SerializeField] string bulletID = "Bullet";
        Animator animator;

        [SerializeField] Transform projectileSpawnPoint;

        [Header("Guide")]
        [SerializeField] Transform arrow;
        [SerializeField] Transform circle;
        new Camera camera;

        void Awake()
        {
            animator = GetComponent<Animator>();
            enabled = false;
            camera = Camera.main;
        }
        void OnEnable() {
            photonView.RPC("AimReticleToggle",RpcTarget.AllBufferedViaServer,true);
        }
        void OnDisable() {
            photonView.RPC("AimReticleToggle",RpcTarget.AllBufferedViaServer,false);
        }
        [PunRPC]
        void AimReticleToggle(bool toEnable){
            circle.gameObject.SetActive(toEnable);
            arrow.gameObject.SetActive(toEnable);
        }
        void Update()
        {
            if(!photonView.IsMine) return;

            Aim();
            if(Input.GetMouseButtonDown(0)){
                photonView.RPC("Shoot",RpcTarget.AllBufferedViaServer);
            }
        }
        void Aim(){
            Vector2 destination = camera.ScreenToWorldPoint(Input.mousePosition);
            arrow.up = destination - (Vector2)transform.position;
        }
        [PunRPC]
        void Shoot()
        {
            GameObject projectileGO = ProjectileManager.Instance.Instantiate(bulletID,projectileSpawnPoint.position,transform.rotation);
            if(projectileGO != null)
            {
                projectileGO.SetActive(true);
                TurnManager.Instance.EndTurnBecausePlayerShot(projectileGO);
            }
        }
    }
}
