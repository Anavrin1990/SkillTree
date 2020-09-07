using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private GameStats gameStats;
    [SerializeField] private SkillTreePresenter skillTreePresenter;
    [SerializeField] private SkillManager skillManager;
    
    public override void InstallBindings()
    {
        Container.BindInstance(gameStats).AsSingle();
        Container.BindInstance(skillTreePresenter).AsSingle();
        Container.BindInstance(skillManager).AsSingle();
        
        Container.Bind<MainUIView>().FromComponentsSibling();
        Container.Bind<GameView>().FromComponentsSibling();
        Container.Bind<SkillTreeView>().FromComponentsSibling();
    }
}