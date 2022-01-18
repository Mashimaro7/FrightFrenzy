using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXMultiplePositionParameterBinder : VFXBinderBase
	{
		public ExposedParameter PositionMapParameter;
		public ExposedParameter PositionCountParameter;
		public GameObject[] Targets;
		public bool EveryFrame;
	}
}
