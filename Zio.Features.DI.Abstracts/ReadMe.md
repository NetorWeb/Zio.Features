## Autofac集成
### 借助Autofac可以实现的功能：
- 根据生命周期特性，分别注册不同生命周期的实例  
- 手动创建作用域获取实例  
- 属性注入获取实例  
- 根据别名获取指定实现统一接口的多个实例
- 自定义数据实例委托工厂，可用于注入自定义数据实例，数据由Dictionary承载
- 拦截器，可用于记录方法调用，声明特性**Intercept**启用