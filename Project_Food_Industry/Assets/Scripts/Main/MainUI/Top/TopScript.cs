using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;
using System;

public class TopScript : UIScript
{
    [SerializeField]
    MidScript mid_script;
    [SerializeField]
    Transform button_parent;
    [SerializeField]
    GameObject build_prefab, research_prefab, food_research_prefab, borrow_prefab, payback_prefab;

    List<GameObject> instances = new List<GameObject>();
    int type;

    protected override void Start()
    {
        base.Start();

        fis.GetSubject(NotificationType.GameOver).Subscribe(x => {
            Close();
        });

        fis.GetSubject(NotificationType.EndNext).Subscribe(x => {
            if(gameObject.activeSelf == true && mid_script.state == MidScriptState.BuildOpen)
            {
                SetBuild(this.type);
            }
            else if (gameObject.activeSelf == true && mid_script.state == MidScriptState.ResearchOpen && type != -1)
            {
                SetResearch(this.type);
            }
            else if (gameObject.activeSelf == true && mid_script.state == MidScriptState.ResearchOpen && type == -1)
            {
                instances[0].GetComponent<TopFoodResearchButtonScript>().SetResearchButton();
            }
            else if(gameObject.activeSelf == true && mid_script.state == MidScriptState.BankOpen && type == 0)
            {
                instances[0].GetComponent<TopBorrowButtonScript>().SetUI();
            }
            else if (gameObject.activeSelf == true && mid_script.state == MidScriptState.BankOpen && type == 1)
            {
                SetPayback();
            }
        });

        //fis.GetSubject(NotificationType.RemoveFood).Subscribe(x => {
        //    if (gameObject.activeSelf == true && mid_script.state == MidScriptState.ResearchOpen && type == -1)
        //    {
        //        Close();
        //    }
        //});

        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    void Clear()
    {
        instances.ForEach(x => Destroy(x));
        instances.Clear();
    }

    public void SetBuild(int type)
    {
        Clear();
        gameObject.SetActive(true);

        this.type = type;
        FoodClassification classification = (FoodClassification)type;

        float x_size = 80.0f * (fis.GetAllFood().Where(x => x.classification == classification).Count() - 1);
        float x_pos = x_size * -0.5f;

        foreach (Food food in fis.GetAllFood().Where(x => x.classification == classification))
        {
            GameObject instance = Instantiate(build_prefab, button_parent);
            instance.GetComponent<TopBuildButtonScript>().Init(food);
            instance.GetComponent<RectTransform>().localPosition = new Vector3(x_pos, 0);
            x_pos += 80.0f;

            instances.Add(instance);
        }
    }
    public void SetResearch()
    {
        SetResearch(this.type);
    }

    public void SetResearch(int type)
    {
        Clear();
        gameObject.SetActive(true);

        this.type = type;
        TechnologyClassification classification = (TechnologyClassification)type;

        float x_size = 80.0f * (fis.GetTechnologyTypes(classification).Count - 1);
        float x_pos = x_size * -0.5f;

        foreach (TechnologyType tec_type in fis.GetTechnologyTypes(classification))
        {
            GameObject instance = Instantiate(research_prefab, button_parent);
            instance.GetComponent<TopResearchButtonScript>().Init(tec_type);
            instance.GetComponent<RectTransform>().localPosition = new Vector3(x_pos, 0);
            x_pos += 80.0f;

            instances.Add(instance);
        }
    }

    public void SetFoodResearch()
    {
        Clear();
        gameObject.SetActive(true);

        this.type = -1;

        GameObject instance = Instantiate(food_research_prefab, button_parent);
        instance.GetComponent<TopFoodResearchButtonScript>().Init();
        instance.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f);
        instances.Add(instance);
    }

    public void SetBorrow()
    {
        Clear();
        gameObject.SetActive(true);

        this.type = 0;

        GameObject instance = Instantiate(borrow_prefab, button_parent);
        instance.GetComponent<TopBorrowButtonScript>().Init();
        instance.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f);
        instances.Add(instance);
    }

    public void SetPayback()
    {
        Clear();
        gameObject.SetActive(true);

        this.type = 1;

        GameObject instance = Instantiate(payback_prefab, button_parent);
        instance.GetComponent<TopPaybackButtonScript>().Init();
        instance.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f);
        instances.Add(instance);
    }
}