using System;
using UnityEngine;

namespace QLE
{
    public class TileDamageManager : MonoBehaviourSingleton<TileDamageManager>
    {
        /// <summary>
        /// defines the sprite at a specified health
        /// </summary>
        [Serializable]
        struct TileInfo {
            public Sprite tile;
            public float health;
        }
        [SerializeField] TileInfo[] tiles;
        public Sprite GetSprite(float health){
            for (int i = 0; i < tiles.Length; i++)
            {
                if(tiles[i].health < health)
                    return tiles[i].tile;
            }
            return null;
        }
    }
}
