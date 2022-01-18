using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Experimental.VFX;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXParameterBinder : MonoBehaviour
	{
		[SerializeField]
		protected bool m_ExecuteInEditor;
		public List<VFXBinderBase> m_Bindings;
		[SerializeField]
		protected VisualEffect m_VisualEffect;
	}
}
