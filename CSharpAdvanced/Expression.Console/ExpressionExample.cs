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
                var propertyId = typeof(Person).GetProperty("Id")!; //id
                //b.p.Id  通过parameterExpression来获取 调用Id
                MemberExpression idExp = Expression.Property(parameterExpression, propertyId);
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
                    Id = 11
                });
                Console.WriteLine(bResult1);
            }
            {
                // 拼接多个条件 --从右往左拼
                Expression<Func<Person, bool>> predicate = p =>
                p.Id.ToString() == "11"
                && p.Name.Equals("Person")
                && p.Age > 18;

                // 1.p.Age > 18;
                ParameterExpression parameterExpression = Expression.Parameter(typeof(Person), "p");
                var age = typeof(Person).GetProperty("Age")!;
                MemberExpression ageExpression = Expression.Property(parameterExpression, age);
                ConstantExpression constant18 = Expression.Constant(10, typeof(int));
                var greateThanExp = Expression.GreaterThan(ageExpression, constant18);

                // p.Name.Equals("Person")
                ConstantExpression constantrichard = Expression.Constant("Person", typeof(string));
                PropertyInfo name = typeof(Person).GetProperty("Name")!;
                var nameExpression = Expression.Property(parameterExpression, name);
                MethodInfo equals = typeof(string).GetMethod("Equals", new Type[] { typeof(string) })!; //todo
                var callEqualsExpression = Expression.Call(nameExpression, equals, constantrichard);

                // p.Id.ToString() == "11"
                ConstantExpression constantExpression10 = Expression.Constant("11", typeof(string));
                PropertyInfo propertyId = typeof(Person).GetProperty("Name")!;
                var idExpression = Expression.Property(parameterExpression, propertyId);

                MethodInfo toString = typeof(int).GetMethod("ToString", new Type[0])!;

                var toStringExp = Expression.Call(idExpression, toString, Array.Empty<Expression>());

                var idEqual = Expression.Equal(toStringExp, constantExpression10);

                var and1 = Expression.AndAlso(idEqual, callEqualsExpression);
                var and2 = Expression.AndAlso(and1, greateThanExp);

                Expression<Func<Person, bool>> predicate1 = Expression.Lambda<Func<Person, bool>>(and2, new ParameterExpression[]
                {
                     parameterExpression
                });

                Func<Person, bool> func1 = predicate1.Compile();

                bool bResult1 = func1.Invoke(new Person()
                {
                    Id = 11,
                    Name = "Person",
                    Age = 20
                });
            }

            #endregion


            {
                // 目的动态拼接条件,灵活编程

                ////SELECT* FROM USER WHERE   name like ""  and  age=10; 
                //// 数据库查询-----拼接Sql语句；
                ////以前根据用户输入拼装条件
                //string sql = "SELECT * FROM USER WHERE 1=1";
                //Console.WriteLine("用户输入个名称，为空就跳过");
                //string name = Console.ReadLine();
                //if (!string.IsNullOrWhiteSpace(name))
                //{
                //    sql += $" and name like '%{name}%'";
                //}
                //Console.WriteLine("用户输入个账号，为空就跳过");
                //string account = Console.ReadLine();
                //if (!string.IsNullOrWhiteSpace(account))
                //{
                //    sql += $" and account like '%{account}%'";
                //}
            }

            {
                // Linq查询，拼接表达式目录树，解析表达式目录树转换成sql语句到数据库中执行

                var dbSet = new List<Person>().AsQueryable();
                var result = dbSet.Where(p => p.Age == 25 && p.Name.Contains("Person"));
                //Expression<Func<Person, bool>> predicate = p => p.Age == 25 && p.Name.Contains("Person"); 
                Expression<Func<Person, bool>>? exp = null;
                Console.WriteLine("用户输入个名称，为空就跳过");
                string? name = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(name))
                {
                    exp = p => p.Name.Contains(name);
                }
                Console.WriteLine("用户输入个最小年纪，为空就跳过");
                string? age = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(age) && int.TryParse(age, out int iAge))
                {
                    exp = p => p.Age > iAge;
                }
            }
        }

        public static void ExpressionMapper()
        {
            // Person 转换到 PersonDto
            // 1.硬编码 2.反射 3.可以直接new一个对象 4.序列化 5.类库(AutoMapper)

            {
                // 1.硬编码 new一个新对象  优点: 性能好，缺点:不灵活
                Person person = new Person()
                {
                    Id = 1,
                    Name = "Person",
                    Age = 18
                };
                PersonDto dto = new PersonDto()
                {
                    Id = person.Id,
                    Name = person.Name,
                    Age = person.Age
                };

            }
            {
                // 2.反射 灵活 性能不好
                Person person = new Person()
                {
                    Id = 1,
                    Name = "Person",
                    Age = 18
                };
                PersonDto dto =  ReflectionMapper.Mapping<Person, PersonDto>(person);
            }

            {
                // 3. 序列化 反序列化 灵活 性能不好
                Person person = new Person()
                {
                    Id = 1,
                    Name = "Person",
                    Age = 18
                };
                PersonDto? dto = SerializeMapper.Mapping<Person, PersonDto>(person);
            }

            {
                // 4. 表达式目录树拼装硬编码 --普通缓存
                Person person = new Person()
                {
                    Id = 1,
                    Name = "Person",
                    Age = 18
                };
                var dto = ExpressionDictMapper.Mapping<Person, PersonDto>(person);
            }

            {
                // 5. 表达式目录树拼装硬编码  --泛型缓存
                Person person = new Person()
                {
                    Id = 1,
                    Name = "Person",
                    Age = 18
                };

                var dto = ExpressionGenericMapper<Person, PersonDto>.Mapping(person);
            }

            {
                Person person = new Person()
                {
                    Id = 1,
                    Name = "Person",
                    Age = 18
                };
                // 普通委托
                Func<Person, PersonDto> func = DelegateMapping;
                var dto = func(person);

                var func2 = (Person p) => new PersonDto()
                {
                    Id=p.Id,
                    Name=p.Name,
                    Age=p.Age
                };

                var s = func2(person);
            }

        }

        public static PersonDto DelegateMapping(Person person)
        {
            PersonDto dto = new PersonDto()
            {
                Id = person.Id,
                Name = person.Name,
                Age = person.Age,
            };

            return dto;

        }
    }
}
