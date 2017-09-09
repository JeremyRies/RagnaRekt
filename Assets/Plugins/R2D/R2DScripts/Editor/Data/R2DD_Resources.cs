//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™ 
//----------------------------------------------

namespace R2D {

	using UnityEngine;
	using UnityEditor;
	using System;
	using System.IO;
	using System.Collections.Generic;

	public class R2DD_Resources {
		static R2DD_Resources instance;
		
		public static R2DD_Resources Instance {
			get {
				if( instance == null ) {
					instance = new R2DD_Resources();
				}
				return instance;
			}
		}

		public Texture2D rulerHBg;
		public Texture2D rulerVBg;
		public Texture2D rulerLinePixel;
		public Texture2D corner;
		public Texture2D pixel;
		public Texture logo;
		public Texture 	windowIcon;
		public Texture 	panelSettings;
		public Texture 	panelGrid; 
		public Texture 	panelToolbox;
		public Texture  barToolbox;
		public Texture 	toolDistribute;
		public Texture	toolPresets;
		public Texture2D help;
		public Font regularFont;
		public Texture2D crossHairPixel;
		public Texture2D liveGuidePixel;
		public Texture2D guidePixel;
		public Texture guideIcon;
		public Texture2D coordBg;
		public Texture2D measureAlertBg;
		public Texture alignTop;
		public Texture alignYMid;
		public Texture alignBot;
		public Texture alignLeft;
		public Texture alignXMid;
		public Texture alignRight;
		public Texture distroTop;
		public Texture distroYMid;
		public Texture distroBot;
		public Texture distroLeft;
		public Texture distroXMid;
		public Texture distroRight;
		public Texture snapLeft;
		public Texture snapRight;
		public Texture snapTop;
		public Texture snapBot;
		public Texture spaceX; 
		public Texture spaceY;
		public Texture measureActive;
		public Texture measureInactive;
		public Texture clearMeasure;
		public Texture measureObj;
		public Texture2D measurePixel;
		public Texture2D measureCross;
		public Texture2D gridBorder;

		string r2dResourcesPath; 

		private R2DD_Resources() {
			string[] dirs = Directory.GetDirectories( Environment.CurrentDirectory, 
			                                          pathResourcesDir,
			                                          SearchOption.AllDirectories );
			r2dResourcesPath = dirs[0].Replace( Environment.CurrentDirectory + Path.DirectorySeparatorChar, "" ) + Path.DirectorySeparatorChar;

			string skinPrefix = "";
			if( EditorGUIUtility.isProSkin ) {
				skinPrefix = pathPro + Path.DirectorySeparatorChar;
			}
			else {
				skinPrefix = pathStandard + Path.DirectorySeparatorChar;
			}

			rulerHBg 		= LoadTexture2D( skinPrefix + pathRulerHBg );
			rulerVBg 		= LoadTexture2D( skinPrefix + pathRulerVBg );
			rulerLinePixel 	= LoadTexture2D( pathRulerLinePixel );
			corner			= LoadTexture2D( skinPrefix + pathImgCorner );
			pixel 			= LoadTexture2D( pathImgPixel );
			help			= LoadTexture2D( pathHelp );
			crossHairPixel 	= LoadTexture2D( pathCrossHairPixel );
			liveGuidePixel	= LoadTexture2D( pathLiveGuidePixel );
			guidePixel		= LoadTexture2D( pathGuidePixel );
			windowIcon		= LoadTexture( skinPrefix + pathWindowIcon );
			panelSettings	= LoadTexture( skinPrefix + pathToolSettings );
			panelGrid		= LoadTexture( skinPrefix + pathToolGrids );
			panelToolbox	= LoadTexture( skinPrefix + pathToolbox );
			toolDistribute	= LoadTexture( skinPrefix + pathToolDistribute );
			toolPresets		= LoadTexture( skinPrefix + pathToolPresets );
			guideIcon 		= LoadTexture( pathGuideIcon );
			coordBg 		= LoadTexture2D( skinPrefix + pathCoordBg );
			alignTop		= LoadTexture( skinPrefix + pathAlignTop );
			alignYMid		= LoadTexture( skinPrefix + pathAlignYMid ); 
			alignBot		= LoadTexture( skinPrefix + pathAlignBot );
			alignLeft		= LoadTexture( skinPrefix + pathAlignLeft );
			alignXMid		= LoadTexture( skinPrefix + pathAlignXMid );
			alignRight		= LoadTexture( skinPrefix + pathAlignRight );
			distroTop		= LoadTexture( skinPrefix + pathDistroTop );
			distroYMid		= LoadTexture( skinPrefix + pathDistroYMid );
			distroBot		= LoadTexture( skinPrefix + pathDistroBot );
			distroLeft		= LoadTexture( skinPrefix + pathDistroLeft );
			distroXMid		= LoadTexture( skinPrefix + pathDistroXMid );
			distroRight		= LoadTexture( skinPrefix + pathDistroRight );
			snapLeft		= LoadTexture( skinPrefix + pathSnapLeft );
			snapRight		= LoadTexture( skinPrefix + pathSnapRight );
			snapBot			= LoadTexture( skinPrefix + pathSnapBot );
			snapTop			= LoadTexture( skinPrefix + pathSnapTop );
			spaceX			= LoadTexture( skinPrefix + pathSpaceX );
			spaceY			= LoadTexture( skinPrefix + pathSpaceY );
			barToolbox		= LoadTexture( skinPrefix + pathBarToolbox );
			measureActive	= LoadTexture( skinPrefix + pathMeasureActive );
			measureInactive = LoadTexture( skinPrefix + pathMeasureInactive ); 
			clearMeasure	= LoadTexture( skinPrefix + pathClearMeasure );
			measureObj		= LoadTexture( skinPrefix + pathMeasureObj );
			measurePixel	= LoadTexture2D( pathMeasurePixel );
			measureCross	= LoadTexture2D( pathMeasureCross );
			measureAlertBg	= LoadTexture2D( skinPrefix + pathMeasureAlertBg );
			gridBorder		= LoadTexture2D( pathGridBorder );
			logo			= LoadTexture( skinPrefix + pathLogo );

			regularFont = LoadFont( pathFntRegular );
		}

		Texture2D LoadTexture2D( string textureName ) {
			return AssetDatabase.LoadAssetAtPath( r2dResourcesPath + textureName, typeof(Texture2D) ) as Texture2D;
		}

		Texture LoadTexture( string textureName ) {
			return AssetDatabase.LoadAssetAtPath( r2dResourcesPath + textureName, typeof(Texture) ) as Texture;
		}

		Font LoadFont( string fontName ) {
			return AssetDatabase.LoadAssetAtPath( r2dResourcesPath + fontName, typeof(Font) ) as Font;
		}

		Sprite LoadSprite( string spriteName ) {
			return AssetDatabase.LoadAssetAtPath( r2dResourcesPath + spriteName, typeof(Sprite) ) as Sprite;
		}
		
		string pathResourcesDir 	= "R2DResources";
		string pathPro				= "Pro";
		string pathStandard			= "Personal";
		string pathRulerHBg			= "R2DImgRulerHBg.png";
		string pathRulerVBg			= "R2DImgRulerVBg.png";
		string pathRulerLinePixel 	= "R2DImgRulerLinePixel.png";
		string pathImgCorner		= "R2DImgCorner.png";
		string pathImgPixel			= "R2DImgPixel.png";
		string pathFntRegular		= "R2DFntMain.ttf";
		string pathWindowIcon		= "R2DImgLogo.png";
		string pathToolSettings		= "R2DImgSettings.png";
		string pathToolGrids		= "R2DImgGrid.png";
		string pathToolbox			= "R2DImgToolbox.png";
		string pathToolDistribute	= "R2DImgDistribute.png";
		string pathToolPresets		= "R2DImgPresets.png";
		string pathHelp				= "R2DImgHelp.png";
		string pathCrossHairPixel	= "R2DImgCrossHairPixel.png";
		string pathGuideIcon		= "R2DImgGuideIcon.png";
		string pathLiveGuidePixel	= "R2DImgLiveGuidePixel.png";
		string pathGuidePixel		= "R2DImgGuidePixel.png";
		string pathCoordBg			= "R2DImgCoordBg.png";
		string pathMeasureAlertBg	= "R2DImgMeasureAlertBg.png";
		string pathAlignTop			= "R2DImgAlignTop.png";
		string pathAlignYMid		= "R2DImgAlignHMid.png";
		string pathAlignBot			= "R2DImgAlignBot.png";
		string pathAlignLeft		= "R2DImgAlignLeft.png";
		string pathAlignXMid		= "R2DImgAlignVMid.png";
		string pathAlignRight		= "R2DImgAlignRight.png";
		string pathDistroTop		= "R2DImgDistroTop.png";
		string pathDistroYMid		= "R2DImgDistroYMid.png";
		string pathDistroBot		= "R2DImgDistroBot.png";
		string pathDistroLeft		= "R2DImgDistroLeft.png";
		string pathDistroXMid		= "R2DImgDistroXMid.png";
		string pathDistroRight		= "R2DImgDistroRight.png";
		string pathSnapLeft			= "R2DImgSnapLeft.png";
		string pathSnapRight		= "R2DImgSnapRight.png";
		string pathSnapTop			= "R2DImgSnapTop.png";
		string pathSnapBot			= "R2DImgSnapBot.png";
		string pathSpaceX			= "R2DImgSpaceX.png";
		string pathSpaceY			= "R2DImgSpaceY.png";
		string pathBarToolbox		= "R2DImgToolboxBar.png";
		string pathMeasureActive	= "R2DImgMeasureActive.png";
		string pathMeasureInactive	= "R2DImgMeasureInactive.png";
		string pathClearMeasure		= "R2DImgClearMeasure.png";
		string pathMeasurePixel		= "R2DImgMeasurePixel.png";
		string pathMeasureCross		= "R2DImgMeasureCross.png"; 
		string pathMeasureObj		= "R2DImgMeasureObj.png";
		string pathGridBorder		= "R2DImgGridBorder.png";
		string pathLogo				= "R2DImgPFLogo.png";



		public string urlSettingsHelp	= "http://pixelfire.co/ruler-2d-guides-grid-and-alignment-tools-for-unity/#settings"; 
		public string urlGridHelp		= "http://pixelfire.co/ruler-2d-guides-grid-and-alignment-tools-for-unity/#grid";
		public string urlToolboxHelp	= "http://pixelfire.co/ruler-2d-guides-grid-and-alignment-tools-for-unity/#toolbox";
	}
}
