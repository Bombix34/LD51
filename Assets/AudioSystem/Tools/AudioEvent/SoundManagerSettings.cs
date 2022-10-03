using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
using Tools.Utils;
#endif
using Tools.Audio;

namespace Tools.Managers
{
	[CreateAssetMenu(menuName ="ManagerSettings/SoundManagerSettings")]
	public class SoundManagerSettings : ScriptableObject
	{

		[SerializeField]
		private AudioDatabase m_SoundDataBase;
		[SerializeField] private List<AudioClip> musicLoops;

		public AudioDatabase SoundDatabase { get => m_SoundDataBase; }

		private int lastLoopIndex=-1;

		public AudioClip RandomMusicLoop
		{
			get 
			{
				int rand = Random.Range(0, musicLoops.Count);
				while(rand == lastLoopIndex)
				{
					rand = Random.Range(0, musicLoops.Count);
				}
				lastLoopIndex = rand;
				return musicLoops[rand];
			}
		}

	}
}