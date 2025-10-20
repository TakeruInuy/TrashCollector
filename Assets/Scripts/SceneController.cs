using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneController : MonoBehaviour
{




    // 🔹 Carregar cena por nome
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // 🔹 Recarregar a cena atual
    public void ReloadCurrentScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;
        LoadScene(sceneName);
    }

    // 🔹 Voltar ao menu principal
    public void LoadMainMenu()
    {
        LoadScene("MainMenu");
    }

    // 🔹 Carregar próxima cena (por build index)
    public void LoadNextScene()
    {
        int current = SceneManager.GetActiveScene().buildIndex;
        int next = current + 1;

        if (next < SceneManager.sceneCountInBuildSettings)
            LoadScene(SceneUtility.GetScenePathByBuildIndex(next));
        else
            Debug.LogWarning("Nenhuma próxima cena configurada!");
    }

    // 🔹 Método assíncrono com tela de loading opcional
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // (Opcional) Carrega tela de loading primeiro
        // yield return SceneManager.LoadSceneAsync("LoadingScreen");

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            // Você pode exibir uma barra de progresso aqui, se quiser
            // float progress = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }
    }

    public void QuitGame()
    {

        Application.Quit();
    }

}
