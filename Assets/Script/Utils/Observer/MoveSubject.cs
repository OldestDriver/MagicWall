﻿/// <summary>
/// 观察者 - 被观察者
/// </summary>
namespace MagicWall
{
    public interface MoveSubject
    {
        void AddObserver(MoveBtnObserver observer);

        void RemoveObserver(MoveBtnObserver observer);

        void NotifyObserver();
    }
}