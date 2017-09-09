//----------------------------------------------
// Ruler 2D
// Copyright © 2015-2020 Pixel Fire™
//----------------------------------------------

namespace R2D {
	
	using UnityEditor;
	using UnityEngine;
	using System;
	using System.Collections.Generic;
	
	public enum ContextType {
		Canvas,
		EditorScene,
		NGUI
	};
	
	public enum UnitsType {
		WorldUnits,
		Pixels
	};
	
	public class Context {
		public ContextType type;
		public GameObject gameObject;
		public int instanceId;
		
		public Context( ContextType type, GameObject gameObject ) {
			this.type = type;
			this.gameObject = gameObject;
			this.instanceId = gameObject == null ? 0 : gameObject.GetInstanceID();
		}
	}

	public enum R2DContext {
		Canvas,
		SceneEditor,
		OrthoCamera
	}
	
	public class R2DFrame {
		public Vector2	botLeft;
		public Vector2	topLeft;
		public Vector2	topRight;
		public Vector2	botRight;
		public float	width;
		public float	height;
	};

	public class Measurement {
		public float x;
		public float y;
		public float w;
		public float h;
		public float length;
		public float angle;
	};

	public class GridData {
		public Vector2 	worldOrigin;
		public Vector2 	worldUnitSize;
		public float 	worldWidth;
		public float 	worldHeight;
		public Vector2  worldCorner;
		public Vector2 	worldZero;
		public Vector2 	worldUnit;
	};


	public static class Globals {
		public const float FALSE_ZERO = 0.00001f;
	}
}