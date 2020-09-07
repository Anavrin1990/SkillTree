using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class GamePresenter : MonoBehaviour
{
    [SerializeField] private GameStats gameStats;
    [SerializeField] private GameView gameView;

    private void Start()
    {
        gameView.boostButton
            .OnClickAsObservable()
            .Subscribe(_ => gameStats.score.Value++)
            .AddTo(this);
    }
}
