using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BankInfoScript : MonoBehaviour
{
    [SerializeField]
    RectTransform money_bar, loan_bar, liabilities_bar, netassets_bar;
    [SerializeField]
    Text agent_name_text, assets_text, money_text, loan_text, liabilities_text, netassets_text;

    public void Init(Agent agent)
    {
        double asset_amount = agent.GetAssets();

        if(asset_amount == 0)
        {

        }
        else
        {
            float liabilities_height = 200.0f * (float)(agent.GetLiabilities() / asset_amount);
            liabilities_bar.sizeDelta = new Vector2(100.0f, liabilities_height);
            liabilities_bar.localPosition = new Vector3(0, 0);

            float netassets_height = 200.0f * (float)(agent.GetNetAssets() / asset_amount);
            netassets_bar.sizeDelta = new Vector2(100.0f, netassets_height);
            netassets_bar.localPosition = new Vector3(0, -liabilities_height);

            float money_height = 200.0f * (float)(agent.GetM1() / asset_amount);
            money_bar.sizeDelta = new Vector2(100.0f, money_height);
            money_bar.localPosition = new Vector3(0, 0);

            float loan_height = 200.0f * (float)(agent.GetAssets(ResourceType.Loan) / asset_amount);
            loan_bar.sizeDelta = new Vector2(100.0f, loan_height);
            loan_bar.localPosition = new Vector3(0, -money_height);
        }

        agent_name_text.text = agent.name;
        assets_text.text = $"{asset_amount:#,0}";
        money_text.text = $"{agent.GetM1():#,0}";
        loan_text.text = $"{agent.GetAssets(ResourceType.Loan):#,0}";
        liabilities_text.text = $"{agent.GetLiabilities():#,0}";
        netassets_text.text = $"{agent.GetNetAssets():#,0}";
    }
}
