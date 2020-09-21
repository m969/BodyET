基于旧版ET6.0，实现了一套实体、组件属性自动同步的流程（类似KBEngine的属性自动同步），另外集成了Box2dSharp物理库，并基于这两个做了一个topdpwn突突突的demo游戏。

基于Odin和ScriptableObject实现了一个protobuf消息协议定义工具。
![ETMessageConfig](https://github.com/m969/BodyET/blob/master/ETMessageConfig.jpg)

添加自动生成客户端和服务端消息处理基类和方法的流程，直接override就可以进入消息处理流程，不需要再新建代码文件、手写消息处理类等。

基于Odin和ScriptableObject实现的灵活的技能、buff配置工具（试验品）。
![技能配置](https://github.com/m969/BodyET/blob/master/SkillConfig.jpg)
![Buff配置](https://github.com/m969/BodyET/blob/master/BuffConfig.jpg)