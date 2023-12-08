using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NodeUI : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public GameObject ui;
    private Node target;
    public Text costText;
    public Text sellText;
    public Button upgradeBtn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

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
    public void SetTarget(Node _target)
    {
        target = _target;
        transform.position = target.GetUiOffsetPosition();
        if(target.isUpgraded)
        {
            costText.text = "Upgraded";
            upgradeBtn.interactable = false;
        }
        else
        {
            costText.text = "   cost:" + target.selectedTurretDesign.upgradeCost;
            upgradeBtn.interactable = true;
        }
        sellText.text = "   sell" + "$" + target.selectedTurretDesign.SellAmount;
        
    }

    public void UpgradeBtnClicked()
    {
        target.UpgradeTurret();
        BuildManager.Instance.DeSelect();
    }

    public void SellBtnClicked()
    {
        target.SellTurret();
        BuildManager.Instance.DeSelect();
    }
}
