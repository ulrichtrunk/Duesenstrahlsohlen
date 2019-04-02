using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Shared
{
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Gets the property from expression.
        /// </summary>
        /// <example>GetPropertyFromExpression(x => x.Id) -> return the id property of the corresponding type.</example>
        /// <typeparam name="TObject">The type of the object.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="selector">The expression (see example).</param>
        /// <returns>The PropertyInfo-Objekt.</returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public static PropertyInfo GetPropertyFromExpression<TObject, TValue>(this Expression<Func<TObject, TValue>> selector)
        {
            Expression body = selector;

            if (body is LambdaExpression)
                body = ((LambdaExpression)body).Body;

            if (body is UnaryExpression)
                body = ((UnaryExpression)body).Operand;

            if (body != null && body.NodeType == ExpressionType.MemberAccess)
                return (PropertyInfo)((MemberExpression)body).Member;
            else
                throw new InvalidOperationException();
        }


        /// <summary>
        /// Gets the property name from expression.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="forProperty">For property.</param>
        /// <returns></returns>
        public static string GetPropertyNameFromExpression<TModel, TValue>(this Expression<Func<TModel, TValue>> forProperty)
        {
            if (forProperty.Body is MemberExpression)
            {
                return ((MemberExpression)forProperty.Body).Member.Name;
            }
            else
            {
                Expression op = ((UnaryExpression)forProperty.Body).Operand;
                return ((MemberExpression)op).Member.Name;
            }
        }
    }
}