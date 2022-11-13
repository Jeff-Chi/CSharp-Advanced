using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateConsole
{
    public class CustomMulticastDelegate
    {
        public void Show()
        {
            Action action = new Action(DoSomeThing);
            action += DoSomeThingStatic;
            action += () => { Console.WriteLine("lambda...");};
            //action -= DoSomeThingStatic;

            action();
        }

        private void DoSomeThing()
        {
            Console.WriteLine("DoSomeThing...");
        }

        private static void DoSomeThingStatic()
        {
            Console.WriteLine("DoSomeThingStatic...");
        }
    }
}
