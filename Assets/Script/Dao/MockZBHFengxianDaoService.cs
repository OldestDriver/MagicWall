﻿
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


/// <summary>
///    智博会 - 奉贤
/// </summary>
namespace MagicWall
{
    public class MockZBHFengxianDaoService : MonoBehaviour, IDaoService
    {
        [SerializeField]
        MockSceneConfig _mockSceneConfig;

        public MockSceneConfig mockSceneConfig
        {
            set
            {
                _mockSceneConfig = value;
            }
            get
            {
                return _mockSceneConfig;
            }
        }



        private List<Enterprise> _enterprises;
        private List<Activity> _activities;
        private List<Product> _products;

        private Dictionary<int, Enterprise> _enterpriseMap;
        private Dictionary<int, Product> _productMap;

        private Dictionary<int, List<Activity>> _activityByEidMap;
        private Dictionary<int, List<Product>> _productByEidMap;
        private Dictionary<int, List<Catalog>> _catalogByEidMap;



        void Awake()
        {


        }

        //
        //  Construct
        //
        protected MockZBHFengxianDaoService() { }



        public void Init()
        {
            _enterprises = new List<Enterprise>();
            _activities = new List<Activity>();
            _products = new List<Product>();

            _productMap = new Dictionary<int, Product>();
            _enterpriseMap = new Dictionary<int, Enterprise>();
        }

        public void Reset()
        {
            Init();
        }

        //
        //  加载信息
        //
        public void LoadInformation()
        {

        }

        //
        //  获取首页企业
        //
        public List<Enterprise> GetEnterprises()
        {
            throw new System.NotImplementedException();
        }

        //
        //  获取首页企业
        //
        public Enterprise GetEnterprise()
        {
            return _enterprises[Random.Range(0, _enterprises.Count)];           
        }

        public List<string> GetEnvCards(int id)
        {
            List<string> list = new List<string>();

            //list.Add("ZBH\\fengxian\\企业名片1.jpg");
            //list.Add("ZBH\\fengxian\\企业名片2.jpg");
            //list.Add("ZBH\\fengxian\\企业名片3.jpg");



            return list;

        }



        //
        //  获取 catalog
        //
        public Catalog GetCatalog(int id)
        {
            throw new System.NotImplementedException();
        }

        //
        //  获取 catalogs
        //
        public List<Catalog> GetCatalogs(int id)
        {
            return _catalogByEidMap[id];
        }



        //
        //  获取企业的详细信息
        //
        public EnterpriseDetail GetEnterprisesDetail(int com_id)
        {
            throw new System.NotImplementedException();
        }

        //
        //  获取首页活动
        //
        public List<Activity> GetActivities()
        {
            throw new System.NotImplementedException();
        }

        //
        //  获取首页活动
        //
        public Activity GetActivity()
        {
            throw new System.NotImplementedException();
        }


        //
        //  获取首页活动的详细信息
        //
        public Activity GetActivityDetail(int act_id)
        {
            throw new System.NotImplementedException();
        }

        public List<ActivityDetail> GetActivityDetails(int act_id)
        {
            throw new System.NotImplementedException();
        }


        //
        //  获取首页的产品
        //
        public List<Product> GetProducts()
        {
            return _products;

        }



        public Product GetProduct()
        {
            List<Product> product = GetProducts();
            int index = Random.Range(0, _products.Count);
            return _products[index];
        }

        //
        //  获取产品详细
        //
        public Product GetProductDetail(int pro_id)
        {
            var product = _productMap[pro_id];

            return product;
        }

        public List<ProductDetail> GetProductDetails(int pro_id)
        {
            List<ProductDetail> productDetails = new List<ProductDetail>();

            return productDetails;
        }

        #region 设置效果与运行时间

        //
        //  获取config
        //
        public AppConfig GetConfigByKey(string key)
        {
            AppConfig appConfig = new AppConfig();
            appConfig.Value = "20";

            if (key.Equals(AppConfig.KEY_CutEffectDuring_CurveStagger))
            {
                appConfig.Value = "10";
            }
            else if (key.Equals(AppConfig.KEY_CutEffectDuring_LeftRightAdjust))
            {
                appConfig.Value = "10";
            }
            else if (key.Equals(AppConfig.KEY_CutEffectDuring_MidDisperseAdjust))
            {
                appConfig.Value = "10";
            }
            else if (key.Equals(AppConfig.KEY_CutEffectDuring_Stars))
            {
                appConfig.Value = "40";
            }
            else if (key.Equals(AppConfig.KEY_CutEffectDuring_UpDownAdjust))
            {
                appConfig.Value = "10";
            }
            else if (key.Equals(AppConfig.KEY_CutEffectDuring_FrontBackUnfold))
            {
                appConfig.Value = "10";
            }
            else
            {

            }
            appConfig.Value = "10";

            return appConfig;
        }

        /// <summary>
        ///     获取场景持续时间
        /// </summary>
        /// <param name="sceneTypeEnum"></param>
        /// <returns></returns>
        public float GetSceneDurTime(SceneTypeEnum sceneTypeEnum)
        {
            string key = "";

            if (sceneTypeEnum == SceneTypeEnum.CurveStagger)
            {
                key = AppConfig.KEY_CutEffectDuring_CurveStagger;
            }
            else if (sceneTypeEnum == SceneTypeEnum.FrontBackUnfold)
            {
                key = AppConfig.KEY_CutEffectDuring_FrontBackUnfold;
            }
            else if (sceneTypeEnum == SceneTypeEnum.LeftRightAdjust)
            {
                key = AppConfig.KEY_CutEffectDuring_LeftRightAdjust;
            }
            else if (sceneTypeEnum == SceneTypeEnum.MidDisperse)
            {
                key = AppConfig.KEY_CutEffectDuring_MidDisperseAdjust;
            }
            else if (sceneTypeEnum == SceneTypeEnum.Stars)
            {
                key = AppConfig.KEY_CutEffectDuring_Stars;
            }
            else if (sceneTypeEnum == SceneTypeEnum.UpDownAdjustCutEffect)
            {
                key = AppConfig.KEY_CutEffectDuring_UpDownAdjust;
            }

            string durTime = GetConfigByKey(key).Value;
            float d = AppUtils.ConvertToFloat(durTime);

            return d;
        }
        #endregion

        public int GetLikesByProductDetail(int id)
        {
            int likes = Random.Range(1, 50);
            return likes;
        }

        public int GetLikesByActivityDetail(int id)
        {
            int likes = Random.Range(1, 50);
            return likes;
        }


        public int GetLikes(int id, CrossCardCategoryEnum category)
        {
            int likes = Random.Range(1, 50);
            return likes;
        }

        //
        //  获取显示配置
        //
        public List<SceneConfig> GetShowConfigs()
        {
            /// 已修改为编辑器配置方式
            ///  -》 config / MockSceneConfig 


            List<SceneConfig> items = new List<SceneConfig>();

            var sceneConfigs = _mockSceneConfig.sceneConfigs;

            for (int i = 0; i < sceneConfigs.Count; i++)
            {
                var scene = sceneConfigs[i].sceneType;
                var data = sceneConfigs[i].dataType;
                var time = sceneConfigs[i].durtime;


                if (scene == SceneTypeEnum.Stars && data == DataTypeEnum.Enterprise)
                {
                    continue;
                }

                if (scene == SceneTypeEnum.FrontBackUnfold && data == DataTypeEnum.Enterprise)
                {
                    continue;
                }


                items.Add(sceneConfigs[i]);
            }

            return items;


        }

        public bool IsCustom()
        {
            //TODO
            int number = Random.Range(0, 5);
            return number > 2;

            return true;

        }




        //
        //  TODO 获取定制屏所配置的图片
        //
        public List<string> GetCustomImage(CustomImageType type)
        {

            string[] leftImages = { "ZBH\\fengxian\\智城第一屏.jpg", "ZBH\\fengxian\\智城第一屏1.jpg", "ZBH\\fengxian\\智城第一屏2.jpg" };
            //string[] middleImages = { "m1.jpg", "m2.jpg", "m3.jpg", "m4.jpg", "m5.jpg" };
            string[] rightImages = { "ZBH\\fengxian\\智城第五屏.jpg" };

            if (type == CustomImageType.LEFT1)
            {
                List<string> images = new List<string>();
                int size = leftImages.Length;
                for (int i = 0; i < size; i++)
                {
                    images.Add(leftImages[i]);
                }
                return images;
            }
            //else if (type == CustomImageType.LEFT2)
            //{
            //    List<string> images = new List<string>();
            //    int size = Random.Range(1, 4);
            //    size = 5;
            //    for (int i = 0; i < size; i++)
            //    {
            //        images.Add("custom\\" + middleImages[i]);
            //    }
            //    return images;
            //}
            else
            {
                List<string> images = new List<string>();
                int size = rightImages.Length;
                for (int i = 0; i < size; i++)
                {
                    images.Add(rightImages[i]);
                }
                return images;
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="keys">关键词</param>
        /// <returns></returns>
        public List<SearchBean> Search(string keys)
        {
            //Debug.Log("搜索KEYS ：" + keys);

            List<SearchBean> beans = new List<SearchBean>();

            for (int i = 0; i < _products.Count; i++)
            {
                var name = _products[i].Name;
                if (name.Contains(keys))
                {
                    SearchBean bean = new SearchBean();
                    bean.type = DataTypeEnum.Product;
                    bean.id = _products[i].Pro_id;
                    bean.cover = _products[i].Image;
                    beans.Add(bean);
                }
            }

            return beans;
        }


        /// <summary>
        ///     获得浮动块数据
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public FlockData GetFlockData(DataType type)
        {
            if (type == DataType.env)
            {
                return GetEnterprise();
            }
            else if (type == DataType.product)
            {
                return GetProduct();
            }
            else if (type == DataType.activity)
            {
                return GetActivity();
            }
            return null;
        }

        public Enterprise GetEnterpriseById(int id)
        {
            return _enterpriseMap[id];
        }

        public Video GetVideoDetail(int envId, int index)
        {
            return new Video().Generator();
        }

        public List<Video> GetVideosByEnvId(int envId)
        {
            return new List<Video>();

        }

        public List<Activity> GetActivitiesByEnvId(int envid)
        {
            if (_activityByEidMap.ContainsKey(envid))
            {
                return _activityByEidMap[envid];
            }
            else {
                return new List<Activity>();
            }


        }

        public List<Product> GetProductsByEnvId(int envid)
        {
            return _productByEidMap[envid];
        }

        public MWConfig GetConfig()
        {
            //Debug.Log("Mock Config");
            return new MWConfig();
        }

        public void InitData()
        {
            print("Init Data");


            _enterpriseMap = new Dictionary<int, Enterprise>();
            _enterprises = new List<Enterprise>();
            _activityByEidMap = new Dictionary<int, List<Activity>>();
            _productByEidMap = new Dictionary<int, List<Product>>();
            _catalogByEidMap = new Dictionary<int, List<Catalog>>();

             string[] names = {
                "01（乡伴集团）朱胜萱工作室",
                "02 上海艾樱健康科技股份有限公司"
             };


            for (int i = 0; i < names.Length; i++) {
                int entId = i + 1;
                AddEnterprise(names[i], entId);
            }

            print("Init Data End");

        }


        //
        private void AddEnterprise(string name,int ent_id) {
            Enterprise enterprise = new Enterprise();
            enterprise.Ent_id = ent_id;

            string logoPath = "ZBH\\fengxian\\" + name + "\\logo.png";            
            string businessCardPath = "ZBH\\fengxian\\" + name + "\\bcard.png";

            // logo path
            enterprise.Logo = logoPath;

            // 公司卡片 path
            enterprise.Business_card = businessCardPath;

            // Add Catalog
            AddCatalogByEnterprise(name, ent_id);

            // Add Product
            AddProductByEnterprise(name, ent_id);

            //增加公司名片
            AddBusinessCard(name, enterprise);            

            _enterpriseMap.Add(ent_id, enterprise);
            _enterprises.Add(enterprise);

        }

        private void AddCatalogByEnterprise(string name, int ent_id) {
            string catalogDirPath = "ZBH\\fengxian\\" + name + "\\catalog";

            //print("PATH :" + (MagicWallManager.FileDir + catalogDirPath));

            if (Directory.Exists(MagicWallManager.FileDir + catalogDirPath)) {
                DirectoryInfo dirInfo = new DirectoryInfo(MagicWallManager.FileDir + catalogDirPath);
                FileInfo[] files = dirInfo.GetFiles();

                List<Catalog> catalogs = new List<Catalog>();
                for (int i = 0; i < files.Length; i++) {
                    var fileName = files[i].Name;
                    var fileNameWithoutExt = fileName.Replace(files[i].Extension,"");

                    Catalog catalog = new Catalog();
                    catalog.Ent_id = ent_id;
                    catalog.Description = fileNameWithoutExt;
                    catalog.Id = i;
                    catalog.Img = catalogDirPath + "\\" + fileName;

                    catalogs.Add(catalog);
                }
                _catalogByEidMap.Add(ent_id, catalogs);
            } 
        }

        private void AddProductByEnterprise(string name, int ent_id)
        {
            string catalogDirPath = "ZBH\\fengxian\\" + name + "\\产品";

            if (Directory.Exists(MagicWallManager.FileDir + catalogDirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(MagicWallManager.FileDir + catalogDirPath);
                FileInfo[] files = dirInfo.GetFiles();

                List<Product> products = new List<Product>();
                for (int i = 0; i < files.Length; i++)
                {
                    var fileName = files[i].Name;
                    var fileNameWithoutExt = fileName.Replace(files[i].Extension, "");

                    Product product = new Product();
                    product.Ent_id = ent_id;
                    product.Description = fileNameWithoutExt;
                    product.Image = catalogDirPath + "\\" + fileName;
                    product.Pro_id = i;
                    product.Name = fileNameWithoutExt;
                    products.Add(product);
                }
                _productByEidMap.Add(ent_id, products);
            }
        }

        private void AddBusinessCard(string name, Enterprise enterprise)
        {
            string catalogDirPath = "ZBH\\fengxian\\" + name + "\\公司名片";

            if (Directory.Exists(MagicWallManager.FileDir + catalogDirPath))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(MagicWallManager.FileDir + catalogDirPath);
                FileInfo[] files = dirInfo.GetFiles();

                List<string> bcards = new List<string>();
                for (int i = 0; i < files.Length; i++)
                {
                    var fileName = files[i].Name;
                    var fileNameWithoutExt = fileName.Replace(files[i].Extension, "");
                    var p = catalogDirPath + "\\" + fileName;
                    bcards.Add(p);
                }

                enterprise.EnvCards = bcards;                
            }
        }


        public int GetLikes(string path)
        {
            return 1;
            //throw new System.NotImplementedException();
        }

        public bool UpdateLikes(string path)
        {
            return true;
            //throw new System.NotImplementedException();
        }

        public FlockData GetFlockData(DataTypeEnum type)
        {
            if (type == DataTypeEnum.Enterprise)
            {
                return GetEnterprise();
            }
            else if (type == DataTypeEnum.Product)
            {
                return GetProduct();
            }
            else if (type == DataTypeEnum.Activity)
            {
                return GetActivity();
            }
            return null;
        }

        public FlockData GetFlockDataByScene(DataTypeEnum type, int sceneIndex)
        {
            return GetFlockData(type);
        }

        public List<string> GetMatImageAddresses()
        {
            var result = new List<string>();

            //for (int i = 0; i < _products.Count; i++)
            //{
            //    result.Add(_products[i].Image);
            //}

            //var c1 = GetCustomImage(CustomImageType.LEFT1);
            //var c2 = GetCustomImage(CustomImageType.RIGHT);

            //for (int i = 0; i < c1.Count; i++)
            //{
            //    result.Add(c1[i]);
            //}

            //for (int i = 0; i < c2.Count; i++)
            //{
            //    result.Add(c2[i]);
            //}


            return result;
        }
    }
}