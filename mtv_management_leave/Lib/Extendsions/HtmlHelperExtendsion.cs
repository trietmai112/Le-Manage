using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace mtv_management_leave.Lib.Extendsions
{
    public static class HtmlHelperExtendsion
    {
        private readonly static string TextboxTemplate =
            @"<div class='form-group'>
                {label}
                <div class='col-sm-9'>
                    <div class='fg-line'>
                        {textbox}
                        {validate-message}
                    </div>                    
                </div>
              </div>";

        private static string CombineVaribleToLayout(string layout, Dictionary<string, string> dictionary)
        {
            var htmlString = layout;
            foreach(var item in dictionary)
            {
                htmlString = Regex.Replace(htmlString, item.Key, item.Value);
            }
            return htmlString;
        }

        private static MvcHtmlString CreateLabelMvcString<TModel, TProperty>(HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            return htmlHelper.LabelFor(expression, new { @class = "col-sm-3 control-label" });
        }

        public static MvcHtmlString vTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, string placeHolder = null, string dataMask = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            var inputClass = "form-control input-sm" + (string.IsNullOrEmpty(dataMask) ? "" : " input-mask");
            var placeHolderText = string.IsNullOrEmpty(placeHolder) ? metadata.GetPlaceHolder() : placeHolder;

            var dic = new Dictionary<string, object>();
            dic.Add("class", inputClass);
            dic.Add("placeholder", placeHolderText);
            if (!string.IsNullOrEmpty(dataMask)) dic.Add("data-mask", dataMask);

            var inputHtmlString = htmlHelper.TextBoxFor(expression, dic);
            var labelHtmlString = CreateLabelMvcString(htmlHelper, expression);
            var validateMessageHtmlString = htmlHelper.ValidationMessageFor(expression);

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("{label}", labelHtmlString.ToHtmlString());
            dictionary.Add("{textbox}", inputHtmlString.ToHtmlString());
            dictionary.Add("{validate-message}", validateMessageHtmlString.ToHtmlString());

            return new MvcHtmlString(CombineVaribleToLayout(TextboxTemplate, dictionary));
        }

        public static MvcHtmlString vPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            var inputHtmlString = htmlHelper.PasswordFor(expression, new { @class = "form-control", placeholder = metadata.GetPlaceHolder() });
            var labelHtmlString = CreateLabelMvcString(htmlHelper, expression);
            var validateMessageHtmlString = htmlHelper.ValidationMessageFor(expression);

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("{label}", labelHtmlString.ToHtmlString());
            dictionary.Add("{textbox}", inputHtmlString.ToHtmlString());
            dictionary.Add("{validate-message}", validateMessageHtmlString.ToHtmlString());

            return new MvcHtmlString(CombineVaribleToLayout(TextboxTemplate, dictionary));
        }

        private readonly static string CheckBoxTemplate =
            @"<div class='form-group'>
                <div class='{offset}'>
                    <div class='checkbox'>
                        <label>
                            {checkbox}
                            <i class='input-helper'></i>
                            {label}
                        </label>
                    </div>
                    {checkbox-hidden}
                </div>
            </div>";

        public static MvcHtmlString vCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, bool>> expression, int? positionOffset = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
            var checkboxHtmlString = htmlHelper.CheckBoxFor(expression).ToHtmlString();
            var splitcheckbox = checkboxHtmlString.Split(new string[] { "/><" }, StringSplitOptions.None);
            var checkbox = splitcheckbox[0] + "/>";
            var checkboxHidden = "<" + splitcheckbox[1];
            var label = metadata.GetDisplayName();

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("{offset}", positionOffset.HasValue ? "col-sm-offset-" + positionOffset.Value + " col-sm-" + (12 - positionOffset.Value) : "");
            dictionary.Add("{label}", label);
            dictionary.Add("{checkbox}", checkbox);
            dictionary.Add("{name}", metadata.PropertyName);
            dictionary.Add("{checkbox-hidden}", checkboxHidden);
            return new MvcHtmlString(CombineVaribleToLayout(CheckBoxTemplate, dictionary));
        }



    }
}
