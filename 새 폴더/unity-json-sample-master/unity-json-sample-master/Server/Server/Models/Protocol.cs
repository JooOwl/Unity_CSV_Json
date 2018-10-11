using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetServer.Models
{
    public class DATA_DUMMY
    {
        public string a;
        public int b;
    }

    public class GAME_REQ_DUMMY
    {
        public DATA_DUMMY dummy = new DATA_DUMMY();
        public string c;
    }


    public class GAME_REQ_USER_LOGIN
    {
        public string Name;
    }

    public class GAME_ANS_USER_LOGIN
    {
        public int Code;
    }


    public class GAME_JSON_HEADER
    {
        public string UserKey;
    }

    public class GAME_REQ_JSON : GAME_JSON_HEADER
    {
        public string Dummy;
    }

    public class GAME_ANS_JSON : GAME_JSON_HEADER
    {
        public string Dummy;
    }
}