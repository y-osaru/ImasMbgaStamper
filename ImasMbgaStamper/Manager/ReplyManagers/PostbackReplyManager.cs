using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ImasMbgaStamper.Model;
using Codeplex.Data;

namespace ImasMbgaStamper.Manager.ReplyManagers
{
    public class PostbackReplyManager : ReplyManager
    {
        public override byte[] CreateReplyBodyBytes(RequestInfoFromLine info)
        {
            //トークルームから除外する為に使われる。レスポンスは空を返す
            

            throw new NotImplementedException();
        }
    }
}