using System.Collections;
using System.Collections.Generic;
using Common.PersistentManager;
using UnityEngine;
using Zenject;

namespace Sample
{
    public class GameInstaller : MonoInstaller<GameInstaller>
    {
        public override void InstallBindings()
        {
            Container.Bind<IPersistentManager>().FromComponentInNewPrefabResource(@"PersistentManager").AsSingle();
        }
        
    }
}
