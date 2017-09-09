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

		public void SpaceX( float space ) {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoSpaceX );
			if( cornerInfos == null ) {
				return;
			}

			LocateXMids( cornerInfos );
			cornerInfos = SortAsc( cornerInfos );

			float runningX = cornerInfos[0].userValue1;

			for( int i = 1; i < cornerInfos.Count; i++ ) {
				runningX += utils.ScaleContextToWorldX( space );
				Transform transform = cornerInfos[i].transform;
				transform.position = new Vector3( 
					runningX,
					transform.position.y,
					transform.position.z );
			}

		}

		public void SpaceY( float space ) {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoSpaceY );
			if( cornerInfos == null ) {
				return;
			}

			LocateYMids( cornerInfos );
			cornerInfos = SortAsc( cornerInfos );

			float runningY = cornerInfos[0].userValue1;

			for( int i = 1; i < cornerInfos.Count; i++ ) {
				runningY += utils.ScaleContextToWorldY( space );
				Transform transform = cornerInfos[i].transform;
				transform.position = new Vector3( 
					transform.position.x,
					runningY,
					transform.position.z );
			}
		}
	}
}
