﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace PrivateSignpost.Models
{
    public class ResponseDetails
    {
        public string ResponseText { get; set; }
        public HttpStatusCode httpStatusCode { get; set; }
    }
}