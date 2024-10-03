using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBuilder : MonoBehaviour
{
    public int[] levelArea;
    
    [SerializeField]
    private GameObject levelTilePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        CreateLevelArea();
    }

    private void CreateLevelArea()
    {
        var longestRow = levelArea[0];
        var longestColumn = levelArea.Length;
        for (var y = 0; y < levelArea.Length; y++)
        {
            for (var x = 0; x < levelArea[y]; x++)
            {
                if (levelArea[y] > longestRow)
                {
                    longestRow = levelArea[y];
                }

                GameObject levelTile = Instantiate(levelTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                levelTile.transform.localScale = new Vector3(1, 1, 1);
                levelTile.transform.position = new Vector3(x, y, 0);
                levelTile.name = $"Tile_{x}_{y}";

                var isOffset = (x + y) % 2 == 0;
                levelTile.GetComponent<SpriteRenderer>().color = isOffset ? Color.black : Color.white;
            }
        }
    }
}
