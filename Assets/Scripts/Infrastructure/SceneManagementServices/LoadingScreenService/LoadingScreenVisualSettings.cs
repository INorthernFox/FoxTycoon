using UnityEngine;

namespace Infrastructure.SceneManagementServices.LoadingScreenService
{
    [CreateAssetMenu(menuName = "Game/LoadingScreen/Settings", fileName = "LoadingScreenVisualSettings", order = 0)]
    public class LoadingScreenVisualSettings : ScriptableObject
    {
        public float FadeDuration = 0.5f;
        public float HideDelay = 2f;
    }
}