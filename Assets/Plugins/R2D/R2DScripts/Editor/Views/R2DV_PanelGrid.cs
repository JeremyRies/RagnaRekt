//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {

	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;

	public class R2DV_PanelGrid {

		static R2DV_PanelGrid instance;

		public static R2DV_PanelGrid Instance {
			get {
				if( instance == null ) {
					instance = new R2DV_PanelGrid(); 
				}
				return instance;
			}
		}

		R2DV_Drawing drawing;
		R2DD_Resources resources;
		R2DC_Utils utils;
		R2DD_State state;
		R2DC_Grid grid;

		private R2DV_PanelGrid() {
			drawing = R2DV_Drawing.Instance;
			resources = R2DD_Resources.Instance;
			utils = R2DC_Utils.Instance;
			state = R2DD_State.Instance;
			grid = R2DC_Grid.Instance;
		}

		public void DrawGUI() {
			// title 
			drawing.BeginEditorHorizontal();
			drawing.DrawPanelTitle( R2DD_Lang.titleGrid, resources.panelGrid );
			drawing.FlexibleSpace();

			if( drawing.DrawImageButton( resources.help ) ) {
				Help.BrowseURL( resources.urlGridHelp );
			};

			drawing.EndEditorHorizontal();

			// Not in 2d error
			if( !utils.IsSceneViewIn2D() ) {
				drawing.DrawErrorBox( R2DD_Lang.sceneModeError );

				drawing.DrawSpace( 9f );
				if( drawing.DrawButton( R2DD_Lang.set2DSceneMode ) ) {
					utils.Set2DMode();
					utils.RepaintSceneView();
				}

				utils.RepaintEditorWindow();
				return;
			}


			// enable grid
			bool oldGridEnabled = state.gridEnabled;
			state.gridEnabled = drawing.DrawToggle( R2DD_Lang.gridEnabled, state.gridEnabled );
			if( oldGridEnabled != state.gridEnabled ) {
				utils.RepaintSceneView();
			}

			// grid columns
			int oldGridCols = state.gridCols;
			state.gridCols = drawing.DrawIntField( R2DD_Lang.gridCols, state.gridCols );
			if( state.gridCols < 2 ) {
				state.gridCols = 2;
			}
			if( oldGridCols != state.gridCols ) {
				utils.RepaintSceneView();
			}

			// grid rows
			int oldGridRows = state.gridRows;
			state.gridRows = drawing.DrawIntField( R2DD_Lang.gridRows, state.gridRows );
			if( state.gridRows < 2 ) {
				state.gridRows = 2;
			}
			if( oldGridRows != state.gridRows ) {
				utils.RepaintSceneView();
			}

			// grid x
			float oldGridX = state.gridX;
			state.gridX = drawing.DrawFloatField( R2DD_Lang.gridX, state.gridX );
			if( state.gridX < 0.1f ) {
				state.gridX = 0.1f;
			}
			if( oldGridX != state.gridX ) {
				utils.RepaintSceneView();
			}

			// grid y
			float oldGridY = state.gridY;
			state.gridY = drawing.DrawFloatField( R2DD_Lang.gridY, state.gridY );
			if( state.gridY < 0.1f ) {
				state.gridY = 0.1f;
			}
			if( oldGridY != state.gridY ) {
				utils.RepaintSceneView();
			}

			// grid origin x
			float oldGridOriginX = state.gridOriginX;
			state.gridOriginX = drawing.DrawFloatField( R2DD_Lang.gridOriginX, state.gridOriginX );
			if( oldGridOriginX != state.gridOriginX ) {
				utils.RepaintSceneView();
			}

			// grid origin y
			float oldGridOriginY = state.gridOriginY;
			state.gridOriginY = drawing.DrawFloatField( R2DD_Lang.gridOriginY, state.gridOriginY );
			if( oldGridOriginY != state.gridOriginY ) {
				utils.RepaintSceneView();
			}

			// snap to grid
			state.snapToGrid = drawing.DrawToggle( R2DD_Lang.snapToGrid, state.snapToGrid );

			// toggle unity grid
			// clear guides
			drawing.DrawSpace( 9f );
			if( drawing.DrawButton( R2DD_Lang.toggleUnityGrid ) ) {
				grid.ToggleUnityGrid();
				utils.RepaintSceneView();
			}

			// NGUI snap issue guide
			drawing.DrawSpace( 4f );
			if( state.context.type == ContextType.NGUI && state.snapToGrid ) {
				drawing.DrawHelpBox( R2DD_Lang.nguiGridHelp );
			}

			// logo
			drawing.FlexibleSpace();
			drawing.BeginEditorHorizontal();
			drawing.FlexibleSpace();
			drawing.DrawPanelTexture( resources.logo );
			drawing.FlexibleSpace();
			drawing.EndEditorHorizontal();
			drawing.DrawSpace( 5f );
		}
	}
}