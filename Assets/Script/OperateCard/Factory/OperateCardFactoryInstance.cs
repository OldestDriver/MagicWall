﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MagicWall {

    /// <summary>
    ///     卡片工厂生成器
    ///     ref : https://www.yuque.com/u314548/fc6a5l/mtblvk#1VNgA
    /// </summary>
    public class OperateCardFactoryInstance : MonoBehaviour
    {

        public static CardAgent Generate(MagicWallManager magicWallManager, Vector3 position, Transform parent, int dataId, DataTypeEnum dataType,FlockAgent refFlockAgent)
        {

            IDaoService daoService = magicWallManager.daoService;

            OperateCardData cardData = null;
            CardAgent cardPrefab = null ;


            //magicWallManager.daoService.getac
            if (dataType == DataTypeEnum.Enterprise)
            {
                Enterprise enterprise = daoService.GetEnterpriseById(dataId);
                var activities = daoService.GetActivitiesByEnvId(enterprise.Ent_id);
                var products = daoService.GetProductsByEnvId(enterprise.Ent_id);
                var videos = daoService.GetVideosByEnvId(enterprise.Ent_id);
                var catalogs = daoService.GetCatalogs(enterprise.Ent_id);
                OperateCardDataCross operateCardDataCross = EnterpriseAdapter.Transfer(enterprise, activities, products, videos, catalogs);
                if (CheckIsSimple(operateCardDataCross))
                {
                    cardPrefab = magicWallManager.operateCardManager.singleCardPrefab;
                    cardData = operateCardDataCross;
                }
                else
                {
                    cardPrefab = magicWallManager.operateCardManager.crossCardPrefab;
                    // 单个卡片的逻辑
                    //OperateCardDataSingle operateCardDataSingle = new OperateCardDataSingle();


                    //cardData = (OperateCardDataSingle)operateCardDataCross;
                }
            }
            else if (dataType == DataTypeEnum.Product)
            {
                Product product = daoService.GetProductDetail(dataId);
                Enterprise enterprise = daoService.GetEnterpriseById(product.Ent_id);
                OperateCardDataSlide operateCardDataSlide = ProductAdapter.Transfer(product, enterprise);
                cardPrefab = magicWallManager.operateCardManager.sliceCardPrefab;
                cardData = operateCardDataSlide;
            }
            else {
                Activity activity = daoService.GetActivityDetail(dataId);
                Enterprise enterprise = daoService.GetEnterpriseById(activity.Ent_id);
                OperateCardDataSlide operateCardDataSlide = ActivityAdapter.Transfer(activity, enterprise);
                cardPrefab = magicWallManager.operateCardManager.sliceCardPrefab;
                cardData = operateCardDataSlide;
            }



            CardAgent cardAgent = Instantiate(cardPrefab, parent);
            cardAgent.GetComponent<Transform>().position = position;
            //cardAgent.DataId = dataId;
            cardAgent.InitCardData(magicWallManager, dataId,dataType,position, refFlockAgent);
            cardAgent.InitData(cardData);
            return cardAgent;
        }



        private static bool CheckIsSimple(OperateCardDataCross operateCardDataCross) {
            if (operateCardDataCross.ScrollDic != null && operateCardDataCross.ScrollDic.Count > 1) {
                return false;
            }
            return true;
        }



    }

}

