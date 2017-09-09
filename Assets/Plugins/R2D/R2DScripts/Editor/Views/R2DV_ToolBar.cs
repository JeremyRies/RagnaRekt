//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;
	
	public class R2DV_Toolbar {
		
		static R2DV_Toolbar instance;
		
		public static R2DV_Toolbar Instance {
			get {
				if( instance == null ) {
					instance = new R2DV_Toolbar(); 
				}
				return instance;
			}
		}
		
		R2DD_State state;
		R2DV_Drawing drawing;
		R2DD_Resources resources;

		private R2DV_Toolbar() {
			state = R2DD_State.Instance;
			drawing = R2DV_Drawing.Instance;
			resources = R2DD_Resources.Instance;
		}
		
		public void DrawGUI() {
			drawing.BeginEditorVertical();
			
			// toolbar
			Texture[] toolBarIcons = new Texture[3];
			toolBarIcons[0] = resources.barToolbox;
			toolBarIcons[1] = resources.panelGrid;
			toolBarIcons[2] = resources.panelSettings;

			state.toolBar = drawing.DrawToolbar( state.toolBar, toolBarIcons );
			if( state.toolBar == 0 ) {
				R2DV_PanelToolbox.Instance.DrawGUI();
			}
			else if( state.toolBar == 1 ) {
				R2DV_PanelGrid.Instance.DrawGUI();
			}
			else if( state.toolBar == 2) {
				R2DV_PanelSettings.Instance.DrawGUI();
			}

			drawing.EndEditorVertical();
		}
	}
}
