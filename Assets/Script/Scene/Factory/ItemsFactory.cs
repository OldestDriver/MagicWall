﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
//  浮块的设计
//
public interface ItemsFactory
{

    FlockAgent Generate(float gen_x, float gen_y, float ori_x, float ori_y, int row, int column, float width, float height);


}
