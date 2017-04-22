using Sitecore.Data.Items;
using Sitecore.Form.Core.Controls.Data;
using Sitecore.Forms.Mvc.Interfaces;
using System;
using System.ComponentModel;
using System.Linq;
using System.Web.Mvc;
using Sitecore.Support.Forms.Mvc.Validators;

namespace Sitecore.Support.Forms.Mvc.Models.Fields
{
  public class RadioListField : Sitecore.Support.Forms.Mvc.Models.Fields.ListField, IHasColumns
  {
    public RadioListField(Item item) : base(item)
    {
      string parameter = base.GetParameter(base.ParametersDictionary, "Direction");
      this.Direction = (parameter == null) ? Sitecore.Forms.Mvc.Interfaces.Direction.Horizontal : (string.Equals(parameter, "Horizontal", StringComparison.OrdinalIgnoreCase) ? Sitecore.Forms.Mvc.Interfaces.Direction.Horizontal : Sitecore.Forms.Mvc.Interfaces.Direction.Vertical);
    }

    public override ControlResult GetResult()
    {
      SelectListItem item = this.Items.SingleOrDefault<SelectListItem>(x => x.Selected);
      string parameters = (item != null) ? item.Text : string.Empty;
      return new ControlResult(base.ID.ToString(), base.Title, (item != null) ? item.Value : string.Empty, parameters);
    }

    [DefaultValue(1)]
    public int Columns { get; set; }

    public Sitecore.Forms.Mvc.Interfaces.Direction Direction { get; set; }

    public int Rows
    {
      get
      {
        int columns = this.Columns;
        if (columns == 0)
        {
          columns = 1;
        }
        int num2 = this.Items.Count % columns;
        if (num2 <= 0)
        {
          return (this.Items.Count / columns);
        }
        return ((this.Items.Count / columns) + 1);
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
