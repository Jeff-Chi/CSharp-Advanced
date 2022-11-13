using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DelegateConsole
{
    public class Mouse
    {
        public void Run()
        {
            Console.WriteLine($"{GetType().Name} 跑了..");
        }
    }
}
