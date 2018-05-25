using System.Diagnostics;
using CompanyX.Base.Helpers;

namespace CompanyX.Base.Extensions
{
    /// <summary>
    /// Extension methods for string
    /// </summary>
    public static class StringExtensions
    {

        [DebuggerStepThrough]
        public static string FormatWith(this string target, params object[] args)
        {
            Guard.IsNotEmpty(target, () => target);

            return string.Format(provider: Constants.CurrentCulture, format: target, args: args);
        }
    }
}
