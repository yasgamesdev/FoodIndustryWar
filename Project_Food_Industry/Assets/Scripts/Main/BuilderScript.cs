using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;

public class BuilderScript : UIScript
{
    [SerializeField]
    MapScript map_script;
    [SerializeField]
    GameObject mock_prefab, build_construct_cost_prefab;
    GameObject mock_instance, build_construct_cost_instance;

    public bool isOn { get; private set; }

    Food food;
    int layerMask;

    protected override void Start()
    {
        base.Start();

        isOn = false;
        layerMask = LayerMask.GetMask("Map");

        mock_instance = Instantiate(mock_prefab, transform);
        build_construct_cost_instance = Instantiate(build_construct_cost_prefab, GameObject.Find("Canvas").transform);

        fis.GetSubject(NotificationType.RemoveFood).Subscribe(x => {
            Food food = x.Get<Food>(0);
            if(this.food == food)
            {
                Cancel();
            }
        });
    }

    public void SetFood(Food food)
    {
        this.food = food;
        isOn = true;
    }

    public void Cancel()
    {
        isOn = false;
        food = null;
    }

    void Update()
    {
        mock_instance.SetActive(false);
        build_construct_cost_instance.SetActive(false);

        if(isOn)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Cancel();
            }
            else if (!UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit, 100.0f, layerMask))
                {
                    Land land = map_script.GetLand((int)hit.point.x, (int)hit.point.z);
                    if (fis.IsPossibleConstruct(land.pos))
                    {
                        mock_instance.SetActive(true);
                        mock_instance.transform.position = new Vector3((int)hit.point.x, 0, (int)hit.point.z) + new Vector3(0.5f, 0, 0.5f);

                        build_construct_cost_instance.SetActive(true);
                        build_construct_cost_instance.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(mock_instance.transform.position + new Vector3(2.0f, 0.0f, 0));
                        if (fis.GetPlayerCompany().GetM1() >= fis.GetTotalConstructCost(food, land.pos, fis.GetPlayerCompany()))
                        {
                            build_construct_cost_instance.GetComponentInChildren<Text>().text = $"{Lang.Get("main_construction_cost")}\n{fis.GetTotalConstructCost(food, land.pos, fis.GetPlayerCompany()):#,0}";
                        }
                        else
                        {
                            build_construct_cost_instance.GetComponentInChildren<Text>().text = $"{Lang.Get("main_construction_cost")}\n<color=red>{fis.GetTotalConstructCost(food, land.pos, fis.GetPlayerCompany()):#,0</color>}";
                        }

                        if (fis.GetPlayerCompany().GetM1() >= fis.GetTotalConstructCost(food, land.pos, fis.GetPlayerCompany()) && Input.GetMouseButtonDown(0))
                        {
                            fis.Plan(land.pos, food, fis.GetPlayerCompany());

                            Cancel();

                            GetComponent<AudioSource>().Play();

                            GameObject.Find("Tutorial").GetComponent<TutorialScript>().SelectBuilder();
                        }
                    }
                }
            }
        }
    }
}
