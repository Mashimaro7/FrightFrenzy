using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXTransformBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_Parameter;
		public Transform Target;
	}
}
