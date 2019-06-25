﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 企业工厂
/// </summary>
public class EnvFactory : Singleton<EnvFactory>, ItemsFactory
{
    private float _gap = 58;
    private float _itemWidth;   // Item Width
    private float _itemHeight;  // Item Height
    private int _column;    // 列数

    // Generate Panel
    private RectTransform _operationPanel;

    // Main Panel
    private RectTransform _mainPanel;

    // Manager
    private MagicWallManager _manager;

    // Agent Manager
    private AgentManager _agentManager;

    // Data
    private DaoService _daoService;

    void Awake() {
    }

    public void Init(MagicWallManager manager)
    {
        _manager = manager;
        _operationPanel = _manager.OperationPanel;
        _agentManager = _manager.agentManager;
        _daoService = _manager.daoService;

        int _row = _manager.Row;

        _gap = _gap * manager.displayFactor;

        int h = (int)_manager.mainPanel.rect.height;    // 高度
        int w = (int)_manager.mainPanel.rect.width; //宽度
        _itemHeight = (h - _gap * 7) / _row;
        _itemWidth = _itemHeight;

        _column = Mathf.CeilToInt(w / (_itemWidth + _gap));

    }



    public EnvFactory() {


    }

    #region 生成浮动块
    //
    //  生成env的浮动组件
    //  - 长宽相等
    //  - 生成在动画前
    //  - 生成在动画后
    //
    public FlockAgent Generate(float gen_x, float gen_y, float ori_x, float ori_y, int row, int column, float width, float height, BaseData data, Transform parent)
    {
        width = (int)width;
        height = (int)height;
        Enterprise env = data as Enterprise;

        if (gen_x == ori_x)
        {
            //print(row+"---"+column);
        }
        //  创建 Agent
        FlockAgent newAgent = _agentManager.GetFlockAgent();
        //  命名
        newAgent.name = "Agent(" + (row + 1) + "," + (column + 1) + ")";

        //  获取rect引用
        RectTransform rectTransform = newAgent.GetComponent<RectTransform>();

        //  定出生位置
        Vector2 postion = new Vector2(gen_x, gen_y);
        rectTransform.anchoredPosition = postion;

        //  定面板位置
        Vector2 ori_position = new Vector2(ori_x, ori_y);
        newAgent.GenVector2 = postion;

        // 调整agent的长与宽
        Vector2 sizeDelta = new Vector2(width, height);
        rectTransform.sizeDelta = sizeDelta;

        //  初始化内容
        newAgent.Initialize(_manager, ori_position, postion, row + 1, column + 1,
            width, height, env.Ent_id, env.Logo, env.IsCustom, MWTypeEnum.Enterprise);

        //  添加到组件袋
        _agentManager.AddItem(newAgent);

        return newAgent;
    }
    #endregion

    #region 生成十字卡片
    public CardAgent GenerateCardAgent(Vector3 genPos, FlockAgent flockAgent, int dataId, bool isActive)
    {

        //  创建 Agent
        CrossCardAgent crossCardAgent = _agentManager.GetCrossCardAgent();
            
        //  获取rect引用
        RectTransform rectTransform = crossCardAgent.GetComponent<RectTransform>();

        //  定义缩放
        Vector3 scaleVector3 = new Vector3(0.2f, 0.2f, 0.2f);

        // 初始化数据
        crossCardAgent.InitCardData(_manager, dataId, MWTypeEnum.Enterprise, genPos, scaleVector3, flockAgent);

        // 添加到effect agent
        _agentManager.AddEffectItem(crossCardAgent);

        // 初始化 CrossAgent 数据
        crossCardAgent.InitCrossCardAgent();

        crossCardAgent.gameObject.SetActive(isActive);

        return crossCardAgent;
    }
    #endregion


    public Vector2 GetOriginPosition(int row, int column)
    {
        //float x = j * (itemWidth + gap) + itemWidth / 2 + gap;
        //float y = i * (itemHeight + gap) + itemHeight / 2 + gap;

        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        float itemHeight = (h - _gap * 7) / _manager.Row;
        float itemWidth = itemHeight;


        float x = column * (itemWidth + _gap) + itemWidth / 2 + _gap;
        float y = row * (itemHeight + _gap) + itemHeight / 2 + _gap;

        _itemWidth = itemWidth;
        _itemHeight = itemHeight;

        return new Vector2(x, y);
    }

    public Vector2 GoUpGetOriginPosition(int row, int column)
    {
        //float x = j * (itemWidth + gap) + itemWidth / 2 + gap;
        //float y = i * (itemHeight + gap) + itemHeight / 2 + gap;

        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        float itemHeight = (h - _gap * 7) / _manager.Row;
        float itemWidth = itemHeight;


        float x = column * (itemWidth + _gap) + itemWidth / 2 + _gap;
        float y = h - (row * (itemHeight + _gap) + itemHeight / 2 + _gap);

        _itemWidth = itemWidth;
        _itemHeight = itemHeight;

        return new Vector2(x, y);
    }

    public float GetItemWidth()
    {
        return _itemWidth;
    }

    public float GetItemHeight()
    {
        return _itemHeight;
    }

    public int GetSceneColumn()
    {
        return _column;
    }

    public float GetSceneGap()
    {
        return _gap;
    }

}
