using _Scripts.Constants;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts
{
    [CreateAssetMenu(fileName = "NewLevel", menuName = MenuConstants.GAME_LEVEL_DATA, order = 0)]
    public class LevelData : ScriptableObject
    {
        public Vector3 playerStartPosition;
        public Vector3[] LevelTilePositions;
        public LevelTileData LevelTileData;
    }
}
