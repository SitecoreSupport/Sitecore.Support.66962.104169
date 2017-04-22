using Sitecore.Data.Items;
using Sitecore.Form.Core.Utility;
using Sitecore.Forms.Mvc.Attributes;
using Sitecore.Forms.Mvc.Controllers.ModelBinders.FieldBinders;
using Sitecore.Forms.Mvc.Interfaces;
using Sitecore.Forms.Mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sitecore.Support.Forms.Mvc.Models.Fields
{
  public abstract class ListField : FieldModel, ISelectList
  {
    protected ListField(Item item) : base(item)
    {
      this.Initialize();
    }

    private void Initialize()
    {
      string parameter = base.GetParameter(base.ParametersDictionary, "selectedvalue");
      List<string> list = string.IsNullOrEmpty(parameter) ? new List<string>() : ParametersUtil.XmlToStringArray(parameter).ToList<string>();
      this.Value = list;
    }

    protected void InitilaizeItems(List<string> selectedValues)
    {
      this.Items = new List<SelectListItem>();
      string parameter = base.GetParameter(base.ParametersDictionary, "items");
      if (!string.IsNullOrEmpty(parameter))
      {
        using (IEnumerator<KeyValuePair<string, string>> enumerator = ParametersUtil.ItemsValuesXmlToDictionaryList(parameter).GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            Func<string, bool> predicate = null;
            KeyValuePair<string, string> item = enumerator.Current;
            SelectListItem selectListItem = new SelectListItem
            {
              Value = item.Key,
              Text = item.Value
            };
            if (selectedValues != null)
            {
              if (predicate == null)
              {
                predicate = x => string.Equals(item.Key, x, StringComparison.CurrentCultureIgnoreCase);
              }
              if (selectedValues.Any<string>(predicate))
              {
                selectListItem.Selected = true;
              }
            }
            this.Items.Add(selectListItem);
          }
        }
      }
    }

    protected override void OnValueUpdated()
    {
      this.InitilaizeItems(this.Value as List<string>);
    }

    public virtual List<SelectListItem> Items { get; set; }

    [PropertyBinder(typeof(ListFieldValueBinder))]
    public override object Value
    {
      get
      {
        return base.Value;
      }
      set
      {
        base.Value = value;
      }
    }
  }
}
