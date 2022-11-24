using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ExpressionTree.Extension;

namespace ExpressionTree
{
    public class ConditionBuilderVisitor : ExpressionVisitor
    {
        private Stack<string> _stack = new Stack<string>();

        public string Condition()
        {
            string condition = string.Concat(_stack.ToArray());
            _stack.Clear();
            return condition;
        }

        /// <summary>
        /// 二元表达式
        /// </summary>
        /// <param name="node">节点</param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression node)
        {
            _stack.Push(")");
            base.Visit(node.Right);
            _stack.Push(" " + node.NodeType.ToSqlOperator() + " ");
            base.Visit(node.Left);
            _stack.Push("(");

            return node;
        }

        /// <summary>
        /// 属性表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            if (node.Expression is ConstantExpression)
            {
                var value1 = InvokeValue(node);
                var value2 = ReflectionValue(node);
                //this.ConditionStack.Push($"'{value1}'");
                _stack.Push("'" + value2 + "'");
            }
            else
            {
                _stack.Push(" [" + node.Member.Name + "] ");
            }
            return node;
        }

        /// <summary>
        /// 常量表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitConstant(ConstantExpression node)
        {
            _stack.Push(" '" + node.Value + "' ");
            return node;
        }


        /// <summary>
        /// 方法表达式
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            string format;
            switch (node.Method.Name)
            {
                case "StartsWith":
                    format = "({0} LIKE {1}+'%')";
                    break;

                case "Contains":
                    format = "({0} LIKE '%'+{1}+'%')";
                    break;

                case "EndsWith":
                    format = "({0} LIKE '%'+{1})";
                    break;

                default:
                    throw new NotSupportedException(node.NodeType + " is not supported!");
            }
            this.Visit(node.Object);
            this.Visit(node.Arguments[0]);
            string right = _stack.Pop();
            string left = _stack.Pop();
            this._stack.Push(string.Format(format, left, right));

            return node;
        }


        #region private methods

        private object InvokeValue(MemberExpression member)
        {
            var objExp = Expression.Convert(member, typeof(object));//struct需要
            return Expression.Lambda<Func<object>>(objExp).Compile().Invoke();
        }

        private object ReflectionValue(MemberExpression member)
        {
            var obj = (member.Expression as ConstantExpression).Value;
            return (member.Member as FieldInfo).GetValue(obj);
        }


        #endregion
    }
}
