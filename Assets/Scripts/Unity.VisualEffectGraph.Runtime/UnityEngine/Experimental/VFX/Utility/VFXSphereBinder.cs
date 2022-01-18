using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXSphereBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_Parameter;
		public SphereCollider Target;
	}
}
