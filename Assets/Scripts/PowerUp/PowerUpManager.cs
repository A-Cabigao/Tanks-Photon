using System;

namespace QLE
{
    public enum PowerUpType
    {
        Ammo,
        Health,
        Damage,
        Mana
    }

    public class PowerUpManager : MonoBehaviourSingleton<PowerUpManager>
    {
        public static Action OnActivatePowerUp;
        public static Action<ProjectileInventoryInfo> OnAmmoPickUp;
    }
}
