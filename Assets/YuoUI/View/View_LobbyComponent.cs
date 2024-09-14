using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using YuoTools.Main.Ecs;
using Sirenix.OdinInspector;
using TMPro;

namespace YuoTools.UI
{

	public static partial class ViewType
	{
		public const string Lobby = "Lobby";
	}
	public partial class View_LobbyComponent : UIComponent 
	{

		public static View_LobbyComponent GetView() => UIManagerComponent.Get.GetUIView<View_LobbyComponent>();


		private Button mButton_Close;

		public Button Button_Close
		{
			get
			{
				if (mButton_Close == null)
					mButton_Close = rectTransform.Find("Item/C_Close").GetComponent<Button>();
				return mButton_Close;
			}
		}


		private ScrollRect mScrollRect_LobbyScroll;

		public ScrollRect ScrollRect_LobbyScroll
		{
			get
			{
				if (mScrollRect_LobbyScroll == null)
					mScrollRect_LobbyScroll = rectTransform.Find("Item/C_LobbyScroll").GetComponent<ScrollRect>();
				return mScrollRect_LobbyScroll;
			}
		}


		private VerticalLayoutGroup mVerticalLayoutGroup_Content;

		public VerticalLayoutGroup VerticalLayoutGroup_Content
		{
			get
			{
				if (mVerticalLayoutGroup_Content == null)
					mVerticalLayoutGroup_Content = rectTransform.Find("Item/C_LobbyScroll/Viewport/C_Content").GetComponent<VerticalLayoutGroup>();
				return mVerticalLayoutGroup_Content;
			}
		}


		private ContentSizeFitter mContentSizeFitter_Content;

		public ContentSizeFitter ContentSizeFitter_Content
		{
			get
			{
				if (mContentSizeFitter_Content == null)
					mContentSizeFitter_Content = rectTransform.Find("Item/C_LobbyScroll/Viewport/C_Content").GetComponent<ContentSizeFitter>();
				return mContentSizeFitter_Content;
			}
		}


		private Button mButton_BackSelect;

		public Button Button_BackSelect
		{
			get
			{
				if (mButton_BackSelect == null)
					mButton_BackSelect = rectTransform.Find("Item/C_LobbyScroll/C_BackSelect").GetComponent<Button>();
				return mButton_BackSelect;
			}
		}


		private Button mButton_LobbyRefresh;

		public Button Button_LobbyRefresh
		{
			get
			{
				if (mButton_LobbyRefresh == null)
					mButton_LobbyRefresh = rectTransform.Find("Item/C_LobbyScroll/C_LobbyRefresh").GetComponent<Button>();
				return mButton_LobbyRefresh;
			}
		}


		private VerticalLayoutGroup mVerticalLayoutGroup_SelectLobby;

		public VerticalLayoutGroup VerticalLayoutGroup_SelectLobby
		{
			get
			{
				if (mVerticalLayoutGroup_SelectLobby == null)
					mVerticalLayoutGroup_SelectLobby = rectTransform.Find("Item/C_SelectLobby").GetComponent<VerticalLayoutGroup>();
				return mVerticalLayoutGroup_SelectLobby;
			}
		}


		private Button mButton_Find;

		public Button Button_Find
		{
			get
			{
				if (mButton_Find == null)
					mButton_Find = rectTransform.Find("Item/C_SelectLobby/C_Find").GetComponent<Button>();
				return mButton_Find;
			}
		}


		private Button mButton_Create;

		public Button Button_Create
		{
			get
			{
				if (mButton_Create == null)
					mButton_Create = rectTransform.Find("Item/C_SelectLobby/C_Create").GetComponent<Button>();
				return mButton_Create;
			}
		}


		private TextMeshProUGUI mTextMeshProUGUI_Tip;

		public TextMeshProUGUI TextMeshProUGUI_Tip
		{
			get
			{
				if (mTextMeshProUGUI_Tip == null)
					mTextMeshProUGUI_Tip = rectTransform.Find("C_Tip").GetComponent<TextMeshProUGUI>();
				return mTextMeshProUGUI_Tip;
			}
		}


		private View_LobbyItemComponent mChild_LobbyItem;

		public View_LobbyItemComponent Child_LobbyItem
		{
			get
			{
				if (mChild_LobbyItem == null)
				{
					mChild_LobbyItem = Entity.AddChild<View_LobbyItemComponent>();
					mChild_LobbyItem.Entity.EntityName = "LobbyItem";
					mChild_LobbyItem.rectTransform = rectTransform.Find("Item/D_LobbyItem") as RectTransform;
					mChild_LobbyItem.RunSystem<IUICreate>();
				}
				return mChild_LobbyItem;
			}
		}


		[FoldoutGroup("ALL")]

		public List<Button> all_Button = new();

		[FoldoutGroup("ALL")]

		public List<ScrollRect> all_ScrollRect = new();

		[FoldoutGroup("ALL")]

		public List<VerticalLayoutGroup> all_VerticalLayoutGroup = new();

		[FoldoutGroup("ALL")]

		public List<ContentSizeFitter> all_ContentSizeFitter = new();

		[FoldoutGroup("ALL")]

		public List<TextMeshProUGUI> all_TextMeshProUGUI = new();

		[FoldoutGroup("ALL")]

		public List<View_LobbyItemComponent> all_View_LobbyItemComponent = new();

		public void FindAll()
		{
				
			all_Button.Add(Button_Close);
			all_Button.Add(Button_BackSelect);
			all_Button.Add(Button_LobbyRefresh);
			all_Button.Add(Button_Find);
			all_Button.Add(Button_Create);;
				
			all_ScrollRect.Add(ScrollRect_LobbyScroll);;
				
			all_VerticalLayoutGroup.Add(VerticalLayoutGroup_Content);
			all_VerticalLayoutGroup.Add(VerticalLayoutGroup_SelectLobby);;
				
			all_ContentSizeFitter.Add(ContentSizeFitter_Content);;
				
			all_TextMeshProUGUI.Add(TextMeshProUGUI_Tip);;
				
			all_View_LobbyItemComponent.Add(Child_LobbyItem);;


		}
	}}
