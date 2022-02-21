using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Web.WebView2.Core;

namespace SimplestWebView2App
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            
            SetupMouseClickHandlers();

            MouseDoubleClicked += (_, __) => new ZoomedSVGForm{Html = Html}.ShowDialog();
            
            Load += async (_, __) => await RenderHtml();
        }

        void LoadSvg() => SvgText = File.ReadAllText(@"Samples\1.svg");

        private async Task RenderHtml()
        {
            //log.Trace("Starting rendering");
            //log.Trace("    ...waiting for initialized webView2");
            await webView2.EnsureCoreWebView2Async();
            LoadSvg();
            //log.Trace("    ...loading Html:\n        " + Html.Left(100));
            webView2.NavigateToString(IsEmpty ? "" : Html);
        }
        
        private string SvgText;
                
        protected bool IsEmpty => string.IsNullOrEmpty(SvgText);

        private const string htmlStart = @"<html lang=""en"" xmlns=""http: //www.w3.org/1999/xhtml"">";
        private const string htmlHead = @"<head>
<meta charset=""utf-8"" http-equiv=""X-UA-Compatible"" content=""IE=Edge""/>
</head>";
        private const string htmlBodyStart = @"<body style=""margin:0; padding: 0;"">";
        private string svgDivStart => $@"<div id=""SVG"">";
        
        /// <summary>
        /// The script should return JSON like "{"Key":"click","Value":elemId}" where Key=='click' and Value== id of the clicked element
        /// </summary>
        private string clickHandlerScript => $@"<script>
document.addEventListener('click', function (event)
{{
    let elem = event.target;
    let jsonObject =
    {{
        Key: '{clickEventTag}',
        Value: (elem.viewportElement || elem).parentElement.id /* parent div's id */
    }};
    window.chrome.webview.postMessage(jsonObject);
}});
</script>";
        
        protected string Html => 
            $@"<!DOCTYPE html>
{htmlStart}
<style>
    svg {{width: 100%; height: 100%; shape-rendering: geometricprecision !important; 
  /* these props make vertical alignment in the middle: */
  position: absolute;
  top: 50%;
  transform: translateY(-50%); }}
</style>
{clickHandlerScript}
{htmlHead}
{htmlBodyStart}
{svgDivStart}
{SvgText}
</div>
</body> 
</html>
";
        
        #region Mouse Click/DoubleClick

        protected const string clickEventTag = "click";
        
        private Timer _clickTimer;
        private string lastClickedElemId = null;
        
        /// <summary>
        /// Happens when mouse click happens on any html element within the body
        /// </summary>
        public event EventHandler<string> MouseClicked;

        private void FireMouseClicked(string clickedElemId) => MouseClicked?.Invoke(this, clickedElemId);

        /// <summary>
        /// Happens when double mouse click happens within the body
        /// </summary>
        public event EventHandler<string> MouseDoubleClicked;

        private void FireMouseDoubleClicked(string clickedElemId) => MouseDoubleClicked?.Invoke(this, clickedElemId);
        
        void SetupMouseClickHandlers()
        {
            _clickTimer = new Timer {Interval = SystemInformation.DoubleClickTime};
            _clickTimer.Tick += (_, __) => _clickTimer.Stop();
            webView2.WebMessageReceived += WebView2OnWebMessageReceived;
        }
        
        private void WebView2OnWebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            //log.Trace($"Received web message: {e.WebMessageAsJson}"); 

            var jsonAsArray = e.WebMessageAsJson.Split(new[] {'{', '}', ',', ':', '"'}, StringSplitOptions.RemoveEmptyEntries);
            var clickedElemId = jsonAsArray.Length == 4 && jsonAsArray[1] == clickEventTag ? jsonAsArray.Last() : null;

            if (clickedElemId == null) return; // not a click or unknown element or unknown json  
            
            if (_clickTimer.Enabled)
            {
                _clickTimer.Stop();
                if (clickedElemId == lastClickedElemId)
                {
                    //log.Trace("ClickTimer is running, stopping timer, firing Click and DoubleClick");
                    FireMouseClicked(clickedElemId);
                    FireMouseDoubleClicked(clickedElemId);
                }
                else // mouse pointer left previously clicked element before next click
                    restartTimerAndFireMouseClicked("ClickTimer is running, but another element is clicked, restarting timer, firing Click");
            }
            else
                restartTimerAndFireMouseClicked("ClickTimer is stopped, starting timer, firing Click");

            lastClickedElemId = clickedElemId;

            void restartTimerAndFireMouseClicked(string logMsg)
            {
                //log.Trace(logMsg);
                _clickTimer.Start();
                FireMouseClicked(clickedElemId);
            }
        }
        
        #endregion
    }
}