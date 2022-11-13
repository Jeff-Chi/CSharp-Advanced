using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateConsole
{
    public class Dog
    {
        public void DoAction()
        {
            this.Call();
        }
        public void Call()
        {
            Console.WriteLine("{0} 汪了一声", this.GetType().Name);
        }
    }
}
