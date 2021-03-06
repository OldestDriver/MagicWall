﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {


    public class FlockAgentFactoryInstance : MonoBehaviour
    {
        
        /// <summary>
        ///     创建
        /// </summary>

        public static FlockAgent Generate(MagicWallManager manager, Vector2 position
            , AgentContainerType parent,float ori_x,float ori_y,
            int row,int column,float width,float height, FlockData flockData,DaoTypeEnum daoTypeEnum)
        {
            //Debug.Log("aa");


            FlockAgent _flockAgent = manager.agentManager.GetFlockAgent(parent);
            _flockAgent.name = "["+ manager.SceneIndex + "]Agent" + row + "-" + column;

            //Debug.Log("bb");


            //  定面板位置
            Vector2 ori_position = new Vector2(ori_x, ori_y);


            //Debug.Log("Generate flock data : " + flockData.GetId());

            _flockAgent.Initialize(manager, ori_position, position,
                row, column, width, height, flockData.GetId(), flockData.GetCover(), false,
                flockData.GetDataType(), parent, daoTypeEnum);

            manager.agentManager.AddItem(_flockAgent);            

            //CardAgent cardAgent = Instantiate(cardPrefab, parent);



            return _flockAgent;
        }
    }


}
