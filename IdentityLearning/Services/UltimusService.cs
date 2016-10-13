using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using IdentityLearning.UltimusServices;
using IdentitySample.Models;
using IdentityLearning.Models;

namespace IdentityLearning.Services
{
    public class UltimusService
    {
        private static UltimusServices.Services _ultService;
        public UltimusService()
        {
            if (_ultService == null)
            {
                _ultService = new UltimusServices.Services();
            }
        }

        public MyUser LoginUser(MyLoginViewModel model, out string errMsg)
        {
            MyUser user;
            string sessionId = string.Empty;

            if (_ultService.LoginUser(model.Domain, model.UserName, model.Password, out sessionId, out errMsg))
            {
                user = new MyUser(sessionId)
                {
                    Domain = model.Domain,
                    UserName = model.UserName,
                    Password = model.Password
                };
            }
            else
            {
                user = null;
            }

            return user;
        }
        
        public bool IsValidSessionID(string sessionId, string adAccount)
        {
            return _ultService.IsValidSessionID(sessionId, adAccount);
        }
    }
}