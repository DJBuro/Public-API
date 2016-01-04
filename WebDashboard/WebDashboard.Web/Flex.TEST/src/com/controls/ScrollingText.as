package com.controls
{
	import flash.display.Shape;
	import flash.events.Event;
	
	import mx.controls.Text;
	import mx.core.UIComponent;
	import mx.messaging.channels.StreamingAMFChannel;

	/**
	 * Sets the size of the characters.
	 * @default 10
	 */
	[Style(name="fontSize",type="Number",inherit="yes")]
	
	/**
	 * Sets the weight (bold or normal) for the text.
	 * @default normal
	 */
	[Style(name="fontWeight",type="String",inherit="yes")]
	
	/**
	 * Sets the font family for the text.
	 * @default system
	 */
	[Style(name="fontFamily",type="String",inherit="yes")]
	
	/**
	 * Sets the color for the text.
	 * @default 0 (black)
	 */
	[Style(name="color",type="Number",format="Color",inherit="yes")]

	/**
	 * This component displays a string of moving text.
	 * 
	 * 
	 * The String to scroll is set using this component's <code>text</code> property. Constants can 
	 * set the <code>direction</code> of the scroll as well as the <code>speed</code>.
	 * 
	 * <pre>
	 * <ScrollingText text="Hello World" direction="rightToLeft" speed="5" width="100%" />
	 * </pre>
	 */
	public class ScrollingText extends UIComponent
	{
		/**
		 * Scrolls the text from the right side to the left side. This is the default.
		 */
		public static const RIGHT_TO_LEFT:String = "rightToLeft";
		
		/**
		 * Scrolls the text from the left side to the right side.
		 */
		public static const LEFT_TO_RIGHT:String = "leftToRight";
		
		/**
		 * Scrolls the text from the bottom to the top of the control.
		 */ 
		public static const BOTTOM_TO_TOP:String = "bottomToTop";
		
		/**
		 * Scrolls the text from the top to the bottom of the control.
		 */ 
		public static const TOP_TO_BOTTOM:String = "topToBottom";
		
		/**
		 * @private
		 */
		private var cache:Array;
		private var clipMask:Shape;
		private var currentIndex:int = 0;
		private var secondIndex:int = 1;
		private var messageText:String;
		private var messageSpeed:int = 5;
		private var textDirection:String = RIGHT_TO_LEFT;
		private var cachedText:String = "";
		
		/**
		 * Constructor
		 */
		public function ScrollingText()
		{
			super();
		}
		
		
		/**
		 * Creates the objects that make this component work. Two Texts are placed into a cache
		 * where they are used to scroll the text. As one Text disappears the other appears.
		 * 
		 * A mask is also created for the component to clip the text flowing out of the component's
		 * boundaries as UIComponent provides no clipping of its own.
		 */
		override protected function createChildren():void
		{
			super.createChildren();
			
			cache = [new Text(),new Text()];
			
			(cache[0] as Text).styleName = this;
			(cache[0] as Text).cacheAsBitmap = true;
			addChild(cache[0]);
			
			(cache[1] as Text).styleName = this;
			(cache[1] as Text).cacheAsBitmap = true;
			addChild(cache[1]);
			
			clipMask = new Shape();
			addChild(clipMask);
			mask = clipMask;
		}
		
		/**
		 * The text message to display in the marquee.
		 */
		public function get text() : String
		{
			return messageText;
		}
		public function set text( value:String ) : void
		{
			messageText = value;
			invalidateProperties();
		}
		
		/**		 * 
		 * Determines how fast the text flows. Minimum is 1.
		 * @default 5
		 */
		public function get speed() : int
		{
			return messageSpeed;
		}
		public function set speed( value:int ) : void
		{
			if( value < 1 ) value = 1;
			messageSpeed = value;
		}
		
		[Inspectable(type="String",enumeration="leftToRight,rightToLeft,topToBottom,bottomToTop")]
		/**
		 * Determines if the text scrolls horizontally (default) or vertically. Possible values are:
		 * <li>RIGHT_TO_LEFT</li>
		 * @default LEFT_TO_RIGHT
		 */
		public function get direction() : String
		{
			return textDirection;
		}
		public function set direction( value:String ) : void
		{
			textDirection = value;
			invalidateProperties();
		}
		
		[Bindable]
		/**
		 * If true, the text is flowing; false if the text is not flowing.
		 */
		public var running:Boolean = false;
		
		/**
		 * This function begins the text scrolling.
		 */
		public function start() : void
		{
			if( !running ) {
				addEventListener( Event.ENTER_FRAME, moveText );
			}
			running = true;
		}
		
		/**
		 * This function stops the text scrolling.
		 */
		public function stop() : void
		{
			if( running ) {
				removeEventListener( Event.ENTER_FRAME, moveText );
			}	
			running = false;
		}
		
		/*
		 * 
		 UPDATE [AndromedaDashboardServer].[dbo].[DashboardData]
   SET 
      [Column_20] = 10
 WHERE [SiteID] = 384
GO
		 */
		
		/**
		 * This Flex framework function is called once all of the properties have been set (or if
		 * invalidateProperties has been called). The Texts are given their text values and the
		 * orientation is set, if necessary, to comply with the direction.
		 */
		override protected function commitProperties() : void
		{
			if( text == null )  messageText = "";
			
			
			
			//if( cachedText == text ) {
			//	cache[0].text = text;
			//	cache[1].text = text;
			//}
			cachedText = text;
			
			if(currentIndex == 0)
			{
				if(cache[0].text == "")
				{
					cache[0].text = cachedText;
				}
			}
			
			
			
			// calling validateNow will help with determining the actual size of the Texts in
			// the measure method.
			//cache[0].validateNow();
			//cache[1].validateNow();
			
			 //Note... buggy
			 // 1 => 5 works ok
			 // 1 => 11 causes a reset...  extra digit, extra space... gets measured again.
			
			//TODO: test if this works???
			//invalidateSize();
			//invalidateDisplayList();
		}
		
		/**
		 * The job of measure() is to given reasonable values to measuredWidth and measuredHeight. The measure()
		 * framework function is called only if an explicit size cannot be determined.
		 */
		override protected function measure() : void
		{
			super.measure();
			
			//measuredWidth = 0;
			//measuredHeight= 0;
			
			// For each Text, set its actual size and then modified the measured width
			// and height.
			for(var i:int=0; i < cache.length; i++)
			{
				var l:Text = cache[i] as Text;
				
				l.setActualSize( l.getExplicitOrMeasuredWidth(), l.getExplicitOrMeasuredHeight() );
				
				measuredWidth = Math.max(measuredWidth,l.getExplicitOrMeasuredWidth());
				measuredHeight = Math.max(measuredHeight,l.getExplicitOrMeasuredHeight());
			}
		}
		
		/**
		 * The updateDisplayList function is called whenever the display list has become invalid. Here,
		 * the initial positions of the Texts are determined and the clipping mask is made.
		 */
		override protected function updateDisplayList(unscaledWidth:Number, unscaledHeight:Number):void
		{
			super.updateDisplayList(unscaledWidth,unscaledHeight);
			
			var Text0:Text = cache[0] as Text;
			//var Text1:Text = cache[1] as Text;
			
			var xpos:Number = (unscaledWidth - Text0.width)/2;
			var ypos:Number = (unscaledHeight- Text0.height)/2;
			
			switch( direction )
			{
				case RIGHT_TO_LEFT:
					Text0.move(unscaledWidth,ypos);
					//Text1.move(Math.max(unscaledWidth,Text0.width+10),ypos);
					break;
			}
			
			clipMask.graphics.clear();
			clipMask.graphics.beginFill(0);
			clipMask.graphics.drawRect(0,0,unscaledWidth,unscaledHeight);
			clipMask.graphics.endFill();
		}
		
		/**
		 * This function is the ENTER_FRAME event handler and it moves the text by measuredSpeed amounts.
		 * 
		 * The direction determines where the text is positioned and moved. When one Text
		 * reaches its destination (eg, RIGHT_TO_LEFT reaching the left edge), the second Text begins its
		 * journey. When that Text reaches the destination, the first Text starts, etc.
		 */
		private function moveText( event:Event ) : void
		{
			// HORIZONTAL
		
			// RIGHT_TO_LEFT
			if( direction == RIGHT_TO_LEFT ) {
				cache[currentIndex].x -= messageSpeed;
				if( cache[currentIndex].x <= 0 ) {
					cache[secondIndex].x -= messageSpeed;
				}
				if( cache[currentIndex].x+cache[currentIndex].width <= 0 ) {
					cache[currentIndex].x = Math.max(cache[currentIndex].parent.width,cache[secondIndex].width+10);
					currentIndex = 1 - currentIndex;
					secondIndex  = 1 - secondIndex;
					
					cache[0].text = cachedText;
				}

			}
			
		}
	}
}