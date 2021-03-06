﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{
    [System.Serializable]
    public class Enterprise : FlockData, Generator<Enterprise>
    {
        #region Data Parameter

        // 企业 ID
        private int _ent_id;
        public int Ent_id { set { _ent_id = value; } get { return _ent_id; } }

        //  企业名片
        private string _business_card;
        public string Business_card { set { _business_card = value; } get { return _business_card; } }

        // 企业的logo
        private string _logo;
        public string Logo { set { _logo = value; } get { return _logo; } }

        // 企业名字
        private string _name;
        public string Name { set { _name = value; } get { return _name; } }

        private bool _isCustom;
        public bool IsCustom { set { _isCustom = value; } get { return _isCustom; } }

        //  描述
        private string _description;
        public string Description { set { _description = value; } get { return _description; } }

        //  喜欢数量 - 针对企业
        private int _likes;
        public int likes { set { _likes = value; } get { return _likes; } }

        //  Env 企业卡片组
        private List<string> _env_cards;
        public List<string> EnvCards { set { _env_cards = value; } get { return _env_cards; } }



        public Sprite SpriteLogo
        {
            get
            {
                string path = MagicWallManager.FileDir + _logo;
                return SpriteResource.Instance.GetData(path);
            }
        }

        #endregion


        #region 资源属性
        string[] logos = new string[] {
            "1.png","2.png","3.png","4.png","5.png","6.png","7.png","8.png","9.png","10.png",
            "11.png","12.png","13.png","14.png","15.png","16.png","17.png","18.png","19.png","20.png",
            "21.png","22.png","23.png","24.png","25.png","26.png","27.png","28.png","29.png","30.png"
        };

        string[] businessCards = new string[] {
            "business-card-1.png",
            "business-card-2.png",
            "business-card-3.png"
        };

        string[] envCards = new string[] {
            "env-card-1.png",
            "env-card-2-1.png",
            "env-card-2-2.png",
            "env-card-2-3.png"
        };



        #endregion

        public Enterprise Generator()
        {
            string[] names = new string[] { "百度", "可口可乐", "谷歌", "阿里巴巴", "豆瓣", "哔哩哔哩", "微软", "搜狗", "印象笔记", "迅雷" };

            string[] descriptions = new string[] {
            "京东商城京东JD.COM-专业的综合网上购物商城，销售超数万品牌、4020万种商品，囊括家电、手机、电脑、母婴、服装等13大品类。秉承客户为先的理念，京东所售商品为正品行货、全国联保、机打发票。",
            "淘宝网是亚太地区较大的网络零售、商圈，由阿里巴巴集团在2003年5月创立。淘宝网是中国深受欢迎的网购零售平台，拥有近5亿的注册用户数，每天有超过6000万的固定访客，同时每天的在线商品数已经超过了8亿件，平均每分钟售出4.8万件商品。",
            "“海澜之家”（英文缩写：HLA）是海澜之家股份有限公司旗下的服装品牌，主要采用连锁零售的模式，销售男性服装、配饰与相关产品。",
            "Unity3D是由Unity Technologies开发的一个让玩家轻松创建诸如三维视频游戏、建筑可视化、实时三维动画等类型互动内容的多平台的综合型游戏开发工具，是一个全面整合的专业游戏引擎。"
        };

            bool[] customs = new bool[] { true, false };

            Enterprise env = new Enterprise();
            int id = Random.Range(1, 10);
            env._ent_id = id;

            int name_index = Random.Range(0, names.Length);
            env._name = names[name_index];

            int logo_index = Random.Range(0, logos.Length);
            env._logo = "logo\\" + logos[logo_index];

            env._isCustom = customs[Random.Range(0, 2)];

            env._description = descriptions[Random.Range(0, descriptions.Length)];

            bool hasLike = Random.Range(0, 5) > 5;
            env.likes = hasLike ? Random.Range(1, 99) : 0;



            if (Random.Range(0, 5) > 2)
            {
                env._business_card = "env\\" + businessCards[Random.Range(0, businessCards.Length)];
            }
            else
            {
                env._business_card = "env\\sh1.jpg";
            }



            // 1. 没有企业卡片 2. 单个企业卡片 3. 多个企业卡片

            int cardType = Random.Range(1, 8);
            cardType = 4;


            if (cardType == 0)
            {
                _env_cards = new List<string>();
            }

            else if (cardType == 1)
            {
                _env_cards = new List<string>();
                string address = "env\\" + "env-card-1.png";
                _env_cards.Add(address);
            }
            else if (cardType == 2)
            {
                _env_cards = new List<string>();
                _env_cards.Add("env\\" + "env-card-2-1.png");
                _env_cards.Add("env\\" + "env-card-2-2.png");
                _env_cards.Add("env\\" + "env-card-2-3.png");
            }
            else
            {
                _env_cards = new List<string>();
                for (int i = 1; i < 9; i++)
                {
                    string s = "env\\" + "sh" + i + ".jpg";
                    _env_cards.Add(s);
                }
            }

            env.EnvCards = _env_cards;

            return env;
        }


        public List<string> GetAssetAddressList()
        {
            List<string> list = new List<string>();

            for (int i = 0; i < logos.Length; i++)
            {
                string address = "logo\\" + logos[i];
                list.Add(address);
            }

            for (int i = 0; i < businessCards.Length; i++)
            {
                string address = "env\\" + businessCards[i];
                list.Add(address);
            }

            for (int i = 0; i < envCards.Length; i++)
            {
                string address = "env\\" + envCards[i];
                list.Add(address);
            }

            return list;
        }


        public override Sprite GetCoverSprite()
        {
            string path = MagicWallManager.FileDir + _logo;
            return SpriteResource.Instance.GetData(path);
        }

        public override string GetCover()
        {
            return _logo;
        }

        public override int GetId()
        {
            return _ent_id;
        }

        public override DataTypeEnum GetDataType()
        {
            return DataTypeEnum.Enterprise;
        }
    }
}