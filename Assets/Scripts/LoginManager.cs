using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField passwordInput;
    [SerializeField] private Button loginButton;
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private string correctPassword = "password";
    [Header("Scene Loading")]
    [Tooltip("Name of the scene to load after successful login. If empty, the next build index will be loaded.")]
    [SerializeField] private string nextSceneName = "";
    [Tooltip("Delay (seconds) before loading the next scene after a successful login.")]
    [SerializeField] private float loadDelay = 0.5f;

    private void Awake()
    {
        if (loginButton != null)
            loginButton.onClick.AddListener(Login);
    }

    public void Login()
    {
        if (passwordInput == null)
        {
            Debug.LogWarning("Password input field is not assigned on LoginManager.");
            return;
        }

        bool success = passwordInput.text == correctPassword;

        if (feedbackText != null)
            feedbackText.text = success ? "Login successful" : "Incorrect password";

        Debug.Log(success ? "Login successful" : "Incorrect password");

        if (success)
        {
            if (loadDelay > 0f)
                StartCoroutine(LoadNextSceneAfterDelay(loadDelay));
            else
                LoadNextScene();
        }
    }

    // Helper if your scene uses the legacy UnityEngine.UI.InputField instead of TMP
    public void LoginFromUnityInputField(InputField input)
    {
        if (input == null) return;

        bool success = input.text == correctPassword;

        if (feedbackText != null)
            feedbackText.text = success ? "Login successful" : "Incorrect password";

        Debug.Log(success ? "Login successful" : "Incorrect password");

        if (success)
        {
            if (loadDelay > 0f)
                StartCoroutine(LoadNextSceneAfterDelay(loadDelay));
            else
                LoadNextScene();
        }
    }

    private System.Collections.IEnumerator LoadNextSceneAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
            return;
        }

        Scene current = SceneManager.GetActiveScene();
        int nextIndex = current.buildIndex + 1;
        int buildCount = SceneManager.sceneCountInBuildSettings;

        if (nextIndex < buildCount)
            SceneManager.LoadScene(nextIndex);
        else
            Debug.LogWarning($"Cannot load next scene: active scene build index {current.buildIndex} is last in build settings.");
    }
}
