using System.Collections.Generic;
using _Scripts.Constants;
using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = "Game/Level Data", order = 0)]
    public class LevelDataSO : ScriptableObject
    {
        public int width = 16;
        public int height = 9;
        public List<TileData> tiles;
        
        [System.Serializable]
        public class TileData
        {
            public bool hasObstacle;
            public LevelTileType levelTileType; 
            public Vector2Int position;
        }
    }
    
    public enum LevelTileType
    {
        Ground,
        Hole
    }
}
