using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialCompletionUI : MonoBehaviour
{
    [SerializeField] private MovementTutorialManager tutorialManager;
    [SerializeField] private GameObject congratsPanel;
    [SerializeField] private Button dismissButton;
    [SerializeField] private Button nextStageButton;
    [SerializeField] private string nextSceneName = "";
    [Tooltip("If nextSceneName is empty, will load the next scene by build index.")]
    [SerializeField] private bool loadByBuildIndex = true;

    private void Start()
    {
        if (tutorialManager == null)
            tutorialManager = FindObjectOfType<MovementTutorialManager>();

        if (congratsPanel != null)
            congratsPanel.SetActive(false);

        if (dismissButton != null)
            dismissButton.onClick.AddListener(DismissCongrats);

        if (nextStageButton != null)
            nextStageButton.onClick.AddListener(LoadNextStage);

        if (tutorialManager != null)
            tutorialManager.OnTutorialComplete.AddListener(ShowCongrats);
    }

    private void ShowCongrats()
    {
        if (congratsPanel != null)
            congratsPanel.SetActive(true);

        Debug.Log("Tutorial Complete! Congratulations message displayed.");
    }

    private void DismissCongrats()
    {
        if (congratsPanel != null)
            congratsPanel.SetActive(false);

        Debug.Log("Congratulations message dismissed.");
    }

    private void LoadNextStage()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
        else if (loadByBuildIndex)
        {
            int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("Next scene name is empty and loadByBuildIndex is false. Cannot load next stage.");
        }
    }

    private void OnDestroy()
    {
        if (dismissButton != null)
            dismissButton.onClick.RemoveListener(DismissCongrats);

        if (nextStageButton != null)
            nextStageButton.onClick.RemoveListener(LoadNextStage);

        if (tutorialManager != null)
            tutorialManager.OnTutorialComplete.RemoveListener(ShowCongrats);
    }
}
