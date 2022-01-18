using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXUISliderBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_Parameter;
		public Slider Target;
	}
}
