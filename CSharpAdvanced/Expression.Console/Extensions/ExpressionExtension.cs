using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree.Extension
{
    /// <summary>
    /// 表达式扩展
    /// </summary>
    public static class ExpressionExtension
    {
        /// <summary>
        /// expression1 and expression2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression1"></param>
        /// <param name="expression2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            if (expression1 == null)
            {
                return expression2;
            }
            if (expression2 == null)
            {
                return expression1;
            }

            ParameterExpression newParameter = Expression.Parameter(typeof(T), "p");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);
            var left = visitor.Replace(expression1.Body);
            var right = visitor.Replace(expression2.Body);
            var body = Expression.And(left, right);

            return Expression.Lambda<Func<T, bool>>(body, newParameter);

        }

        /// <summary>
        /// expression1 or expression2
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr1"></param>
        /// <param name="expr2"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {

            ParameterExpression newParameter = Expression.Parameter(typeof(T), "p");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);

            var left = visitor.Replace(expression1.Body);
            var right = visitor.Replace(expression2.Body);
            var body = Expression.Or(left, right);
            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }

        /// <summary>
        /// not
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expr"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Not<T>(this Expression<Func<T, bool>> expr)
        {
            var candidateExpr = expr.Parameters[0];
            var body = Expression.Not(expr.Body);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }

        public static Expression<Func<T, bool>> And2<T>(this Expression<Func<T, bool>> expression1, Expression<Func<T, bool>> expression2)
        {
            if (expression1 == null)
            {
                return expression2;
            }
            if (expression2 == null)
            {
                return expression1;
            }

            ParameterExpression newParameter = Expression.Parameter(typeof(T), "p");
            NewExpressionVisitor visitor = new NewExpressionVisitor(newParameter);
            var body = Expression.And(expression1.Body, expression2.Body);

            return Expression.Lambda<Func<T, bool>>(body, newParameter);
        }
    }
}
