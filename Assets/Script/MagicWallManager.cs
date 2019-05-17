﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
using DG.Tweening;

//
//  入口类
//
public class MagicWallManager : Singleton<MagicWallManager>
{
    //
    //  Single
    //
    protected MagicWallManager() { }

    #region PUBLIC PARAMETER
    public RectTransform mainPanel;
	public FlockAgent agentPrefab;
    public CardAgent crossCardgent;
    public CardAgent sliceCardgent;

    public RectTransform backPanel;//前后层展开效果的后层

    // 背景管理器相关设置
    public GameObject backgroundPrefab;//气泡预制体
    [Range(10f,30f)]
    public float backgroundUpDuration = 20f;//气泡上升时间
    [Range(0.1f, 10f)]
    public float backgroundUubbleInterval = 0.4f;//生成气泡时间间隔

    [Range(0f, 2f), Header("影响距离（当为1时，表示半径）")]
    public float InfluenceFactor;   // 影响距离

    [Range(0f, 10f)]
    public float InfluenceMoveFactor;   // 影响移动距离

    // 面板的差值
    [SerializeField]
    float panelOffsetX = 0f;
    public float PanelOffsetX { get { return panelOffsetX; } set { panelOffsetX = value; } }

	[SerializeField]
	float panelOffsetY = 0f;
	public float PanelOffsetY { get { return panelOffsetY; } set { panelOffsetY = value; } }

	[SerializeField,Range(1f, 600f)]
	public float MoveFactor_Panel;

    private AgentType theItemType;
    public AgentType TheItemType { set { theItemType = value; } get { return theItemType; } }

    //顶部logo
 //   Transform wallLogo;
	//public Transform WallLogo { get { return wallLogo; } }

    //layout
    public int row = 6;
    public int Row { get { return row; } }

    int gap = 58;
    public int Gap { get { return gap; } }
    #endregion

    #region PRIVATE PARAMETER
    [SerializeField]
    private int _sceneIndex = 0; // 场景索引
    public int SceneIndex { get { return _sceneIndex; } set { _sceneIndex = value; } }

    [SerializeField]
    private IScene _currentScene; // 当前场景
    public IScene CurrentScene { get { return _currentScene; } set { _currentScene = value; } }


    private WallStatusEnum status;
	public WallStatusEnum Status{get { return status;} set { status = value;}}

    private RectTransform _operationPanel;





    #endregion

    #region Private Parameter - Data


    //public static string URL_ASSET_LOGO = "E:\\workspace\\MagicWall\\Assets\\Files\\logo\\";
    public static string URL_ASSET_LOGO = "D:\\MagicWall\\Assets\\Files\\logo\\";
    //public static string URL_ASSET_LOGO = "D:\\MagicWall\\Files\\logo\\";


    //public static string URL_ASSET = "E:\\workspace\\MagicWall\\Assets\\Files\\";
    public static string URL_ASSET = "D:\\MagicWall\\Assets\\Files\\";
    //public static string URL_ASSET = "D:\\MagicWall\\Files\\";


    //从下往上 列与底
    public Dictionary<int, float> columnAndBottoms;
    //从上往下 列与顶
    public Dictionary<int, float> columnAndTops;
    //行与右
    public Dictionary<int, float> rowAndRights;

    #endregion

    public DaoService daoService;

    // Awake - init manager of Singleton
    private void Awake()
    {
        // 初始化场景管理器
        SceneManager sceneManager = SceneManager.Instance;

        // 初始化背景管理器
        BackgroundManager backgroundManager = BackgroundManager.Instance;

        // 初始化实体管理器
        AgentManager agentManager = AgentManager.Instance;

        // 初始化效果工厂
        CutEffectFactory cutEffectFactory = CutEffectFactory.Instance;

        //  装载内容
        TheDataSource theDataSource = TheDataSource.Instance;

        //  初始化操作监听
        OperateManager _operateManager = OperateManager.Instance;

        daoService = DaoService.Instance;
    }


    // Start is called before the first frame update
    void Start()
    {
        // 初始化 UI 索引
        _operationPanel = GameObject.Find("OperatePanel").GetComponent<RectTransform>();

  //      // Raycaster - event
  //      m_Raycaster = GetComponent<GraphicRaycaster>();
		//m_EventSystem = GetComponent<EventSystem> ();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // 开启场景效果
        SceneManager.Instance.Run();

        //  启动监听
        //OperateManager.Instance.DoListening();

       
    }

    #region 清理面板
    public bool Clear() {
        AgentManager.Instance.ClearAgents(); //清理 agent 袋
        mainPanel.anchoredPosition = Vector2.zero;  //主面板归位
        PanelOffsetX = 0f;   // 清理两个panel偏移量
		PanelOffsetY = 0f;   // 清理两个panel偏移量

		while (mainPanel.anchoredPosition != Vector2.zero) {
            StartCoroutine(AfterFixedUpdate());
        }
        return true;
    }
    #endregion

    #region 拖拽动作
    public void DoDragItem(FlockAgent agent) {
        if (agent.IsChoosing) {
            agent.GetComponent<RectTransform>().DOAnchorPos((Vector2)Input.mousePosition, Time.deltaTime);
            UpdateAgents();

        }
    }
    #endregion

    #region 获取两块画布的偏移量
    public void updateOffsetOfCanvas() {
        if (status == WallStatusEnum.Cutting)
        {
            PanelOffsetX = 0f;
        }
        else {
            Vector2 v1 = mainPanel.anchoredPosition;
            Vector2 v2 = _operationPanel.anchoredPosition;
            float offset = (v2.x - v1.x);
			PanelOffsetX = offset;


			Vector2 v3 = mainPanel.anchoredPosition;
			Vector2 v4 = _operationPanel.anchoredPosition;
			float offsety = (v4.y - v3.y);
			PanelOffsetY = offsety;
		}
		//Debug.Log("OFFSET IS " + PanelOffset);
	}
    #endregion

    #region 更新 Agents 
    public void UpdateAgents() {
        AgentManager.Instance.UpdateAgents();
    }
    #endregion


    IEnumerator AfterFixedUpdate()
    {
        yield return new WaitForFixedUpdate();
    }

}

