using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXInputButtonBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_ButtonParameter;
		[SerializeField]
		protected ExposedParameter m_ButtonSmoothParameter;
		public string ButtonName;
		public float SmoothSpeed;
		public bool UseButtonSmooth;
	}
}
