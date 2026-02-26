using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName = "Loading";

    [SerializeField]
    private GameObject difficultyPanel;

    public void OnClickStartButton()
    {
        Debug.Log("게임을 시작합니다");

        difficultyPanel.SetActive(true);
    }

    public void OnClickExitButton()
    {
        Debug.Log("게임을 종료합니다.");

#if UNITY_EDITOR

#else
        Application.Quit();
#endif

    }

    public void SelectEasy()
    {
        GameManager.Instance.currentDifficultyData = GameManager.Instance.easyData;
        SceneManager.LoadScene(nextSceneName);
    }

    public void SelectHard()
    {
        GameManager.Instance.currentDifficultyData = GameManager.Instance.hardData;
        SceneManager.LoadScene(nextSceneName);
    }
}
