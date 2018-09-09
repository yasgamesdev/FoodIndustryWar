using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Food_Industry;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class SaveLoadScript : UIScript {
    [SerializeField]
    AutoScript auto_script;
    [SerializeField]
    Button save_button, load_button, ranking_button;
    [SerializeField]
    GameObject save_prefab, load_prefab, ranking_prefab;

    GameObject instance, ranking_instance;
    SaveLoadState state;

    [SerializeField]
    AudioClip open_clip, close_clip;

    AudioSource audio_source;


    protected override void Start()
    {
        base.Start();

        audio_source = GetComponent<AudioSource>();

        state = SaveLoadState.Close;

        fis.GetSubject(NotificationType.GameOver).Subscribe(x => {
            Close();
        });

        fis.GetSubject(NotificationType.EndNext).Subscribe(x => {
            if(ranking_instance != null)
            {
                Destroy(ranking_instance);
                ranking_instance = null;

                ranking_instance = Instantiate(ranking_prefab, transform);
                ranking_instance.GetComponent<RankingScript>().Init();
                ranking_instance.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f);

                GameObject.Find("Tutorial").GetComponent<TutorialScript>().SelectRanking();
            }
        });


        save_button.onClick.AsObservable().Subscribe(x => {
            if(!fis.IsGameOver())
            {
                if (state == SaveLoadState.Close)
                {
                    SaveOpen();

                    PlayOpenClip();
                }
                else if (state == SaveLoadState.SaveOpen)
                {
                    Close();
                    auto_script.SetInteractable(true);

                    PlayCloseClip();
                }
                else
                {
                    SaveOpen();

                    PlayOpenClip();
                }
            }
        });

        load_button.onClick.AsObservable().Subscribe(x =>
        {
            if (state == SaveLoadState.Close)
            {
                LoadOpen();

                PlayOpenClip();
            }
            else if (state == SaveLoadState.LoadOpen)
            {
                Close();
                auto_script.SetInteractable(true);

                PlayCloseClip();
            }
            else
            {
                LoadOpen();

                PlayOpenClip();
            }
        });

        ranking_button.onClick.AsObservable().Subscribe(x =>
        {
            if (ranking_instance == null)
            {
                ranking_instance = Instantiate(ranking_prefab, transform);
                ranking_instance.GetComponent<RankingScript>().Init();
                ranking_instance.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f);

                GameObject.Find("Tutorial").GetComponent<TutorialScript>().SelectRanking();

                PlayOpenClip();
            }
            else
            {
                Destroy(ranking_instance);
                ranking_instance = null;

                PlayCloseClip();
            }
        });
    }

    void SaveOpen()
    {
        Close();

        instance = Instantiate(save_prefab, transform);
        instance.GetComponent<SaveScript>().Init();
        instance.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f);

        auto_script.SetInteractable(false);

        state = SaveLoadState.SaveOpen;
    }

    void LoadOpen()
    {
        Close();

        instance = Instantiate(load_prefab, transform);
        instance.GetComponent<LoadScript>().Init();
        instance.GetComponent<RectTransform>().position = new Vector3(Screen.width / 2.0f, Screen.height / 2.0f);

        auto_script.SetInteractable(false);

        state = SaveLoadState.LoadOpen;
    }

    public void Close()
    {
        if(instance != null)
        {
            Destroy(instance);
            instance = null;
        }

        state = SaveLoadState.Close;
    }

    public void RankingClose()
    {
        if (ranking_instance != null)
        {
            Destroy(ranking_instance);
            ranking_instance = null;
        }
    }

    enum SaveLoadState
    {
        Close,
        SaveOpen,
        LoadOpen,
    }

    void PlayOpenClip()
    {
        audio_source.clip = open_clip;
        audio_source.Play();
    }

    void PlayCloseClip()
    {
        audio_source.clip = close_clip;
        audio_source.Play();
    }
}
