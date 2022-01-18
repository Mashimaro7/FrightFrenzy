using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXInputKeyBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_KeyParameter;
		[SerializeField]
		protected ExposedParameter m_KeySmoothParameter;
		public KeyCode Key;
		public float SmoothSpeed;
		public bool UseKeySmooth;
	}
}
