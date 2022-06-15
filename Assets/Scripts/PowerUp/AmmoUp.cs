using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QLE
{
    public class AmmoUp : PowerUp
    {
        float value;
        public override void Awake()
        {
            base.Awake();
            PowerUpManager.OnAmmoPickUp += PickUpAmmo;
        }

        private void OnDestroy()
        {
            PowerUpManager.OnAmmoPickUp -= PickUpAmmo;
        }

        void PickUpAmmo(ProjectileInventoryInfo info)
        {
            Debug.Log("Got " + value + " of " + info.projectile.name + " ammo.");
        }


        public override void ActivatePowerUp(Transform playerTransform)
        {
            value = Mathf.FloorToInt(Random.Range(minRecover, maxRecover));
            playerTransform.GetComponentInChildren<ProjectileInventory>().
                IncreaseProjectileBulletCount((int)minRecover, (int)maxRecover);      
            base.ActivatePowerUp(playerTransform);
        }
    }
}
