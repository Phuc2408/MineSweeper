using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public Game game;
    public void ChangeSceneByName(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }
}
