using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QLE
{
    public class DamageUp : PowerUp
    {
        public override void ActivatePowerUp(Transform playerTransform)
        {
            Debug.Log("Acquired double damage");
            base.ActivatePowerUp(playerTransform);
        }

    }
}
