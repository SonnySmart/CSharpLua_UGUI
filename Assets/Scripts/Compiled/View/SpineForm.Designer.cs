using UnityEngine;
using UnityEngine.UI;

//自动生成于：2021/10/27 16:53:07
	public partial class SpineForm
	{

		private Button m_Btn_Button;

		protected override void InitializeComponent()
		{
			ComponentAutoBindTool autoBindTool = gameObject.GetComponent<ComponentAutoBindTool>();

			m_Btn_Button = (Button)autoBindTool.GetBindComponent(0);
		}
	}
