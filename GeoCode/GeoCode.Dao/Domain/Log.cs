
using System;

namespace GeoCode.Domain
{
    public class Log
    {
        public Log() { }

		public Log( string storeId, string severity, string message, string method, string source, DateTime? created )
		{
			this.StoreId = storeId;
			this.Severity = severity;
			this.Message = message;
			this.Method = method;
			this.Source = source;
			this.Created = created;
		}

		public string StoreId { get; set;}

		public string Severity{ get; set;}

        public string Message { get; set; }

        public string Method { get; set; }

        public string Source { get; set; }

        public DateTime? Created { get; set; }
    }
}
