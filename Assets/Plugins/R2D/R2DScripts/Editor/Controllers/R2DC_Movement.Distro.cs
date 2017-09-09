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
		
		public void DistroTop() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoDistroTop );
			if( cornerInfos == null ) {
				return;
			}
			
			LocateTopEdges( cornerInfos );

			float highestTopEdge = cornerInfos[0].userValue1;
			float lowestTopEdge	= highestTopEdge; 
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				if( cornerInfo.userValue1 > highestTopEdge ) {
					highestTopEdge = cornerInfo.userValue1;
				}
				else if( cornerInfo.userValue1 < lowestTopEdge ) {
					lowestTopEdge = cornerInfo.userValue1;
				}
			}

			float spacing = ( highestTopEdge - lowestTopEdge ) / (float)( cornerInfos.Count - 1 );
			cornerInfos = SortDesc( cornerInfos );
			float runningEdge = cornerInfos[0].userValue1;

			for( int i = 1; i < ( cornerInfos.Count - 1 ); i++ ) {
				runningEdge -= spacing;
				float delta = runningEdge - cornerInfos[i].userValue1;
				Transform transform = cornerInfos[i].transform;
				transform.position = new Vector3( transform.position.x,
				                                 transform.position.y + delta,
				                                 transform.position.z );
			}
		}

		public void DistroYMid() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoDistroVertical );
			if( cornerInfos == null ) {
				return;
			}
			
			LocateYMids( cornerInfos );

			float highestYMid = cornerInfos[0].userValue1;
			float lowestYMid = highestYMid; 
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				if( cornerInfo.userValue1 > highestYMid ) {
					highestYMid = cornerInfo.userValue1;
				}
				else if( cornerInfo.userValue1 < lowestYMid ) {
					lowestYMid = cornerInfo.userValue1;
				}
			}
			
			float spacing = ( highestYMid - lowestYMid ) / (float)( cornerInfos.Count - 1 );
			cornerInfos = SortDesc( cornerInfos );
			float runningEdge = cornerInfos[0].userValue1;
			
			for( int i = 1; i < ( cornerInfos.Count - 1 ); i++ ) {
				runningEdge -= spacing;
				float delta = runningEdge - cornerInfos[i].userValue1;
				Transform transform = cornerInfos[i].transform;
				transform.position = new Vector3( transform.position.x,
				                                 transform.position.y + delta,
				                                 transform.position.z );
			}

		}

		public void DistroBot() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoDistroBottom );
			if( cornerInfos == null ) {
				return;
			}
			
			LocateBotEdges( cornerInfos );
			
			float highestBotEdge = cornerInfos[0].userValue1;
			float lowestBotEdge	= highestBotEdge; 
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				if( cornerInfo.userValue1 > highestBotEdge ) {
					highestBotEdge = cornerInfo.userValue1;
				}
				else if( cornerInfo.userValue1 < lowestBotEdge ) {
					lowestBotEdge = cornerInfo.userValue1;
				}
			}
			
			float spacing = ( highestBotEdge - lowestBotEdge ) / (float)( cornerInfos.Count - 1 );
			cornerInfos = SortDesc( cornerInfos );
			float runningEdge = cornerInfos[0].userValue1;
			
			for( int i = 1; i < ( cornerInfos.Count - 1 ); i++ ) {
				runningEdge -= spacing;
				float delta = runningEdge - cornerInfos[i].userValue1;
				Transform transform = cornerInfos[i].transform;
				transform.position = new Vector3( transform.position.x,
				                                 transform.position.y + delta,
				                                 transform.position.z );
			}
		}

		public void DistroLeft() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoDistroLeft );
			if( cornerInfos == null ) {
				return;
			}
			
			LocateLeftEdges( cornerInfos );
			
			float leftMostLeftEdge = cornerInfos[0].userValue1;
			float rightMostLeftEdge	= leftMostLeftEdge; 
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				if( cornerInfo.userValue1 < leftMostLeftEdge ) {
					leftMostLeftEdge = cornerInfo.userValue1;
				}
				else if( cornerInfo.userValue1 > rightMostLeftEdge ) {
					rightMostLeftEdge = cornerInfo.userValue1;
				}
			}
			
			float spacing = ( rightMostLeftEdge - leftMostLeftEdge ) / (float)( cornerInfos.Count - 1 );
			cornerInfos = SortAsc( cornerInfos );
			float runningEdge = cornerInfos[0].userValue1;
			
			for( int i = 1; i < ( cornerInfos.Count - 1 ); i++ ) {
				runningEdge += spacing;
				float delta = runningEdge - cornerInfos[i].userValue1;
				Transform transform = cornerInfos[i].transform;
				transform.position = new Vector3( transform.position.x + delta,
				                                 transform.position.y,
				                                 transform.position.z );
			}
		}
		
		public void DistroXMid() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoDistroHorizontal );
			if( cornerInfos == null ) {
				return;
			}
			
			LocateXMids( cornerInfos );
			
			float leftMostXMid = cornerInfos[0].userValue1;
			float rightMostXMid	= leftMostXMid; 
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				if( cornerInfo.userValue1 < leftMostXMid ) {
					leftMostXMid = cornerInfo.userValue1;
				}
				else if( cornerInfo.userValue1 > rightMostXMid ) {
					rightMostXMid = cornerInfo.userValue1;
				}
			}
			
			float spacing = ( rightMostXMid - leftMostXMid ) / (float)( cornerInfos.Count - 1 );
			cornerInfos = SortAsc( cornerInfos );
			float runningEdge = cornerInfos[0].userValue1;
			
			for( int i = 1; i < ( cornerInfos.Count - 1 ); i++ ) {
				runningEdge += spacing;
				float delta = runningEdge - cornerInfos[i].userValue1;
				Transform transform = cornerInfos[i].transform;
				transform.position = new Vector3( transform.position.x + delta,
				                                 transform.position.y,
				                                 transform.position.z );
			}
			
		}
		
		public void DistroRight() {
			List<CornerInfo> cornerInfos = PrepareCornerInfos( R2DD_Lang.undoDistroRight );
			if( cornerInfos == null ) {
				return;
			}
			
			LocateRightEdges( cornerInfos );
			
			float leftMostRightEdge = cornerInfos[0].userValue1;
			float rightMostRightEdge = leftMostRightEdge; 
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				if( cornerInfo.userValue1 < leftMostRightEdge ) {
					leftMostRightEdge = cornerInfo.userValue1;
				}
				else if( cornerInfo.userValue1 > rightMostRightEdge ) {
					rightMostRightEdge = cornerInfo.userValue1;
				}
			}
			
			float spacing = ( rightMostRightEdge - leftMostRightEdge ) / (float)( cornerInfos.Count - 1 );
			cornerInfos = SortAsc( cornerInfos );
			float runningEdge = cornerInfos[0].userValue1;

			for( int i = 1; i < ( cornerInfos.Count - 1 ); i++ ) {
				runningEdge += spacing;
				float delta = runningEdge - cornerInfos[i].userValue1;
				Transform transform = cornerInfos[i].transform;
				transform.position = new Vector3( transform.position.x + delta,
				                                 transform.position.y,
				                                 transform.position.z );
			}
		}
	}
}
