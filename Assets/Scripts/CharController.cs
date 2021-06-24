using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CharController : MonoBehaviour
{
    private SpriteRenderer sprRen;
    public Sprite[] charSpr = new Sprite[4]; // 방향별 스프라이트 저장
    
    [Header("*초기 설정하기")]
    public int curDir; // 캐릭터의 현재 방향 (초기설정: 맵 구성 시 인스펙터창에서)
    public bool isAbleWalk;
    public bool isMainNext = true;
    public bool isFuncNext = true;
    public bool isLoopNext = true;
    public bool isAbleSeed = false;
    public int clrSeedCnt = 0;

    public Grid grid;
    
    private Tilemap tile; 
    private bool isFirst = true; // 맨처음에 불릴때 타일 정중앙에 놓기 위함
    private float distance; // 한칸 움직일 때 가는 거리 (cell사이즈로 계산) 
    private Vector2[] walkDir = new Vector2[4]; // 이동 방향 저장
    private GameObject curSeed;
    private BtnPanelManager panelManager;
    private GameManager gameManager;
    private StageInfo stageInfo;


    // Start is called before the first frame update
    void Start()
    {
        walkDir[0] = new Vector2(-2, - 1);
        walkDir[1] = new Vector2(-2, 1);
        walkDir[2] = new Vector2(2, 1);
        walkDir[3] = new Vector2(2, -1);

        sprRen = gameObject.GetComponent<SpriteRenderer>();
        sprRen.sprite = charSpr[curDir];
        //sprRen.sprite = charSpr[0];
        distance = Mathf.Sqrt(Mathf.Pow(grid.cellSize.x * grid.transform.lossyScale.x / 4, 2) + Mathf.Pow(grid.cellSize.y * grid.transform.lossyScale.y / 4, 2));
        //print(distance);
        //int i = 0;
        panelManager = GameObject.FindGameObjectWithTag("btnPanel").GetComponent<BtnPanelManager>();
        gameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        stageInfo = GameObject.FindGameObjectWithTag("StageManager").GetComponent<StageInfo>();

    }

    Ray2D ray;
    RaycastHit2D hit;
    int j = 0; 
    // Update is called once per frame
    void Update()
    {
        

        if (isFirst)
        {
            float rayLen = 0.2f;

            int groundMask = 1 << LayerMask.NameToLayer("Ground");

            ray = new Ray2D(transform.position, new Vector2(0, -1));
            hit = Physics2D.Raycast(ray.origin, ray.direction, rayLen, groundMask);

            if (hit)
            {
                tile = hit.collider.gameObject.GetComponent<Tilemap>();
                tile.RefreshAllTiles();
                
                
                int x, y;
                x = tile.WorldToCell(ray.origin).x;
                y = tile.WorldToCell(ray.origin).y;
                Vector3Int v3Int = new Vector3Int(x, y, 0);
                
                transform.position = new Vector2(tile.GetCellCenterWorld(v3Int).x, tile.GetCellCenterWorld(v3Int).y);


            }
            isFirst = false;
            j++;
        }

        
        //if (isAbleWalk)
        //{
        //    transform.position = Vector2.MoveTowards(transform.position, newPos, 0.2f);

        //}
    }

    // 방향전환 눌렸을 때
    public void ChangDirBtn(string dir, int type)
    {
        if(dir == "right")
        {
            if (curDir < 3)
            {
                sprRen.sprite = charSpr[curDir + 1]; curDir += 1;
            }
            else
            {
                sprRen.sprite = charSpr[0]; curDir = 0;
            }

                
        }
        else
        {

            if (curDir > 0)
            {
                sprRen.sprite = charSpr[curDir - 1]; curDir -= 1;
            }
            else
            {
                sprRen.sprite = charSpr[3]; curDir = 3;
            }
           
            
        }
        if (type == 0)
            Invoke("SetPossibleNext", 1f);
        else if (type == 1)
            Invoke("SetFuncNext", 1f);
        else
            Invoke("SetLoopNext", 1f);
    }

    Vector2 newPos;

    public void WalkBtn(int type)
    {
        float rayLen = distance;
        
        int rayMask = 1 << LayerMask.NameToLayer("Obstacle");

        ray = new Ray2D(transform.position, walkDir[curDir]);
        hit = Physics2D.Raycast(ray.origin, ray.direction, rayLen, rayMask);

        if (hit)
        {
            Debug.DrawRay(transform.position, walkDir[curDir] * rayLen, Color.green, rayLen);

            print("전진불가" + hit.transform.gameObject.name);
            if (type == 0)
                Invoke("SetPossibleNext", 1f);
            else if (type == 1)
                Invoke("SetFuncNext", 1f);
            else
                Invoke("SetLoopNext", 1f);

        }
        else
        {
            
            
            newPos = new Vector2(transform.position.x + walkDir[curDir].x * grid.cellSize.x * grid.transform.lossyScale.x /4 , transform.position.y + walkDir[curDir].y * grid.cellSize.y * grid.transform.lossyScale.y / 2);
            
            StartCoroutine(Move());
            if (type == 0)
                Invoke("SetPossibleNext", 1f);
            else if (type == 1)
                Invoke("SetFuncNext", 1f);
            else
                Invoke("SetLoopNext", 1f);
        }
    }

    IEnumerator Move()
    {
        float count = 0;
        Vector2 startPos = transform.position;
        while(true)
        {
            count += Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, newPos, count);

            if (count >= 1f)
            {
                transform.position = newPos;
                break;
            }
            
            yield return null;
        }
    }

    public void SeedBtn (int type)
    {
        if(isAbleSeed)
        {
            Destroy(curSeed);
            clrSeedCnt++; 

            if(stageInfo.seedAmount[stageInfo.curSelectedStage] == clrSeedCnt)
            {
                gameManager.ClearStage();
            }
            
        }
        if (type == 0)
            Invoke("SetPossibleNext", 1f);
        else if (type == 1)
            Invoke("SetFuncNext", 1f);
        else
            Invoke("SetLoopNext", 1f);
    }

    public void FuncBtn(int type)
    {
        StartCoroutine(FuncPlay(type));

        
    }

    // 테스트
    IEnumerator PressPlay()
    {
        int i = 0;

        while(true)
        {
            if (isMainNext && i >= panelManager.filledMainBtn)
                break;
            if(isMainNext)
            {


                if (panelManager.mainBtn[i].CompareTag("Walk"))
                {
                    i++;
                    WalkBtn(0);
                    isMainNext = false;
                } else if(panelManager.mainBtn[i].CompareTag("Left"))
                {
                    i++;
                    ChangDirBtn("left", 0);
                    isMainNext = false;
                } else if(panelManager.mainBtn[i].CompareTag("Right"))
                {
                    i++;
                    ChangDirBtn("right", 0);
                    isMainNext = false;
                } else if(panelManager.mainBtn[i].CompareTag("SeedBtn"))
                {
                    i++;
                    SeedBtn(0);
                    isAbleSeed = false;
                    isMainNext = false;
                } else if(panelManager.mainBtn[i].CompareTag("Func"))
                {
                    i++;
                    FuncBtn(1);
                    isMainNext = false;
                }
            }
        
            yield return null;
        }
    }

    bool isNext;
    IEnumerator FuncPlay(int type)
    {
        int i = 0;
        
        

        while (true)
        {
            
            if (i >= panelManager.filledFuncBtn)
            {
                if (type == 1)
                    Invoke("SetPossibleNext", 1f);
                else
                    Invoke("SetFuncNext", 1f);

                break;
            }

            if (type == 1)
                isNext = isFuncNext;
            else
                isNext = isLoopNext;

            if (isNext)
            {
                if (panelManager.funcBtn[i].CompareTag("Walk"))
                {
                    i++;
                    WalkBtn(type);
                    if (type == 1)
                        isFuncNext = false;
                    else
                        isLoopNext = false;
                }
                else if (panelManager.funcBtn[i].CompareTag("Left"))
                {
                    i++;
                    ChangDirBtn("left",type);
                    if (type == 1)
                        isFuncNext = false;
                    else
                        isLoopNext = false;
                }
                else if (panelManager.funcBtn[i].CompareTag("Right"))
                {
                    i++;
                    ChangDirBtn("right",type);
                    if (type == 1)
                        isFuncNext = false;
                    else
                        isLoopNext = false;
                }
                else if (panelManager.funcBtn[i].CompareTag("SeedBtn"))
                {
                    i++;
                    SeedBtn(type);
                    isAbleSeed = false;
                    if (type == 1)
                        isFuncNext = false;
                    else
                        isLoopNext = false;
                }
                else if (panelManager.funcBtn[i].CompareTag("Func"))
                {
                    i++;
                    FuncBtn(type);
                    if (type == 1)
                        isFuncNext = false;
                    else
                        isLoopNext = false;
                }
            }
            
            yield return null;
        }
    }

    public void StartPressPlay()
    {
        StartCoroutine("PressPlay");
        //print("아ㅏ아아");
    }

    void SetPossibleNext()
    {
        
         isMainNext = true;
        
    }

    void SetFuncNext ()
    {
        isFuncNext = true;
    }

    void SetLoopNext()
    {
        isLoopNext = true;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Seed"))
        {
            curSeed = collision.gameObject;
            isAbleSeed = true; 
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Seed"))
        {
            isAbleSeed = false;
        }
    }
}
