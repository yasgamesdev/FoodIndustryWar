using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Food_Industry;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class TopBorrowButtonScript : UIScript {
    [SerializeField]
    InputField borrow_amount_inputfield;
    [SerializeField]
    Button borrow_button, cancel_button;
    [SerializeField]
    Text limit_text, current_interest_text, estimate_interest_text;

    public void Init()
    {
        base.Start();

        limit_text.text = $"({Lang.Get("main_credit_limit")} = {fis.GetMaxBorrowAmount(fis.GetPlayerCompany()):#,0})";
        current_interest_text.text = $"{fis.GetInterestRate(0):P}";

        borrow_amount_inputfield.onValueChanged.AsObservable().Subscribe(x =>
        {
            double borrow_amount = 0;
            if (double.TryParse(borrow_amount_inputfield.text, out borrow_amount))
            {
                estimate_interest_text.text = $"{fis.GetInterestRate(borrow_amount) + fis.GetIndividualInterestRate(borrow_amount, fis.GetPlayerCompany()):P}";
            }
        });

        borrow_button.onClick.AsObservable().Subscribe(x =>
        {
            double borrow_amount = 0;
            if(double.TryParse(borrow_amount_inputfield.text, out borrow_amount))
            {
                if(0 < borrow_amount && borrow_amount <= fis.GetMaxBorrowAmount(fis.GetPlayerCompany()))
                {
                    fis.BorrowMoney(fis.GetPlayerCompany(), borrow_amount);
                    GameObject.Find("MainUI_Top").GetComponent<TopScript>().Close();
                }
            }
        });

        cancel_button.onClick.AsObservable().Subscribe(x =>
        {
            GameObject.Find("MainUI_Top").GetComponent<TopScript>().Close();
        });
    }

    public void SetUI()
    {
        limit_text.text = $"(限度額 = {fis.GetMaxBorrowAmount(fis.GetPlayerCompany()):#,0})";
        current_interest_text.text = $"{fis.GetInterestRate(0):P}";

        double borrow_amount = 0;
        if (double.TryParse(borrow_amount_inputfield.text, out borrow_amount))
        {
            estimate_interest_text.text = $"{fis.GetInterestRate(borrow_amount) + fis.GetIndividualInterestRate(borrow_amount, fis.GetPlayerCompany()):P}";
        }
    }
}
