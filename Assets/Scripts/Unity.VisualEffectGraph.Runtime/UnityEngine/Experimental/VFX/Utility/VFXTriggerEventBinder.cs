using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXTriggerEventBinder : VFXEventBinderBase
	{
		public enum Activation
		{
			OnEnter = 0,
			OnExit = 1,
			OnStay = 2,
		}

		public List<Collider> colliders;
		public Activation activation;
	}
}
