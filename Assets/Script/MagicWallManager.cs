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

    // 背景管理器相关设置
    public GameObject backgroundPrefab;//气泡预制体
    [Range(10f,30f)]
    public float backgroundUpDuration = 20f;//气泡上升时间
    [Range(0.1f, 10f)]
    public float backgroundUubbleInterval = 0.4f;//生成气泡时间间隔

    [Range(0, 6000)]
    public float TheDistance;   // 影响距离
    [Range(0, 20)]
    public float MoveFactor; // 移动因素

    // 面板的差值
    [SerializeField]
    float panelOffset = 0f;
    public float PanelOffset { get { return panelOffset; } set { panelOffset = value; } }

    [SerializeField,Range(1f, 100f)]
	public float MoveFactor_Panel;

    private AgentType theItemType;
    public AgentType TheItemType { set { theItemType = value; } get { return theItemType; } }


    //顶部logo
 //   Transform wallLogo;
	//public Transform WallLogo { get { return wallLogo; } }

    //layout
    public int row = 6;
    public int column = 15;

    #endregion

    #region PRIVATE PARAMETER
	[SerializeField]
	private WallStatusEnum status;
	public WallStatusEnum Status{get { return status;} set { status = value;}}

    private RectTransform operationPanel;

    // 上一次点击的时间
    float lastClickDownTime = 0f;

    // 按下与抬起的间隔
    float clickIntervalTime = 0.5f;

    // 选中的agent
    FlockAgent chooseFlockAgent = null;


    GraphicRaycaster m_Raycaster;
    PointerEventData m_PointerEventData;
    EventSystem m_EventSystem;
    #endregion

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

    }



    // Start is called before the first frame update
    void Start()
    {
        // 初始化 UI 索引
        operationPanel = GameObject.Find("OperatePanel").GetComponent<RectTransform>();

        // Raycaster - event
        m_Raycaster = GetComponent<GraphicRaycaster>();
		m_EventSystem = GetComponent<EventSystem> ();
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        // 开启场景效果
        SceneManager.Instance.Run();

        // 开启手势监听
        if (Input.GetMouseButtonDown(0))
        {
            //Debug.Log("GetMouseButtonDown");
            if (lastClickDownTime == 0f)
            {
                lastClickDownTime = Time.time;
            }
            chooseFlockAgent = getAgentsByMousePosition();
            //Debug.Log("chooseFlockAgent : " + chooseFlockAgent);
        }

        if (Input.GetMouseButton(0))
        {
            //Debug.Log("GetMouseButton");
            if ((Time.time - lastClickDownTime) > clickIntervalTime)
            {
                Debug.Log("recognize Drag");
                // 此处为拖拽事件
                if (chooseFlockAgent != null)
                {
                    DoDragItem(chooseFlockAgent);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            //Debug.Log("GetMouseButtonUp");
            if ((Time.time - lastClickDownTime) < clickIntervalTime)
            {
                Debug.Log("recognize click");
                // 此处为点击事件
                if (chooseFlockAgent != null)
                {
                    AgentManager.Instance.DoChosenItem(chooseFlockAgent);
                }
            }
            lastClickDownTime = 0f;
        }

    }

    #region 清理面板
    public bool Clear() {
        Debug.Log("进行销毁！");

        AgentManager.Instance.ClearAgents(); //清理 agent 袋
        mainPanel.anchoredPosition = Vector2.zero;  //主面板归位
        PanelOffset = 0f;   // 清理两个panel偏移量

        while (mainPanel.anchoredPosition != Vector2.zero) {
            StartCoroutine(AfterFixedUpdate());
        }
        return true;
    }
    #endregion



    #region 拖拽动作
    public void DoDragItem(FlockAgent agent) {
        if (agent.IsChoosing) {

            //Vector3 v = Camera.main.WorldToScreenPoint(Input.mousePosition);
            //Debug.Log(Input.mousePosition);
            //Input.mousePosition

            agent.GetComponent<RectTransform>().DOAnchorPos((Vector2)Input.mousePosition, Time.deltaTime);
            UpdateAgents();

        }
    }
    #endregion



    #region 根据鼠标点击位置获取 agent
    FlockAgent getAgentsByMousePosition() {
        //Set up the new Pointer Event
        m_PointerEventData = new PointerEventData(m_EventSystem);
        //Set the Pointer Event Position to that of the mouse position
        m_PointerEventData.position = Input.mousePosition;

        //Create a list of Raycast Results
        List<RaycastResult> results = new List<RaycastResult>();

        //Raycast using the Graphics Raycaster and mouse click position
        m_Raycaster.Raycast(m_PointerEventData, results);

        FlockAgent choseFlockAgent = null;

        foreach (RaycastResult result in results)
        {
            GameObject go = result.gameObject;

            // 通过layer取到agents的子图片
            if (go.layer == 10)
            {
                //Debug.Log(go.name);
                choseFlockAgent = go.transform.parent.GetComponent<FlockAgent>();
            }
            if (go.GetComponent<FlockAgent>() != null)
            {
                choseFlockAgent = go.GetComponent<FlockAgent>();
            }
        }
        return choseFlockAgent;
    }
    #endregion

    #region 获取两块画布的偏移量
    public void updateOffsetOfCanvas() {
        if (status == WallStatusEnum.Cutting)
        {
            PanelOffset = 0f;
        }
        else {
            Vector2 v1 = mainPanel.anchoredPosition;
            Vector2 v2 = operationPanel.anchoredPosition;
            float offset = (v2.x - v1.x);
            PanelOffset = offset;
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

public enum AgentType {
    env, // 企业
    activity, // 活动
    product // 产品
}

public enum AgentStatus
{
    NORMAL, // 正常
    MOVING, // 移动中
    CHOOSING    //已选择
}


public enum WallStatusEnum{
	Cutting, // 过场中
    Displaying // 显示中
}

public enum SceneType
{
    env, // 企业
    activity, // 活动
    product // 产品
}

public enum SceneStatus
{
    PREPARING, // 准备中
    STARTTING, // 启动动画中
    DISPLAYING, // 运行中
    DESTORING //  销毁中
}



