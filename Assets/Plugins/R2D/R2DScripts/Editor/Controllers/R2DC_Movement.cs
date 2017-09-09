//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;
	using System;
	using System.Linq;
	
	public partial class R2DC_Movement {
		
		static R2DC_Movement instance;
		
		public static R2DC_Movement Instance {
			get {
				if( instance == null ) {
					instance = new R2DC_Movement(); 
				}
				return instance;
			}
		}

		public enum ADError {
			None,
			NGUIReflection,
			NGUIContext,
			CanvasContext
		}

		class CornerInfo {
			public Transform transform;
			public Vector3[] corners;
			public float userValue1;
			public float userValue2;
		}

		public bool alignEnabled = false;
		public bool distroEnabled = false;
		public bool guideSnapEnabled = false;
		public bool spaceEnabled = false;
		public ADError error = ADError.None;

		R2DC_Selection selection;
		R2DC_Bounds bounds;
		R2DC_NGUI ngui;
		R2DD_State state;
		R2DC_Utils utils;
	
		private R2DC_Movement() { 
			selection = R2DC_Selection.Instance;
			bounds = R2DC_Bounds.Instance;
			ngui = R2DC_NGUI.Instance;
			state = R2DD_State.Instance;
			utils = R2DC_Utils.Instance;
		}

		public void SelectionUpdated() {
			
			guideSnapEnabled 	= ( selection.GetSelection().Count > 0 ) ? true : false;
			alignEnabled 		= ( selection.GetSelection().Count > 1 ) ? true : false;
			distroEnabled 		= ( selection.GetSelection().Count > 2 ) ? true	: false;
			spaceEnabled		= ( selection.GetSelection().Count > 1 ) ? true : false;

			if( R2DE_EditorWindow.Instance != null ) {
				utils.RepaintEditorWindow();
			}
		}

		float LocateTopY( List<CornerInfo> cornerInfos ) {
			float topY = cornerInfos[0].corners[0].y;
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				cornerInfo.userValue1 = cornerInfo.corners[0].y;
				for( int i = 0; i < 4; i++ ) {
					float potentialTarget = cornerInfo.corners[i].y;
					if( potentialTarget > cornerInfo.userValue1 ) {
						cornerInfo.userValue1 = potentialTarget;
					}
					if( potentialTarget > topY ) {
						topY = potentialTarget;
					}
				}
			}
			return topY;
		}

		float LocateBotY( List<CornerInfo> cornerInfos ) {
			float botY = cornerInfos[0].corners[0].y;
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				cornerInfo.userValue1 = cornerInfo.corners[0].y;
				for( int i = 0; i < 4; i++ ) {
					float potentialTarget = cornerInfo.corners[i].y;
					if( potentialTarget < cornerInfo.userValue1 ) {
						cornerInfo.userValue1 = potentialTarget;
					}
					if( potentialTarget < botY ) {
						botY = potentialTarget;
					}
				}
			}
			return botY;
		}

		float LocateLeftX( List<CornerInfo> cornerInfos ) {
			float leftX = cornerInfos[0].corners[0].x;
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				cornerInfo.userValue1 = cornerInfo.corners[0].x;
				for( int i = 0; i < 4; i++ ) {
					float potentialTarget = cornerInfo.corners[i].x;
					if( potentialTarget < cornerInfo.userValue1 ) {
						cornerInfo.userValue1 = potentialTarget;
					}
					if( potentialTarget < leftX ) {
						leftX = potentialTarget;
					}
				}
			}
			return leftX;
		}

		float LocateRightX( List<CornerInfo> cornerInfos ) {
			float rightX = cornerInfos[0].corners[0].x;
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				cornerInfo.userValue1 = cornerInfo.corners[0].x;
				for( int i = 0; i < 4; i++ ) {
					float potentialTarget = cornerInfo.corners[i].x;
					if( potentialTarget > cornerInfo.userValue1 ) {
						cornerInfo.userValue1 = potentialTarget;
					}
					if( potentialTarget > rightX ) {
						rightX = potentialTarget;
					}
				}
			}
			return rightX;
		}

		void LocateTopEdges( List<CornerInfo> cornerInfos ) {
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				cornerInfo.userValue1 = cornerInfo.corners[0].y;
				for( int i = 0; i < 4; i++ ) {
					float potentialTarget = cornerInfo.corners[i].y;
					if( potentialTarget > cornerInfo.userValue1 ) {
						cornerInfo.userValue1 = potentialTarget;
					}
				}
			}
		}

		void LocateBotEdges( List<CornerInfo> cornerInfos ) {
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				cornerInfo.userValue1 = cornerInfo.corners[0].y;
				for( int i = 0; i < 4; i++ ) {
					float potentialTarget = cornerInfo.corners[i].y;
					if( potentialTarget < cornerInfo.userValue1 ) {
						cornerInfo.userValue1 = potentialTarget;
					}
				}
			}
		}

		void LocateYMids( List<CornerInfo> cornerInfos ) {
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				float top = cornerInfo.corners[0].y;
				float bot = cornerInfo.corners[0].y;
				cornerInfo.userValue1 = top;
				for( int i = 0; i < 4; i++ ) {
					float potentialTarget = cornerInfo.corners[i].y;
					if( potentialTarget > top ) {
						top = potentialTarget;
					}
					else if( potentialTarget < bot ) {
						bot = potentialTarget;
					}
				}
				cornerInfo.userValue1 = ( top + bot ) / 2f;
			}
		}

		void LocateLeftEdges( List<CornerInfo> cornerInfos ) {
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				cornerInfo.userValue1 = cornerInfo.corners[0].x;
				for( int i = 0; i < 4; i++ ) {
					float potentialTarget = cornerInfo.corners[i].x;
					if( potentialTarget < cornerInfo.userValue1 ) {
						cornerInfo.userValue1 = potentialTarget;
					}
				}
			}
		}
		
		void LocateRightEdges( List<CornerInfo> cornerInfos ) {
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				cornerInfo.userValue1 = cornerInfo.corners[0].x;
				for( int i = 0; i < 4; i++ ) {
					float potentialTarget = cornerInfo.corners[i].x;
					if( potentialTarget > cornerInfo.userValue1 ) {
						cornerInfo.userValue1 = potentialTarget;
					}
				}
			}
		}
		
		void LocateXMids( List<CornerInfo> cornerInfos ) {
			foreach( CornerInfo cornerInfo in cornerInfos ) {
				float left = cornerInfo.corners[0].x;
				float right = cornerInfo.corners[0].x;
				cornerInfo.userValue1 = left;
				for( int i = 0; i < 4; i++ ) {
					float potentialTarget = cornerInfo.corners[i].x;
					if( potentialTarget < left ) {
						left = potentialTarget;
					}
					else if( potentialTarget > right ) {
						right = potentialTarget;
					}
				}
				cornerInfo.userValue1 = ( left + right ) / 2f;
			}
		}


		List<CornerInfo> PrepareCornerInfos( string undoLabel ) {
			List<Transform> objs = selection.GetSelection();
			error = ADError.None;

			// load edge info & store the orginal obj position for undo  
			List<CornerInfo> cornerInfos = new List<CornerInfo>();
			foreach( Transform obj in objs ) {
				// make sure the Canvas is setup correctly
				CanvasRenderer canvasRenderer = obj.GetComponent<CanvasRenderer>();
				if( canvasRenderer != null && state.context.type != ContextType.Canvas ) {
					error = ADError.CanvasContext;
					return null;
				}

				// make sure NGUI is setup correctly
				R2DC_NGUI.NGUIState nguiState = ngui.ValidateState( obj.gameObject );
				if( nguiState == R2DC_NGUI.NGUIState.InvalidContext ) {
					error = ADError.NGUIContext;
					return null;
				}
				else if( nguiState == R2DC_NGUI.NGUIState.ReflectionError ) {
					error = ADError.NGUIReflection;
					return null;
				}

				CornerInfo cornerInfo = new CornerInfo();
				cornerInfo.transform = obj;
				Vector3[] corners = bounds.GetWorldCorners( obj, state.preferColliders );
				if( corners == null ) {
					return null;
				}
				cornerInfo.corners = corners;
				cornerInfos.Add( cornerInfo );

				Undo.RecordObject( obj.transform, undoLabel );
			}

			return cornerInfos;
		}

		List<CornerInfo> SortAsc( List<CornerInfo> infos ) {
			return infos.OrderBy( info => info.userValue1 ).ToList(); 
		}

		List<CornerInfo> SortDesc( List<CornerInfo> infos ) {
			return infos.OrderByDescending( info => info.userValue1 ).ToList(); 
		}
	}
}
