using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MrCMS.Entities.Documents.Web;
using MrCMS.Entities.Documents.Web.FormProperties;
using MrCMS.Settings;

namespace MrCMS.Shortcodes.Forms
{
    public class RadioButtonListRenderer : IFormElementRenderer<RadioButtonList>
    {
        public TagBuilder AppendElement(FormProperty formProperty, string existingValue, FormRenderingType formRenderingType)
        {
            var values = existingValue == null
                             ? new List<string>()
                             : existingValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                            .Select(s => s.Trim())
                                            .ToList();

            var tagBuilder = new TagBuilder("div");
            foreach (var checkbox in formProperty.Options)
            {
                var cbLabelBuilder = new TagBuilder("label");
                cbLabelBuilder.Attributes["for"] = TagBuilder.CreateSanitizedId(formProperty.Name + "-" + checkbox.Value);
                cbLabelBuilder.InnerHtml = checkbox.Value;
                cbLabelBuilder.AddCssClass("radio");

                var checkboxBuilder = new TagBuilder("input");
                checkboxBuilder.Attributes["type"] = "radio";
                checkboxBuilder.Attributes["value"] = checkbox.Value;
                checkboxBuilder.AddCssClass(formProperty.CssClass);

                if (existingValue != null)
                {
                    if (values.Contains(checkbox.Value))
                        checkboxBuilder.Attributes["checked"] = "checked";
                }
                else if (checkbox.Selected)
                    checkboxBuilder.Attributes["checked"] = "checked";

                checkboxBuilder.Attributes["name"] = formProperty.Name;
                checkboxBuilder.Attributes["id"] = TagBuilder.CreateSanitizedId(formProperty.Name + "-" + checkbox.Value);
                cbLabelBuilder.InnerHtml += checkboxBuilder.ToString();
                if (formRenderingType == FormRenderingType.Bootstrap3)
                {
                    var radioContainer = new TagBuilder("div");
                    radioContainer.AddCssClass("radio");
                    radioContainer.InnerHtml += cbLabelBuilder.ToString();
                    tagBuilder.InnerHtml += radioContainer;
                }
                else
                {
                    tagBuilder.InnerHtml += cbLabelBuilder;
                }
            }
            return tagBuilder;
        }
        public bool IsSelfClosing { get { return false; } }
    }
}