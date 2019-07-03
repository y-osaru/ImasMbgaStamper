using ImasMbgaStamper.Manager.ReplyManagers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImasMbgaStamper.Manager
{
    public class ReplyManagerFactory
    {
        public static ReplyManager Create(string eventType)
        {
            //LINEからのリクエストタイプによって、処理を分岐
            switch (eventType)
            {
                case CommonSettings.EventType.FOLLOW:
                    return new FollowReplyManager();
                case CommonSettings.EventType.JOIN:
                    return new JoinReplyManager();
                case CommonSettings.EventType.MESSAGE:
                    return new MessageReplyManager();
                default:
                    throw new Exception("eventType Undefined!:" + eventType);
            }
        }
    }
}