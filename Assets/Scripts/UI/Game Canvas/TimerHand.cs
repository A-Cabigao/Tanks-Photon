using System.Collections;
using UnityEngine;
using UnityEngine.Events;

namespace QLE
{
    public class TimerHand : MonoBehaviour
    {
        Coroutine rotateRoutine;
        Transform activePlayer;

        public void StartTimer(Transform player){
            activePlayer = player;
            rotateRoutine = StartCoroutine(Rotate());
        }

        /// <summary>
        /// stops and raises the event that updates the player turn
        /// </summary>
        /// <param name="causedByFiringBullet">if true, we will wait for the bullet to arrive before moving to the next player.
        /// if false, we will end the turn and move to the next player</param>
        public void StopTimer(bool causedByFiringBullet){
            if(rotateRoutine != null){
                StopCoroutine(rotateRoutine);
                rotateRoutine = null;

                // raise done event
                PlayerTurn turn = new PlayerTurn(causedByFiringBullet ? TurnState.WaitForBulletImpact:TurnState.Ended,activePlayer);
                TurnManager.TurnEvent?.Invoke(turn);
            }
        }
        IEnumerator Rotate(){
            // raise start event
            PlayerTurn turn = new PlayerTurn(TurnState.Started,activePlayer);
            TurnManager.TurnEvent?.Invoke(turn);

            // initialize
            float playerTimeInTurn = TurnManager.Instance.TotalPlayerTimeInTurn;
            float degrees = 360/playerTimeInTurn;

            // main loop
            for (int i = 0; i < playerTimeInTurn; i++)
            {
                transform.Rotate(new Vector3(0,0,degrees));
                yield return new WaitForSeconds(1);
            }

            // raise done event
            turn = new PlayerTurn(TurnState.Ended,activePlayer);
            TurnManager.TurnEvent?.Invoke(turn);
            rotateRoutine = null;
        }
    }
}