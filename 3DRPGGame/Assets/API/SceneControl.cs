using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControl : MonoBehaviour
{
    
    public void Replay()
    {
        Invoke("DelayRe", 0.7f);
    }
    void DelayRe()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void BackToMenu()
    {
        Invoke("DelBackM", 0.7f);
    }
    void DelBackM()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Invoke("DelQ", 0.7f);
    }
    void DelQ()
    {
        Application.Quit();
    }
}
