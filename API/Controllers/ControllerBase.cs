using Aplicacao.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;

namespace API.Controllers
{
    [Authorize]
    public class ControllerBase : ApiController
    {
        private Stopwatch _stopWatch;
        private JsonViewModel _jsonResult;
        private string _UserLogin = string.Empty;
        private int _UserId = 0;
        public string UserLogin
        {
            get
            {
                if (string.IsNullOrEmpty(_UserLogin)) { GetUserContext(); }
                return _UserLogin;
            }
        }
        public int UserId
        {
            get
            {
                if (_UserId == 0) { GetUserContext(); }
                return _UserId;
            }
        }
        public Stopwatch StopWatch
        {
            get
            {
                return _stopWatch;
            }
            set
            {
                _stopWatch = value;
            }
        }
        public JsonViewModel JsonResult
        {
            get
            {
                return _jsonResult;
            }
            set
            {
                _jsonResult = value;
            }
        }
        public ControllerBase()
        {
            _stopWatch = new Stopwatch();
            _jsonResult = new JsonViewModel();
        }
        private void GetUserContext()
        {
            if (Request != null)
            {
                var claimPrincipal = Request.GetRequestContext().Principal as ClaimsPrincipal;
                _UserLogin = claimPrincipal.HasClaim(x => x.Type.Equals("userLogin")) ? claimPrincipal.FindFirst(x => x.Type.Equals("userLogin")).Value : string.Empty;
                var userId = claimPrincipal.HasClaim(x => x.Type.Equals("userId")) ? claimPrincipal.FindFirst(x => x.Type.Equals("userId")).Value : string.Empty;
                Int32.TryParse(userId, out _UserId);
            }
        }
    }
}