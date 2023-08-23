#undef ENABLE_CAPPING_TIME
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UniRx;

namespace Assets._SDK.Ads
{
    public class CappingTimer
    {
        private float _cappingTime;
        private Stopwatch _cappingStopwatch;
        private FirebaseService firebaseService;

        private CompositeDisposable disposables = new CompositeDisposable();

#if ENABLE_CAPPING_TIME
        public CappingTimer()
        {
            _cappingTime = AdsConfig.CappingTimeDefaultValue;
            firebaseService = FirebaseService.Instance;

            firebaseService.IsConnected
                .Subscribe(isConnected =>
                {
                    if (!isConnected)
                    {
                        return;
                    }
                    SetCappingTimeFromFireBase();
                }).AddTo(disposables);
            _cappingStopwatch = new Stopwatch();
        }
        private void SetCappingTimeFromFireBase()
        {
            _cappingTime = (float)firebaseService.GetRemoteConfigByKey(AdsConfig.CappingTimeRemoteConfigKey).DoubleValue;
            disposables.Clear();
        }
#endif
        public bool IsInterstitialCapped()
        {
#if ENABLE_CAPPING_TIME
            return _cappingStopwatch.Elapsed.TotalSeconds > _cappingTime;
#else
            return false;
#endif
        }

        public void Start()
        {
            _cappingStopwatch?.Start();
        }
        public void Reset()
        {
            _cappingStopwatch?.Reset();
        }

        public void Stop()
        {
            _cappingStopwatch?.Stop();
        }

        public bool IsRunning => _cappingStopwatch != null && _cappingStopwatch.IsRunning;

        ~CappingTimer()
        {
            disposables.Clear();
        }
    }
}