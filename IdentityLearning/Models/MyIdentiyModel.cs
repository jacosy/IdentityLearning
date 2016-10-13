using IdentitySample.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using IdentityLearning.Services;

namespace IdentityLearning.Models
{
    public class MyUser : IUser
    {
        public MyUser(string sessionId)
        {
            this._sessionId = sessionId;
        }
        public string Id
        {
            get
            {
                //return _sessionId;
                return $"{this.Domain}/{this.UserName}";
            }
        }

        public string UserName { get; set; }

        public string Password { get; set; }

        private string _sessionId;
        public string SessionId
        {
            get
            {
                return _sessionId;
            }
        }

        public string Domain { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<MyUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            userIdentity.AddClaim(new Claim(ClaimTypes.PrimarySid, this.SessionId));
            userIdentity.AddClaim(new Claim(ClaimTypes.Email, $"{this.UserName}@{this.Domain}"));
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class MyUserStore : IUserStore<MyUser>
    {
        private string GetSessionId()
        {
            var user = HttpContext.Current.User;
            return user == null ? null : (HttpContext.Current.User.Identity as ClaimsIdentity).FindFirstValue(ClaimTypes.PrimarySid);
        }

        public Task CreateAsync(MyUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(MyUser user)
        {
            throw new NotImplementedException();
        }

        public Task<MyUser> FindByIdAsync(string userId)
        {
            //throw new NotImplementedException();
            MyUser user = null;

            UltimusService service = new UltimusService();
            string sessionId = GetSessionId();
            if (!string.IsNullOrEmpty(sessionId))
            {
                if (service.IsValidSessionID(sessionId, userId))
                {
                    return Task.Run<MyUser>(() => new MyUser(sessionId));
                }
                else
                {
                    return Task.FromResult(user);
                }
            }
            else
            {
                return Task.FromResult(user);
            }
        }

        public Task<MyUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(MyUser user)
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // 偵測多餘的呼叫

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 處置 Managed 狀態 (Managed 物件)。
                }

                // TODO: 釋放 Unmanaged 資源 (Unmanaged 物件) 並覆寫下方的完成項。
                // TODO: 將大型欄位設為 null。

                disposedValue = true;
            }
        }

        // TODO: 僅當上方的 Dispose(bool disposing) 具有會釋放 Unmanaged 資源的程式碼時，才覆寫完成項。
        // ~MyUserStore() {
        //   // 請勿變更這個程式碼。請將清除程式碼放入上方的 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 加入這個程式碼的目的在正確實作可處置的模式。
        public void Dispose()
        {
            // 請勿變更這個程式碼。請將清除程式碼放入上方的 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果上方的完成項已被覆寫，即取消下行的註解狀態。
            GC.SuppressFinalize(this);
        }
        #endregion

    }

    public class MyUserLoginStore : IUserLoginStore<MyUser>
    {
        public Task AddLoginAsync(MyUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(MyUser user)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(MyUser user)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<MyUser> FindAsync(UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task<MyUser> FindByIdAsync(string userId)
        {
            throw new NotImplementedException();
        }

        public Task<MyUser> FindByNameAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(MyUser user)
        {
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(MyUser user, UserLoginInfo login)
        {
            throw new NotImplementedException();
        }

        public Task UpdateAsync(MyUser user)
        {
            throw new NotImplementedException();
        }
    }


    public class MyRole : IRole
    {
        public string Id { get; }

        public string Name { get; set; }
    }
}