using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FloatText : MonoBehaviour
{
    // Start is called before the first frame update
    public float textDuration;
    public GameObject textComponent;
    private float textLifeTime;

    void Start()
    {
        textLifeTime = textDuration;

    }

   
    // Update is called once per frame
    void Update()
    {
        textLifeTime -= Time.deltaTime;
        if(textLifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetText(string text)
    {
        this.gameObject.SetActive(true);
        this.textComponent.GetComponent<TextMeshPro>().text = text;
        textLifeTime = textDuration;
    }
}
