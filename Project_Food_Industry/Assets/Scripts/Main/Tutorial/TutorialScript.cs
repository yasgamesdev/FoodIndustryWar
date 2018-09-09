using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Food_Industry;
using UnityEngine.UI;
using UniRx;
using System.Linq;

public class TutorialScript : UIScript
{
    [SerializeField]
    Button build_button, research_button, bank_button, skill_button, next_button, ranking_button, save_button, load_button;
    [SerializeField]
    Toggle auto_toggle;
    [SerializeField]
    Text tutorial_text;

    [SerializeField]
    GameObject select_prefab, arrow_prefab, right_arrow_prefab, left_arrow_prefab;
    [SerializeField]
    GameObject panel_0_prefab, panel_1_prefab, panel_2_prefab;

    List<GameObject> instances = new List<GameObject>();

    protected override void Start()
    {
        base.Start();

        if(fis.GetCurrentDateTime() == MEnv.init_date && fis.GetPlayerCompany().GetM1() == MEnv.init_company_money)
        {
            SetInteractable(false);

            GameObject instance = Instantiate(select_prefab, transform);
            instance.GetComponent<TutorialSelectScript>().Init(this);
        }
    }

    IEnumerator TutorialRoutine()
    {
        yield return new WaitForSeconds(2.0f);

        yield return ShowCompanies();

        yield return ConstructFactory();

        yield return SetResearch();

        yield return ExplainOther();

        SetInteractable(true);
    }

    void Clear()
    {
        instances.ForEach(x => Destroy(x));
        instances.Clear();

        tutorial_text.text = "";
    }

    void SetInteractable(bool value)
    {
        build_button.interactable = value;
        research_button.interactable = value;
        bank_button.interactable = value;
        skill_button.interactable = value;
        next_button.interactable = value;
        ranking_button.interactable = value;
        auto_toggle.interactable = value;
        save_button.interactable = value;
        load_button.interactable = value;
    }

    public void SelectOk()
    {
        StartCoroutine(TutorialRoutine());
    }

    public void SelectSkip()
    {
        SetInteractable(true);
    }

    IEnumerator ShowCompanies()
    {
        List<Factory> company_factories = fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetSupplyFactories((int)ProductType.None).Where(x => x.GetOwner() is Company).ToList();
        FoodFactory player_factory = (FoodFactory)company_factories.First(x => ((Company)x.GetOwner()).is_player);

        GameObject player_instance = Instantiate(arrow_prefab, transform);
        player_instance.transform.position = new Vector3(player_factory.GetLand().pos.x + 0.5f, 2.5f, player_factory.GetLand().pos.y + 0.5f);
        instances.Add(player_instance);

        tutorial_text.text = "これがあなたの会社です";

        yield return new WaitForSeconds(5.0f);

        tutorial_text.text = "クリックしてみましょう";

        select_playercompany_flag = false;

        while (!select_playercompany_flag)
        {
            yield return null;
        }

        Clear();

        tutorial_text.text = "バランスシートが表示されました";

        yield return new WaitForSeconds(5.0f);

        tutorial_text.text = "バランスシートにはあなたの会社の保有するお金、工場、負債などが表示されます";

        yield return new WaitForSeconds(10.0f);

        List<FoodFactory> ai_factories = company_factories.Where(x => !((Company)x.GetOwner()).is_player).Select(x => (FoodFactory)x).ToList();
        foreach (FoodFactory ai_factory in ai_factories)
        {
            GameObject instance = Instantiate(arrow_prefab, transform);
            instance.transform.position = new Vector3(ai_factory.GetLand().pos.x + 0.5f, 2.5f, ai_factory.GetLand().pos.y + 0.5f);
            instances.Add(instance);
        }
        tutorial_text.text = "これがライバル会社です";

        yield return new WaitForSeconds(5.0f);

        tutorial_text.text = "どれかをクリックしてみましょう";

        select_othercompany_flag = ai_factories.Count > 0 ? false : true;

        while (!select_othercompany_flag)
        {
            yield return null;
        }

        Clear();

        tutorial_text.text = "他社のバランスシートが表示されました";

        yield return new WaitForSeconds(5.0f);

        tutorial_text.text = "このように各会社の財政状態は、会社の建物をクリックすることで確認ができます";

        yield return new WaitForSeconds(10.0f);

        ranking_button.interactable = true;

        tutorial_text.text = "このボタンを押してみましょう";

        GameObject right_arrow_instance = Instantiate(right_arrow_prefab, transform);
        right_arrow_instance.GetComponent<RectTransform>().position = GameObject.Find("Ranking_Button").GetComponent<RectTransform>().position - new Vector3(100.0f, 0, 0) * GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
        instances.Add(right_arrow_instance);

        while (!select_ranking)
        {
            yield return null;
        }

        Clear();

        tutorial_text.text = "各会社の純資産一覧が表示されます";

        yield return new WaitForSeconds(7.0f);

        tutorial_text.text = "ゲームの目標はライバル会社との競争に勝ち、純資産で一位になることです";

        yield return new WaitForSeconds(10.0f);

        tutorial_text.text = "このようにして現在の順位をいつでも確認することができます";

        yield return new WaitForSeconds(10.0f);

        GameObject.Find("SaveLoadUI").GetComponent<SaveLoadScript>().RankingClose();

        Clear();
    }

    bool select_playercompany_flag = false;
    public void SelectPlayerCompany()
    {
        select_playercompany_flag = true;
    }

    bool select_othercompany_flag = false;
    public void SelectOtherCompany()
    {
        select_othercompany_flag = true;
    }

    bool select_ranking = false;
    public void SelectRanking()
    {
        select_ranking = true;
    }

    IEnumerator ConstructFactory()
    {
        tutorial_text.text = "工場を建設してみましょう";

        yield return new WaitForSeconds(5.0f);

        tutorial_text.text = "このボタンを押してください";

        build_button.interactable = true;

        GameObject left_arrow_instance = Instantiate(left_arrow_prefab, transform);
        left_arrow_instance.GetComponent<RectTransform>().position = GameObject.Find("Build_Button").GetComponent<RectTransform>().position + new Vector3(128.0f, 64.0f, 0) * GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
        instances.Add(left_arrow_instance);

        while (!select_build)
        {
            yield return null;
        }

        Clear();

        tutorial_text.text = "「穀物」>「小麦」を選択し、地面をクリックして建設予定地を決定しましょう";

        while (!select_builder)
        {
            yield return null;
        }

        tutorial_text.text = "これで工場の建設が予約されました";

        yield return new WaitForSeconds(5.0f);

        tutorial_text.text = "このボタンを押して一日を進めましょう";

        next_button.interactable = true;

        GameObject right_arrow_instance = Instantiate(right_arrow_prefab, transform);
        right_arrow_instance.GetComponent<RectTransform>().position = GameObject.Find("Next_Button").GetComponent<RectTransform>().position - new Vector3(192.0f, 0, 0) * GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
        instances.Add(right_arrow_instance);

        while(fis.GetCurrentDateTime() == MEnv.init_date)
        {
            yield return null;
        }

        Clear();

        tutorial_text.text = "工場が建設されました";

        next_button.interactable = false;

        yield return new WaitForSeconds(5.0f);

        Clear();

        instances.Add(Instantiate(panel_0_prefab, transform));
        instances[0].GetComponent<TutorialPanelScript>().Init(this);

        while (!select_panel[0])
        {
            yield return null;
        }

        yield return new WaitForSeconds(5.0f);

        tutorial_text.text = "工場の詳細な情報を見てみましょう";

        yield return new WaitForSeconds(5.0f);

        tutorial_text.text = "建設したばかりの工場をクリックしましょう";

        var factories = fis.GetModule<ResourcesCaches>(ModuleType.ResourcesCaches).GetConstructedFoodFactory();
        foreach (FoodFactory factory in factories)
        {
            GameObject instance = Instantiate(arrow_prefab, transform);
            instance.transform.position = new Vector3(factory.GetLand().pos.x + 0.5f, 2.0f, factory.GetLand().pos.y + 0.5f);
            instances.Add(instance);
        }

        select_factory = false;
        while (!select_factory)
        {
            yield return null;
        }

        Clear();

        tutorial_text.text = "工場の詳細なデータが表示されました";

        yield return new WaitForSeconds(5.0f);

        Clear();

        instances.Add(Instantiate(panel_1_prefab, transform));
        instances[0].GetComponent<TutorialPanelScript>().Init(this);

        while (!select_panel[1])
        {
            yield return null;
        }

        yield return new WaitForSeconds(5.0f);

        Clear();
    }

    bool select_build = false;
    public void SelectBuild()
    {
        select_build = true;
    }

    bool select_builder = false;
    public void SelectBuilder()
    {
        select_builder = true;
    }

    bool[] select_panel = new bool[10];
    public void SelectPanel(int select_code)
    {
        select_panel[select_code] = true;
    }

    bool select_factory = false;
    public void SelectFactory()
    {
        select_factory = true;
    }

    IEnumerator SetResearch()
    {
        tutorial_text.text = "最後に研究を設定しましょう";

        yield return new WaitForSeconds(5.0f);

        tutorial_text.text = "プレイヤーはひとつだけ研究を選択することができます";

        yield return new WaitForSeconds(5.0f);

        GameObject left_arrow_instance = Instantiate(left_arrow_prefab, transform);
        left_arrow_instance.GetComponent<RectTransform>().position = GameObject.Find("Research_Button").GetComponent<RectTransform>().position + new Vector3(128.0f, 64.0f, 0) * GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;
        instances.Add(left_arrow_instance);

        tutorial_text.text = "黄色い「!」のマークは、研究が選択されていないことを示しています";

        yield return new WaitForSeconds(5.0f);

        research_button.interactable = true;

        tutorial_text.text = "ボタンを押してみてください";

        while (!select_research)
        {
            yield return null;
        }
        Clear();

        tutorial_text.text = "ジャンルを選択してから、好きな研究を選択してください";

        while (!select_top_research)
        {
            yield return null;
        }

        tutorial_text.text = "研究が選択されました";

        yield return new WaitForSeconds(5.0f);

        Clear();

        instances.Add(Instantiate(panel_2_prefab, transform));
        instances[0].GetComponent<TutorialPanelScript>().Init(this);

        while (!select_panel[2])
        {
            yield return null;
        }

        yield return new WaitForSeconds(5.0f);

        Clear();
    }

    bool select_research = false;
    public void SelectResearch()
    {
        select_research = true;
    }

    bool select_top_research = false;
    public void SelectTopResearch()
    {
        select_top_research = true;
    }

    public IEnumerator ExplainOther()
    {
        instances.Add(Instantiate(left_arrow_prefab, transform));
        instances[0].GetComponent<RectTransform>().position = GameObject.Find("Bank_Button").GetComponent<RectTransform>().position + new Vector3(128.0f, 64.0f, 0) * GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;

        bank_button.interactable = true;

        tutorial_text.text = "銀行からお金を借りることができます";

        yield return new WaitForSeconds(10.0f);

        Clear();

        tutorial_text.text = "借りたお金は二年後に利息をつけて自動的に返済されます。十分なお金を持っていない場合はゲームオーバーになります";

        yield return new WaitForSeconds(10.0f);

        Clear();

        instances.Add(Instantiate(left_arrow_prefab, transform));
        instances[0].GetComponent<RectTransform>().position = GameObject.Find("Skill_Button").GetComponent<RectTransform>().position + new Vector3(128.0f, 64.0f, 0) * GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;

        skill_button.interactable = true;

        tutorial_text.text = "お金を払ってさまざまなスキルを習得できます";

        yield return new WaitForSeconds(10.0f);

        Clear();

        instances.Add(Instantiate(right_arrow_prefab, transform));
        instances[0].GetComponent<RectTransform>().position = GameObject.Find("SaveLoadUI_Save_Button").GetComponent<RectTransform>().position + new Vector3(-100.0f, 0, 0) * GameObject.Find("Canvas").GetComponent<Canvas>().scaleFactor;

        save_button.interactable = true;
        load_button.interactable = true;

        tutorial_text.text = "ここからセーブロードができます";

        yield return new WaitForSeconds(10.0f);

        Clear();

        tutorial_text.text = "以上でチュートリアルを終わります";

        yield return new WaitForSeconds(5.0f);

        tutorial_text.text = "工場をどんどん建てていくもよし、特許料で稼いでいくもよし、純資産一位を目指してください";

        yield return new WaitForSeconds(10.0f);

        tutorial_text.text = "素早く資金を調達したい場合は、銀行からお金を借りるといいでしょう";

        yield return new WaitForSeconds(10.0f);

        Clear();
    }
}
