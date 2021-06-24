using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageInfo : MonoBehaviour
{
    [Header("*�� ���������� �������ϱ�")]
    public int[] seedAmount;
    public int curSelectedStage; // �������� 0�� �𺧷� ����
    public int clearedStage; // ���ݱ��� Ŭ������ ��������. �÷��̾� �����۷����� ����
    public GameObject curStageObj;
    [Header("�ν����Ϳ��� ����")]
    public int stageCnt; // �������� ����. �𺧷� ����
    private bool isSaved;

    // �̱��� 
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
            // � ���������� ���� ����
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
    // ���� ������� �� ���� �������� ����� �ٽ� ����
    // ���� ó�� ������ �� �������� ������ �ҷ�����
    public void OpenStage()
    {
       
        string name = curSelectedStage.ToString();

        //print("�ƾƾƾƾƾƾƾ�");
        
        prefab = Resources.Load<GameObject>(name);

        if (prefab != null)
            curStageObj = Instantiate(prefab, prefab.transform.position, Quaternion.identity);
        else
            print("����");
    }

    // ���� ����� ��, �ٷ� ���� �������� �� ��, �������� ȭ������ ���ư� �� 
    public void CloseStage()
    {
        //print("�ƾƾƾ�");
        DestroyImmediate(curStageObj, true);
    }

}
