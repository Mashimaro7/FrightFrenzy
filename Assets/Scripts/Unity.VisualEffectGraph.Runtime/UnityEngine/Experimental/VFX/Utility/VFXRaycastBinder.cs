using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXRaycastBinder : VFXBinderBase
	{
		public enum Space
		{
			Local = 0,
			World = 1,
		}

		[SerializeField]
		protected ExposedParameter m_TargetPosition;
		[SerializeField]
		protected ExposedParameter m_TargetNormal;
		[SerializeField]
		protected ExposedParameter m_TargetHit;
		public GameObject RaycastSource;
		public Vector3 RaycastDirection;
		public Space RaycastDirectionSpace;
		public LayerMask Layers;
		public float MaxDistance;
	}
}
