using Photon.Pun;
using UnityEngine;

namespace QLE
{
    public class ProjectileManager : SingletonPUN<ProjectileManager>
    {
        [SerializeField] GameObject[] projectiles;

        public GameObject Instantiate(string projectileName, Vector3 position, Quaternion rotation)
        {
            for (int i = 0; i < projectiles.Length; i++)
            {
                if(projectileName == projectiles[i].name){
                    Projectile proj = PhotonNetwork.InstantiateRoomObject(projectileName,position,rotation).GetComponent<Projectile>();
                    return proj.gameObject;
                }
            }
            Debug.LogError("No projectile found in scriptable objects");
            return null;
        }
    }
}
