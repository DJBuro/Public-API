using System;
using System.Configuration;
using System.Net;
using System.Text;
using GeoCode.Dao;
using GeoCode.Dao.Domain;
using System.IO;
using System.Xml.XPath;
using Newtonsoft.Json.Linq;

namespace GeoCode
{
    public class Service : IGeoCode
    {
        //public ICoordinatesDao CoordinatesDao { get; set; }
        public IPostCodeDao PostCodeDao { get; set; }

        public Coordinates GeoCodeAddress(string externalStoreId, string buildingNumber, string buildingName, string roadName, string townCity, string postCode, string country)
        {
            Coordinates coordinates = null;

            try
            {
                Logging.LogNotice("GeoCodeAddress", externalStoreId, "info", "GeoCoding Address " + buildingName + " " + buildingNumber + " " + roadName + " - " + townCity + " - " + postCode + " - " + country);

                var address = new Address
                {
                    BuildingName = buildingName,
                    BuildingNumber = buildingNumber,
                    RoadName = roadName,
                    TownCity = townCity,
                    PostCode = postCode,
                    Country = country
                };

                // Special super hack for Russian store 693 - Lyusinovskaya
                if (externalStoreId == "693")
                {
                    // Use Yandex to do the geocoding
                    coordinates = YandexGeoCode(externalStoreId, address);
                }
                else
                {
                    // There is a lookup table of UK postcode lat/lons in the database
                    // Is the postcode in the lookup table?
                    var pCode = PostCodeDao.Find(postCode.Replace(" ", ""));
                    if (pCode != null)
                    {
                        coordinates = new Coordinates((float)pCode.Longitude, (float)pCode.Latitude);

                        Logging.LogNotice("GeoCodeAddress", externalStoreId, "info", "Using cached co-ordinates: lat " + coordinates.Latitude.ToString() + " lon " + coordinates.Longitude.ToString());
                    }

                    if (coordinates == null)
                    {
                        // Use Google to geocode the address
                        coordinates = GoogleGeoCode(externalStoreId, address, ConfigurationManager.AppSettings.Get("Deployed"));
                    }
                }
            }
            catch (Exception exception)
            {
                Logging.LogNotice("GeoCodeAddress", externalStoreId, "error", "Google GeoCoding Address " + buildingName + " " + buildingNumber + " " + roadName + " - " + townCity + " - " + postCode + " - " + country);
            }

            return coordinates;
        }

        private static Coordinates GoogleGeoCode<TAddress>(string externalStoreId, TAddress address, string environment) where TAddress : IGeoAddress
        {
            // ROB: Switched to JSON - below comments no longer valid
            // https://developers.google.com/maps/documentation/geocoding/

            //note : returning as CSV (not json) faster, smaller.
            //http://code.google.com/apis/maps/documentation/geocoding/index.html

            //note: add gl=gb in the future for uk/foreign geocodes... 
            //http://en.wikipedia.org/wiki/Country_code_top-level_domain#List_of_ccTLDs

            /*
             * france fr
             * netherlands nl 
             * USA us
             * ireland ie
             * uk gb
            */
            var sbAddress = new StringBuilder();

            if (!string.IsNullOrEmpty(address.BuildingName))
                sbAddress.Append(address.BuildingName + ",");

            if (!string.IsNullOrEmpty(address.BuildingNumber))
                sbAddress.Append(address.BuildingNumber + ",");

            if (!string.IsNullOrEmpty(address.RoadName))
                sbAddress.Append(address.RoadName + ",");

            if (!string.IsNullOrEmpty(address.TownCity))
                sbAddress.Append(address.TownCity + ",");

            if (!string.IsNullOrEmpty(address.PostCode))
                sbAddress.Append(address.PostCode + ",");

            if (!string.IsNullOrEmpty(address.Country))
                sbAddress.Append(address.Country + ",");

            //tidy up for google
            sbAddress.Remove(sbAddress.Length - 1, 1); //remove last comma
            sbAddress.Replace(' ', '+');//spaces need to be '+'

            // used on each read operation
            var buf = new byte[8192];

            //different keys for test/live
           // var googleKey = environment != "Live" ? "ABQIAAAAObgT0Ow5uhBTml1-xciFUhSa1fbgjkZQcqRzqZ6QigwuSrVQiRS2LcaNo-jeCiJEu4waDuLPdwMQmQ" : "ABQIAAAAObgT0Ow5uhBTml1-xciFUhSG12XL8ghkPchQ2edm-Ke-gqXmYhR_tNO-XSjrQ0sdXRHJou9cPb1f_A";
           // string url = "http://maps.google.com/maps/geo?q=" + sbAddress.ToString() + "&output=csv&sensor=false&key=" + googleKey;

            // http://maps.google.com/maps/api/geocode/json?address=1600+Ampitheatre+Parkway,+Mountain+View,+CA&sensor=false
            string url = "http://maps.google.com/maps/api/geocode/json?address=" + sbAddress.ToString() + "&sensor=false";

            Stream resStream = null;
            try
            {
                var request = WebRequest.Create(url);
                var response = request.GetResponse();
                resStream = response.GetResponseStream();
            }
            catch (Exception exception)
            {
                Logging.LogNotice("GoogleGeoCode", externalStoreId, "error", url);
                Logging.LogNotice("GoogleGeoCode", externalStoreId, "error", exception.Message);
            }

            int count;

            //build input
            var sb = new StringBuilder();

            do
            {
                // fill the buffer
                count = resStream.Read(buf, 0, buf.Length);

                if (count != 0)
                {
                    // translate from bytes to ASCII text
                    sb.Append(Encoding.ASCII.GetString(buf, 0, count));
                }
            }
            while (count > 0);

            var coordinates = new Coordinates();
            JObject root = JObject.Parse(sb.ToString());

            if ((string)root["status"] == "OK")
            {
                JArray items = (JArray)root["results"];
                if (items.Count == 0)
                {
                    Logging.LogNotice("GoogleGeoCode", externalStoreId, "warning", "Bad Google Json: " + sb.ToString());
                }
                else
                {
                    JObject geometry = (JObject)items[0]["geometry"];
                    coordinates.Latitude = float.Parse((string)geometry["location"]["lat"]);
                    coordinates.Longitude = float.Parse((string)geometry["location"]["lng"]);

                    Logging.LogNotice("GoogleGeoCode", externalStoreId, "notice", "Geocode success: lat " + coordinates.Latitude.ToString() + " lon " + coordinates.Longitude.ToString() + " for " + url);
                }
            }
            else
            {
                Logging.LogNotice("GoogleGeoCode", externalStoreId, "warning", "Geocode failed with status " + (string)root["status"]  + " " + url);
            }

            //JArray items = (JArray)root["results"];

            //JObject item;
            //JToken jtoken;

            //for (int i = 0; i < items.Count; i++) //loop through rows
            //{
            //    item = (JObject)items[i];
            //    jtoken = item.First;

            //    while (jtoken != null)//loop through columns
            //    {
            //        Response.Write(((JProperty)jtoken).Name.ToString() + " : " + ((JProperty)jtoken).Value.ToString() + "<br />");

            //        jtoken = jtoken.Next;
            //    }
            //}

            //var googleReturn = sb.ToString().Split(',');
            //var coordinates = new Coordinates();

            //if (googleReturn.Length == 4)
            //{
            //    if (googleReturn[0] == "200")
            //    {
            //        if (Convert.ToInt16(googleReturn[1]) > 5) //accuracy below 5 is useless (accuracy isn't confidence in this instance!)
            //        {
            //            coordinates.Latitude = Convert.ToSingle(googleReturn[2]);
            //            coordinates.Longitude = Convert.ToSingle(googleReturn[3]);
            //        }
            //    }
            //    else // failed
            //    {
            //        //todo: add logging
            //        Logging.LogNotice("GoogleGeoCode", externalStoreId, "warning", "Google Failed");
            //    }
            //}

            return coordinates;
        }

        private static Coordinates YandexGeoCode<TAddress>(string externalStoreId, TAddress address) where TAddress : IGeoAddress
        {
            // Build a string containing the address to be geocoded
            StringBuilder addressString = new StringBuilder();

            // Default to Russia for the country
            addressString.Append("Россия, ");

            // Add the town/city if we have it
            if (string.IsNullOrEmpty(address.TownCity))
            {
                // Default to Moscow!
                addressString.Append(", Москва");
            }
            else
            {
                string cityTown = address.TownCity.Trim();
                if (cityTown.Length < 3)
                {
                    // Default to Moscow!
                    addressString.Append(", Москва");
                }
                else
                {
                    addressString.Append(",");
                    addressString.Append(address.TownCity);
                }
            }

            // Add the road name if we have it
            if (!string.IsNullOrEmpty(address.RoadName))
            {
                addressString.Append(",");
                addressString.Append(address.RoadName);
            }

            // Add the building name if we have it
            if (!string.IsNullOrEmpty(address.BuildingName))
            {
                addressString.Append(",");
                addressString.Append(address.BuildingName);
            }
            
            // Add the building number if we have it
            if (!string.IsNullOrEmpty(address.BuildingNumber))
            {
                addressString.Append(",");
                addressString.Append(address.BuildingNumber);
            }

            addressString.Replace(' ', '+');//spaces need to be '+'

            string key = "AOYqbUwBAAAALm5GCwMADk87ueq-F278ZI_9DIZ7s3OLnxAAAAAAAAAAAAAc2KPxNsMdcev6mPWjsGrjd5HwyQ==";

            string url = "http://geocode-maps.yandex.ru/1.x/?geocode=" + addressString.ToString() + "&key=" + key;

            Logging.LogNotice("YandexGeoCode", externalStoreId, "notice", "Yandex GeoCoding Address buildingName =" + address.BuildingName + " buildingNumber = " + address.BuildingNumber + " roadName = " + address.RoadName + " townCity = " + address.TownCity + " postCode = " + address.PostCode + " country = " + address.Country + " url=" + url);

            WebRequest webRequest = WebRequest.Create(url);
            WebResponse webResponse = webRequest.GetResponse();

            // Get the http response
            Stream receiveStream = webResponse.GetResponseStream();

            Encoding encode = System.Text.Encoding.GetEncoding("utf-8");

            // Pipe the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, encode);

            string xml = readStream.ReadToEnd();

//            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\"?>  <ymaps xmlns=\"http://maps.yandex.ru/ymaps/1.x\" xmlns:x=\"http://www.yandex.ru/xscript\">    <GeoObjectCollection>      <metaDataProperty xmlns=\"http://www.opengis.net/gml\">        <GeocoderResponseMetaData xmlns=\"http://maps.yandex.ru/geocoder/1.x\">          <request>Москва,Тверская улица,дом 18,корпус 1</request>          <found>9</found>          <results>10</results>        </GeocoderResponseMetaData>      </metaDataProperty>      <featureMember xmlns=\"http://www.opengis.net/gml\">        <GeoObject xmlns=\"http://maps.yandex.ru/ymaps/1.x\">          <metaDataProperty xmlns=\"http://www.opengis.net/gml\">            <GeocoderMetaData xmlns=\"http://maps.yandex.ru/geocoder/1.x\">              <kind>house</kind>              <text>Россия, Москва, улица Тверская, 18к1</text>              <precision>exact</precision>              <AddressDetails xmlns=\"urn:oasis:names:tc:ciq:xsdschema:xAL:2.0\">                <Country>                  <CountryName>Россия</CountryName>                  <Locality>                    <LocalityName>Москва</LocalityName>                    <Thoroughfare>                      <ThoroughfareName>улица Тверская</ThoroughfareName>                      <Premise>                        <PremiseNumber>18к1</PremiseNumber>                      </Premise>                    </Thoroughfare>                  </Locality>                </Country>              </AddressDetails>            </GeocoderMetaData>          </metaDataProperty>          <boundedBy xmlns=\"http://www.opengis.net/gml\">            <Envelope>              <lowerCorner>37.600980 55.763640</lowerCorner>              <upperCorner>37.609191 55.768269</upperCorner>            </Envelope>          </boundedBy>          <Point xmlns=\"http://www.opengis.net/gml\">            <pos>37.605086 55.765954</pos>          </Point>        </GeoObject>      </featureMember>      <featureMember xmlns=\"http://www.opengis.net/gml\">        <GeoObject xmlns=\"http://maps.yandex.ru/ymaps/1.x\">          <metaDataProperty xmlns=\"http://www.opengis.net/gml\">            <GeocoderMetaData xmlns=\"http://maps.yandex.ru/geocoder/1.x\">              <kind>house</kind>              <text>Россия, Москва, улица 1-я Тверская-Ямская, 18</text>              <precision>number</precision>              <AddressDetails xmlns=\"urn:oasis:names:tc:ciq:xsdschema:xAL:2.0\">                <Country>                  <CountryName>Россия</CountryName>                  <Locality>                    <LocalityName>Москва</LocalityName>                    <Thoroughfare>                      <ThoroughfareName>улица 1-я Тверская-Ямская</ThoroughfareName>                      <Premise>                        <PremiseNumber>18</PremiseNumber>                      </Premise>                    </Thoroughfare>                  </Locality>                </Country>              </AddressDetails>            </GeocoderMetaData>          </metaDataProperty>          <boundedBy xmlns=\"http://www.opengis.net/gml\">            <Envelope>              <lowerCorner>37.587110 55.770871</lowerCorner>              <upperCorner>37.595321 55.775500</upperCorner>            </Envelope>          </boundedBy>          <Point xmlns=\"http://www.opengis.net/gml\">            <pos>37.591216 55.773186</pos>          </Point>        </GeoObject>      </featureMember>      <featureMember xmlns=\"http://www.opengis.net/gml\">        <GeoObject xmlns=\"http://maps.yandex.ru/ymaps/1.x\">          <metaDataProperty xmlns=\"http://www.opengis.net/gml\">            <GeocoderMetaData xmlns=\"http://maps.yandex.ru/geocoder/1.x\">              <kind>house</kind>              <text>Россия, Москва, улица 2-я Тверская-Ямская, 18</text>              <precision>number</precision>              <AddressDetails xmlns=\"urn:oasis:names:tc:ciq:xsdschema:xAL:2.0\">                <Country>                  <CountryName>Россия</CountryName>                  <Locality>                    <LocalityName>Москва</LocalityName>                    <Thoroughfare>                      <ThoroughfareName>улица 2-я Тверская-Ямская</ThoroughfareName>                      <Premise>                        <PremiseNumber>18</PremiseNumber>                      </Premise>                    </Thoroughfare>                  </Locality>                </Country>              </AddressDetails>            </GeocoderMetaData>          </metaDataProperty>          <boundedBy xmlns=\"http://www.opengis.net/gml\">            <Envelope>              <lowerCorner>37.590461 55.769519</lowerCorner>              <upperCorner>37.598672 55.774148</upperCorner>            </Envelope>          </boundedBy>          <Point xmlns=\"http://www.opengis.net/gml\">            <pos>37.594567 55.771834</pos>          </Point>        </GeoObject>      </featureMember>      <featureMember xmlns=\"http://www.opengis.net/gml\">        <GeoObject xmlns=\"http://maps.yandex.ru/ymaps/1.x\">          <metaDataProperty xmlns=\"http://www.opengis.net/gml\">            <GeocoderMetaData xmlns=\"http://maps.yandex.ru/geocoder/1.x\">              <kind>house</kind>              <text>Россия, Москва, улица 3-я Тверская-Ямская, 20</text>              <precision>near</precision>              <AddressDetails xmlns=\"urn:oasis:names:tc:ciq:xsdschema:xAL:2.0\">                <Country>                  <CountryName>Россия</CountryName>                  <Locality>                    <LocalityName>Москва</LocalityName>                    <Thoroughfare>                      <ThoroughfareName>улица 3-я Тверская-Ямская</ThoroughfareName>                      <Premise>                        <PremiseNumber>20</PremiseNumber>                      </Premise>                    </Thoroughfare>                  </Locality>                </Country>              </AddressDetails>            </GeocoderMetaData>          </metaDataProperty>          <boundedBy xmlns=\"http://www.opengis.net/gml\">            <Envelope>              <lowerCorner>37.591925 55.770152</lowerCorner>              <upperCorner>37.600136 55.774781</upperCorner>            </Envelope>          </boundedBy>          <Point xmlns=\"http://www.opengis.net/gml\">            <pos>37.596031 55.772467</pos>          </Point>        </GeoObject>      </featureMember>      <featureMember xmlns=\"http://www.opengis.net/gml\">        <GeoObject xmlns=\"http://maps.yandex.ru/ymaps/1.x\">          <metaDataProperty xmlns=\"http://www.opengis.net/gml\">            <GeocoderMetaData xmlns=\"http://maps.yandex.ru/geocoder/1.x\">              <kind>house</kind>              <text>Россия, Москва, улица 4-я Тверская-Ямская, 16</text>              <precision>near</precision>              <AddressDetails xmlns=\"urn:oasis:names:tc:ciq:xsdschema:xAL:2.0\">                <Country>                  <CountryName>Россия</CountryName>                  <Locality>                    <LocalityName>Москва</LocalityName>                    <Thoroughfare>                      <ThoroughfareName>улица 4-я Тверская-Ямская</ThoroughfareName>                      <Premise>                        <PremiseNumber>16</PremiseNumber>                      </Premise>                    </Thoroughfare>                  </Locality>                </Country>              </AddressDetails>            </GeocoderMetaData>          </metaDataProperty>          <boundedBy xmlns=\"http://www.opengis.net/gml\">            <Envelope>              <lowerCorner>37.591952 55.771540</lowerCorner>              <upperCorner>37.600163 55.776168</upperCorner>            </Envelope>          </boundedBy>          <Point xmlns=\"http://www.opengis.net/gml\">            <pos>37.596058 55.773854</pos>          </Point>        </GeoObject>      </featureMember>      <featureMember xmlns=\"http://www.opengis.net/gml\">        <GeoObject xmlns=\"http://maps.yandex.ru/ymaps/1.x\">          <metaDataProperty xmlns=\"http://www.opengis.net/gml\">            <GeocoderMetaData xmlns=\"http://maps.yandex.ru/geocoder/1.x\">              <kind>street</kind>              <text>Россия, Московская область, Солнечногорский район, Солнечногорск, улица Тверская</text>              <precision>street</precision>              <AddressDetails xmlns=\"urn:oasis:names:tc:ciq:xsdschema:xAL:2.0\">                <Country>                  <CountryName>Россия</CountryName>                  <AdministrativeArea>                    <AdministrativeAreaName>Московская область</AdministrativeAreaName>                    <SubAdministrativeArea>                      <SubAdministrativeAreaName>Солнечногорский район</SubAdministrativeAreaName>                      <Locality>                        <LocalityName>Солнечногорск</LocalityName>                        <Thoroughfare>                          <ThoroughfareName>улица Тверская</ThoroughfareName>                        </Thoroughfare>                      </Locality>                    </SubAdministrativeArea>                  </AdministrativeArea>                </Country>              </AddressDetails>            </GeocoderMetaData>          </metaDataProperty>          <boundedBy xmlns=\"http://www.opengis.net/gml\">            <Envelope>              <lowerCorner>36.951750 56.202426</lowerCorner>              <upperCorner>36.984682 56.220779</upperCorner>            </Envelope>          </boundedBy>          <Point xmlns=\"http://www.opengis.net/gml\">            <pos>36.968216 56.211603</pos>          </Point>        </GeoObject>      </featureMember>      <featureMember xmlns=\"http://www.opengis.net/gml\">        <GeoObject xmlns=\"http://maps.yandex.ru/ymaps/1.x\">          <metaDataProperty xmlns=\"http://www.opengis.net/gml\">            <GeocoderMetaData xmlns=\"http://maps.yandex.ru/geocoder/1.x\">              <kind>street</kind>              <text>Россия, Московская область, Сергиево-Посадский район, Сергиев Посад, улица Тверская</text>              <precision>street</precision>              <AddressDetails xmlns=\"urn:oasis:names:tc:ciq:xsdschema:xAL:2.0\">                <Country>                  <CountryName>Россия</CountryName>                  <AdministrativeArea>                    <AdministrativeAreaName>Московская область</AdministrativeAreaName>                    <SubAdministrativeArea>                      <SubAdministrativeAreaName>Сергиево-Посадский район</SubAdministrativeAreaName>                      <Locality>                        <LocalityName>Сергиев Посад</LocalityName>                        <Thoroughfare>                          <ThoroughfareName>улица Тверская</ThoroughfareName>                        </Thoroughfare>                      </Locality>                    </SubAdministrativeArea>                  </AdministrativeArea>                </Country>              </AddressDetails>            </GeocoderMetaData>          </metaDataProperty>          <boundedBy xmlns=\"http://www.opengis.net/gml\">            <Envelope>              <lowerCorner>38.104414 56.330650</lowerCorner>              <upperCorner>38.137347 56.348941</upperCorner>            </Envelope>          </boundedBy>          <Point xmlns=\"http://www.opengis.net/gml\">            <pos>38.120880 56.339796</pos>          </Point>        </GeoObject>      </featureMember>      <featureMember xmlns=\"http://www.opengis.net/gml\">        <GeoObject xmlns=\"http://maps.yandex.ru/ymaps/1.x\">          <metaDataProperty xmlns=\"http://www.opengis.net/gml\">            <GeocoderMetaData xmlns=\"http://maps.yandex.ru/geocoder/1.x\">              <kind>street</kind>              <text>Россия, Московская область, Сергиево-Посадский район, Сергиев Посад, улица 1-я Тверская</text>              <precision>street</precision>              <AddressDetails xmlns=\"urn:oasis:names:tc:ciq:xsdschema:xAL:2.0\">                <Country>                  <CountryName>Россия</CountryName>                  <AdministrativeArea>                    <AdministrativeAreaName>Московская область</AdministrativeAreaName>                    <SubAdministrativeArea>                      <SubAdministrativeAreaName>Сергиево-Посадский район</SubAdministrativeAreaName>                      <Locality>                        <LocalityName>Сергиев Посад</LocalityName>                        <Thoroughfare>                          <ThoroughfareName>улица 1-я Тверская</ThoroughfareName>                        </Thoroughfare>                      </Locality>                    </SubAdministrativeArea>                  </AdministrativeArea>                </Country>              </AddressDetails>            </GeocoderMetaData>          </metaDataProperty>          <boundedBy xmlns=\"http://www.opengis.net/gml\">            <Envelope>              <lowerCorner>38.102923 56.331004</lowerCorner>              <upperCorner>38.135855 56.349295</upperCorner>            </Envelope>          </boundedBy>          <Point xmlns=\"http://www.opengis.net/gml\">            <pos>38.119389 56.340151</pos>          </Point>        </GeoObject>      </featureMember>      <featureMember xmlns=\"http://www.opengis.net/gml\">        <GeoObject xmlns=\"http://maps.yandex.ru/ymaps/1.x\">          <metaDataProperty xmlns=\"http://www.opengis.net/gml\">            <GeocoderMetaData xmlns=\"http://maps.yandex.ru/geocoder/1.x\">              <kind>house</kind>              <text>Россия, Московская область, Дубна, улица Тверская, 18</text>              <precision>number</precision>              <AddressDetails xmlns=\"urn:oasis:names:tc:ciq:xsdschema:xAL:2.0\">                <Country>                  <CountryName>Россия</CountryName>                  <AdministrativeArea>                    <AdministrativeAreaName>Московская область</AdministrativeAreaName>                    <Locality>                      <LocalityName>Дубна</LocalityName>                      <Thoroughfare>                        <ThoroughfareName>улица Тверская</ThoroughfareName>                        <Premise>                          <PremiseNumber>18</PremiseNumber>                        </Premise>                      </Thoroughfare>                    </Locality>                  </AdministrativeArea>                </Country>              </AddressDetails>            </GeocoderMetaData>          </metaDataProperty>          <boundedBy xmlns=\"http://www.opengis.net/gml\">            <Envelope>              <lowerCorner>37.126158 56.756779</lowerCorner>              <upperCorner>37.142615 56.765818</upperCorner>            </Envelope>          </boundedBy>          <Point xmlns=\"http://www.opengis.net/gml\">            <pos>37.134387 56.761299</pos>          </Point>        </GeoObject>      </featureMember>    </GeoObjectCollection>  </ymaps>";

            xml = xml.Replace("xmlns=\"http://maps.yandex.ru/ymaps/1.x\"", "");
            xml = xml.Replace("xmlns:x=\"http://www.yandex.ru/xscript\"", "");
            xml = xml.Replace("xmlns=\"http://www.opengis.net/gml\"", "");

            //using (FileStream fileStream = File.Open(@"c:\templog.txt", FileMode.Append))
            //{
            //    StreamWriter s = new StreamWriter(fileStream);
            //    s.WriteLine(url);
            //    s.Write(xml);
            //    s.Flush();
            //}

            Coordinates coordinates = new Coordinates();
            
            using (StringReader stream = new StringReader(xml))
            {
                XPathDocument document = new XPathDocument(stream);
                XPathNavigator navigator = document.CreateNavigator();
                XPathNodeIterator geocodeNodes = navigator.Select("/ymaps/GeoObjectCollection/featureMember/GeoObject/Point/pos");

                // Did we find an element containing the geocode results?
                if (geocodeNodes.MoveNext())
                {
                    // The xml contains the co-ordinates as a string e.g. "37.611724 55.759932"
                    string latLon = geocodeNodes.Current.Value;
                    
                    // Split the lat/lon where ever there is a space character
                    string[] latLonChunks = latLon.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);

                    // There should be two bits, lat and lon
                    if (latLonChunks.Length == 2)
                    {
                        float latitude = 0;
                        // Is the latitude bit a number? (it's the second chunk)
                        if (float.TryParse(latLonChunks[1], out latitude))
                        {
                            float longitude = 0;

                            // Is the longitude bit a number? (it's the first chunk)
                            if (float.TryParse(latLonChunks[0], out longitude))
                            {
                                coordinates.Latitude = latitude;
                                coordinates.Longitude = longitude;
                            }
                            else
                            {
                                Logging.LogNotice("GeoCodeAddress", externalStoreId, "warning", "Yandex returned non-numeric longitude: " + latLon);
                            }   
                        }
                        else
                        {
                            Logging.LogNotice("GeoCodeAddress", externalStoreId, "warning", "Yandex returned non-numeric latitude: " + latLon);
                        }
                    }
                    else
                    {
                        Logging.LogNotice("GeoCodeAddress", externalStoreId, "warning", "Yandex returned invalid coordinates: " + latLon);
                    }
                }
                else
                {
                    Logging.LogNotice("GeoCodeAddress", externalStoreId, "warning", "Yandex Failed");
                }
            }
            
            return coordinates;
        }
    }
}
