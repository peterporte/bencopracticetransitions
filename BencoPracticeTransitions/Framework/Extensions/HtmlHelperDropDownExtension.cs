using BencoPracticeTransitions.Framework.Utility;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq.Expressions;

namespace BencoPracticeTransitions.Framework.Extensions
{
    public static class HtmlHelperDropDownExtension
    {
        public static IHtmlContent DropDownListFor<TModel, TProperty>(this IHtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression, Type enumType, bool useIntValue, bool includeBlank, object htmlAttributes) where TModel : class
        {
            var selected = string.Empty;
            try
            {
                var value = htmlHelper.ViewData.Model == null
                    ? default(TProperty)
                    : expression.Compile()(htmlHelper.ViewData.Model);

                selected = value == null ? string.Empty : value.ToString();
            }
            catch (Exception)
            {
                //do nothing   
            }

            var list = SelectListItemUtility.ConvertEnumToSelectList(enumType, selected, useIntValue, includeBlank);

            return htmlHelper.DropDownListFor(expression, list, htmlAttributes);
        }
    }
}