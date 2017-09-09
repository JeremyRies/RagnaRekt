// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {

	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;

	public class R2DV_Measure {
		static R2DV_Measure instance;

		public static R2DV_Measure Instance {
			get {
				if( instance == null ) {
					instance = new R2DV_Measure();
				}
				return instance;
			}
		}

		R2DV_Drawing drawing;
		R2DFrame viewPixelFrame;
		R2DD_Resources resources;
		R2DC_Measure measureController;
		R2DD_State state;
		R2DC_Utils utils;

		private R2DV_Measure() {
			state 				= R2DD_State.Instance;
			drawing 			= R2DV_Drawing.Instance;
			viewPixelFrame 		= R2DD_ContextInfo.Instance.viewPixelFrame;
			resources 			= R2DD_Resources.Instance;
			measureController 	= R2DC_Measure.Instance;
			utils				= R2DC_Utils.Instance;
		}

		public void DrawMeasurements() {
			bool measurementAvailable = false;
			Vector2 worldStartCoord = Vector2.zero;
			Vector2 worldEndCoord = Vector2.zero;

			if( measureController.IsMeasuring() ) {
				worldStartCoord = measureController.startCoord;	
				worldEndCoord = measureController.currentCoord;
				measurementAvailable = true;
			}
			else if( state.measurements.Count > 0 ) {
				worldStartCoord = new Vector2(
					float.Parse( state.measurements[0] ),
					float.Parse( state.measurements[1] )
				);

				worldEndCoord = new Vector2(
					float.Parse( state.measurements[2] ),
					float.Parse( state.measurements[3] )
				);
				measurementAvailable = true;
			}
				
			if( measurementAvailable ) {
				Vector2 startCoord = utils.GetWorldToScreenPixelCoord( worldStartCoord );
				startCoord.y = viewPixelFrame.height - startCoord.y;
				Vector2 currentCoord = utils.GetWorldToScreenPixelCoord( worldEndCoord );
				currentCoord.y = viewPixelFrame.height - currentCoord.y;
				drawing.DrawLine( resources.measurePixel, startCoord, currentCoord );

				DrawCross( startCoord );
				DrawCross( currentCoord );

				bool below = currentCoord.y < startCoord.y + 5f;
				Measurement measurement = 
					measureController.GetMeasurement( worldStartCoord, worldEndCoord );

				drawing.DrawMeausureLabel( 
					"X:" + measurement.x.ToString( "F2" ) +
					" Y:" + measurement.y.ToString( "F2" )  +
					" W:" + measurement.w.ToString( "F2" )  +
					" H:" + measurement.h.ToString( "F2" )  +
					" A:" + measurement.angle.ToString( "F2" )  + "°" +
					" L: " + measurement.length.ToString( "F2" ) ,
					startCoord.x, startCoord.y, below );

			}

		}

		void DrawCross( Vector2 location ) {
			drawing.DrawSimpleTexture( resources.measureCross, location );
		}
	}
}
