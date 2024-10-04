using _Scripts.Constants;
using UnityEngine;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "NewLevelTile", menuName = MenuConstants.GAME_LEVEL_TILE_DATA, order = 0)]
    public class LevelTileData : CoreLevelTileData
    {
        public GameObject TilePrefab;
        
        public override void InitializeTileData()
        {
            base.InitializeTileData();
            AddTileData("Grass", Color.green, TilePrefab);
            AddTileData("Water", Color.blue, TilePrefab);
            AddTileData("Sand", Color.yellow, TilePrefab);
        }
        
        
    }
}
