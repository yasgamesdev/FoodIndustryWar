using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;

public class TopResearchButtonScript : UIScript
{
    [SerializeField]
    Text top_text, bot_text;
    [SerializeField]
    Image button_surface;
    [SerializeField]
    GameObject balloon;
    [SerializeField]
    Text balloon_text;

    TechnologyType type;

    public void Init(TechnologyType type)
    {
        base.Start();

        this.type = type;

        top_text.text = Lib.GetTechnologyName(type);
        bot_text.text = $"{fis.GetPlayerCompany().GetTechnologySuccessProbability(type):P}";

        Research research = fis.GetPlayerCompany().GetResearch();
        if (research.research_type == ResearchType.Technology && research.technology_type == type)
        {
            button_surface.color = new Color(0.0f, 192.0f / 255.0f, 1.0f);
        }

        GetComponent<Button>().onClick.AsObservable().Subscribe(x =>
        {
            fis.GetPlayerCompany().GetResearch().SetResearchTechnology(type);

            GameObject.Find("MainUI_Left").GetComponent<LeftScript>().SetCaution();
            GameObject.Find("MainUI_Mid").GetComponent<MidScript>().SetResearch();
            GameObject.Find("MainUI_Top").GetComponent<TopScript>().SetResearch();

            GameObject.Find("MainUI_Top").GetComponent<AudioSource>().Play();

            GameObject.Find("Tutorial").GetComponent<TutorialScript>().SelectTopResearch();
        });

        string text = "";
        fis.GetAllFood().Where(x => x.tec_types.Contains(type)).ToList().ForEach(x => text += x.food_name + "\n");
        if (text == "")
        {
            text = "なし";
        }
        balloon_text.text = text;

        balloon.SetActive(false);
    }

    public void PointEnter()
    {
        balloon.SetActive(true);
    }

    public void PointExit()
    {
        balloon.SetActive(false);
    }
}
