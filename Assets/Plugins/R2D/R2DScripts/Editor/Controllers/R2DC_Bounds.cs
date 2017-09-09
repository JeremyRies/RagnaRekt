//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;
	
	public class R2DC_Bounds {
		
		static R2DC_Bounds instance;
		
		public static R2DC_Bounds Instance {
			get {
				if( instance == null ) {
					instance = new R2DC_Bounds(); 
				}
				return instance;
			}
		}

		R2DC_NGUI ngui;
		R2DD_State state;

		private R2DC_Bounds() {
			ngui = R2DC_NGUI.Instance;	
			state = R2DD_State.Instance;
		}

		public Vector3[] GetWorldCorners( Transform transform, bool preferColliders ) {
			Bounds bounds = new Bounds();
			bool worldSpaceBounds = false;
			Collider collider = null;
			Collider2D collider2D = null;

			if( preferColliders ) {
				collider = transform.GetComponent<Collider>();
				collider2D = transform.GetComponent<Collider2D>();
			}

			if( preferColliders && collider != null ) {
				bounds = collider.bounds;
				worldSpaceBounds = true;
			}
			else if( preferColliders && collider2D != null ) {
				bounds = collider2D.bounds;
				worldSpaceBounds = true;
			}
			else if( state.context.type == ContextType.Canvas && transform.GetComponent<RectTransform>() != null ) {
				Vector3[] corners = new Vector3[4];
				transform.GetComponent<RectTransform>().GetWorldCorners( corners );
				return corners;
			}
			else if( state.context.type == ContextType.NGUI && ngui.HasWidget( transform.gameObject ) ) {
				bounds = ngui.GetBounds( transform.gameObject );
			}
			else {
				Renderer renderer = transform.GetComponent<Renderer>();
				if( renderer != null ) {
					bounds = renderer.bounds;
					worldSpaceBounds = true;
				}
				else {
					Vector3[] corners = new Vector3[4];
					for( int i = 0; i < 4; i++ ) {
						corners[i] = transform.position;
					}
					return corners;
				}
			}

			return GetCorners( transform, bounds, worldSpaceBounds );
		}
		
		Vector3[] GetCorners( Transform transform, Bounds bounds, bool worldSpaceBounds ) {
			Vector3[] corners = new Vector3[4];
			Transform cornerTracker = new GameObject().transform;
			cornerTracker.parent = transform;
			cornerTracker.localScale = Vector3.one;
			
			// top left
			Vector3 topLeft = new Vector3( ( bounds.center.x - bounds.extents.x ), ( bounds.center.y + bounds.extents.y ), 0 );
			if( worldSpaceBounds ) {
				cornerTracker.position = topLeft;
			}
			else {
				cornerTracker.localPosition = topLeft;
			}
			corners[0] = cornerTracker.position;
			
			// top right
			Vector3 topRight = new Vector3( ( bounds.center.x + bounds.extents.x ), ( bounds.center.y + bounds.extents.y ), 0 );
			if( worldSpaceBounds ) {
				cornerTracker.position = topRight;
			}
			else {
				cornerTracker.localPosition = topRight;
			}
			corners[1] = cornerTracker.position;
			
			// bot right
			Vector3 botRight = new Vector3( ( bounds.center.x + bounds.extents.x ), ( bounds.center.y - bounds.extents.y ), 0 );
			if( worldSpaceBounds ) {
				cornerTracker.position = botRight;
			}
			else {
				cornerTracker.localPosition = botRight;
			}
			corners[2] = cornerTracker.position;
			
			// bot left
			Vector3 botLeft = new Vector3( ( bounds.center.x - bounds.extents.x ), ( bounds.center.y - bounds.extents.y ), 0 );
			if( worldSpaceBounds ) {
				cornerTracker.position = botLeft;
			}
			else {
				cornerTracker.localPosition = botLeft;
			}
			corners[3] = cornerTracker.position;
			
			GameObject.DestroyImmediate( cornerTracker.gameObject );

			return corners;
		}
	}
}