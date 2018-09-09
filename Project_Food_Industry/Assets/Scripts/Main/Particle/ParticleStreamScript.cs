using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleStreamScript : MonoBehaviour {
    [SerializeField]
    GameObject demand_prefab, supply_prefab;

    GameObject instance;

    bool is_supply;
    Vector3 start_pos;
    Vector3 end_pos;
    float timer;
    double demand;
    double total_demand;

    List<Vector3> path = new List<Vector3>();

    public bool destroy = false;

    public void Init(bool is_supply, Vector3 start_pos, Vector3 end_pos, float timer, double demand, double total_demand)
    {
        this.is_supply = is_supply;
        this.start_pos = start_pos;
        this.end_pos = end_pos;
        this.timer = timer;
        this.demand = demand;
        this.total_demand = total_demand;

        CreatePath();

        CreateInstance();
    }

    void CreatePath()
    {
        GameObject obj = new GameObject();
        obj.transform.position = start_pos;
        obj.transform.LookAt(end_pos);

        Vector3 mid_pos = (start_pos + end_pos) * 0.5f + obj.transform.up * ((end_pos - start_pos).magnitude * 0.5f);

        Destroy(obj);

        path.Add(start_pos);
        for(int i=0; i<10; i++)
        {
            path.Add(
                BezierCurve(
                    start_pos,
                    end_pos,
                    mid_pos,
                    (i + 1) * 0.1f
                )
            );
        }
    }

    void CreateInstance()
    {
        instance = Instantiate(is_supply ? supply_prefab : demand_prefab, transform);
        instance.transform.position = GetPos();
        if(total_demand == 0)
        {
            var ma = instance.GetComponent<ParticleSystem>().main;
            ma.startSize = 0.0f;
        }
        else
        {
            var ma = instance.GetComponent<ParticleSystem>().main;
            ma.startSize = Mathf.Max((float)(demand / total_demand) * 2.0f, 0.1f);
        }
    }

    Vector3 BezierCurve(Vector3 pt1, Vector3 pt2, Vector3 ctrlPt, float t)
    {
        if (t > 1.0f)
            t = 1.0f;

        Vector3 result = new Vector3();
        float cmp = 1.0f - t;
        result.x = cmp * cmp * pt1.x + 2 * cmp * t * ctrlPt.x + t * t * pt2.x;
        result.y = cmp * cmp * pt1.y + 2 * cmp * t * ctrlPt.y + t * t * pt2.y;
        result.z = cmp * cmp * pt1.z + 2 * cmp * t * ctrlPt.z + t * t * pt2.z;
        return result;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 1.0f)
        {
            timer = 0.0f;

            Destroy(instance);

            if(!destroy)
            {
                CreateInstance();
            }
        }

        if(!destroy)
        {
            instance.transform.position = GetPos();
        }
    }

    Vector3 GetPos()
    {
        int index = (int)Mathf.Floor(timer * 10.0f);
        return Vector3.Lerp(path[index], path[index + 1], timer * 10.0f - index);
    }
}
