using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;

public class NextScript : UIScript
{
    [SerializeField]
    Button next_button;

    protected override void Start()
    {
        base.Start();

        fis.GetSubject(NotificationType.GameOver).Subscribe(x => {
            next_button.interactable = false;
        });

        next_button.onClick.AsObservable().Subscribe(x =>
        {
            fis.Stop();

            while (fis.GetState() != CalculatorState.Stop)
            {
                Task.Delay(1);
            }

            fis.Next();
        });

        fis.GetSubject(NotificationType.EndNext).Subscribe(x => {
            if (!fis.IsGameOver())
            {
                fis.Calculate();
            }
        });

        fis.Calculate();
    }
}
