using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Food_Industry;
using System.Threading.Tasks;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;
using System.Linq;

public class RankingScript : UIScript {
    [SerializeField]
    Transform content;
    [SerializeField]
    GameObject ranking_node_prefab;
    [SerializeField]
    Button close_button;

    List<GameObject> instances = new List<GameObject>();

    public void Init()
    {
        base.Start();

        var companies = fis.GetModule<Companies>(ModuleType.Companies).GetAllCompanies();
        companies = companies.OrderByDescending(x => x.GetNetAssets()).ToList();

        for (int i = 0; i < companies.Count; i++)
        {
            GameObject instance = Instantiate(ranking_node_prefab, content);
            instance.GetComponent<RankingNodeScript>().Init(i, companies[i]);
            instances.Add(instance);
        }

        close_button.onClick.AsObservable().Subscribe(x => {
            GameObject.Find("Ranking_Button").GetComponent<Button>().onClick.Invoke();
        });
    }
}
