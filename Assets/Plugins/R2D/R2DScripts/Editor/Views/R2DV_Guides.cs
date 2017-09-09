// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;
	
	public class R2DV_Guides {
		static R2DV_Guides instance;
		
		public static R2DV_Guides Instance {
			get {
				if( instance == null ) {
					instance = new R2DV_Guides();
				}
				return instance;
			}
		}
		
		R2DV_Drawing drawing;
		R2DFrame viewPixelFrame;
		R2DD_Resources resources;
		R2DC_Guides guidesController;
		R2DD_State state;
		R2DC_Utils utils;

		
		private R2DV_Guides() {
			state 				= R2DD_State.Instance;
			drawing 			= R2DV_Drawing.Instance;
			viewPixelFrame 		= R2DD_ContextInfo.Instance.viewPixelFrame;
			resources 			= R2DD_Resources.Instance;
			guidesController 	= R2DC_Guides.Instance;
			utils				= R2DC_Utils.Instance;
			
		}

		public void DrawGuides() {
			// live guides
			Vector2 liveGuides = guidesController.liveGuide;
			if( liveGuides.x != 0 ) {
				Vector2 pixelCoord = utils.GetWorldToScreenPixelCoord( new Vector2( liveGuides.x, 0 ) );
				drawing.DrawFloatTexture( resources.liveGuidePixel, pixelCoord.x, 18f, 1f, viewPixelFrame.height );

			
			}
			else if( liveGuides.y != 0 ) {
				Vector2 pixelCoord = utils.GetWorldToScreenPixelCoord( new Vector2( 0, liveGuides.y ) );
				pixelCoord.y = viewPixelFrame.height - pixelCoord.y;
				drawing.DrawFloatTexture( resources.liveGuidePixel, 18f, pixelCoord.y, viewPixelFrame.width, 1f );
			}

			// perma guides
			if( state.displayGuides ) {
				foreach( string xStr in state.vGuides ) {
					Vector2 pixelCoord = utils.GetWorldToScreenPixelCoord( new Vector2( float.Parse( xStr ), 0 ) );
					drawing.DrawFloatTexture( resources.guidePixel, pixelCoord.x, 18f, 1f, viewPixelFrame.height );
				}

				foreach( string yStr in state.hGuides ) {
					Vector2 pixelCoord = utils.GetWorldToScreenPixelCoord( new Vector2( 0, float.Parse( yStr ) ) );
					pixelCoord.y = viewPixelFrame.height - pixelCoord.y;
					drawing.DrawFloatTexture( resources.guidePixel, 18f, pixelCoord.y, viewPixelFrame.width, 1f );
				}
			}
		}
	}
}
