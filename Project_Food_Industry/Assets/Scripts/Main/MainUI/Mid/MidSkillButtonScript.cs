using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Food_Industry;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class MidSkillButtonScript : UIScript
{
    [SerializeField]
    MidSkillNodeScript[] nodes;
    [SerializeField]
    Button close_button;

    public void Init()
    {
        base.Start();

        close_button.onClick.AsObservable().Subscribe(x => {
            GameObject.Find("Skill_Button").GetComponent<Button>().onClick.Invoke();
        });

        SetUI();
    }

    void SetUI()
    {
        nodes[0].Init(this, SKillType.reduce_construction_cost);
        nodes[1].Init(this, SKillType.increases_research_probability);
    }

    public void LevelUpClicked(SKillType type)
    {
        fis.GetPlayerCompany().GetSkill().LevelUp(type);
        SetUI();
    }
}
