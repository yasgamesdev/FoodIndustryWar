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

public class LoadScript : MonoBehaviour {
    [SerializeField]
    Transform content;
    [SerializeField]
    GameObject load_node_prefab;
    [SerializeField]
    Button close_button, title_button;

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
            GameObject instance = Instantiate(load_node_prefab, content);
            instance.GetComponent<LoadNodeScript>().Init(this, overview);
        }

        close_button.onClick.AsObservable().Subscribe(x => {
            Close();
        });

        title_button.onClick.AsObservable().Subscribe(x => {
            SceneManager.LoadScene("Title");
        });

        if (SceneManager.GetActiveScene().name == "Main")
        {
            close_button.GetComponent<RectTransform>().localPosition = new Vector3(-60, -200);
            title_button.GetComponent<RectTransform>().localPosition = new Vector3(60, -200);
        }
        else if (SceneManager.GetActiveScene().name == "Title")
        {
            title_button.gameObject.SetActive(false);
        }
    }

    public void Load(SaveOverview overview)
    {
        if(SceneManager.GetActiveScene().name == "Main")
        {
            StopFIS();
        }

        FIS fis = Lib.ConvertFromBase64<FIS>(File.ReadAllText($"{Application.persistentDataPath}/s{overview.index}"));
        FISLoader.SetInstance(fis);

        SceneManager.LoadScene("Main");
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
            GameObject.Find("SaveLoadUI_Load_Button").GetComponent<Button>().onClick.Invoke();
        }
        else if(SceneManager.GetActiveScene().name == "Title")
        {
            //GameObject.Find("Continue").GetComponent<Button>().onClick.Invoke();
            GameObject.Find("Canvas").GetComponent<TitleScript>().ContinueClicked();
        }
    }
}
