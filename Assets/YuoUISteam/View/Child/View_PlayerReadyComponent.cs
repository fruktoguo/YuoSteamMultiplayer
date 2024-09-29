using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine.UI;
using TMPro;

namespace YuoTools.UI
{

	public partial class View_PlayerReadyComponent : UIComponent 
	{

		private Image mainImage;

		public Image MainImage
		{
			get
			{
				if (mainImage == null)
					mainImage = rectTransform.GetComponent<Image>();
				return mainImage;
			}
		}

		private TextMeshProUGUI mTextMeshProUGUI_PlayerName;

		public TextMeshProUGUI TextMeshProUGUI_PlayerName => mTextMeshProUGUI_PlayerName ??= rectTransform.Find("C_PlayerName").GetComponent<TextMeshProUGUI>();


		private RawImage mRawImage_Head;

		public RawImage RawImage_Head => mRawImage_Head ??= rectTransform.Find("C_Head").GetComponent<RawImage>();


		private Button mButton_Ready;

		public Button Button_Ready => mButton_Ready ??= rectTransform.Find("C_Ready").GetComponent<Button>();


		private TextMeshProUGUI mTextMeshProUGUI_ReadyText;

		public TextMeshProUGUI TextMeshProUGUI_ReadyText => mTextMeshProUGUI_ReadyText ??= rectTransform.Find("C_Ready/C_ReadyText").GetComponent<TextMeshProUGUI>();


		[FoldoutGroup("ALL")]
		public List<Image> all_Image = new();
		[FoldoutGroup("ALL")]
		public List<TextMeshProUGUI> all_TextMeshProUGUI = new();
		[FoldoutGroup("ALL")]
		public List<RawImage> all_RawImage = new();
		[FoldoutGroup("ALL")]
		public List<Button> all_Button = new();

		public void FindAll()
		{
				
			all_Image.Add(MainImage);
				
			all_TextMeshProUGUI.Add(TextMeshProUGUI_PlayerName);
			all_TextMeshProUGUI.Add(TextMeshProUGUI_ReadyText);
				
			all_RawImage.Add(RawImage_Head);
				
			all_Button.Add(Button_Ready);

		}
	}}
