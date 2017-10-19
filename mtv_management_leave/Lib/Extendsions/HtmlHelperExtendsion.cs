using mtv_management_leave.Lib.Controls;
using mtv_management_leave.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

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

        public static MvcHtmlString vTextAreaFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
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

            var inputHtmlString = htmlHelper.TextAreaFor(expression, dic);
            var labelHtmlString = CreateLabelMvcString(htmlHelper, expression);
            var validateMessageHtmlString = htmlHelper.ValidationMessageFor(expression);

            var dictionary = new Dictionary<string, string>();
            dictionary.Add("{label}", labelHtmlString.ToHtmlString());
            dictionary.Add("{textarea}", inputHtmlString.ToHtmlString());
            dictionary.Add("{validate-message}", validateMessageHtmlString.ToHtmlString());

            return new MvcHtmlString(CombineVaribleToLayout(_textareaTemplate, dictionary));
        }

        //public static MvcHtmlString vDateTimePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //     Expression<Func<TModel, TProperty>> expression,
        //     string placeHolder = null,
        //     object attributes = null,
        //     bool showTitle = true)
        //{
        //    var inputClass = "form-control date-time-picker input-sm ";
        //    return RenderDateTimePicker(htmlHelper, expression, placeHolder, attributes, inputClass, showTitle);
        //}

        //public static MvcHtmlString vDatePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression,
        //    string placeHolder = null,
        //    object attributes = null,
        //    bool showTitle = true)
        //{
        //    var inputClass = "form-control date-picker input-sm ";
        //    return RenderDateTimePicker(htmlHelper, expression, placeHolder, attributes, inputClass, showTitle);
        //}

        public static DateTimePickerControl vDateTimePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression = null, object attributeHtml = null)
        {
            return new DateTimePickerControl(htmlHelper).Binding(expression, attributeHtml);
        }

        //public static MvcHtmlString vTimePickerFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
        //    Expression<Func<TModel, TProperty>> expression,
        //    string placeHolder = null,
        //    object attributes = null,
        //    bool showTitle = true)
        //{
        //    var inputClass = "form-control time-picker input-sm ";
        //    return RenderDateTimePicker(htmlHelper, expression, placeHolder, attributes, inputClass, showTitle);
        //}

        public static MvcHtmlString vPasswordFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, TProperty>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            var inputHtmlString = htmlHelper.PasswordFor(expression, new { @class = "form-control input-sm", placeholder = metadata.GetPlaceHolder() });
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


        /*<div class="radio m-b-15">
                                <label>
                                    <input type="radio" name="sample" value="">
                                    <i class="input-helper"></i>
                                    Option one is this and that-be sure to include why it's great
                                </label>
                            </div>*/

        public static MvcHtmlString vRadio(this HtmlHelper htmlHelper, string title, string controlName, bool value = false, object attribute = null)
        {
            var radioTag = new TagBuilder("input")
               .vMergeAttribute("type", "radio")
               .vMergeAttribute("name", controlName)
               .vMergeAttribute("id", controlName)
               //.vMergeAttribute("checked", value.vToString().ToLower())
               .vMergeAttribute("value", value.vToString())
               .vMergeAttributes(attribute.vGetDictionary());
            if (value)
                radioTag.vMergeAttribute("checked", "checked");
            var html = $@"<div class='radio m-b-15'>
                                  <label>  
                                      {radioTag.ToString()}    
                                           <i class='input-helper'></i>
                                    {title}
                                </label>
                            </div>";
            return new MvcHtmlString(html);
        }

        public static MvcHtmlString vRadioFor<TModel>(this HtmlHelper<TModel> htmlHelper,
            Expression<Func<TModel, bool>> expression, object attribute = null)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

            return vRadio(htmlHelper,
                metadata.GetDisplayName(),
                metadata.PropertyName,
                expression.Compile().Invoke((TModel)metadata.Container),
                attribute);

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
            dictionary.Add("{label}",labelHtmlString.ToHtmlString());
            dictionary.Add("{select}", selectHtmlString.ToHtmlString());
            dictionary.Add("{validate-message}", validateMessageHtmlString.ToHtmlString());

            return new MvcHtmlString(CombineVaribleToLayout(_selectTemplate, dictionary));
        }

        public static T GetT<T>(this HtmlHelper htmlHelper, 
            string controller, string action, params object[] parameters)
        {
            DefaultControllerFactory factory = new DefaultControllerFactory();
            var icontroller = factory.CreateController(HttpContext.Current.Request.RequestContext, controller);
            //icontroller.Execute(HttpContext.Current.Request.RequestContext);
            var type = icontroller.GetType();

            var initilizingMethod = type.GetMethod("Initializing");
            if(initilizingMethod != null)
            {
                initilizingMethod.Invoke(icontroller,new[] { HttpContext.Current.Request.RequestContext });
            }            

            var method = type.GetMethod(action, System.Reflection.BindingFlags.IgnoreCase 
                | System.Reflection.BindingFlags.Instance 
                | System.Reflection.BindingFlags.Public);

            if (method == null) throw new NullReferenceException($"Can not find any action with name '{action}'");

            var list = new List<object>();
            var functionParams = method.GetParameters();

            if (parameters.Length < functionParams.Length)
            {
                for (int pIndex = 0; pIndex < functionParams.Length; pIndex++)
                {
                    if (parameters.Length <= pIndex) {
                        try
                        {
                            list.Add(Activator.CreateInstance(functionParams[pIndex].ParameterType));
                        }
                        catch
                        {
                            list.Add(null);
                        }
                    }
                    else
                    {
                        list.Add(parameters[pIndex]);
                    }
                }
            }
            else
            {
                list.AddRange(parameters);
            }            

            var result = method.Invoke(icontroller, list.ToArray());
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

        public static TableHeaderControl vBootGridHeader(this HtmlHelper htmlHelper)
        {
            return new TableHeaderControl();
        }
    }
}
