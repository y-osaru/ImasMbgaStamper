using Codeplex.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace ImasMbgaStamper.Model
{
    /// <summary>LINEからのリクエストをまとめるクラス</summary>
    public class RequestInfoFromLine
    {
        /// <summary>リプライトークン</summary>
        public string ReplyToken;
        /// <summary>イベント種別</summary>
        public string EventType;
        /// <summary>トークルームからかグループからか</summary>
        public string FromType;
        /// <summary>送信元のユーザID/グループID</summary>
        public string FromId;
        /// <summary>内容</summary>
        public string Message;
        
        /// <summary>リクエストボディのバイト配列(シグネチャチェック用)</summary>
        public byte[] RequestBodyBytes;

        public RequestInfoFromLine(HttpRequest request) {
            using (StreamReader reader = new StreamReader(request.InputStream))
            {
                string body = reader.ReadToEnd();
                RequestBodyBytes = Encoding.UTF8.GetBytes(body);
            }

            if (RequestBodyBytes != null)
            {
                dynamic requestJson = DynamicJson.Parse(Encoding.UTF8.GetString(RequestBodyBytes));
                if (requestJson != null && requestJson.events != null)
                {
                    ReplyToken = requestJson.events[0].replyToken;
                    EventType = requestJson.events[0].type;
                    FromId = SetFromId(requestJson);

                    switch (EventType) {
                        case CommonSettings.EventType.MESSAGE:
                            Message = requestJson.events[0].message.text;
                            break;
                        case CommonSettings.EventType.POSTBACK:
                            Message = requestJson.events[0].postback.data;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// 送信元IDを設定
        /// </summary>
        /// <param name="requestJson"></param>
        /// <returns></returns>
        private string SetFromId(dynamic requestJson) {
            if (requestJson.events[0].source.type == "group")
            {
                FromType = "group";
                return requestJson.events[0].source.groupId;
            }
            else if (requestJson.events[0].source.type == "room")
            {
                FromType = "room";
                return requestJson.events[0].source.roomId;
            }
            else {
                return requestJson.events[0].source.userId;
            }
        }
    }
}