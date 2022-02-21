using System.Threading.Tasks;
using System.Windows.Forms;

namespace SimplestWebView2App
{
    public partial class ZoomedSVGForm : Form
    {
        public string Html;
        
        public ZoomedSVGForm()
        {
            InitializeComponent();
            Load += async (_, __) => await RenderHtml();
        }

        private async Task RenderHtml()
        {
            //log.Trace("Starting rendering");
            //log.Trace("    ...waiting for initialized webView2");
            await webView2.EnsureCoreWebView2Async();
            // //log.Trace("    ...loading Html:\n        " + Html.Left(100));
            webView2.NavigateToString(Html);
        }
        
        
    }
}