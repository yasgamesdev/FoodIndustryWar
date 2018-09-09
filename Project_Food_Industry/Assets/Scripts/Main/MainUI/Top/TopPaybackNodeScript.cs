using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Food_Industry;
using UnityEngine.UI;
using UniRx;

public class TopPaybackNodeScript : UIScript
{
    [SerializeField]
    Text amount_text, payday_text;
    [SerializeField]
    Button payback_button;

    Debt debt;

    public void Init(Debt debt)
    {
        base.Start();

        this.debt = debt;

        amount_text.text = $"{debt.GetTotalPayAmount():#,0}";
        payday_text.text = debt.GetPayday().ToString("yyyy/MM/dd");

        payback_button.onClick.AsObservable().Subscribe(x =>
        {
            if(fis.GetPlayerCompany().GetM1() >= debt.GetTotalPayAmount())
            {
                fis.PayBack(debt, fis.GetPlayerCompany());
                GameObject.Find("MainUI_Top").GetComponent<TopScript>().SetPayback();
            }
        });
    }
}
