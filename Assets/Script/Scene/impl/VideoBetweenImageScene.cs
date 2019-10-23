﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;
using UnityEngine.Video;

// 6屏幕，视频在中间
namespace MagicWall
{
    public class VideoBetweenImageScene : IScene
    {
        //
        //  Parameter
        //
        private MagicWallManager _manager;
        private MagicSceneManager _magicSceneManager;

        private CutEffectDestoryBehavior _destoryBehavior;


        Action _onSceneCompleted;

        /// <summary>
        /// 场景持续时间
        /// </summary>
        float _durTime;

        SceneConfig _sceneConfig;


        private bool _hasInit = false;
        private float _startTime;



        //  场景状态
        SceneStatus status = SceneStatus.PREPARING; //场景状态
        public SceneStatus Status { get { return status; } set { status = value; } }

        MagicSceneEnum _magicSceneEnumStatus;

        public void Init(SceneConfig sceneConfig, MagicWallManager manager,Action onSceneCompleted)
        {
            _manager = manager;
            _sceneConfig = sceneConfig;
            _onSceneCompleted = onSceneCompleted;

            _magicSceneManager = GameObject.Find("Scene").GetComponent<MagicSceneManager>();
            _magicSceneManager._videoBetweenImageController.Init(manager);

            // 销毁
            _destoryBehavior = DestoryBehaviorFactory.GetBehavior(sceneConfig.destoryBehavior);
            _destoryBehavior.Init(_manager, null, OnDestoryCompleted);

        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData() {
            _durTime = _sceneConfig.durtime;
            _magicSceneManager._videoBetweenImageController.StartPlay();
            _startTime = Time.time;
            _hasInit = true;
        }

        //  运行
        public bool Run()
        {
            if (!_hasInit) {
                InitData();
            }

            // 整体淡入

            // 左侧、右侧轮播图片

            // 播放视频

            // 当场景结束调用

            if ((Time.time - _startTime) > _durTime) {
               _hasInit = false;

                // 淡出

                _destoryBehavior.Run();

            }


            return true;
        }


        public DataTypeEnum GetDataType()
        {
            return DataTypeEnum.Video;
        }

        public MagicSceneEnum GetSceneStatus()
        {
            return _magicSceneEnumStatus;
        }

        void OnDestoryCompleted() {
            _magicSceneManager._videoBetweenImageController.StopPlay();
            _onSceneCompleted.Invoke();
        }

    }
}