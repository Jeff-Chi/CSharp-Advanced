using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    public class ExpressionExample
    {
        /// <summary>
        /// Expression和委托
        /// </summary>
        public static void ExpressionTest()
        {
            #region Expression基本使用
            {
                // linq中的Expression和Func<T,T>参数
                List<Person> list = new List<Person>();
                Func<Person, bool> func = p => p.Id > 51;
                // Func<TSource,boo> predicate 委托
                list.Where(func);

                IQueryable<Person> persionQuery = new List<Person>().AsQueryable();
                Expression<Func<Person, bool>> expression = p => p.Id > 51;
                // Expression<Func<TSource, bool>> 表达式 泛型参数是一个委托
                var query = persionQuery.Where(expression);

                // 1.表达式目录树和委托的区别
                // Expression<Func<TSource, bool>> -- Linq to sql  -- 不能带有大括号，只能有一行代码
                // Func<TSource,bool> -- Linq to object
                // 表达式目录树可以转为委托，委托不能转为表达式目录树
                // 表达式目录树-- 是一个类主要属性为body，body中存在left和right两个节点，可以嵌套多层，每一个节点都是一个表达式树
                // lambda表达式声明 表达式树
                Expression<Func<Person, bool>> expression1 = p => p.Id > 51;
                // Expression<Func<TSource, bool>> -> Func<TSource,bool>
                Func<Person, bool> func1 = expression1.Compile();

                bool result = func1.Invoke(new Person()
                {
                    Id = 52,
                    Name = "person"
                });

                // example
                Expression<Func<string, string, string>> expression2 = (x, y) => x + y;
                // 转为委托执行
                var func2 = expression2.Compile();
                func2.Invoke("a", "b");
            }

            #endregion

            #region 动态拼接Expression
            {
                // 基础版

                // 使用lambda声明
                Expression<Func<int>> expression = () => 1 + 2;
                // 常量表达式
                ConstantExpression expression1 = Expression.Constant(1, typeof(int));
                var expression2 = Expression.Constant(2, typeof(int));
                // 二进制表达式 表达式拼接
                BinaryExpression binaryExpression = Expression.Add(expression1, expression2);

                // 最终可以执行的表达式
                Expression<Func<int>> result = Expression.Lambda<Func<int>>(binaryExpression);
                // 转换为委托
                var func = result.Compile();

                var funcResult = func.Invoke();

                Console.WriteLine(funcResult);
            }

            {
                // 带参数
                Expression<Func<int, int>> expression = (x) => x + 2;
                ParameterExpression parameterExpression = Expression.Parameter(typeof(int), "x");
                ConstantExpression constantExpression = Expression.Constant(2, typeof(int));
                BinaryExpression binaryExpression = Expression.Add(parameterExpression, constantExpression);
                Expression<Func<int, int>> result = Expression.Lambda<Func<int, int>>(binaryExpression, new ParameterExpression[] {
                    parameterExpression
                });

                Func<int, int> func = result.Compile();
                Console.WriteLine(func(1));
            }

            {
                // 多个参数
                Expression<Func<int, int, int>> expression = (x, y) => x * y + 2;
                ParameterExpression parameterExpressionX = Expression.Parameter(typeof(int), "x");
                ParameterExpression parameterExpressionY = Expression.Parameter(typeof(int), "y");
                BinaryExpression binaryExpression = Expression.Multiply(parameterExpressionX, parameterExpressionY);
                ConstantExpression constantExpression = Expression.Constant(2, typeof(int));
                BinaryExpression addExpression = Expression.Add(binaryExpression, constantExpression);

                Expression<Func<int, int, int>> result = Expression.Lambda<Func<int, int, int>>(addExpression, new ParameterExpression[] {
                    parameterExpressionX,
                    parameterExpressionY
                });

                var func = result.Compile();
                Console.WriteLine(func(10, 12));
            }

            {
                // 复杂类型
                //1.声明一个变量C；
                ParameterExpression parameterExpression = Expression.Parameter(typeof(Person), "p");
                //2.p.id，调用p.的属性---Person的属性id,先获取属性
                //a.获取属性--反射 
                FieldInfo fieldId = typeof(Person).GetField("Id")!; //id
                //b.p.Id  通过parameterExpression来获取 调用Id
                MemberExpression idExp = Expression.Field(parameterExpression, fieldId);
                ConstantExpression constant10 = Expression.Constant(10, typeof(int));
                //p.id==10;
                Expression expressionExp = Expression.Equal(idExp, constant10);
                Expression<Func<Person, bool>> predicate1 = Expression.Lambda<Func<Person, bool>>(expressionExp, new ParameterExpression[]
                {
                        parameterExpression
                });

                Func<Person, bool> func1 = predicate1.Compile();
                bool bResult1 = func1.Invoke(new Person()
                {
                    Id = 10
                });
                Console.WriteLine(bResult1);
            }
            {
                // 拼接多个条件

            }

            #endregion

        }
    }
}
