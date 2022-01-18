using UnityEngine;
using System;
using UnityEngine.UI;

namespace SimpleFileBrowser
{
	public class FileBrowser : MonoBehaviour
	{
		[Serializable]
		private struct QuickLink
		{
			public Environment.SpecialFolder target;
			public string name;
			public Sprite icon;
		}

		[Serializable]
		private struct FiletypeIcon
		{
			public string extension;
			public Sprite icon;
		}

		[SerializeField]
		internal Color normalFileColor;
		[SerializeField]
		internal Color hoveredFileColor;
		[SerializeField]
		internal Color selectedFileColor;
		[SerializeField]
		internal Color wrongFilenameColor;
		[SerializeField]
		internal int minWidth;
		[SerializeField]
		internal int minHeight;
		[SerializeField]
		private float narrowScreenWidth;
		[SerializeField]
		private float quickLinksMaxWidthPercentage;
		[SerializeField]
		private bool sortFilesByName;
		[SerializeField]
		private string[] excludeExtensions;
		[SerializeField]
		private QuickLink[] quickLinks;
		[SerializeField]
		private bool generateQuickLinksForDrives;
		[SerializeField]
		private bool contextMenuShowDeleteButton;
		[SerializeField]
		private bool contextMenuShowRenameButton;
		[SerializeField]
		private bool showResizeCursor;
		[SerializeField]
		private Sprite folderIcon;
		[SerializeField]
		private Sprite driveIcon;
		[SerializeField]
		private Sprite defaultIcon;
		[SerializeField]
		private FiletypeIcon[] filetypeIcons;
		[SerializeField]
		internal Sprite multiSelectionToggleOffIcon;
		[SerializeField]
		internal Sprite multiSelectionToggleOnIcon;
		[SerializeField]
		private FileBrowserMovement window;
		[SerializeField]
		private RectTransform topViewNarrowScreen;
		[SerializeField]
		private RectTransform middleView;
		[SerializeField]
		private RectTransform middleViewQuickLinks;
		[SerializeField]
		private RectTransform middleViewFiles;
		[SerializeField]
		private RectTransform middleViewSeparator;
		[SerializeField]
		private FileBrowserItem itemPrefab;
		[SerializeField]
		private FileBrowserQuickLink quickLinkPrefab;
		[SerializeField]
		private Text titleText;
		[SerializeField]
		private Button backButton;
		[SerializeField]
		private Button forwardButton;
		[SerializeField]
		private Button upButton;
		[SerializeField]
		private InputField pathInputField;
		[SerializeField]
		private RectTransform pathInputFieldSlotTop;
		[SerializeField]
		private RectTransform pathInputFieldSlotBottom;
		[SerializeField]
		private InputField searchInputField;
		[SerializeField]
		private RectTransform quickLinksContainer;
		[SerializeField]
		private RectTransform filesContainer;
		[SerializeField]
		private ScrollRect filesScrollRect;
		[SerializeField]
		private RecycledListView listView;
		[SerializeField]
		private InputField filenameInputField;
		[SerializeField]
		private Text filenameInputFieldOverlayText;
		[SerializeField]
		private Image filenameImage;
		[SerializeField]
		private Dropdown filtersDropdown;
		[SerializeField]
		private RectTransform filtersDropdownContainer;
		[SerializeField]
		private Text filterItemTemplate;
		[SerializeField]
		private Toggle showHiddenFilesToggle;
		[SerializeField]
		private Toggle saveAllMatchesToggle;
		[SerializeField]
		private Text submitButtonText;
		[SerializeField]
		private RectTransform moreOptionsContextMenuPosition;
		[SerializeField]
		private FileBrowserRenamedItem renameItem;
		[SerializeField]
		private FileBrowserContextMenu contextMenu;
		[SerializeField]
		private FileBrowserDeleteConfirmationPanel deleteConfirmationPanel;
		[SerializeField]
		private FileBrowserCursorHandler resizeCursorHandler;
	}
}
