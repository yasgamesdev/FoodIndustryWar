using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BubblePrefabScript : MonoBehaviour
{
    [SerializeField]
    Text profit_text;
    [SerializeField]
    AudioClip[] clips;

    bool audio_isOn;

    float scale;
    Vector3 pos;
    float start_time;

    public void Init(FoodFactory factory, double average, bool audio_isOn)
    {
        this.audio_isOn = audio_isOn;

        scale = Mathf.Min((float)(factory.GetFoodFactoryCoreWithMarketPrices().GetDailyNetIncome() / average), 2.0f);

        profit_text.text = GetProfitString(factory.GetFoodFactoryCoreWithMarketPrices().GetDailyNetIncome());

        pos = new Vector3(factory.GetLand().pos.x + 0.5f, 2.0f, factory.GetLand().pos.y + 0.5f);
        start_time = Random.Range(0, 0.45f);

        StartCoroutine(SetUI());
    }

    public string GetProfitString(double profit)
    {
        string[] sizes = { "", "K", "M", "G", "T", "P" };
        double len = profit;
        int order = 0;
        while (len >= 1000 && order < sizes.Length - 1)
        {
            order++;
            len = len / 1000;
        }

        return $"{len:0.##}{sizes[order]}";
    }

    IEnumerator SetUI()
    {
        transform.GetComponent<RectTransform>().localScale = Vector3.zero;
        yield return new WaitForSeconds(start_time);
        transform.GetComponent<RectTransform>().localScale = new Vector3(scale, scale, scale);

        if(audio_isOn)
        {
            GetComponent<AudioSource>().clip = clips[Random.Range(0, clips.Length)];
            GetComponent<AudioSource>().Play();
        }

        float ypos = 0.0f;
        for(int i=0; i<25; i++)
        {
            transform.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(pos) + new Vector3(0, ypos, 0);
            ypos += 1.0f;
            yield return null;
        }

        Destroy(gameObject);
    }
}
