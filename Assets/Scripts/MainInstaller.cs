using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class MainInstaller : MonoInstaller{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private MainGameManager _mainGameManager;
    [SerializeField] private UIManager _uiManager;
    [SerializeField] private ObjectPooler _objectPooler;
    [SerializeField] private Timer _timer;
    [SerializeField] private BottomCells _bottomCells;
    [SerializeField] private StackManager _stackManager;


    public override void InstallBindings() {
        Container.BindInstance(_mainCamera).WithId("mainCamera");
        Bind(_audioManager);
        Bind(_mainGameManager);
        Bind(_uiManager);
        Bind(_objectPooler);
        Bind(_timer);
        Bind(_bottomCells);
        Bind(_stackManager);
    }

    private protected void Bind<T>(T instance) {
        Container.BindInterfacesAndSelfTo<T>().FromInstance(instance).AsSingle();
    }

}
