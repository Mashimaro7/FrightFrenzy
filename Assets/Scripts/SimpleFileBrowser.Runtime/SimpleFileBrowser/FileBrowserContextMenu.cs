using UnityEngine;
using UnityEngine.UI;

namespace SimpleFileBrowser
{
	public class FileBrowserContextMenu : MonoBehaviour
	{
		[SerializeField]
		private FileBrowser fileBrowser;
		[SerializeField]
		private RectTransform rectTransform;
		[SerializeField]
		private Button selectAllButton;
		[SerializeField]
		private Button deselectAllButton;
		[SerializeField]
		private Button deleteButton;
		[SerializeField]
		private Button renameButton;
		[SerializeField]
		private GameObject selectAllButtonSeparator;
		[SerializeField]
		private float minDistanceToEdges;
	}
}
