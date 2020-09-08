using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private SkillTreePresenter skillTreePresenter;
    [SerializeField] private SkillStorage skillStorage;
    
    public override void InstallBindings()
    {
        Container.Bind<SkillManager>().AsSingle();
        Container.Bind<GameStats>().AsSingle();
        
        Container.BindInstance(skillTreePresenter).AsSingle();
        Container.BindInstance(skillStorage).AsSingle();

        Container.Bind<MainUIView>().FromComponentsSibling();
        Container.Bind<GameView>().FromComponentsSibling();
        Container.Bind<SkillTreeView>().FromComponentsSibling();
    }
}