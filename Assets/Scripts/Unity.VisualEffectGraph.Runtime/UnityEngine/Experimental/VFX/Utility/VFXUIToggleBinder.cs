using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXUIToggleBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_Parameter;
		public Toggle Target;
	}
}
