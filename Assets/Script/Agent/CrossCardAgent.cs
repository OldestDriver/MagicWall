﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

public class CrossCardAgent : CardAgent
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


