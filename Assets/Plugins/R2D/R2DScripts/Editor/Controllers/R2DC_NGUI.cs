//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;
	using System.Reflection;
	using System;
	
	public class R2DC_NGUI {
		
		static R2DC_NGUI instance;
		
		public static R2DC_NGUI Instance {
			get {
				if( instance == null ) {
					instance = new R2DC_NGUI(); 
				}
				return instance;
			}
		}

		public enum NGUIState {
			OK,
			NA,
			InvalidContext,
			ReflectionError
		};

		public struct NGUIBounds {
			public NGUIState state;
			public Bounds bounds;
		}

		static MethodInfo calculateBoundsMethodInfo = null;

		public bool HasNGUIRoot( GameObject gameObj) {
			return ( gameObj.GetComponent( "UIRoot" ) != null );
		}

		public bool HasWidget( GameObject gameObj ) {
			return ( gameObj.GetComponent( "UIWidget" ) != null );
		}

		public NGUIState ValidateState( GameObject gameObj ) {
			NGUIState state = NGUIState.NA;

			Component uiWidget = gameObj.GetComponent( "UIWidget" );
			if( uiWidget != null ) {
				// assume we are ok
				state = NGUIState.OK;

				// verify context
				if( R2DD_State.Instance.context.type != ContextType.NGUI ) {
					state = NGUIState.InvalidContext;
				}

				// verify calculate bounds method
				else if( calculateBoundsMethodInfo == null ) {
					calculateBoundsMethodInfo = uiWidget.GetType().GetMethod( "CalculateBounds", new Type[]{} );
					if( calculateBoundsMethodInfo == null ) {
						state = NGUIState.ReflectionError;
					}
				}
			}

			return state;
		}

		public Bounds GetBounds( GameObject gameObj) {
			Component uiWidget = gameObj.GetComponent( "UIWidget" );
			return (Bounds)calculateBoundsMethodInfo.Invoke( uiWidget, null);
		}
	}
}