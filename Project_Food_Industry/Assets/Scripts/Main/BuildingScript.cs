using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BuildingScript : UIScript
{
    [SerializeField]
    GameObject humans_factory_model, food_factory_model, restaurant_model, bank_model, humans_model, company_model;

    Land[,] lands;
    GameObject[,] builds;

    int layerMask;

    protected override void Start()
    {
        base.Start();

        lands = fis.GetLands();

        builds = new GameObject[lands.GetLength(0), lands.GetLength(1)];
        SetAllBuilding();

        layerMask = LayerMask.GetMask("Building");

        Subscribes();
    }

    void Subscribes()
    {
        fis.GetSubject(NotificationType.ChangeLand).Subscribe(x => {
            Land land = x.Get<Land>(0);
            if (builds[land.pos.x, land.pos.y] != null)
            {
                Destroy(builds[land.pos.x, land.pos.y]);
                builds[land.pos.x, land.pos.y] = null;
            }
            SetBuilding(land.pos.x, land.pos.y);
        });
    }

    public GameObject GetBuilding(int x, int y)
    {
        return builds[x, y];
    }

    public static BuildType GetBuildType(Factory factory)
    {
        if (factory is HumansFactory)
        {
            return BuildType.HumansFactory;
        }
        else
        {
            FoodFactory food_factory = (FoodFactory)factory;
            if (food_factory.GetFood().output_product_id != (int)ProductType.None)
            {
                return BuildType.FoodFactory;
            }
            else
            {
                if (food_factory.GetOwner() == food_factory.GetFIS().GetModule(ModuleType.Bank))
                {
                    return BuildType.Bank;
                }
                else if (food_factory.GetOwner() == food_factory.GetFIS().GetModule(ModuleType.Humans))
                {
                    return BuildType.Humans;
                }
                else
                {
                    return BuildType.Company;
                }
            }
        }
    }

    void SetBuilding(int x, int y)
    {
        if (lands[x, y].GetFactory() != null)
        {
            Factory factory = lands[x, y].GetFactory();

            GameObject instance = null;
            switch (GetBuildType(factory))
            {
                case BuildType.HumansFactory:
                    instance = Instantiate(humans_factory_model, transform);
                    break;
                case BuildType.FoodFactory:
                    if(((FoodFactory)factory).GetFood().output_product_id >= System.Enum.GetValues(typeof(ProductType)).Length)
                    {
                        instance = Instantiate(restaurant_model, transform);
                    }
                    else
                    {
                        instance = Instantiate(food_factory_model, transform);
                    }
                    break;
                case BuildType.Bank:
                    instance = Instantiate(bank_model, transform);
                    break;
                case BuildType.Humans:
                    instance = Instantiate(humans_model, transform);
                    break;
                case BuildType.Company:
                    instance = Instantiate(company_model, transform);
                    break;
            }
            instance.GetComponent<ModelScript>().Init(factory);
            instance.transform.position = new Vector3(x + 0.5f, 0.0f, y + 0.5f);
            builds[x, y] = instance;
        }
    }

    void SetAllBuilding()
    {
        for(int y=0; y<lands.GetLength(1); y++)
        {
            for (int x = 0; x < lands.GetLength(0); x++)
            {
                SetBuilding(x, y);
            }
        }
    }
}

public enum BuildType
{
    HumansFactory,
    FoodFactory,
    Bank,
    Humans,
    Company,
}