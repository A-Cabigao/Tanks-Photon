using UnityEngine;

namespace QLE
{
    public enum TurnState{
        Started,            // player can move and shoot
        WaitForBulletImpact,// player cannot move and camera is focused at bullet
        Ended               // player turn ends and next player's turn will start
    }
    /// <summary>
    /// contains information about the current player's turn state
    /// </summary>
    public class PlayerTurn
    {
        public Transform activePlayer {get;}
        public TurnState state {get;}
        public PlayerTurn(TurnState state, Transform activePlayer){
            this.state = state;
            this.activePlayer = activePlayer;
        }
    }
}
