using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Button saveButton;
    public Button loadButton;
    public Button generateButton;
    public LevelBuilder levelBuilder;

    void Start()
    {
        saveButton.onClick.AddListener(levelBuilder.SaveGeneratedLevel);
        loadButton.onClick.AddListener(levelBuilder.LoadLevel);
        generateButton.onClick.AddListener(levelBuilder.RegenerateLevel);
    }
}
