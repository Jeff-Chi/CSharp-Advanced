// See https://aka.ms/new-console-template for more information
using DelegateConsole;

Console.WriteLine("Hello, World!");

// 1.委托基本用法 委托本质是一个类 有构造函数，参数是一个方法。通过new来实例化
// CustomDelegate.Show();

// 2.委托应用场景(作用及意义)
// 不同国家的人说话不一样
Person person = new Person()
{
    Id =1,
    Name ="Wow"
};

// 实现1 根据不同类型判断调用不同的分支 代码不稳定 添加新的类型或者修改现有内容，需要修改Say方法
//person.Say(PersonType.Chinese);
//person.Say(PersonType.American);
//person.Say(PersonType.Japan);

// 实现2 根据不同类型的人定义单独的方法  添加一个功能，需要在每个方法中写相同的重复代码
//person.SayChinese();
//person.SayAmerican();
//person.SayJapan();

// 实现3 使用委托当参数，直接将要执行的方法当作参数传递。需要修改或添加时，只修改特定的方法而无需修改其他地方，有新功能则在SayDelegateMethod中添加
//person.SayDelegateMethod(person.SayChinese);
//person.SayDelegateMethod(person.SayAmerican);
//person.SayDelegateMethod(person.SayJapan);

// 内置委托
//Console.WriteLine("内置委托...");
//person.SayActionDelegateMethod(person.SayJapan);

// 什么情况下可以使用委托: 方法内部业务逻辑耦合严重，如果多个方法，有很多重复代码--去掉重复代码--逻辑重用

// 3.委托嵌套, ASP.NET CORE的管道核心设计-- 委托的多层嵌套  TODO..


// 4.多播委托,观察者模式
// 多播委托：---任何一个委托都是继承自MulticastDelegate(多播委托)，定义的所有的委托都是多播委托. 使用+=/-= 添加或移动委托，执行委托时依次执行委托绑定的方法

// new CustomMulticastDelegate().Show();

// 5.多播委托应用: Cat Call之后引发的一系列动作
// 5.1 Call 方法调用其他类的方法
var cat = new Cat();
cat.Call();

// 5.2 通过委托属性调用
Console.WriteLine("==========delegate===========");
cat.CallAction += new Dog().Call;
cat.CallAction += new Mouse().Run;

// 执行委托......
cat.CallDelagate();
cat.CallAction();

// 5.3 通过事件字段调用
cat.CallEventAction += new Dog().Call;
cat.CallEventAction += new Mouse().Run;

cat.CallEvent();
// cat.CallEventAction(); 无法在cat外调用
// 委托和事件都保证了执行动作时，保持猫的稳定，把一些可能变化的动作转到上端

// 6. 标准事件的定义
EventStandard.Init();

EventStandard.Show();

EventStandard2.Show();

Console.WriteLine("");
Console.ReadKey();