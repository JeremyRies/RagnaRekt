//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™ 
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;
	
	public class R2DV_PanelToolbox {
		
		static R2DV_PanelToolbox instance;
		
		public static R2DV_PanelToolbox Instance {
			get {
				if( instance == null ) {
					instance = new R2DV_PanelToolbox(); 
				}
				return instance;
			}
		}

		R2DV_Drawing drawing;
		R2DD_Resources resources;
		R2DC_Movement movement;
		R2DD_State state;
		R2DC_Utils utils;
		R2DC_Measure measure;
		R2DC_Selection selection;
		
		private R2DV_PanelToolbox() {
			drawing 	= R2DV_Drawing.Instance;
			resources 	= R2DD_Resources.Instance;
			movement 	= R2DC_Movement.Instance;
			state 		= R2DD_State.Instance;
			utils 		= R2DC_Utils.Instance;
			measure 	= R2DC_Measure.Instance;
		}
		
		public void DrawGUI() {
			bool alignEnabled = movement.alignEnabled;

			// title
			drawing.BeginEditorHorizontal();
			drawing.DrawPanelTitle( R2DD_Lang.titleToolbox, resources.panelToolbox );
			drawing.FlexibleSpace();
			
			if( drawing.DrawImageButton( resources.help ) ) {
				Help.BrowseURL( resources.urlToolboxHelp );
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

			// align tools
			drawing.DrawCenteredLabel( R2DD_Lang.lblAlign );

			drawing.BeginEditorHorizontal();
			drawing.FlexibleSpace();
			drawing.DrawSpace( 4f );
			if( drawing.DrawToolButton( resources.alignTop, alignEnabled, R2DD_Lang.tipAlignTop ) ) {
				movement.AlignTop();
			}
			if( drawing.DrawToolButton( resources.alignYMid, alignEnabled, R2DD_Lang.tipAlignHMid ) ) {
				movement.AlignYMid();
			}
			if( drawing.DrawToolButton( resources.alignBot, alignEnabled, R2DD_Lang.tipAlignBot ) ) {
				movement.AlignBot();
			}
			if( drawing.DrawToolButton( resources.alignLeft, alignEnabled, R2DD_Lang.tipAlignLeft ) ) {
				movement.AlignLeft();
			}
			if( drawing.DrawToolButton( resources.alignXMid, alignEnabled, R2DD_Lang.tipAlignVMid ) ) {
				movement.AlignXMid();
			}
			if( drawing.DrawToolButton( resources.alignRight, alignEnabled, R2DD_Lang.tipAlignRight ) ) {
				movement.AlignRight();
			}
			drawing.FlexibleSpace();
			drawing.EndEditorHorizontal();

			// Distribute tools
			bool distroEnabled = movement.distroEnabled;

			drawing.DrawSpace( 6f );
			drawing.DrawCenteredLabel( R2DD_Lang.lblDistribute );
			
			drawing.BeginEditorHorizontal();
			drawing.FlexibleSpace();
			drawing.DrawSpace( 4f );
			if( drawing.DrawToolButton( resources.distroTop, distroEnabled, R2DD_Lang.tipDistTop ) ) {
				movement.DistroTop();
			}

			if( drawing.DrawToolButton( resources.distroYMid, distroEnabled, R2DD_Lang.tipDistYMid ) ) {
				movement.DistroYMid();
			}

			if( drawing.DrawToolButton( resources.distroBot, distroEnabled, R2DD_Lang.tipDistBot ) ) {
				movement.DistroBot();
			}

			if( drawing.DrawToolButton( resources.distroLeft, distroEnabled, R2DD_Lang.tipDistLeft ) ) {
				movement.DistroLeft();
			}

			if( drawing.DrawToolButton( resources.distroXMid, distroEnabled, R2DD_Lang.tipDistXMid ) ) {
				movement.DistroXMid();
			}

			if( drawing.DrawToolButton( resources.distroRight, distroEnabled, R2DD_Lang.tipDistRight ) ) {
				movement.DistroRight();
			}
			drawing.FlexibleSpace();
			drawing.EndEditorHorizontal();

			// Spacing tools
			bool spaceEnabled = movement.spaceEnabled;

			drawing.DrawSpace( 6f );
			drawing.DrawCenteredLabel( R2DD_Lang.lblSpace );

			drawing.BeginEditorHorizontal();
			drawing.FlexibleSpace();
			bool spaceXPressed = drawing.DrawToolButton( resources.spaceX, spaceEnabled, R2DD_Lang.tipSpaceX );

			drawing.BeginEditorVertical();
			drawing.DrawSpace( 9f );
			state.spaceX = drawing.DrawFloatFieldWithWidth( "", state.spaceX );
			drawing.EndEditorVertical();

			if( spaceXPressed ) {
				movement.SpaceX( state.spaceX );
			}

			bool spaceYPressed = drawing.DrawToolButton( resources.spaceY, spaceEnabled, R2DD_Lang.tipSpaceY );

			drawing.BeginEditorVertical();
			drawing.DrawSpace( 9f );
			state.spaceY = drawing.DrawFloatFieldWithWidth( "", state.spaceY );
			drawing.EndEditorVertical();

			if( spaceYPressed ) {
				movement.SpaceY( state.spaceY );
			}

			drawing.FlexibleSpace();
			drawing.EndEditorHorizontal();

			// Guide Snap Tool
			bool guideSnapEnabled = movement.guideSnapEnabled;
			
			drawing.DrawSpace( 6f );
			drawing.DrawCenteredLabel( R2DD_Lang.lblSnapToGuide );
			
			drawing.BeginEditorHorizontal();
			drawing.FlexibleSpace();
			drawing.DrawSpace( 4f );

			bool leftRightEnabled = state.vGuides.Count > 0 && guideSnapEnabled && state.displayGuides ? true : false;
			bool topDownEnabled = state.hGuides.Count > 0  && guideSnapEnabled && state.displayGuides ? true : false;

			if( drawing.DrawToolButton( resources.snapLeft, leftRightEnabled, R2DD_Lang.tipSnapLeft ) ) {
				movement.SnapLeft();
			}
			
			if( drawing.DrawToolButton( resources.snapRight, leftRightEnabled, R2DD_Lang.tipSnapRight ) ) {
				movement.SnapRight();
			}
			
			if( drawing.DrawToolButton( resources.snapTop, topDownEnabled, R2DD_Lang.tipSnapTop ) ) {
				movement.SnapTop();
			}
			
			if( drawing.DrawToolButton( resources.snapBot, topDownEnabled, R2DD_Lang.tipSnapBot ) ) {
				movement.SnapBot();
			}

			drawing.FlexibleSpace();
			drawing.EndEditorHorizontal();


			// Measure
			drawing.DrawSpace( 6f );
			drawing.DrawCenteredLabel( R2DD_Lang.lblMeasure );

			drawing.BeginEditorHorizontal();
			drawing.FlexibleSpace();
			drawing.DrawSpace( 4f );

			Texture measureButton = measure.IsMeasureToolActive() ? resources.measureActive : resources.measureInactive;

			if( drawing.DrawToolButton( measureButton, true, R2DD_Lang.tipMeasure ) ) {
				measure.ToggleMeasureTool();
			}

			if( drawing.DrawToolButton( resources.measureObj, measure.IsMeasureObjEnabled(), R2DD_Lang.tipMeasureObj ) ) {
				measure.MeasureObj();
			}

			if( drawing.DrawToolButton( resources.clearMeasure, true, R2DD_Lang.tipClearMeasure ) ) {
				measure.ClearMeasureTool();
			}

			drawing.FlexibleSpace();
			drawing.EndEditorHorizontal();

			// clear guides
			drawing.DrawSpace( 9f );
			if( drawing.DrawButton( R2DD_Lang.clearGuides ) ) {
				state.hGuides.Clear();
				state.vGuides.Clear();
				utils.RepaintSceneView();
			}

			// tip
			drawing.DrawSpace( 4f );
			if( movement.error == R2DC_Movement.ADError.NGUIContext ) {
				drawing.DrawErrorBox( R2DD_Lang.nguiInvalidContext );
			}
			else if( movement.error == R2DC_Movement.ADError.NGUIReflection ) {
				drawing.DrawErrorBox( R2DD_Lang.nguiError );
			}
			else if( movement.error == R2DC_Movement.ADError.CanvasContext ) {
				drawing.DrawErrorBox( R2DD_Lang.canvasInvalidContext );
			}
			else {
				drawing.DrawHelpBox( R2DD_Lang.guideHelp );
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
