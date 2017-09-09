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
		
		public void AlignTop() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoAlignTop );
			if( cornerInfos == null ) {
				return;
			}

			float topY = LocateTopY( cornerInfos );

			foreach( CornerInfo cornerInfo in cornerInfos ) {
				float delta = topY - cornerInfo.userValue1;
				Transform transform = cornerInfo.transform;
				transform.position = new Vector3( transform.position.x,
				                                  transform.position.y + delta,
				                                  transform.position.z );
			}
		}

		public void AlignBot() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoAlignBottom );
			if( cornerInfos == null ) {
				return;
			}
			
			float botY = LocateBotY( cornerInfos );

			foreach( CornerInfo cornerInfo in cornerInfos ) {
				float delta = botY - cornerInfo.userValue1;
				Transform transform = cornerInfo.transform;
				transform.position = new Vector3( transform.position.x,
				                                 transform.position.y + delta,
				                                 transform.position.z );
			}
		}
		
		public void AlignLeft() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos(  R2DD_Lang.undoAlignLeft );
			if( cornerInfos == null ) {
				return;
			}
			
			float leftX = LocateLeftX( cornerInfos );

			foreach( CornerInfo cornerInfo in cornerInfos ) {
				float delta = leftX - cornerInfo.userValue1;
				Transform transform = cornerInfo.transform;
				transform.position = new Vector3( transform.position.x + delta,
				                                 transform.position.y,
				                                 transform.position.z );
			}
		}

		public void AlignRight() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos(  R2DD_Lang.undoAlignRight );
			if( cornerInfos == null ) {
				return;
			}

			float rightX = LocateRightX( cornerInfos );
			
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				float delta = rightX - cornerInfo.userValue1;
				Transform transform = cornerInfo.transform;
				transform.position = new Vector3( transform.position.x + delta,
				                                 transform.position.y,
				                                 transform.position.z );
			}
		}

		public void AlignYMid() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoAlignVertical );
			if( cornerInfos == null ) {
				return;
			}

			float topY = LocateTopY( cornerInfos );

			// save these into uservalue2 since the next call overwrites them
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				cornerInfo.userValue2 = cornerInfo.userValue1;
			}

			float botY = LocateBotY( cornerInfos );
			float midpoint = ( topY + botY ) / 2f;

			foreach( CornerInfo cornerInfo in cornerInfos ) {
				float objMidpoint = ( cornerInfo.userValue1 + cornerInfo.userValue2 ) / 2f;
				float delta = midpoint - objMidpoint;
				Transform transform = cornerInfo.transform;
				transform.position = new Vector3( transform.position.x,
				                                 transform.position.y + delta,
				                                 transform.position.z );
			}
		}

		public void AlignXMid() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoAlignHorizontal );
			if( cornerInfos == null ) {
				return;
			}
			
			float leftX = LocateLeftX( cornerInfos );

			// save these into uservalue2 since the next call overwrites them
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				cornerInfo.userValue2 = cornerInfo.userValue1;
			}

			float rightX = LocateRightX( cornerInfos );
			float midpoint = ( leftX + rightX ) / 2f;
			
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				float objMidpoint = ( cornerInfo.userValue1 + cornerInfo.userValue2 ) / 2f;
				float delta = midpoint - objMidpoint;
				Transform transform = cornerInfo.transform;
				transform.position = new Vector3( transform.position.x + delta,
				                                 transform.position.y,
				                                 transform.position.z );
			}
		}
	}
}
