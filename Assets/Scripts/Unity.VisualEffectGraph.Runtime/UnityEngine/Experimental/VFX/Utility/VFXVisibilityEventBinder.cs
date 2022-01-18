namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXVisibilityEventBinder : VFXEventBinderBase
	{
		public enum Activation
		{
			OnBecameVisible = 0,
			OnBecameInvisible = 1,
		}

		public Activation activation;
	}
}
