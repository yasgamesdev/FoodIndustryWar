using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Food_Industry;

public class LeftScript : UIScript
{
    [SerializeField]
    Button build_button, research_button, bank_button, skill_button;
    [SerializeField]
    GameObject caution;
    [SerializeField]
    MidScript mid_script;
    [SerializeField]
    TopScript top_script;
    [SerializeField]
    AudioClip open_clip, close_clip;

    AudioSource audio_source;

    protected override void Start()
    {
        base.Start();

        audio_source = GetComponent<AudioSource>();

        SetCaution();

        fis.GetSubject(NotificationType.GameOver).Subscribe(x => {
            build_button.interactable = false;
            research_button.interactable = false;
            bank_button.interactable = false;
            skill_button.interactable = false;
        });

        fis.GetSubject(NotificationType.EndNext).Subscribe(x => {
            SetCaution();
        });

        build_button.onClick.AsObservable().Subscribe(x => {
            if(mid_script.state != MidScriptState.BuildOpen)
            {
                mid_script.SetBuild();
                top_script.Close();

                PlayOpenClip();

                GameObject.Find("Tutorial").GetComponent<TutorialScript>().SelectBuild();
            }
            else
            {
                mid_script.Close();
                top_script.Close();

                PlayCloseClip();
            }
        });

        research_button.onClick.AsObservable().Subscribe(x => {
            if (mid_script.state != MidScriptState.ResearchOpen)
            {
                mid_script.SetResearch();
                top_script.Close();

                PlayOpenClip();

                GameObject.Find("Tutorial").GetComponent<TutorialScript>().SelectResearch();
            }
            else
            {
                mid_script.Close();
                top_script.Close();

                PlayCloseClip();
            }
        });

        bank_button.onClick.AsObservable().Subscribe(x => {
            if (mid_script.state != MidScriptState.BankOpen)
            {
                mid_script.SetBank();
                top_script.Close();

                PlayOpenClip();
            }
            else
            {
                mid_script.Close();
                top_script.Close();

                PlayCloseClip();
            }
        });

        skill_button.onClick.AsObservable().Subscribe(x => {
            if (mid_script.state != MidScriptState.SkillOpen)
            {
                mid_script.SetSkill();
                top_script.Close();

                PlayOpenClip();
            }
            else
            {
                mid_script.Close();
                top_script.Close();

                PlayCloseClip();
            }
        });
    }

    public void SetCaution()
    {
        caution.SetActive(fis.GetPlayerCompany().GetResearch().research_type == ResearchType.None);
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
