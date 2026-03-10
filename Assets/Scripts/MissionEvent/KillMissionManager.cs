using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class KillMissionManager : MonoBehaviour
{
    [SerializeField]
    private int targetKillCount = 5;

    private int currentKillCount = 0;

    [SerializeField]
    private float gameTimer = 0.0f;

    [SerializeField]
    private float gamePlayTime = 180.0f;

    [SerializeField]
    private string targetAreaName = "ClearPoint";

    [SerializeField]
    private TMP_Text text_KillCount;

    [SerializeField]
    private TMP_Text text_GameTimer;

    [SerializeField]
    private TMP_Text text_Mission;

    [SerializeField]
    private GameObject panel_GameClear;

    [SerializeField]
    private GameObject panel_GameFail;

    private int lastSecond = -1;

    private void Start()
    {
        gameTimer = gamePlayTime;
        text_Mission.text = $"시간 내 {targetKillCount}명을 처치하라!";
        panel_GameClear.SetActive(false);
        panel_GameFail.SetActive(false);
    }

    private void Update()
    {
        UpdateUI_Timer();
    }

    private void OnEnable()
    {
        MissionEventBus.OnEnemyKilled += UpdateUI_EnemyKilled;
        MissionEventBus.OnAreaReached += CheckAreaMission;
        MissionEventBus.OnGameFailed += ShowFail;
    }

    private void OnDisable()
    {
        MissionEventBus.OnEnemyKilled -= UpdateUI_EnemyKilled;
        MissionEventBus.OnAreaReached -= CheckAreaMission;
        MissionEventBus.OnGameFailed -= ShowFail;
    }

    void ShowClear()
    {
        panel_GameClear.SetActive(true);
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    void ShowFail()
    { 
        panel_GameFail.SetActive(true);
        Time.timeScale = 0.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
    }

    void UpdateUI_EnemyKilled()
    {
        currentKillCount++;
        text_KillCount.text = $"Kill : {currentKillCount} / {targetKillCount}";

        if (currentKillCount >= targetKillCount) 
        {
            text_Mission.text = "목표 구역에 도달하라!";
        }
    }

    void UpdateUI_Timer()
    {
        gameTimer -= Time.deltaTime;

        int currentSecond = Mathf.FloorToInt(gameTimer);

        if (currentSecond != lastSecond)
        {
            int minutes = currentSecond / 60;
            int seconds = currentSecond % 60;

            text_GameTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);

            lastSecond = currentSecond;
        }

        if (gameTimer <= 0.0f)
        {
            MissionEventBus.PublishGameFailed();
        }
    }

    void CheckAreaMission(string areaName)
    {
        if (areaName == targetAreaName && currentKillCount >= targetKillCount)
        {
            ShowClear();
        }
    }
}
