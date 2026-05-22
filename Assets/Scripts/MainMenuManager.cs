using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        // Buradaki isim Build Settings'deki isimle (Görsel 3) birebir ayný olmalý
        SceneManager.LoadScene("InfinateRunnerGameLevel");
    }

   

    public void QuitGame()
    {
        Application.Quit();
    }
}