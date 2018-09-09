using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Food_Industry;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class TopPaybackButtonScript : UIScript {
    [SerializeField]
    Transform content;
    [SerializeField]
    Button cancel_button;
    [SerializeField]
    GameObject node_prefab;

    public void Init()
    {
        base.Start();

        List<Debt> debts = fis.GetDebts(fis.GetPlayerCompany());
        foreach(Debt debt in debts)
        {
            GameObject instance = Instantiate(node_prefab, content);
            instance.GetComponent<TopPaybackNodeScript>().Init(debt);
        }

        cancel_button.onClick.AsObservable().Subscribe(x =>
        {
            GameObject.Find("MainUI_Top").GetComponent<TopScript>().Close();
        });
    }
}
