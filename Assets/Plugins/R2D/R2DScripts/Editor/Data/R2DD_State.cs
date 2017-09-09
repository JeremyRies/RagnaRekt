//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {

	using UnityEditor;
	using UnityEngine;
	using System;
	using System.Collections.Generic;
	
	public class R2DD_State {

		static R2DD_State instance;

		public static R2DD_State Instance {
			get {
				if( instance == null ) {
					instance = new R2DD_State(); 
				}
				return instance;
			}
		}

		public Context context;
		public bool displayCoords;
		public bool preferColliders;
		public bool snapEdges;
		public bool displayGuides;
		public int toolBar;
		public float spaceX;
		public float spaceY;
		public bool snapGuideToInt;
		public List<string> hGuides = new List<string>();
		public List<string> vGuides = new List<string>();
		public List<string> measurements = new List<string>();
		public float gridX;
		public float gridY;
		public int gridCols;
		public int gridRows;
		public float gridOriginX;
		public float gridOriginY;
		public bool snapToGrid;
		public bool gridEnabled;

		private R2DD_State() {
			LoadContext();
			LoadDisplayCoords();
			LoadPreferColliders();
			LoadDisplayGuides();
			LoadToolBar();
			LoadGuides();
			LoadMeasurements();
			LoadSnapGuideToInt();
			LoadSpacing();
			LoadCurrentSelection();
			LoadGrid();
		}

		public void Save() {
			SaveContext();
			SaveDisplayCoords();
			SavePreferColliders();
			SaveDisplayGuides();
			SaveToolBar();
			SaveGuides();
			SaveMeasurements();
			SaveSnapGuideToInt();
			SaveSpacing();
			SaveCurrentSelection();
			SaveGrid();
		}

		#region Context
		void SaveContext() {
			EditorPrefs.SetInt( prefContextInstanceId, context.instanceId );
		}

		void LoadContext() {
			int instanceId = EditorPrefs.GetInt( prefContextInstanceId, 0 );

			if( instanceId == 0 ) {
				context = new Context( ContextType.EditorScene, null );
			}
			else {
				object[] objs = GameObject.FindObjectsOfType( typeof(GameObject) );
				foreach( object obj in objs ) {
					GameObject gameObj = (GameObject)obj;
					if( gameObj.GetInstanceID() == instanceId ) {
						if( gameObj.GetComponent<Canvas>() != null ) {
							context = new Context( ContextType.Canvas, gameObj );
							break;
						}
						else if( R2DC_NGUI.Instance.HasNGUIRoot( gameObj ) ) {
							context = new Context( ContextType.NGUI, gameObj );
							break;
						}
					}
				}
			}

			// catch all
			if( context == null ) {
				context = new Context( ContextType.EditorScene, null );
			}
		}
		#endregion

		#region Display Coords
		void SaveDisplayCoords() {
			EditorPrefs.SetBool( prefDisplayCoords, displayCoords );
		}
		
		void LoadDisplayCoords() {
			displayCoords = EditorPrefs.GetBool( prefDisplayCoords, true );
		}
		#endregion

		#region Prefer Colliders
		void SavePreferColliders() {
			EditorPrefs.SetBool( prefPreferColliders, preferColliders );
		}
		
		void LoadPreferColliders() {
			preferColliders = EditorPrefs.GetBool( prefPreferColliders, true );
		}
		#endregion

		#region Snap Edges
		void SaveSnapEdges() {
			EditorPrefs.SetBool( prefSnapEdges, snapEdges );
		}
		
		void LoadSnapEdges() {
			snapEdges = EditorPrefs.GetBool( prefSnapEdges, true );
		}
		#endregion

		#region Display Guides
		void SaveDisplayGuides() {
			EditorPrefs.SetBool( prefDisplayGuides, displayGuides );
		}
		
		void LoadDisplayGuides() {
			displayGuides = EditorPrefs.GetBool( prefDisplayGuides, true );
		}
		#endregion


		#region ToolBar
		void SaveToolBar() {
			EditorPrefs.SetInt( prefToolBar, toolBar );
		}
		
		void LoadToolBar() {
			toolBar = EditorPrefs.GetInt( prefToolBar, 0 );
		}
		#endregion

		#region Spacing
		void SaveSpacing() {
			EditorPrefs.SetFloat( prefSpaceX, spaceX );
			EditorPrefs.SetFloat( prefSpaceY, spaceY );
		}

		void LoadSpacing() {
			spaceX = EditorPrefs.GetFloat( prefSpaceX, 1f );
			spaceY = EditorPrefs.GetFloat( prefSpaceY, 1f );
		}
		#endregion

		#region Snap Guide To Int
		void SaveSnapGuideToInt() {
			EditorPrefs.SetBool( prefSnapGuideToInt, snapGuideToInt );
		}
		
		void LoadSnapGuideToInt() {
			snapGuideToInt = EditorPrefs.GetBool( prefSnapGuideToInt, false );
		}
		#endregion

		#region Guides
		void SaveGuides() {
			EditorPrefs.SetString( prefHGuides, string.Join( ",", hGuides.ToArray() ) );
			EditorPrefs.SetString( prefVGuides, string.Join( ",", vGuides.ToArray() ) );
		}
		
		void LoadGuides() {
			string hGuidesStr = EditorPrefs.GetString( prefHGuides, "" );
			string vGuidesStr = EditorPrefs.GetString( prefVGuides, "" );

			if( hGuidesStr.Length > 0 ) {
				hGuides.AddRange( hGuidesStr.Split( ',' ) );
			}

			if( vGuidesStr.Length > 0 ) {
				vGuides.AddRange( (IEnumerable<string>)vGuidesStr.Split( ',' ) );
			}
		}
		#endregion

		#region Measurements
		void SaveMeasurements() {
			EditorPrefs.SetString( prefMeasurements, string.Join( ",", measurements.ToArray() ) );
		}

		void LoadMeasurements() {
			string measurementsStr = EditorPrefs.GetString( prefMeasurements, "" );

			if( measurementsStr.Length > 0 ) {
				measurements.AddRange( measurementsStr.Split( ',' ) );
			}
		}
		#endregion

		#region Current Selection
		void SaveCurrentSelection() {
			List<string> instanceIds = new List<string>();
			List<Transform> selection = R2DC_Selection.Instance.GetSelection();
			for( int i = 0; i < selection.Count; i++ ) {
				instanceIds.Add( selection[i].GetInstanceID().ToString() );
			}
			EditorPrefs.SetString( prefCurrentSelection, string.Join( ",", instanceIds.ToArray() ) );
		}

		void LoadCurrentSelection() {
			string selectionStr = EditorPrefs.GetString( prefCurrentSelection, "" );
			if( selectionStr.Length > 0 ) {
				List<Transform> selection = R2DC_Selection.Instance.GetSelection();
				selection.Clear();

				string[] instanceIds = selectionStr.Split( ',' );
				for( int i = 0; i < instanceIds.Length; i++ ) {
					Transform transform = null;
					try {
						transform = (Transform)EditorUtility.InstanceIDToObject( int.Parse( instanceIds[i] ) );
					}
					catch( Exception e ) {
						e.ToString();
					}
					if( transform != null ) {
						selection.Add( transform );
					}
				}
			}
		}
		#endregion

		#region Grid
		void SaveGrid() {
			EditorPrefs.SetFloat( prefGridX, gridX );
			EditorPrefs.SetFloat( prefGridY, gridY );
			EditorPrefs.SetInt( prefGridCols, gridCols );
			EditorPrefs.SetInt( prefGridRows, gridRows );
			EditorPrefs.SetFloat( prefGridOriginX, gridOriginX );
			EditorPrefs.SetFloat( prefGridOriginY, gridOriginY );
			EditorPrefs.SetBool( prefSnapToGrid, snapToGrid );
			EditorPrefs.SetBool( prefGridEnabled, gridEnabled );
		}

		void LoadGrid() {
			gridX 		= EditorPrefs.GetFloat( prefGridX, 1f );
			gridY 		= EditorPrefs.GetFloat( prefGridY, 1f );
			gridCols 	= EditorPrefs.GetInt( prefGridCols, 2 );
			gridRows 	= EditorPrefs.GetInt( prefGridRows, 2 );
			gridOriginX = EditorPrefs.GetFloat( prefGridOriginX, 0 );
			gridOriginY = EditorPrefs.GetFloat( prefGridOriginY, 0 );
			snapToGrid 	= EditorPrefs.GetBool( prefSnapToGrid, true );
			gridEnabled = EditorPrefs.GetBool( prefGridEnabled, false );
		}
		#endregion

		const string prefContextInstanceId 	= "R2DPREF_ContextInstanceId";
		const string prefToolBar			= "R2DPREF_ToolBar";
		const string prefSpaceX				= "R2DPREF_SpaceX";
		const string prefSpaceY				= "R2DPREF_SpaceY";
		const string prefDisplayCoords		= "R2DPREF_DisplayCoords";
		const string prefPreferColliders	= "R2DPREF_PreferColliders";
		const string prefSnapEdges			= "R2DPREF_SnapEdges";
		const string prefDisplayGuides		= "R2DPREF_DisplayGuides";
		const string prefDisplayGuideCoords	= "R2DPREF_DisplayGuideCoords";
		const string prefHGuides			= "R2DPREF_HGuides";
		const string prefVGuides			= "R2DPREF_VGuides";
		const string prefSnapGuideToInt		= "R2DPREF_SnapGuideToInt";
		const string prefCurrentSelection	= "R2DPREF_CurrentSelection";
		const string prefMeasurements		= "R2DPREF_Measurements";
		const string prefGridX				= "R2DPREF_GridX";
		const string prefGridY				= "R2DPREF_GridY";
		const string prefGridOriginX		= "R2DPREF_GridOriginX";
		const string prefGridOriginY		= "R2DPREF_GridOriginY";
		const string prefGridCols			= "R2DPREF_GridCols";
		const string prefGridRows			= "R2DPREF_GridRows";
		const string prefSnapToGrid			= "R2DPREF_SnapToGrid";
		const string prefGridEnabled		= "R2DPREF_GridEnabled";
	}
}
