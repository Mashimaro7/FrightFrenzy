using UnityEngine;
using UnityEngine.Experimental.VFX;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXEventBinderBase : MonoBehaviour
	{
		[SerializeField]
		protected VisualEffect target;
		public string EventName;
	}
}
