using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ForSelectStage : MonoBehaviour
{
    public Button[] stageBtn = new Button[15];
    private StageInfo stageInfo;

    void Start()
    {
        stageInfo = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageInfo>();
        stageBtn = this.gameObject.GetComponentsInChildren<Button>(true);
        SetDIsAble();
    }

    private void SetDIsAble()
    {

        if (stageInfo.clearedStage == 0)
        {
            for (int i = 1; i < 15; i++)
            {
                stageBtn[i].interactable = false;
            }

        }
        else
        {
            for (int i = stageInfo.clearedStage; i < 15; i++)
            {
                stageBtn[i].interactable = false;
            }
        }
    }
}
