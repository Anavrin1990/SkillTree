using UniRx;
using UnityEngine;
using Zenject;

public class GameStats
{
    public readonly ReactiveProperty<int> Score = new ReactiveProperty<int>();
}
