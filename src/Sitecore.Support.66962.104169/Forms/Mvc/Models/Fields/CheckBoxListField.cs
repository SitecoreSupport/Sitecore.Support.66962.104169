using Sitecore.Data.Items;
using Sitecore.Form.Core.Controls.Data;
using Sitecore.Forms.Mvc.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Sitecore.Support.Forms.Mvc.Validators;

namespace Sitecore.Support.Forms.Mvc.Models.Fields
{
  public class CheckBoxListField : Sitecore.Support.Forms.Mvc.Models.Fields.RadioListField
  {
    public CheckBoxListField(Item item) : base(item)
    {
      this.Initialize();
    }

    public override ControlResult GetResult()
    {
      StringBuilder builder = new StringBuilder();
      StringBuilder builder2 = new StringBuilder();
      foreach (SelectListItem item in from item in this.Items
                                      where item.Selected
                                      select item)
      {
        builder.AppendFormat("<item>{0}</item>", item.Value);
        builder2.AppendFormat("{0}, ", item.Text);
      }
      return new ControlResult(base.ID.ToString(), base.Title, builder.ToString(), builder2.ToString(0, (builder2.Length > 0) ? (builder2.Length - 2) : 0));
    }

    private void Initialize()
    {
      base.InitilaizeItems(this.Value as List<string>);
      string parameter = base.GetParameter(base.ParametersDictionary, "Direction");
      base.Direction = (parameter == null) ? Direction.Vertical : (string.Equals(parameter, "Horizontal", StringComparison.OrdinalIgnoreCase) ? Direction.Horizontal : Direction.Vertical);
    }

    protected override void OnValueUpdated()
    {
    }

    public override List<SelectListItem> Items
    {
      get
      {
        return base.Items;
      }
      set
      {
        base.Items = value;
        this.Value = (from x in this.Items
                      where x.Selected
                      select x.Value).ToList<string>();
      }
    }

    [DynamicRequiredCheckBoxList("IsRequired", ErrorMessage = "The {0} field is required.")]
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
