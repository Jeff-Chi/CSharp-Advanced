using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ExpressionTree
{
    public class NewExpressionVisitor : ExpressionVisitor
    {
        public ParameterExpression ParameterExpression { get; private set; }

        public NewExpressionVisitor(ParameterExpression _parameterExpression)
        {
            ParameterExpression = _parameterExpression;
        }

        public Expression Replace(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return ParameterExpression;
        }
    }
}
