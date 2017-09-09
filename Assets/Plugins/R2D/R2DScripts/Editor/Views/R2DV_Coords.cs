//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;
	
	public class R2DV_Coords {
		static R2DV_Coords instance;
		
		public static R2DV_Coords Instance {
			get {
				if( instance == null ) {
					instance = new R2DV_Coords();
				}
				return instance;
			}
		}
		
		R2DFrame viewPixelFrame;
		R2DV_Drawing drawing;
		R2DD_State state;
		R2DC_Guides guides;
		R2DC_Utils utils;
		R2DC_Measure measure;

		private R2DV_Coords() {
			viewPixelFrame 	= R2DD_ContextInfo.Instance.viewPixelFrame;
			drawing 		= R2DV_Drawing.Instance;
			state			= R2DD_State.Instance;
			guides			= R2DC_Guides.Instance;
			utils			= R2DC_Utils.Instance;
			measure			= R2DC_Measure.Instance;
		}

		public void DrawCoords() {
			drawing.BeginGUIArea( displayCoordXOffset, displayCoordYOffset, viewPixelFrame.width, viewPixelFrame.height );
			drawing.BeginGUIVertical();

			if( measure.IsMeasureToolActive() ) {
				ExecDrawMeasureAlert();
			}

			if( state.displayCoords ) {
				List<Transform> transforms = R2DC_Selection.Instance.GetSelection();
				for( int i = 0; i < transforms.Count; i++ ) {
					Transform transform = transforms[i];

					if( transform == null ) {
						// selection must be dirty
						R2DC_Selection.Instance.UpdateSelection();
						return;
					}

					ExecDrawCoord( transform.name, transform.localPosition.x, transform.localPosition.y, transform.localPosition.z );
				}
			}

			// live guides
			Vector2 liveGuide = guides.liveGuide;
			if ( liveGuide.x != 0 ) {
				float x = utils.GetWorldToContextX( liveGuide.x );
				if( state.snapGuideToInt ) {
					x = Mathf.RoundToInt( x );
				}
				ExecDrawCoord( R2DD_Lang.guide, x, 0, 0 );
			}
			else if( liveGuide.y != 0 ) {
				float y = utils.GetWorldToContextY( liveGuide.y );
				if( state.snapGuideToInt ) {
					y = Mathf.RoundToInt( y );
				}
				ExecDrawCoord( R2DD_Lang.guide, 0, y, 0 );
			}
			else {
				// hover guides
				Vector2 hoverGuide = guides.hoverGuide;
				if( hoverGuide.x != 0 ) {
					float x = utils.GetWorldToContextX( hoverGuide.x );
					if( x == Globals.FALSE_ZERO ) {
						x = 0;
					}
					ExecDrawCoord( R2DD_Lang.guide, x, 0, 0 );
				}
				else if( hoverGuide.y != 0 ) {
					float y = utils.GetWorldToContextY( hoverGuide.y );
					if( y == Globals.FALSE_ZERO ) {
						y = 0;
					}
					ExecDrawCoord( R2DD_Lang.guide, 0, y, 0 );
				}
			}

			drawing.EndGUIVertical();
			drawing.EndGUIArea();

		}
		
		void ExecDrawCoord( string name, float x, float y, float z ) {			
			drawing.DrawSpace();
			drawing.BeginGUIHorizontal();
			drawing.DrawCoords( name + " (" + x + ", " + y + ", " + z + ")");
			drawing.EndGUIHorizontal();
		}

		void ExecDrawMeasureAlert() {			
			drawing.DrawSpace();
			drawing.BeginGUIHorizontal();
			drawing.DrawMeasureAlert( R2DD_Lang.measureAlert );
			drawing.EndGUIHorizontal();
		}
					
		const int displayCoordXOffset	= 23;
		const int displayCoordYOffset	= 22;
	}
}