using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;
using System.Linq;

public class PriceUIParticleScript : UIScript
{
    [SerializeField]
    GameObject particle_stream_prefab;

    Vector3 bank_pos, humans_pos;
    Transform stream_parent;
    Vector3 stream_end;

    List<GameObject> particles = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        foreach (Factory factory in fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetSupplyFactories((int)ProductType.None))
        {
            if (factory.GetOwner() == fis.GetModule<Bank>(ModuleType.Bank) && factory.GetLand() != null)
            {
                bank_pos = new Vector3(factory.GetLand().pos.x, 0, factory.GetLand().pos.y);
            }
            else if (factory.GetOwner() == fis.GetModule<Humans>(ModuleType.Humans) && factory.GetLand() != null)
            {
                humans_pos = new Vector3(factory.GetLand().pos.x, 0, factory.GetLand().pos.y);
            }
        }

        stream_parent = GameObject.Find("Stream").transform;
        stream_end = GameObject.Find("StreamEnd").transform.position;

        GetComponent<Toggle>().onValueChanged.AsObservable().Subscribe(x =>
        {
            SetStream(GameObject.Find("PriceUI").GetComponent<PriceUIScript>().select.product_id);
        });
    }

    void Clear()
    {
        particles.ForEach(x => x.GetComponent<ParticleStreamScript>().destroy = true);
        particles.ForEach(x => Destroy(x, 1.0f));
        particles.Clear();
    }

    public void SetStream(int product_id)
    {
        Clear();

        if (GetComponent<Toggle>().isOn)
        {
            List<IDemandFunc> demands = new List<IDemandFunc>();
            List<ISupplyFunc> supplies = new List<ISupplyFunc>();
            MarketPrice market_price = fis.GetModule<MarketPrice>(ModuleType.MarketPrice);
            double price = market_price.GetPrice(product_id);

            fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetDemandFactories(product_id).ForEach(x => demands.Add(x.GetIDemandFunc(product_id, market_price.GetPrices())));
            double demand_amount = demands.Sum(x => x.GetDemandAmount(product_id, price));
            foreach (Factory factory in fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetDemandFactories(product_id))
            {
                Vector3 start_pos = Vector3.zero;

                if (factory is HumansFactory)
                {
                    start_pos = humans_pos;
                }
                else
                {
                    if (factory.GetLand() == null)
                    {
                        start_pos = bank_pos;
                    }
                    else
                    {
                        start_pos = new Vector3(factory.GetLand().pos.x, 0, factory.GetLand().pos.y);
                    }
                }

                GameObject particle = Instantiate(particle_stream_prefab, stream_parent);
                particle.GetComponent<ParticleStreamScript>().Init(false, stream_end, start_pos + new Vector3(0.5f, 1.0f, 0.5f), Random.value, factory.GetIDemandFunc(product_id, market_price.GetPrices()).GetDemandAmount(product_id, price), demand_amount);
                particles.Add(particle);
            }

            fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetSupplyFactories(product_id).ForEach(x => supplies.Add(x.GetISupplyFunc(market_price.GetPrices())));
            double supply_amount = supplies.Sum(x => x.GetSupplyAmount(product_id, price));
            foreach (Factory factory in fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetSupplyFactories(product_id))
            {
                Vector3 start_pos = Vector3.zero;

                if (factory is HumansFactory)
                {
                    start_pos = humans_pos;
                }
                else
                {
                    if (factory.GetLand() == null)
                    {
                        start_pos = bank_pos;
                    }
                    else
                    {
                        start_pos = new Vector3(factory.GetLand().pos.x, 0, factory.GetLand().pos.y);
                    }
                }

                GameObject particle = Instantiate(particle_stream_prefab, stream_parent);
                particle.GetComponent<ParticleStreamScript>().Init(true, start_pos + new Vector3(0.5f, 1.0f, 0.5f), stream_end, Random.value, factory.GetISupplyFunc(market_price.GetPrices()).GetSupplyAmount(product_id, price), supply_amount);
                particles.Add(particle);
            }
        }
    }
}
