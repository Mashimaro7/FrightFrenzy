using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXLightBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_ColorParameter;
		[SerializeField]
		protected ExposedParameter m_BrightnessParameter;
		[SerializeField]
		protected ExposedParameter m_RadiusParameter;
		public Light Target;
		public bool BindColor;
		public bool BindBrightness;
		public bool BindRadius;
	}
}
