using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class SaveNodeScript : MonoBehaviour {
    [SerializeField]
    Text date_text, difficulty_text, money_text;
    
    SaveScript parent;
    SaveOverview overview;

    public void Init(SaveScript parent, SaveOverview overview)
    {
        this.parent = parent;
        this.overview = overview;

        date_text.text = overview.date.ToString("yyyy/MM/dd");
        difficulty_text.text = Lib.GetDifficultyName(overview.difficulty);
        money_text.text = $"{overview.money:#,0}";

        GetComponentInChildren<Button>().onClick.AsObservable().Subscribe(x => {
            parent.OverWrite(overview);
        });
    }
}
