using UnityEngine;
using UnityEngine.SceneManagement;
public class ReloadScene : MonoBehaviour
{
    public void ReloadGame()
    {
        SceneManager.LoadScene("SampleScene");
        Time.timeScale = 1;
    }
    public void Exitgame()
    {
        Application.Quit();
    }
}
