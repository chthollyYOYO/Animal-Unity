using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Framework
{
    /// <summary>
    /// 简单计时器
    /// </summary>
    public class SimpleTimer
    {
        private CancellationTokenSource cts;

        private SimpleTimer()
        {

        }

        public static SimpleTimer Create(float delaySeconds, Action callback)
        {
            SimpleTimer simpleTimer = new();
            simpleTimer.UpdateAsync(delaySeconds, callback, (simpleTimer.cts = new()).Token).Forget();
            return simpleTimer;
        }

        public static SimpleTimer Create<T>(float delaySeconds, Action<T> callback, T data)
        {
            return Create(delaySeconds, () => callback(data));
        }

        public void Destroy()
        {
            cts?.Cancel();
        }

        private async UniTaskVoid UpdateAsync(float delaySeconds, Action callback, CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delaySeconds), cancellationToken: cancellationToken);
            callback?.Invoke();
        }


        public static SimpleTimer CreateFrame(int frameCount, Action callback)
        {
            SimpleTimer simpleTimer = new();
            simpleTimer.UpdateAsyncFrame(frameCount, callback, (simpleTimer.cts = new()).Token).Forget();
            return simpleTimer;
        }

        private async UniTaskVoid UpdateAsyncFrame(int frameCount, Action callback, CancellationToken cancellationToken)
        {
            await UniTask.DelayFrame(frameCount, cancellationToken: cancellationToken);
            callback?.Invoke();
        }
    }
}