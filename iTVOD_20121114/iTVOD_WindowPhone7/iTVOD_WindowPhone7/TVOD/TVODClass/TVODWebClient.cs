using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace iTVOD_WindowPhone7.TVOD.TVODClass
{
    public class TVODWebClient : WebClient
    {
       
            public CookieContainer CookieContainer { get;  set; }

            public TVODWebClient()
            {
                this.CookieContainer = new CookieContainer();
            }

            protected override WebRequest GetWebRequest(Uri address)
            {
                WebRequest request = base.GetWebRequest(address);

                if (request is HttpWebRequest)
                    (request as HttpWebRequest).CookieContainer = this.CookieContainer;

                return request;
            }
        
    }
}
