using mtv_management_leave.Lib.Controls;
using mtv_management_leave.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace mtv_management_leave.Lib.Extendsions
{
    public static partial class HtmlHelperExtendsion
    {
        public static MvcHtmlString vTextBoxFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            string placeHolder = null, 
            string dataMask = null,
            string tooltip = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            var inputClass = "form-control input-sm" + (string.IsNullOrEmpty(dataMask) ? "" : " input-mask");
            var placeHolderText = string.IsNullOrEmpty(placeHolder) ? metadata.GetPlaceHolder() : placeHolder;

            var dic = new Dictionary<string, object>();
            dic.Add("class", inputClass);
            dic.Add("placeholder", placeHolderText);
            if (!string.IsNullOrEmpty(tooltip))
            {
                dic.Add("data-toggle", "tool tip");
                dic.Add("data-placement", "top");
                dic.Add("title", tooltip);
            }
            if (!string.IsNullOrEmpty(dataMask)) dic.Add("data-mask", dataMask);

            var inputHtmlString = htmlHelper.TextBoxFor(expression, dic);
            var labelHtmlString = CreateLabelMvcString(htmlHelper, expression);
            var validateMessageHtmlString = htmlHelper.ValidationMessageFor(expression);

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("{label}", labelHtmlString.ToHtmlString());
            dictionary.Add("{textbox}", inputHtmlString.ToHtmlString());
            dictionary.Add("{validate-message}", validateMessageHtmlString.ToHtmlString());

            return new MvcHtmlString(CombineVaribleToLayout(_textboxTemplate, dictionary));
        }

        public static MvcHtmlString vPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            var inputHtmlString = htmlHelper.PasswordFor(expression, new { @class = "form-control", placeholder = metadata.GetPlaceHolder() });
            var labelHtmlString = CreateLabelMvcString(htmlHelper, expression);
            var validateMessageHtmlString = htmlHelper.ValidationMessageFor(expression);

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("{label}", labelHtmlString.ToHtmlString());
            dictionary.Add("{textbox}", inputHtmlString.ToHtmlString());
            dictionary.Add("{validate-message}", validateMessageHtmlString.ToHtmlString());

            return new MvcHtmlString(CombineVaribleToLayout(_textboxTemplate, dictionary));
        }

        public static MvcHtmlString vCheckBoxFor<TModel>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, bool>> expression, int? positionOffset = null)
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
            return new MvcHtmlString(CombineVaribleToLayout(_checkBoxTemplate, dictionary));
        }
        
        public static MvcHtmlString vDropDownListFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TProperty>> expression, 
            IEnumerable<SelectListItem> selectList, 
            bool liveSearch = false,
            bool multiSelect = false)
        {
            var dic = new Dictionary<string, object>();
            dic.Add("class", "selectpicker");
            if (liveSearch) dic.Add("data-live-search", "true");
            if (multiSelect) dic.Add("multiple", "true");

            var selectHtmlString = htmlHelper.DropDownListFor(expression, selectList, dic);
            var labelHtmlString = CreateLabelMvcString(htmlHelper, expression);
            var validateMessageHtmlString = htmlHelper.ValidationMessageFor(expression);

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("{label}", labelHtmlString.ToHtmlString());
            dictionary.Add("{select}", selectHtmlString.ToHtmlString());
            dictionary.Add("{validate-message}", validateMessageHtmlString.ToHtmlString());

            return new MvcHtmlString(CombineVaribleToLayout(_selectTemplate, dictionary));
        }

        public static T GetT<T>(this HtmlHelper htmlHelper, 
            string controller, string action, params object[] parameters)
        {
            DefaultControllerFactory factory = new DefaultControllerFactory();
            var icontroller = factory.CreateController(HttpContext.Current.Request.RequestContext, controller);
            var type = icontroller.GetType();
            var method = type.GetMethod(action, System.Reflection.BindingFlags.IgnoreCase | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            if (method == null) throw new NullReferenceException($"Can not find any action with name '{action}'"); 
            var result = method.Invoke(icontroller, parameters);
            factory.ReleaseController(icontroller);
            return (T)result;
        }

        public static TableControl vBootGridFor(this HtmlHelper htmlHelper)
        {            
            return new TableControl();
        }

        public static CardControl vCardPanel(this HtmlHelper htmlHelper)
        {
            return new CardControl();
        }

        public static MvcHtmlString vCardPanel(this HtmlHelper htmlHelper,
            string header, string headerSummary = null, MvcHtmlString cardBody = null, Dictionary<string, object> action = null)
        {
            var dic = new Dictionary<string, string>();
            dic.Add("{header}", header);
            dic.Add("{headerSummary}", headerSummary);
            dic.Add("{body}", cardBody != null ? cardBody.ToHtmlString() : "");
            dic.Add("{action}", RenderCardActionHtml(action));

            return new MvcHtmlString(CombineVaribleToLayout(_cardPanelTemplate, dic));
        }
    }
}
