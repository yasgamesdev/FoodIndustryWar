using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Food_Industry;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class MidSkillNodeScript : UIScript
{
    [SerializeField]
    Text level_text, next_level_cost_text;
    [SerializeField]
    Button level_up_button;

    MidSkillButtonScript parent;
    SKillType type;

    public void Init(MidSkillButtonScript parent, SKillType type)
    {
        base.Start();

        this.parent = parent;
        this.type = type;

        level_text.text = $"{fis.GetPlayerCompany().GetSkill().GetLevel(type)}";
        next_level_cost_text.text = $"+{Lang.Get("main_level_up")}\n{fis.GetPlayerCompany().GetSkill().GetLevelUpCost(type):#,0}";
        level_up_button.interactable = fis.GetPlayerCompany().GetSkill().IsPossibleLevelUp(type);
    }

    public void LevelUpClicked()
    {
        parent.LevelUpClicked(type);
    }
}
