using UnityEngine;
using UnityEngine.UI;

//自动生成于：2021/10/27 14:33:34
	public partial class PromptForm
	{

		private Image m_Img_Sprite;
		private Text m_Txt_Label;
		private Button m_Btn_Open;
		private ScrollRect m_SRect_Scroll;
		private RectTransform m_Tr_Content;

		protected override void InitializeComponent()
		{
			ComponentAutoBindTool autoBindTool = gameObject.GetComponent<ComponentAutoBindTool>();

			m_Img_Sprite = (Image)autoBindTool.GetBindComponent(0);
			m_Txt_Label = (Text)autoBindTool.GetBindComponent(1);
			m_Btn_Open = (Button)autoBindTool.GetBindComponent(2);
			m_SRect_Scroll = (ScrollRect)autoBindTool.GetBindComponent(3);
			m_Tr_Content = (RectTransform)autoBindTool.GetBindComponent(4);
		}
	}
