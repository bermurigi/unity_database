using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerData : MonoBehaviour
{
    public int money;
    public int lv;
    public int exp;
    public int score;
    public Text moText, lvText, exText,scoreText;
    public Slider expbar;
    public GameObject playPanel,gamePanel;

    private void Update()
    {
        TextUpdate();
    }

    public void TextUpdate()
    {
        //moText.text = money.ToString();
        
        lvText.text = lv.ToString();
        exText.text = exp.ToString();
        expbar.value = exp;
        scoreText.text = "최고점수: "+score.ToString();
    }

    public void GameStart()
    {
        
        
        exp++;
        money += lv;
        if (exp == 5)
        {
            lv++;
            exp = 0;
        }
        TextUpdate();
        PlayFabManager.instance.GrantVirtualCurrency();
        PlayFabManager.instance.GetVirtualCurrencies();
        playPanel.SetActive(true);
        gamePanel.SetActive(false);
        
    }

    public void GoMenu()
    {
        playPanel.SetActive(false);
        gamePanel.SetActive(true);
    }

    

     public void SetApperance(string Lv,string Exp)
     {

         exp = Int32.Parse(Exp);
         lv = Int32.Parse(Lv);
     }
}
