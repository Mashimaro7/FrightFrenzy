using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXAudioSpectrumBinder : VFXBinderBase
	{
		public enum AudioSourceMode
		{
			AudioSource = 0,
			AudioListener = 1,
		}

		[SerializeField]
		protected ExposedParameter m_CountParameter;
		[SerializeField]
		protected ExposedParameter m_TextureParameter;
		public FFTWindow FFTWindow;
		public uint Samples;
		public AudioSourceMode Mode;
		public AudioSource AudioSource;
	}
}
