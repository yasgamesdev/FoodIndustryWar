using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;

public class BotScript : UIScript
{
    [SerializeField]
    Text date_text, population_text, money_text;

    protected override void Start()
    {
        base.Start();

        date_text.text = fis.GetCurrentDateTime().ToString("yyyy/MM/dd");
        population_text.text = $"{fis.GetPopulation():#,0}";
        money_text.text = $"{fis.GetPlayerCompany().GetM1():#,0}";

        fis.GetSubject(NotificationType.ChangeDate).Subscribe(x => {
            System.DateTime date = x.Get<System.DateTime>(0);
            date_text.text = date.ToString("yyyy/MM/dd");
        });

        fis.GetSubject(NotificationType.ChangePopulation).Subscribe(x => {
            double size = x.Get<double>(0);
            population_text.text = $"{size:#,0}";
        });

        fis.GetSubject(NotificationType.ChangePlayerMoney).Subscribe(x => {
            double money = x.Get<double>(0);
            money_text.text = $"{money:#,0}";
        });
    }
}
