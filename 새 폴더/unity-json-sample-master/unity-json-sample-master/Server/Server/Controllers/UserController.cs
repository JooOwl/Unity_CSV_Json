using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace AspNetServer.Controllers
{
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        [HttpGet]
        public string Get()
        {
            return "aa";
        }

        // POST api/user/login
        [HttpPost]
        [Route("login")]
        public IHttpActionResult Login([FromBody] Models.GAME_REQ_USER_LOGIN request)
        {
            Models.GAME_ANS_USER_LOGIN answer = new Models.GAME_ANS_USER_LOGIN();
            answer.Code = -1;

            DBInterface.SimpleGameDB db = new DBInterface.SimpleGameDB();
            if(db.IsUserExists(request.Name) == true)
            {
                answer.Code = 0;
            }

            return Json(answer);
        }

        // POST api/user/json
        [HttpPost]
        [Route("json")]
        public IHttpActionResult Json([FromBody] Models.GAME_REQ_JSON request)
        {
            Models.GAME_ANS_JSON answer = new Models.GAME_ANS_JSON();
            answer.UserKey = request.UserKey;
            answer.Dummy = "키가 정상";

            return Json(answer);
        }

        [HttpPost]
        [Route("dummy")]
        public IHttpActionResult Dummy([FromBody] Models.GAME_REQ_DUMMY request)
        {
            Models.GAME_REQ_DUMMY b = new Models.GAME_REQ_DUMMY();
            b.dummy.a = "asd";
            b.dummy.b = 123;
            b.c = "efg";

            return Json(b);
        }
    }
}
