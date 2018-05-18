using hdcontext.IdentityDomain;
using hdcore;
using hdidentity.Interface;
using MyStore.Context.IdentityConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace MyStore.Attributes
{
    public class RBACAuthorizeForAPIAttribute : AuthorizeAttribute
    {
        public string RoleName { get; set; }

        public RBACAuthorizeForAPIAttribute() : base()
        {
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            try
            {
                var userId = CurrentUser.Instance.User.Id;
                var groups = IoC.Resolve<IUserGroupService>().GetMulti(n => n.UserId.Equals(userId));
                var lstRoleId = new List<Role>();
                foreach (var group in groups)
                {
                    var roleIds = IoC.Resolve<IRoleGroupService>().GetMulti(n => n.GroupId.Equals(group.GroupId)).Select(n => n.RoleId);
                    foreach (var id in roleIds)
                    {
                        lstRoleId.Add(IoC.Resolve<IRoleService>().GetbyKey(id));
                    }
                }

                return lstRoleId.Distinct().Any(n => n.Name == RoleName);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (!IsAuthorized(actionContext))
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Unauthorized);
            }
        }
    }
}