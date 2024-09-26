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
		public const string Wait = "Wait";
	}

	public partial class View_WaitComponent : UIComponent 
	{

		public static View_WaitComponent GetView() => UIManagerComponent.Get.GetUIView<View_WaitComponent>();


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

		private Image mImage_Mask;

		public Image Image_Mask
		{
			get
			{
				if (mImage_Mask == null)
					mImage_Mask = rectTransform.Find("C_Mask").GetComponent<Image>();
				return mImage_Mask;
			}
		}


		private TextMeshProUGUI mTextMeshProUGUI_Text;

		public TextMeshProUGUI TextMeshProUGUI_Text
		{
			get
			{
				if (mTextMeshProUGUI_Text == null)
					mTextMeshProUGUI_Text = rectTransform.Find("C_Text").GetComponent<TextMeshProUGUI>();
				return mTextMeshProUGUI_Text;
			}
		}



		[FoldoutGroup("ALL")]
		public List<RectTransform> all_RectTransform = new();

		[FoldoutGroup("ALL")]
		public List<Image> all_Image = new();

		[FoldoutGroup("ALL")]
		public List<TextMeshProUGUI> all_TextMeshProUGUI = new();

		public void FindAll()
		{
				
			all_RectTransform.Add(MainRectTransform);;
				
			all_Image.Add(Image_Mask);;
				
			all_TextMeshProUGUI.Add(TextMeshProUGUI_Text);;

		}
	}}
