using QFramework;
using UnityEngine;

namespace SkateGame
{
    public interface IAudioSystem : ISystem
    {
        void PlayTrickSound(string trickName);
        void PlayLandingSound();
        void PlayJumpSound();
    }

    public class AudioSystem : AbstractSystem, IAudioSystem
    {
        protected override void OnInit()
        {
            // 监听音频相关事件
            this.RegisterEvent<TrickPerformedEvent>(OnTrickPerformed);
            this.RegisterEvent<PlayerLandedEvent>(OnPlayerLanded);
        }

        public void PlayTrickSound(string trickName)
        {
            // Debug.Log($"播放技巧音效: {trickName}");
            // 实际的音频播放逻辑
        }

        public void PlayLandingSound()
        {
            // Debug.Log("播放落地音效");
            // 实际的音频播放逻辑
        }

        public void PlayJumpSound()
        {
            // Debug.Log("播放跳跃音效");
            // 实际的音频播放逻辑
        }

        private void OnTrickPerformed(TrickPerformedEvent evt)
        {
            PlayTrickSound(evt.TrickName);
        }

        private void OnPlayerLanded(PlayerLandedEvent evt)
        {
            PlayLandingSound();
        }
    }
}
