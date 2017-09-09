//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;
	using System;
	
	public partial class R2DC_Movement {

		public void SnapLeft() {	
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoSnapLeft );
			if( cornerInfos == null ) {
				return;
			}

			// fill left edges
			LocateLeftX( cornerInfos );

			// fill right edges if we're going for the midpoint
			if( !state.snapEdges ) {
				// save these into uservalue2 since the next call overwrites them
				foreach( CornerInfo cornerInfo in cornerInfos ) {
					cornerInfo.userValue2 = cornerInfo.userValue1;
				}
				LocateRightX( cornerInfos );
			}

			// lets move these objects
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				float objTarget = state.snapEdges ? cornerInfo.userValue1 : ( cornerInfo.userValue1 + cornerInfo.userValue2 ) / 2f;

				float guideDelta = 0f;
				foreach( string xStr in state.vGuides ) {
					float potentialDelta = objTarget - float.Parse( xStr );
					if( potentialDelta > 0.0001f ) {
						if( guideDelta == 0  || potentialDelta < guideDelta ) {
							guideDelta = potentialDelta;
						}
					}
				}
				if( guideDelta > 0 ) {
					Transform transform = cornerInfo.transform;
					transform.position = new Vector3( transform.position.x - guideDelta,
					                                 transform.position.y,
					                                 transform.position.z );
				}
			}
		}

		public void SnapRight() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoSnapRight );
			if( cornerInfos == null ) {
				return;
			}
			
			// fill right edges
			LocateRightX( cornerInfos );
			
			// fill left edges if we're going for the midpoint
			if( !state.snapEdges ) {
				// save these into uservalue2 since the next call overwrites them
				foreach( CornerInfo cornerInfo in cornerInfos ) {
					cornerInfo.userValue2 = cornerInfo.userValue1;
				}
				LocateLeftX( cornerInfos );
			}
			
			// lets move these objects
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				float objTarget = state.snapEdges ? cornerInfo.userValue1 : ( cornerInfo.userValue1 + cornerInfo.userValue2 ) / 2f;
				
				float guideDelta = 0f;
				foreach( string xStr in state.vGuides ) {
					float potentialDelta = float.Parse( xStr ) - objTarget;
					if( potentialDelta > 0.0001f ) {
						if( guideDelta == 0  || potentialDelta < guideDelta ) {
							guideDelta = potentialDelta;
						}
					}
				}
				if( guideDelta > 0 ) {
					Transform transform = cornerInfo.transform;
					transform.position = new Vector3( transform.position.x + guideDelta,
					                                 transform.position.y,
					                                 transform.position.z );
				}
			}
		}

		public void SnapTop() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoSnapUp );
			if( cornerInfos == null ) {
				return;
			}
			
			// fill top edges
			LocateTopY( cornerInfos );
			
			// fill bot edges if we're going for the midpoint
			if( !state.snapEdges ) {
				// save these into uservalue2 since the next call overwrites them
				foreach( CornerInfo cornerInfo in cornerInfos ) {
					cornerInfo.userValue2 = cornerInfo.userValue1;
				}
				LocateBotY( cornerInfos );
			}

			// lets move these objects
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				float objTarget = state.snapEdges ? cornerInfo.userValue1 : ( cornerInfo.userValue1 + cornerInfo.userValue2 ) / 2f;
				
				float guideDelta = 0f;
				foreach( string yStr in state.hGuides ) {
					float potentialDelta = float.Parse( yStr ) - objTarget;
					if( potentialDelta > 0.0001f ) {
						if( guideDelta == 0  || potentialDelta < guideDelta ) {
							guideDelta = potentialDelta;
						}
					}
				}
				if( guideDelta > 0 ) {
					Transform transform = cornerInfo.transform;
					transform.position = new Vector3( transform.position.x,
					                                 transform.position.y + guideDelta,
					                                 transform.position.z );
				}
			}
		}

		public void SnapBot() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoSnapDown );
			if( cornerInfos == null ) {
				return;
			}
			
			// fill bot edges
			LocateBotY( cornerInfos );
			
			// fill top edges if we're going for the midpoint
			if( !state.snapEdges ) {
				// save these into uservalue2 since the next call overwrites them
				foreach( CornerInfo cornerInfo in cornerInfos ) {
					cornerInfo.userValue2 = cornerInfo.userValue1;
				}
				LocateTopY( cornerInfos );
			}
			
			// lets move these objects
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				float objTarget = state.snapEdges ? cornerInfo.userValue1 : ( cornerInfo.userValue1 + cornerInfo.userValue2 ) / 2f;
				
				float guideDelta = 0f;
				foreach( string yStr in state.hGuides ) {
					float potentialDelta = objTarget - float.Parse( yStr );
					if( potentialDelta > 0.0001f ) {
						if( guideDelta == 0  || potentialDelta < guideDelta ) {
							guideDelta = potentialDelta;
						}
					}
				}
				if( guideDelta > 0 ) {
					Transform transform = cornerInfo.transform;
					transform.position = new Vector3( transform.position.x,
					                                 transform.position.y - guideDelta,
					                                 transform.position.z );
				}
			}
		}

	}
}
