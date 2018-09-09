using Food_Industry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class LogUIScript : UIScript
{
    [SerializeField]
    Text fade_text, fix_text;
    

    Queue<string> queue = new Queue<string>();
    float timer;

    protected override void Start()
    {
        base.Start();

        timer = 0.0f;

        fis.GetSubject(NotificationType.AddTechnology).Subscribe<Parameters>(x => {
            Technology technology = x.Get<Technology>(0);
            Enqueue($"第{technology.generation + 1}世代の{Lib.GetTechnologyName(technology.tec_type)}の技術が開発されました");
        });

        fis.GetSubject(NotificationType.AddFood).Subscribe<Parameters>(x => {
            Food food = x.Get<Food>(0);
            Enqueue($"料理「{food.food_name}」が生まれました");
        });

        fis.GetSubject(NotificationType.RemoveFood).Subscribe<Parameters>(x => {
            Food food = x.Get<Food>(0);
            Enqueue($"料理「{food.food_name}」と、それを提供するレストランがなくなりました");
        });

        fis.GetSubject(NotificationType.GameOver).Subscribe<Parameters>(x => {
            fix_text.text = "GameOver";
        });
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if(timer <= 0.0f)
        {
            if(queue.Count > 0)
            {
                fade_text.text = queue.Dequeue();
                timer = 5.0f;
            }
            else
            {
                fade_text.color = new Color(1.0f, 1.0f, 1.0f, 0.0f);
            }
        }
        else
        {
            if(0.0f < timer && timer <= 0.5f)
            {
                fade_text.color = new Color(1.0f, 1.0f, 1.0f, timer / 0.5f);
            }
            else
            {
                fade_text.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
            }
        }
    }

    public void Enqueue(string text)
    {
        queue.Enqueue(text);
    }
}
