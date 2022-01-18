using UnityEngine.UI;
using UnityEngine;

namespace SimpleFileBrowser
{
	public class FileBrowserItem : ListItem
	{
		[SerializeField]
		private Image background;
		[SerializeField]
		private Image icon;
		[SerializeField]
		private Image multiSelectionToggle;
		[SerializeField]
		private Text nameText;
	}
}
