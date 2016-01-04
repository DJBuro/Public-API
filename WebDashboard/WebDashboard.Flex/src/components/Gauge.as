package components
{
	public class Gauge
	{
		[Bindable]
		public var _gaugeDiameter:Number = 140;

		public var _gaugeCurrentValue:Number;
		
		
		//What the gaugeLabels can have
		private var minuteLabels : Array = [0, 10, 20, 30, 40, 50, 60];      
		private var hundredLabels : Array = [0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100];           
		private var fourLabels : Array = [0, 1, 2, 3];
		
		
		private var _gaugeHandLocation:Number;
		
		private var _gaugeTextLocation:Number;
		

		private var _gaugeHandLength:Number = 70;

		private var _fontSize:Number = 16;
		
		/*CTOR*/
		public function Gauge(gaugeDiameter:Number)
		{
			this._gaugeDiameter = gaugeDiameter;
		}
		public function get currentValue() : Number
		{
			return _gaugeCurrentValue;
		}
		public function set currentValue( value:Number ) : void
		{
			_gaugeCurrentValue = value;
		}
		

	}
}