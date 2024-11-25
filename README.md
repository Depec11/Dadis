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
- **物品需要注册UID，所有的物品在Resources/Ingredients中**

## 计划

- Depec 制作黑天场景。
- Ffffishi 制作白天场景。

## 物品UID

### 规则一：1** 怪兽掉落物或者是宝箱中的物资

- 100 史莱姆凝液
- 101 蝙蝠翅膀
- 102 暗之花
- 103 木头
- 104 木之心
- 105 森林矿

### 规则二：2** 道具

- 200 梦境果
- 201 箭矢
- 202 提灯
- 203 火把
- 204 钥匙

### 规则三：3** 卷轴、铠甲、护盾、武器

- 300 随机卷轴
- 301 护盾卷轴
- 302 显示卷轴
- 303 传送卷轴
- 304 地形无效卷轴
- 305 森林甲
- 306 森林盾
- 307 森林弓
