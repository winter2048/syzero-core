using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Linq;
using System.Reflection;
using SyZero.Application.Attributes;
using SyZero.Application.Routing;
using SyZero.Application.Service;
using SyZero.DynamicWebApi.Helpers;
using SyZero.Extension;

namespace SyZero.DynamicWebApi
{
    public class DynamicWebApiConvention : IApplicationModelConvention
    {

        public void Apply(ApplicationModel application)
        {
            foreach (var controller in application.Controllers)
            {
                var type = controller.ControllerType.AsType();
                var dynamicWebApiAttr = ReflectionHelper.GetSingleAttributeOrDefaultByFullSearch<DynamicWebApiAttribute>(type.GetTypeInfo());
                if (typeof(IDynamicWebApi).GetTypeInfo().IsAssignableFrom(type))
                {
                    controller.ControllerName = RoutingHelper.GetControllerName(controller.ControllerName);
                    ConfigureArea(controller, dynamicWebApiAttr);
                    ConfigureDynamicWebApi(controller, dynamicWebApiAttr);
                }
                else
                {
                    if (dynamicWebApiAttr != null)
                    {
                        ConfigureArea(controller, dynamicWebApiAttr);
                        ConfigureDynamicWebApi(controller, dynamicWebApiAttr);
                    }
                }

            }
        }

        private void ConfigureArea(ControllerModel controller, DynamicWebApiAttribute attr)
        {
            if (attr == null)
            {
                throw new ArgumentException(nameof(attr));
            }

            if (!controller.RouteValues.ContainsKey("area"))
            {
                if (!string.IsNullOrEmpty(attr.Module))
                {
                    controller.RouteValues["area"] = attr.Module;
                }
                else if (!string.IsNullOrEmpty(AppConsts.DefaultAreaName))
                {
                    controller.RouteValues["area"] = AppConsts.DefaultAreaName;
                }
            }

        }

        private void ConfigureDynamicWebApi(ControllerModel controller, DynamicWebApiAttribute controllerAttr)
        {
            ConfigureApiExplorer(controller);
            ConfigureSelector(controller, controllerAttr);
            ConfigureParameters(controller);
        }


        private void ConfigureParameters(ControllerModel controller)
        {
            foreach (var action in controller.Actions)
            {
                if (!CheckNoMapMethod(action))
                    foreach (var para in action.Parameters)
                    {
                        if (para.BindingInfo != null)
                        {
                            continue;
                        }

                        if (!TypeHelper.IsPrimitiveExtendedIncludingNullable(para.ParameterInfo.ParameterType))
                        {
                            if (CanUseFormBodyBinding(action, para))
                            {
                                para.BindingInfo = BindingInfo.GetBindingInfo(new[] { new FromBodyAttribute() });
                            }
                        }
                    }
            }
        }


        private bool CanUseFormBodyBinding(ActionModel action, ParameterModel parameter)
        {
            if (AppConsts.FormBodyBindingIgnoredTypes.Any(t => t.IsAssignableFrom(parameter.ParameterInfo.ParameterType)))
            {
                return false;
            }

            foreach (var selector in action.Selectors)
            {
                if (selector.ActionConstraints == null)
                {
                    continue;
                }

                foreach (var actionConstraint in selector.ActionConstraints)
                {

                    var httpMethodActionConstraint = actionConstraint as HttpMethodActionConstraint;
                    if (httpMethodActionConstraint == null)
                    {
                        continue;
                    }

                    if (httpMethodActionConstraint.HttpMethods.All(hm => hm.IsIn("GET", "DELETE", "TRACE", "HEAD")))
                    {
                        return false;
                    }
                }
            }

            return true;
        }


        #region ConfigureApiExplorer

        private void ConfigureApiExplorer(ControllerModel controller)
        {
            if (controller.ApiExplorer.GroupName.IsNullOrEmpty())
            {
                controller.ApiExplorer.GroupName = controller.ControllerName;
            }

            if (controller.ApiExplorer.IsVisible == null)
            {
                controller.ApiExplorer.IsVisible = true;
            }

            foreach (var action in controller.Actions)
            {
                if (!CheckNoMapMethod(action))
                    ConfigureApiExplorer(action);
            }
        }

        private void ConfigureApiExplorer(ActionModel action)
        {
            if (action.ApiExplorer.IsVisible == null)
            {
                action.ApiExplorer.IsVisible = true;
            }
        }

        #endregion
        /// <summary>
        /// //不映射指定的方法
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private bool CheckNoMapMethod(ActionModel action)
        {
            bool isExist = false;
            
            var noMapMethod = ReflectionHelper.GetSingleAttributeOrDefault<NonDynamicMethodAttribute>(action.ActionMethod.GetInterfaceMemberInfo());

            if (noMapMethod != null)
            {
                action.ApiExplorer.IsVisible = false;//对应的Api不映射
                isExist = true;
            }

            return isExist;
        }
        private void ConfigureSelector(ControllerModel controller, DynamicWebApiAttribute controllerAttr)
        {

            if (controller.Selectors.Any(selector => selector.AttributeRouteModel != null))
            {
                return;
            }

            var areaName = string.Empty;

            if (controllerAttr != null)
            {
                areaName = controllerAttr.Module;
            }

            if (string.IsNullOrEmpty(areaName))
            {
                areaName = AppConsts.DefaultAreaName;
            }

            foreach (var action in controller.Actions)
            {
                if (!CheckNoMapMethod(action))
                    ConfigureSelector(areaName, controller.ControllerName, action);
            }
        }

        private void ConfigureSelector(string areaName, string controllerName, ActionModel action)
        {

            var nonAttr = ReflectionHelper.GetSingleAttributeOrDefault<NonDynamicWebApiAttribute>(action.ActionMethod);

            if (nonAttr != null)
            {
                return;
            }

            if (action.Selectors.IsNullOrEmpty() || action.Selectors.Any(a => a.ActionConstraints.IsNullOrEmpty()))
            {
                if (!CheckNoMapMethod(action))
                    AddAppServiceSelector(areaName, controllerName, action);
            }
            else
            {
                NormalizeSelectorRoutes(areaName, controllerName, action);
            }
        }

        private void AddAppServiceSelector(string areaName, string controllerName, ActionModel action)
        {

            var verb = RoutingHelper.GetHttpVerb(action.ActionMethod);

            var appServiceSelectorModel = action.Selectors[0];

            if (appServiceSelectorModel.AttributeRouteModel == null)
            {
                appServiceSelectorModel.AttributeRouteModel = CreateActionRouteModel(areaName, controllerName, action);
            }

            if (!appServiceSelectorModel.ActionConstraints.Any())
            {
                appServiceSelectorModel.ActionConstraints.Add(new HttpMethodActionConstraint(new[] { verb.ToString() }));
                switch (verb)
                {
                    case HttpMethod.GET:
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpGetAttribute());
                        break;
                    case HttpMethod.POST:
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpPostAttribute());
                        break;
                    case HttpMethod.PUT:
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpPutAttribute());
                        break;
                    case HttpMethod.DELETE:
                        appServiceSelectorModel.EndpointMetadata.Add(new HttpDeleteAttribute());
                        break;
                    default:
                        throw new Exception($"Unsupported http verb: {verb}.");
                }
            }
        }

    

        private static void NormalizeSelectorRoutes(string areaName, string controllerName, ActionModel action)
        {
            foreach (var selector in action.Selectors)
            {
                selector.AttributeRouteModel = selector.AttributeRouteModel == null ?
                     CreateActionRouteModel(areaName, controllerName, action) :
                     AttributeRouteModel.CombineAttributeRouteModel(CreateActionRouteModel(areaName, controllerName, action), selector.AttributeRouteModel);
            }
        }


        private static AttributeRouteModel CreateActionRouteModel(string areaName, string controllerName, ActionModel action)
        {
            var routeStr = RoutingHelper.GetRouteUrl(areaName, controllerName, action.ActionMethod);
            return new AttributeRouteModel(new RouteAttribute(routeStr));
        }
    }
}