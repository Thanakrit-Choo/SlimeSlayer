using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DemoSceneManager : MonoBehaviour
{
    public GameObject player;
    public GameObject enemy;

    PlayerController2Dnew playerCon;
    SlimeBoss slimeBoss;
    PlayerHealth playerHealth;
    // Start is called before the first frame update
    void Start()
    {
        playerCon = player.GetComponent<PlayerController2Dnew>();
        slimeBoss = enemy.GetComponent<SlimeBoss>();
        playerHealth = player.GetComponent<PlayerHealth>();
        FindObjectOfType<AudioManager>().PlaySound("BGMusic");
    }

    // Update is called once per frame
    void Update()
    {
        if(slimeBoss.isDead)
        {
            playerCon.isWin = true;
            FindObjectOfType<AudioManager>().StopSound("BGMusic");
            StartCoroutine(LoadScene("MainMenu"));
        }
        if(playerHealth.GetisDead())
        {
            FindObjectOfType<AudioManager>().StopSound("BGMusic");
            StartCoroutine(LoadScene("MainMenu"));
        }
    }

    IEnumerator LoadScene(string name)
    {
        yield return new WaitForSeconds(5);
        SceneManager.LoadSceneAsync(name);
    }
}
