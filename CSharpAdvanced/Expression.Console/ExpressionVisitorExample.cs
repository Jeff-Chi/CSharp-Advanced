using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ExpressionTree.Extension;

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

                ConditionBuilderVisitor visitor = new ConditionBuilderVisitor();

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

            // 表达式拼接
            {
                Expression<Func<Person, bool>> expression1 = x => x.Age > 5;
                Expression<Func<Person, bool>> expression2 = x => x.Id > 5;

                Expression<Func<Person, bool>> expression3 = expression1.And<Person>(expression2);
                Expression<Func<Person, bool>> expression4 = expression1.Or<Person>(expression2);
                Expression<Func<Person, bool>> expression5 = expression1.Not<Person>();


                Console.WriteLine("表达式拼接===========================================");
                var list2 = Do2(expression1);
                foreach (var item in list2)
                {
                    Console.WriteLine(item.Id);
                }

                Console.WriteLine("转换委托===========================================");
                var list1 = Do1(expression1.Compile());
                //Do1(lambda5);
            }


            {
                // 根据条件是否正确 拼接表达式
                //Expression<Func<Person, bool>>? expression = null;

                //string str = "ce";
                //Expression<Func<Person, bool>> lambda = x => x.Age > 5 && x.Name == str || x.Id > 5;
                //Expression<Func<Person, bool>> lambda2 = x => x.Age > 5 && x.Name == "ce" || x.Id > 5;
                //Console.WriteLine("输入名称，为空则跳过");

                //string? name = Console.ReadLine();
                //if (!string.IsNullOrEmpty(name))
                //{
                //    expression = expression!.And(p => p.Name.Contains(name));
                //}

                //Console.WriteLine("用户输入个创建人Id，为空就跳过");

                //string? id = Console.ReadLine();

                //if (!string.IsNullOrWhiteSpace(id) && int.TryParse(id, out int cId))
                //{
                //    expression = expression.And(p => p.Id == cId); ;
                //}

                //SqlServerHelper sqlServer = new SqlServerHelper();
                //List<SysCompany> list = sqlServer.Query<SysCompany>(exp);
            }

            {
                // AND2
                //Expression<Func<Person, bool>>? expression = null;

                //Console.WriteLine("输入名称，为空则跳过");

                //string? name = Console.ReadLine();
                //if (!string.IsNullOrEmpty(name))
                //{
                //    expression = expression!.And2(p => p.Name.Contains(name));
                //}

                //Console.WriteLine("用户输入个创建人Id，为空就跳过");

                //string? id = Console.ReadLine();

                //if (!string.IsNullOrWhiteSpace(id) && int.TryParse(id, out int cId))
                //{
                //    expression = expression.And2(p => p.Id == cId); ;
                //}
            }
        }


        #region private methods

        private static IEnumerable<Person> Do1(Func<Person, bool> func)
        {
            List<Person> people = new List<Person>()
            {
                new Person(){Id=4,Name="123",Age=4},
                new Person(){Id=5,Name="234",Age=5},
                new Person(){Id=6,Name="345",Age=6},
            };
            return people.Where(func);
        }

        private static IEnumerable<Person> Do2(Expression<Func<Person, bool>> expression)
        {
            List<Person> people = new List<Person>()
            {
                new Person(){Id=4,Name="123",Age=4},
                new Person(){Id=5,Name="234",Age=5},
                new Person(){Id=6,Name="345",Age=6},
            };

           return people.Where(expression.Compile()).ToList();
        }

        private static IQueryable<Person> GetQueryable(Expression<Func<Person, bool>> func)
        {
            List<Person> person = new List<Person>()
            {
                new Person(){Id=4,Name="123",Age=4},
                new Person(){Id=5,Name="234",Age=5},
                new Person(){Id=6,Name="345",Age=6},
            };

            return person.AsQueryable<Person>().Where(func);
        }

        #endregion
    }
}
