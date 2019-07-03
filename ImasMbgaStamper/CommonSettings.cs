using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ImasMbgaStamper
{
    /// <summary>
    /// 設定関連をまとめるクラス
    /// ※一先ず使うものだけに絞っている
    /// </summary>
    public class CommonSettings
    {
        /// <summary>LINEのエンドポイントURL</summary>
        public class EndPointUrl
        {
            /// <summary>リプライを送る際のURL</summary>
            public const string REPLY_URL = "https://api.line.me/v2/bot/message/reply";
            /// <summary>プッシュを送る際のURL</summary>
            public const string PUSH_URL = "https://api.line.me/v2/bot/message/push";
            /// <summary>トークルーム/グループから退出する際のURL</summary>
            public const string LEAVE_URL_TEMPLATE = "https://api.line.me/v2/bot/${FromType}/${FromId}/leave";
        }

        /// <summary>LINEのイベント種別</summary>
        public class EventType {
            /// <summary></summary>
            public const string FOLLOW = "follow";
            public const string JOIN = "join";
            public const string MESSAGE = "message";
            public const string POSTBACK = "postback";
        }

        ///ココらへんも定数だが、外部ファイルに持たせる。
        ///※Gitに上げる関係上。
        /// <summary>チャネルシークレット</summary>
        public static string ChannelSercret;
        /// <summary>チャネルアクセストークン</summary>
        public static string ChannelAccessToken;

        /// <summary>
        /// 初期化
        /// </summary>
        public static void Init()
        {
            try
            {
                ChannelSercret = ConfigurationManager.AppSettings["channelSercret"];
                ChannelAccessToken = ConfigurationManager.AppSettings["channelAccessToken"];
            }
            catch (Exception ex)
            {

            }
        }
    }
}