using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageInfo : MonoBehaviour
{
    [Header("*각 스테이지에 따라설정하기")]
    public int[] seedAmount;
    public int curSelectedStage; // 스테이지 0은 디벨롭 전용
    public int clearedStage; // 지금까지 클리어한 스테이지. 플레이어 프리퍼런스로 저장
    public GameObject curStageObj;
    [Header("인스펙터에서 설정")]
    public int stageCnt; // 스테이지 갯수. 디벨롭 제외
    private bool isSaved;

    // 싱글톤 
    private static StageInfo instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if(instance != this)
        {

            Destroy(instance.gameObject);
            instance = this;
            DontDestroyOnLoad(gameObject);

        }

        PlayerPrefs.DeleteAll();

        isSaved = PlayerPrefs.HasKey("Cleared_stage");

        if (isSaved)
        {
            clearedStage = PlayerPrefs.GetInt("Cleared_stage");

        }
        else
        {
            // 어떤 스테이지도 깨지 못함
            clearedStage = 0;
        }
    }

    public void SetClearedStage()
    {
        PlayerPrefs.SetInt("Cleared_stage", clearedStage);
    }
    
    public void SceneLoad(string type)
    {
        SceneManager.LoadScene(type);

        if (type == "GameScene")
        {
            Invoke("OpenStage",0.5f);
        }
    }

    public void SetStageName(int name)
    {
        if (name == 0)
        {
            curSelectedStage += 1;
        }
        else
        {
            curSelectedStage = name;
        }
    }

    GameObject prefab;
    // 게임 재시작할 때 현재 스테이지 지우고 다시 열기
    // 게임 처음 시작할 때 스테이지 프리펩 불러오기
    public void OpenStage()
    {
       
        string name = curSelectedStage.ToString();

        //print("아아아아아아아아");
        
        prefab = Resources.Load<GameObject>(name);

        if (prefab != null)
            curStageObj = Instantiate(prefab, prefab.transform.position, Quaternion.identity);
        else
            print("없음");
    }

    // 게임 재시작 시, 바로 다음 스테이지 갈 시, 스테이지 화면으로 돌아갈 시 
    public void CloseStage()
    {
        //print("아아아아");
        DestroyImmediate(curStageObj, true);
    }

}
