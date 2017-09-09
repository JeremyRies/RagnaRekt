//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;

	public class R2DD_ContextInfo {
		static R2DD_ContextInfo instance;
		
		public static R2DD_ContextInfo Instance {
			get {
				if( instance == null ) {
					instance = new R2DD_ContextInfo();
				}
				return instance;
			}
		}

		public R2DFrame	viewPixelFrame;
		public R2DFrame	viewUnitFrame;
		public Vector2	gridPixelSquareSize;
		public float	editorCameraSize;
		public Vector3	origin;
		public Vector3 	scale;
		public SceneView sceneView;
		public Camera cam;

		private R2DD_ContextInfo( ) { 
			viewPixelFrame = new R2DFrame();
			viewUnitFrame = new R2DFrame();
		}

		public bool LoadContextInfo( SceneView sceneView ) {
			R2DD_State state = R2DD_State.Instance;
			this.sceneView = sceneView;
			cam = sceneView.camera;
			editorCameraSize = cam.orthographicSize;
			gridPixelSquareSize = cam.WorldToScreenPoint( new Vector3(1, 1) ) - cam.WorldToScreenPoint( new Vector3(0, 0) );
			scale = Vector3.one;
			origin = Vector3.zero;
			
			Context context = state.context;
			
			if( context.type == ContextType.Canvas ) {
				if( context.gameObject == null ) {
					return false;
				}

				scale = context.gameObject.transform.lossyScale;
				origin = context.gameObject.GetComponent<RectTransform>().position;
				
				if( scale.x <= 0.001f || scale.y <= 0.001f ) {
					return false;
				}
			}
			else if( context.type == ContextType.NGUI ) {
				if( context.gameObject == null ) {
					return false;
				}

				scale = context.gameObject.transform.lossyScale;
				origin = context.gameObject.GetComponent<Transform>().position;

			}
			
			LoadPixelFrame();
			LoadWorldUnits();

			return true;
		}

		void LoadPixelFrame() {
			viewPixelFrame.topLeft = cam.pixelRect.min;
			viewPixelFrame.botLeft = new Vector2( viewPixelFrame.topLeft.x, viewPixelFrame.topLeft.y + cam.pixelRect.height );
			viewPixelFrame.botLeft.y--;
			
			viewPixelFrame.botRight	= cam.pixelRect.max;
			viewPixelFrame.botRight.x--;
			viewPixelFrame.botRight.y--;
			viewPixelFrame.topRight	= new Vector2( viewPixelFrame.botRight.x, viewPixelFrame.topLeft.y );
			
			viewPixelFrame.width = viewPixelFrame.topRight.x - viewPixelFrame.topLeft.x;
			viewPixelFrame.height = viewPixelFrame.botRight.y - viewPixelFrame.topRight.y;
		}

		void LoadWorldUnits() {
			viewUnitFrame.botLeft	= cam.ScreenToWorldPoint( viewPixelFrame.topLeft );
			viewUnitFrame.topLeft	= cam.ScreenToWorldPoint( viewPixelFrame.botLeft );
			viewUnitFrame.topRight	= cam.ScreenToWorldPoint( viewPixelFrame.topRight );
			viewUnitFrame.botRight	= cam.ScreenToWorldPoint( viewPixelFrame.botRight );
		}

	}
}
