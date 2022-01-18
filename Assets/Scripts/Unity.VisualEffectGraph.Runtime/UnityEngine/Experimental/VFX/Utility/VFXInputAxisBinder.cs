using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXInputAxisBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_AxisParameter;
		public string AxisName;
		public float AccumulateSpeed;
		public bool Accumulate;
	}
}
