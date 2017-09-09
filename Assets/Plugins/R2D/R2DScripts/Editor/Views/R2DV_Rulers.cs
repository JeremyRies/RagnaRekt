//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {

	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;

	public class R2DV_Rulers {
		static R2DV_Rulers instance;
		
		public static R2DV_Rulers Instance {
			get {
				if( instance == null ) {
					instance = new R2DV_Rulers();
				}
				return instance;
			}
		}

		Scale[] scaleTable;
		R2DV_Drawing drawing;
		R2DFrame viewPixelFrame;
		R2DFrame viewUnitFrame;
		R2DD_Resources resources;
		R2DD_ContextInfo contextInfo;
		R2DC_Utils utils;

		private R2DV_Rulers() {
			contextInfo 	= R2DD_ContextInfo.Instance;
			drawing 		= R2DV_Drawing.Instance;
			viewUnitFrame 	= contextInfo.viewUnitFrame;
			viewPixelFrame 	= contextInfo.viewPixelFrame;
			resources 		= R2DD_Resources.Instance;
			utils			= R2DC_Utils.Instance;

			InitScaleTable();
		}

		public void DrawRulers() {
			DrawHRulers();
			DrawVRulers();
			DrawCorner();

			if( Selection.transforms.Length > 0 ) {
				DrawCrossHairs();
			}
		}

		void DrawHRulers() {
			drawing.DrawTexture( resources.rulerHBg, 0, 0, (int)viewPixelFrame.width, stripBgSize );

			float camSize = contextInfo.editorCameraSize / contextInfo.scale.x;
			float rawSegmentUnitWidth = scaleTable[ GetScaleKey( camSize ) ].cap;
			float segmentUnitWidth = rawSegmentUnitWidth * contextInfo.scale.x;

			float startingLeftUnit = viewUnitFrame.botLeft.x - segmentUnitWidth;
			startingLeftUnit = startingLeftUnit - (startingLeftUnit % segmentUnitWidth);
			
			float endingRightUnit = viewUnitFrame.botRight.x + segmentUnitWidth;
			endingRightUnit = endingRightUnit - (endingRightUnit % segmentUnitWidth);

			startingLeftUnit += contextInfo.origin.x % segmentUnitWidth;;

			float runningUnit = startingLeftUnit; 
			float runningPixel = utils.GetWorldToScreenPixelCoord( new Vector3( runningUnit, viewUnitFrame.topLeft.y ) ).x;
			float runningPixelCap = utils.GetWorldToScreenPixelCoord( new Vector3( runningUnit + segmentUnitWidth, viewUnitFrame.topLeft.y ) ).x;
			float innerSlicesDist = ( runningPixelCap - runningPixel ) / 4f;

			while ( runningUnit < endingRightUnit ) {
				// Draw main coord
				DrawVLine( runningPixel, viewPixelFrame.topLeft.y, stripHiSize, resources.rulerLinePixel );
				string virtualXCoord = FormatNumber( utils.GetWorldToContextX( runningUnit ), camSize );
				DrawVLineLabel( virtualXCoord, runningPixel + vLineLabelXOffset, viewPixelFrame.topLeft.y + vLineLabelYOffset );
			
				// Draw slices
				runningPixel += innerSlicesDist;
				DrawVLine( runningPixel, viewPixelFrame.topLeft.y + stripLowOffset, stripLowSize, resources.rulerLinePixel );
				runningPixel += innerSlicesDist;
				DrawVLine( runningPixel, viewPixelFrame.topLeft.y + stripMidOffset, stripMidSize, resources.rulerLinePixel );
				runningPixel += innerSlicesDist;
				DrawVLine( runningPixel, viewPixelFrame.topLeft.y + stripLowOffset, stripLowSize, resources.rulerLinePixel );

				// Move to next segment
				runningUnit += segmentUnitWidth;
				runningPixel = utils.GetWorldToScreenPixelCoord( new Vector3( runningUnit, viewUnitFrame.topLeft.y ) ).x;
			}
		}

		void DrawVRulers() {
			drawing.DrawTexture( resources.rulerVBg, 0, 0, stripBgSize, (int)viewPixelFrame.height );

			float camSize = contextInfo.editorCameraSize / contextInfo.scale.y;
			float rawSegmentUnitHeight = scaleTable[ GetScaleKey( camSize ) ].cap;
			float segmentUnitHeight = rawSegmentUnitHeight * contextInfo.scale.y;
			
			float startingBotUnit = viewUnitFrame.botLeft.y - segmentUnitHeight;
			startingBotUnit = startingBotUnit - (startingBotUnit % segmentUnitHeight);

			float endingTopUnit = viewUnitFrame.topLeft.y + segmentUnitHeight;
			endingTopUnit = endingTopUnit - (endingTopUnit % segmentUnitHeight);

			startingBotUnit += contextInfo.origin.y % segmentUnitHeight;;

			float runningUnit = startingBotUnit; 
			float runningPixel = viewPixelFrame.height - utils.GetWorldToScreenPixelCoord( new Vector3( viewUnitFrame.topLeft.x, runningUnit ) ).y; 
			float runningPixelCap = viewPixelFrame.height - utils.GetWorldToScreenPixelCoord( new Vector3( viewUnitFrame.topLeft.x, runningUnit + segmentUnitHeight ) ).y; 
			float innerSlicesDist = ( runningPixelCap - runningPixel ) / 4f; 

			while ( runningUnit < endingTopUnit ) {
				// Draw main coord
				DrawHLine( viewPixelFrame.topLeft.x, runningPixel, stripHiSize, resources.rulerLinePixel );
				string virtualYCoord = FormatNumber( utils.GetWorldToContextY( runningUnit ), camSize );
				DrawHLineLabel( virtualYCoord.ToString(), viewPixelFrame.topLeft.x + hLineLabelXOffset, runningPixel + hLineLabelYOffset );
			
				// Draw slices
				runningPixel += innerSlicesDist;
				DrawHLine( viewPixelFrame.topLeft.x + stripLowOffset, runningPixel, stripLowSize, resources.rulerLinePixel );
				runningPixel += innerSlicesDist;
				DrawHLine( viewPixelFrame.topLeft.x + stripMidOffset, runningPixel, stripMidSize, resources.rulerLinePixel );
				runningPixel += innerSlicesDist;
				DrawHLine( viewPixelFrame.topLeft.x + stripLowOffset, runningPixel, stripLowSize, resources.rulerLinePixel );

				// Move to next segment
				runningUnit += segmentUnitHeight;
				runningPixel =  viewPixelFrame.height - utils.GetWorldToScreenPixelCoord( new Vector3( viewUnitFrame.topLeft.x, runningUnit ) ).y;

			}
		}

		void DrawCrossHairs() {
			int numSelections = Selection.transforms.Length;
			Transform[] transforms = Selection.transforms;

			for( int i = 0; i < numSelections; i++ ) {
				float xPixel = utils.GetWorldToScreenPixelCoord( new Vector3( transforms[i].position.x, viewUnitFrame.topLeft.y ) ).x;
				DrawVLine( xPixel, viewPixelFrame.topLeft.y + stripMidOffset, stripMidSize, resources.crossHairPixel );

				float yPixel = viewPixelFrame.height - utils.GetWorldToScreenPixelCoord( new Vector3( viewUnitFrame.topLeft.x, transforms[i].position.y ) ).y;
				DrawHLine( viewPixelFrame.topLeft.x + stripMidOffset, yPixel, stripMidSize, resources.crossHairPixel );
			}
		}

		string FormatNumber( float num, float camSize ) {
			if( camSize > 25000 ) {
				return "";
			}

			num = Mathf.Round( num * 2f ) / 2f;
			string prefix = "";
			if ( num < 0 ) {
				prefix = "-";
			}
			num = Mathf.Abs( num );

			if (num >= 1000000)
				return prefix + (num / 1000000).ToString("0.#") + "M";
			
			if (num >= 100000)
				return prefix + (num / 1000).ToString("#0K");
			
			if (num >= 10000)
				return prefix + (num / 1000).ToString("0.#") + "K";

			
			return prefix + num.ToString();
		}
		
		void DrawVLine( float xPos, float yPos, int height, Texture2D texture ) {
			drawing.DrawTexture( 	texture, 
			                        Mathf.FloorToInt( xPos ),
			                        Mathf.FloorToInt( yPos ),
			                        1,
			                        height );
		}

		void DrawVLineLabel( string text, float xPos, float yPos ) {
			drawing.DrawPanelLabel( text, 
			                      Mathf.FloorToInt( xPos ), 
			                      Mathf.FloorToInt( yPos ) );
		}

		void DrawHLine( float xPos, float yPos, int width, Texture2D texture ) {

			drawing.DrawTexture( 	texture, 
			                        Mathf.CeilToInt( xPos ),
			                        Mathf.CeilToInt( yPos ),
			                        width,
			                        1 );
		}
		
		void DrawHLineLabel( string text, float xPos, float yPos ) {
			drawing.DrawVLabel( text, 
			                    Mathf.CeilToInt( xPos ),
			                    Mathf.CeilToInt( yPos ) );
		}


		void DrawCorner() {
			Texture2D corner = resources.corner;
			drawing.DrawTexture( corner, 0, 0, corner.width, corner.height );
		}




		

		struct Scale {
			public float cap;
			
			public Scale( float cap ) {
				this.cap = cap;
			}
		};

		int GetScaleKey( float cameraSize ) {
			int key;
			if ( cameraSize < 1.5 )			key = 1;
			else if ( cameraSize < 5 )		key = 1;
			else if ( cameraSize < 30 ) 	key = 2;
			else if ( cameraSize < 55 ) 	key = 3;
			else if ( cameraSize < 250 ) 	key = 4;
			else if ( cameraSize < 500 ) 	key = 5;
			else if ( cameraSize < 2000 ) 	key = 6;
			else if ( cameraSize < 5000 ) 	key = 7;
			else 							key = 8;
			return key;
		}

		void InitScaleTable() {
			scaleTable = new Scale[9];
			scaleTable[ 0 ] = new Scale( 0.5f	);
			scaleTable[ 1 ] = new Scale( 1		);
			scaleTable[ 2 ] = new Scale( 5		);
			scaleTable[ 3 ] = new Scale( 10		);
			scaleTable[ 4 ] = new Scale( 50		);
			scaleTable[ 5 ] = new Scale( 100	);
			scaleTable[ 6 ] = new Scale( 500	);
			scaleTable[ 7 ] = new Scale( 1000	);
			scaleTable[ 8 ] = new Scale( 5000	);
		}

		void DrawPixelTest() {
			DrawPixel( viewPixelFrame.topLeft ); 
			DrawPixel( viewPixelFrame.botLeft );
			DrawPixel( viewPixelFrame.topRight ); 
			DrawPixel( viewPixelFrame.botRight ); 
		}
		
		void DrawPixel( Vector2 coords ) {
			int x = (int)coords.x;
			int y = (int)coords.y;
			drawing.DrawTexture( resources.pixel, x, y, 1, 1 );
			
		}

		const int vLineLabelXOffset	 = 4;
		const int vLineLabelYOffset  = 3;
		const int hLineLabelXOffset  = 2;
		const int hLineLabelYOffset  = 4;
		
		const int cornerSize		= 20;
		const int stripBgSize	 	= 18;
		const int stripHiSize 	 	= 18;
		const int stripMidSize  	= 6;
		const int stripMidOffset 	= 11;
		const int stripLowSize  	= 4;
		const int stripLowOffset 	= 13;

		const float scaleShiftThreshold = 22f;
	}
}