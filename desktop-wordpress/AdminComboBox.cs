using System;using System.Drawing;using System.Windows.Forms;
namespace Videira {public class AdminComboBox:ComboBox{
 public AdminComboBox(){DrawMode=DrawMode.OwnerDrawFixed;ItemHeight=22;}
 protected override void OnDrawItem(DrawItemEventArgs e){e.DrawBackground();int i=e.Index<0?SelectedIndex:e.Index;string value=i>=0&&i<Items.Count?GetItemText(Items[i]):Text;TextRenderer.DrawText(e.Graphics,value,Font,e.Bounds,e.ForeColor,TextFormatFlags.Left|TextFormatFlags.VerticalCenter|TextFormatFlags.EndEllipsis);e.DrawFocusRectangle();}
}
public partial class VideiraSite {
 System.Collections.Generic.IEnumerable<Control> Walk(Control parent){foreach(Control child in parent.Controls){yield return child;foreach(Control nested in Walk(child))yield return nested;}}
}}
