using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using SyZero.Extension;

namespace SyZero.Application.Routing
{
    public class RoutingHelper
    {
        private readonly static List<string> RemoveControllerPostfixes = new List<string>() { "AppService", "ApplicationService" };

        public static HttpMethod GetHttpVerb(MemberInfo member)
        {
            MemberInfo method = member.GetInterfaceMemberInfo();

            var apiMethod = method.GetCustomAttribute<ApiMethodAttribute>();

            return apiMethod?.HttpMethod ?? HttpMethod.GET;
        }

        public static string GetHttpTemplate(MemberInfo member)
        {
            MemberInfo method = member.GetInterfaceMemberInfo();

            var apiMethod = method.GetCustomAttribute<ApiMethodAttribute>();

            return apiMethod?.Template;
        }

        public static string GetApiPreFix()
        {
            return "api";
        }

        public static string GetcontrollerRouteUrl(string areaName, string controllerName)
        {
            var apiPreFix = GetApiPreFix();
            return $"{apiPreFix}/{areaName}/{GetControllerName(controllerName)}".Replace("//", "/");
        }

        public static string GetRouteUrl(string areaName, string controllerName, MemberInfo action)
        {
            var apiPreFix = GetApiPreFix();
            return $"{apiPreFix}/{areaName}/{GetControllerName(controllerName)}/{action.Name}".Replace("//", "/");
        }

        public static string GetRouteUrlByInterface(string areaName, MemberInfo action)
        {
            if (string.IsNullOrEmpty(areaName))
            {
                areaName = action.ReflectedType.Assembly.GetName().Name.Replace(".IApplication","");
            }
            string cname = GetControllerName(action.DeclaringType.FullName.Split('.')[action.DeclaringType.FullName.Split('.').Length - 1]);
            var apiPreFix = GetApiPreFix();
            return $"{apiPreFix}/{areaName}/{cname.Substring(1)}/{action.Name}".Replace("//", "/");
        }

        public static string GetRouteUrlByInterface(string endPoint,string areaName, MemberInfo action)
        {
            return $"{endPoint}/{GetRouteUrlByInterface(areaName, action)}";
        }

        public static string GetControllerName(string controllerName)
        {
            return controllerName.RemovePostFix(RemoveControllerPostfixes.ToArray());
        }
    }
}
