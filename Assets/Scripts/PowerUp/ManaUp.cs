using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QLE
{
    public class ManaUp : PowerUp
    {
        public override void ActivatePowerUp(Transform playerTransform)
        {
            var value = Mathf.FloorToInt(Random.Range(minRecover, maxRecover));
            playerTransform.GetComponentInChildren<Mana>().Add(value);
            Debug.Log("Recovered " + value + " mana.");
            base.ActivatePowerUp(playerTransform);
        }
    }
}
