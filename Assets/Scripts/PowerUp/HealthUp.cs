using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QLE
{
    public class HealthUp : PowerUp
    {
        public override void ActivatePowerUp(Transform playerTransform)
        {
            var value = (Mathf.FloorToInt(Random.Range(minRecover, maxRecover)));
            playerTransform.GetComponentInChildren<Health>().Add(value);
            Debug.Log("Recovered " + value + " health.");
            base.ActivatePowerUp(playerTransform);
        }
    }
}
