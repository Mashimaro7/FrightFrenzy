using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXEnabledBinder : VFXBinderBase
	{
		public enum Check
		{
			ActiveInHierarchy = 0,
			ActiveSelf = 1,
		}

		public Check check;
		[SerializeField]
		protected ExposedParameter m_Parameter;
		public GameObject Target;
	}
}
