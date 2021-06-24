using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnPanelManager : MonoBehaviour
{
    public int selectedPanel = 0; // 0: main 1: func
    public int filledMainBtn = 0, filledFuncBtn = 0; // 버튼 얼마나 찼는지, 0-15 (15개) 리셋시 0으로 
    public Button[] mainBtn = new Button[15], funcBtn = new Button[15];

    [Header("*초기 설정하기")]
    public Button[] toggleBtn = new Button[2];
    public Sprite[] btnImg = new Sprite[5];
    

    // Start is called before the first frame update
    void Start()
    {
        mainBtn = transform.Find("mainPanel").GetChild(0).gameObject.GetComponentsInChildren<Button>(true);
        funcBtn = transform.Find("funcPanel").GetChild(0).gameObject.GetComponentsInChildren<Button>(true);

        toggleBtn[0].image.color = new Color(0, 0, 255f, 50);
        toggleBtn[1].image.color = new Color(0, 0, 0, 0);

        //print(transform.Find("mainPanel").GetChild(0).gameObject.name);
    }
    

    public void ChangePanel(string panelName)
    {
        if(panelName == "main")
        {
            selectedPanel = 0;
            
            toggleBtn[0].image.color = new Color(0, 0, 255f, 80);
            toggleBtn[1].image.color = new Color(0, 0, 0, 0);

        }
        else
        {
            selectedPanel = 1;

            toggleBtn[1].image.color = new Color(0, 0, 255f, 80);
            toggleBtn[0].image.color = new Color(0, 0, 0, 0);
        }
    }

    public void ActiveBtn(int index)
    {
        if(selectedPanel == 0)
        {
            if(filledMainBtn < 15)
            {
                filledMainBtn++;
                mainBtn[filledMainBtn - 1].gameObject.SetActive(true);
                mainBtn[filledMainBtn - 1].GetComponent<Image>().sprite = btnImg[index];

                if(filledMainBtn > 1)
                {
                    mainBtn[filledMainBtn - 2].interactable = false;
                }
                mainBtn[filledMainBtn - 1].interactable = true;

                switch (index)
                {
                    case 0:
                        mainBtn[filledMainBtn - 1].tag = "Walk";
                        break;
                    case 1:
                        mainBtn[filledMainBtn - 1].tag = "Left";
                        break;
                    case 2:
                        mainBtn[filledMainBtn - 1].tag = "Right";
                        break;
                    case 3:
                        mainBtn[filledMainBtn - 1].tag = "SeedBtn";
                        break;
                    case 4:
                        mainBtn[filledMainBtn - 1].tag = "Func";
                        break;

                }
            }else
            {
                print("메인 못 넣어");
            }
            
        }
        else
        {
            if (filledFuncBtn < 15)
            {
                filledFuncBtn++;
                funcBtn[filledFuncBtn - 1].gameObject.SetActive(true);
                funcBtn[filledFuncBtn - 1].GetComponent<Image>().sprite = btnImg[index];

                if (filledFuncBtn > 1)
                {
                    mainBtn[filledFuncBtn - 2].interactable = false;
                }
                mainBtn[filledFuncBtn - 1].interactable = true;


                switch (index)
                {
                    case 0:
                        funcBtn[filledFuncBtn - 1].tag = "Walk";
                        break;
                    case 1:
                        funcBtn[filledFuncBtn - 1].tag = "Left";
                        break;
                    case 2:
                        funcBtn[filledFuncBtn - 1].tag = "Right";
                        break;
                    case 3:
                        funcBtn[filledFuncBtn - 1].tag = "SeedBtn";
                        break;
                    case 4:
                        funcBtn[filledFuncBtn - 1].tag = "Func";
                        break;

                }
            }
            else
            {
                print("함수 못 넣어");
            }
        }
    }

    public void DeleteBtn()
    {
        if(selectedPanel == 0)
        {
            mainBtn[filledMainBtn - 1].gameObject.SetActive(false);
            if(filledMainBtn > 1)
            {
                mainBtn[filledMainBtn - 2].interactable = true;
            }
            filledMainBtn--;
           
        }
        else
        {
            funcBtn[filledFuncBtn - 1].gameObject.SetActive(false);
            if (filledFuncBtn > 1)
            {
                funcBtn[filledFuncBtn - 2].interactable = true;
            }
            filledFuncBtn--;
        }
    }
    
 }
