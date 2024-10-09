using System.Collections.Generic;
using System.IO;
using _Scripts;
using _Scripts.Extensions.ScriptableObjectExtensions;
using UnityEngine;
using Bogus;
using JetBrains.Annotations;

public class LevelBuilder : MonoBehaviour
{
    private const float CameraZPosition = -10f;
    private const float ObstacleProbability = 0.3f;
    private const string TileNameFormat = "TILE [{0},{1}]";
    private const string LevelDataFilePath = "/levelData.json";
    private LevelDataSO levelDataSO;
    private string savePath;

    [SerializeField]
    private GameObject groundTilePrefab;
    [SerializeField]
    private GameObject obstaclePrefab;
    [SerializeField]
    private int width = 16;
    [SerializeField]
    private int height = 9;

    /// <summary>
    ///     Initializes the level builder.
    /// </summary>
    void Start()
    {
        savePath = Application.persistentDataPath + LevelDataFilePath;
        LoadLevel();
        CenterCameraOnLevel();
    }

    /// <summary>
    ///     Generates a new level.
    /// </summary>
    /// <remarks>
    ///     The level is generated using a faker to generate tile data.
    /// </remarks>
    public void GenerateLevel()
    {
        InitializeLevelData();
        var levelDataFaker = CreateTileDataFaker();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                var tileData = levelDataFaker.Generate();
                tileData.position = new Vector2Int(x, y);
                levelDataSO.tiles.Add(tileData);

                InstantiateTile(tileData, x, y);
            }
        }
    }

    /// <summary>
    ///     Saves the generated level to a file.
    /// </summary>
    public void SaveGeneratedLevel()
    {
        ScriptableObjectSerializer<LevelDataSO>.SaveToFile(levelDataSO, savePath);
        Debug.Log($"Level Saved: {savePath}");
    }

    /// <summary>
    ///     Loads the level from a file.
    /// </summary>
    public void LoadLevel()
    {
        if (File.Exists(savePath))
        {
            levelDataSO = ScriptableObjectSerializer<LevelDataSO>.LoadFromFile(savePath);
            Debug.Log($"Level Loaded: {savePath}");
            RecreateLoadedLevel();
        }
        else
        {
            Debug.Log("No saved level found. Generating a new level...");
            GenerateAndSaveLevel();
        }
    }

    /// <summary>
    ///     Regenerates the level.
    /// </summary>
    public void RegenerateLevel()
    {
        Debug.Log("Regenerating level...");
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        GenerateLevel();
    }

    /// <summary>
    ///     Centers the camera on the level.
    /// </summary>
    private void CenterCameraOnLevel()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            Vector3 centerPosition = new Vector3((width - 1) / 2.0f, (height - 1) / 2.0f, CameraZPosition);
            mainCamera.transform.position = centerPosition;
        }
    }

    /// <summary>
    ///     Initializes the level data.
    /// </summary>
    private void InitializeLevelData()
    {
        levelDataSO = ScriptableObject.CreateInstance<LevelDataSO>();
        levelDataSO.width = width;
        levelDataSO.height = height;
        levelDataSO.tiles = new List<LevelDataSO.TileData>();
    }

    /// <summary>
    ///     Creates a faker for generating tile data.
    /// </summary>
    /// <returns>
    ///     A faker for generating tile data.
    /// </returns>
    private Faker<LevelDataSO.TileData> CreateTileDataFaker()
    {
        return new Faker<LevelDataSO.TileData>()
            .RuleFor(td => td.hasObstacle, f => f.Random.Bool(ObstacleProbability))
            .RuleFor(td => td.levelTileType, f => f.PickRandom<LevelTileType>());
    }

    
    /// <summary>
    ///     Instantiates a tile at the given position.
    /// </summary>
    /// <param name="tileData">
    ///     The tile data to use for instantiation.
    /// </param>
    /// <param name="x">
    ///     The x position of the tile.
    /// </param>
    /// <param name="y">
    ///     The y position of the tile.
    /// </param>
    private void InstantiateTile(LevelDataSO.TileData tileData, int x, int y)
    {
        Vector3 position = new Vector3(x, y, 0);
        GameObject instance = tileData.hasObstacle ? obstaclePrefab : groundTilePrefab;
        GameObject tileInstance = Instantiate(instance, position, Quaternion.identity);
        tileInstance.name = string.Format(TileNameFormat, x, y);
        
    }

    /// <summary>
    ///     Recreates the level from the loaded level data.
    /// </summary>
    private void RecreateLoadedLevel()
    {
        foreach (LevelDataSO.TileData tileData in levelDataSO.tiles)
        {
            InstantiateTile(tileData, tileData.position.x, tileData.position.y);
        }
    }

    /// <summary>
    ///     Generates a new level and saves it to a file.
    /// </summary>
    private void GenerateAndSaveLevel()
    {
        GenerateLevel();
        SaveGeneratedLevel();
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
