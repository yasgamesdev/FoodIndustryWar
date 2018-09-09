using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodFactoryEducationScript : MonoBehaviour
{
    [SerializeField]
    GameObject color_dot_prefab;
    [SerializeField]
    Transform canvas, canvas2;
    [SerializeField]
    Text text2, text4, fomula_text, text6, text8;

    FoodFactory factory;
    FoodFactoryCore core;
    Prices prices;

    public void Init(FoodFactory factory)
    {
        this.factory = factory;
        core = factory.GetFoodFactoryCoreWithMarketPrices();
        prices = factory.GetFIS().GetModule<MarketPrice>(ModuleType.MarketPrice).GetPrices();

        SetProductionFunctionGlaph();

        SetRightGraph();
    }

    void SetProductionFunctionGlaph()
    {
        double labor_num = MEnv.half_output_labor_num * factory.size;
        double total_output = core.GetTotalOutput(labor_num);

        int dot_num = 50;
        float canvas_size = 150.0f;

        for (int i = 0; i < dot_num; i++)
        {
            if (i != dot_num - 1)
            {
                double output_A = core.GetTotalOutput(labor_num * i / dot_num);
                double output_B = core.GetTotalOutput(labor_num * (i + 1) / dot_num);

                if (double.IsInfinity(output_A) || double.IsInfinity(output_B))
                {
                    continue;
                }
                else
                {
                    DrawLine(new Vector3(canvas_size * (1.0f / dot_num) * i, (float)(output_A / total_output) * (canvas_size), 0), new Vector3(canvas_size * (1.0f / dot_num) * (i + 1), (float)(output_B / total_output) * (canvas_size), 0), new Color(1.0f, 0, 128.0f / 255.0f), canvas);
                }
            }
        }

        text2.text = $"{labor_num:F0}";
        text4.text = $"{total_output:F0}";

        fomula_text.text = $"Y = {factory.GetOutput():F2} * ({factory.GetNewBase():F2} ^ L - 1) / Log {factory.GetNewBase():F2}";
    }

    void DrawLine(Vector3 pointA, Vector3 pointB, Color color, Transform tar_canvas)
    {
        Vector3 differenceVector = pointB - pointA;

        GameObject dot_instance = Instantiate(color_dot_prefab, tar_canvas);
        dot_instance.GetComponent<Image>().color = color;
        dot_instance.GetComponent<RectTransform>().sizeDelta = new Vector2(differenceVector.magnitude, 4);
        dot_instance.GetComponent<RectTransform>().pivot = new Vector2(0, 0.5f);
        dot_instance.GetComponent<RectTransform>().localPosition = pointA;
        float angle = Mathf.Atan2(differenceVector.y, differenceVector.x) * Mathf.Rad2Deg;
        dot_instance.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0, 0, angle);
        //dots.Add(dot_instance);
    }

    void SetRightGraph()
    {
        double price = prices.GetPrice(factory.GetFood().output_product_id);
        double labor_price = prices.GetPrice((int)ProductType.Labor);
        double labor_num = core.GetLaborNum((int)ProductType.Labor, labor_price);

        double sales = core.GetSales();

        text6.text = $"{labor_num:F0}";
        text8.text = $"{sales:#,0}";

        int dot_num = 50;
        float canvas_size = 150.0f;

        for (int i = 0; i < dot_num; i++)
        {
            if (i != dot_num - 1)
            {
                double sales_A = price * core.GetTotalOutput(labor_num * i / dot_num);
                double sales_B = price * core.GetTotalOutput(labor_num * (i + 1) / dot_num);

                if (double.IsInfinity(sales_A) || double.IsInfinity(sales_B))
                {
                    continue;
                }
                else
                {
                    DrawLine(new Vector3(canvas_size * (1.0f / dot_num) * i, (float)(sales_A / sales) * (canvas_size), 0), new Vector3(canvas_size * (1.0f / dot_num) * (i + 1), (float)(sales_B / sales) * (canvas_size), 0), new Color(1.0f, 87.0f / 255.0f, 20.0f / 255.0f), canvas2);
                }
            }
        }

        double input_cost_per_output = core.GetInputCostPerOutput(prices, factory.GetFood().output_product_id, price);
        for (int i = 0; i < dot_num; i++)
        {
            if (i != dot_num - 1)
            {
                double cost_A = input_cost_per_output * core.GetTotalOutput(labor_num * i / dot_num);
                double cost_B = input_cost_per_output * core.GetTotalOutput(labor_num * (i + 1) / dot_num);

                if (double.IsInfinity(cost_A) || double.IsInfinity(cost_B))
                {
                    continue;
                }
                else
                {
                    DrawLine(new Vector3(canvas_size * (1.0f / dot_num) * i, (float)(cost_A / sales) * (canvas_size), 0), new Vector3(canvas_size * (1.0f / dot_num) * (i + 1), (float)(cost_B / sales) * (canvas_size), 0), new Color(1.0f, 184.0f / 255.0f, 0), canvas2);
                }
            }
        }

        for (int i = 0; i < dot_num; i++)
        {
            if (i != dot_num - 1)
            {
                double cost_A = labor_price * (labor_num * i / dot_num);
                double cost_B = labor_price * (labor_num * (i + 1) / dot_num);

                if (double.IsInfinity(cost_A) || double.IsInfinity(cost_B))
                {
                    continue;
                }
                else
                {
                    DrawLine(new Vector3(canvas_size * (1.0f / dot_num) * i, (float)(cost_A / sales) * (canvas_size), 0), new Vector3(canvas_size * (1.0f / dot_num) * (i + 1), (float)(cost_B / sales) * (canvas_size), 0), new Color(255.0f / 255.0f, 240.0f / 255.0f, 0.0f / 255.0f), canvas2);
                }
            }
        }

        for (int i = 0; i < dot_num; i++)
        {
            if (i != dot_num - 1)
            {
                double cost_A = (price * core.GetTotalOutput(labor_num * i / dot_num)) - (input_cost_per_output * core.GetTotalOutput(labor_num * i / dot_num)) - (labor_price * (labor_num * i / dot_num));
                double cost_B = (price * core.GetTotalOutput(labor_num * (i + 1) / dot_num)) - (input_cost_per_output * core.GetTotalOutput(labor_num * (i + 1) / dot_num)) - (labor_price * (labor_num * (i + 1) / dot_num));

                if (double.IsInfinity(cost_A) || double.IsInfinity(cost_B))
                {
                    continue;
                }
                else
                {
                    DrawLine(new Vector3(canvas_size * (1.0f / dot_num) * i, (float)(cost_A / sales) * (canvas_size), 0), new Vector3(canvas_size * (1.0f / dot_num) * (i + 1), (float)(cost_B / sales) * (canvas_size), 0), new Color(110.0f / 255.0f, 235.0f / 255.0f, 131.0f / 255.0f), canvas2);
                }
            }
        }
    }
}
