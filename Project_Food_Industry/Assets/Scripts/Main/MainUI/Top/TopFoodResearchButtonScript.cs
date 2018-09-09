using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Food_Industry;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class TopFoodResearchButtonScript : UIScript {
    [SerializeField]
    Dropdown[] dropdowns;
    [SerializeField]
    InputField food_name_inputfield;
    [SerializeField]
    Button research_button, cancel_button;
    [SerializeField]
    Text research_button_bot_text;

    Dictionary<string, ProductType> dropdown_labels;

    public void Init()
    {
        base.Start();

        dropdown_labels = new Dictionary<string, ProductType>();
        dropdown_labels.Add(Lang.Get("main_not_selected"), 0);
        foreach (ProductType type in System.Enum.GetValues(typeof(ProductType)))
        {
            if(type != ProductType.None && type != ProductType.Labor)
            {
                dropdown_labels.Add(fis.GetModule<GameData>(ModuleType.GameData).GetFoodData(type).food_name, type);
            }
        }

        foreach (Dropdown dropdown in dropdowns)
        {
            dropdown.options.Clear();
            dropdown.captionText.text = Lang.Get("main_not_selected");
            dropdown_labels.Keys.ToList().ForEach(x => dropdown.options.Add(new Dropdown.OptionData(x)));
        }

        SetResearchButton();

        Research research = fis.GetPlayerCompany().GetResearch();

        food_name_inputfield.text = research.food_requirement.food_name;

        foreach(Dropdown dropdown in dropdowns)
        {
            dropdown.onValueChanged.Invoke(0);
        }

        for(int i=0; i<research.food_requirement.mats.Count; i++)
        {
            ProductType type = research.food_requirement.mats[i];
            string food_name = fis.GetModule<GameData>(ModuleType.GameData).GetFoodData(type).food_name;
            for(int j=0; j<dropdown_labels.Keys.Count; j++)
            {
                if(food_name == dropdown_labels.Keys.ToList()[j])
                {
                    dropdowns[i].value = j;
                    break;
                }
            }
        }

        research_button.onClick.AsObservable().Subscribe(x =>
        {
            List<ProductType> mats = new List<ProductType>();

            foreach(Dropdown dropdown in dropdowns)
            {
                if(dropdown.value != 0)
                {
                    mats.Add(dropdown_labels[dropdown.options[dropdown.value].text]);
                }
            }

            if(mats.Count > 0)
            {
                mats = mats.Distinct().ToList();

                if(food_name_inputfield.text == "")
                {
                    food_name_inputfield.text = NameGenerator.GetFoodName(fis.GetModule<Rand>(ModuleType.Rand));
                }

                fis.GetPlayerCompany().GetResearch().SetResearchFood(new FoodRequirement(food_name_inputfield.text, mats));

                GameObject.Find("MainUI_Left").GetComponent<LeftScript>().SetCaution();
                GameObject.Find("MainUI_Mid").GetComponent<MidScript>().SetResearch();
                GameObject.Find("MainUI_Top").GetComponent<TopScript>().SetFoodResearch();

                GameObject.Find("MainUI_Top").GetComponent<AudioSource>().Play();

                GameObject.Find("Tutorial").GetComponent<TutorialScript>().SelectTopResearch();
            }
        });

        cancel_button.onClick.AsObservable().Subscribe(x =>
        {
            fis.GetPlayerCompany().GetResearch().Cancel();
            GameObject.Find("MainUI_Left").GetComponent<LeftScript>().SetCaution();
            GameObject.Find("MainUI_Mid").GetComponent<MidScript>().SetResearch();
            GameObject.Find("MainUI_Top").GetComponent<TopScript>().Close();
        });
    }

    public void SetResearchButton()
    {
        Research research = fis.GetPlayerCompany().GetResearch();
        if (research.research_type == ResearchType.Food)
        {
            research_button.GetComponent<Image>().color = new Color(0.0f, 192.0f / 255.0f, 1.0f);
        }
        else
        {
            research_button.GetComponent<Image>().color = Color.white;
        }

        research_button_bot_text.text = $"{fis.GetPlayerCompany().GetFoodSuccessProbability():P}";
    }
}
