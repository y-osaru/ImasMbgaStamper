using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImasMbgaStamper.Model;
using Codeplex.Data;
using System.Text;

namespace ImasMbgaStamper.Manager.ReplyManagers
{
    public class MessageReplyManager : ReplyManager
    {
        public override byte[] CreateReplyBodyBytes(RequestInfoFromLine info)
        {
            dynamic replyBody = new DynamicJson();

            if (info.Message.Contains("グループID"))
            {
                replyBody.replyToken = info.ReplyToken;

                if (info.FromId != null)
                {
                    replyBody.messages = new[] {
                        new {
                            type="text",
                            text="このグループのURLは以下ですよ"
                        },
                        new {
                            type="text",
                            text="https://script.google.com/macros/s/AKfycbz8izLdmSMTJZUGR6ZkY5UMdwAbYIXGx5THlukI61wZMQbUFN8/exec?gid=" + info.FromId// +"&version=2"
                        }
                    };
                }
            }
            else if (info.Message.Contains("スタンプBotいらないよ")) {
                replyBody.replyToken = info.ReplyToken;
                replyBody.messages = new[] {
                    new {
                        type = "template",
                        altText = "除外機能が使えないようです。管理画面URL下部のtwitterより連絡お願い致します。",
                        template = new {
                            type = "confirm",
                            text = "MOB@MP M@STERをトークルームから除外しますか？",
                            actions = new[] {
                                new {
                                    type = "postback",
                                    label = "はい",
                                    data = "ok"
                                },
                                new {
                                    type = "postback",
                                    label = "いいえ",
                                    data = "no"
                                }
                            }
                        }
                    }
                };
            }

            return Encoding.UTF8.GetBytes(replyBody.ToString());
        }
    }
}