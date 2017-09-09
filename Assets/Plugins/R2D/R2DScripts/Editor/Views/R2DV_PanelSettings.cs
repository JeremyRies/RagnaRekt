//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;

	public class R2DV_PanelSettings {

		static R2DV_PanelSettings instance;
		
		public static R2DV_PanelSettings Instance {
			get {
				if( instance == null ) {
					instance = new R2DV_PanelSettings(); 
				}
				return instance;
			}
		}


		R2DV_Drawing drawing;
		R2DD_Resources resources;
		R2DC_Utils utils;
		R2DD_State state;

		private R2DV_PanelSettings() {
			drawing = R2DV_Drawing.Instance;
			resources = R2DD_Resources.Instance;
			utils = R2DC_Utils.Instance;
			state = R2DD_State.Instance;
		}

		public void DrawGUI() {
			// grab data
			R2DC_Settings controller = R2DC_Settings.Instance;
			List<string> contextNames = controller.contextNames;

			// title
			drawing.BeginEditorHorizontal();
			drawing.DrawPanelTitle( R2DD_Lang.titleRulerSettings, resources.panelSettings );
			drawing.FlexibleSpace();

			if( drawing.DrawImageButton( resources.help ) ) {
				Help.BrowseURL( resources.urlSettingsHelp );
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

			// context
			drawing.DrawPanelLabel( R2DD_Lang.context );

			int oldContextInstanceId = state.context.instanceId;
			int contextIndex = drawing.DrawPopup( controller.contextIndex, contextNames.ToArray() );
			int newContextInstanceId = controller.availableContexts[contextIndex].instanceId;

			if( oldContextInstanceId != newContextInstanceId ) {
				controller.SetContext( contextIndex );
				utils.RepaintSceneView();
			}
			drawing.DrawSpace( 9f );

			// show coordinates
			bool oldDisplayCoords = state.displayCoords;
			state.displayCoords = drawing.DrawToggleWithWidth( R2DD_Lang.displaySelectedCoords, state.displayCoords, toggleWidth );
			if( oldDisplayCoords != state.displayCoords ) {
				utils.RepaintSceneView();
			}

			// prefer colliders
			state.preferColliders = drawing.DrawToggleWithWidth( R2DD_Lang.preferColliders, state.preferColliders, toggleWidth );

			// show/hide guides
			bool oldDisplayGuides = state.displayGuides;
			state.displayGuides = drawing.DrawToggleWithWidth( R2DD_Lang.displayGuides, state.displayGuides, toggleWidth );
			if( oldDisplayGuides != state.displayGuides ) {
				utils.RepaintSceneView();
			}

			// snap guide to int
			state.snapGuideToInt = drawing.DrawToggleWithWidth( R2DD_Lang.snapGuideToInt, state.snapGuideToInt, toggleWidth );

			// use edges for snapping
			state.snapEdges = drawing.DrawToggleWithWidth( R2DD_Lang.lblUseEdgesForSnap, state.snapEdges, toggleWidth );

			// logo
			drawing.FlexibleSpace();
			drawing.BeginEditorHorizontal();
			drawing.FlexibleSpace();
			drawing.DrawPanelTexture( resources.logo );
			drawing.FlexibleSpace();
			drawing.EndEditorHorizontal();
			drawing.DrawSpace( 5f );
		}

		const float toggleWidth = 180f;
	}
}
