﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall {

    /// <summary>
    /// ref : https://www.yuque.com/books/share/4f5397bb-9ecf-4622-bf62-f812a38d2057
    /// </summary>
    public class KinectManager : MonoBehaviour
    {
        [SerializeField, Header("Prefab")] KinectAgent _kinectAgentPrefab;
        [SerializeField, Header("UI")] RectTransform _agentContainer;
        [SerializeField, Header("Service")] KinectService _kinect2Service;

        [SerializeField] KinectType _kinectType;

        private bool isMonitoring = false;

        private IKinectService _kinectService;


        private Action _startSuccessAction;
        private Action<string> _startFailedAction;

        private MagicWallManager _manager;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if (_manager != null) {
                _kinectService.Monitoring();
            }
            
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init(MagicWallManager magicWallManager) {
            _manager = magicWallManager;

            if (_kinectType == KinectType.Kinect2)
            {
                _kinectService = _kinect2Service;
            }
            else if (_kinectType == KinectType.AzureKinect){                
                // 缺失 kinect 3
            }

            _startSuccessAction = StartKinectSuccess;
            _startFailedAction = StartKinectFailed;

            _kinectService.Init(_agentContainer, _kinectAgentPrefab);

        }


        /// <summary>
        /// 开始监控
        /// </summary>
        public void StartMonitoring() {            
            _kinectService.StartMonitoring(_startSuccessAction, _startFailedAction);
            // 开启成功/失败后调用回调，修改isMonitoring
        }

        /// <summary>
        /// 关闭监控
        /// </summary>
        public void StopMonitoring()
        {
            // 关闭逻辑
            _kinectService.StopMonitoring();

            isMonitoring = false;
        }




        void StartKinectSuccess() {
            Debug.Log("启动 kinect 成功");
            isMonitoring = true;
        }

        void StartKinectFailed(string msg)
        {
            Debug.Log("启动 kinect 失败");
            isMonitoring = false;
        }


    }

}