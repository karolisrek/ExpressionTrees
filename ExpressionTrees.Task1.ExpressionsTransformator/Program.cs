/*
 * Create a class based on ExpressionVisitor, which makes expression tree transformation:
 * 1. converts expressions like <variable> + 1 to increment operations, <variable> - 1 - into decrement operations.
 * 2. changes parameter values in a lambda expression to constants, taking the following as transformation parameters:
 *    - source expression;
 *    - dictionary: <parameter name: value for replacement>
 * The results could be printed in console or checked via Debugger using any Visualizer.
 */
using System;
using System.Linq.Expressions;

namespace ExpressionTrees.Task1.ExpressionsTransformer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Expression Visitor for increment/decrement.");
            Console.WriteLine();

            Expression<Func<int, int>> ex = x => x - 1;
            Test(x => x + 1);
            Test(x => x - 1);
            Test(variableName => variableName + 1);
            Test(variableName => variableName - 1);

            Test(x => x + 2);
            Test(x => x - 2);
            Test(variableName => variableName + 2);
            Test(variableName => variableName - 2);

            Console.ReadLine();
        }

        private static void Test(Expression<Func<int, int>> expression)
        {
            Console.WriteLine($"Testing expression - {expression}");

            var expressionVisitor = new IncDecExpressionVisitor();
            var result = expressionVisitor.Visit(expression);

            Console.WriteLine($"Received expression - {result}");
            Console.WriteLine("");
        }
    }
}
