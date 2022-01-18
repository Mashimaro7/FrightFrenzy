using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXInputMouseBinder : VFXBinderBase
	{
		[SerializeField]
		protected ExposedParameter m_MouseLeftClickParameter;
		[SerializeField]
		protected ExposedParameter m_MouseRightClickParameter;
		[SerializeField]
		protected ExposedParameter m_PositionParameter;
		[SerializeField]
		protected ExposedParameter m_VelocityParameter;
		public Camera Target;
		public float Distance;
		public bool SetVelocity;
		public bool CheckLeftClick;
		public bool CheckRightClick;
	}
}
