using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;
    
    public void StartButton()
    {
        StartCoroutine(LoadScene("SlimeSlayer"));
        Debug.Log("ez");
    }

    IEnumerator LoadScene(string name)
    {
        loadingScreen.SetActive(true);
        yield return new WaitForSeconds(1);
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            slider.value = progress;
            yield return null;
        }
        
    }

    public void QuitButton()
    {
        Debug.Log("Quit");
        Application.Quit();
    }
}
