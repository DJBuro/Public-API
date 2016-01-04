namespace GeoCode.Dao.Domain
{
    /// <summary>
    /// PostCode object for NHibernate mapped table 'tbl_PostCode'.
    /// </summary>
    public class PostCode
    {
        #region Public Properties

        public long? Id { get; set; }
        public string PCode { get; set; }
        public double Longitude{ get; set;}
        public double Latitude { get; set; }

        #endregion        
        
        #region Constructors

        public PostCode() { }

        public PostCode( string postCode, double longitude, double latitude)
        {
            this.PCode = postCode;
            this.Longitude = longitude;
            this.Latitude = latitude;
        }

        #endregion

		
    }
}