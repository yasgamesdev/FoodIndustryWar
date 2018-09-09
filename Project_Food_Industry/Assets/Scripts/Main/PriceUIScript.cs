using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;
using System.Linq;

public class PriceUIScript : UIScript
{
    [SerializeField]
    Button toggle_button, expand_button;
    [SerializeField]
    Text product_text;
    [SerializeField]
    RectTransform toggle_arrow;
    [SerializeField]
    GameObject scroll_view, graph;
    [SerializeField]
    Transform content;
    [SerializeField]
    GameObject price_node_prefab;
    [SerializeField]
    PriceUIParticleScript particle_script;

    bool is_graph_close = false;
    bool is_expand = true;

    List<GameObject> nodes = new List<GameObject>();
    public PriceNodeScript select { get; private set; }

    protected override void Start()
    {
        base.Start();

        toggle_button.onClick.AsObservable().Subscribe(x => {
            if(is_graph_close)
            {
                is_graph_close = false;

                scroll_view.SetActive(true);
                graph.SetActive(false);
                Reload();
            }
            else
            {
                is_graph_close = true;

                scroll_view.SetActive(false);
                graph.SetActive(true);
                graph.GetComponent<PriceUIGraphScript>().Reload(select.product_id);
            }
        });

        expand_button.onClick.AsObservable().Subscribe(x => {
            if (is_expand)
            {
                is_expand = false;
                toggle_arrow.localRotation = Quaternion.Euler(0, 0, 0);

                graph.GetComponent<RectTransform>().localScale = new Vector2(0, 0);
                scroll_view.GetComponent<RectTransform>().localScale = new Vector2(0, 0);
            }
            else
            {
                is_expand = true;
                toggle_arrow.localRotation = Quaternion.Euler(0, 0, 180.0f);

                graph.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
                scroll_view.GetComponent<RectTransform>().localScale = new Vector2(1.0f, 1.0f);
            }
        });

        SetOption();

        Select(nodes[1].GetComponent<PriceNodeScript>());


        fis.GetSubject(NotificationType.EndNext).Subscribe(x => {
            if (is_graph_close)
            {
                graph.GetComponent<PriceUIGraphScript>().Reload(select.product_id);
            }
            else
            {
                Reload();
            }

            particle_script.SetStream(select.product_id);
        });

        fis.GetSubject(NotificationType.AddFood).Subscribe<Parameters>(x => {
            Food food = x.Get<Food>(0);
            GameObject instance = Instantiate(price_node_prefab, content);
            instance.GetComponent<PriceNodeScript>().Init(this, food.output_product_id, food.food_name);
            nodes.Add(instance);
        });

        fis.GetSubject(NotificationType.RemoveFood).Subscribe<Parameters>(x => {
            Food food = x.Get<Food>(0);
            if (select.product_id == food.output_product_id)
            {
                select = nodes[1].GetComponent<PriceNodeScript>();
                product_text.text = select.food_name;
                graph.GetComponent<PriceUIGraphScript>().Reload(select.product_id);
            }
            GameObject remove_instance = nodes.First(y => y.GetComponent<PriceNodeScript>().product_id == food.output_product_id);
            Destroy(remove_instance);
            nodes.Remove(remove_instance);

            if (is_graph_close)
            {
                graph.GetComponent<PriceUIGraphScript>().Reload(select.product_id);
            }
            else
            {
                Reload();
            }
        });
    }

    void SetOption()
    {
        List<int> market_product_id = fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetMarketProductID();
        foreach(int product_id in market_product_id)
        {
            string food_name = fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetFoodName(product_id);
            GameObject instance = Instantiate(price_node_prefab, content);
            instance.GetComponent<PriceNodeScript>().Init(this, product_id, food_name);
            nodes.Add(instance);
        }
    }

    void Reload()
    {
        nodes.ForEach(x => x.GetComponent<PriceNodeScript>().Reload());
    }

    public void Select(PriceNodeScript select)
    {
        this.select = select;
        product_text.text = select.food_name;

        toggle_button.onClick.Invoke();

        particle_script.SetStream(select.product_id);
    }
}
