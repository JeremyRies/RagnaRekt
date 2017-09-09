//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {

	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;

	public class R2DV_Grid {
		static R2DV_Grid instance;

		public static R2DV_Grid Instance {
			get {
				if( instance == null ) {
					instance = new R2DV_Grid();
				}
				return instance;
			}
		}

		R2DV_Drawing drawing;
		R2DD_Resources resources;
		R2DD_State state;
		R2DC_Grid grid;
		R2DC_Utils utils;
		R2DFrame viewPixelFrame;

		private R2DV_Grid() {
			state 				= R2DD_State.Instance;
			drawing 			= R2DV_Drawing.Instance;
			resources 			= R2DD_Resources.Instance;
			grid				= R2DC_Grid.Instance;
			utils				= R2DC_Utils.Instance;
			viewPixelFrame 		= R2DD_ContextInfo.Instance.viewPixelFrame;
		}

		public void DrawGrid() {
			if( !state.gridEnabled ) {
				return;
			}

			GridData gridData = grid.GetGridData();

			// calc pixel data
			Vector2 pixelOrigin = utils.GetWorldToScreenPixelCoord( gridData.worldOrigin );
			pixelOrigin.y = viewPixelFrame.height - pixelOrigin.y;

			Vector2 pixelUnitSize = 
				utils.GetWorldToScreenPixelCoord( gridData.worldUnit ) -
				utils.GetWorldToScreenPixelCoord( gridData.worldZero );

			float pixelWidth = state.gridCols * pixelUnitSize.x;
			float pixelHeight = state.gridRows * pixelUnitSize.y;

			// draw rows
			for( int i = 0; i < state.gridRows + 1; i++ ) {
				Texture2D tex = ( i == 0 || i == state.gridRows ) ? resources.gridBorder : resources.measurePixel;
				drawing.DrawFloatTexture( 
					tex,
					pixelOrigin.x,
					pixelOrigin.y - ( i * pixelUnitSize.y ), 
					pixelWidth,
					1f );
			}

			// draw cols 
			for( int i = 0; i < state.gridCols + 1; i++ ) {
				Texture2D tex = ( i == 0 || i == state.gridCols ) ? resources.gridBorder : resources.measurePixel;
				drawing.DrawFloatTexture( 
					tex,
					pixelOrigin.x + ( i * pixelUnitSize.x ),
					pixelOrigin.y - pixelHeight, 
					1f,
					pixelHeight );
			}
		}
	}
}
