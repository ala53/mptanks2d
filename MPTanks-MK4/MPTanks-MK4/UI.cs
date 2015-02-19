using Awesomium.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPTanks_MK4
{
    class UI
    {
        private static WebView webView;

        public static void Initialize(int width, int height)
        {
            if (webView == null)
            {
                WebCore.Initialize(new WebConfig() { });
                webView = WebCore.CreateWebView(width, height,
                    WebCore.CreateWebSession(new WebPreferences() { }), WebViewType.Offscreen);
              
            }
        }

        public static void Update()
        {
        }

        public static void Resize(int newWidth, int newHeight)
        {
            webView.Resize(newWidth, newHeight);
        }

        public static void Draw()
        {
            if (((BitmapSurface)(webView.Surface)).IsDirty)
            {
                //Revalidate the buffer
                ((BitmapSurface)(webView.Surface)).IsDirty = false;
                //Update texture
            }

            //Draw
        }
    }
}
