using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CreditScript : MonoBehaviour
{
    [SerializeField]
    Button back_button;

    void Start()
    {
        back_button.onClick.AsObservable().Subscribe(x => {
            SceneManager.LoadScene("Title");
        });
    }
}
