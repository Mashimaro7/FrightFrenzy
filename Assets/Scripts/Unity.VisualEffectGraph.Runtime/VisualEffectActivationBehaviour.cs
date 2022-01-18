using System;
using UnityEngine.Playables;
using UnityEngine.Experimental.VFX.Utility;
using UnityEngine;

[Serializable]
public class VisualEffectActivationBehaviour : PlayableBehaviour
{
	[Serializable]
	public struct EventState
	{
		public ExposedParameter attribute;
		public VisualEffectActivationBehaviour.AttributeType type;
		public float[] values;
	}

	public enum AttributeType
	{
		Float = 1,
		Float2 = 2,
		Float3 = 3,
		Float4 = 4,
		Int32 = 5,
		Uint32 = 6,
		Boolean = 17,
	}

	[SerializeField]
	private ExposedParameter onClipEnter;
	[SerializeField]
	private ExposedParameter onClipExit;
	[SerializeField]
	private EventState[] clipEnterEventAttributes;
	[SerializeField]
	private EventState[] clipExitEventAttributes;
}
