using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [SerializeField] private SkillTreePresenter _skillTreePresenter;
    [SerializeField] private SkillStorage _skillStorage;
    
    public override void InstallBindings()
    {
        Container.Bind<SkillManager>().AsSingle();
        Container.Bind<GameStats>().AsSingle();
        
        Container.BindInstance(_skillTreePresenter).AsSingle();
        Container.BindInstance(_skillStorage).AsSingle();

        Container.Bind<MainUIView>().FromComponentsSibling();
        Container.Bind<GameView>().FromComponentsSibling();
        Container.Bind<SkillTreeView>().FromComponentsSibling();
    }
}