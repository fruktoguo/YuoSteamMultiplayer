using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace YuoTools.UI
{
	public static partial class ViewType
	{
		public const string GameReady = "GameReady";
	}

	public partial class View_GameReadyComponent : UIComponent 
	{

		public static View_GameReadyComponent GetView() => UIManagerComponent.Get.GetUIView<View_GameReadyComponent>();


		private RectTransform mainRectTransform;

		public RectTransform MainRectTransform
		{
			get
			{
				if (mainRectTransform == null)
					mainRectTransform = rectTransform.GetComponent<RectTransform>();
				return mainRectTransform;
			}
		}

		private Button mButton_Mask;

		public Button Button_Mask => mButton_Mask ??= rectTransform.Find("C_Mask").GetComponent<Button>();


		private TextMeshProUGUI mTextMeshProUGUI_Title;

		public TextMeshProUGUI TextMeshProUGUI_Title => mTextMeshProUGUI_Title ??= rectTransform.Find("Item/BackGround/C_Title").GetComponent<TextMeshProUGUI>();


		private Button mButton_StartGame;

		public Button Button_StartGame => mButton_StartGame ??= rectTransform.Find("Item/BackGround/C_StartGame").GetComponent<Button>();


		private Button mButton_Close;

		public Button Button_Close => mButton_Close ??= rectTransform.Find("Item/C_Close").GetComponent<Button>();


		private View_PlayerReadyComponent mChild_PlayerReady;

		public View_PlayerReadyComponent Child_PlayerReady
		{
            get
            {
                if (mChild_PlayerReady == null)
                {
                    mChild_PlayerReady = Entity.AddChild<View_PlayerReadyComponent>();
                    mChild_PlayerReady.Entity.EntityName = "PlayerReady";
                    mChild_PlayerReady.rectTransform = rectTransform.Find("Item/BackGround/PlayerList/D_PlayerReady") as RectTransform;
                    mChild_PlayerReady.RunSystem<IUICreate>();
                }
                return mChild_PlayerReady;
            }
        }

		[FoldoutGroup("ALL")]
		public List<RectTransform> all_RectTransform = new();
		[FoldoutGroup("ALL")]
		public List<Button> all_Button = new();
		[FoldoutGroup("ALL")]
		public List<TextMeshProUGUI> all_TextMeshProUGUI = new();
		[FoldoutGroup("ALL")]
		public List<View_PlayerReadyComponent> all_View_PlayerReadyComponent = new();

		public void FindAll()
		{
				
			all_RectTransform.Add(MainRectTransform);
				
			all_Button.Add(Button_Mask);
			all_Button.Add(Button_StartGame);
			all_Button.Add(Button_Close);
				
			all_TextMeshProUGUI.Add(TextMeshProUGUI_Title);
				
			all_View_PlayerReadyComponent.Add(Child_PlayerReady);

		}
	}}
