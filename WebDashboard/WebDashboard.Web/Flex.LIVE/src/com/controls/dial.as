package com.controls
{
	import mx.core.UIComponent;

	public class dial extends UIComponent
	{
		
		//What the gaugeLabels can have
        private var minuteLabels : Array = [0, 10, 20, 30, 40, 50, 60];      
        private var hundredLabels : Array = [0, 10, 20, 30, 40, 50, 60, 70, 80, 90, 100];           
        private var fourLabels : Array = [0, 1, 2, 3];
        
        [Bindable]
        private var _gaugeAngle:int;
        
        [Bindable]
        public var _GAUGESTART : Number = 210;
        
        [Bindable]
        public var _TickDivisor : Number = 10;
        
        [Bindable]
        private var _gaugeRadius:Number = 300;
        
        [Bindable]
        private var _columnWidth:Number = 300;
        
        [Bindable]
        private var _applicationWidth:Number;
		
		[Bindable]
        private var _gaugeHandLocation:Number;
        
        [Bindable]
        private var _gaugeTextLocation:Number;
        
        [Bindable]
        private var _gaugeHandLength:Number = 140;
	
		[Bindable]
        private var _gaugeDistance:Number = 140;
        
        [Bindable]
        public var _columnHeight : Number = 600;
        
        //gauge face etc.
        [Bindable]
        private var _gauge01MaxValue:int;
		[Bindable]
        private var _gauge01BenchmarkArc:Number;
        [Bindable]
        private var _gauge01GreenArc:Number;
        [Bindable]
        private var _gauge01GreenArcStart:Number;
        [Bindable]
        private var _gauge01MajorTickCount:Number;
        [Bindable]
        private var _gauge01MajorTickOffset:Number;
		
		public function dial()
		{
			super();
		}
		
		override protected function createChildren():void
		{
			super.createChildren();			
		}
		
		override protected function updateDisplayList (unscaledWidth : Number, unscaledHeight : Number) : void {
			super.updateDisplayList(unscaledWidth,unscaledHeight);					
		}
		
	}
}