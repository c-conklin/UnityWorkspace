using System.Collections.Generic;
using _Scripts.Constants;
using _Scripts.Models;
using UnityEngine;

namespace _Scripts
{
    
    
    [CreateAssetMenu(fileName = "NewCoreLevelTile", menuName = MenuConstants.GAME__CORE__LEVEL_TILE, order = 0)]
    public abstract class CoreLevelTileData : ScriptableObject
    {
        public Dictionary<string, Color> TileColors;
        public Dictionary<string, GameObject> TilePrefabs;
        
        public virtual void InitializeTileData()
        {
            TileColors = new Dictionary<string, Color>();
            TilePrefabs = new Dictionary<string, GameObject>();
        }
        
        public virtual void AddTileData(string tileName, Color tileColor, GameObject tilePrefab)
        {
            TileColors.Add(tileName, tileColor);
            TilePrefabs.Add(tileName, tilePrefab);
        }
        
        public virtual Color GetTileColor(string tileName)
        {
            return TileColors[tileName];
        }
        
        public virtual GameObject GetTilePrefab(string tileName)
        {
            return TilePrefabs[tileName];
        }
    }
}
