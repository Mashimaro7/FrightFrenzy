using UnityEngine;
using UnityEngine.UI;

namespace SimpleFileBrowser
{
	public class FileBrowserDeleteConfirmationPanel : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] deletedItems;
		[SerializeField]
		private Image[] deletedItemIcons;
		[SerializeField]
		private Text[] deletedItemNames;
		[SerializeField]
		private GameObject deletedItemsRest;
		[SerializeField]
		private Text deletedItemsRestLabel;
		[SerializeField]
		private RectTransform yesButtonTransform;
		[SerializeField]
		private RectTransform noButtonTransform;
		[SerializeField]
		private float narrowScreenWidth;
	}
}
