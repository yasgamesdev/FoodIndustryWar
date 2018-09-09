using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour
{
    [SerializeField]
    Button newgame_button, continue_button, option_button, credits_button;
    [SerializeField]
    GameObject load_prefab, menu;
    [SerializeField]
    Transform pivot;

    GameObject instance;

    void Start()
    {
        newgame_button.onClick.AsObservable().Subscribe(x => {
            if(pivot.localEulerAngles.y > 0)
            {
                SceneManager.LoadScene("Select");
            }
        });

        continue_button.onClick.AsObservable().Subscribe(x => {
            if (pivot.localEulerAngles.y > 0)
            {
                ContinueClicked();
            }
        });

        option_button.onClick.AsObservable().Subscribe(x => {
            SceneManager.LoadScene("Option");
        });

        credits_button.onClick.AsObservable().Subscribe(x => {
            SceneManager.LoadScene("Credits");
        });
    }

    public void ContinueClicked()
    {
        if (instance == null)
        {
            instance = Instantiate(load_prefab, transform);
            instance.GetComponent<RectTransform>().localPosition = Vector3.zero;
            instance.GetComponent<LoadScript>().Init();

            menu.SetActive(false);
        }
        else
        {
            Destroy(instance);
            instance = null;

            menu.SetActive(true);
        }
    }

    void Update()
    {
        float max_y_rot = 135.0f;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            pivot.localEulerAngles = pivot.localEulerAngles + new Vector3(0, Time.deltaTime * max_y_rot * 2.0f, 0);
            if(pivot.localEulerAngles.y > max_y_rot)
            {
                pivot.localEulerAngles = new Vector3(pivot.localEulerAngles.x, max_y_rot, pivot.localEulerAngles.z);
            }
        }
        else
        {
            pivot.localEulerAngles = pivot.localEulerAngles - new Vector3(0, Time.deltaTime * max_y_rot * 2.0f, 0);
            if (pivot.localEulerAngles.y < 0 || max_y_rot < pivot.localEulerAngles.y)
            {
                pivot.localEulerAngles = new Vector3(pivot.localEulerAngles.x, 0, pivot.localEulerAngles.z);
            }
        }
    }
}
