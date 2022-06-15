using UnityEngine;

namespace QLE
{
    /// <summary>
    /// Contains information about bullet damage, range, cost
    /// </summary>
    [CreateAssetMenu(fileName = "Bullet Name", menuName = "Scriptable Objects/Bullet Info")]
    public class ProjectileInfo : ScriptableObject {
        [Tooltip("Damage it will be inflicting within its range")]
        public float damage = 50;

        [Tooltip("Damage radius")]
        public float range = 2.5f;
        
        [Tooltip("How fast the bullet is when shot")]
        public float speed = 2.5f;
        
        [Tooltip("Price per ammo bought")]
        public float price = 5;

        [Tooltip("Defines how heavy the bullet is")]
        public float weight = 1;

        public bool doesSplashDamage = true;
    }
}
