using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class GamePresenter : MonoBehaviour
{
    private GameStats _gameStats;
    private GameView _gameView;

    [Inject] void Init
    (
        GameStats gameStats,
        GameView gameView
    )
    {
        _gameStats = gameStats;
        _gameView = gameView;
    }
    
    private void Start()
    {
        _gameView.BoostButton
            .OnClickAsObservable()
            .Subscribe(_ => _gameStats.Score.Value++)
            .AddTo(this);
    }
}
