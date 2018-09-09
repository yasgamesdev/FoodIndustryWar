using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;
using System;

public class MidScript : UIScript
{
    [SerializeField]
    Transform button_parent;
    [SerializeField]
    GameObject mid_button_prefab, mid_skill_button_prefab;
    [SerializeField]
    TopScript top_script;

    public MidScriptState state { get; private set; }
    List<GameObject> instances = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        fis.GetSubject(NotificationType.GameOver).Subscribe(x => {
            Close();
        });

        fis.GetSubject(NotificationType.EndNext).Subscribe(x => {
            if (state == MidScriptState.ResearchOpen)
            {
                SetResearch();
            }
            else if(state == MidScriptState.SkillOpen)
            {
                SetSkill();
            }
        });

        Close();
    }

    public void Close()
    {
        state = MidScriptState.Close;
        gameObject.SetActive(false);
    }

    void Clear()
    {
        instances.ForEach(x => Destroy(x));
        instances.Clear();
    }

    public void SetBuild()
    {
        Clear();

        state = MidScriptState.BuildOpen;
        gameObject.SetActive(true);

        float x_size = 60.0f * (System.Enum.GetValues(typeof(FoodClassification)).Length - 1);
        float x_pos = x_size * -0.5f;
        foreach (FoodClassification classification in System.Enum.GetValues(typeof(FoodClassification)))
        {
            if (classification != FoodClassification.None)
            {
                GameObject instance = Instantiate(mid_button_prefab, button_parent);
                instance.GetComponent<MidButtonScript>().Init(this, Lib.GetFoodCassificationName(classification), (int)classification);
                instance.GetComponent<RectTransform>().localPosition = new Vector3(x_pos, 0);
                x_pos += 60.0f;

                instances.Add(instance);
            }
        }
    }

    public void SetResearch()
    {
        Clear();

        state = MidScriptState.ResearchOpen;
        gameObject.SetActive(true);

        Research research = fis.GetPlayerCompany().GetResearch();

        float x_size = 60.0f * (System.Enum.GetValues(typeof(TechnologyClassification)).Length + 1);
        float x_pos = x_size * -0.5f;
        foreach (TechnologyClassification classification in System.Enum.GetValues(typeof(TechnologyClassification)))
        {
            GameObject instance = Instantiate(mid_button_prefab, button_parent);
            instance.GetComponent<MidButtonScript>().Init(this, Lib.GetTechnologyCassificationName(classification), (int)classification);
            instance.GetComponent<RectTransform>().localPosition = new Vector3(x_pos, 0);
            x_pos += 60.0f;

            instances.Add(instance);

            if (research.research_type == ResearchType.Technology &&
                (((GameData)fis.GetModule(ModuleType.GameData)).GetTechnologyData(research.technology_type).classification == classification))
            {
                instance.GetComponent<Image>().color = new Color(0.0f, 192.0f / 255.0f, 1.0f);
            }
        }

        //Food Research
        x_pos += 60.0f;
        GameObject food_research_instance = Instantiate(mid_button_prefab, button_parent);
        food_research_instance.GetComponent<MidButtonScript>().Init(this, "料理開発", -1);
        food_research_instance.GetComponent<RectTransform>().localPosition = new Vector3(x_pos, 0);

        instances.Add(food_research_instance);

        if (research.research_type == ResearchType.Food)
        {
            food_research_instance.GetComponent<Image>().color = new Color(0.0f, 192.0f / 255.0f, 1.0f);
        }
    }

    public void SetBank()
    {
        Clear();

        state = MidScriptState.BankOpen;
        gameObject.SetActive(true);

        float x_size = 2;
        float x_pos = x_size * -0.5f;

        GameObject borrow_instance = Instantiate(mid_button_prefab, button_parent);
        borrow_instance.GetComponent<MidButtonScript>().Init(this, Lang.Get("main_borrow"), 0);
        borrow_instance.GetComponent<RectTransform>().localPosition = new Vector3(x_pos, 0);
        x_pos += 60.0f;
        instances.Add(borrow_instance);

        GameObject payback_instance = Instantiate(mid_button_prefab, button_parent);
        payback_instance.GetComponent<MidButtonScript>().Init(this, Lang.Get("main_repay"), 1);
        payback_instance.GetComponent<RectTransform>().localPosition = new Vector3(x_pos, 0);
        x_pos += 60.0f;
        instances.Add(payback_instance);
    }

    public void SetSkill()
    {
        Clear();

        state = MidScriptState.SkillOpen;
        gameObject.SetActive(true);

        GameObject instance = Instantiate(mid_skill_button_prefab, button_parent);
        instance.GetComponent<MidSkillButtonScript>().Init();
        instance.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f);
        instances.Add(instance);
    }

    public void ChildButtonClicked(int type)
    {
        if(state == MidScriptState.BuildOpen)
        {
            top_script.SetBuild(type);
        }
        else if(state == MidScriptState.ResearchOpen && type != -1)
        {
            top_script.SetResearch(type);
        }
        else if(state == MidScriptState.ResearchOpen && type == -1)
        {
            top_script.SetFoodResearch();
        }
        else if(type == 0)
        {
            top_script.SetBorrow();
        }
        else
        {
            top_script.SetPayback();
        }
    }
}

public enum MidScriptState
{
    Close,
    BuildOpen,
    ResearchOpen,
    BankOpen,
    SkillOpen,
}

