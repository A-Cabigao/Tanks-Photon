using System.Collections.Generic;
using UnityEngine;

namespace QLE
{
    public class ProjectileInventoryInfo
    {
        public GameObject projectilePrefab { get; private set; }
        public Projectile projectile { get; private set; }
        public int ammo { get; private set; }

        public ProjectileInventoryInfo(Projectile projectile, int ammoCount, GameObject prefab)
        {
            this.projectile = projectile;
            ammo = ammoCount;
            projectilePrefab = prefab;
        }
        public void IncreaseAmmoCount(int value) => ammo += value;
        public void SetAmmoCount(int value) => ammo = value;
    }


    public class ProjectileInventory : MonoBehaviour
    {
        public ProjectileInventoryInfo[] inventory = new ProjectileInventoryInfo[8];

        public List<GameObject> DEBUG_projectiles = new List<GameObject>();

        int currentIndex = 0;

        private void Awake()
        {
            CreateDebugList();
        }

        void CreateDebugList()
        {
            for (int i = 0; i < DEBUG_projectiles.Count; i++)
            {
                inventory[i] = new ProjectileInventoryInfo(DEBUG_projectiles[i].GetComponent<Projectile>(), 0, DEBUG_projectiles[i]);
            }
        }

        #region ProjectileGetters

        public ProjectileInventoryInfo GetProjectileInventoryInfo(string projectileName)
        {
            // No projectiles in inventory.
            if (inventory.Length == 0)           
                return default;
            
            foreach(ProjectileInventoryInfo p in inventory)
            {
                if (p.projectile.info.name == projectileName)
                    return p;
            }
            // No projectile of name projectileName was found.
            return default;
        }

        public ProjectileInventoryInfo GetProjectileInventoryInfo(int index)
        {
            if (index >= inventory.Length || index < 0)
                return default;
            else
                return inventory[index];
        }

        public int ProjectileInventoryCount() => inventory.Length;
        #endregion

        #region ProjectileInfoSetters

        // Creates a new ProjectileInventoryInfo and add to the ProjectileInventoryInfo list.
        public void AddProjectileToInventory(Projectile projectile, GameObject prefab)
        {
            if (currentIndex >= inventory.Length)
                return;
            else
            {
                inventory[currentIndex] = new ProjectileInventoryInfo(projectile, 0, prefab);
                currentIndex++;
            }
        }

        // Increases the ammo count by random amount of a random projectile in projectileList.
        // min = minimum amount to add, max = maximum amount to add.
        public void IncreaseProjectileBulletCount(int min, int max)
        {
            if (inventory.Length == 0)
                return;
            else
                inventory[Random.Range(0, inventory.Length - 1)].
                    IncreaseAmmoCount(Random.Range(min, max + 1));          
        }

        // Decrease the ammo count of a projectile in projectileInventory by 1.
        public void DecreaseProjectileAmmoCount(Projectile projectile)
        {
            if (inventory.Length == 0)
                return;
            if (!IsProjectileInInventory(projectile))
                return;
            else
            {
                ProjectileInventoryInfo info = GetProjectileInventoryInfo(projectile.name);
                if (info.ammo <= 0)
                    return;
                else
                    info.IncreaseAmmoCount(-1);
            }
        }
        #endregion

        #region Booleans
        // Checks if projectile in parameter exists in projectileInventory.
        public bool IsProjectileInInventory(Projectile projectile)
        {
            //return GetProjectileInventoryInfo(projectile.name) != null;
            return ReferenceEquals(GetProjectileInventoryInfo(projectile.name), default);
        }

        public bool IsProjectileInInventory(string projectileName)
        {
            return ReferenceEquals(GetProjectileInventoryInfo(projectileName), default);
        }
        #endregion
    }
}
