# Unity简介

Unity是一款由Unity Technoligies开发的跨平台2D/3D游戏引擎，并可以发布到Windows, MacOS, Linux, iOS等诸多平台
- Unity官网：https://unity.com/
- Unity官方学习资源：https://learn.unity.com/
- Unity官方文档：https://docs.unity3d.com/Manual/index.html
- Google/Unity Forums

## Unity开发环境

Unity工程及Unity Editor版本管理：Unity Hub
- 图形化编辑界面：Unity Editor
- 下载地址：https://unity3d.com/get-unity/download
- 需要注册Unity账号。非盈利的个人开发者免费使用
- 代码编辑器：Visual Studio（安装时选择"使用Unity的游戏开发"），VS Code（在Unity Editor中修改默认代码编辑器）

注意
- 在一个电脑中不能安装两个及以上版本的unity，否则会产生意想不到的问题

## Unity基本知识

- Scene
  - Scene包含了若干GameObject，一个游戏可以包含多个场景，不同的场景之间是独立的，并且可以通过脚本切换场景
- GameObject
  - 一个GameObject就是场景中的一个对象，比如一个球，一个人；它可以独立地活动。GameObject之间存在Parent和Children的关系
  - GameObject之间的父子关系并非继承关系，而只是一种从属关系
- Component
  - 一个GameObject可以拥有若干个Component，两者之间仍是一种从属关系
  - 每个Component可以独立地实现GameObject需求的某些功能。常用的如Rigid（刚体），Animator（动画）等
- Prefab(个人理解是“副本”)
  - Prefab与GameObject之间的关系类似于类和对象的关系。即后者是前者的实例
  - Prefab的使用能够减少不必要的重复工作。当Prefab被修改时，它的所有GameObject实例也会被修改
- Script
  - 通过编写代码，并将Script添加为某个GameObject的Component，当该GameObject被实例化并且处于活动状态，且该Component也处于活动状态时，它便可以控制GameObject的行为
  - 通常我们使用的Script由C#语言构成
  - Unity脚本是基于Mono的，所有需要使用Unity引擎的脚本，都应该继承自MonoBehaviour类
  - 只有那些完全不依赖于Unity引擎进行初始化和更新，也不调用Unity的API的脚本，比如单纯进行数学计算的类，是不需要继承MonoBehaviour的
- Assets（资源库）
  - 你的所有Prefab, Script以及Material等资源都在Assets中进行管理。对于规模较大的工程，总应该建立规范的目录结构。

## Unity运行方式
- 首先加载一个场景：当场景被加载时，场景中的所有GameObject都会被加载
- 当GameObject加载时，其所有Component也随之实例化
- 当某个Script实例化时，其Awake()方法和Start()方法被先后调用
- 当所有Start()方法都被调用完毕后，游戏会进入第一帧，每隔一段时间就会Update一次
