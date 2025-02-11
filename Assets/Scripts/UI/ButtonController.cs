using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    private const float DOUBLE_CLICK_TIME = 0.2f;

    private float lastTouch = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount <= 0)
        {
            return;
        }
        if (Input.GetTouch(0).tapCount == 2)
        {
            //Double tap
            StartCoroutine(LoadScene("game"));
            
        }

        else if (Input.GetTouch(0).tapCount == 1)
        {
            //Single tap
           // FindObjectOfType<AudioManager>().PlaySound("FullClap");
            Input.GetKeyDown("enter");
        }
    }

    IEnumerator LoadScene(string name)
    {
        //loadingScreen.SetActive(true);
        yield return new WaitForSeconds(1);
        AsyncOperation operation = SceneManager.LoadSceneAsync(name);
        

    }
}
