﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace QLBH_MVC.Models
{
    public class LoginModel
    {
        public string UID { get; set; }
        public string PWD { get; set; }
        public bool RememberMe { get; set; }
        public string RetUrl { get; set; }
    }
}