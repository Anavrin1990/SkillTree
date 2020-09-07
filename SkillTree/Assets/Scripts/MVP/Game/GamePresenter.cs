using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class GamePresenter : MonoBehaviour
{
    private GameStats gameStats;
    private GameView gameView;

    [Inject] void Init
    (
        GameStats gameStats,
        GameView gameView
    )
    {
        this.gameStats = gameStats;
        this.gameView = gameView;
    }
    
    private void Start()
    {
        gameView.boostButton
            .OnClickAsObservable()
            .Subscribe(_ => gameStats.score.Value++)
            .AddTo(this);
    }
}
