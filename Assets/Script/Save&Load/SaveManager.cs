using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    private FileDataHandler dataHandler;
    private GameData gameData;
    private List<ISaveable> allSaveables;

    [SerializeField] private string fileName = "Stats.json";
    [SerializeField] private bool encryptData;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log(Application.persistentDataPath);
            dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
            allSaveables = FindISaveables();
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {

    }

    public void LoadGame()
    {
        gameData = dataHandler.LoadData();

        if (gameData == null)
        {
            Debug.Log("No save data found, creating new save!");
            gameData = new GameData();
            return;
        }

        // 第一次尝试查找
        var currentSaveables = FindISaveables();

        // 如果没找到，延迟重试
        if (currentSaveables.Count == 0)
        {
            Debug.Log("No saveables found, retrying...");
            StartCoroutine(RetryLoadGame());
            return;
        }

        foreach (var saveable in currentSaveables)
            saveable.LoadData(gameData);

        Debug.Log($"Loaded game with {currentSaveables.Count} saveable components");
    }

    private System.Collections.IEnumerator RetryLoadGame()
    {
        yield return new WaitForSeconds(0.1f); // 等待100ms

        var retrySaveables = FindISaveables();
        Debug.Log($"Retry found {retrySaveables.Count} saveable components");

        foreach (var saveable in retrySaveables)
            saveable.LoadData(gameData);
    }
    public void SaveGame()
    {
        var currentSaveables =FindISaveables();
        foreach (var saveable in currentSaveables)
            saveable.SaveData(ref gameData);

        dataHandler.SaveData(gameData);
        Debug.Log("save game");
    }

    public GameData GetGameData() => gameData;

    [ContextMenu("Delete save data")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encryptData);
        dataHandler.Delete();

        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveable> FindISaveables()
    {
        return
            FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include, FindObjectsSortMode.None)
            .OfType<ISaveable>()
            .ToList();
    }

    public bool HasSavedData()
    {
        if(dataHandler.LoadData()!=null)
        {
            return true;
        }
        return false;
    }
}
