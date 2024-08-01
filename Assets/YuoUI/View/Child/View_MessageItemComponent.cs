using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using YuoTools.Main.Ecs;
using Sirenix.OdinInspector;
using TMPro;

namespace YuoTools.UI
{

	public partial class View_MessageItemComponent : UIComponent 
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

		private VerticalLayoutGroup mainVerticalLayoutGroup;

		public VerticalLayoutGroup MainVerticalLayoutGroup
		{
			get
			{
				if (mainVerticalLayoutGroup == null)
					mainVerticalLayoutGroup = rectTransform.GetComponent<VerticalLayoutGroup>();
				return mainVerticalLayoutGroup;
			}
		}

		private ContentSizeFitter mainContentSizeFitter;

		public ContentSizeFitter MainContentSizeFitter
		{
			get
			{
				if (mainContentSizeFitter == null)
					mainContentSizeFitter = rectTransform.GetComponent<ContentSizeFitter>();
				return mainContentSizeFitter;
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


		private ContentSizeFitter mContentSizeFitter_Text;

		public ContentSizeFitter ContentSizeFitter_Text
		{
			get
			{
				if (mContentSizeFitter_Text == null)
					mContentSizeFitter_Text = rectTransform.Find("C_Text").GetComponent<ContentSizeFitter>();
				return mContentSizeFitter_Text;
			}
		}



		[FoldoutGroup("ALL")]

		public List<Image> all_Image = new();

		[FoldoutGroup("ALL")]

		public List<VerticalLayoutGroup> all_VerticalLayoutGroup = new();

		[FoldoutGroup("ALL")]

		public List<ContentSizeFitter> all_ContentSizeFitter = new();

		[FoldoutGroup("ALL")]

		public List<TextMeshProUGUI> all_TextMeshProUGUI = new();

		public void FindAll()
		{
				
			all_Image.Add(MainImage);;
				
			all_VerticalLayoutGroup.Add(MainVerticalLayoutGroup);;
				
			all_ContentSizeFitter.Add(MainContentSizeFitter);
			all_ContentSizeFitter.Add(ContentSizeFitter_Text);;
				
			all_TextMeshProUGUI.Add(TextMeshProUGUI_Text);;


		}
	}}
