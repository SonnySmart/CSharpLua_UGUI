using UnityEngine;
using UnityEngine.UI;

//自动生成于：2021/10/26 14:36:53
	public partial class PromptForm
	{

		private Image m_Img_Sprite;
		private Text m_Txt_Label;
		private Button m_Btn_Open;
		private RectTransform m_Tr_Grid;

		private void GetBindComponents(GameObject go)
		{
			ComponentAutoBindTool autoBindTool = go.GetComponent<ComponentAutoBindTool>();

			m_Img_Sprite = (Image)autoBindTool.GetBindComponent(0);
			m_Txt_Label = (Text)autoBindTool.GetBindComponent(1);
			m_Btn_Open = (Button)autoBindTool.GetBindComponent(2);
			m_Tr_Grid = (RectTransform)autoBindTool.GetBindComponent(3);
		}
	}
