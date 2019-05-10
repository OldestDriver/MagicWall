﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

//
//	向左移动
//
public class GoDownDisplayBehavior : CutEffectDisplayBehavior
{
    private MagicWallManager _manager;
    private DisplayBehaviorConfig _displayBehaviorConfig;
    private bool flag = false;

    //
    //  初始化 （参数：内容类型，row）
    //
    public void Init(DisplayBehaviorConfig displayBehaviorConfig)
    {
        _displayBehaviorConfig = displayBehaviorConfig;
        flag = false;

    }

    public void Run()
	{
		_manager = MagicWallManager.Instance;

		// 面板向下移动
		float y = _manager.mainPanel.anchoredPosition.y - Time.deltaTime * _manager.MoveFactor_Panel;
		Vector2 to = new Vector2(_manager.mainPanel.anchoredPosition.x, y);
        _manager.mainPanel.DOAnchorPos(to, Time.deltaTime);

        // 调整panel的差值
        _manager.updateOffsetOfCanvas();

        // 调整所有agent
        _manager.UpdateAgents();

        UpdateAgents();
    }

    private void UpdateAgents() {
        if (_displayBehaviorConfig.SceneContentType == SceneContentType.activity){
            UpdateAgentsOfActivity();
        }
        else {
            UpdateAgentsOfEnvProduct();
        }
    }

    private void UpdateAgentsOfActivity() {
        float _itemWidth = 300;
        float _itemHeight = 0;
        float gap = _displayBehaviorConfig.ItemsFactory.GetSceneGap();
        if (Math.Abs(_manager.PanelOffsetY) > 0)
        {
            if (flag == false)
            {
                foreach (KeyValuePair<int, float> pair in _manager.columnAndTops)
                {
                    //Debug.Log(pair.Key + "+++" + pair.Value);
                    float y = pair.Value;
                    for (int i = 0; i < 7; i++)
                    {
                        float ori_x = pair.Key * (_itemWidth + gap) + _itemWidth / 2 + gap;
                        float ori_y = y;

                        Enterprise env = _manager.daoService.GetEnterprise();
                        //宽固定
                        _itemHeight = _itemWidth / env.TextureLogo.width * env.TextureLogo.height;
                        ori_y = ori_y + _itemHeight / 2 + gap;

                        // 生成 agent
                        FlockAgent go = _displayBehaviorConfig.ItemsFactory.Generate(ori_x, ori_y, ori_x, ori_y, pair.Key, i, _itemWidth, _itemHeight, env);
                        y = y + go.Height + gap;
                        //Debug.Log(go.name + " i : " + i + " y : " + y + "gap : " + gap + " go.Height : " + go.Height);
                    }
                    y = pair.Value;
                }
                flag = true;
            }
        }
    }

    private void UpdateAgentsOfEnvProduct()
    {
        int h = (int)_manager.mainPanel.rect.height;
        int w = (int)_manager.mainPanel.rect.width;

        int offsetUnit = Mathf.CeilToInt((h * 1.0f) / 3);
        int page = _displayBehaviorConfig.Page;

        if (h + _manager.PanelOffsetY - (offsetUnit * page) > 0)
        {
            float i = h + _manager.PanelOffsetY - (offsetUnit * page);

            // 需要获得当前的 column
            int rows_offsets = Mathf.CeilToInt(_displayBehaviorConfig.Row * 1.0f / 3);
            int x = page * rows_offsets + 1;
            int cols = _displayBehaviorConfig.Column;

            for (int y = 1; y <= cols; y++)
            {
                for (int z = x; z < (x + rows_offsets); z++)
                {
                    FlockAgent agent = CreateItem(_displayBehaviorConfig.ItemsFactory, z, y); // 创建新的
                    _displayBehaviorConfig.AddFlockAgentToAgentsOfPages(page, agent); // 加入list
                }
            }
            _displayBehaviorConfig.Page += 1;

            if ((_displayBehaviorConfig.Page - 5) > 0) {
                AgentManager.Instance.ClearAgentsByList(_displayBehaviorConfig.AgentsOfPages[_displayBehaviorConfig.Page - 5]); // 清理最下侧
            }
        }
        else
        {

        }
    }

    private FlockAgent CreateItem(ItemsFactory factory, int row, int column)
    {
        row = row - 1;
        column = column - 1;

        Vector2 vector2 = factory.GetOriginPosition(row, column);

        float x = vector2.x;
        float y = vector2.y;

        return factory.Generate(x, y, x, y, row, column, factory.GetItemWidth(), factory.GetItemHeight(), DaoService.Instance.GetEnterprise());

    }


}
