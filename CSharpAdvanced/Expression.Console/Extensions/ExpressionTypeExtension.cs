using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree.Extension
{
    public static class ExpressionTypeExtension
    {
        public static string ToSqlOperator(this ExpressionType type)
        {
            string sqlOperator = string.Empty;
            switch (type)
            {
                case ExpressionType.And:
                case ExpressionType.AndAlso:
                    sqlOperator = "AND";
                    break;
                case ExpressionType.Or:
                case ExpressionType.OrElse:
                    sqlOperator = "OR";
                    break;
                case (ExpressionType.Not):
                    sqlOperator = "NOT";
                    break;
                case (ExpressionType.NotEqual):
                    sqlOperator = "<>";
                    break;
                case ExpressionType.GreaterThan:
                    sqlOperator = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    sqlOperator = ">=";
                    break;
                case ExpressionType.LessThan:
                    sqlOperator = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    sqlOperator = "<=";
                    break;
                case (ExpressionType.Equal):
                    sqlOperator = "=";
                    break;
                default: throw new Exception("不支持该方法");
            }
            return sqlOperator;
        }
    }
}
