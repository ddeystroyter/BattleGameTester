using BattleGameTester.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace BattleGameTester.Core
{
    public class AudioManager : MonoBehaviour, IAudioManager
    {
        private List<AudioSource> EffectList = new List<AudioSource>();
        private Dictionary<EAudio, AudioSource> MusicDict = new Dictionary<EAudio, AudioSource>();

        private IResourceManager ResourceManager;
        private AudioMixer MusicAudioMixer;
        private AudioMixer EffectsAudioMixer;

        private void Awake()
        {
            ResourceManager = CompositionRoot.GetResourceManager();

            MusicAudioMixer = ResourceManager.GetAsset<AudioMixer, EComponents>(EComponents.MusicAudioMixer);
            EffectsAudioMixer = ResourceManager.GetAsset<AudioMixer, EComponents>(EComponents.EffectsAudioMixer);
        }

        public void PlayEffect(EAudio audio)
        {
            var effect = ResourceManager.CreatePrefabInstance<AudioSource, EAudio>(audio);

            EffectList.Add(effect);

            StartCoroutine(PlayAndDestroy(effect));
        }
        public void PlayEffect(AttackType type)
        {
            EAudio audio;
            switch (type)
            {
                case AttackType.Melee:
                    audio = EAudio.Combat_MeleeAttack;
                    break;
                case AttackType.Range:
                    audio = EAudio.Combat_RangeAttack;
                    break;
                case AttackType.CC:
                    audio = EAudio.Combat_CCAttack;
                    break;
                default:
                    return;

            }

            var effect = ResourceManager.CreatePrefabInstance<AudioSource, EAudio>(audio);
            EffectList.Add(effect);
            StartCoroutine(PlayAndDestroy(effect));
        }
        public void PlayMusic(EAudio audio, bool isLoop = true)
        {
            var alreadyPlaying = MusicDict.ContainsKey(audio);

            if (alreadyPlaying)
            {
                MusicDict[audio].time = 0f;
                return;
            }

            var music = ResourceManager.CreatePrefabInstance<AudioSource, EAudio>(audio);
            music.loop = isLoop;
            music.Play();

            MusicDict.Add(audio, music);
        }

        public void SetEffectsActive(bool isActive)
        {
            var volume = isActive ? -15f : -80f;
            EffectsAudioMixer.SetFloat("Volume", volume);
        }

        public void SetMusicActive(bool isActive)
        {
            var volume = isActive ? -15f : -80f;
            MusicAudioMixer.SetFloat("Volume", volume);
        }

        public void StopMusic(EAudio audio)
        {
            var music = MusicDict[audio];
            music.Stop();

            MusicDict.Remove(audio);
        }

        private IEnumerator PlayAndDestroy(AudioSource source)
        {
            var length = source.clip.length;

            source.Play();

            yield return new WaitForSeconds(length);

            EffectList.Remove(source);

            GameObject.Destroy(source.gameObject);
        }
    }
}
