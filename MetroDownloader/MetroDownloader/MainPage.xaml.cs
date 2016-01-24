using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace MetroDownloader
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private const string HOMEPAGE = "http://songsdrive.net/";
        public MainPage()
        {
            this.InitializeComponent();
            this.Loaded += MainPage_Loaded;

        }
        /// <summary>
        /// Main Page is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            
            GoTo(HOMEPAGE);
        }

        async void WebViewMain_ScriptNotify(object sender, NotifyEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(e.Value));
        }
        /// <summary>
        /// Load content fisrt and replace a tag to javascript to raise event when user click to a link
        /// because WebView in Windows 8 is not support event Navigating
        /// </summary>
        /// <param name="url"></param>
        async void GoTo(string url)
        {
            HtmlWeb htmlWeb = new HtmlWeb();
            // Using HtmlDocument to parse content to xml linq formart
            HtmlDocument doc = await htmlWeb.LoadFromWebAsync(url);
            foreach (HtmlNode link in doc.DocumentNode.Descendants("a"))
            {
                HtmlAttribute att = link.Attributes["href"];
                att.Value = FixLink(att);
            }

            //WebViewMain.NavigateToString(doc.DocumentNode.OuterHtml);
            WebViewMain.Navigate(new Uri(url));
            WebViewMain.LoadCompleted += WebViewMain_LoadCompleted;
        }

        void WebViewMain_LoadCompleted(object sender, NavigationEventArgs e)
        {
            WebViewMain.AllowedScriptNotifyUris = WebView.AnyScriptNotifyUri;
            WebViewMain.ScriptNotify += WebViewMain_ScriptNotify;
        }
        /// <summary>
        /// Fix link to javascript
        /// </summary>
        /// <param name="att">attribute</param>
        /// <returns>new link</returns>
        private string FixLink(HtmlAttribute att)
        {
            return String.Format("javascript:window.external.notify('{0}')", att.Value);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
    }
}
