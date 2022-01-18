using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXInputTouchBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_TouchEnabledParameter;
		[SerializeField]
		protected ExposedParameter m_Parameter;
		[SerializeField]
		protected ExposedParameter m_VelocityParameter;
		public int TouchIndex;
		public Camera Target;
		public float Distance;
		public bool SetVelocity;
	}
}
