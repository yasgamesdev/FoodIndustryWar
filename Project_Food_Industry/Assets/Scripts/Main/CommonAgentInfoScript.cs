using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommonAgentInfoScript : MonoBehaviour
{
    [SerializeField]
    RectTransform money_bar, factory_bar, plan_bar, skill_bar, liabilities_bar, netassets_bar;
    [SerializeField]
    Text agent_name_text, assets_text, money_text, factory_text, plan_text, skill_text, liabilities_text, netassets_text;

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

            float factory_height = 200.0f * (float)(agent.GetAssets(ResourceType.Factory) / asset_amount);
            factory_bar.sizeDelta = new Vector2(100.0f, factory_height);
            factory_bar.localPosition = new Vector3(0, -money_height);

            float plan_height = 200.0f * (float)(agent.GetAssets(ResourceType.Plan) / asset_amount);
            plan_bar.sizeDelta = new Vector2(100.0f, plan_height);
            plan_bar.localPosition = new Vector3(0, -(money_height + factory_height));

            float skill_height = 200.0f * (float)(agent.GetAssets(ResourceType.Skill) / asset_amount);
            skill_bar.sizeDelta = new Vector2(100.0f, skill_height);
            skill_bar.localPosition = new Vector3(0, -(money_height + factory_height + plan_height));
        }

        if(agent is Company)
        {
            agent_name_text.text = $"<color={((Company)agent).color.GetColorNameForTag()}>{((Company)agent).color.color_name}\n{agent.name}</color>";
        }
        else
        {
            agent_name_text.text = agent.name;
        }
        assets_text.text = $"{asset_amount:#,0}";
        money_text.text = $"{agent.GetM1():#,0}";
        factory_text.text = $"{agent.GetAssets(ResourceType.Factory):#,0}";
        plan_text.text = $"{agent.GetAssets(ResourceType.Plan):#,0}";
        skill_text.text = $"{agent.GetAssets(ResourceType.Skill):#,0}";
        liabilities_text.text = $"{agent.GetLiabilities():#,0}";
        netassets_text.text = $"{agent.GetNetAssets():#,0}";
    }
}
