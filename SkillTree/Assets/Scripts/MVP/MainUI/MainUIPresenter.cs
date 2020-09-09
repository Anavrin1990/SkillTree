using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class MainUIPresenter : MonoBehaviour
{
    private GameStats _gameStats;
    private MainUIView _mainUiView;
    private SkillTreePresenter _skillTreePresenter;

    [Inject] void Init
    (
        MainUIView mainUiView,
        GameStats gameStats,
        SkillTreePresenter skillTreePresenter
    )
    {
        _mainUiView = mainUiView;
        _gameStats = gameStats;
        _skillTreePresenter = skillTreePresenter;
    }
    
    private void Start()
    {
        _gameStats.Score
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(score => _mainUiView.ScoreText.text = $"Счет {score.ToString()}")
            .AddTo(this);

        _mainUiView.SkillTreeButton
            .OnClickAsObservable()
            .Subscribe(_ => _skillTreePresenter.ToggleSetActive())
            .AddTo(this);
    }
}
