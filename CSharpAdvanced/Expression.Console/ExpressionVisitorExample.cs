using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    /// <summary>
    /// 表达式访问者
    /// 表达式目录树的拆解：通过ExpressionVisitor来进行拆解
    /// ExpressionVisitor中有一个Visit方法：这是访问表达式目录树，解析表达式目录树的一个入口（要解析表达式目把树，都是从这个方法开始）；
    /// Visit方法判断是什么一个什么表达式目录树，把表达式目录树转换表达式；交给更加专业的方法进一步解析；
    /// </summary>
    public class ExpressionVisitorExample
    {

        public static void ExpressionVisitorTest()
        {
            {
                // 例子：修改表达式目录树
                //Expression<Func<int, int, int>> expression = (x, y) => x * y + 2;
                //MyExpressionVisitor visitor = new MyExpressionVisitor();

                //Console.WriteLine(expression);

                //var ex = visitor.Visit(expression);

                //Console.WriteLine(ex.ToString());

                //var ex2 = visitor.Modify(expression);
                //Console.WriteLine(ex2);
            }

            {
                // 解析表达式目录树，提高重用性，通过不断访问解析来生成对应sql语句
                Expression<Func<Person, bool>> expression = x => x.Age > 10
                                                            && x.Id > 10
                                                            && x.Name.StartsWith("p")   //  like 'p%'
                                                            && x.Name.EndsWith("p") // like '%p'
                                                            && x.Name.Contains("p"); // like '%p%'

                // 需要转换成 sql
                string sql = string.Format("Delete From [{0}] WHERE [Age]>5 AND [ID] >5"
                    , typeof(Person).Name
                    , " [Age]>5 AND [ID] >5");

                Console.WriteLine(expression.ToString());

                ConditionBuilderVisitor visitor =new ConditionBuilderVisitor();

                visitor.Visit(expression);

                string sql2 = visitor.Condition();

                Console.WriteLine(sql2);


            }


            {
                //  ((( [Age] > '5') AND( [Name] =  [name] )) OR( [Id] > '5' )) 
                string name = "AAA";
                Expression<Func<Person, bool>> lambda = x => x.Age > 5 && x.Name == name || x.Id > 5;
                ConditionBuilderVisitor vistor = new ConditionBuilderVisitor();
                vistor.Visit(lambda);
                Console.WriteLine(vistor.Condition());
            }
            {
                Console.WriteLine("==================================");
                Expression<Func<Person, bool>> lambda = x => x.Age > 5 || (x.Name == "A" && x.Id > 5);
                ConditionBuilderVisitor vistor = new ConditionBuilderVisitor();
                vistor.Visit(lambda);
                Console.WriteLine(vistor.Condition());
            }
        }
    }
}
