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
		public const string Room = "Room";
	}
	public partial class View_RoomComponent : UIComponent 
	{

		public static View_RoomComponent GetView() => UIManagerComponent.Get.GetUIView<View_RoomComponent>();


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


		private ScrollRect mScrollRect_RoomScroll;

		public ScrollRect ScrollRect_RoomScroll
		{
			get
			{
				if (mScrollRect_RoomScroll == null)
					mScrollRect_RoomScroll = rectTransform.Find("Item/Panel/C_RoomScroll").GetComponent<ScrollRect>();
				return mScrollRect_RoomScroll;
			}
		}


		private Button mButton_Send;

		public Button Button_Send
		{
			get
			{
				if (mButton_Send == null)
					mButton_Send = rectTransform.Find("Item/Panel/C_Send").GetComponent<Button>();
				return mButton_Send;
			}
		}


		private TMP_InputField mTMP_InputField_Input;

		public TMP_InputField TMP_InputField_Input
		{
			get
			{
				if (mTMP_InputField_Input == null)
					mTMP_InputField_Input = rectTransform.Find("Item/Panel/C_Input").GetComponent<TMP_InputField>();
				return mTMP_InputField_Input;
			}
		}


		private View_MessageItemComponent mChild_MessageItem;

		public View_MessageItemComponent Child_MessageItem
		{
			get
			{
				if (mChild_MessageItem == null)
				{
					mChild_MessageItem = Entity.AddChild<View_MessageItemComponent>();
					mChild_MessageItem.Entity.EntityName = "MessageItem";
					mChild_MessageItem.rectTransform = rectTransform.Find("Item/Panel/C_RoomScroll/Viewport/Content/D_MessageItem") as RectTransform;
					mChild_MessageItem.RunSystem<IUICreate>();
				}
				return mChild_MessageItem;
			}
		}


		[FoldoutGroup("ALL")]

		public List<Button> all_Button = new();

		[FoldoutGroup("ALL")]

		public List<ScrollRect> all_ScrollRect = new();

		[FoldoutGroup("ALL")]

		public List<TMP_InputField> all_TMP_InputField = new();

		[FoldoutGroup("ALL")]

		public List<View_MessageItemComponent> all_View_MessageItemComponent = new();

		public void FindAll()
		{
				
			all_Button.Add(Button_Close);
			all_Button.Add(Button_Send);;
				
			all_ScrollRect.Add(ScrollRect_RoomScroll);;
				
			all_TMP_InputField.Add(TMP_InputField_Input);;
				
			all_View_MessageItemComponent.Add(Child_MessageItem);;


		}
	}}
