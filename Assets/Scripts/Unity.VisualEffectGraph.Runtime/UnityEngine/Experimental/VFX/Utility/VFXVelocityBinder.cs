using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXVelocityBinder : VFXBinderBase
	{
		[SerializeField]
		public ExposedParameter m_Parameter;
		public Transform Target;
	}
}
