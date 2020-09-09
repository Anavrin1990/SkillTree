using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;

public class MainUIPresenter : MonoBehaviour
{
    private GameStats gameStats;
    private MainUIView mainUiView;
    private SkillTreePresenter skillTreePresenter;

    [Inject] void Init
    (
        MainUIView mainUiView,
        GameStats gameStats,
        SkillTreePresenter skillTreePresenter
    )
    {
        this.mainUiView = mainUiView;
        this.gameStats = gameStats;
        this.skillTreePresenter = skillTreePresenter;
    }
    
    private void Start()
    {
        gameStats.Score
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(score => mainUiView.scoreText.text = $"Счет {score.ToString()}")
            .AddTo(this);

        mainUiView.skillTreeButton
            .OnClickAsObservable()
            .Subscribe(_ => skillTreePresenter.ToggleSetActive())
            .AddTo(this);
    }
}
