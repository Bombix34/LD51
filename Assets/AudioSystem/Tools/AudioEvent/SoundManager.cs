using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools.Audio;
using DG.Tweening;

namespace Tools.Managers
{
	public class SoundManager : Singleton<SoundManager>
	{
        private bool isLaunch=false;
        private AudioSource audioSourceDefault;
        private AudioSource audioSourceMusic;
        private AudioSource audioSourceUI;
        private AudioSource audioSourceEffects;

        [SerializeField] private SoundManagerSettings settings;

        private void Awake()
		{
			//settings = (SoundManagerSettings)Resources.Load("ManagersSettings/SoundManagerSettings", typeof(SoundManagerSettings));

            GameObject audioSourceDefault_GO = new GameObject("SM_AudioSource_Default");
            audioSourceDefault_GO.transform.parent = transform;
            audioSourceDefault = audioSourceDefault_GO.AddComponent<AudioSource>();
            audioSourceDefault.playOnAwake = false;
            audioSourceDefault.spatialBlend = 0;

            GameObject audioSourceMusic_GO = new GameObject("SM_AudioSource_Music");
            audioSourceMusic_GO.transform.parent = transform;
            audioSourceMusic = audioSourceMusic_GO.AddComponent<AudioSource>();
            audioSourceMusic.playOnAwake = false;
            audioSourceMusic.spatialBlend = 0;

            GameObject audioSourceUI_GO = new GameObject("SM_AudioSource_UI");
            audioSourceUI_GO.transform.parent = transform;
            audioSourceUI = audioSourceUI_GO.AddComponent<AudioSource>();
            audioSourceUI.playOnAwake = false;
            audioSourceUI.spatialBlend = 0;

            GameObject audioSourceEffects_GO = new GameObject("SM_AudioSource_Effects");
            audioSourceEffects_GO.transform.parent = transform;
            audioSourceEffects = audioSourceEffects_GO.AddComponent<AudioSource>();
            audioSourceEffects.playOnAwake = false;
            audioSourceEffects.spatialBlend = 0;
        }

        private void Update()
        {
            if(!audioSourceMusic.isPlaying && isLaunch)
                PlayMusic();
        }

        private void OnEnable()
        {
            ScoreBoard.Instance.OnGameOver += OnGameOver;
        }

        private void Ondisable()
        {
            ScoreBoard.Instance.OnGameOver -= OnGameOver;
        }

        public void Launch()
        {
            isLaunch=true;
        }

        private void OnGameOver(int score)
        {
            FadeOutVolume(AudioSourceType.Music, 0.5f);
        }


        public void PlaySound(AudioFieldEnum sound)
        {
            AudioSourceType sourceType = settings.SoundDatabase.GetAudioEvent(sound).audioSourceType;
            settings.SoundDatabase.PlaySound(sound, GetAudioSource(sourceType));
        }

        public void PlayMusic()
        {
            audioSourceMusic.Stop();
            audioSourceMusic.clip = settings.RandomMusicLoop;
            audioSourceMusic.Play();
        }

        public AudioSource GetAudioSource(AudioSourceType audioSourceType)
        {
            switch (audioSourceType)
            {
                case AudioSourceType.Music:
                    return audioSourceMusic;
                case AudioSourceType.UI:
                    return audioSourceUI;
                case AudioSourceType.Effects:
                    return audioSourceEffects;
                default:
                    return audioSourceDefault;
            }
        }

        public void SetVolume(AudioSourceType audioSourceType, float volume)
        {
            GetAudioSource(audioSourceType).volume = volume;
        }

        public void FadeInVolume(AudioSourceType audioSourceType, float time)
        {
            AudioSource source = GetAudioSource(audioSourceType);
            DOTween.To(() => source.volume, x => source.volume = x, 1, time);
        }

        public void FadeOutVolume(AudioSourceType audioSourceType, float time)
        {
            AudioSource source = GetAudioSource(audioSourceType);
            DOTween.To(() => source.volume, x => source.volume = x, 0, time);
        }

        public void StopAudioSource(AudioSourceType audioSourceType)
        {
            GetAudioSource(audioSourceType).Stop();
        }


        public enum AudioSourceType
        {
            Default = 0,
            Music = 1,
            UI = 2,
            Effects = 3
        }
    }
}

