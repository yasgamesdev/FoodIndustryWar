using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlanScript : UIScript {

    [SerializeField]
    GameObject mock_prefab;

    Dictionary<Plan, GameObject> plans;

    protected override void Start()
    {
        base.Start();

        plans = new Dictionary<Plan, GameObject>();
        foreach(Plan plan in fis.GetPlans())
        {
            GameObject instance = Instantiate(mock_prefab, transform);
            instance.transform.position = new Vector3(plan.GetLand().pos.x, 0, plan.GetLand().pos.y) + new Vector3(0.5f, 0, 0.5f);
            plans.Add(plan, instance);
        }

        Subscribes();
    }

    void Subscribes()
    {
        fis.GetSubject(NotificationType.AddPlan).Subscribe(x => {
            Plan plan = x.Get<Plan>(0);

            GameObject instance = Instantiate(mock_prefab, transform);
            instance.transform.position = new Vector3(plan.GetLand().pos.x, 0, plan.GetLand().pos.y) + new Vector3(0.5f, 0, 0.5f);
            plans.Add(plan, instance);
        });

        fis.GetSubject(NotificationType.RemovePlan).Subscribe(x => {
            Plan plan = x.Get<Plan>(0);

            Destroy(plans[plan]);
            plans.Remove(plan);
        });
    }
}
