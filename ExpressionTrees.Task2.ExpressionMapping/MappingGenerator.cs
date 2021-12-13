using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace ExpressionTrees.Task2.ExpressionMapping
{
    public class MappingGenerator
    {
        public Mapper<TSource, TDestination> Generate<TSource, TDestination>()
        {
            var sourceParam = Expression.Parameter(typeof(TSource), "bar");

            var bindingExpressions = GetBindingExpressions(sourceParam, typeof(TDestination), typeof(TSource));
            var memberInitExpr = Expression.MemberInit(Expression.New(typeof(TDestination)), bindingExpressions);

            var mapFunction = Expression.Lambda<Func<TSource, TDestination>>(memberInitExpr, sourceParam);

            return new Mapper<TSource, TDestination>(mapFunction.Compile());
        }

        private IEnumerable<MemberAssignment> GetBindingExpressions(ParameterExpression sourceParam, Type destType, Type sourceType)
        {
            var sourceFieldAndPropertyNames = GetSourceFieldAndPropertyNames(sourceType);

            var destProperties = GetPropertyInfos(destType, sourceFieldAndPropertyNames);
            var destFields = GetFieldInfos(destType, sourceFieldAndPropertyNames);

            return GetPropertyBindings(destProperties, sourceType, sourceParam).Concat(GetFieldBindings(destFields, sourceType, sourceParam));
        }

        private IEnumerable<string> GetSourceFieldAndPropertyNames(Type sourceType) => sourceType
            .GetProperties()
            .Select(x => x.Name).Concat(sourceType.GetFields().Select(x => x.Name));

        private IEnumerable<MemberAssignment> GetPropertyBindings(IEnumerable<PropertyInfo> destProperties, Type sourceType, ParameterExpression sourceParam)
        {
            foreach (var destProperty in destProperties)
            {
                var sourcePropertyOrFieldType = GetPropertyOrFieldType(sourceType, destProperty.Name);

                if (sourcePropertyOrFieldType == destProperty.PropertyType)
                {
                    yield return Expression.Bind(destProperty, Expression.PropertyOrField(sourceParam, destProperty.Name));
                }
            }
        }

        private IEnumerable<MemberAssignment> GetFieldBindings(IEnumerable<FieldInfo> destFields, Type sourceType, ParameterExpression sourceParam)
        {
            foreach (var destProperty in destFields)
            {
                var sourcePropertyOrFieldType = GetPropertyOrFieldType(sourceType, destProperty.Name);

                if (sourcePropertyOrFieldType == destProperty.FieldType)
                {
                    yield return Expression.Bind(destProperty, Expression.PropertyOrField(sourceParam, destProperty.Name));
                }
            }
        }

        private Type GetPropertyOrFieldType(Type type, string propertyOrFieldName) =>
            type.GetProperty(propertyOrFieldName)?.PropertyType ?? type.GetField(propertyOrFieldName)?.FieldType;

        private IEnumerable<PropertyInfo> GetPropertyInfos(Type destType, IEnumerable<string> sourceFieldAndPropertyNames) =>
            destType.GetProperties().Where(x => sourceFieldAndPropertyNames.Contains(x.Name));

        private IEnumerable<FieldInfo> GetFieldInfos(Type destType, IEnumerable<string> sourceFieldAndPropertyNames) =>
            destType.GetFields().Where(x => sourceFieldAndPropertyNames.Contains(x.Name));
    }
}
