using System;
using Infrastructure.SaveServices.Interfaces;
using Newtonsoft.Json;
using UniRx;
using UnityEngine;

namespace Infrastructure.SaveServices
{
    public abstract class BaseSaveable<T> : ISaveable
    {
        public abstract string Key { get; }
        public virtual bool SaveAlways => false;
        public IReadOnlyReactiveProperty<bool> HaveChanges => _haveChanges;
        private readonly ReactiveProperty<bool> _haveChanges = new(false);

        public string Save()
        {
            T data = GetData();
            _haveChanges.Value = false;
            return JsonConvert.SerializeObject(data);
        }

        public void Load(string save)
        {
            T data = JsonConvert.DeserializeObject<T>(save);
            OnLoad(data);
        }

        protected void SetChanges() =>
            _haveChanges.Value = true;

        protected abstract T GetData();
        protected abstract void OnLoad(T data);
    }
}