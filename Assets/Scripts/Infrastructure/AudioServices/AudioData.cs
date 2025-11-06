using System;
using Infrastructure.SaveServices;
using Infrastructure.SaveServices.Interfaces;
using UniRx;
using UnityEngine;

namespace Infrastructure.AudioServices
{

    public class AudioData : BaseSaveable<AudioData.Data>, IAudioData, ISettingsSaveable
    {
        public IReadOnlyReactiveProperty<float> Volume => _volume;
        private readonly ReactiveProperty<float> _volume = new(0.1f);

        public void ChangeVolume(float volume)
        {
            volume = Math.Clamp(volume, 0, 1);
            if(Mathf.Approximately(volume, _volume.Value))
                return;

            _volume.Value = volume;
            SetChanges();
        }

    #region Save
        public override string Key => "audio_data";

        protected override Data GetData() =>
            new(_volume.Value);

        protected override void OnLoad(Data data)
        {
            _volume.Value = data.Volume;
        }

        [Serializable]
        public struct Data
        {
            public readonly float Volume;

            public Data(float volume)
            {
                Volume = volume;
            }
        }
    #endregion

    }
}