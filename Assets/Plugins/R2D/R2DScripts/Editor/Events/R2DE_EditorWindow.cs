//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™ 
//----------------------------------------------

namespace R2D {

	using UnityEngine;
	using UnityEditor;
	using System;

	class R2DE_EditorWindow : EditorWindow { 

		public static R2DE_EditorWindow Instance;

		[MenuItem ( R2DD_Lang.menuAccess )]
		static void OpenWindow() {
			if( Instance == null ) {
				R2DE_EditorWindow r2dWindow = ( R2DE_EditorWindow )EditorWindow.GetWindow( typeof( R2DE_EditorWindow ) );
				r2dWindow.minSize = new Vector2( windowWidth, windowHeight );
				r2dWindow.maxSize = r2dWindow.minSize;
			}
			else {
				Instance.Close();
			}
		}

		[MenuItem ( R2DD_Lang.toggleGuides )]
		static void ToggleGuides() {
			R2DD_State.Instance.displayGuides = !R2DD_State.Instance.displayGuides;
			R2DC_Utils.Instance.RepaintEditorWindow();
			R2DC_Utils.Instance.RepaintSceneView();
		}

		[MenuItem ( R2DD_Lang.toggleGrid )]
		static void ToggleGrid() {
			R2DD_State.Instance.gridEnabled = !R2DD_State.Instance.gridEnabled;
			R2DC_Utils.Instance.RepaintEditorWindow();
			R2DC_Utils.Instance.RepaintSceneView();
		}

		void OnEnable() {
			Instance = this;
			this.titleContent = new GUIContent( R2DD_Lang.windowTitle, R2DD_Resources.Instance.windowIcon );

			R2DC_Main.Instance.EnabledR2D();

			SceneView.onSceneGUIDelegate += OnSceneGUI;
			EditorApplication.hierarchyWindowChanged += OnHierarchyChanged;
			AppDomain.CurrentDomain.DomainUnload += OnDomainUnload;
		}

		void OnDisable() {
			SceneView.onSceneGUIDelegate -= OnSceneGUI;
			EditorApplication.hierarchyWindowChanged -= OnHierarchyChanged;

			R2DC_Main.Instance.DisableR2D();
		}

		void OnDomainUnload(object sender, EventArgs e) {
			R2DC_Main.Instance.SaveState();
		}

		void OnHierarchyChanged() {
			R2DC_Main.Instance.SceneHeirarchyChanged();
		}

		public void OnSelectionChange() {
			R2DC_Main.Instance.OnSelectionChange();
		}

		public void OnFocus() {
			R2DC_Main.Instance.OnSelectionChange(); 
			Repaint(); 
		}

		void OnGUI() {
			R2DC_Main.Instance.DrawWindowGUI();
		}
	
		void OnSceneGUI( SceneView sceneView ) {
			Handles.BeginGUI();
			if( Event.current.type != EventType.used ) {
				R2DC_Main.Instance.DrawScene( sceneView );
				if( Event.current.type != EventType.Layout ) {
					R2DC_Main.Instance.HandleInteraction();
				}
			}
			Handles.EndGUI();
		}

		const float windowWidth 	= 220f;
		const float windowHeight 	= 480f;
	}
}
