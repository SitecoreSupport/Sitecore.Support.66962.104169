using Sitecore.Diagnostics;
using Sitecore.Forms.Mvc.Extensions;
using Sitecore.Forms.Mvc.Models;
using Sitecore.Forms.Mvc.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Support.Forms.Mvc.Validators
{
  [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
  public class DynamicRequiredCheckBoxListAttribute : DynamicValidationBase
  {
    public string Property
    {
      get;
      private set;
    }

    public DynamicRequiredCheckBoxListAttribute(string property)
    {
      Assert.ArgumentNotNullOrEmpty(property, "property");
      this.Property = property;
    }

    public override IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
    {
      FieldModel model = base.GetModel(metadata);
      bool? propertyValue = model.GetPropertyValue<bool?>(this.Property);
      if (propertyValue.HasValue && propertyValue.Value)
      {
        string errorMessage = this.FormatErrorMessage(model, new object[0]);
        ModelClientValidationRequiredRule modelClientValidationRequiredRule = new ModelClientValidationRequiredRule(errorMessage);
        yield return modelClientValidationRequiredRule;
      }
      yield break;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
      FieldModel model = base.GetModel(validationContext);
      if (model == null || !model.GetPropertyValue<bool>(this.Property))
      {
        return ValidationResult.Success;
      }
      List<string> list = model.Value as List<string>;
      if (list != null && list.Count > 0)
      {
        if (list.TrueForAll((string x) => !string.IsNullOrWhiteSpace(x)))
        {
          return ValidationResult.Success;
        }
      }
      HttpPostedFileBase httpPostedFileBase = value as HttpPostedFileBase;
      if (httpPostedFileBase != null && httpPostedFileBase.ContentLength > 0)
      {
        return ValidationResult.Success;
      }
      return new ValidationResult(this.FormatErrorMessage(model, new object[0]));
    }
  }
}
