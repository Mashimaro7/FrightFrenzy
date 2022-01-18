using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXPositionBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_Parameter;
		public Transform Target;
	}
}
