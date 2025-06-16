using src.Application.Services;
using System.ComponentModel;
using System.Reflection;

namespace src.Application.Utils
{
    public static class ResultHelper
    {
        public static string GetDescription(ServiceResult result)
        {
            var field = result.GetType().GetField(result.ToString());

            if(field is null)
            {
                return result.ToString();
            }

            var attribute = field.GetCustomAttribute<DescriptionAttribute>();

            return attribute?.Description ?? result.ToString();
        }
    }
}
