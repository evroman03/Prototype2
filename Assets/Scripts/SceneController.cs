using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called before the first frame update
    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    //Level Select
    public void PlayLevel1()
    {
        SceneManager.LoadScene(2);
    }
    public void PlayLevel2()
    {
        SceneManager.LoadScene(3);
    }
    public void PlayLevel3()
    {
        SceneManager.LoadScene(4);
    }
    //Menu
    public void HowToPlay()
    {
        SceneManager.LoadScene(5);
    }
    public void Cards()
    {
        SceneManager.LoadScene(6);
    }
    public void Credits()
    {
        SceneManager.LoadScene(7);
    }
    
    
    public void Quit()
    {
        print("Let Me Out");
        Application.Quit();
    }
}
