using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.EventSystems;

public class FactoryInfoUIScript : UIScript
{
    [SerializeField]
    BuilderScript builder_script;
    [SerializeField]
    BuildingScript building_script;

    int layerMask;

    [SerializeField]
    GameObject foodfactory_info_prefab, common_agent_info_prefab, bank_info_prefab;

    GameObject info_instance;
    Pos pos;
    Vector3 display_world_pos;

    protected override void Start()
    {
        base.Start();

        layerMask = LayerMask.GetMask("Building");

        //fis.GetSubject(NotificationType.BeginNext).Subscribe(x => {
        //    Close();
        //});

        fis.GetSubject(NotificationType.EndNext).Subscribe(x => {
            if (info_instance != null)
            {
                Close();
                if (building_script.GetBuilding(pos.x, pos.y) != null)
                {
                    GameObject instance = building_script.GetBuilding(pos.x, pos.y);
                    switch (instance.GetComponent<ModelScript>().type)
                    {
                        case BuildType.Bank:
                            Bank bank = instance.GetComponent<ModelScript>().GetBank();
                            SetBankInfo(bank);
                            break;
                        case BuildType.Humans:
                            Humans humans = instance.GetComponent<ModelScript>().GetHumans();
                            SetCommonAgentInfo(humans);
                            break;
                        case BuildType.Company:
                            Company company = instance.GetComponent<ModelScript>().GetCompany();
                            SetCommonAgentInfo(company);
                            break;
                        case BuildType.FoodFactory:
                            FoodFactory food_factory = instance.GetComponent<ModelScript>().GetFoodFactory();
                            SetFoodFactoryInfo(food_factory);
                            break;
                    }
                }
            }
        });
    }

    void Update()
    {
        if (!builder_script.isOn)
        {
            if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && !EventSystem.current.IsPointerOverGameObject())
            {
                Close();

                if(Input.GetMouseButtonDown(0))
                {
                    RaycastHit hit;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
                    {
                        GameObject collider = hit.collider.gameObject;
                        display_world_pos = collider.transform.position + new Vector3(0.0f, 0.0f, 2.0f);
                        pos = collider.transform.parent.GetComponent<ModelScript>().GetPos();

                        switch (collider.transform.parent.GetComponent<ModelScript>().type)
                        {
                            case BuildType.Bank:
                                Bank bank = collider.transform.parent.GetComponent<ModelScript>().GetBank();
                                SetBankInfo(bank);

                                PlayOpenClip();

                                break;
                            case BuildType.Humans:
                                Humans humans = collider.transform.parent.GetComponent<ModelScript>().GetHumans();
                                SetCommonAgentInfo(humans);

                                PlayOpenClip();

                                break;
                            case BuildType.Company:
                                Company company = collider.transform.parent.GetComponent<ModelScript>().GetCompany();
                                SetCommonAgentInfo(company);

                                PlayOpenClip();

                                if (company.is_player)
                                {
                                    GameObject.Find("Tutorial").GetComponent<TutorialScript>().SelectPlayerCompany();
                                }
                                else
                                {
                                    GameObject.Find("Tutorial").GetComponent<TutorialScript>().SelectOtherCompany();
                                }
                                break;
                            case BuildType.FoodFactory:
                                FoodFactory food_factory = collider.transform.parent.GetComponent<ModelScript>().GetFoodFactory();
                                SetFoodFactoryInfo(food_factory);

                                PlayOpenClip();

                                GameObject.Find("Tutorial").GetComponent<TutorialScript>().SelectFactory();
                                break;
                        }
                    }
                }
            }
        }
    }

    void Close()
    {
        if(info_instance != null)
        {
            Destroy(info_instance);
            info_instance = null;
        }
    }

    void SetFoodFactoryInfo(FoodFactory factory)
    {
        info_instance = Instantiate(foodfactory_info_prefab, transform);
        info_instance.GetComponent<FoodFactoryInfoScript>().Init(factory);
    }

    void SetCommonAgentInfo(Agent agent)
    {
        info_instance = Instantiate(common_agent_info_prefab, transform);
        info_instance.GetComponent<CommonAgentInfoScript>().Init(agent);
    }

    void SetBankInfo(Agent agent)
    {
        info_instance = Instantiate(bank_info_prefab, transform);
        info_instance.GetComponent<BankInfoScript>().Init(agent);
    }

    void PlayOpenClip()
    {
        GetComponent<AudioSource>().Play();
    }
}
