using System.Collections;
using System.Collections.Generic;
using _Scripts;
using UnityEngine;


public class LevelBuilder : MonoBehaviour
{
    public int[] levelArea;
    
    public string[][] levelData;
    
    [SerializeField]
    private LevelData levelDataScriptableObject;
    
    [SerializeField]
    private GameObject levelTilePrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        // CreateLevelArea();
        CreateLevelData();
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
    
    private void CreateLevelData()
    {
        levelData = new []
            {
                new [] {"0-8", "12-15"},
                new [] {"0-15"},
                new [] {"0-15"},
                new [] {"0-3", "7-8", "12-15"},
            };
        
        for (var y = 0; y < levelData.Length; y++)
        {
            for (var x = 0; x < levelData[y].Length; x++)
            {
                Debug.Log($"LevelData[{x}][{y}] = {levelData[y][x]}");
                
                if (levelData[y][x].Contains("-"))
                {
                    var levelDataSplit = levelData[y][x].Split('-');
                    var start = int.Parse(levelDataSplit[0]);
                    var end = int.Parse(levelDataSplit[1]);
                    
                    for (var i = start; i <= end; i++)
                    {
                        GameObject levelTile = Instantiate(levelTilePrefab, new Vector3(i, y, 0), Quaternion.identity);
                        levelTile.transform.localScale = new Vector3(1, 1, 1);
                        levelTile.transform.position = new Vector3(i, y, 0);
                        levelTile.name = $"Tile_{i}_{y}";

                        var isOffset = (i + y) % 2 == 0;
                        levelTile.GetComponent<SpriteRenderer>().color = isOffset ? Color.black : Color.white;
                    }
                }
                else
                {
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
}
