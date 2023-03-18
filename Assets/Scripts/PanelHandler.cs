using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;
using Crosstales.RTVoice.Tool;
using Crosstales.RTVoice;
using ChatGPTExampleName;

public class PanelHandler : MonoBehaviour {
	TMP_InputField inputContent;
	Button btn;
	TMP_Text resText;
	Animator girl;
	string rule = "Anya";
	// 动画关键词
	string[] keywords = {"a", "e", "i", "o", "u","早上好","晚安"};
	int animationIndex = 0;


	void Start () {
		// 提问输入框
		GameObject requestButtonObj = GameObject.Find("RequestButton");
		btn = requestButtonObj.GetComponent<Button>();
		btn.onClick.AddListener (OnClick);
		// 提问按钮
		GameObject requestInputFieldObj = GameObject.Find("RequestInputField");
		inputContent = requestInputFieldObj.GetComponent<TMP_InputField>();

		// 回答文本
		GameObject responseTextObj = GameObject.Find("ResponseText");
		resText = responseTextObj.GetComponent<TMP_Text>();

		// 通过GameObject.Find方法获取场景中名为"girl"的游戏对象，并获取其Animator组件
		girl = GameObject.Find("girl").GetComponent<Animator>();

	}

	private void OnClick(){
		Debug.Log ("Button Clicked. ClickHandler."+inputContent.text);
		updateResponseText(conbineRule("emmm..."));
		requestGPTSync(inputContent.text);
	}

	// 播放动画状态
	private void playAnimation(float duration) {
		var state = keywords[animationIndex];
		animationIndex = (animationIndex + 1) %keywords.Length;
		girl.CrossFade(state, duration);
	}

	private async void requestGPTSync(string prompt) {
    	// string result = await SocketClient.request(prompt);
		// 请求 GPT API，需要配置Token
		string result = await ChatGPTExample.chat(prompt);
		string content = format(result);
		// 在UI线程中更新文本控件的内容
    	updateResponseText(conbineRule(content));
		// 播放动画状态，并在0.02秒内平滑地将当前动画状态过渡到新的动画状态
		playAnimation(0.02f);
		speak(content);
	}

	// 朗读
	private void speak(string content) {
		Speaker.Speak(content, null, null, true, 1, 1, "", 1);
	}

	// format 返回语句格式
	private string format(string prompt) {
		return prompt.TrimStart('\n');
	}

	// 角色名+回答
	private string conbineRule(string prompt) {
		return rule +":"+"\n"+prompt;
	}

	// 回复面板
	private void updateResponseText(string response) {
		resText.SetText(response);
	}

}