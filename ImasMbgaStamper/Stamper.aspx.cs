using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ImasMbgaStamper
{
    public partial class Stamper : System.Web.UI.Page
    {
        /// <summary>スタンプのURL※結局はただの画像</summary>
        private string StampUrl;
        /// <summary>送信先ID</summary>
        private string SendTo;

        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] requestBodyBytes;
            using (StreamReader reader = new StreamReader(Request.InputStream))
            {
                string body = reader.ReadToEnd();
                requestBodyBytes = Encoding.UTF8.GetBytes(body);
            }

            dynamic requestBody = DynamicJson.Parse(Encoding.UTF8.GetString(requestBodyBytes));

            if (requestBody != null)
            {
                StampUrl = requestBody.url;
                SendTo = requestBody.to;

                //スタンプ送信のリクエストボディ
                dynamic pushBody = new DynamicJson();
                pushBody.to = SendTo;
                pushBody.messages = new[] {
                    new {
                        type="image",
                        originalContentUrl=StampUrl,
                        previewImageUrl=StampUrl
                    }
                };

                SendPush(Encoding.UTF8.GetBytes(pushBody.ToString()));

                System.Diagnostics.Trace.WriteLine("stamper: to:" + SendTo + " image:" + StampUrl);
            }

            Response.AppendHeader("Access-Control-Allow-Origin", "*");
            return;
        }

        private void SendPush(byte[] pushBodyBytes)
        {
            string endPointUrl = CommonSettings.EndPointUrl.PUSH_URL;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endPointUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers["Authorization"] = "Bearer " + CommonSettings.ChannelAccessToken;

            request.ContentLength = pushBodyBytes.Length;

            using (Stream pushStream = request.GetRequestStream())
            {
                pushStream.Write(pushBodyBytes, 0, pushBodyBytes.Length);
            }
        }
    }
}