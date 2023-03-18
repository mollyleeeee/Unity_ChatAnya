# Unity_ChatAnya
基于Unity+ChatGPT+RtVioce的二次元聊天机器人demo
# 简介
基于Unity+ChatGPT+RtVioce的二次元聊天机器人demo。
前端是 Unity 游戏引擎，主体是一个日系女孩形象，我们叫她 Anya，我们可以通过文字和  Anya 交流， Anya 会回答问题，并自动朗读，并根据提问作出不同反应（开心，难过等）。
后台接入 OpenAI API 服务，目前使用模型：text-davinci-003。总的来说不是很聪明，后续有新的 API 会接入。
## ChatGPT
申请 OpenAI Key
https://platform.openai.com/account/api-keys
注意请求 IP，异常 IP 会被官方 ban。。
## Unity
主体女孩Anya模型来自互联网，包括一个基础动作+多个触发动作。
Anya 会根据 ChatGPT 的回答随机触发不同动作。
## RtVioce
文字朗读插件(https://assetstore.unity.com/packages/tools/audio/rt-voice-pro-41068)。
自动朗读 ChatGPT 回复。
支持多平台：Mac | iOS | Android
支持男声/女生，支持中英日韩等多国语言，见下图：
<img width="300" src="https://github.com/mollyleeeee/Unity_ChatAnya/blob/master/snapshots/snap5.png">


# 示例图
<img width="500" src="https://github.com/mollyleeeee/Unity_ChatAnya/blob/master/snapshots/snap1.png">
<img width="500" src="https://github.com/mollyleeeee/Unity_ChatAnya/blob/master/snapshots/snap2.png">
<img width="500" src="https://github.com/mollyleeeee/Unity_ChatAnya/blob/master/snapshots/snap3.png" >
<img width="500" src="https://github.com/mollyleeeee/Unity_ChatAnya/blob/master/snapshots/snap4.png" >