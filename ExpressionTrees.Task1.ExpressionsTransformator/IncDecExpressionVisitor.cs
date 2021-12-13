using System.Linq.Expressions;
using System.Reflection.Metadata;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    public class IncDecExpressionVisitor : ExpressionVisitor
    {
        private string name = "";
        private bool isOne = false;
        private bool isAddition = false;
        private bool isSubtraction = false;

        public override Expression Visit(Expression node)
        {
            isOne = false;
            isAddition = false;
            isSubtraction = false;

            var key = node.NodeType.ToString();

            if (node is BinaryExpression binarryNode)
            {
                VisitBinary(binarryNode);
            }

            var ex =  base.Visit(node);

            if (ex is BinaryExpression && isOne)
            {
                var parameter = Expression.Parameter(typeof(int), name);

                if (isAddition)
                {
                    return Expression.Increment(parameter);
                }

                if (isSubtraction)
                {
                    return Expression.Decrement(parameter);
                }
            }

            return ex;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            if (node.Left is ParameterExpression parameterExpression)
            {
                VisitParameter(parameterExpression);
            }

            if (node.Right is ConstantExpression constantExpression)
            {
                VisitConstant(constantExpression);
            }

            if (node.NodeType == ExpressionType.Add)
            {
                isAddition = true;
            }

            if (node.NodeType == ExpressionType.Subtract)
            {
                isSubtraction = true;
            }

            return node;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            if (node.NodeType == ExpressionType.Parameter)
            {
                name = node.Name;
            }

            return node;
        }
        protected override Expression VisitConstant(ConstantExpression node)
        {
            if (node.NodeType == ExpressionType.Constant)
            {
                if (node.Value is int value)
                {
                    isOne = value == 1;
                }
            }

            return node;
        }
        
        //protected override Expression VisitOperation()
    }
}

