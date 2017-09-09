//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEngine;
	using UnityEditor;
	using System.Collections.Generic;
	
	public class R2DC_Guides {
		
		static R2DC_Guides instance;
		
		public static R2DC_Guides Instance {
			get {
				if( instance == null ) {
					instance = new R2DC_Guides(); 
				}
				return instance;
			}
		}

		public Vector2 liveGuide;
		public Vector2 hoverGuide;

		bool creatingHGuide = false;
		bool creatingVGuide = false;
		R2DFrame viewPixelFrame;
		R2DD_State state;
		R2DC_Utils utils;

		private R2DC_Guides() {
			viewPixelFrame = R2DD_ContextInfo.Instance.viewPixelFrame;
			state = R2DD_State.Instance;
			utils = R2DC_Utils.Instance;
		}

		public void HandleInteraction() {
			EventType eventType = Event.current.type;
			if( Event.current.button != 0 ) {
				eventType = EventType.Ignore;
			}

			Vector2 mousePosition = Event.current.mousePosition;
			mousePosition.y = viewPixelFrame.height - mousePosition.y + 5f; 

			int hGuideIndex = -1;
			int vGuideIndex = -1;

			if( hoverGuide != Vector2.zero) {
				utils.RepaintSceneView();
			}
			hoverGuide = Vector2.zero;

			if( creatingHGuide ) {
				ShowHorizontalCursor();
			}
			else if( creatingVGuide ) {
				ShowVerticalCursor();
			}
			else {
				if( IsInRenderBounds( mousePosition ) && state.displayGuides ) {
					hGuideIndex = GetHGuideHitBoxIndex( mousePosition );
					if( hGuideIndex == -1 ) {
						vGuideIndex = GetVGuideHitBoxIndex( mousePosition );
						if( vGuideIndex != -1 ) {
							ShowVerticalCursor();
							hoverGuide.x = float.Parse( state.vGuides[vGuideIndex] );
							utils.RepaintSceneView();
						}
					}
					else {
						ShowHorizontalCursor();
						hoverGuide.y = float.Parse( state.hGuides[hGuideIndex] );
						utils.RepaintSceneView();
					}
				}
			}

			switch ( eventType ) {
			
			case EventType.MouseDown:
				if( !IsCreatingGuides() ) {
					if( IsOnTopRuler( mousePosition ) ) {
						HotControl();
						creatingHGuide = true;
						utils.RepaintSceneView();
					} 
					else if( IsOnLeftRuler( mousePosition ) ) {
						HotControl();
						creatingVGuide = true;
						utils.RepaintSceneView();
					} 
					else {

						if ( hGuideIndex > -1 ) {
							HotControl();
							state.hGuides.RemoveAt( hGuideIndex );
							creatingHGuide = true;
							UpdateLiveGuidePosition( mousePosition );
							utils.RepaintSceneView();
						}
						else {

							if ( vGuideIndex > -1 ) {
								HotControl();
								state.vGuides.RemoveAt( vGuideIndex );
								creatingVGuide = true;
								UpdateLiveGuidePosition( mousePosition );
								utils.RepaintSceneView();
							}
						}
					}
				}
				break;
			
			case EventType.MouseUp:
				if( IsCreatingGuides() ) {
					if( IsInRenderBounds( mousePosition ) ) {
						CompleteGuideCreation();
					}
					else {
						CancelGuideCreation();
					}
					GUIUtility.hotControl = 0; 
					utils.RepaintSceneView();
				} 
				break;

			case EventType.MouseDrag:
				if( IsCreatingGuides() ) {
					if( IsInRenderBounds( mousePosition ) ) {
						UpdateLiveGuidePosition( mousePosition );
					}
					else if( IsOnCorner( mousePosition ) ||
					        ( IsOnTopRuler( mousePosition ) && creatingHGuide ) ||
					        ( IsOnLeftRuler( mousePosition ) && creatingVGuide )) {
						// just hide the live guide
						liveGuide = Vector2.zero; 
					}
					utils.RepaintSceneView();
				}
				break;

			default:
				if( IsCreatingGuides() && IsOutOfBounds( mousePosition ) ) {
					GUIUtility.hotControl = 0;
					CancelGuideCreation();
					utils.RepaintSceneView();
				} 
				break;
			}
		}

		void HotControl() {
			GUIUtility.hotControl = GUIUtility.GetControlID( FocusType.Passive );
			Event.current.Use();
		}

		void UpdateLiveGuidePosition( Vector2 mousePosition ) {
			liveGuide = utils.GetWorldCoord( mousePosition );
			if( creatingHGuide ) {
				liveGuide.x = 0;

			}
			else {
				liveGuide.y = 0;
			}
		}

		public bool IsCreatingGuides() {
			return ( creatingHGuide || creatingVGuide );
		}

		void CompleteGuideCreation( ) {
			if( creatingHGuide ) {
				if( state.snapGuideToInt ) {
					int contextY = Mathf.RoundToInt( utils.GetWorldToContextY( liveGuide.y ) );
					liveGuide.y = utils.GetContextToWorldY( contextY );
				}
				// pure zero is reserved for logical testing
				if( liveGuide.y == 0 ) {
					liveGuide.y = Globals.FALSE_ZERO;
				}
				state.hGuides.Add( liveGuide.y.ToString() );
			}
			else {
				if( state.snapGuideToInt ) {
					int contextX = Mathf.RoundToInt( utils.GetWorldToContextX( liveGuide.x ) );
					liveGuide.x = utils.GetContextToWorldX( contextX );
				}
				// pure zero is reserved for logical testing
				if( liveGuide.x == 0 ) {
					liveGuide.x = Globals.FALSE_ZERO;
				}
				state.vGuides.Add( liveGuide.x.ToString() );
			}
			
			liveGuide = Vector2.zero;
			creatingHGuide = false;
			creatingVGuide = false;
			state.displayGuides = true;
			utils.RepaintEditorWindow();
		}


		void CancelGuideCreation() {
			liveGuide = Vector2.zero;
			creatingHGuide = false;
			creatingVGuide = false;
			
			utils.RepaintEditorWindow();
		}

		bool IsOnCorner( Vector2 pos ) {
			return ( pos.x > 0 && pos.x < 20f && pos.y < viewPixelFrame.height && pos.y >= viewPixelFrame.height - 20f );
		}

		bool IsOnTopRuler( Vector2 pos ) {
			return ( pos.x >= 20f && pos.y < viewPixelFrame.height && pos.y >= viewPixelFrame.height - 20f );
		}

		bool IsOnLeftRuler( Vector2 pos ) {
			return ( pos.x > 0f && pos.x <= 20f && pos.y < viewPixelFrame.height - 20f );
		}

		bool IsOutOfBounds( Vector2 pos ) {
			return ( pos.x <= 0 || pos.y <= 0 || pos.x >= viewPixelFrame.width || pos.y >= viewPixelFrame.height );
		}

		bool IsInRenderBounds( Vector2 pos ) {
			return ( pos.x > 20f && pos.y > 0f && pos.x < viewPixelFrame.width && pos.y < viewPixelFrame.height - 20f );
		}

		int GetVGuideHitBoxIndex( Vector2 pos ) {
			bool mouseDown = Event.current.type == EventType.MouseDown ? true : false;
			List<string> vGuides = state.vGuides;
			for( int i = 0; i < vGuides.Count; i++ ) {
				Vector2 pixelCoord = utils.GetWorldToScreenPixelCoord( new Vector2( float.Parse( vGuides[i] ), 0 ) ); 
				if( mouseDown ) {
					if( pos.x < pixelCoord.x + guideMouseDownHitBox && pos.x > pixelCoord.x - guideMouseDownHitBox ) {
						return i;
					}
				}
				else {
					if( pos.x < pixelCoord.x + guideHitBox && pos.x > pixelCoord.x - guideHitBox ) {
						return i;
					}
				}
			}
			return -1;
		}

		int GetHGuideHitBoxIndex( Vector2 pos ) {
			bool mouseDown = Event.current.type == EventType.MouseDown ? true : false;
			List<string> hGuides = state.hGuides;
			for( int i = 0; i < hGuides.Count; i++ ) {
				Vector2 pixelCoord = utils.GetWorldToScreenPixelCoord( new Vector2( 0, float.Parse( hGuides[i] ) ) );
				if( mouseDown ) {
					if( pos.y < pixelCoord.y + guideMouseDownHitBox && pos.y > pixelCoord.y - guideMouseDownHitBox ) {
						return i;
					}
				}
				else
				{
					if( pos.y < pixelCoord.y + ( guideHitBox * 2f ) && pos.y > pixelCoord.y - guideHitBox ) {
						return i;
					}
				}
			}
			return -1;
		}

		void ShowHorizontalCursor() { 
			EditorGUIUtility.AddCursorRect( R2DD_ContextInfo.Instance.cam.pixelRect, MouseCursor.SplitResizeUpDown);
		}

		void ShowVerticalCursor() {
			EditorGUIUtility.AddCursorRect( R2DD_ContextInfo.Instance.cam.pixelRect, MouseCursor.SplitResizeLeftRight);
		}

		const int guideHitBox = 3;
		const int guideMouseDownHitBox = 6;
	}
}
