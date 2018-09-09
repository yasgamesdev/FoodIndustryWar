using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Runtime.Serialization.Formatters.Binary;
using Food_Industry;
using System.Threading.Tasks;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;

public class SaveScript : MonoBehaviour {
    [SerializeField]
    Transform content;
    [SerializeField]
    GameObject save_node_prefab;
    [SerializeField]
    Button save_button, close_button;

    SaveOverviews overviews;
    string overview_path;

    public void Init()
    {
        overview_path = Application.persistentDataPath + "/overview";

        if (!File.Exists(overview_path))
        {
            overviews = new SaveOverviews();
            File.WriteAllText(overview_path, Lib.ConvertToBase64(overviews));
        }
        else
        {
            SaveOverviews temp_overviews = Lib.ConvertFromBase64<SaveOverviews>(File.ReadAllText(overview_path));
            if(temp_overviews.version == MEnv.version)
            {
                overviews = temp_overviews;
            }
            else
            {
                overviews = new SaveOverviews();
                File.WriteAllText(overview_path, Lib.ConvertToBase64(overviews));
            }
        }

        foreach(SaveOverview overview in overviews.overviews)
        {
            GameObject instance = Instantiate(save_node_prefab, content);
            instance.GetComponent<SaveNodeScript>().Init(this, overview);
        }

        save_button.onClick.AsObservable().Subscribe(x => {
            NewSave();
        });

        close_button.onClick.AsObservable().Subscribe(x => {
            Close();
        });
    }

    public void NewSave()
    {
        StopFIS();

        FIS fis = FISLoader.GetFIS();
        overviews.Add(fis.GetCurrentDateTime(), fis.difficulty, fis.GetPlayerCompany().GetM1());
        File.WriteAllText(overview_path, Lib.ConvertToBase64(overviews));

        File.WriteAllText($"{Application.persistentDataPath}/s{overviews.overviews.Count - 1}", Lib.ConvertToBase64(fis));

        Close();
    }

    public void OverWrite(SaveOverview overview)
    {
        StopFIS();

        FIS fis = FISLoader.GetFIS();
        overviews.OverWrite(overview.index, fis.GetCurrentDateTime(), fis.difficulty, fis.GetPlayerCompany().GetM1());
        File.WriteAllText(overview_path, Lib.ConvertToBase64(overviews));

        File.WriteAllText($"{Application.persistentDataPath}/s{overview.index}", Lib.ConvertToBase64(fis));

        Close();
    }

    void StopFIS()
    {
        FIS fis = FISLoader.GetFIS();
        fis.Stop();
        while(fis.GetState() != CalculatorState.Stop)
        {
            Task.Delay(1);
        }
    }

    void Close()
    {
        if(SceneManager.GetActiveScene().name == "Main")
        {
            GameObject.Find("SaveLoadUI_Save_Button").GetComponent<Button>().onClick.Invoke();
        }
    }
}
