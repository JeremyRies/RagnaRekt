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

	public class R2DC_Settings {
		
		static R2DC_Settings instance;
		
		public static R2DC_Settings Instance {
			get {
				if( instance == null ) {
					instance = new R2DC_Settings(); 
				}
				return instance;
			}
		}		
		
		public int contextIndex;
		public List<string> contextNames = new List<string>();
		public List<Context> availableContexts = new List<Context>();
		R2DD_State state;
		
		private R2DC_Settings() {
			state = R2DD_State.Instance;

			// Contexts list 
			UpdateContextsList();
		}
		
		public void UpdateContextsList() {
			//// Context names drop down
			availableContexts.Clear();
			availableContexts.Add( new Context( ContextType.EditorScene, null ) );
			contextNames.Clear();
			contextNames.Add( R2DD_Lang.editorScene );

			object[] objs = GameObject.FindObjectsOfType( typeof(GameObject) );
			foreach( object obj in objs ) {
				GameObject gameObj = (GameObject)obj;

				// add any canvas to the list
				if( gameObj.GetComponent<Canvas>() != null ) {
					availableContexts.Add( new Context( ContextType.Canvas, gameObj ) );
					contextNames.Add( gameObj.name );
				}
				// add any UIRoot to the list
				else if( R2DC_NGUI.Instance.HasNGUIRoot( gameObj) ) {
					availableContexts.Add( new Context( ContextType.NGUI, gameObj ) );
					contextNames.Add( gameObj.name );
				}
			}

			int contextIndex = 0;
			for( int i = 0; i < availableContexts.Count; i++ ) {
				if( state.context.instanceId == availableContexts[i].instanceId ) {
					contextIndex = i;
					break;
				}
			}

			SetContext( contextIndex );
		}

		public void SetContext( int pContextIndex ) {
			contextIndex = pContextIndex;
			state.context = availableContexts[contextIndex];
			R2DC_Movement.Instance.error = R2DC_Movement.ADError.None;
		}
	}
}
