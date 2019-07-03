using Codeplex.Data;
using ImasMbgaStamper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ImasMbgaStamper.Manager.ReplyManagers
{
    public class JoinReplyManager : ReplyManager
    {
        public override byte[] CreateReplyBodyBytes(RequestInfoFromLine info)
        {
            //グループ追加の場合、以下の固定メッセージを返す。
            dynamic replyBody = new DynamicJson();

            replyBody.replyToken = info.ReplyToken;
            replyBody.messages = new[] {
                new {
                    type="text",
                    text="グループ追加ありがとうございます！！\r\n このグループのURLは以下です。"
                },
                new {
                    type="text",
                    text="https://script.google.com/macros/s/AKfycbz8izLdmSMTJZUGR6ZkY5UMdwAbYIXGx5THlukI61wZMQbUFN8/exec?gid=" + info.FromId// +"&version=2"
                }
            };

            return Encoding.UTF8.GetBytes(replyBody.ToString());
        }
    }
}