//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {

	using UnityEngine;
	using UnityEditor;
	using System.Text;

	public class R2DV_Drawing {

		static R2DV_Drawing instance;
		
		public static R2DV_Drawing Instance {
			get {
				if( instance == null ) {
					instance = new R2DV_Drawing(); 
				}
				return instance;
			}
		}

		private R2DV_Drawing() {
			LoadFontStyles();
		}


		#region Scene View Helpers
		public void DrawTexture( Texture2D texture, int x, int y, int width, int height ) {
			GUI.DrawTexture( new Rect(x, y, width, height ),
			                 texture, 
			                 ScaleMode.StretchToFill,
			                 true, 
			                 0	);
		}

		public void DrawFloatTexture( Texture2D texture, float x, float y, float width, float height ) {
			GUI.DrawTexture( new Rect(x, y, width, height ),
			                texture, 
			                ScaleMode.StretchToFill,
			                false, 
			                0	);
		}

		public void DrawSimpleTexture( Texture2D texture, Vector2 location ) {
			location.x -= texture.width / 2f;
			location.y -= texture.height / 2f;
			GUI.DrawTexture( new Rect( location, new Vector2( texture.width, texture.height ) ), texture );
		}
			
		public void DrawLine( Texture2D texture, Vector2 start, Vector2 end ) {
			DrawLineExec( (int)start.x, (int)start.y, (int)end.x, (int)end.y, texture );
		}

		public void DrawLineExec( int x,int y,int x2, int y2, Texture2D texture ) {
			int w = x2 - x;
			int h = y2 - y;
			int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
			if ( w < 0 ) dx1 = -1 ; else if ( w > 0 ) dx1 = 1;
			if ( h < 0 ) dy1 = -1 ; else if ( h > 0 ) dy1 = 1;
			if ( w < 0 ) dx2 = -1 ; else if ( w > 0 ) dx2 = 1;
			int longest = Mathf.Abs( w );
			int shortest = Mathf.Abs( h );
			if ( !( longest > shortest ) ) {
				longest = Mathf.Abs( h );
				shortest = Mathf.Abs( w );
				if ( h < 0 ) dy2 = -1 ; else if ( h > 0 ) dy2 = 1;
				dx2 = 0;            
			}
			int numerator = longest >> 1;
			for ( int i = 0; i <= longest; i++ ) {
				GUI.DrawTexture( new Rect( x, y, 1, 1 ), texture );
				numerator += shortest;
				if ( !( numerator < longest ) ) {
					numerator -= longest;
					x += dx1;
					y += dy1;
				} else {
					x += dx2;
					y += dy2;
				}
			}
		}

		public void DrawMeausureLabel( string text, float x, float y, bool below ) {
			styleCoord.normal.background = R2DD_Resources.Instance.coordBg;
			Rect labelRect = GUILayoutUtility.GetRect( new GUIContent( text ), styleCoord );
			labelRect.x = x - ( labelRect.width / 2f );
			if( below ) {
				labelRect.y = y + 5f;
			}
			else {
				labelRect.y = y - labelRect.height - 5f;
			}
			GUI.Label( labelRect, text, styleCoord );
		}

		public void DrawPanelLabel( string text, int x, int y ) {
			GUI.Label( new Rect( x, y, 100, 10), text, styleRegular );
		}

		public void DrawVLabel( string text, int x, int y ) {
			StringBuilder sb = new StringBuilder();

			foreach (char ch in text) {
				sb.Append(ch).Append("\n");
			}

			GUI.Label( new Rect( x, y, 100, 10), sb.ToString(), styleRegular );
		}
		#endregion

		#region Panel Helpers
		public float DrawFloatFieldWithWidth( string label, float value, float width = 40f ) {
			return EditorGUILayout.FloatField( label, value, GUILayout.Width( width ) );
		}

		public float DrawFloatField( string label, float value ) {
			return EditorGUILayout.FloatField( label, value );
		}

		public int DrawIntField( string label, int value ) {
			return EditorGUILayout.IntField( label, value );
		}

		public int DrawToolbar( int selectedIndex, Texture[] icons ) {
			return GUILayout.Toolbar( selectedIndex, icons );
		}
		
		public int DrawPopup( int selectedIndex, string[] selStrings ) {
			return EditorGUILayout.Popup( selectedIndex, selStrings ) ; 
		}
		
		public void DrawPanelLabel( string label ) {
			EditorGUILayout.LabelField( label );
		}

		public void DrawCenteredLabel( string label ) {
			GUIStyle labelStyle = GUI.skin.GetStyle("Label");
			TextAnchor oldAnchor = labelStyle.alignment;
			labelStyle.alignment = TextAnchor.MiddleCenter;
			EditorGUILayout.LabelField( label, labelStyle );
			labelStyle.alignment = oldAnchor;
		}

		public void DrawSpace( float amount = 1f ) {
			GUILayout.Space( amount );
		}

		public void DrawPanelTitle( string label, Texture image ) {
			EditorGUILayout.LabelField( new GUIContent( label, image ), EditorStyles.boldLabel, GUILayout.Width( 150f ), GUILayout.Height( 22f ) );
		}
		
		public int DrawIntegerField( string label, int value ) {
			return EditorGUILayout.IntField( label, value );
		}

		public bool DrawImageButton( Texture2D image ) {
			Texture2D oldNormalBg = GUI.skin.button.normal.background;
			Texture2D oldHoverBg  = GUI.skin.button.hover.background;
			Texture2D oldActiveBg = GUI.skin.button.active.background;
			
			GUI.skin.button.normal.background = image;
			GUI.skin.button.hover.background = image;
			GUI.skin.button.active.background = image;
			
			bool wasClicked = GUILayout.Button( "", GUILayout.Width( image.width ), GUILayout.Height( image.height ) );
			
			GUI.skin.button.normal.background = oldNormalBg;
			GUI.skin.button.hover.background = oldHoverBg;
			GUI.skin.button.active.background = oldActiveBg;
			
			return wasClicked;
		}

		public bool DrawButton( string label ) {
			return GUILayout.Button( label );
		}

		public bool DrawToolButton( Texture texture, bool enabled, string toolTip ) {
			bool oldEnabled = GUI.enabled;
			if( !enabled ) {
				GUI.enabled = false; 
			}
			bool state = GUILayout.Button( new GUIContent( texture, toolTip ), GUILayout.MaxWidth( 30 ), GUILayout.MaxHeight( 30 ) );
			GUI.enabled = oldEnabled;
			return state;
		}

		public bool DrawToolTextButton( string label, bool enabled ) {
			bool oldEnabled = GUI.enabled;
			if( !enabled ) {
				GUI.enabled = false; 
			}
			bool state = GUILayout.Button( label );
			GUI.enabled = oldEnabled;
			return state;
		}
	
		public void DrawCoords( string label ) {
			styleCoord.normal.background = R2DD_Resources.Instance.coordBg;
			GUILayout.Label( label, styleCoord );
		}

		public void DrawMeasureAlert( string label ) {
			styleCoord.normal.background = R2DD_Resources.Instance.measureAlertBg;
			GUILayout.Label( label, styleCoord );
		}
		
		public bool DrawToggle( string label, bool value  ) {
			return EditorGUILayout.Toggle( label, value );
		}

		public bool DrawToggleWithWidth( string label, bool value, float width  ) {
			float oldLabelWidth = EditorGUIUtility.labelWidth;
			EditorGUIUtility.labelWidth = width;
			bool toggleVal = EditorGUILayout.Toggle( label, value );
			EditorGUIUtility.labelWidth = oldLabelWidth;
			return toggleVal;
		}

		public void DrawPanelTexture( Texture texture ) {
			GUILayout.Label( new GUIContent( texture ) );
		}

		public void DrawHelpBox( string text ) {
			EditorGUILayout.HelpBox( text, MessageType.Info );
		}

		public void DrawErrorBox( string text ) {
			EditorGUILayout.HelpBox( text, MessageType.Error );
		}
		
		public void BeginEditorVertical() {
			EditorGUILayout.BeginVertical();
		}
		
		public void EndEditorVertical() {
			EditorGUILayout.EndVertical();
		}
		
		public void BeginEditorHorizontal() {
			EditorGUILayout.BeginHorizontal();
		}
		
		public void EndEditorHorizontal() {
			EditorGUILayout.EndHorizontal();
		}
		
		public void FlexibleSpace() {
			GUILayout.FlexibleSpace();
		}

		public void BeginGUIArea( float x, float y, float width, float height ) {
			GUILayout.BeginArea( new Rect( x, y, width, height ) );	
		}

		public void BeginGUIHorizontal() {
			GUILayout.BeginHorizontal();
		}

		public void EndGUIHorizontal() {
			GUILayout.EndHorizontal();
		}

		public void BeginGUIVertical() {
			GUILayout.BeginVertical();
		}

		public void EndGUIVertical() {
			GUILayout.EndVertical();
		}

		public void EndGUIArea() {
			GUILayout.EndArea();
		}

		void LoadFontStyles() {
			styleRegular = new GUIStyle();
			styleRegular.font = R2DD_Resources.Instance.regularFont;
			styleRegular.fontSize = 9;
			if( EditorGUIUtility.isProSkin)
				styleRegular.normal.textColor = new Color(0.705f, 0.705f, 0.705f); 
			styleRegular.stretchWidth = false;
			styleRegular.stretchHeight = false;
			styleRegular.fontStyle = FontStyle.Normal;

			styleCoord = new GUIStyle();
			styleCoord.font = R2DD_Resources.Instance.regularFont;
			styleCoord.fontSize = 9;
			if( EditorGUIUtility.isProSkin )
				styleCoord.normal.textColor = new Color(0.705f, 0.705f, 0.705f);
			styleCoord.stretchWidth = false;
			styleCoord.stretchHeight = false;
			styleCoord.fontStyle = FontStyle.Normal;
			styleCoord.border = new RectOffset( 5, 5, 5, 5 );
			styleCoord.padding = new RectOffset( 3, 3, 3, 2 );
		}
		#endregion

		GUIStyle styleRegular;
		GUIStyle styleCoord;
	}
}