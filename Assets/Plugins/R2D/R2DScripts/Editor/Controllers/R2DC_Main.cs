//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	
	public class R2DC_Main {
		
		static R2DC_Main instance;
		
		public static R2DC_Main Instance {
			get {
				if( instance == null ) {
					instance = new R2DC_Main(); 
				}
				return instance;
			}
		}

		private R2DC_Main() { }

		public void EnabledR2D() {
			R2DC_Utils.Instance.Set2DMode();
		}

		public void DisableR2D() {
			R2DD_State.Instance.Save();
			R2DC_Utils.Instance.RepaintSceneView();
		}

		public void SaveState() {
			R2DD_State.Instance.Save();
		}

		public void SceneHeirarchyChanged() {
			R2DC_Settings.Instance.UpdateContextsList();
			R2DC_Utils.Instance.RepaintEditorWindow();
			R2DC_Utils.Instance.RepaintSceneView();
		}

		public void OnSelectionChange() {
			R2DC_Selection.Instance.UpdateSelection();
		}

		public void DrawWindowGUI() {
			R2DV_Toolbar.Instance.DrawGUI();
		}

		public void DrawScene( SceneView sceneView ) {
			if ( R2DC_Utils.Instance.IsSceneViewIn2D() ) {
				if( R2DD_ContextInfo.Instance.LoadContextInfo( sceneView ) ) {
					R2DV_Grid.Instance.DrawGrid();
					R2DV_Guides.Instance.DrawGuides();
					R2DV_Measure.Instance.DrawMeasurements();
					R2DV_Rulers.Instance.DrawRulers(); 
					R2DV_Coords.Instance.DrawCoords();
				}
			}
			else {
				R2DC_Utils.Instance.RepaintEditorWindow();
			}
		}

		public void HandleInteraction() {
			if( Event.current.commandName == "UndoRedoPerformed" ) {
				Event.current.Use();
			}
			R2DC_Guides.Instance.HandleInteraction();
			R2DC_Measure.Instance.HandleInteraction();
			R2DC_Grid.Instance.HandleInteraction();
		}
	}
}

