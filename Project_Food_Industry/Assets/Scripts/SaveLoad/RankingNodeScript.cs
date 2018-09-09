using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class RankingNodeScript : MonoBehaviour {
    [SerializeField]
    Text rank_text, name_text, money_text;

    public void Init(int rank, Company company)
    {
        rank_text.text = $"{rank + 1}";
        name_text.text = $"<color={company.color.GetColorNameForTag()}>{company.color.color_name} Company</color>";
        money_text.text = $"{company.GetNetAssets():#,0}";
    }
}
