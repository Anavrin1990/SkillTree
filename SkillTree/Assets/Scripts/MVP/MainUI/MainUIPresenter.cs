using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

public class MainUIPresenter : MonoBehaviour
{
    [SerializeField] private GameStats gameStats;
    [SerializeField] private MainUIView mainUiView;
    [SerializeField] private SkillTreePresenter skillTreePresenter;
    
    private void Start()
    {
        gameStats.score
            .ObserveEveryValueChanged(x => x.Value)
            .Subscribe(score => mainUiView.scoreText.text = score.ToString())
            .AddTo(this);

        mainUiView.skillTreeButton
            .OnClickAsObservable()
            .Subscribe(_ => skillTreePresenter.ToggleSetActive())
            .AddTo(this);
    }
}
