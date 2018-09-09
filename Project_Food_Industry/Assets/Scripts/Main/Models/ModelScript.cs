using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelScript : MonoBehaviour {

    protected Factory factory;

    public BuildType type
    {
        get
        {
            return BuildingScript.GetBuildType(factory);
        }
    }

    public void Init(Factory factory)
    {
        this.factory = factory;

        if(type == BuildType.Company)
        {
            Company company = (Company)factory.GetOwner();
            transform.Find("company/Group2").GetComponent<Renderer>().materials[1].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
        }
        else if(type == BuildType.FoodFactory)
        {
            Company company = (Company)factory.GetOwner();

            if (((FoodFactory)factory).GetFood().output_product_id >= System.Enum.GetValues(typeof(ProductType)).Length)
            {
                transform.Find("restaurant/Group3").GetComponent<Renderer>().materials[0].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
                transform.Find("restaurant/Group5").GetComponent<Renderer>().materials[0].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
                transform.Find("restaurant/Group7").GetComponent<Renderer>().materials[0].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
                transform.Find("restaurant/Group8").GetComponent<Renderer>().materials[0].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
                transform.Find("restaurant/Group9/Group10").GetComponent<Renderer>().materials[0].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
                transform.Find("restaurant/Group9/Group11").GetComponent<Renderer>().materials[0].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
                transform.Find("restaurant/Group28").GetComponent<Renderer>().materials[0].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
            }
            else
            {
                transform.Find("factory/Group3/Group4").GetComponent<Renderer>().materials[0].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
                transform.Find("factory/Group3/Group5").GetComponent<Renderer>().materials[0].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
                transform.Find("factory/Group3/Group6").GetComponent<Renderer>().materials[0].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
                transform.Find("factory/Group7").GetComponent<Renderer>().materials[1].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
                transform.Find("factory/Group8").GetComponent<Renderer>().materials[1].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
                transform.Find("factory/Group9").GetComponent<Renderer>().materials[1].color = new Color(company.color.float_r, company.color.float_g, company.color.float_b, company.color.float_a);
            }
        }
    }

    public Pos GetPos()
    {
        return factory.GetLand().pos;
    }

    public FoodFactory GetFoodFactory()
    {
        return (FoodFactory)factory;
    }

    public HumansFactory GetHumansFactory()
    {
        return (HumansFactory)factory;
    }

    public Bank GetBank()
    {
        return (Bank)factory.GetOwner();
    }

    public Humans GetHumans()
    {
        return (Humans)factory.GetOwner();
    }

    public Company GetCompany()
    {
        return (Company)factory.GetOwner();
    }
}
