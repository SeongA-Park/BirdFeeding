using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("*��ũ��Ʈ���� ����")]
    public CharController charCtrl;
    public BtnPanelManager panelManager;
    private StageInfo stageInfo;

    [Header("*�ν����Ϳ��� ����")]
    public GameObject playBtn;
    public GameObject clearPanel;


    private void Start()
    {
        stageInfo = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageInfo>();
    }

    public void PressPlay()
    {
        // ���߿� �������� ������ �ҷ��� �� �����ϴ� ������ �ű� ����
        if(charCtrl == null)
            charCtrl = GameObject.FindGameObjectWithTag("Player").GetComponent<CharController>();
        charCtrl.StartPressPlay();

    }

    public void GameRestart()
    {
        if (panelManager == null)
            panelManager = GameObject.FindGameObjectWithTag("btnPanel").GetComponent<BtnPanelManager>();
        
        int mainCnt = panelManager.filledMainBtn;
        int funcCnt = panelManager.filledFuncBtn;

        if (mainCnt != 0)
        {
            for (int i = 0; i < mainCnt; i++)
            {
                panelManager.mainBtn[i].gameObject.SetActive(false);
            }

            panelManager.filledMainBtn = 0;
        }
        if (funcCnt != 0)
        {
            for (int i = 0; i < funcCnt; i++)
            {
                panelManager.funcBtn[i].gameObject.SetActive(false);
            }
            panelManager.filledFuncBtn = 0;
        }

        stageInfo.CloseStage();
        stageInfo.OpenStage();
    }

    public void ClearStage()
    {
        // ���ο� ���������� ���� ���
        if (stageInfo.clearedStage < stageInfo.curSelectedStage)
        {
            stageInfo.clearedStage++;
            stageInfo.SetClearedStage();
        }

        // �� ���� ���������� ���� ���
        if (stageInfo.clearedStage == stageInfo.stageCnt)
        {
            clearPanel.gameObject.transform.Find("nextStage").gameObject.SetActive(false);
        }
        else
        {
            clearPanel.gameObject.transform.Find("nextStage").gameObject.SetActive(true);
        }
        
        clearPanel.SetActive(true);
    }

    // ���� �������� �Ѿ �� ���. name�� 0�̸� ���� ���������� ���°ɷ� ���� 
    public void CallSetStageName()
    {
        stageInfo.SetStageName(0);
    }

    public void CloseClearPan()
    {
        clearPanel.SetActive(false);
    }

    public void TogglePlayBtn(int type)
    {
        // type 0: ���÷��� ��ư Ȱ��ȭ, �÷��� ��ư���� ������  

        playBtn.transform.GetChild(type).gameObject.SetActive(true);
        
        if(type == 0)
        {
            playBtn.transform.GetChild(type + 1).gameObject.SetActive(false);
        } else
        {
            playBtn.transform.GetChild(type - 1).gameObject.SetActive(false);
        }
       
    }
}
