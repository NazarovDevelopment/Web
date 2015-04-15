using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;
using System.Web.Security;
using SportStore.WebUI.Infrastructure.Abstract;

namespace SportStore.WebUI.Infrastructure.Concrete
{
    public class FormsAuthProvider: IAuthProvider
    {
        public bool Authenticate(string username, string password)
        {
            bool result = FormsAuthentication.Authenticate(username, false);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(username,false);
            }
            return result;
        }
    }
}