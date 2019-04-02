using Website.Controllers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
using System.Web.WebPages;

namespace Website
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Displays the success message from temporary data.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="messageKey">The message key.</param>
        /// <returns></returns>
        public static MvcHtmlString DisplaySuccessMessageFromTempData(this HtmlHelper htmlHelper, string messageKey = BaseController.DefaultTempMessageKey)
        {
            object htmlAttributes = new
            {
                @class = "alert-success"
            };

            var page = (WebViewPage)htmlHelper.ViewDataContainer;

            return DisplayAlert(htmlHelper, page.TempData[messageKey]?.ToString(), htmlAttributes);
        }


        /// <summary>
        /// Displays the alert.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="message">The message.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <returns></returns>
        public static MvcHtmlString DisplayAlert(this HtmlHelper htmlHelper, string message, object htmlAttributes = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                return new MvcHtmlString("");
            }

            var div = new TagBuilder("div");
            div.MergeAttributes(new RouteValueDictionary(htmlAttributes), true);
            div.AddCssClass("alert");
            div.InnerHtml = message;

            return new MvcHtmlString(div.ToString());
        }


        /// <summary>
        /// Jqs the grid number column.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="label">The label.</param>
        /// <param name="name">The name.</param>
        /// <param name="suffix">The suffix.</param>
        /// <param name="sortable">if set to <c>true</c> [sortable].</param>
        /// <returns></returns>
        public static MvcHtmlString JqGridNumberColumn(this HtmlHelper htmlHelper, string label, string name, string suffix, bool sortable = true)
        {
            string options = "{ suffix: '" + suffix + "', " + 
                "decimalSeparator: '" + CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator + "', " +
                "thousandsSeparator: \"" + CultureInfo.CurrentCulture.NumberFormat.NumberGroupSeparator + "\", " +
                "defaultValue: '" + decimal.Zero.ToString("n") + "'" +
                "}";
            
            return JqGridColumn(htmlHelper, label, name, false, sortable, "number", options);
        }


        /// <summary>
        /// Jqs the grid column.
        /// </summary>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="label">The label.</param>
        /// <param name="name">The name.</param>
        /// <param name="key">if set to <c>true</c> [key].</param>
        /// <param name="sortable">if set to <c>true</c> [sortable].</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="formatOptions">The format options.</param>
        /// <param name="formatterIsFunction">if set to <c>true</c> [formatter is function].</param>
        /// <returns></returns>
        public static MvcHtmlString JqGridColumn(this HtmlHelper htmlHelper, string label, string name, bool key = false, bool sortable = true, string formatter = null, string formatOptions = null, bool formatterIsFunction = false)
        {
            List<string> attributes = new List<string>();

            if (!string.IsNullOrEmpty(label))
            {
                attributes.Add($"label: '{label}'");
            }

            if (!string.IsNullOrEmpty(name))
            {
                attributes.Add($"name: '{name}'");
            }

            if(key)
            {
                attributes.Add($"key: {key.ToString().ToLower()}");
            }

            attributes.Add($"sortable: {sortable.ToString().ToLower()}");

            if (!string.IsNullOrEmpty(formatter))
            {
                if (formatterIsFunction)
                {
                    attributes.Add($"formatter: {formatter}");
                }
                else
                {
                    attributes.Add($"formatter: '{formatter}'");
                }
            }

            if (!string.IsNullOrEmpty(formatOptions))
            {
                attributes.Add($"formatoptions: {formatOptions}");
            }

            string column = $"{{ {string.Join(",", attributes)} }}";

            return new MvcHtmlString(column);
        }


        /// <summary>
        /// Returns an HTML5 numeric input element.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the value.</typeparam>
        /// <param name="htmlHelper">The HTML helper instance that this method extends.</param>
        /// <param name="expression">An expression that identifies the object that contains the properties to render.</param>
        /// <param name="htmlAttributes">An object that contains the HTML attributes to set for the element.</param>
        /// <returns>An HTML input element whose type attribute is set to "text" for each property in the object that is represented by the expression.</returns>
        public static MvcHtmlString NumericTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, bool readoOnly = false)
        {
            RouteValueDictionary attributes = ReadOnlyAttribute(htmlAttributes, readoOnly);
            attributes.Add("type", "number");
            attributes.Add("step", "any");

            return InputExtensions.TextBoxFor(htmlHelper, expression, attributes);
        }


        /// <summary>
        /// Texts the box for.
        /// </summary>
        /// <typeparam name="TModel">The type of the model.</typeparam>
        /// <typeparam name="TProperty">The type of the property.</typeparam>
        /// <param name="htmlHelper">The HTML helper.</param>
        /// <param name="expression">The expression.</param>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="readoOnly">if set to <c>true</c> [reado only].</param>
        /// <returns></returns>
        public static MvcHtmlString TextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, object htmlAttributes, bool readoOnly = false)
        {
            return InputExtensions.TextBoxFor(htmlHelper, expression, ReadOnlyAttribute(htmlAttributes, readoOnly));
        }


        /// <summary>
        /// Reads the only attribute.
        /// </summary>
        /// <param name="htmlAttributes">The HTML attributes.</param>
        /// <param name="readOnly">if set to <c>true</c> [read only].</param>
        /// <returns></returns>
        private static RouteValueDictionary ReadOnlyAttribute(object htmlAttributes, bool readOnly)
        {
            // get the attributes
            RouteValueDictionary attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);

            // ignore the readonly value from the given html attributes to be sure that the value is always the same
            attributes.Remove("readonly");

            if (readOnly)
            {
                attributes.Add("readonly", "readonly");
            }

            return attributes;
        }
    }
}