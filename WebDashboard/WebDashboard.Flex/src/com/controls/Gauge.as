package com.controls
{
	import com.controls.gauge.GaugeSkin;
	
	import flash.display.DisplayObject;
	
	import mx.core.IFlexDisplayObject;
	import mx.core.UIComponent;
	import mx.styles.ISimpleStyleClient;
	import mx.core.IInvalidating;
	import mx.core.IProgrammaticSkin;
	/** faceColor
	 * The color for the background of the frame. Default: white
	 */
	[Style(name="faceColor", type="Number", format="Color", inherit="no")]

	public class Gauge extends UIComponent
	{
		/**
		 * PROPERTIES
		 *
		 * When a property is set, its value is copied to the class variable (eg,
		 * _value) and then invalidateDisplayList is called. This allows the Flex framework
		 * to call updateDisplayList at the proper time. For example, it is possible
		 * to set a property before there are any graphics present; calling updateDisplayList
		 * then would lead to an error.
		 */
		 
		private var _faceColor : String = "333333";
		private var theCaption : String = "";
		
		[Persistent]
		[Editable(Name="Caption")]
		[Bindable]
		public function set caption (s : String) : void {
			theCaption = s;
		}
		
		
		
		// Variables holding the skin instances
		protected var faceSkin 	: IFlexDisplayObject;		
		
		override public function set height (value : Number) : void {
			diameter = value;
		}
		
		override public function set width (value : Number) : void {
			diameter = value;
			
		}
		
		public function set diameter (value : Number) : void {
			super.width = value;
			super.height = value;
		}
		
		public function Gauge()
		{
			super();
		}
		
		override protected function updateDisplayList (unscaledWidth : Number, unscaledHeight : Number) : void {
			super.updateDisplayList(unscaledWidth,unscaledHeight);
			faceSkin.setActualSize(unscaledWidth, unscaledHeight);
					
		}
		
		override protected function createChildren () : void {
			super.createChildren();
			faceSkin  = createSkin("faceSkin", com.controls.gauge.GaugeSkin); 
			addChild(DisplayObject(faceSkin));
		}
		
		protected function createSkin (skinName : String, defaultSkin : Class ) : IFlexDisplayObject {
			// Look up the skin by its name to see if it is already created. Note
			// below where addChild() is called; this makes getChildByName possible.
			var newSkin : IFlexDisplayObject = IFlexDisplayObject(getChildByName(skinName));
			if (newSkin == null) {
				// Attempt to get the class for the skin. If one has not been supplied
				// by a style, use the default skin.
				
				var newSkinClass : Class = Class(getStyle(skinName));
				if(newSkinClass == null) 
					newSkinClass = defaultSkin;
				
				if (newSkinClass != null) {
					// Create an instance of the class.
					newSkin = IFlexDisplayObject(new newSkinClass());
					if (newSkin == null) 
						newSkin = new defaultSkin();
					
					// Set its name so that we can find it in the future
					// using getChildByName().
					newSkin.name = skinName;
	
					// Make the getStyle() calls in the skin class find the styles
					// for this Gauge instance. In other words, by setting the styleName
					// to 'this' it allows the skin to query the component for styles. For
					// example, when the skin code does getStyle('backgroundColor') it 
					// retrieves the style from this Gauge and not from the skin.
					var styleableSkin : ISimpleStyleClient = newSkin as ISimpleStyleClient;
					if (styleableSkin != null)
						styleableSkin.styleName = this;
						
					// If the skin is programmatic, and we've already been
	        // initialized, update it now to avoid flicker.
	        if (newSkin is IInvalidating) { 
	        	IInvalidating(newSkin).validateNow();
	        }
	        else if (newSkin is IProgrammaticSkin && initialized) {
	        	IProgrammaticSkin(newSkin).validateDisplayList()
	        }
					
				}
			}
			
			return newSkin;
		}
		
		public var _maximum : Number = 100;
		public var _minimum : Number = 0;
		public var _minimumAngle : Number = 0;
		public var _maximumAngle : Number = 200;
		public static const SCALE_LINEAR : String = "linear";
		private var _valueScale:String=SCALE_LINEAR;
		public static const SCALE_LOG : String = "log";

		[Bindable]
		[Persistent]
		public function get valueScale () : String {
			return _valueScale;
		}
		
        public function calculateAngleFromValue (v : Number) : Number {
	    	var max : Number = 100;
	    	var _maximum: Number = 100;
	    	if (max == 0)
	    		max = _maximum;
	    	
	    	var ratio : Number = (v - _minimum) / (max  -_minimum); //percentage value
	    	
	    	if (valueScale == SCALE_LOG) {       		
	    		//On less than 1 values set to 1 otherwise log function gets out-of-whack.
	    		if (v < 1) 
	    			v = 1;      		
	    			
	    		var computedMaximum : Number = Math.ceil(Math.log(max) / Math.LN10);
	    		var computedMinimum : Number = Math.floor(Math.log(_minimum) / Math.LN10);
	    		
		    		ratio = (Math.log(v) * Math.LOG10E) / (computedMaximum - computedMinimum);
		      }
		 
		 			var angle : Number = (_maximumAngle - _minimumAngle) * ratio + _minimumAngle;  
		 		
		    	return angle;
    	}

	}
}