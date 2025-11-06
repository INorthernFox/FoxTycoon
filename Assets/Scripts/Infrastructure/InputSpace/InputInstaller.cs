using Infrastructure.InputSpace.Camera;
using Zenject;

namespace Infrastructure.InputSpace
{
    public class InputInstaller : MonoInstaller
    {
        #if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        public MainCamera PCMainCamera;
        #endif
        #if UNITY_EDITOR|| UNITY_ANDROID
        public MainCamera MobileMainCamera;
        #endif

        public override void InstallBindings()
        {
            #if UNITY_STANDALONE || UNITY_WEBGL
            Container.Bind<MainCamera>().FromInstance(PCMainCamera).AsSingle();
            #elif UNITY_ANDROID
            Container.Bind<MainCamera>().FromInstance(MobileMainCamera).AsSingle();
            #endif
            
            Container.BindInterfacesTo<InputManager>().AsSingle();
        }

    }
}