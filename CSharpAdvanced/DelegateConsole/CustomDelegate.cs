using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateConsole
{

    /// <summary>
    /// 无参数无返回值委托
    /// </summary>
    public delegate void NoReturnNoParaOutClass();
    /// <summary>
    /// 有参数无返回值
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public delegate void NoReturnWithPara(int x,int y);
    public class CustomDelegate
    {
        public static void Show()
        {
            // 实例化委托 最基础用法
            NoReturnNoParaOutClass noReturnNoPara = new NoReturnNoParaOutClass(NoReturnNoParaMehtod);

            // 直接赋值-语法糖
            //NoReturnNoParaOutClass noReturnNoPara1 = NoReturnNoParaMehtod;

            // lambda表达式用法
            NoReturnNoParaOutClass noReturnNoPara2 = () => { Console.WriteLine("lambda表达式"); };

            // 执行委托
            noReturnNoPara.Invoke();
            // 
            noReturnNoPara();

            #region 有参数

            NoReturnWithPara noReturnWithPara = new NoReturnWithPara(NoReturnWithParaMehtod);
            //NoReturnWithPara noReturnWithPara2 = NoReturnWithParaMehtod;
            // lambda表达式
            NoReturnWithPara noReturnWithPara2 = (int x, int u) => { Console.WriteLine("lambda表达式...."); };

            // 有参数委托
            noReturnWithPara.Invoke(1,2);
            noReturnWithPara(1,2);
            // 开启新线程执行委托  BeginInvoke EndInvoke .netcore 之后不支持
            // noReturnWithPara.BeginInvoke(1,2,null,null);
            //ad ad =new ad();
            //noReturnWithPara.EndInvoke(ad);

            #endregion


        }

        public class ad : IAsyncResult
        {
            public object? AsyncState => throw new NotImplementedException();

            public WaitHandle AsyncWaitHandle => throw new NotImplementedException();

            public bool CompletedSynchronously => true;

            public bool IsCompleted => true;
        }

        /// <summary>
        /// 无参数无返回值
        /// </summary>
        private static void NoReturnNoParaMehtod()
        {
            Console.WriteLine("无参数无返回值的方法");
        }

        /// <summary>
        /// 有参数无返回值
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private static void NoReturnWithParaMehtod(int x, int y)
        {
            Console.WriteLine($"x+y: {x + y}");
        }
    }
}
