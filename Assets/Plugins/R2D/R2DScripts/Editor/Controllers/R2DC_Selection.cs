//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;
	
	public class R2DC_Selection {
		
		static R2DC_Selection instance;
		
		public static R2DC_Selection Instance {
			get {
				if( instance == null ) {
					instance = new R2DC_Selection(); 
				}
				return instance;
			}
		}
		
		private R2DC_Selection() { }

		List<Transform> orderedSelection = new List<Transform>();

		public void UpdateSelection() {
			List<Transform> freshTransforms = new List<Transform>( Selection.GetTransforms( SelectionMode.Unfiltered ) );

			// add new selections
			for( int i = 0; i < freshTransforms.Count; i++ ) {
				if( !orderedSelection.Contains( freshTransforms[i] ) ) {
					orderedSelection.Add( freshTransforms[i] );
				}
			}

			// remove unselected
			List<Transform> removeMe = new List<Transform>();
			for( int i = 0; i < orderedSelection.Count; i++ ) {
				if( !freshTransforms.Contains( orderedSelection[i] ) ) {
					removeMe.Add( orderedSelection[i] );
				}
			}

			for( int i = 0; i < removeMe.Count; i++ ) {
				orderedSelection.Remove( removeMe[i] );
			}

			// nofity align tools
			R2DC_Movement.Instance.SelectionUpdated();
		}

		public List<Transform> GetSelection() {
			return orderedSelection;
		}
	}
}
