using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace QLE
{
    public class ShootProjectile : MonoBehaviour
    {
        [SerializeField] InventoryControls inventoryControls;
        [SerializeField] float maxForce = 10f;

        // Checks if player is allowed to use controls for shooting.
        // public for debug
        public bool isAllowedToShoot = false;

        private void Update()
        {
            if (!isAllowedToShoot)
                return;
            else
            // Check if there is a bullet equipped.
                if(inventoryControls.currentProjectile != null)
                    if(Input.GetMouseButtonDown(0))
                    // Check if equipped bullet has ammo.
                        if(inventoryControls.currentProjectile.ammo > 0)
                            StartCoroutine(ShotPreparation());

            // Stop shot preparation on right mouse button.
            if (Input.GetMouseButtonDown(1))
                StopCoroutine(ShotPreparation());
        }

        IEnumerator ShotPreparation()
        {
            var force = 0f;
            bool hasFired = false;
            bool isIncreasing = true;
            var modifierSpeed = Time.deltaTime * (maxForce / 2f);

            while(!hasFired)
            {
                if (isIncreasing)
                {
                    force += modifierSpeed;
                    if (force >= maxForce)
                        isIncreasing = false;
                }
                else
                {
                    force -= modifierSpeed;
                    if (force <= maxForce)
                        isIncreasing = true;
                }

                Debug.Log(force);

                if (Input.GetMouseButtonUp(0))
                {
                    Shoot(force);
                    hasFired = true;
                }
                yield return null;
            }
        }

        // TODO: Turn to PUN
        void Shoot(float force)
        {
            GameObject obj = Instantiate(inventoryControls.currentProjectile.projectilePrefab, 
                transform.position, Quaternion.identity);
            obj.GetComponent<Projectile>().SetSpawnInfo(force, transform.right);
        }
    }
}
