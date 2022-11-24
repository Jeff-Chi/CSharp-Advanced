using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    public class SerializeMapper
    {
        /// <summary>
        /// 序列化反序列化方式
        /// </summary>
        /// <typeparam name="TIn"></typeparam>
        /// <typeparam name="TOut"></typeparam>
        public static TOut? Mapping<TIn, TOut>(TIn tIn) where TIn : class where TOut : class
        {
            string strJson = JsonConvert.SerializeObject(tIn);
            return JsonConvert.DeserializeObject<TOut>(strJson);
        }
    }
}
