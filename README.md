# Dadis小组Game Jam
## 基础架构
- 分为白天和黑天两个弱场景（即为Prefabs）。
- 两个弱场景在生成后从StreamingAssets中加载Json数据初始化场景，或者使用Global。
- 有一个全局主场景，目前只能用于跳转弱场景。
## 切记
- 不要使用Text Mesh Pro，UI就用Lengacy。
- **白天场景的工作区域：Scripts/Day，Resources/Day。**
- **黑天场景的工作区域：Scripts/Night，Resources/Night。**
- **全局的工作空间在Scripts/General。**
- **Scripts/Frame是Depec常用的扩展脚本。**
- **玩家属性通过MainScene.PlayerState获得。**
- **建议所有的物品（道具、合成表之流）都是脚本化、且继承自Item，Item和Backpack是之前项目的架构，复用性较高。**
## 计划
- Depec 制作黑天场景。
- Ffffishi 制作白天场景。