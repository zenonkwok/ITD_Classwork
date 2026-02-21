using UnityEngine;
using UnityEngine.UI;

public class TutorialCompletionUI : MonoBehaviour
{
    [SerializeField] private MovementTutorialManager tutorialManager;
    [SerializeField] private GameObject congratsPanel;
    [SerializeField] private Button dismissButton;

    private void Start()
    {
        if (tutorialManager == null)
            tutorialManager = FindObjectOfType<MovementTutorialManager>();

        if (congratsPanel != null)
            congratsPanel.SetActive(false);

        if (dismissButton != null)
            dismissButton.onClick.AddListener(DismissCongrats);

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

    private void OnDestroy()
    {
        if (dismissButton != null)
            dismissButton.onClick.RemoveListener(DismissCongrats);

        if (tutorialManager != null)
            tutorialManager.OnTutorialComplete.RemoveListener(ShowCongrats);
    }
}
