using UnityEngine;
using UnityEngine.SceneManagement;
public class RestartManager : MonoBehaviour
{
    

    public void PauseGame()
    {
        SceneManager.LoadScene("MainMenu");
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

}
