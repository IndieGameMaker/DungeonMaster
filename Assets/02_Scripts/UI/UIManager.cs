using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GamePlay");
        // SceneManager.LoadScene(0);
    }
}
