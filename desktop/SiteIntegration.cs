using System;
using System.Windows.Forms;
namespace Videira {
partial class MainForm {
 SitePanel sitePanel;
 void OpenSitePanel(){if(sitePanel==null||sitePanel.IsDisposed)sitePanel=new SitePanel();page="Site";Header("Catálogo do site");content.Controls.Add(sitePanel);sitePanel.BringToFront();}
 protected override void OnFormClosing(FormClosingEventArgs e){if(sitePanel!=null&&!sitePanel.IsDisposed&&!sitePanel.Discard()){e.Cancel=true;return;}base.OnFormClosing(e);}
}
}
