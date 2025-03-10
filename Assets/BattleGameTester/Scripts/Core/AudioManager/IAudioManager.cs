namespace BattleGameTester.Core
{
    public interface IAudioManager
    {
        void PlayEffect(EAudio audio);
        void PlayEffect(AttackType type);
        void PlayMusic(EAudio audio, bool isLoop = true);
        void StopMusic(EAudio audio);

        void SetMusicActive(bool isActive);
        void SetEffectsActive(bool isActive);
    }
}
