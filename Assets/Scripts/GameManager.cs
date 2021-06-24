using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("*스크립트에서 받음")]
    public CharController charCtrl;
    public BtnPanelManager panelManager;
    private StageInfo stageInfo;

    [Header("*인스펙터에서 받음")]
    public GameObject playBtn;
    public GameObject clearPanel;


    private void Start()
    {
        stageInfo = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageInfo>();
    }

    public void PressPlay()
    {
        // 나중엔 스테이지 프리펩 불러온 후 실행하는 곳으로 옮길 예정
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
        // 새로운 스테이지를 깼을 경우
        if (stageInfo.clearedStage < stageInfo.curSelectedStage)
        {
            stageInfo.clearedStage++;
            stageInfo.SetClearedStage();
        }

        // 그 다음 스테이지가 없는 경우
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

    // 다음 스테이지 넘어갈 때 사용. name이 0이면 다음 스테이지로 가는걸로 하자 
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
        // type 0: 리플레이 버튼 활성화, 플레이 버튼에서 보내줌  

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
