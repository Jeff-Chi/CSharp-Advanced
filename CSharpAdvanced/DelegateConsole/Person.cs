using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateConsole
{
    public class Person
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 实现1：根据类型做判断
        /// </summary>
        public void Say(PersonType type)
        {
            switch (type)
            {
                case PersonType.Chinese:
                    Console.WriteLine("你好");
                    break;
                case PersonType.American:
                    Console.WriteLine("Hello");
                    break;
                case PersonType.Japan:
                    Console.WriteLine("汪汪汪");
                    break;
                default:
                    throw new Exception("PersonType Not Found");
            }
        }

        public void SayChinese()
        {
            Console.WriteLine("你好");
        }

        public void SayAmerican()
        {
            Console.WriteLine("Hello");
        }

        public void SayJapan()
        {
            Console.WriteLine("汪汪汪");
        }

        /// <summary>
        /// 使用委托作为参数
        /// </summary>
        /// <param name="sayDelegate"></param>
        public void SayDelegateMethod(SayDelegate sayDelegate) 
        {
            // 新功能..
            Console.WriteLine("say之前调用...");

            // 执行传递的方法
            sayDelegate.Invoke();
            sayDelegate();

            Console.WriteLine("say之后调用...");
        }

        /// <summary>
        /// 使用内置委托作为参数
        /// </summary>
        public void SayActionDelegateMethod(Action action)
        {
            // 新功能..
            Console.WriteLine("say之前调用...");

            // 执行传递的方法
            action.Invoke();
            action();

            Console.WriteLine("say之后调用...");
        }

    }

    public delegate void SayDelegate();

    public enum PersonType 
    {
        Chinese,
        American,
        Japan
    }

}
