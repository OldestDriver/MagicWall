﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

//
//  滑动卡片代理，用于product和activity
//
public class SliceCardAgent : CardAgent
{
       
    void Awake() {
        AwakeAgency();
    }

    //
    //  更新
    //
    void Update() {
        UpdateAgency();
    }
   
}

