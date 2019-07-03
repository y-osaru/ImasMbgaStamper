using Codeplex.Data;
using ImasMbgaStamper.Manager;
using ImasMbgaStamper.Manager.ReplyManagers;
using ImasMbgaStamper.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ImasMbgaStamper
{
    public partial class Entry : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                RequestInfoFromLine info = new RequestInfoFromLine(Request);
                
                if (!CheckSignature(info))
                {
                    //不正リクエストなので、即return
                    Response.StatusCode = 200;
                    return;
                }

                //postbackイベントの場合
                if (info.EventType == CommonSettings.EventType.POSTBACK && info.Message == "ok") {
                    SendLeaveRequest(info);
                    return;
                }

                ReplyManager manager = ReplyManagerFactory.Create(info.EventType);

                byte[] replyBodyBytes = manager.CreateReplyBodyBytes(info);

                SendReply(replyBodyBytes);

                System.Diagnostics.Trace.WriteLine("Entry: " + info.FromId);
            }catch(Exception ex) {
                
            }
        }

        private void SendReply(byte[] replyBodyBytes)
        {
            string endPointUrl = CommonSettings.EndPointUrl.REPLY_URL;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endPointUrl);
            request.Method = "POST";
            request.ContentType = "application/json";
            request.Headers["Authorization"] = "Bearer " + CommonSettings.ChannelAccessToken;

            request.ContentLength = replyBodyBytes.Length;

            using (Stream repStream = request.GetRequestStream())
            {
                repStream.Write(replyBodyBytes, 0, replyBodyBytes.Length);
            }
        }

        private void SendLeaveRequest(RequestInfoFromLine info) {
            string endPointUrl = CommonSettings.EndPointUrl.LEAVE_URL_TEMPLATE.Replace("${FromType}",info.FromType).Replace("${FromId}",info.FromId);
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(endPointUrl);
            request.Method = "POST";
            request.Headers["Authorization"] = "Bearer " + CommonSettings.ChannelAccessToken;

            using (Stream repStream = request.GetRequestStream())
            {
                repStream.Write(new byte[] { }, 0, 0);
            }
        }

        /// <summary>
        /// リクエストの署名をチェック
        /// </summary>
        /// <returns></returns>
        private bool CheckSignature(RequestInfoFromLine info)
        {
            string channelSercret = CommonSettings.ChannelSercret;
            byte[] key = Encoding.UTF8.GetBytes(channelSercret);
            HMACSHA256 hmac = new HMACSHA256(key);
            byte[] bs = hmac.ComputeHash(info.RequestBodyBytes);
            hmac.Clear();
            string signature = Convert.ToBase64String(bs);

            return signature == Request.Headers["X-Line-Signature"];
        }
    }
}