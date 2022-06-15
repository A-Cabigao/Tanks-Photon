using UnityEngine;

namespace QLE
{
    public class InventoryControls : MonoBehaviour
    {
        ProjectileInventory projectileInventory;

        public ProjectileInventoryInfo currentProjectile;

        // checks if game needs to check for inventory switching controls
        // public for debug
        public bool allowControl = false;

        private void Awake()
        {
            projectileInventory = GetComponent<ProjectileInventory>();
        }

        private void Update()
        {
            if(allowControl)
                ChangeCurrentProjectile();
        }

        public void SetEnabledControl(bool enabled) => allowControl = enabled;

        void ChangeCurrentProjectile()
        {
            if(Input.anyKeyDown)
            {
                // Get keyboard input to be used as index accessor for projectile invectory.
                int index = Controls();

                if (index >= projectileInventory.ProjectileInventoryCount() || index < 0)
                    return;
                else
                {
                    currentProjectile = projectileInventory.GetProjectileInventoryInfo(index);
                    Debug.Log(currentProjectile.projectile.name);
                }
            }
        }

        // Return an int from a keyboard input.
        // Returns -1 if input is not an Alpha key.
        int Controls()
        {
            for(int i = 1; i < 10; i++)
            {
                if (Input.GetKeyDown((KeyCode)System.Enum.Parse(typeof(KeyCode), "Alpha" + i)))
                {
                    return i - 1;
                }
            }
            return -1;
        }       
    }
}
