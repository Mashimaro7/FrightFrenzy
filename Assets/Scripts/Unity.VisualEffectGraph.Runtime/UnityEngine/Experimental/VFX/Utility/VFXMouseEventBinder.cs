namespace UnityEngine.Experimental.VFX.Utility
{
	public class VFXMouseEventBinder : VFXEventBinderBase
	{
		public enum Activation
		{
			OnMouseUp = 0,
			OnMouseDown = 1,
			OnMouseEnter = 2,
			OnMouseExit = 3,
			OnMouseOver = 4,
			OnMouseDrag = 5,
		}

		public Activation activation;
		public bool RaycastMousePosition;
	}
}
