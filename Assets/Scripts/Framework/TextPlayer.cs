using Cysharp.Threading.Tasks;
using System;
using System.Threading;

namespace Framework
{
    public class TextPlayer
    {
        public event Action<string> OnTextChanged;
        public event Action OnPlayEnd;
        public string Content { get; set; }
        /// <summary>
        /// 单位：字/秒
        /// </summary>
        public float Speed { get; set; }
        public bool IsPlaying { get; private set; }
        public int CurrentLength { get; private set; }

        private CancellationTokenSource cts;

        private readonly string punctuationChars = @"(\！|\!|\？|\?|\，|\,|\。|\.|\》|\）|\)|\：|\:|\、|\\|\；|\;|\+|\-|\·|\”|\】|\]|\——|\/)";

        ~TextPlayer()
        {
            cts?.Cancel();
        }

        public void Play(string content, bool restart = true, float speed = 20)
        {
            Content = content;

            if (restart)
                CurrentLength = 0;

            Speed = speed;

            if (Content.Length > 0 && !IsPlaying)
            {
                IsPlaying = true;
                PlayTyping((cts = new()).Token).Forget();
            }
        }

        private async UniTaskVoid PlayTyping(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1 / Speed), cancellationToken: cancellationToken);
            while (CurrentLength < Content.Length)
            {
                var lc = Content[CurrentLength];
                var c = Content[CurrentLength];
                if (CurrentLength < Content.Length - 1)
                {
                    CurrentLength++;
                    c = Content[CurrentLength];
                    if (IsPunctuation(c) && !IsPunctuation(lc))
                    {
                        if (CurrentLength > 0)
                            CurrentLength--;

                        OnTextChanged?.Invoke(Content[..CurrentLength]);
                        CurrentLength++;
                    }
                    else
                    {
                        OnTextChanged?.Invoke(Content[..CurrentLength]);
                        await UniTask.Delay(TimeSpan.FromSeconds(1 / Speed), cancellationToken: cancellationToken);
                    }
                }
                else
                {
                    CurrentLength++;
                    OnTextChanged?.Invoke(Content[..CurrentLength]);
                    await UniTask.Delay(TimeSpan.FromSeconds(1 / Speed), cancellationToken: cancellationToken);
                }
                AudioManager.Instance.PlaySound("Music/jiaohu/duihua");
            }
            IsPlaying = false;
            OnPlayEnd?.Invoke();
        }

        /// <summary>
        /// 判断当前字符是否是标点符号
        /// </summary>
        private bool IsPunctuation(char currentChar)
        {
            return punctuationChars.Contains(currentChar);
        }

        public void Stop()
        {
            if (IsPlaying)
            {
                cts.Cancel();
                IsPlaying = false;
                OnPlayEnd?.Invoke();
            }
        }
    }
}
