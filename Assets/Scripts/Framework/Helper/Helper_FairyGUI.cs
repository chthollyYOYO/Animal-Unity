using Cysharp.Threading.Tasks;
using FairyGUI;

namespace Framework
{
    public partial class Helper
    {
        public static string GetUIKey(GComponent com) => com.GetType().Name;

        public static string GetUIKey<T>() where T : GComponent => typeof(T).Name;

        public static async UniTask SetAnimation(GLoader3D loader3D, string animationName, float mixDuration = 0, bool loop = true, string skinName = null)
        {
            if (loader3D.spineAnimation == null)
            {
                loader3D.visible = false;
                await UniTask.WaitUntil(() => loader3D.spineAnimation != null);
            }
            if (skinName != null && loader3D.spineAnimation.initialSkinName != skinName)
            {
                loader3D.spineAnimation.initialSkinName = skinName;
                loader3D.spineAnimation.Initialize(true);
            }

            loader3D.visible = true;
            loader3D.spineAnimation.loop = loop;
            var track = loader3D.spineAnimation.state.SetAnimation(0, animationName, loop);
            track.MixDuration = mixDuration;
        }

        public static float GetAnimDuration(GLoader3D loader3D, string animName)
        {
            if (loader3D.spineAnimation != null)
            {
                var data = loader3D.spineAnimation.skeletonDataAsset.GetSkeletonData(false);
                if (data != null)
                {
                    var anim = data.Animations.Find(a => a.Name == animName);
                    if (anim != null)
                        return anim.Duration;
                }
            }
            return 0;
        }

        public static void PlayOneShotSound(string soundRes, float? volumeScale = null)
        {
            if (UIPackage.GetItemAssetByURL(soundRes) is NAudioClip sound && sound.nativeClip != null)
            {
                float volume = volumeScale == null ? UIConfig.buttonSoundVolumeScale : volumeScale.Value;
                Stage.inst.PlayOneShotSound(sound.nativeClip, volume);
            }
        }
    }
}
