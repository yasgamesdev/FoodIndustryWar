using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;
using Food_Industry;
using System.Threading.Tasks;

public class CalculateScript : MonoBehaviour
{
    [SerializeField]
    Slider progress_slider;
    [SerializeField]
    Text progress_text, bot_text;
    [SerializeField]
    float calculate_time;

    FIS fis;
    float timer;

    void Start()
    {
        fis = FISLoader.GetFIS();
        fis.Calculate();

        timer = 0.0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        progress_slider.value = timer / calculate_time;
        progress_text.text = $"{timer / calculate_time:P}";

        bot_text.color = new Color(0, 0, 0, Mathf.Ceil(timer) % 2 == 1 ? Mathf.Ceil(timer) - timer : timer - Mathf.Floor(timer));

        if(timer >= calculate_time || fis.GetState() == CalculatorState.Stop)
        {
            fis.Stop();
            while(fis.GetState() != CalculatorState.Stop)
            {
                Task.Delay(1);
            }
            fis.Merge();
            SceneManager.LoadScene("Main");
        }
    }
}
