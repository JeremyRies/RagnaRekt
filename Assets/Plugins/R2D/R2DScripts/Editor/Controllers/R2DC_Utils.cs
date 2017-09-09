//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™ 
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	using System.Reflection;
	using System;
	
	public class R2DC_Utils {
		
		static R2DC_Utils instance;
		
		public static R2DC_Utils Instance {
			get {
				if( instance == null ) {
					instance = new R2DC_Utils(); 
				}
				return instance;
			}
		}

		R2DD_ContextInfo contextInfo;

		private R2DC_Utils() {
			contextInfo = R2DD_ContextInfo.Instance;
		}

		public Vector2 GetWorldToScreenPixelCoord( Vector3 worldCoord ) {
			return contextInfo.cam.WorldToScreenPoint( worldCoord );
		}

		public Vector2 GetWorldCoord( Vector2 pixelCoord ) {
			return contextInfo.cam.ScreenToWorldPoint( pixelCoord );
		}

		public float GetWorldToContextX( float x ) {
			x -= contextInfo.origin.x;
			x /= contextInfo.scale.x;

			return x;
		}
		
		public float GetWorldToContextY( float y ) {
			y -= contextInfo.origin.y;
			y /= contextInfo.scale.y;
			
			return y;
		}

		public float GetContextToWorldX( float x ) {
			x *= contextInfo.scale.x;
			x += contextInfo.origin.x;

			return x;
		}
		
		public float GetContextToWorldY( float y ) {
			y *= contextInfo.scale.y;
			y += contextInfo.origin.y;

			return y;
		}

		public float ScaleContextToWorldX( float x ) {
			return x * contextInfo.scale.x;
		}

		public float ScaleContextToWorldY( float y ) {
			return y * contextInfo.scale.y;
		}

		public void RepaintSceneView() {
			if ( SceneView.lastActiveSceneView != null ) {
				SceneView.lastActiveSceneView.Repaint();
			}
		}

		public void RepaintEditorWindow() {
			if ( R2DE_EditorWindow.Instance != null ) {
				R2DE_EditorWindow.Instance.Repaint();
			}
		}

		public bool IsSceneViewIn2D() {
			if ( SceneView.lastActiveSceneView != null ) {
				return SceneView.lastActiveSceneView.in2DMode;
			}
			return true;
		}

		public void Set2DMode() {
			if ( SceneView.lastActiveSceneView != null ) {
				SceneView.lastActiveSceneView.in2DMode = true;
			}
		}


	}
}
