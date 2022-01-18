using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXHierarchyAttributeMapBinder : VFXBinderBase
	{
		public enum RadiusMode
		{
			Fixed = 0,
			Interpolate = 1,
		}

		[SerializeField]
		protected ExposedParameter m_BoneCount;
		[SerializeField]
		protected ExposedParameter m_PositionMap;
		[SerializeField]
		protected ExposedParameter m_TargetPositionMap;
		[SerializeField]
		protected ExposedParameter m_RadiusPositionMap;
		public Transform HierarchyRoot;
		public float DefaultRadius;
		public uint MaximumDepth;
		public RadiusMode Radius;
	}
}
