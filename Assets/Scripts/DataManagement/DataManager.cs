using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class DataManager : MonoBehaviour
{
    public static DataManager instance;

    public GameData currentData;

    private string savePath;

    private void Awake()
    {
        instance = this;

        // 저장 경로 설정 : 유니티가 제공하는 내부 저장소의 경로 + 저장할 파일의 이름을 합쳐서 최종 경로 문자열 생성  
        savePath = Path.Combine(Application.persistentDataPath, "SavedData.json");

        LoadGameByPlayerPrefs();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.O))
        {
            SaveGame();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            SaveGameByPlayerPrefs();
        }
    }

    public void SaveGame()
    {
        // 클래스의 멤버 변수들을 직렬화 ->
        string jsonString = JsonUtility.ToJson(currentData, true);

        File.WriteAllText(savePath, jsonString);

        Debug.Log("내부 저장소 경로: " + savePath);
        Debug.Log("저장된 내용: " + jsonString);
    }

    public void SaveGameByPlayerPrefs()
    {
        // int, float, string
        PlayerPrefs.SetInt("Score", 100);
        PlayerPrefs.SetFloat("HP", 50.0f);
        PlayerPrefs.SetString("PlayerName", "Je");

        Vector3 position = new Vector3(1.0f, 10.0f, 20.0f);

        PlayerPrefs.SetFloat("PlayerXPos", position.x);
        PlayerPrefs.SetFloat("PlayerYPos", position.y);
        PlayerPrefs.SetFloat("PlayerZPos", position.z);
    }

    public void LoadGameByPlayerPrefs()
    {
        int score = PlayerPrefs.GetInt("Score", 0);
        float hp = PlayerPrefs.GetFloat("HP", 0);
        string playerName = PlayerPrefs.GetString("PlayerName", "Player");

        Vector3 position = new Vector3(PlayerPrefs.GetFloat("PlayerXPos", 0.0f), PlayerPrefs.GetFloat("PlayerYPos", 0.0f), PlayerPrefs.GetFloat("PlayerZPos", 0.0f));

        Debug.Log($"{playerName} {score}점, hp : {hp}, position : {position}");
    }

    public void LoadGame()
    {
        if (File.Exists(savePath))
        {
            string jsonString = File.ReadAllText(savePath);

            currentData = JsonUtility.FromJson<GameData>(jsonString);
        }
        else
        {
            currentData = new GameData();
            Debug.Log("저장 파일이 존재하지 않음");
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
