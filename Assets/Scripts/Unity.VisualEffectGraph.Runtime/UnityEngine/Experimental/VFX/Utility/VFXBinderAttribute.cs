using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXBinderAttribute : PropertyAttribute
	{
		public VFXBinderAttribute(string menuPath)
		{
		}

		public string MenuPath;
	}
}
