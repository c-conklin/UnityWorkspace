using System;
using System.Collections.Generic;
using System.IO;
using _Scripts;
using _Scripts.Extensions.ScriptableObjectExtensions;
using UnityEngine;
using Bogus;
using Random = UnityEngine.Random;


public class LevelBuilder : MonoBehaviour
{
    public GameObject groundTilePrefab;
    public GameObject obstaclePrefab;
    public int width = 10;
    public int height = 10;
    public LevelDataSO levelDataSO;

    private string savePath;

    void Start()
    {
        savePath = Application.persistentDataPath + "/levelData.json";
        LoadLevel();
        CenterCameraOnLevel();
    }

    public void GenerateLevel()
    {
        levelDataSO = ScriptableObject.CreateInstance<LevelDataSO>();
        levelDataSO.width = width;
        levelDataSO.height = height;

        var levelDataFaker = new Faker<LevelDataSO.TileData>()
            .RuleFor(td => td.hasObstacle, f => f.Random.Bool(0.3f))
            .RuleFor(td => td.levelTileType, f => f.PickRandom<LevelTileType>());

        levelDataSO.tiles = new List<LevelDataSO.TileData>();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var tileData = levelDataFaker.Generate();
                tileData.position = new Vector2Int(x, y);
                levelDataSO.tiles.Add(tileData);

                Vector3 position = new Vector3(x, y, 0);
                var instance = tileData.hasObstacle ? obstaclePrefab : groundTilePrefab;
                var tileInstance = Instantiate(instance, position, Quaternion.identity);
                tileInstance.name = $"TILE [{x},{y}]";
            }
        }
    }

    public void SaveGeneratedLevel()
    {
        ScriptableObjectSerializer<LevelDataSO>.SaveToFile(levelDataSO, savePath);
        Debug.Log("Level Saved: " + savePath);
    }

    public void LoadLevel()
    {
        if (File.Exists(savePath))
        {
            levelDataSO = ScriptableObjectSerializer<LevelDataSO>.LoadFromFile(savePath);
            Debug.Log("Level Loaded: " + savePath);

            // Recreate level from loaded data
            foreach (var tileData in levelDataSO.tiles)
            {
                Vector3 position = new Vector3(x: tileData.position.x, z: 0, y: tileData.position.y);
                var instance = tileData.hasObstacle ? obstaclePrefab : groundTilePrefab;
                var tileInstance = Instantiate(instance, position, Quaternion.identity);
                tileInstance.name = $"TILE [{tileData.position.x},{tileData.position.y}]";
            }
        }
        else
        {
            Debug.Log("No saved level found. Generating a new level...");
            GenerateLevel();
            SaveGeneratedLevel();  // Save the newly generated level for future use
        }
    }
    
    public void RegenerateLevel()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        GenerateLevel();
    }
    
    private void CenterCameraOnLevel()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            // Calculate the center of the level
            var centerX = (width - 1) / 2.0f;
            var centerY = (height - 1) / 2.0f;

            // Set camera position to center above the level
            Vector3 centerPosition = new Vector3(centerX, centerY, -10f); // Adjust y value for height above level
            mainCamera.transform.position = centerPosition;
        }
    }
    
    // private void CreateLevelData()
    // {
    //     levelData = new []
    //         {
    //             new [] {"0-8", "12-15"},
    //             new [] {"0-15"},
    //             new [] {"0-15"},
    //             new [] {"0-3", "7-8", "12-15"},
    //         };
    //     
    //     for (var y = 0; y < levelData.Length; y++)
    //     {
    //         for (var x = 0; x < levelData[y].Length; x++)
    //         {
    //             Debug.Log($"LevelData[{x}][{y}] = {levelData[y][x]}");
    //             
    //             if (levelData[y][x].Contains("-"))
    //             {
    //                 var levelDataSplit = levelData[y][x].Split('-');
    //                 var start = int.Parse(levelDataSplit[0]);
    //                 var end = int.Parse(levelDataSplit[1]);
    //                 
    //                 for (var i = start; i <= end; i++)
    //                 {
    //                     GameObject levelTile = Instantiate(levelTilePrefab, new Vector3(i, y, 0), Quaternion.identity);
    //                     levelTile.transform.localScale = new Vector3(1, 1, 1);
    //                     levelTile.transform.position = new Vector3(i, y, 0);
    //                     levelTile.name = $"Tile_{i}_{y}";
    //
    //                     var isOffset = (i + y) % 2 == 0;
    //                     levelTile.GetComponent<SpriteRenderer>().color = isOffset ? Color.black : Color.white;
    //                 }
    //             }
    //             else
    //             {
    //                 GameObject levelTile = Instantiate(levelTilePrefab, new Vector3(x, y, 0), Quaternion.identity);
    //                 levelTile.transform.localScale = new Vector3(1, 1, 1);
    //                 levelTile.transform.position = new Vector3(x, y, 0);
    //                 levelTile.name = $"Tile_{x}_{y}";
    //                 
    //                 var isOffset = (x + y) % 2 == 0;
    //                 levelTile.GetComponent<SpriteRenderer>().color = isOffset ? Color.black : Color.white;
    //             }
    //         }
    //     }
    // }
}
