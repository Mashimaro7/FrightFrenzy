using UnityEngine;

namespace SimpleFileBrowser
{
	public class RecycledListView : MonoBehaviour
	{
		[SerializeField]
		private FileBrowser fileBrowser;
		[SerializeField]
		private RectTransform viewportTransform;
		[SerializeField]
		private RectTransform contentTransform;
	}
}
