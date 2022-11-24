using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    public class ReflectionMapper
    {
        public static TOut Mapping<TIn, TOut>(TIn tIn) where TIn : class where TOut : class
        {
            TOut tOut = Activator.CreateInstance<TOut>();

            // 属性 
            foreach (var itemOut in tOut.GetType().GetProperties())
            {
                var propIn = tIn.GetType().GetProperty(itemOut.Name);
                itemOut.SetValue(tOut, propIn?.GetValue(tIn));
            }

            // 字段
            foreach (var itemOut in tOut.GetType().GetFields())
            {
                var fieldIn = tIn.GetType().GetField(itemOut.Name);
                itemOut.SetValue(tOut, fieldIn?.GetValue(tIn));
            }
            return tOut;
        }
    }
}
