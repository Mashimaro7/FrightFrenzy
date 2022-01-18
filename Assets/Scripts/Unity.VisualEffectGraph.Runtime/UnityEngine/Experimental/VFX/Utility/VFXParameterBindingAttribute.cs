using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXParameterBindingAttribute : PropertyAttribute
	{
		public VFXParameterBindingAttribute(string[] editorTypes)
		{
		}

		public string[] EditorTypes;
	}
}
