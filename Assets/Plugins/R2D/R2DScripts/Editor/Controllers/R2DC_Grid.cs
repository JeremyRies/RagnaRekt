//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {

	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;
	using System;
	using System.Reflection;

	public class R2DC_Grid {

		static R2DC_Grid instance;

		public static R2DC_Grid Instance {
			get {
				if( instance == null ) {
					instance = new R2DC_Grid(); 
				}
				return instance;
			}
		}

		GridData gridData;

		R2DD_State state;
		R2DC_Utils utils;
		R2DC_Selection selection;

		private R2DC_Grid() {
			state 				= R2DD_State.Instance;
			utils				= R2DC_Utils.Instance;
			selection			= R2DC_Selection.Instance;
		}

		public GridData GetGridData() {
			if( gridData == null ) {
				gridData = new GridData();
			}
			UpdateGridData();
			return gridData; 
		}

		public void UpdateGridData() {
			gridData.worldOrigin = new Vector2( 
				utils.GetContextToWorldX( state.gridOriginX ), 
				utils.GetContextToWorldY( state.gridOriginY )
			);

			gridData.worldZero = new Vector2( 
				utils.GetContextToWorldX( 0f ),
				utils.GetContextToWorldY( 0f ) );
			
			gridData.worldUnit = new Vector2( 
				utils.GetContextToWorldX( state.gridX ),
				utils.GetContextToWorldY( state.gridY ) );

			gridData.worldUnitSize = gridData.worldUnit - gridData.worldZero;

			gridData.worldWidth = state.gridCols * gridData.worldUnitSize.x;
			gridData.worldHeight = state.gridRows * gridData.worldUnitSize.y;

			gridData.worldCorner = new Vector2(
				gridData.worldOrigin.x + gridData.worldWidth,
				gridData.worldOrigin.y + gridData.worldHeight
			);
		}

		public void HandleInteraction() {
			if( !state.gridEnabled || !state.snapToGrid || gridData == null ) {
				return;
			}

			EventType eventType = Event.current.type;
			if( Event.current.button != 0 ) {
				eventType = EventType.Ignore;
			}

			switch ( eventType ) {
			case EventType.MouseUp:
				List<Transform> selectedObjs = selection.GetSelection();
				if( selectedObjs.Count == 1 ) {
					Transform transform = selectedObjs[0];
					if( IsInGridBounds( transform.position ) ) {
						transform.position = SnapToGrid( transform.position );
					}
				}
				break;

			default:
				break;
			}
		}

		bool IsInGridBounds( Vector2 pos ) {
			if( pos.x > gridData.worldOrigin.x &&
				pos.x < gridData.worldOrigin.x + gridData.worldWidth &&
				pos.y > gridData.worldOrigin.y && 
				pos.y < gridData.worldOrigin.y + gridData.worldHeight ) {
				return true;
			}
			return false;
		}

		public Vector2 SnapToGrid( Vector2 pos ) {
			Vector2 gridCoord = pos - gridData.worldOrigin;
			gridCoord.x = Mathf.CeilToInt( gridCoord.x / gridData.worldUnitSize.x );
			gridCoord.y = Mathf.CeilToInt( gridCoord.y / gridData.worldUnitSize.y );

			Vector2 snapCoord = new Vector2(
				( gridCoord.x * gridData.worldUnitSize.x ) - gridData.worldUnitSize.x / 2f,
				( gridCoord.y * gridData.worldUnitSize.y ) - gridData.worldUnitSize.y / 2f
			) + gridData.worldOrigin;

			return snapCoord;
		}

		public void ToggleUnityGrid() {
			var annotationUtility = Type.GetType( "UnityEditor.AnnotationUtility, UnityEditor" );
			var showGrid = annotationUtility.GetProperty( "showGrid", BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static );
			bool gridOn = !( (bool)showGrid.GetValue( null, null) );
			showGrid.SetValue( null, gridOn, null );
		}
	}
}
