using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject ui;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            ShowUI();
        }
        if(Input.GetKeyDown(KeyCode.B))
        {
            HideUI();
        }
    }
    
    public void ShowUI()
    {
        StartCoroutine(FadeIn());   
    }

    public void HideUI()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeIn()
    {
        canvasGroup.alpha = 0;
        ui.SetActive(true);
        while (canvasGroup.alpha < 1 )
        {
            canvasGroup.alpha += Time.deltaTime * 5;
            yield return null;
        }
    }

    IEnumerator FadeOut()
    {
        canvasGroup.alpha = 1;
        while(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime*5;
            yield return null;
        }
        ui.SetActive(false);
    }
}
