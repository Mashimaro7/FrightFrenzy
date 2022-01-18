using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXPlaneBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_Parameter;
		public Transform Target;
	}
}
