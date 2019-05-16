﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Collider2D))]
public class FlockAgent : MonoBehaviour
{
    #region Data Parameter 
    private int _data_type;  //类型 0:env 1:prod 2:act
    private bool _data_iscustom; // 是定制的
    private string _data_img;    //背景图片
    private int _data_id; // id

    public int DataType { set { _data_type = value; } get { return _data_type; } }
    public string DataImg { set { _data_img = value; } get { return _data_img; } }
    public int DataId { set { _data_id = value; } get { return _data_id; } }
    public bool DataIsCustom { set { _data_iscustom = value; } get { return _data_iscustom; } }

    #endregion


    #region Component Parameter

    private int _sceneIndex;    //  场景的索引
    public int SceneIndex
    {
        set { _sceneIndex = value; }
        get { return _sceneIndex; }
    }

    int x;
    int y;

    private float delayX;
    public float DelayX { set { delayX = value; } get { return delayX; } }

    private float delayY;
    public float DelayY { set { delayY = value; } get { return delayY; } }

    private float delay;
    public float Delay { set { delay = value; } get { return delay; } }

    private float delayTime;
    public float DelayTime { set { delayTime = value; } get { return delayTime; } }

    private float duration;
    public float Duration { set { duration = value; } get { return duration; } }

    // 宽度
    [SerializeField]
    private float _width;
    public float Width { set { _width = value; } get { return _width; } }

    // 高度
    [SerializeField]
    private float _height;
    public float Height { set { _height = value; } get { return _height; } }

    // 原位
    [SerializeField]
    private Vector2 oriVector2;
    public Vector2 OriVector2 { set { oriVector2 = value; } get { return oriVector2; } }

    // 生成的位置
    private Vector2 genVector2;
    public Vector2 GenVector2 { set { genVector2 = value; } get { return genVector2; } }

    // 下个移动的位置
    [SerializeField]
    private Vector2 nextVector2;
    public Vector2 NextVector2 { set { nextVector2 = value; } get { return nextVector2; } }

    // 是否被选中
    private bool _isChoosing = false;
    public bool IsChoosing { set { _isChoosing = value; } get { return _isChoosing; } }

    // 是否被改变
    private bool isChanging = false;
    public bool IsChanging { set { isChanging = value; } get { return isChanging; } }

    // 卡片代理
    CardAgent _cardAgent;
    public CardAgent GetCardAgent{ get {return _cardAgent; }}
		
    RectTransform agentRectTransform;
    public RectTransform AgentRectTransform { get { return agentRectTransform; } }

    // 能被影响
    private bool _canEffected = true;
    public bool CanEffected { set { _canEffected = value; } get { return _canEffected; } }


    //  工厂 & 管理器
    ItemsFactory _itemsFactory;



    public Text signTextComponent;
    public Text nameTextComponent;
    public Text signTextComponent2;

    [SerializeField]
    Vector2 showTargetVector2;
    [SerializeField]
    Vector2 showRefVector2;
    [SerializeField]
    Vector2 showRefVector2WithOffset;
    [SerializeField]
    float showMoveOffset;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        agentRectTransform = GetComponent<RectTransform>();
//        nameTextComponent.text = name;
    }

    //
    //  初始化 Agent 信息
    //      originVector : 在屏幕上显示的位置
    //      genVector ： 出生的位置
    //
	public virtual void Initialize(Vector2 originVector,Vector2 genVector,int row,
        int column,float width,float height,int dataId,string dataImg,bool dataIsCustom,int dataType)
    {    
        OriVector2 = originVector;
        GenVector2 = genVector;
        x = row;
        y = column;
        _width = width;
        _height = height;
        _data_id = dataId;
        _data_img = dataImg;
        _data_iscustom = dataIsCustom;
        _data_type = dataType;

        // 定义 agent 的名字
        nameTextComponent.text = row + " - " + column;

        _sceneIndex = MagicWallManager.Instance.SceneIndex;

        // 定义工厂
        if (dataType == 0)
        {
            _itemsFactory = EnvFactory.Instance;
        }
        else if (dataType == 1)
        {
            _itemsFactory = EnvFactory.Instance;
        }
        else {
            _itemsFactory = EnvFactory.Instance;
        }



    }


    #region 更新位置
    public void updatePosition()
    {
        if (CanEffected)
        {
            UpdatePositionEffect();
        }
        else {

        }

    }



    private void UpdatePositionEffect(){
        MagicWallManager manager = MagicWallManager.Instance;

		Vector2 refVector2; // 参照的目标位置
		if(manager.Status == WallStatusEnum.Cutting){
			// 当前场景正在切换时，参考位置为目标的下个移动位置
			refVector2 = NextVector2;
		} else{
			//当前场景为正常展示时，参考位置为固定位置
			refVector2 = oriVector2;
		}
        Vector2 refVector2WithOffset = refVector2 - new Vector2(manager.PanelOffsetX, manager.PanelOffsetY); //获取带偏移量的参考位置
        showRefVector2 = refVector2;
        showRefVector2WithOffset = refVector2WithOffset;

        // 如果是被选中的，则不要移动
        if (IsChoosing){
			return;
		}

        // 此时的坐标位置可能已处于偏移状态
		RectTransform m_transform = GetComponent<RectTransform>();

        // 获取施加影响的目标物
        //  判断是否有多个影响体，如有多个，取距离最近的那个
        List<FlockAgent> transforms = AgentManager.Instance.EffectAgent;
        FlockAgent targetAgent = null;
        Vector2 targetVector2; // 目标物位置
        float distance = 1000f;

		foreach (FlockAgent item in transforms)
		{
            Vector2 effectPosition = item.GetComponent<RectTransform>().anchoredPosition;

            float newDistance = Vector2.Distance(refVector2WithOffset, effectPosition);
		    if (newDistance < distance)
		    {
		        distance = newDistance;
                targetAgent = item;
		    }
		}
        float w,h;
        if (targetAgent != null)
        {
            showTargetVector2 = targetAgent.GetComponent<RectTransform>().anchoredPosition;
            Vector3 scaleVector3 = targetAgent.GetComponent<RectTransform>().localScale;
            w = targetAgent.Width * scaleVector3.x;
            h = targetAgent.Height * scaleVector3.y;
        }
        else {
            w = 0;
            h = 0;
        }
        // 判断结束


        // 获取有效影响范围，是宽度一半以上
        float effectDistance = (w / 2) + (w / 2) * MagicWallManager.Instance.InfluenceFactor;
        // 获取差值，差值越大，则表明两个物体距离越近，MAX（offsest） = effectDistance
        float offset = effectDistance - distance;
        signTextComponent.text = "OFFSET : " + offset.ToString();
        //signTextComponent2.text = "ed : " + effectDistance.ToString();

        // 进入影响范围
        if (offset >= 0)
		{
            targetVector2 = targetAgent.GetComponent<RectTransform>().anchoredPosition;
            //m_transform.gameObject.GetComponentInChildren<Image>().color = Color.blue;
            float m_scale = -(1f / effectDistance) * offset + 1f;

            //
            //  上下移动
            //
            float move_offset = offset * ((h / 2) / effectDistance);
            showMoveOffset = move_offset;
            move_offset += h/10 * manager.InfluenceMoveFactor;

            float move_offset_x = offset * ((w / 2) / effectDistance);
            move_offset_x += w / 10 * manager.InfluenceMoveFactor;

            signTextComponent2.text = "mo: " + move_offset.ToString() + " / " + move_offset_x.ToString();

            float to_y,to_x;
            if (refVector2.y > targetVector2.y)
            {
                to_y = refVector2.y + move_offset;
            }
            else if (refVector2.y < targetVector2.y)
            {
                to_y = refVector2.y - move_offset;
            }
            else {
                to_y = refVector2.y;
            }

            if (refVector2WithOffset.x > targetVector2.x)
            {
                //m_transform.gameObject.GetComponentInChildren<Image>().color = Color.red;
                to_x = refVector2.x + move_offset_x;
            }
            else if (refVector2WithOffset.x < targetVector2.x)
            {
                //m_transform.gameObject.GetComponentInChildren<Image>().color = Color.grey;
                to_x = refVector2.x - move_offset_x;
            }
            else {
                to_x = refVector2.x;
            }

            Vector2 to = new Vector2(to_x, to_y); //目标位置

            //float k = offset / effectDistance;
            float overshootOrAmplitude = 3f;
            float k = (offset = offset / effectDistance - 1f) * offset * ((overshootOrAmplitude + 1f) * offset + overshootOrAmplitude) + 1f;

            m_transform.DOAnchorPos(Vector2.Lerp(refVector2, to, k), 0.5f);
            m_transform.DOScale(Mathf.Lerp(1f, 0.3f, k), Time.deltaTime);
            
            //
            // 尝试向外扩散
            //
            //Vector2 toV = refVector2WithOffset + (refVector2WithOffset - targetVector2).normalized * offset * manager.InfluenceMoveFactor;
            //float k = offset / effectDistance;
            //Vector2 to = Vector2.Lerp(refVector2WithOffset, toV, k);
            //m_transform.DOAnchorPos(to, Time.deltaTime);

			IsChanging = true;
		}
		else
			// 未进入影响范围
		{
//			if (IsChanging)
//			{
				Vector2 toy = new Vector2(refVector2.x, refVector2.y);
				m_transform.DOAnchorPos(toy, Time.deltaTime);
				m_transform.DOScale(1, Time.deltaTime);
				//m_transform.gameObject.GetComponentInChildren<Image> ().color = Color.green;

				IsChanging = false;
//				return toy;
//			}
		}
	}
    #endregion

    #region 点击选择

    public void DoChoose() {
        MagicWallManager _manager = MagicWallManager.Instance;

        if (!_isChoosing)
        {
            _isChoosing = true;
            float offset = _manager.PanelOffsetX;

            //  先缩小（向后退）
            RectTransform rect = GetComponent<RectTransform>();
            Vector2 positionInMainPanel = rect.anchoredPosition;

            //  移到后方、缩小、透明
            rect.DOScale(0.1f, 0.3f);
            Vector3 to = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y, 200);
            Vector3 cardGenPosition = new Vector3(rect.anchoredPosition.x - _manager.PanelOffsetX - 1f, rect.anchoredPosition.y - _manager.PanelOffsetY - 1f, 200);


            // 同时创建十字卡片，加载数据，以防因加载数据引起的卡顿
            _cardAgent = _itemsFactory.GenerateCardAgent(cardGenPosition, this,false);

            // 完成缩小与移动后创建十字卡片
            rect.DOAnchorPos3D(to, 0.3f).OnComplete(() => {
                // 使原组件消失
                gameObject.SetActive(false);

                //// 此处需要区分
                //_cardAgent = _itemsFactory.GenerateCardAgent(cardGenPosition,this);
                _cardAgent.gameObject.SetActive(true);

                Vector3 to2 = new Vector3(cardGenPosition.x, cardGenPosition.y, 0);
                _cardAgent.GetComponent<RectTransform>().DOAnchorPos3D(to2, 0.3f);

                Vector3 scaleVector3 = new Vector3(1f, 1f, 1f);
                DoScaleAgency(_cardAgent,scaleVector3, 0.5f);
            }); 



            // TODO: 当两个选择框体相近时，需要处理

        }
    }


    #endregion

    #region 恢复

    public void DoRecoverAfterChoose()
    {
        _isChoosing = false;

        MagicWallManager _manager = MagicWallManager.Instance;

        // 如果组件已不在原场景，则不进行恢复
        if (_sceneIndex != _manager.SceneIndex) {
            gameObject.SetActive(true);
            Destroy(gameObject);
            return;
        }

        //  将原组件启用
        gameObject.SetActive(true);

        // 调整位置
        RectTransform rect = GetComponent<RectTransform>();
        RectTransform cardRect = _cardAgent.GetComponent<RectTransform>();

        rect.anchoredPosition3D = new Vector3(cardRect.anchoredPosition3D.x + _manager.PanelOffsetX,
            cardRect.anchoredPosition3D.y + _manager.PanelOffsetY,
            cardRect.anchoredPosition3D.z);

        // 恢复原位
        Vector3 to = new Vector3(OriVector2.x, OriVector2.y, 0);
        rect.DOAnchorPos3D(to, 0.3f);

        // 放大至原大小
        Vector3 scaleVector3 = Vector3.one;
        DoScaleAgency(this,scaleVector3, 1f);

    }


    #endregion

    //
    //  缩放代理
    //
    public void DoScaleAgency(FlockAgent agent ,Vector3 scaleVector, float second)
    {
        agent.GetComponent<RectTransform>().DOScale(scaleVector, second)
            .OnUpdate(() => {
                Width = GetComponent<RectTransform>().sizeDelta.x;
                Height = GetComponent<RectTransform>().sizeDelta.y;
                AgentManager.Instance.UpdateAgents();
            });
    }



    protected void DoDestoryOnCompleteCallBack(FlockAgent agent)
    {

        // 进行销毁
        if (typeof(CrossCardAgent).IsAssignableFrom(agent.GetType())) {
            AgentManager.Instance.RemoveItemFromEffectItems(agent as CardAgent);

            CardAgent ca = agent as CardAgent;

            Destroy(ca.gameObject);
            Destroy(ca.OriginAgent.gameObject);

        }
        else if (typeof(FlockAgent).IsAssignableFrom(agent.GetType())) {
            Destroy(agent.gameObject);
        }

    }



    //
    //  获取Logo
    //
    public RectTransform GetLogo() {
        Transform transform_thumb = null;
        Transform transform_logo = null;
        foreach(Transform child in transform){
            if (child.name == "thumb") {
                transform_thumb = child;
                break;
            }
        }

        if (transform_thumb != null) {
            foreach (Transform child in transform_thumb)
            {
                if (child.name == "logo")
                {
                    transform_logo = child;
                    return transform_logo.GetComponent<RectTransform>();
                }
            }

        }
        
        return null;
    }


}


