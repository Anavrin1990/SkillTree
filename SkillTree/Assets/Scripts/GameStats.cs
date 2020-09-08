using UniRx;
using UnityEngine;
using Zenject;

public class GameStats
{
    public ReactiveProperty<int> Score = new ReactiveProperty<int>();
}
