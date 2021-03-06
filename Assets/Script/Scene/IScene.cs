﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{
    public interface IScene
    {

        void Init(SceneConfig sceneConfig, MagicWallManager manager,Action onSceneCompleted);
        //
        //	启动,返回值表明场景是否正在运行,当为false时表示该场景已运行完毕
        //
        bool Run();

        //
        //  获取内容类型
        //
        DataTypeEnum GetDataType();

        MagicSceneEnum GetSceneStatus();

        void RunEnd(Action onEndCompleted);


        SceneConfig GetSceneConfig();

    }

}