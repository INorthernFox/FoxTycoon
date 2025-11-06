using UnityEngine;

namespace Infrastructure.AudioServices
{
    [CreateAssetMenu(menuName = "Game/Audio/PlayList", fileName = "AudioPlayList", order = 0)]
    public class AudioPlayList : ScriptableObject
    {
        public AudioClip[] Clips;
    }

}