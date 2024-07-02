using System;
using UnityEngine;

namespace WheelOfLuck
{
    public abstract class BaseSpinCostProvider : MonoBehaviour
    {
        public bool Available { get; protected set; }
        
        public event Action<bool> SpinAvailableUpdated;

        protected abstract void UpdateAvailable();

        public abstract void OnSpinStart();
        
        protected virtual void OnSpinAvailableUpdated(bool available)
        {
            SpinAvailableUpdated?.Invoke(available);
        }
    }
}
