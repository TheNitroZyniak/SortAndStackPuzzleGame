using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class MainInstaller : MonoInstaller{
    [SerializeField] private Camera _mainCamera;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private MainGameManager _mainGameManager;
    [SerializeField] private Spawner _spawner;

    public override void InstallBindings() {
        Container.BindInstance(_mainCamera).WithId("mainCamera");
        Bind(_audioManager);
        Bind(_mainGameManager);
        //Bind(_spawner);
    }

    private protected void Bind<T>(T instance) {
        Container.BindInterfacesAndSelfTo<T>().FromInstance(instance).AsSingle();
    }

}
