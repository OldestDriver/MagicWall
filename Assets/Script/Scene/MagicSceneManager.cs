﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UI;

//
//  场景管理器
//  ---
//  1. 场景问题
//  2. 背景问题
//  3. 过场动画问题
//
namespace MagicWall
{
    public class MagicSceneManager : MonoBehaviour
    {
        private MagicWallManager _manager;

        [SerializeField,Tooltip("运行背景")] bool _runBackground = true;
        [SerializeField, Tooltip("运行logo动画")] bool _runLogoAni = true;
        public bool runLogoAni { get { return _runLogoAni; } }

        [SerializeField,Header("Transform")] RectTransform _backContainer;

        [SerializeField] Sprite _logo;

        [SerializeField] public VideoBetweenImageController _videoBetweenImageController;
        [SerializeField] SubCutEffectManager _subCutEffectManager;
        [SerializeField] public bool _preLoadResource;
        [SerializeField,Header("常驻效果管理器")] WholeEffectManager _wholeEffectManager;
        
        //public VideoBetweenImageController videoBetweenImageController { get { return videoBetweenImageController; } }


        public Sprite logo { get { return _logo; } }



        //
        //  paramater
        //
        bool _hasInit = false;
        bool _hasInitTheme = false;

        List<IScene> _scenes; // 场景列表
        int _index; // 当前的索引

        private MagicSceneEnum _sceneStatus;

        Action _onSceneEnterLoop;
        Action _onStartCompleted;


        //
        // Awake instead of Constructor
        //
        private void Awake()
        {

        }

        /// <summary>
        /// 初始化场景状态
        /// </summary>
        /// <param name="manager"></param>
        public void Init(MagicWallManager manager,Action onSceneEnterLoop,Action onStartCompleted)
        {

            System.Diagnostics.Stopwatch sw2 = new System.Diagnostics.Stopwatch();
            sw2.Start();

            _onSceneEnterLoop = onSceneEnterLoop;
            _onStartCompleted = onStartCompleted;

            _manager = manager;
            //_sceneStatus = MagicSceneEnum.Running;

            //  初始化场景列表
            _scenes = new List<IScene>();

            //  初始化当前场景索引
            _index = 0;

            //  加载场景
            // - 加载开始场景
            StartScene startScene = new StartScene();
            startScene.Init(null, manager, OnSceneCompleted);
            _scenes.Add(startScene);

            // - 加载普通场景
            List<SceneConfig> sceneConfigs = manager.daoServiceFactory.GetShowConfigs();
            for (int i = 0; i < sceneConfigs.Count; i++)
            {
                //Debug.Log(sceneConfigs[i].ToString());

                IScene scene;
                if (sceneConfigs[i].sceneType == SceneTypeEnum.Stars)
                {
                    scene = new StarScene();
                }
                else if (sceneConfigs[i].sceneType == SceneTypeEnum.VideoBetweenImageSixScene) {
                    scene = new VideoBetweenImageScene();
                }
                else if (sceneConfigs[i].sceneType == SceneTypeEnum.VideoBetweenImageEightScene)
                {
                    scene = new VideoBetweenImageSceneEightScreen();
                }
                else
                {
                    scene = new CommonScene();
                }

                scene.Init(sceneConfigs[i], _manager, OnSceneCompleted);
                _scenes.Add(scene);
            }

            sw2.Stop();
            //Debug.Log(" Scene init : " + sw2.ElapsedMilliseconds / 1000f);

            // 初始化管理器标志
            _manager.CurrentScene = _scenes[0];

            // 常驻效果管理器
            if (_wholeEffectManager.isOpen()) {
                _wholeEffectManager.Init(this);
            }

            _hasInit = true;
        }



        //
        // 开始场景调整
        //
        public void Run()
        {
            // 设置主题
            if (!_hasInitTheme) {
                // 设置主题
                InitTheme();
                _hasInitTheme = true;
            }


            // 背景始终运行
            if (_runBackground) {
                _manager.backgroundManager.run();
            }

            if (_wholeEffectManager.isOpen()) {
                _wholeEffectManager.Run();
            }


            if (!_hasInit && _scenes != null)
            {
                return;
            }

            _scenes[_index].Run();

        }

        /// <summary>
        ///  重启场景
        /// </summary>
        public void Reset()
        {
            _hasInit = false;

        }



        private void OnSceneCompleted() {
            _manager.mainPanel.GetComponent<CanvasGroup>().alpha = 1;
            _manager.mainPanel.GetComponentInChildren<CanvasGroup>().alpha = 1;
            _manager.Clear();
            GoNext();
        }




        private void GoNext()
        {
            if (_index == 0) {
                // 预加载场景结束
                _onStartCompleted.Invoke();
            }

            if (_index == _scenes.Count - 1)
            {
                _index = 0;
            }
            else
            {
                _index++;
            }

            _manager.SceneIndex = _manager.SceneIndex + 1;


            //Debug.Log("进入到下个场景，索引为： " + _index + " 类型为： " + _scenes[_index].GetSceneConfig().sceneType);

            _manager.CurrentScene = _scenes[_index];

        }

        public void CloseCurrent(Action onCloseCompleted) {
            _scenes[_index].RunEnd(onCloseCompleted);
        }

        public void JumpTo(int sceneIndex)
        {
            _index = sceneIndex;            
        }

        public IScene GetCurrentScene() {
            return _scenes[_index];

        }


        void InitTheme() {
            // 设置背景图片
            var backSprite = _manager.themeManager.GetService().GetBackSprite();
            _backContainer.GetComponent<Image>().sprite = backSprite;
        }

    }
}