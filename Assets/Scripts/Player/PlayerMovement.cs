using System;
using System.Collections;
using UnityEngine;

namespace QLE
{
    [RequireComponent(typeof(Mana))]
    public class PlayerMovement : MovementBase2D
    {
        public Action OnJump;

        Vector2 inputVelo;
        bool isGrounded = true;
        [SerializeField] LayerMask ground;
        [SerializeField] float maxSpeed = 2;
        [SerializeField] float jumpForce = 5;

        [SerializeField] ParticleSystem dustParticle;

        bool goingPositiveX;

        [Header("Sounds")]
        [SerializeField] SoundMaker soundMaker;
        
        [Tooltip("Grounded and Jump")]
        [SerializeField] SoundMakerInfo[] noise;
        [SerializeField] bool hasSound;
        bool canPlayJumpSound = true;

        [Header("Mana cost")]
        [SerializeField] float manaCostToMove = 2;
        public Mana Mana {get; private set;}

        protected override void Awake() {
            base.Awake();
            Mana = GetComponent<Mana>();
            Mana.OnDeplete.AddListener(DisableMovement);
            enabled = false;
        }

        void DisableMovement(BaseAmount args) {
            EnableMovement(false);
            TurnManager.Instance.EndTurnBecauseNoFuel();
        }

        protected override void Move()
        {
            if(isGrounded) CheckIfMovingInOtherDirection();

            inputVelo = movementInput * currentMoveSpeed * Time.fixedDeltaTime;
            float newX = MyRigidbody2D.velocity.x + inputVelo.x;
            newX = Mathf.Clamp(newX, -maxSpeed,maxSpeed);

            Jump();

            MyRigidbody2D.velocity = new Vector2(newX,MyRigidbody2D.velocity.y);

            // subtract mana because player moved
            float manaTickCost = manaCostToMove * Time.deltaTime * newX;
            if(manaTickCost != 0)
                Mana.Subtract(manaTickCost);
        }

        /// <summary>
        /// gives the player a smoke effect when he is thrown upwards / falling to the ground / moving over the battlefield
        /// </summary>
        void Jump()
        {
            bool previousIsGrounded = isGrounded;
            isGrounded = Physics2D.Raycast(transform.position,Vector2.down,.52f,ground);
            if(!previousIsGrounded && isGrounded) {
                CreateDust();
                if(hasSound) soundMaker.Play(noise[0]);
                OnJump?.Invoke();
            }

            if(inputVelo.y > 0 && isGrounded){
                MyRigidbody2D.velocity += new Vector2(0,jumpForce);
                if(hasSound){
                    soundMaker.Play(noise[1]);
                //     // if(canPlayJumpSound){
                //     //     soundMaker.Play(Sound.Jump);
                //     //     canPlayJumpSound = false;
                //     // }
                //     // else{
                //     //     canPlayJumpSound = true;
                //     // }
                } 
                CreateDust();
            }
        }

        void CheckIfMovingInOtherDirection(){
            bool isSwitchingSides = (goingPositiveX && movementInput.x < 0) || (!goingPositiveX && movementInput.x > 0);
            bool wasMovingPreviously = Mathf.Abs(MyRigidbody2D.velocity.x) != 0; 
            if(isSwitchingSides && wasMovingPreviously){
                goingPositiveX = !goingPositiveX;
                CreateDust();
            }
        }

        protected override void Movement_GameEvent(GameEventArgs args)
        {
            if(args.HasGameStarted){
                EnableMovement(true);
            }
        }

        protected override void Movement_PausedEvent(PauseEventArgs args) => EnableMovement(!args.isPaused);
        void CreateDust() => dustParticle.Play();
    }
}