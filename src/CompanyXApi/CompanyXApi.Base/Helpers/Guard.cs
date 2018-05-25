using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using CompanyX.Base.Extensions;
using CompanyX.Resource;

namespace CompanyX.Base.Helpers
{
    /// <summary>
    /// Guard class
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// check if argument is not empty
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="expr"></param>
        [DebuggerStepThrough]
        public static void IsNotEmpty<T>(string argument, Expression<Func<T>> expr)
        {
            if (string.IsNullOrEmpty((argument ?? string.Empty).Trim()))
            {
                throw new ArgumentException(Global.ArgumentCannotBeBlank.FormatWith(expr.GetParameterName()), expr.GetParameterName());
            }
        }
        /// <summary>
        /// Check if argument isn't null
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="expr"></param>
        [DebuggerStepThrough]
        public static void IsNotNull<T>(object argument, Expression<Func<T>> expr)
        {
            if (argument is null)
            {
                throw new ArgumentNullException(expr.GetParameterName());
            }
        }

        /// <summary>
        /// Check if enumerable has at least one element
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="expr"></param>
        [DebuggerStepThrough]
        public static void IsNotNullOrEmpty<T>(IEnumerable<object> argument, Expression<Func<T>> expr)
        {
            if (!argument.SafeAny())
            {
                throw new ArgumentNullException(Global.ArgumentCannotBeNullOrEmpty.FormatWith(expr.GetParameterName()), expr.GetParameterName());
            }
        }
    }
}
