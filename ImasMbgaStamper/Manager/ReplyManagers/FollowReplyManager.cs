using Codeplex.Data;
using ImasMbgaStamper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace ImasMbgaStamper.Manager.ReplyManagers
{
    public class FollowReplyManager : ReplyManager
    {
        public override byte[] CreateReplyBodyBytes(RequestInfoFromLine info)
        {
            //友だち追加の場合、以下の固定メッセージを返す。
            dynamic replyBody = new DynamicJson();

            replyBody.replyToken = info.ReplyToken;
            replyBody.messages = new[] {
                new {
                    type="text",
                    text="友だち追加ありがとうございます！\r\n このBotをグループに追加する事で、\r\n モバマスのスタンプを送る事が出来ますよ！"
                }
            };

            return Encoding.UTF8.GetBytes(replyBody.ToString());
        }
    }
}