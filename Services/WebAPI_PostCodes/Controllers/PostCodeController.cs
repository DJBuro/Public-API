using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using WebAPI_PostCodes.Models;
using System.Text.RegularExpressions;
using System.Data.Entity.Spatial;
using System.Data.Entity.SqlServer;

namespace WebAPI_PostCodes.Controllers
{
    public class PostCodeController : ApiController
    {
        const double milesToMetersConversion = 0.00062;
        const double metersToMilesConversion = 1609.344;

        //AndroPostCodeEntities entities;

        // api/postcode/?origin=B166&radius=5
        // used to retrieve list of postcode sectors with in the range of given radius
        public IEnumerable<string> GetSectors(string origin, double radius)
        {
            try
            {
                List<string> sectorsList = new List<string>();
                string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
                SqlDataReader reader;
                if (!string.IsNullOrWhiteSpace(origin) && radius != 0)
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        // list of postcode sectors are retrieved from database by using stored procedure - GetPostCodeSectors_SP and it takes two parameters 1. origin postcode sector and 2. radius in metres. It returns list of postcode sectors covered with in the given radius.
                        using (SqlCommand command = new SqlCommand("GetPostCodeSectors_SP", con))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.CommandTimeout = 0;
                            // origin postcode sector is assigned to @origin
                            command.Parameters.Add("@origin", SqlDbType.NVarChar).Value = origin;
                            // radius covered by the sector is assigned to @radius. The miles are converted to metres and its value is assigned.
                            command.Parameters.Add("@radius", SqlDbType.Float).Value = MilestoMetres(radius);
                            reader = command.ExecuteReader();
                            if (reader != null)
                            {
                                while (reader.Read())
                                {
                                    // sector value is retrieved and added to list
                                    sectorsList.Add(Convert.ToString(reader[0]));
                                }
                            }
                        }
                        con.Close();
                    }
                }
                return sectorsList;
            }
            // if any exception or error occurs return null
            catch (Exception e)
            {
                return null;
            }
        }

        // api/postcode/?postcode=AB101FE&radius=5
        // used to retrieve list of postcode sectors with in the range of given radius
        //public IEnumerable<string> GetPostcodeSectors(string postcode, double radius)
        //[Route("api/test")]
        [HttpGet]
        public async Task<IEnumerable<string>> Test(string postcode, double radius)
        {
            
            var result = Enumerable.Empty<string>();

            try
            {
                using(var entities = new AndroPostCodeEntities())
                {
                    var normalized = postcode.Trim().Replace(" ", "");
                    var location = await entities.OSPostCodes.Where(e => e.PostCodeNormalized == normalized).FirstOrDefaultAsync();
                    
                    var latLong = location.Location;

                    //STDistance by 0.00062 to convert meters to miles.
                    var metres = radius *metersToMilesConversion;
                    var withinRadius = latLong.Buffer(metres);

                    var areasTable = entities.OSPostCodeSectors;
                    var withinArea = areasTable.Where(e => SqlSpatialFunctions.Filter(e.Location, withinRadius) == true);
                    var results = await withinArea.Select(e => new { 
                        e.PostCodeSector,
                        DistanceMeters = latLong.Distance(e.Location)
                    }).ToListAsync();

                    var resultsInMiles = results.Select(e => new { 
                        e.PostCodeSector,
                        e.DistanceMeters,
                        DistanceInMiles = e.DistanceMeters / 0.00062
                    });


                    result = resultsInMiles.Select(e => e.PostCodeSector);

                    //DbGeography.FromText(string.Format("POINT({1} {0})", latitude, longitude), 4326); 
                }
                

                //List<string> postcodeList = new List<string>();
                //List<string> postcodeSectorList = new List<string>();
                //string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
                //SqlDataReader reader;
                //if (!string.IsNullOrWhiteSpace(postcode) && radius != 0)
                //{
                //    using (SqlConnection con = new SqlConnection(connectionString))
                //    {
                //        con.Open();
                //        // list of postcode sectors are retrieved from database by using stored procedure - GetPostCodes_SP and it takes two parameters 1. origin postcode and 2. radius in metres. It returns list of postcodes covered with in the given radius.
                //        using (SqlCommand command = new SqlCommand("GetPostCodes_SP", con))
                //        {
                //            command.CommandType = CommandType.StoredProcedure;
                //            command.CommandTimeout = 0;
                //            // origin postcode sector is assigned to @origin
                //            command.Parameters.Add("@postcode", SqlDbType.NVarChar).Value = postcode;
                //            // radius covered by the sector is assigned to @radius. The miles are converted to metres and its value is assigned.
                //            command.Parameters.Add("@radius", SqlDbType.Float).Value = MilestoMetres(radius);
                //            reader = command.ExecuteReader();
                //            if (reader != null)
                //            {
                //                while (reader.Read())
                //                {
                //                    // sector value is retrieved and added to list
                //                    postcodeList.Add(Convert.ToString(reader[0]));
                //                }
                //            }
                //        }
                //        con.Close();
                //    }
                //    if (postcodeList == null || postcodeList.Count == 0)
                //    {
                //        var response = new Response() { Status = "Invalid", Message = "Postcode is invalid or does not exist", PostcodeList = null };
                //        return response;
                //    }
                //    else
                //    {
                //        foreach (var v in postcodeList)
                //        {
                //            postcodeSectorList.Add(v.Substring(0, v.Length - 3) + " " + v.Substring(v.Length - 3, 1));
                //        }
                //        var response = new Response() { Status = "OK", Message = "", PostcodeList = postcodeSectorList.Distinct().ToList() };
                //        return response;
                //    }
                //}
                //else
                //{
                //    var response = new Response() { Status = "Error", Message = "Invalid Postcode or radius", PostcodeList = null };
                //    return response;
                //}
            }
            // if any exception or error occurs return null
            catch (Exception e)
            {
                throw e;
                //var response = new Response() { Status = "Error", Message = e.Message, PostcodeList = null };
                //return response;
            }

            return result;
        }

        // used to convert miles to metres
        private double MilestoMetres(double miles)
        {
            return miles * 1609.34;
        }



        // api/postcode/?postcode=AB251XS
        // api/postcode/?postcode=AB251XS,AB101AA
        // used to retrieve lat and long values for given postcode.
        // Postcode can be a single postcode value or a list of postcodes in comma separated format.
        public List<LatLong> GetLatLong(string postcode)
        {
            try
            {
                using (var entities = new AndroPostCodeEntities()) { 
                List<LatLong> latlongList = new List<LatLong>();
                OSPostCode code;
                // check whether postcode is null or empty or whitespace
                if (!string.IsNullOrWhiteSpace(postcode))
                {
                    // split the postcode string with ',' and store the postcodes in an array
                    string[] postcodeList = postcode.Split(',');
                    // loop through the postcodes
                    foreach (var post in postcodeList)
                    {
                        // get the matching OSPostCodes record by comparing with postcode
                        code = entities.OSPostCodes.FirstOrDefault(t => t.PostCodeNormalized == post);
                        if (code != null)
                        {
                            // if obtained, add it latlong list - assign postcode, lat, long values
                            latlongList.Add(new LatLong() { Postcode = post, Lat = Convert.ToDouble(code.Lat), Long = Convert.ToDouble(code.Long) });
                        }
                    }
                }
                // return the list
                return latlongList;

                }
            }
            // if any exception or error occurs return null
            catch (Exception e)
            {
                return null;
            }
        }

        public List<string> GetCodes(int count)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBConnectionString"].ToString();
            SqlDataReader reader;
            List<string> postcodeList = new List<string>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string query = "select replace(PostCode,'\"','') as Postcode from OSPostCodes where LEN(PostCodeNormalized)=" + count;
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.CommandType = CommandType.Text;
                    command.CommandTimeout = 0;
                    reader = command.ExecuteReader();
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            postcodeList.Add(Convert.ToString(reader[0]));
                        }
                    }
                }
                con.Close();
            }
            Dictionary<string, string> failList = new Dictionary<string, string>();
            List<string> psLists = new List<string>();
            string[] pattern = { @"[a-zA-Z]{2}[0-9][a-zA-Z][0-9][a-zA-Z]{2}", @"[a-zA-Z]{2}[0-9]{3}[a-zA-Z]{2}", @"[a-zA-Z]{2}[\\s][0-9]", @"[a-zA-Z]{2}[a-zA-Z]{2}[0-9][\\s][0-9][a-zA-Z]{2}", @"[a-zA-Z][0-9][\\s][0-9][a-zA-Z]{2}", @"[a-zA-Z]{2}[0-9][a-zA-Z][0-9][a-zA-Z]{2}", @"[a-zA-Z][0-9]{2}[\\s][0-9][a-zA-Z]{2}" };
            foreach (var v in postcodeList)
            {
                //foreach(var regEx in pattern){
                //    if (!Regex.Match(v, regEx).Success)
                //        failList.Add(v,regEx);
                //}
                bool exists = false;
                foreach (string pat in pattern)
                {
                    //if (Regex.Match(v, pat).Success)
                    if (IsPostCode(v))
                    {
                        exists = true;
                    }
                }
                if (!exists) { psLists.Add(v); }

            }
            return psLists;
        }

        static public bool IsPostCode(string postcode)
        {
            return (
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][0-9][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][0-9][0-9][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][0-9][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][0-9][A-HJKS-UWa-hjks-uw][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[A-PR-UWYZa-pr-uwyz][A-HK-Ya-hk-y][0-9][A-Za-z][ ]*[0-9][ABD-HJLNP-UW-Zabd-hjlnp-uw-z]{2}$)") ||
                Regex.IsMatch(postcode, "(^[Gg][Ii][Rr][]*0[Aa][Aa]$)")
                );
        }
    }
}
