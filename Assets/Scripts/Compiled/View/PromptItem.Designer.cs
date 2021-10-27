using UnityEngine;
using UnityEngine.UI;

//自动生成于：2021/10/27 9:46:00
	public partial class PromptItem
	{

		private Text m_Txt_Text;

		protected override void InitializeComponent()
		{
			ComponentAutoBindTool autoBindTool = gameObject.GetComponent<ComponentAutoBindTool>();

			m_Txt_Text = (Text)autoBindTool.GetBindComponent(0);
		}
	}
