using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXUIDropdownBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_Parameter;
		public Dropdown Target;
	}
}
