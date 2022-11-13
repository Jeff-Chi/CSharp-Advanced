using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateConsole
{
    /// <summary>
    /// 猫叫了一声，从而引发了一系列的动作
    /// </summary>
    public class Cat
    {
        /// <summary>
        /// 缺点：
        /// 1. 职责不单一： 一个方法中包含的职责太多，除了Call 依赖其他的类太多
        /// 2. 依赖太严重 --依赖Dog Mouse 任何一个类的修改都可能会影响Cat
        /// </summary>
        public void Call()
        {
            Console.WriteLine($"{GetType().Name} 喵了一声...");
            new Dog().Call();
            new Mouse().Run();
        }

        public Action? CallAction { get; set; }

        public event Action? CallEventAction;

        /// <summary>
        /// 使用委托
        /// 改造：
        /// 1. 职责单一： 仅仅Call 一下，其他的动作不是猫的动作，应该甩出去
        /// 2. 触发的一系列动作，不属于猫，不能放在猫的内部
        /// </summary>
        public void CallDelagate()
        {
            Console.WriteLine($"{GetType().Name} Delegate...");

            CallAction?.Invoke();
        }

        /// <summary>
        /// 使用事件
        /// 相较于委托--只能在类的内部执行--安全
        /// 在类的外面只能+=/-=
        /// 设计：猫的内部：存在不变的逻辑，Call固定在内部； 可变的逻辑；放在外部，通过事件注册动作；
        /// 在执行的时候，只需要执行这个事件即可，给这个事件注册了什么动作，就执行什么动作；
        /// </summary>
        public void CallEvent()
        {
            Console.WriteLine($"{GetType().Name} Event...");
            CallEventAction?.Invoke();
        }



    }
}
