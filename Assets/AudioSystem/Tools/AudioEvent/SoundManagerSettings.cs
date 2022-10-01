using UnityEngine;
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

		public AudioDatabase SoundDatabase { get => m_SoundDataBase; }

	}
}