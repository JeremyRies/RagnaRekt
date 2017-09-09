//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {

	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;

	public class R2DC_Measure {

		static R2DC_Measure instance;

		public static R2DC_Measure Instance {
			get {
				if( instance == null ) {
					instance = new R2DC_Measure(); 
				}
				return instance;
			}
		}

		public Vector2 startCoord;
		public Vector2 currentCoord;

		R2DFrame viewPixelFrame;
		bool measureToolActive = false;
		bool measuring = false;
		bool readyToMeasure = false;
		R2DC_Utils utils;
		R2DC_Guides guides;
		R2DD_State state;
		R2DC_Selection selection;

		private R2DC_Measure() {
			viewPixelFrame = R2DD_ContextInfo.Instance.viewPixelFrame;
			utils = R2DC_Utils.Instance;
			guides = R2DC_Guides.Instance;
			state = R2DD_State.Instance;
			selection = R2DC_Selection.Instance;
		}

		public void HandleInteraction() {
			if( !measureToolActive ) {
				return;
			}
			EventType eventType = Event.current.type;
			if( Event.current.button != 0 ) {
				eventType = EventType.Ignore;
			}

			Vector2 mousePosition = Event.current.mousePosition;
			mousePosition.y = viewPixelFrame.height - mousePosition.y + 5f; 

			switch ( eventType ) {

			case EventType.MouseDown:
				if ( IsInRenderBounds( mousePosition ) && !guides.IsCreatingGuides() ) {
					ClearMeasurement();
					HotControl();
					readyToMeasure = true;
					startCoord = utils.GetWorldCoord( mousePosition );
				}
				break;
				
			case EventType.MouseDrag:
				if( readyToMeasure ) {
					if( IsInRenderBounds( mousePosition ) ) {
						measuring = true;
						currentCoord = utils.GetWorldCoord( mousePosition );	
						utils.RepaintSceneView();
					}
					else {
						CancelMeasurement();
					}
				}
				break;
			case EventType.MouseUp:
				if( measuring ) {
					if( IsInRenderBounds( mousePosition ) ) {
						CompleteMeasurement();
					}
					else {
						CancelMeasurement();
					}
				} 
				break;
			default:
				if( measuring && !IsInRenderBounds( mousePosition ) ) {
					CancelMeasurement();
				} 
				break;
			}
		}

		void CompleteMeasurement() {
			state.measurements.Add( startCoord.x.ToString() );
			state.measurements.Add( startCoord.y.ToString() );
			state.measurements.Add( currentCoord.x.ToString() );
			state.measurements.Add( currentCoord.y.ToString() );

			Reset();
		}

		void CancelMeasurement() {
			Reset();
			ClearMeasurement();
		}

		void ClearMeasurement() {
			state.measurements.Clear();
		}

		void Reset() {
			GUIUtility.hotControl = 0;
			measuring = false;
			readyToMeasure = false;
			measureToolActive = false;
			utils.RepaintSceneView();
			utils.RepaintEditorWindow();
		}

		void HotControl() {
			GUIUtility.hotControl = GUIUtility.GetControlID( FocusType.Passive );
			Event.current.Use();
		}

		bool IsInRenderBounds( Vector2 pos ) {
			return ( pos.x > 20f && pos.y > 0f && pos.x < viewPixelFrame.width && pos.y < viewPixelFrame.height - 20f );
		}

		public void ToggleMeasureTool() {
			measureToolActive = measureToolActive ? false : true;
			R2DC_Utils.Instance.RepaintSceneView();
		}

		public void ClearMeasureTool() {
			ClearMeasurement();
			R2DC_Utils.Instance.RepaintSceneView();
		}

		public bool IsMeasureToolActive() {
			return measureToolActive;
		}

		public bool IsMeasuring() {
			return measuring;
		}

		public bool IsMeasureObjEnabled() {
			return selection.GetSelection().Count == 2;
		}

		public void MeasureObj() {
			ClearMeasurement();
			List<Transform> selectedObjs = selection.GetSelection();

			startCoord = new Vector2( selectedObjs[0].position.x, selectedObjs[0].position.y );
			currentCoord = new Vector2( selectedObjs[1].position.x, selectedObjs[1].position.y );

			CompleteMeasurement();
		}

		public Measurement GetMeasurement( Vector2 startWorldCoord, Vector2 endWorldCood ) {
			Vector2 start = new Vector2(
				utils.GetWorldToContextX( startWorldCoord.x ),
				utils.GetWorldToContextY( startWorldCoord.y )
			);

			Vector2 end = new Vector2(
				utils.GetWorldToContextX( endWorldCood.x ),
				utils.GetWorldToContextY( endWorldCood.y )
			);

			Measurement measurement = new Measurement();
			measurement.x = start.x;
			measurement.y = start.y;
			float yDiff = end.y - start.y;
			float xDiff = end.x - start.x;
			measurement.h = Mathf.Abs( yDiff );
			measurement.w = Mathf.Abs( xDiff );
			measurement.angle = Mathf.Atan2( yDiff, xDiff ) * 180.0f / Mathf.PI;
			measurement.length = Mathf.Sqrt( Mathf.Pow( yDiff, 2 ) + Mathf.Pow( xDiff, 2 ) );

			return measurement;
		}
	}
}

