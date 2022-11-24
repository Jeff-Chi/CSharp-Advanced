using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    public class MyExpressionVisitor : ExpressionVisitor
    {
        /// <summary>
        /// 1.Visit方法--访问表达式目录树的入口---分辨是什么类型的表达式目录
        /// 2.调度到更加专业的方法中进一步访问,访问一遍之后，生成一个新的表达式目录
        /// 3.表达式目录树类似二叉树，ExpressionVisitor一直往下访问，一直到叶节点；那就访问了所有的节点
        /// 4.在访问的任何一个环节，都可以拿到对应当前环节的内容(参数名称、参数值。。)，就可以进一步扩展
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public Expression Modify(Expression expression)
        {
            Console.WriteLine(expression.ToString());

            return Visit(expression);
        }

        /// <summary>
        /// 重写父类方法
        /// </summary>
        /// <param name="binaryExpression"></param>
        /// <returns></returns>
        protected override Expression VisitBinary(BinaryExpression binaryExpression)
        {
            // 将表达式类型的加改为减，乘改为除
            if (binaryExpression.NodeType == ExpressionType.Add)
            {
                Expression left = Visit(binaryExpression.Left);
                Expression right = Visit(binaryExpression.Right);
                return Expression.Subtract(left, right);
            }
            else if (binaryExpression.NodeType == ExpressionType.Multiply)
            {
                Expression left = Visit(binaryExpression.Left);
                Expression right = Visit(binaryExpression.Right);
                return Expression.Divide(left, right); 
            }

            return base.VisitBinary(binaryExpression);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            return base.VisitConstant(node);
        }
    }
}
