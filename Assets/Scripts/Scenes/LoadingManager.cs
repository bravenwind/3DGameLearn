using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LoadingManager : MonoBehaviour
{
    [SerializeField]
    private Image loadingBar;

    [SerializeField]
    private TMP_Text progressText;

    [SerializeField]
    private string nextSceneName = "Game";

    private void Start()
    {
        loadingBar.fillAmount = 0.0f;
        progressText.text = "Loading... 0%";
        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess()
    {
        // 비동기 로딩 시작
        AsyncOperation operation = SceneManager.LoadSceneAsync(nextSceneName);

        // 로딩이 끝나도 자동으로 씬을 넘기지 않도록 막음.
        // 로딩이 너무 빠를 때 로딩 화면이 휙 지나가는 것 방지
        operation.allowSceneActivation = false;

        float timer = 0.0f;

        while (!operation.isDone)
        {
            yield return null;  // 한 프레임 대기

            timer += Time.deltaTime;
            Mathf.Clamp01(timer);

            if (operation.progress < 0.9f)
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, operation.progress, timer);

                if (progressText != null)
                {
                    // Mathf.RoundToInt : 반올림해서 정수로 반환해 주는 함수
                    progressText.text = "Loading..." + Mathf.RoundToInt(loadingBar.fillAmount * 100.0f) + "%";
                }

                if (loadingBar.fillAmount >= operation.progress)
                {
                    timer = 0.0f;
                }
            }
            else
            {
                loadingBar.fillAmount = Mathf.Lerp(loadingBar.fillAmount, 1.0f, timer);

                if (progressText != null)
                {
                    progressText.text = "Loading... 100%";
                }

                if (loadingBar.fillAmount == 1.0f)
                {
                    operation.allowSceneActivation = true;
                }
            }
        }
        

    }
}
