using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine;

namespace QLE
{
    /// <summary>
    /// contains info about the player's client network info, health, if he's alive, and animator properties
    /// </summary>
    [RequireComponent(typeof(Health),typeof(PhotonView),typeof(PhotonTransformView))]
    [RequireComponent(typeof(Mana),typeof(PlayerMovement))]
    public class PlayerInfo : MonoBehaviourPun
    {
        public static PlayerInfo local;
        Player player;
        public Player Player {
            get=>player;
            set{
                player = value;
                photonView.TransferOwnership(player);
                if(player == PhotonNetwork.LocalPlayer){
                    local = this;Debug.Log("local player set");
                }
            }
        }

        public Health Health {get; private set;}
        // inventory;
        public bool IsAlive {get; protected set;}

        public Mana Mana {get; private set;}
        public Money Money {get; private set;}
        public PlayerAttack Attack {get; private set;}
        public PlayerMovement Movement {get; private set;}
        public PlayerLook Look {get; private set;}

        // animation related
        public SpriteRenderer spriteRenderer { get; private set; }
        public Animator animator { get; private set; }
        public Color Color { get=> spriteRenderer.color; set=>spriteRenderer.color = value; }

        //[SerializeField] GameObject deathPrefab;
        
        void Awake() {
            Health = GetComponent<Health>();
            Mana = GetComponent<Mana>();
            Money = GetComponent<Money>();
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            Movement = GetComponent<PlayerMovement>();
            Attack = GetComponent<PlayerAttack>();
            Look = GetComponentInChildren<PlayerLook>();
            
            Health.OnDeplete.AddListener(RaisePlayerDeath);
            TurnManager.TurnEvent += Player_Turn;
            photonView.RPC("RaisePlayerObjectReady",RpcTarget.AllBufferedViaServer);
        }
        [PunRPC]
        public void RaisePlayerObjectReady(){
            SpawnManager.Instance.PlayerObjectReady();
        }
        void Player_Turn(PlayerTurn args)
        {
            //if(!photonView) return;
            
            if(args.activePlayer == transform && photonView.IsMine){
                if(args.state == TurnState.Started){
                    TogglePlayerControls(true);
                }
                else if(args.state == TurnState.WaitForBulletImpact){
                    TogglePlayerControls(false);
                }
            }
        }
        void TogglePlayerControls(bool toEnable){
            Movement.EnableMovement(toEnable);
            Attack.enabled = toEnable;
            Look.enabled = toEnable;
        }

        void RaisePlayerDeath(BaseAmount args)
        {
            if(PhotonNetwork.IsMasterClient){
                photonView.RPC("RPC_RaisePlayerDeath",RpcTarget.AllBufferedViaServer);
            }
        }
        [PunRPC]
        void RPC_RaisePlayerDeath(){
            // TODO: spawn particle explosion
            // TODO: disable collision
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            IsAlive = false;
            //animator.SetTrigger("Died");
        }
    }
}
