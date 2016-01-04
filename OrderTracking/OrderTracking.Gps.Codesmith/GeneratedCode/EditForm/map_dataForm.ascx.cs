
using System;
using Cacd.Web.Common.Core;


namespace OrderTracking.Gps.Data
{
	#region Mapdata

	/// <summary>
	/// Mapdata form for NHibernate mapped table 'map_data'.
	/// </summary>
	public partial class MapdataForm : BaseUserControl
		{
		#region Fields
		
		private OrderTracking.Gps.Dao.Domain.Mapdata _Mapdata;

			
		#endregion
		
		#region Properties

		public OrderTracking.Gps.Dao.Domain.Mapdata Mapdata
		{
			set { _Mapdata = value; }
			get { return _Mapdata; }
		}
		
		#endregion
		
		#region Model Management and data binding methods
		
		protected override void InitializeModel()
    	{
        	_Mapdata = new OrderTracking.Gps.Dao.Domain.Mapdata ();
    	}
		
		protected override void LoadModel(object savedModel)
    	{
       		 _Mapdata = (OrderTracking.Gps.Dao.Domain.Mapdata)savedModel;
    	}
		
		protected override object SaveModel()
		{
			return _Mapdata;
		}
		
		
		protected override void InitializeDataBindings()
    	{
			//initialise data bindings for the standard columns (Normal Columns)
				BindingManager.AddBinding("BotypeText.Text","Mapdata.Botype");
				BindingManager.AddBinding("NameText.Text","Mapdata.Name");
				BindingManager.AddBinding("BmtilewidthText.Text","Mapdata.Bmtilewidth");
				BindingManager.AddBinding("BmtileheightText.Text","Mapdata.Bmtileheight");
				BindingManager.AddBinding("BmtotalwidthText.Text","Mapdata.Bmtotalwidth");
				BindingManager.AddBinding("BmtotalheightText.Text","Mapdata.Bmtotalheight");
				BindingManager.AddBinding("GrididText.Text","Mapdata.Gridid");
				BindingManager.AddBinding("DatumidText.Text","Mapdata.Datumid");
				BindingManager.AddBinding("GeomineText.Text","Mapdata.Geomine");
				BindingManager.AddBinding("GeominnText.Text","Mapdata.Geominn");
				BindingManager.AddBinding("GeomaxeText.Text","Mapdata.Geomaxe");
				BindingManager.AddBinding("GeomaxnText.Text","Mapdata.Geomaxn");
				BindingManager.AddBinding("ProjtypeText.Text","Mapdata.Projtype");
				BindingManager.AddBinding("ProjorigoeText.Text","Mapdata.Projorigoe");
				BindingManager.AddBinding("ProjorigonText.Text","Mapdata.Projorigon");
				BindingManager.AddBinding("ProjdedxText.Text","Mapdata.Projdedx");
				BindingManager.AddBinding("ProjdedyText.Text","Mapdata.Projdedy");
				BindingManager.AddBinding("ProjdndxText.Text","Mapdata.Projdndx");
				BindingManager.AddBinding("ProjdndyText.Text","Mapdata.Projdndy");
				BindingManager.AddBinding("XmlfilepathText.Text","Mapdata.Xmlfilepath");
				BindingManager.AddBinding("VirtualpathText.Text","Mapdata.Virtualpath");
				BindingManager.AddBinding("CreatedText.Text","Mapdata.Created");
				BindingManager.AddBinding("ProjorigoxText.Text","Mapdata.Projorigox");
				BindingManager.AddBinding("ProjorigoyText.Text","Mapdata.Projorigoy");
				BindingManager.AddBinding("ProjdvdeText.Text","Mapdata.Projdvde");
				BindingManager.AddBinding("ProjdrdnText.Text","Mapdata.Projdrdn");
    	}
		
		#endregion

		#region Page Lifecycle Methods
	
		protected override void OnInitializeControls(EventArgs e)
		{
			if (!IsPostBack)
			{
				BindMapdataDropdownsAndDivSelectRegions();
			}
		}
	
		protected override void OnUserCultureChanged(EventArgs e)
		{
			BindMapdataDropdownsAndDivSelectRegions();
		}


		
		
		private void BindMapdataDropdownsAndDivSelectRegions()
		{
		}
		#endregion
		#region Controller Methods
			
			public void SaveMapdata(object sender, EventArgs e)
			{
				if (Validate()){
					OrderTrackingGpsDAO.Save(Mapdata);
				}
			
			}
		
		#endregion
	}
	#endregion
		
}




