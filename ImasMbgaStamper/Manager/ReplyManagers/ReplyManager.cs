using ImasMbgaStamper.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ImasMbgaStamper.Manager.ReplyManagers
{
    public abstract class ReplyManager : ReplyManagerFactory
    {
        /// <summary>
        /// リプライのボディのバイト配列を作成する
        /// </summary>
        /// <returns></returns>
        public abstract byte[] CreateReplyBodyBytes(RequestInfoFromLine info);
    }
}