﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MagicWall
{

    /// <summary>
    ///   碰撞基础实体
    ///   https://www.yuque.com/u314548/fc6a5l/no8f5t
    /// </summary>
    public interface CollisionMoveBasicAgent
    {
        // 更新下一个移动位置
        void UpdateNextPosition(Vector3 vector);

        void UpdatePosition(List<CollisionEffectAgent> effectAgents);

        void CalculateEffectedDestination(List<CollisionEffectAgent> effectAgents);

        Vector3 GetCollisionRefPosition();

    }
}