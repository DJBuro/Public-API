using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyAndromeda.Services.GprsGateway
{
    public static class TicketExtensions 
    {
        public static async Task<ICollection<T>> LoadAsync<T>(this ICollection<T> collection)
            where T : class
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            var entityCollection = collection as System.Data.Entity.Core.Objects.DataClasses.EntityCollection<T>;

            if (entityCollection == null || entityCollection.IsLoaded)
                return collection;
            
            await entityCollection.LoadAsync(CancellationToken.None).ConfigureAwait(false);

            return collection;
        }

        public static string NewLineAtBeginning(this string text) 
        {
            return @"\r" + text;    
        }
        
        public static string NewLineAtEnd(this string text) 
        {
            return text + @"\r";
        }

        public static StringBuilder StartOrEndOfOrder(this StringBuilder sb) 
        {
            sb.Append("#");
            return sb;
        }

        public static StringBuilder StartNewLine(this StringBuilder sb, int lines = 1) 
        {
            for (var i = lines; i > 0; i--) 
            {
                sb.Append("\r");
            }
            return sb;
        }

        public static StringBuilder AddSpace(this StringBuilder sb, int spaces = 1)
        {
            for (var i = spaces; i > 0; i--) 
            {
                sb.Append(" ");
            }
            return sb;
        }

        public static StringBuilder EndOfPart(this StringBuilder sb, TicketDepth depth) 
        {
            if (depth == TicketDepth.Segment)
            {
                sb.Append("*");
            }
            if (depth == TicketDepth.SegmentChild)
            {
                sb.Append(";");
            }
            if (depth == TicketDepth.ContentChild)
            {
                sb.Append(",");
            }

            return sb;
        }

        public static string EndOfPart(this string text, TicketDepth depth) 
        {
            if (depth == TicketDepth.Segment) 
            {
                return text += "*";
            }
            if (depth == TicketDepth.SegmentChild)
            {
                return text += ";";
            }
            if (depth == TicketDepth.ContentChild) 
            {
                return text += ",";
            }

            return null;
        }
    }
}