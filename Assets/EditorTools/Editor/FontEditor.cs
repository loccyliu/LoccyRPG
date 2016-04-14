#pragma warning disable 0618

using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace hpjFonts {
	public class FontEditor : EditorWindow {
		#region Variables
		GUISkin theSkin;
		Texture2D gradientTex;
		GUISkin editorSkin;
		float graphOffsetXcurrent, graphOffsetYcurrent;
		float graphOffsetXtarget, graphOffsetYtarget;
		Color gradientColor = new Color( 0.55f, 0.55f, 0.55f, 0.45f );
		Color BGCol = new Color( 0.46f, 0.5f, 0.46f, 1f );
		float windowPosHeight;
		Shader defaultShader;

		Rect r;
		float sidebarWidth = 240, nodeHeight = 378;
		float dividerSize = 10;
		int clickNodeIndex = -1, highlightedNodeIndex = -1, clickCornerIndex = -1;
		bool showOptions, editPivots, moveAllPivots, moveAllNodes;
		Rect oldRect;

		Texture2D fontMap, packedFontMap;
		string packedfontMapPath;
		Font font, packedFont;

		string errorString = "";
		bool showError;

		List<Rect> partitions;
		List<Vector2> anchors;
		PackNode getPackNode, swapPackNode;
		bool packOptionNPOT = true, packOptionRotate = true, packOptionSort = true, packOptionFit = true, packComplete;
		int packBuffer = 1, packSizeX, packSizeY, packMaxSizeX, packMaxSizeY;
		int packMethod, packSort;
		string[] packMethods = new string[] { "Type 1", "Type 2", "Type 3", "Type 4", "Unity Type" };
		string[] packSorts = new string[] { "By Height", "By Width", "Longest Length", "Shortest Length" };
		string[] snapModes = new string[] { "None", "Snap", "Axis Constraint X", "Axis Constraint Y" };
		int snapMode = 0;
		bool snapFound;
		Rect snapRect;
		Vector2 clickOffset, oldPivot, pivotOffset;


		bool smartMode, packMode;
		Color smartColor = Color.red;
		int[,] intFontMap;
		List<Vector2> shapeSmartLine, shape;
		int floodfillPadding = 0; //increase this to pad Rects out in autoset and shrinkwrap shape finding, no equivalent GUI control
		List<Node> autoNodeList, autoNodeListunsorted;
		bool autoSetDialog, repeating;
		List<char> repeatList;
		bool refocus;

		bool snapToPixels = true;

		enum ScreenArea {
			None,
			Graph,
			TopBar,
			Nodes,
			GraphDivider,
			NodesDivider,
			Options

		}


		bool nodesOverlap, nodesDupe;
		int nodesOverlapA, nodesOverlapB, nodesDupeA, nodesDupeB;

		bool onekeyPivot=false;

		Vector2 nodeScroll, optionScroll;

		int dInt;

		Dictionary<ScreenArea, Rect> screenAreas;
		Rect[] cornerHandles = new Rect[5];

		ScreenArea clickedArea;
		float zoomTarget = 1, zoomCurrent = 1;

		[SerializeField]
		List<Node> nodes;
		[SerializeField]
		List<PackNode> startPackNodeList, resultPackNodeList;
		#endregion

		#region classes

		[System.Serializable]
		class PackNode {
			public CharacterInfo CI;
			public int CIIndex;
			public Rect StartRect, ResultRect;
			public float Height;
			public bool SameOrient = true;

			public PackNode ( PackNode f ) {
				this.CI = f.CI;
				this.CIIndex = f.CIIndex;
				this.StartRect = f.StartRect;
				this.ResultRect = f.ResultRect;
				this.Height = f.Height;
				this.SameOrient = f.SameOrient;
			}
			public PackNode () {
				this.CI = new CharacterInfo();
				this.CIIndex = 0;
				this.StartRect = new Rect( 0, 0, 0, 0 );
				this.ResultRect = new Rect( 0, 0, 0, 0 );
				this.Height = 0;
				this.SameOrient = true;
			}

		}


		[System.Serializable]
		class Node {
			public Rect rect;
			public string ch;
			public bool orient;
			public Vector2 pivot, scale;
			public float advance;
			public bool overrideAdvance, overrideScale;
			public Node ( Rect r ) {
				this.rect = r;
				this.ch = "?";
				this.scale = Vector2.one;
			}
			public Node ( Node node ) {
				this.rect = new Rect( node.rect.x + node.rect.width, node.rect.y, node.rect.width, node.rect.height );
				this.ch = "" + (char) (node.ch[0] + 1);
				this.orient = node.orient;
				this.pivot = node.pivot;
				this.advance = node.advance;
				this.overrideAdvance = node.overrideAdvance;
				this.scale = node.scale;
				this.overrideScale = node.overrideScale;
			}
			public Node () {
				this.rect = new Rect( 0, 0, 100, 100 );
				this.ch = "?";
				this.scale = Vector2.one;
			}
		}
		#endregion

		#region Init
		[MenuItem( "EditorTools/Font Editor" )]
		static void Init () {
			FontEditor window = (FontEditor) EditorWindow.GetWindow( typeof( FontEditor ) );
			window.Show();
			window.position = new Rect( 20, 80, 770, 550 );
			window.titleContent = new GUIContent( "Font Editor" );
		}

		void OnEnable () {
			Undo.undoRedoPerformed -= UndoCallback;
			Undo.undoRedoPerformed += UndoCallback;
		}
		void OnDisable () {
			Undo.undoRedoPerformed -= UndoCallback;
		}

		void UndoCallback () {
			this.Repaint();
		}



		void ResetResources () {
			if (theSkin == null) {
				theSkin = (GUISkin) EditorGUIUtility.Load( "FontMaker/FontSetterSkin.GUISkin" );
			}
			if (defaultShader == null) {
				defaultShader = (Shader) EditorGUIUtility.Load( "FontMaker/Textured Text Shader.shader" );
			}
			if (screenAreas == null) {
				screenAreas = new Dictionary<ScreenArea, Rect>();
				screenAreas.Add( ScreenArea.Graph, new Rect( 0, 0, 0, 0 ) );
				screenAreas.Add( ScreenArea.GraphDivider, new Rect( 0, 0, 0, 0 ) );
				screenAreas.Add( ScreenArea.Nodes, new Rect( 0, 0, 0, 0 ) );
				screenAreas.Add( ScreenArea.NodesDivider, new Rect( 0, 0, 0, 0 ) );
				screenAreas.Add( ScreenArea.Options, new Rect( 0, 0, 0, 0 ) );
				screenAreas.Add( ScreenArea.TopBar, new Rect( 0, 0, 0, 0 ) );
			}
			if (nodes == null) {
				nodes = new List<Node>();
				nodes.Add( new Node( new Rect( 0, 0, 100, 100 ) ) );
				nodes.Add( new Node( new Rect( 200, 0, 100, 100 ) ) );
				nodes.Add( new Node( new Rect( 0, 200, 100, 100 ) ) );
				nodes.Add( new Node( new Rect( 200, 200, 100, 100 ) ) );
			}

			this.Repaint();
		}

		#endregion


		#region Toolbar functions
		void FileNew () {
			autoSetDialog = false;
			packMode = false;
			packComplete = false;
			highlightedNodeIndex = clickNodeIndex = -1;
			string path = EditorUtility.OpenFilePanel( "New Font From Image", "Assets", "" );
			if (path.Length != 0) {
				if (path.Contains( Application.dataPath )) {
					path = path.Replace( Application.dataPath, "" );
					path = "Assets" + path;
					fontMap = (Texture2D) AssetDatabase.LoadAssetAtPath( path, typeof( Texture2D ) );
					if (fontMap == null) {
						showError = true;
						errorString = "Unable to load this Image";
						fontMap = null;
					} else {
						NewFont( path );
					}
				} else {
					showError = true;
					errorString = "Image file should be in project assets";
				}
			}
		}

		void FileOpen () {
			autoSetDialog = false;
			packMode = false;
			packComplete = false;
			highlightedNodeIndex = clickNodeIndex = -1;
			string path = EditorUtility.OpenFilePanel( "Open Font", "Assets", "fontsettings" );
			if (path.Length != 0) {
				if (path.Contains( Application.dataPath )) {
					path = path.Replace( Application.dataPath, "" );
					path = "Assets" + path;
					font = (Font) AssetDatabase.LoadAssetAtPath( path, typeof( Font ) );
					LoadFont();
				} else {
					showError = true;
					errorString = "Font Settings file should be in project assets";
				}
			}
		}

		void LoadFont () {
			if (font.material == null) {
				showError = true;
				errorString = "Font has no Default Material";
				font = null;
				return;
			}
			if (font.material.mainTexture == null) {
				showError = true;
				errorString = "Font Material has no 'Main Texture'";
				font = null;
				return;
			}
			fontMap = (Texture2D) font.material.mainTexture;
			if (CheckMap()) {
				font = null;
				fontMap = null;
				return;
			}
			PopulateNodesFromFont();
			clickCornerIndex = clickNodeIndex = highlightedNodeIndex = -1;
			ViewReset();
			CheckDupeChars();
			CheckOverlapNodes();
		}

		void NewFont ( string path ) {
			if (CheckMap()) {
				fontMap = null;
				return;
			}

			path = path.Substring( 0, path.LastIndexOf( "." ) );

			Font existingFont = (Font) AssetDatabase.LoadAssetAtPath( path + "(Font).fontsettings", typeof( Font ) );
			if (existingFont != null) {
				string newPath = AssetDatabase.GenerateUniqueAssetPath( path + "(Font).fontsettings" );
				string newName = newPath.Substring( newPath.LastIndexOf( "/" ) + 1 );
				newName = newName.Substring( 0, newName.LastIndexOf( "." ) );
				if (EditorUtility.DisplayDialog( "Asset Exists", "There is already a Font called " + existingFont.name, "Create new Font [\"" + newName + "\"]", "Use Existing Font" )) {
					font = new Font();
					AssetDatabase.CreateAsset( font, newPath );
				} else {
					font = existingFont;
				}
			} else {
				font = new Font();
				AssetDatabase.CreateAsset( font, path + "(Font).fontsettings" );
			}

			Material fontMaterial = font.material;
			if (fontMaterial == null) {
				Material existingMaterial = (Material) AssetDatabase.LoadAssetAtPath( path + "(Material).mat", typeof( Material ) );
				if (existingMaterial != null) {
					string newPath = AssetDatabase.GenerateUniqueAssetPath( path + "(Material).mat" );
					string newName = newPath.Substring( newPath.LastIndexOf( "/" ) + 1 );
					newName = newName.Substring( 0, newName.LastIndexOf( "." ) );
					if (EditorUtility.DisplayDialog( "Asset Exists", "There is already a Material called " + existingMaterial.name, "New Material [\"" + newName + "\"]", "Use Existing" )) {
						fontMaterial = new Material( defaultShader );
						AssetDatabase.CreateAsset( fontMaterial, newPath );
					} else {
						fontMaterial = existingMaterial;
					}
				} else {
					fontMaterial = new Material( defaultShader );
					AssetDatabase.CreateAsset( fontMaterial, path + "(Material).mat" );
				}


				font.material = fontMaterial;
			}
			fontMaterial.mainTexture = fontMap;

			PopulateNodesFromFont();
			clickCornerIndex = clickNodeIndex = highlightedNodeIndex = -1;
			ViewReset();

		}

		bool CheckMap () {
			if (!CheckNPOT( fontMap )) {
				showError = true;
				errorString = "Font Map is NPOT, Change the Import Settings property for NPOT maps to 'None'";
				font = null;
				return true;
			}
			if (CheckImportSize( fontMap )) {
				showError = true;
				errorString = "Your texture Map is larger than the import size, change this in the Import Settings";
				font = null;
				return true;
			}
			if (!CheckImportFormat( fontMap )) {
				showError = true;
				errorString = "Please set the Font Map to a compatible format in its Texture Settings (eg. Compressed, Truecolor, RGBA32)";
				font = null;
				return true;
			}
			return false;
		}
		void FileClose () {
			if (autoSetDialog) {
				RemoveNode();
				EndAutoSet();
			}
			highlightedNodeIndex = clickNodeIndex = -1;
			autoSetDialog = false;
			packMode = false;
			packComplete = false;
			fontMap = null;
			font = null;
			//nodes.Clear();

		}
		void FileExit () {
			this.Close();
		}
		void EditUndo () {
			Undo.PerformUndo();
		}
		void EditRedo () {
			Undo.PerformRedo();
		}
		void EditExpandNodes () {
			string s = "Expanded nodes for:";
			for (int i = 0; i < nodes.Count; i++) {
				if (nodes[i].rect.width < 1 || nodes[i].rect.height < 1) {
					nodes[i].rect.width = 4;
					nodes[i].rect.height = 4;
					UpdateFont( i );
					s += "'" + nodes[i].ch + "', ";
				}
			}
			if (s.Length > 19) {
				s = s.Substring( 0, s.Length - 2 );
				Debug.Log( s );
			} else {
				Debug.Log( "No tiny nodes found" );
			}
		}
		void ViewReset () {
			zoomTarget = 1;
			if (packComplete) {
				graphOffsetXtarget = Mathf.Round( screenAreas[ScreenArea.Graph].width / 2 - packedFontMap.width / 2 );
				graphOffsetYtarget = Mathf.Round( screenAreas[ScreenArea.Graph].height / 2 - packedFontMap.height / 2 );
			} else {
				graphOffsetXtarget = Mathf.Round( screenAreas[ScreenArea.Graph].width / 2 - fontMap.width / 2 );
				graphOffsetYtarget = Mathf.Round( screenAreas[ScreenArea.Graph].height / 2 - fontMap.height / 2 );
			}
		}

		void ViewFocus () {
			if (highlightedNodeIndex == -1)
				return;
			zoomTarget = 4;
			while (nodes[highlightedNodeIndex].rect.width * zoomTarget > screenAreas[ScreenArea.Graph].width || nodes[highlightedNodeIndex].rect.height * zoomTarget > screenAreas[ScreenArea.Graph].height) {
				zoomTarget = zoomTarget / 2;
			}
			graphOffsetXtarget = Mathf.Round( screenAreas[ScreenArea.Graph].width / 2 - (nodes[highlightedNodeIndex].rect.x + nodes[highlightedNodeIndex].rect.width / 2) * zoomTarget );
			graphOffsetYtarget = Mathf.Round( screenAreas[ScreenArea.Graph].height / 2 - (fontMap.height - nodes[highlightedNodeIndex].rect.y - nodes[highlightedNodeIndex].rect.height / 2) * zoomTarget );

		}

		void ToolsShrinkWrap () {
			if (!CheckReadWrite( fontMap ))
				return;
			ShrinkWrap();
		}
		void ToolsAutoBasic () {
			if (!CheckReadWrite( fontMap ))
				return;
			smartMode = false;
			AutoSet();
		}
		void ToolsAutoSmart () {
			if (!CheckReadWrite( fontMap ))
				return;
			smartMode = true;
			AutoSet();
		}
		void ToolsPack () {
			if (!CheckReadWrite( fontMap ))
				return;
			packMode = true;
			showOptions = false;
			nodeHeight = position.height - 22;
		}

		#endregion

		void OnGUI () {
			if (refocus) {
				GUI.FocusControl( "" );
				refocus = false;
			}
			Undo.RecordObject( this, "Edited Font Settings" );
			wantsMouseMove = true;
			Event e = Event.current;


			if (screenAreas == null || theSkin == null)
				ResetResources();

			EditorGUILayout.BeginHorizontal();

			EditorGUILayout.BeginVertical();
			#region Top bar
			screenAreas[ScreenArea.TopBar] = EditorGUILayout.BeginHorizontal( EditorStyles.toolbar, GUILayout.ExpandWidth( true ) );

			Rect buttonRect = GUILayoutUtility.GetRect( new GUIContent( "File" ), EditorStyles.toolbarDropDown );
			if (GUI.Button( buttonRect, "File", EditorStyles.toolbarDropDown )) {
				GenericMenu menu = new GenericMenu();
				menu.AddItem( new GUIContent( "Open Font" ), false, FileOpen );
				menu.AddItem( new GUIContent( "Create New Font From Image" ), false, FileNew );
				menu.AddItem( new GUIContent( "Close Font" ), false, FileClose );
				menu.AddItem( new GUIContent( "Exit" ), false, FileExit );
				menu.DropDown( buttonRect );
				EditorGUIUtility.ExitGUI();
			}

			GUI.enabled = (font != null && fontMap != null) && !autoSetDialog && !packMode;

			buttonRect = GUILayoutUtility.GetRect( new GUIContent( "Edit" ), EditorStyles.toolbarDropDown );
			if (GUI.Button( buttonRect, "Edit", EditorStyles.toolbarDropDown )) {
				GenericMenu menu = new GenericMenu();
				menu.AddItem( new GUIContent( "Undo" ), false, EditUndo );
				menu.AddItem( new GUIContent( "Redo" ), false, EditRedo );
				menu.AddItem( new GUIContent( "Expand Tiny Nodes" ), false, EditExpandNodes );
				menu.DropDown( buttonRect );
				EditorGUIUtility.ExitGUI();
			}

			buttonRect = GUILayoutUtility.GetRect( new GUIContent( "View" ), EditorStyles.toolbarDropDown );
			if (GUI.Button( buttonRect, "View", EditorStyles.toolbarDropDown )) {
				GenericMenu menu = new GenericMenu();
				menu.AddItem( new GUIContent( "Reset View" ), false, ViewReset );
				menu.AddItem( new GUIContent( "Focus Selected" ), false, ViewFocus );
				menu.DropDown( buttonRect );
				EditorGUIUtility.ExitGUI();
			}
			buttonRect = GUILayoutUtility.GetRect( new GUIContent( "Tools" ), EditorStyles.toolbarDropDown );
			if (GUI.Button( buttonRect, "Tools", EditorStyles.toolbarDropDown )) {
				GenericMenu menu = new GenericMenu();
				menu.AddItem( new GUIContent( "Shrinkwrap" ), false, ToolsShrinkWrap );
				menu.AddItem( new GUIContent( "Auto-set gylphs" ), false, ToolsAutoBasic );
				menu.AddItem( new GUIContent( "Auto-set gylphs (smart)" ), false, ToolsAutoSmart );
				menu.AddItem( new GUIContent( "Pack Font Map" ), false, ToolsPack );
				menu.DropDown( buttonRect );
				EditorGUIUtility.ExitGUI();
			}
			GUI.enabled = true;
			GUILayout.FlexibleSpace();



			GUILayout.EndHorizontal();

			#endregion

			#region Graph Area
			GUILayout.Box( string.Empty, GUILayout.ExpandHeight( true ), GUILayout.ExpandWidth( true ) );
			if (Event.current.type == EventType.Repaint)
				screenAreas[ScreenArea.Graph] = GUILayoutUtility.GetLastRect();
			GUI.color = gradientColor;
			GUI.Box( screenAreas[ScreenArea.Graph], "", theSkin.GetStyle( "Gradient" ) );
			GUI.color = Color.white;


			#region No loaded
			if (fontMap == null || font == null) {
				GUI.BeginGroup( screenAreas[ScreenArea.Graph] );
				Rect r = new Rect( screenAreas[ScreenArea.Graph].x + screenAreas[ScreenArea.Graph].width / 2 - 170, screenAreas[ScreenArea.Graph].y + screenAreas[ScreenArea.Graph].height / 2 - 60, 150, 33 );
				GUI.Box( r, "Open Font" );
				Rect r2 = new Rect( r.x + 190, r.y, r.width, r.height );
				GUI.Box( r2, "New Font\nFrom Image" );
				r.y += 50;
				r2.y += 50;
				r.height = 16;
				r2.height = 16;
				font = (Font) EditorGUI.ObjectField( r, font, typeof( Font ), false );
				fontMap = (Texture2D) EditorGUI.ObjectField( r2, fontMap, typeof( Texture2D ), false );

				if (font != null) {
					LoadFont();
					GUIUtility.ExitGUI();
				}
				if (fontMap != null) {
					NewFont( AssetDatabase.GetAssetPath( fontMap ) );
					GUIUtility.ExitGUI();
				}
				GUI.EndGroup();

				#endregion
			} else {

				if (packMode) {
					GUI.BeginGroup( screenAreas[ScreenArea.Graph] );
					GUI.color = BGCol;
					if (packComplete) {
						r = CalculatePackRect( new Rect( 0, 0, packedFontMap.width, packedFontMap.height ) );
					} else {
						r = CalculateNodeRect( new Rect( 0, 0, fontMap.width, fontMap.height ) );
					}
					GUI.Box( new Rect( r.x - 6, r.y - 6, r.width + 12, r.height + 12 ), "", theSkin.GetStyle( "MapBackground" ) );
					GUI.color = Color.white;
					GUI.DrawTexture( r, packComplete ? packedFontMap : fontMap );



					GUI.EndGroup();
				} else {

					GUI.BeginGroup( screenAreas[ScreenArea.Graph] );
					GUI.color = BGCol;
					r = CalculateNodeRect( new Rect( 0, 0, fontMap.width, fontMap.height ) );
					GUI.Box( new Rect( r.x - 6, r.y - 6, r.width + 12, r.height + 12 ), "", theSkin.GetStyle( "MapBackground" ) );
					GUI.color = Color.white;

					GUI.DrawTexture( r, fontMap );

					for (int i = 0; i < nodes.Count; i++) {
						r = CalculateNodeRect( nodes[i].rect );
						GUI.Box( r, nodes[i].ch + "", theSkin.GetStyle( "Rect" ) );
						GUI.Label( r, nodes[i].ch + "", theSkin.GetStyle( "RectLabel" ) );
						if (editPivots) {
							GUI.color = new Color( 1, 1, 1, 0.5f );
							GUI.Box( new Rect( r.x - 7 + nodes[i].pivot.x * zoomCurrent, r.y - 7 + (nodes[i].rect.height - nodes[i].pivot.y) * zoomCurrent, 16, 16 ), "", EditorGUIUtility.GetBuiltinSkin( EditorSkin.Inspector ).GetStyle( "U2D.pivotDot" ) );
							GUI.color = Color.white;
						}
						if (i == highlightedNodeIndex) {
							if (autoSetDialog) {
								GUI.Box( r, "", theSkin.GetStyle( "AutoSet" ) );
							}

							cornerHandles[0] = new Rect( r.x - 4, r.y - 4, 10, 10 );
							cornerHandles[1] = new Rect( r.x + r.width - 6, r.y - 4, 10, 10 );
							cornerHandles[2] = new Rect( r.x + r.width - 6, r.y + r.height - 6, 10, 10 );
							cornerHandles[3] = new Rect( r.x - 4, r.y + r.height - 6, 10, 10 );
							cornerHandles[4] = new Rect( r.x - 7 + nodes[i].pivot.x * zoomCurrent, r.y - 7 + (nodes[i].rect.height - nodes[i].pivot.y) * zoomCurrent, 16, 16 );

							if (editPivots) {
								GUI.Box( cornerHandles[4], "", EditorGUIUtility.GetBuiltinSkin( EditorSkin.Inspector ).GetStyle( "U2D.pivotDotActive" ) );
								EditorGUIUtility.AddCursorRect( cornerHandles[4], MouseCursor.MoveArrow );
							} else {
								GUI.color = new Color( 1, 1, 1, 0.5f );
								GUI.Box( cornerHandles[4], "", EditorGUIUtility.GetBuiltinSkin( EditorSkin.Inspector ).GetStyle( "U2D.pivotDot" ) );
								GUI.color = Color.white;
								GUI.Box( cornerHandles[0], "", EditorGUIUtility.GetBuiltinSkin( EditorSkin.Inspector ).GetStyle( "U2D.dragDot" ) );
								GUI.Box( cornerHandles[1], "", EditorGUIUtility.GetBuiltinSkin( EditorSkin.Inspector ).GetStyle( "U2D.dragDot" ) );
								GUI.Box( cornerHandles[2], "", EditorGUIUtility.GetBuiltinSkin( EditorSkin.Inspector ).GetStyle( "U2D.dragDot" ) );
								GUI.Box( cornerHandles[3], "", EditorGUIUtility.GetBuiltinSkin( EditorSkin.Inspector ).GetStyle( "U2D.dragDot" ) );

								for (int j = 0; j < 4; j++) {
									EditorGUIUtility.AddCursorRect( cornerHandles[j], j % 2 == 0 ? MouseCursor.ResizeUpLeft : MouseCursor.ResizeUpRight );
								}
							}


						}
					}


					r = screenAreas[ScreenArea.Graph];
					if (nodesDupe) {
						GUI.Box( new Rect( r.width - 126, r.height - 56, 126, 56 ), "There is more than one node for '" + nodes[nodesDupeA].ch + "':" );
						if (GUI.Button( new Rect( r.width - 124, r.height - 22, 60, 20 ), "Node 1" )) {
							highlightedNodeIndex = nodesDupeA;
							ViewFocus();
						}
						if (GUI.Button( new Rect( r.width - 62, r.height - 22, 60, 20 ), "Node 2" )) {
							highlightedNodeIndex = nodesDupeB;
							ViewFocus();
						}
					}
					if (nodesOverlap) {
						GUI.Box( new Rect( r.width - (nodesDupe ? 215 : 90), r.height - 56, 90, 56 ), "These nodes overlap:" );
						if (GUI.Button( new Rect( r.width - (nodesDupe ? 213 : 88), r.height - 22, 42, 20 ), "'" + nodes[nodesOverlapA].ch + "'" )) {
							highlightedNodeIndex = nodesOverlapA;
							ViewFocus();
						}
						if (GUI.Button( new Rect( r.width - (nodesDupe ? 169 : 44), r.height - 22, 42, 20 ), "'" + nodes[nodesOverlapB].ch + "'" )) {
							highlightedNodeIndex = nodesOverlapB;
							ViewFocus();
						}
					}

					GUI.EndGroup();
				}
			}

			EditorGUILayout.EndVertical();

			#endregion

			GUILayout.Box( "", theSkin.GetStyle( "Divider" ), GUILayout.Width( dividerSize ), GUILayout.ExpandHeight( true ) ); //divider
			if (Event.current.type == EventType.Repaint)
				screenAreas[ScreenArea.GraphDivider] = GUILayoutUtility.GetLastRect();
			EditorGUIUtility.AddCursorRect( screenAreas[ScreenArea.GraphDivider], MouseCursor.ResizeHorizontal );

			#region Side Bar (Top)

			EditorGUILayout.BeginVertical( GUILayout.Width( sidebarWidth - dividerSize - 11 ) );

			nodeScroll = GUILayout.BeginScrollView( nodeScroll, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Height( nodeHeight ) );

			if (fontMap == null || font == null) {
			} else {
				if (autoSetDialog) {
					GUILayout.Label( "What character is this?" );

					if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Return && GUI.GetNameOfFocusedControl() == "AutoInput" && nodes[highlightedNodeIndex].ch != "") {
						if (nodes.Count == 1)
							repeatList.Clear();
						repeatList.Add( nodes[nodes.Count - 1].ch[0] );
						if (autoNodeList.Count == 0) {
							EndAutoSet();
							GUIUtility.ExitGUI();
						} else {
							AddNode();
						}
					}
					if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Escape && GUI.GetNameOfFocusedControl() == "AutoInput") {
						if (nodes.Count == 1)
							repeatList.Clear();
						repeatList.Add( (char) 0 );
						RemoveNode();
						if (autoNodeList.Count == 0) {
							EndAutoSet();
							GUIUtility.ExitGUI();
						} else {
							GUI.FocusControl( "" );
							AddNode();
						}
					}

					if (nodes[highlightedNodeIndex].ch.Length > 1) {
						nodes[highlightedNodeIndex].ch = nodes[highlightedNodeIndex].ch.Substring( 0, 1 );
						EditorGUI.FocusTextInControl( "" );
					}
					GUI.SetNextControlName( "AutoInput" );
					nodes[highlightedNodeIndex].ch = EditorGUILayout.TextField( "Character:", nodes[highlightedNodeIndex].ch );

					GUILayout.BeginHorizontal();

					if (GUILayout.Button( "Next" ) && nodes[highlightedNodeIndex].ch != "") {
						GUI.FocusControl( "" );
						refocus = true;
						if (nodes.Count == 1)
							repeatList.Clear();
						repeatList.Add( nodes[nodes.Count - 1].ch[0] );
						if (autoNodeList.Count == 0) {
							EndAutoSet();
							GUIUtility.ExitGUI();
						} else {
							AddNode();
						}
					}
					if (GUILayout.Button( "Skip" )) {
						if (nodes.Count == 1)
							repeatList.Clear();
						repeatList.Add( (char) 0 );
						RemoveNode();
						if (autoNodeList.Count == 0) {
							EndAutoSet();
							GUIUtility.ExitGUI();
						} else {
							AddNode();
						}
					}
					GUILayout.EndHorizontal();
					if (GUILayout.Button( "Stop" )) {
						repeatList.Add( (char) 1 );
						RemoveNode();
						EndAutoSet();
						GUIUtility.ExitGUI();
					}

					if (repeatList.Count > 1 && nodes.Count == 1 && GUILayout.Button( "Repeat Previous" )) {
						for (int i = 0; i < repeatList.Count; i++) {
							if (repeatList[i] == 0) {
								nodes.RemoveAt( nodes.Count - 1 );
							} else if (repeatList[i] == 1) {
								nodes.RemoveAt( nodes.Count - 1 );
								break;
							} else {
								nodes[highlightedNodeIndex].ch = "" + repeatList[i];
							}
							if (i != repeatList.Count - 1)
								AddNode();
						}

						EndAutoSet();
						GUIUtility.ExitGUI();
					}


					EditorGUI.FocusTextInControl( "AutoInput" );


				} else if (packMode) {
					if (GUILayout.Button( "Pack", GUILayout.Height( 25 ) )) {
						BeginPack();
					}

					EditorGUI.BeginChangeCheck();
					packedFont = (Font) EditorGUILayout.ObjectField( "Packed Font", packedFont, typeof( Font ), false );
					if (EditorGUI.EndChangeCheck() && packedFont != null) {

						if (packedFont.material == null) {
							errorString = "This font has no material";
							showError = true;
							packedFont = null;
							EditorGUIUtility.ExitGUI();
						}
						if (packedFont.material.mainTexture == null) {
							errorString = "This font has a material with no 'MainTexture' font map";
							showError = true;
							packedFont = null;
							EditorGUIUtility.ExitGUI();
						}
						packedfontMapPath = AssetDatabase.GetAssetPath( packedFont.material.mainTexture );
					}
					if (packedFont == null) {

						GUI.enabled = false;
						GUILayout.Label( "(Leave blank to create\nautomatically on packing" );
						GUI.enabled = true;
					}

					GUILayout.Space( 10 );

					packMethod = EditorGUILayout.Popup( "Pack method", packMethod, packMethods );
					packSort = EditorGUILayout.Popup( "Sort Method", packSort, packSorts );

					packOptionNPOT = GUILayout.Toggle( packOptionNPOT, "Allow NPOT result" );
					packOptionRotate = GUILayout.Toggle( packOptionRotate, "Allow rotation test" );
					packOptionSort = GUILayout.Toggle( packOptionSort, "Sort anchors" );
					packOptionFit = GUILayout.Toggle( packOptionFit, "Fit to POT Texture" );

					packBuffer = Mathf.Max( EditorGUILayout.IntField( "Glyph buffer size", packBuffer ), 0 );

					GUILayout.Space( 10 );

					if (GUILayout.Button( "Return to font" )) {
						packMode = false;
						packComplete = false;
					}
					GUI.enabled = packComplete;
					if (GUILayout.Button( "Accept Pack Result", GUILayout.Height( 25 ) )) {
						packComplete = false;
						packMode = false;
						font = packedFont;
						fontMap = packedFontMap;
						LoadFont();
						EditorGUIUtility.ExitGUI();
					}
					GUI.enabled = true;

				} else {
					GUILayout.BeginHorizontal();
					if (GUILayout.Button( "+1" )) {
						if (nodes.Count == 0 || highlightedNodeIndex == -1)
							nodes.Add( new Node() );
						else
							nodes.Add( new Node( nodes[highlightedNodeIndex] ) );
						SerializedObject SO = new SerializedObject( font );
						SerializedProperty p = SO.FindProperty( "m_CharacterRects.Array" );
						p.InsertArrayElementAtIndex( p.arraySize );
						SO.ApplyModifiedProperties();
						highlightedNodeIndex = nodes.Count - 1;
						UpdateFont( highlightedNodeIndex );

					}
					GUI.enabled = highlightedNodeIndex != -1;
					if (e.type == EventType.KeyDown && e.keyCode == KeyCode.Delete && highlightedNodeIndex != -1) {
						nodes.RemoveAt( highlightedNodeIndex );
						SerializedObject SO = new SerializedObject( font );
						SerializedProperty p = SO.FindProperty( "m_CharacterRects.Array" );
						p.DeleteArrayElementAtIndex( highlightedNodeIndex );
						SO.ApplyModifiedProperties();
						highlightedNodeIndex -= 1;
						CheckDupeChars();
						CheckOverlapNodes();
						GUI.FocusControl( "" );
						this.Repaint();
					}

					if (GUILayout.Button( "Remove" )) {
						nodes.RemoveAt( highlightedNodeIndex );
						SerializedObject SO = new SerializedObject( font );
						SerializedProperty p = SO.FindProperty( "m_CharacterRects.Array" );
						p.DeleteArrayElementAtIndex( highlightedNodeIndex );
						SO.ApplyModifiedProperties();
						highlightedNodeIndex -= 1;
						CheckDupeChars();
						CheckOverlapNodes();
						GUI.FocusControl( "" );

					}
					GUI.enabled = true;

					GUILayout.EndHorizontal();
					GUILayout.Space( 10 );


					if (highlightedNodeIndex != -1) {



						EditorGUI.BeginChangeCheck();
						EditorGUIUtility.labelWidth = 80f;
						EditorGUIUtility.fieldWidth = 20f;
						nodes[highlightedNodeIndex].ch = EditorGUILayout.TextField( "Character:", nodes[highlightedNodeIndex].ch );

						if (nodes[highlightedNodeIndex].ch.Length > 1) {
							nodes[highlightedNodeIndex].ch = nodes[highlightedNodeIndex].ch.Substring( 0, 1 );
							GUI.FocusControl( "" );
						}

						EditorGUIUtility.labelWidth = 30f;
						GUILayout.BeginHorizontal();
						nodes[highlightedNodeIndex].rect.x = (snapToPixels) ? EditorGUILayout.IntField( "X:", (int) nodes[highlightedNodeIndex].rect.x ) : EditorGUILayout.FloatField( "X:", nodes[highlightedNodeIndex].rect.x );
						nodes[highlightedNodeIndex].rect.y = (snapToPixels) ? EditorGUILayout.IntField( "Y:", (int) nodes[highlightedNodeIndex].rect.y ) : EditorGUILayout.FloatField( "Y:", nodes[highlightedNodeIndex].rect.y );
						GUILayout.EndHorizontal();
						GUILayout.BeginHorizontal();
						nodes[highlightedNodeIndex].rect.width = Mathf.Max( (snapToPixels) ? EditorGUILayout.IntField( "W:", (int) nodes[highlightedNodeIndex].rect.width ) : EditorGUILayout.FloatField( "W:", nodes[highlightedNodeIndex].rect.width ), 1 );
						nodes[highlightedNodeIndex].rect.height = Mathf.Max( (snapToPixels) ? EditorGUILayout.IntField( "H:", (int) nodes[highlightedNodeIndex].rect.height ) : EditorGUILayout.FloatField( "H:", nodes[highlightedNodeIndex].rect.height ), 1 );
						GUILayout.EndHorizontal();

						GUILayout.Space( 10 );

						nodes[highlightedNodeIndex].pivot = EditorGUILayout.Vector2Field( "Glyph Pivot", nodes[highlightedNodeIndex].pivot );
						r = GUILayoutUtility.GetLastRect();
						r.x += 75;
						editPivots = GUI.Toggle( r, editPivots, " (enable edit)" );

						EditorGUIUtility.labelWidth = 0;

						onekeyPivot =  GUILayout.Button("oneKey Pivot");
						if(onekeyPivot)
						{
							for(int ii =1;ii<nodes.Count;ii++){
								nodes[ii].pivot = nodes[0].pivot;
								UpdateFont(ii);
							}
						}

						GUILayout.Space( 10 );
						nodes[highlightedNodeIndex].orient = EditorGUILayout.Toggle( "Rotated Gylph", nodes[highlightedNodeIndex].orient );


						GUILayout.Space( 10 );
						nodes[highlightedNodeIndex].overrideAdvance = EditorGUILayout.Toggle( "Override Glyph Width", nodes[highlightedNodeIndex].overrideAdvance );
						if (nodes[highlightedNodeIndex].overrideAdvance) {
							EditorGUI.indentLevel = 1;
							nodes[highlightedNodeIndex].advance = (snapToPixels) ? EditorGUILayout.IntField( "Advance width:", (int) nodes[highlightedNodeIndex].advance ) : EditorGUILayout.FloatField( "Advance width:", nodes[highlightedNodeIndex].advance );
							EditorGUI.indentLevel = 0;
						}

						GUILayout.Space( 10 );
						nodes[highlightedNodeIndex].overrideScale = EditorGUILayout.Toggle( "Override Glyph Size", nodes[highlightedNodeIndex].overrideScale );
						if (nodes[highlightedNodeIndex].overrideScale) {
							EditorGUI.indentLevel = 1;
							nodes[highlightedNodeIndex].scale = EditorGUILayout.Vector2Field( "Glyph Scaler:", nodes[highlightedNodeIndex].scale );
							EditorGUI.indentLevel = 0;
						}

						if (EditorGUI.EndChangeCheck()) {
							UpdateFont( highlightedNodeIndex );
						}
					}
				}
			}
			GUILayout.EndScrollView();
			if (Event.current.type == EventType.Repaint)
				screenAreas[ScreenArea.Nodes] = GUILayoutUtility.GetLastRect();

			#endregion

			#region Side Bar (Bottom)
			if (showOptions) {
				GUILayout.Box( "", EditorGUIUtility.GetBuiltinSkin( EditorSkin.Inspector ).GetStyle( "RL DragHandle" ), GUILayout.Height( dividerSize ), GUILayout.ExpandWidth( true ) ); //divider

				if (Event.current.type == EventType.Repaint)
					screenAreas[ScreenArea.NodesDivider] = GUILayoutUtility.GetLastRect();
				EditorGUIUtility.AddCursorRect( screenAreas[ScreenArea.NodesDivider], MouseCursor.ResizeVertical );
				if (GUILayout.Button( "Hide Options" )) {
					showOptions = false;
					nodeHeight = position.height - 22;
				}

				optionScroll = GUILayout.BeginScrollView( optionScroll );


				snapMode = EditorGUILayout.Popup( "Snap:", snapMode, snapModes );
				snapToPixels = EditorGUILayout.Toggle( "Snap to pixels", snapToPixels );
				moveAllPivots = EditorGUILayout.Toggle( "Move all pivots", moveAllPivots );
				moveAllNodes = EditorGUILayout.Toggle( "Move all nodes", moveAllNodes );
				gradientColor = EditorGUILayout.ColorField( "Gradient Color", gradientColor );
				BGCol = EditorGUILayout.ColorField( "Background Color", BGCol );
				smartColor = EditorGUILayout.ColorField( "Smart Color", smartColor );
				defaultShader = (Shader) EditorGUILayout.ObjectField( "Default Font Shader", defaultShader, typeof( Shader ), false );
				GUILayout.EndScrollView();
			} else {
				if (GUILayout.Button( "Options" )) {
					showOptions = true;
					nodeHeight = position.height - 180;
				}
			}
			#endregion
			EditorGUILayout.EndVertical();

			EditorGUILayout.EndHorizontal();

			//////Draw regions////
			//GUI.color = new Color( 1, 0, 0, 0.2f );
			//foreach (KeyValuePair<ScreenArea, Rect> kvp in screenAreas) {
			//	GUI.Box( kvp.Value, kvp.Key + ": " + kvp.Value );
			//}



			#region Mouse Events
			if (e.type == EventType.MouseDown) {
				clickedArea = ScreenArea.None;
				clickCornerIndex = -1;
				foreach (KeyValuePair<ScreenArea, Rect> kvp in screenAreas) {
					if (kvp.Value.Contains( e.mousePosition )) {
						clickedArea = kvp.Key;
						break;
					}
				}
				if (clickedArea == ScreenArea.Graph && !autoSetDialog && !packMode && fontMap != null && e.button == 0) {
					for (int i = editPivots ? 4 : 3; i > -1; i--) {
						if (cornerHandles[i].Contains( e.mousePosition - new Vector2( screenAreas[ScreenArea.Graph].x, screenAreas[ScreenArea.Graph].y ) )) {
							clickCornerIndex = i;
							clickNodeIndex = highlightedNodeIndex;
							break;
						}
					}
					if (clickCornerIndex == -1) {
						clickNodeIndex = -1;
						Vector2 mousePos = CalcuateGraphMousePos( e.mousePosition );
						for (int i = 0; i < nodes.Count; i++) {
							if (nodes[i].rect.Contains( mousePos )) {
								clickNodeIndex = i;
								highlightedNodeIndex = i;
								clickOffset = new Vector2( mousePos.x - nodes[i].rect.x, mousePos.y - nodes[i].rect.y );
								if (snapToPixels) {
									clickOffset.x = Mathf.Round( clickOffset.x );
									clickOffset.y = Mathf.Round( clickOffset.y );
								}
								GUI.FocusControl( "" );
								this.Repaint();
								break;
							}
						}
					}

				}
			}

			if (e.type == EventType.MouseDrag) {
				switch (clickedArea) {
					case ScreenArea.GraphDivider:
						sidebarWidth = Mathf.Clamp( sidebarWidth - e.delta.x, 240, position.width - 165 );
						this.Repaint();
						break;
					case ScreenArea.NodesDivider:
						nodeHeight = Mathf.Clamp( nodeHeight + e.delta.y, 50, position.height - 65 );
						this.Repaint();
						break;
					case ScreenArea.Graph:
						if (fontMap == null)
							break;
						Vector2 mousePos = CalcuateGraphMousePos( e.mousePosition );
						if (snapToPixels) {
							mousePos.x = Mathf.Round( mousePos.x );
							mousePos.y = Mathf.Round( mousePos.y );
						}
						if (clickNodeIndex == -1) {
							graphOffsetXtarget = (int) Mathf.Clamp( graphOffsetXtarget + e.delta.x, screenAreas[ScreenArea.Graph].width / 2 - (packComplete ? packedFontMap.width : fontMap.width) * zoomTarget, screenAreas[ScreenArea.Graph].width / 2 );
							graphOffsetYtarget = (int) Mathf.Clamp( graphOffsetYtarget + e.delta.y, screenAreas[ScreenArea.Graph].height / 2 - (packComplete ? packedFontMap.height : fontMap.height) * zoomTarget, screenAreas[ScreenArea.Graph].height / 2 );
						} else if (!autoSetDialog) {
							if (clickCornerIndex == -1) {
								if (moveAllNodes)
									oldRect = nodes[clickNodeIndex].rect;
								if (snapMode != 3)
									nodes[clickNodeIndex].rect.x = mousePos.x - clickOffset.x;
								if (snapMode != 2)
									nodes[clickNodeIndex].rect.y = mousePos.y - clickOffset.y;
								if (snapMode == 1)
									nodes[clickNodeIndex].rect = SnapToPos( nodes[clickNodeIndex].rect, clickNodeIndex );
								if (snapToPixels) {
									nodes[clickNodeIndex].rect.x = Mathf.Round( nodes[clickNodeIndex].rect.x );
									nodes[clickNodeIndex].rect.y = Mathf.Round( nodes[clickNodeIndex].rect.y );
									nodes[clickNodeIndex].rect.width = Mathf.Round( nodes[clickNodeIndex].rect.width );
									nodes[clickNodeIndex].rect.height = Mathf.Round( nodes[clickNodeIndex].rect.height );
								}
								if (moveAllNodes) {
									pivotOffset = new Vector2( nodes[clickNodeIndex].rect.x - oldRect.x, nodes[clickNodeIndex].rect.y - oldRect.y );
									for (int i = 0; i < nodes.Count; i++) {
										if (i == clickNodeIndex)
											continue;
										nodes[i].rect.x += pivotOffset.x;
										nodes[i].rect.y += pivotOffset.y;
										UpdateFont( i, false );
									}
								}
							} else {
								if (!snapFound) {
									if (nodes[clickNodeIndex].orient) {
										pivotOffset.x = nodes[clickNodeIndex].rect.width - nodes[clickNodeIndex].pivot.x;
										pivotOffset.y = nodes[clickNodeIndex].rect.height - nodes[clickNodeIndex].pivot.y;
									} else {
										pivotOffset.y = nodes[clickNodeIndex].rect.height - nodes[clickNodeIndex].pivot.y;
									}
									switch (clickCornerIndex) {
										case 0: // top left
											if (snapMode != 3)
												nodes[clickNodeIndex].rect.xMin = Mathf.Min( mousePos.x, nodes[clickNodeIndex].rect.xMax - 1 );
											if (snapMode != 2)
												nodes[clickNodeIndex].rect.yMax = Mathf.Max( mousePos.y, nodes[clickNodeIndex].rect.y + 1 );
											break;
										case 1: // top right
											if (snapMode != 3)
												nodes[clickNodeIndex].rect.xMax = Mathf.Max( mousePos.x, nodes[clickNodeIndex].rect.x + 1 );
											if (snapMode != 2)
												nodes[clickNodeIndex].rect.yMax = Mathf.Max( mousePos.y, nodes[clickNodeIndex].rect.y + 1 );
											break;
										case 2: // bot right
											if (snapMode != 3)
												nodes[clickNodeIndex].rect.xMax = Mathf.Max( mousePos.x, nodes[clickNodeIndex].rect.x + 1 );
											if (snapMode != 2)
												nodes[clickNodeIndex].rect.yMin = Mathf.Min( mousePos.y, nodes[clickNodeIndex].rect.yMax - 1 );
											break;
										case 3: // bot left
											if (snapMode != 3)
												nodes[clickNodeIndex].rect.xMin = Mathf.Min( mousePos.x, nodes[clickNodeIndex].rect.xMax - 1 );
											if (snapMode != 2)
												nodes[clickNodeIndex].rect.yMin = Mathf.Min( mousePos.y, nodes[clickNodeIndex].rect.yMax - 1 );
											break;
										case 4: // Pivot Point
											oldPivot = nodes[clickNodeIndex].pivot;
											if (snapMode != 3)
												nodes[clickNodeIndex].pivot.x = mousePos.x - nodes[clickNodeIndex].rect.x;
											if (snapMode != 2)
												nodes[clickNodeIndex].pivot.y = mousePos.y - nodes[clickNodeIndex].rect.y;
											if (snapMode == 1)
												nodes[clickNodeIndex].pivot = SnapPivot( nodes[clickNodeIndex].pivot, nodes[clickNodeIndex].rect, clickNodeIndex );
											if (moveAllPivots) {
												pivotOffset = nodes[clickNodeIndex].pivot - oldPivot;
												if (nodes[clickNodeIndex].orient) {
													pivotOffset = new Vector2( -pivotOffset.y, pivotOffset.x );
												}
												for (int i = 0; i < nodes.Count; i++) {
													if (i == clickNodeIndex)
														continue;
													if (nodes[i].orient) {
														nodes[i].pivot += new Vector2( pivotOffset.y, -pivotOffset.x );
													} else {
														nodes[i].pivot += pivotOffset;
													}
													UpdateFont( i, false );
												}
											}
											break;
									}
									if (clickCornerIndex != 4) {
										if (nodes[clickNodeIndex].orient) {
											nodes[clickNodeIndex].pivot.x = nodes[clickNodeIndex].rect.width - pivotOffset.x;
											nodes[clickNodeIndex].pivot.y = nodes[clickNodeIndex].rect.height - pivotOffset.y;
										} else {
											nodes[clickNodeIndex].pivot.y = nodes[clickNodeIndex].rect.height - pivotOffset.y;
										}
									}
									if (snapMode == 1 && clickCornerIndex != 4)
										nodes[clickNodeIndex].rect = SnapToSize( nodes[clickNodeIndex].rect, clickNodeIndex );
								} else if (!snapRect.Contains( e.mousePosition )) {
									snapFound = false;
								}
							}
							UpdateFont( clickNodeIndex );
						}
						this.Repaint();
						break;
				}
			}
			if (e.type == EventType.MouseUp) {
				clickedArea = ScreenArea.None;
				clickNodeIndex = -1;
			}
			if (e.type == EventType.ScrollWheel) {
				float oldZoom = zoomTarget;
				zoomTarget = (Mathf.Clamp( zoomTarget * (e.delta.y < 0 ? 2 : 0.5f), 0.25f, 4 ));
				graphOffsetXtarget = Mathf.Round( ((graphOffsetXtarget - screenAreas[ScreenArea.Graph].width / 2) / oldZoom) * zoomTarget + screenAreas[ScreenArea.Graph].width / 2 );
				graphOffsetYtarget = Mathf.Round( ((graphOffsetYtarget - screenAreas[ScreenArea.Graph].height / 2) / oldZoom) * zoomTarget + screenAreas[ScreenArea.Graph].height / 2 );
			}

			#endregion

			#region Lerp camera values

			if (Mathf.Abs( graphOffsetXcurrent - graphOffsetXtarget ) != 0) {
				if (Mathf.Abs( graphOffsetXcurrent - graphOffsetXtarget ) < 0.01f) {
					graphOffsetXcurrent = graphOffsetXtarget;
				} else {
					graphOffsetXcurrent = Mathf.Lerp( graphOffsetXcurrent, graphOffsetXtarget, 0.02f );
				}
				this.Repaint();
			}
			if (Mathf.Abs( graphOffsetYcurrent - graphOffsetYtarget ) != 0) {
				if (Mathf.Abs( graphOffsetYcurrent - graphOffsetYtarget ) < 0.01f) {
					graphOffsetYcurrent = graphOffsetYtarget;
				} else {
					graphOffsetYcurrent = Mathf.Lerp( graphOffsetYcurrent, graphOffsetYtarget, 0.02f );
				}
				this.Repaint();
			}
			if (Mathf.Abs( zoomCurrent - zoomTarget ) != 0) {
				if (Mathf.Abs( zoomCurrent - zoomTarget ) < 0.001f) {
					zoomCurrent = zoomTarget;
				} else {
					zoomCurrent = Mathf.Lerp( zoomCurrent, zoomTarget, 0.02f );
				}
				this.Repaint();
			}
			#endregion



			if (showError) {
				if (EditorUtility.DisplayDialog( "Error", errorString, "OK" )) {
					showError = false;
				}
			}


			if (position.height != windowPosHeight) {
				windowPosHeight = position.height;
				nodeHeight = showOptions ? position.height - 180 : position.height - 22;
			}

		}


		#region Helper Functions

		Vector2 CalcuateGraphMousePos ( Vector2 v ) {
			return new Vector2( (v.x - screenAreas[ScreenArea.Graph].x - graphOffsetXcurrent) / zoomCurrent, fontMap.height - (v.y - screenAreas[ScreenArea.Graph].y - graphOffsetYcurrent) / zoomCurrent );
		}


		Rect CalculateNodeRect ( Rect r ) {
			return new Rect( r.x * zoomCurrent + graphOffsetXcurrent, (-r.y + fontMap.height - r.height) * zoomCurrent + graphOffsetYcurrent, r.width * zoomCurrent, r.height * zoomCurrent );
		}
		Rect CalculatePackRect ( Rect r ) {
			return new Rect( r.x * zoomCurrent + graphOffsetXcurrent, (-r.y + packedFontMap.height - r.height) * zoomCurrent + graphOffsetYcurrent, r.width * zoomCurrent, r.height * zoomCurrent );
		}

		//Snaps Pivots to node corners
		Vector2 SnapPivot ( Vector2 v, Rect inSpace, int index ) {
			Vector2 v2 = new Vector2( v.x + inSpace.x, v.y + inSpace.y );

			//First snap to local node
			Vector2[] SnapPoints = new Vector2[] {
				new Vector2( inSpace.x, inSpace.y ), //top left
				new Vector2( inSpace.x + inSpace.width/2, inSpace.y ), //top center
				new Vector2( inSpace.xMax, inSpace.y ), //top right

				new Vector2( inSpace.x, inSpace.y + inSpace.height/2 ), //middle left
				new Vector2( inSpace.x + inSpace.width/2,  inSpace.y + inSpace.height/2 ), //middle center
				new Vector2( inSpace.xMax,  inSpace.y + inSpace.height/2 ), //middle right

				new Vector2( inSpace.x, inSpace.yMax ), //bottom left
				new Vector2( inSpace.x + inSpace.width/2, inSpace.yMax ), //bottom center
				new Vector2( inSpace.xMax, inSpace.yMax ) //bottom right
			};

			for (int i = 0; i < SnapPoints.Length; i++) {
				if (Vector2.Distance( v2, SnapPoints[i] ) < 10.0f / zoomCurrent) {
					snapFound = true;
					snapRect = new Rect( Event.current.mousePosition.x - 7, Event.current.mousePosition.y - 7, 14, 14 );
					return new Vector2( SnapPoints[i].x - inSpace.x, SnapPoints[i].y - inSpace.y );
				}
			}

			//if failed, seee if snapping to other nodes
			for (int i = 0; i < nodes.Count; i++) {
				if (i == index)
					continue;
				if (nodes[i].rect.Contains( v2 ) && nodes[index].orient == nodes[i].orient) {
					snapFound = true;
					snapRect = new Rect( Event.current.mousePosition.x - 7, Event.current.mousePosition.y - 7, 14, 14 );
					if (nodes[i].orient) {
						return new Vector2( nodes[i].rect.x - inSpace.x + nodes[i].pivot.x, nodes[index].rect.height - (nodes[i].rect.height - nodes[i].pivot.y) );
					} else {
						return new Vector2( nodes[i].pivot.x, nodes[i].rect.y - inSpace.y + nodes[i].pivot.y );
					}

				}

			}

			return v;
		}


		//Snaps 2 nodes togther by their corners, for moving nodes
		Rect SnapToPos ( Rect r, int ind ) {
			Vector2[] rCorner = new Vector2[] { new Vector2( r.x, r.y ), new Vector2( r.xMax, r.y ), new Vector2( r.x, r.yMax ), new Vector2( r.xMax, r.yMax ) };
			for (int j = 0; j < nodes.Count; j++) {
				if (j == ind) { continue; }
				Vector2[] RCorner = new Vector2[] { new Vector2( nodes[j].rect.x, nodes[j].rect.y ), new Vector2( nodes[j].rect.xMax, nodes[j].rect.y ), new Vector2( nodes[j].rect.x, nodes[j].rect.yMax ), new Vector2( nodes[j].rect.xMax, nodes[j].rect.yMax ) };
				foreach (Vector2 C in RCorner) {
					for (int i = 0; i < 4; i++) {
						if (Vector2.Distance( rCorner[i], C ) < 10.0f / zoomCurrent) {
							Rect result = new Rect();
							switch (i) {
								case 0:
									result = new Rect( C.x, C.y, r.width, r.height );
									break;
								case 1:
									result = new Rect( C.x - r.width, C.y, r.width, r.height );
									break;
								case 2:
									result = new Rect( C.x, C.y - r.height, r.width, r.height );
									break;
								case 3:
									result = new Rect( C.x - r.width, C.y - r.height, r.width, r.height );
									break;
							}
							return result;
						}
					}
				}
			}
			return r;
		}


		//Snaps 2 nodes togther by their corners, for resizing nodes
		Rect SnapToSize ( Rect r, int ind ) {
			Vector2[] rCorner = new Vector2[] { new Vector2( r.x, r.yMax ), new Vector2( r.xMax, r.yMax ), new Vector2( r.xMax, r.y ), new Vector2( r.x, r.y ) };
			for (int j = 0; j < nodes.Count; j++) {
				if (j == ind)
					continue;
				Vector2[] RCorner = new Vector2[] { new Vector2( nodes[j].rect.x, nodes[j].rect.y ), new Vector2( nodes[j].rect.xMax, nodes[j].rect.y ), new Vector2( nodes[j].rect.x, nodes[j].rect.yMax ), new Vector2( nodes[j].rect.xMax, nodes[j].rect.yMax ) };
				foreach (Vector2 C in RCorner) {
					if (Vector2.Distance( rCorner[clickCornerIndex], C ) < 10.0f / zoomCurrent) {
						Rect result = new Rect();
						switch (clickCornerIndex) {
							case 0:
								result = new Rect( C.x, r.y, Mathf.Abs( r.xMax - C.x ), Mathf.Abs( r.y - C.y ) );
								break;
							case 1:
								result = new Rect( r.x, r.y, Mathf.Abs( r.x - C.x ), Mathf.Abs( r.y - C.y ) );
								break;
							case 2:
								result = new Rect( r.x, C.y, Mathf.Abs( r.x - C.x ), Mathf.Abs( C.y - r.yMax ) );
								break;
							case 3:
								result = new Rect( C.x, C.y, Mathf.Abs( r.xMax - C.x ), Mathf.Abs( r.yMax - C.y ) );
								break;
						}
						result.width = Mathf.Max( result.width, 1 );
						result.height = Mathf.Max( result.height, 1 );
						snapFound = true;
						snapRect = new Rect( Event.current.mousePosition.x - 7, Event.current.mousePosition.y - 7, 14, 14 );
						return result;
					}
				}
			}
			return r;
		}

		//Checks if image is NPOT, then checks if NPOT import is set to none
		bool CheckNPOT ( Texture2D T ) {
			bool result = false;
			if (T != null) {
				string assetPath = AssetDatabase.GetAssetPath( T );
				TextureImporter importer = AssetImporter.GetAtPath( assetPath ) as TextureImporter;

				if (importer != null) {
					object[] args = new object[2] { 0, 0 };
					System.Reflection.MethodInfo M = typeof( TextureImporter ).GetMethod( "GetWidthAndHeight", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance );
					M.Invoke( importer, args );
					int w = (int) args[0];
					int h = (int) args[1];
					result = ((w & (w - 1)) == 0 && (h & (h - 1)) == 0);
					if (!result) {
						if (importer.npotScale == TextureImporterNPOTScale.None) { result = true; }
					}
				}
			}
			return result;
		}

		//Compares image size to importer max size
		bool CheckImportSize ( Texture2D T ) {
			if (T != null) {
				string assetPath = AssetDatabase.GetAssetPath( T );
				TextureImporter importer = AssetImporter.GetAtPath( assetPath ) as TextureImporter;

				if (importer != null) {
					object[] args = new object[2] { 0, 0 };
					System.Reflection.MethodInfo M = typeof( TextureImporter ).GetMethod( "GetWidthAndHeight", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance );
					M.Invoke( importer, args );
					int w = (int) args[0];
					int h = (int) args[1];

					if (importer.maxTextureSize < w || importer.maxTextureSize < h) {
						return true;
					} else {
						return false;
					}
				}
			}
			return true;
		}

		//Checks the tex format (best to use compressed or truecolor)
		bool CheckImportFormat ( Texture2D t ) {
			if (t.format == TextureFormat.ARGB32 ||
				t.format == TextureFormat.RGBA32 ||
				t.format == TextureFormat.BGRA32 ||
				t.format == TextureFormat.RGB24 ||
				t.format == TextureFormat.Alpha8 ||
				t.format == TextureFormat.DXT1 ||
				t.format == TextureFormat.DXT5) {
				return true;
			}
			return false;
		}

		//Checks texture for Read/Write enabled
		bool CheckReadWrite ( Texture2D t ) {
			SerializedObject SO = new SerializedObject( t );
			SerializedProperty p = SO.FindProperty( "m_IsReadable" );
			if (!p.boolValue) {
				errorString = "Font texture must have read/write enabled to do that. Change the texture import settings";
				showError = true;
			}
			return p.boolValue;

		}

		void PopulateNodesFromFont () {
			if (font == null)
				return;
			CharacterInfo[] CI = font.characterInfo;
			nodes = new List<Node>();
			for (int i = 0; i < CI.Length; i++) {
				Node newNode = new Node();
				newNode.rect.x = CI[i].uv.x * fontMap.width;
				newNode.rect.y = CI[i].uv.y * fontMap.height;
				newNode.rect.width = CI[i].uv.width * fontMap.width;
				newNode.rect.height = CI[i].uv.height * fontMap.height;

				if (newNode.rect.height < 0) {
					newNode.rect.height *= -1;
					newNode.rect.y -= newNode.rect.height;
				}
				if (CI[i].flipped) {
					newNode.pivot = new Vector2( newNode.rect.width - CI[i].vert.y, newNode.rect.height - CI[i].vert.x );
				} else {
					newNode.pivot = new Vector2( -CI[i].vert.x, newNode.rect.height - CI[i].vert.y );
				}
				if (CI[i].width != CI[i].vert.width) {
					newNode.advance = CI[i].width;
					newNode.overrideAdvance = true;
				}
				if (Mathf.Abs( CI[i].vert.width - (CI[i].flipped ? newNode.rect.height : newNode.rect.width) ) > 0.1f) {
					newNode.overrideScale = true;
					newNode.scale = new Vector2( CI[i].vert.width / newNode.rect.width, -CI[i].vert.height / newNode.rect.height );
				}

				newNode.orient = CI[i].flipped;

				newNode.ch = "" + (char) CI[i].index;

				nodes.Add( newNode );
			}
		}

		void UpdateFont ( int nodeIndex ) {
			UpdateFont( nodeIndex, true );
		}

		void UpdateFont ( int nodeIndex, bool runChecks ) {

			SerializedObject SO = new SerializedObject( font );
			SerializedProperty p = SO.FindProperty( "m_CharacterRects.Array" );
			p = p.GetArrayElementAtIndex( nodeIndex );
			p.Next( true ); //p is now CharIndex
			p.intValue = (int) nodes[nodeIndex].ch[0];
			p.Next( false ); //p is now UV Rect
			Rect newRect = new Rect( nodes[nodeIndex].rect.x / fontMap.width, nodes[nodeIndex].rect.y / fontMap.height, nodes[nodeIndex].rect.width / fontMap.width, nodes[nodeIndex].rect.height / fontMap.height );
			if (nodes[nodeIndex].orient) {
				newRect.y = newRect.y + newRect.height;
				newRect.height = newRect.height * -1;
			}
			p.rectValue = newRect;
			p.Next( false ); //p is now Vert Rect
			if (nodes[nodeIndex].orient) {
				newRect = new Rect( nodes[nodeIndex].pivot.y - nodes[nodeIndex].rect.height, nodes[nodeIndex].rect.width - nodes[nodeIndex].pivot.x, nodes[nodeIndex].rect.height, nodes[nodeIndex].rect.width * -1 );
			} else {
				newRect = new Rect( -nodes[nodeIndex].pivot.x, nodes[nodeIndex].rect.height - nodes[nodeIndex].pivot.y, nodes[nodeIndex].rect.width, -nodes[nodeIndex].rect.height );
			}
			if (nodes[nodeIndex].overrideScale) {
				newRect.width *= nodes[nodeIndex].scale.x;
				newRect.height *= nodes[nodeIndex].scale.y;
			}
			p.rectValue = newRect;
			p.Next( false ); //p is now Width
			if (nodes[nodeIndex].overrideAdvance) {
				p.floatValue = nodes[nodeIndex].advance;
			} else {
				p.floatValue = newRect.width;
				nodes[nodeIndex].advance = newRect.width;
			}
			p.Next( false ); //p is now Flip bool
			p.boolValue = nodes[nodeIndex].orient;
			SO.ApplyModifiedProperties();

			if (runChecks) {
				CheckDupeChars();
				CheckOverlapNodes();
			}
		}

		//Checks for duped characters
		void CheckDupeChars () {
			nodesDupe = false;
			for (int i = 0; i < nodes.Count; i++) {
				for (int j = 0; j < nodes.Count; j++) {
					if (i == j) { continue; }
					if (nodes[i].ch == nodes[j].ch) {
						nodesDupe = true;
						nodesDupeA = i;
						nodesDupeB = j;
						return;
					}
				}
			}
		}

		//Checks rects for overlap, sets error string
		void CheckOverlapNodes () {
			nodesOverlap = false;
			for (int i = 0; i < nodes.Count; i++) {
				int n = AutoNodeIsFree( nodes[i].rect );
				if (n != -1) {
					nodesOverlap = true;
					nodesOverlapA = i;
					nodesOverlapB = n;
				}
			}
		}


		#endregion


		#region Autoset Methods
		//To reduce repeated GetPixel calls, this creates a 2d array of ints, where 0 is ignored, 1 is a pixel, 2 is a checked pixel, 3 is a smart pixel
		void MakeintFontMap () {
			intFontMap = new int[fontMap.width, fontMap.height];
			for (int i = 0; i < fontMap.width; i++) {
				for (int j = 0; j < fontMap.height; j++) {
					Color c = fontMap.GetPixel( i, j );
					intFontMap[i, j] = IgnoreCondition( c ) ? 0 : (smartMode && (IsSmart( c ))) ? 3 : 1;
				}
			}
		}

		//When reading the texture into int array, pixels that match this condition will be ignored in other functions
		bool IgnoreCondition ( Color c ) {
			return c.a == 0;
		}

		//When reading the texture into int array, pixels that match this condition will be treated as 'Smart'
		bool IsSmart ( Color c ) {
			return
				c.r > smartColor.r - 0.02f && c.r < smartColor.r + 0.02f &&
				c.g > smartColor.g - 0.02f && c.g < smartColor.g + 0.02f &&
				c.b > smartColor.b - 0.02f && c.b < smartColor.b + 0.02f &&
				c.a > smartColor.a - 0.02f && c.a < smartColor.a + 0.02f;
		}

		//Checks given autorect for overlap with all others, after autoset, returns the index of overlap
		int AutoNodeIsFree ( Rect r1 ) {
			for (int i = 0; i < nodes.Count; i++) {
				Rect r2 = nodes[i].rect;
				if (r1 == r2) { continue; }
				if ((r1.xMin < r2.xMax) && (r1.xMax > r2.xMin) && (r1.yMin < r2.yMax) && (r1.yMax > r2.yMin)) {
					return i;
				}
			}
			return -1;
		}

		//remove Final node from list when stopping autoset early
		void RemoveNode () {
			nodes.RemoveAt( nodes.Count - 1 );
			SerializedObject SO = new SerializedObject( font );
			SerializedProperty p = SO.FindProperty( "m_CharacterRects.Array" );
			p.DeleteArrayElementAtIndex( p.arraySize - 1 );
			SO.ApplyModifiedProperties();
		}

		//Add node to working list from auto list
		void AddNode () {
			autoNodeList[0].ch = "";
			nodes.Add( autoNodeList[0] );
			autoNodeList.RemoveAt( 0 );
			SerializedObject SO = new SerializedObject( font );
			SerializedProperty p = SO.FindProperty( "m_CharacterRects.Array" );
			p.InsertArrayElementAtIndex( p.arraySize );
			SO.ApplyModifiedProperties();
			highlightedNodeIndex = nodes.Count - 1;
			ViewFocus();
		}

		//Auto set, looks for pixels which match the ignore condition, then floodfills to find the whole shape
		void AutoSet () {
			nodesDupe = nodesOverlap = false;
			nodes.Clear();
			font.characterInfo = new CharacterInfo[0];
			if (repeatList == null)
				repeatList = new List<char>();
			autoNodeList = new List<Node>();
			autoNodeListunsorted = new List<Node>();
			shape = new List<Vector2>();
			shapeSmartLine = new List<Vector2>();
			MakeintFontMap();
			int maxHeight = 1;
			for (int j = fontMap.height - 1; j > -1; j--) {
				for (int i = 0; i < fontMap.width; i++) {
					if (intFontMap[i, j] != 0 && intFontMap[i, j] != 2) {
						shape.Clear();
						if (smartMode) {
							shapeSmartLine.Clear();
						}
						FloodFill( i, j );
						int xMin = fontMap.width, xMax = 0, yMin = fontMap.height, yMax = 0;
						foreach (Vector2 a in shape) {
							if (a.x > xMax)
								xMax = (int) a.x;
							if (a.x < xMin)
								xMin = (int) a.x;
							if (a.y > yMax)
								yMax = (int) a.y;
							if (a.y < yMin)
								yMin = (int) a.y;
						}
						Rect theRect = new Rect( xMin - floodfillPadding, yMin - floodfillPadding, xMax - xMin + 1 + floodfillPadding * 2, yMax - yMin + 1 + floodfillPadding * 2 );
						Node newNode = new Node();
						newNode.rect = theRect;
						if (maxHeight < theRect.height)
							maxHeight = (int) theRect.height;
						if (smartMode) {
							if (shapeSmartLine.Count > 1) {
								if (shapeSmartLine[0].y == shapeSmartLine[1].y) {
									//Compare the first smartline pixel with the second, if they have the same y,
									//then the character is upright, otherwise rotated
									newNode.orient = false;
									newNode.pivot.y = shapeSmartLine[0].y - theRect.y;
								} else {
									newNode.orient = true;
									newNode.pivot.x = shapeSmartLine[0].x - theRect.x + 1;
									newNode.pivot.y = theRect.height;
									if (maxHeight < theRect.width)
										maxHeight = (int) theRect.width;
								}
							}
						} else {
							newNode.pivot = new Vector2( 0, newNode.rect.height );
						}
						autoNodeListunsorted.Add( newNode );
					}
				}
			}

			autoNodeList.Add( autoNodeListunsorted[0] );
			while (autoNodeListunsorted.Count > 0) {
				int next = FindLeftMost();
				autoNodeList.Add( autoNodeListunsorted[next] );
				autoNodeListunsorted.RemoveAt( next );
			}
			autoNodeList.RemoveAt( 0 );


			autoSetDialog = true;
			GUI.FocusControl( "" );

			AddNode();

			autoNodeListunsorted.Clear();
			shape.Clear();
			shapeSmartLine.Clear();

			SerializedObject SO = new SerializedObject( font );
			SerializedProperty p = SO.FindProperty( "m_LineSpacing" );
			p.floatValue = maxHeight;
			SO.ApplyModifiedProperties();


		}

		void EndAutoSet () {
			for (int i = 0; i < nodes.Count; i++) {
				UpdateFont( i );
			}
			clickNodeIndex = highlightedNodeIndex = -1;
			autoSetDialog = false;
			ViewReset();
			autoNodeList.Clear();
			GUI.FocusControl( "" );
			Debug.Log( "Dont forget to add a node for spacebar!" );
		}

		//Gets the left-most rect, but only if it has a vertical position within the height of the previous rect
		int FindLeftMost () {
			int lowest = fontMap.width, backuplowest = fontMap.width, lowestInd = -1, backupInd = -1;
			for (int i = 0; i < autoNodeListunsorted.Count; i++) {
				if (autoNodeListunsorted[i].rect.x < lowest) {
					if (autoNodeListunsorted[i].rect.center.y < autoNodeList[autoNodeList.Count - 1].rect.center.y + autoNodeList[autoNodeList.Count - 1].rect.height / 2 &&
							autoNodeListunsorted[i].rect.center.y > autoNodeList[autoNodeList.Count - 1].rect.center.y - autoNodeList[autoNodeList.Count - 1].rect.height / 2) {
						lowest = (int) autoNodeListunsorted[i].rect.x;
						lowestInd = i;
					}
				}
				if (autoNodeListunsorted[i].rect.x < backuplowest) {
					backuplowest = (int) autoNodeListunsorted[i].rect.x;
					backupInd = i;
				}
			}
			if (lowestInd != -1) {
				return lowestInd;
			} else {
				return backupInd;
			}
		}

		//floodfill to find the whole shape, using a scanline algo
		void FloodFill ( int x, int y ) {
			if (intFontMap[x, y] == 0 || intFontMap[x, y] == 2) { return; }
			int y1 = y;
			while (y1 < fontMap.height && (intFontMap[x, y1] == 1 || intFontMap[x, y1] == 3)) {
				if (smartMode) {
					if (intFontMap[x, y1] == 3) {
						shapeSmartLine.Add( new Vector2( x, y1 ) );
					}
				}
				intFontMap[x, y1] = 2;
				shape.Add( new Vector2( x, y1 ) );
				y1++;
			}
			int maxY = y1 - 1;
			y1 = y - 1;
			while (y1 > -1 && (intFontMap[x, y1] == 1 || intFontMap[x, y1] == 3)) {
				if (smartMode) {
					if (intFontMap[x, y1] == 3) {
						shapeSmartLine.Add( new Vector2( x, y1 ) );
					}
				}
				intFontMap[x, y1] = 2;
				shape.Add( new Vector2( x, y1 ) );
				y1--;
			}
			int minY = y1 + 1;
			for (int i = minY; i < maxY + 1; i++) {
				if (x > 0 && (intFontMap[x - 1, i] == 1 || intFontMap[x - 1, i] == 3)) {
					FloodFill( x - 1, i );
				}
				if (x < fontMap.width - 1 && (intFontMap[x + 1, i] == 1 || intFontMap[x + 1, i] == 3)) {
					FloodFill( x + 1, i );
				}
			}
		}
		#endregion

		#region Srinkwrap methods
		//During shrinkwrap, check all pixels along the specified side
		bool ShrinkSide ( int indx, int side, int startMode ) {
			for (int i = 0; i < ((side == 0 || side == 2) ? nodes[indx].rect.width : nodes[indx].rect.height); i++) {
				int x = 0, y = 0;
				switch (side) {
					case 0:
						x = (int) nodes[indx].rect.x + i;
						y = (int) nodes[indx].rect.y;
						break;
					case 1:
						x = (int) nodes[indx].rect.x;
						y = (int) nodes[indx].rect.y + i;
						break;
					case 2:
						x = (int) nodes[indx].rect.x + i;
						y = (int) (nodes[indx].rect.y + nodes[indx].rect.height - 1);
						break;
					case 3:
						x = (int) (nodes[indx].rect.x + nodes[indx].rect.width - 1);
						y = (int) nodes[indx].rect.y + i;
						break;
				}
				if (intFontMap[x, y] != 0) {
					if (startMode == 1) {
						return true;
					} else if (((side == 1 || side == 3) && (x == 0 || x == fontMap.width - 1)) ||
							   ((side == 0 || side == 2) && (y == 0 || y == fontMap.height - 1))) {
						return startMode == 1 ? true : false;
					} else if (intFontMap[x + ((side == 0 || side == 2) ? 0 : (side == 1 ? -1 : 1)), y + ((side == 1 || side == 3) ? 0 : (side == 0 ? -1 : 1))] != 0) {
						return true;
					} else {
						continue;
					}
				}
			}
			return false;
		}

		//Checks if a Rect actually has pixels inside it, and if it is inside the bounds of the texture
		bool CheckShrinkForPixels ( int indx, Rect StartR ) {
			if (nodes[indx].rect.x > fontMap.width - 1 || nodes[indx].rect.y > fontMap.height - 1 || nodes[indx].rect.xMax < 0 || nodes[indx].rect.yMax < 0) {
				//if a rect is entirely outside the texture, return false
				return false;
			}
			//the following 4 parts clip the rect inside the texture boundary
			if (nodes[indx].rect.x < 0) {
				nodes[indx].rect.width += nodes[indx].rect.x;
				nodes[indx].rect.x = 0;
			}
			if (nodes[indx].rect.xMax > fontMap.width) {
				nodes[indx].rect.xMax = fontMap.width;
			}
			if (nodes[indx].rect.y < 0) {
				nodes[indx].rect.height += nodes[indx].rect.y;
				nodes[indx].rect.y = 0;
			}
			if (nodes[indx].rect.yMax > fontMap.height) {
				nodes[indx].rect.yMax = fontMap.height;
			}
			//Check every pixel in the rect. if one is found, snap to pixel grid and return true
			for (int i = 0; i < nodes[indx].rect.width; i++) {
				for (int j = 0; j < nodes[indx].rect.height; j++) {
					if (intFontMap[(int) nodes[indx].rect.x + i, (int) nodes[indx].rect.y + j] != 0) {
						nodes[indx].rect = Rect.MinMaxRect( Mathf.FloorToInt( nodes[indx].rect.x ), Mathf.FloorToInt( nodes[indx].rect.y ), Mathf.CeilToInt( nodes[indx].rect.xMax ), Mathf.CeilToInt( nodes[indx].rect.yMax ) );
						return true;
					}
				}
			}
			//if the above check didnt find a pixel, put the rect back to non-clipped and snapped, and return false
			nodes[indx].rect = StartR;
			return false;
		}


		//Shrink or expand all rects to accomodate all included pixels
		void ShrinkWrap () {
			string outString = "Shrinkwrapped ";
			MakeintFontMap();
			for (int i = 0; i < nodes.Count; i++) {
				Rect StartR = nodes[i].rect;
				if (!CheckShrinkForPixels( i, nodes[i].rect )) {
					continue;
				}
				int step = 1;
				while (step != 0) {
					step = 0;
					bool mode = ShrinkSide( i, 0, 3 );
					while (mode == ShrinkSide( i, 0, mode ? 2 : 1 )) {
						step++;
						nodes[i].rect.y += mode ? -1 : 1;
						nodes[i].rect.height += mode ? 1 : -1;
					}

					mode = ShrinkSide( i, 1, 3 );
					while (mode == ShrinkSide( i, 1, mode ? 2 : 1 )) {
						step++;
						nodes[i].rect.x += mode ? -1 : 1;
						nodes[i].rect.width += mode ? 1 : -1;
					}

					mode = ShrinkSide( i, 2, 3 );
					while (mode == ShrinkSide( i, 2, mode ? 2 : 1 )) {
						step++;
						nodes[i].rect.height += mode ? 1 : -1;
					}

					mode = ShrinkSide( i, 3, 3 );
					while (mode == ShrinkSide( i, 3, mode ? 2 : 1 )) {
						step++;
						nodes[i].rect.width += mode ? 1 : -1;
					}
				}
				if (nodes[i].rect != StartR) {
					outString += "'" + nodes[i].ch + "' ";
				}
				UpdateFont( i );
			}

			CheckOverlapNodes();

			if (outString.Length > 14) {
				if (outString[outString.Length - 1] == ',')
					outString = outString.TrimEnd( ',' );
				Debug.Log( outString );
			} else {
				Debug.Log( "No need for shrinkwrap" );
			}
		}
		#endregion





		#region Packer methods

		//Start the Rect Packer according to the selected gui buttons
		void BeginPack () {
			System.DateTime TimeA = System.DateTime.Now;
			packComplete = false;
			if (packedFont == null) {

				packedFont = new Font();
				string path = AssetDatabase.GetAssetPath( font );
				path = path.Insert( path.IndexOf( ".fontsettings" ), "(Packed)" );
				AssetDatabase.CreateAsset( packedFont, path );


				SerializedObject SO = new SerializedObject( packedFont );
				SerializedProperty p = SO.FindProperty( "m_LineSpacing" );
				p.floatValue = font.lineHeight;
				SO.ApplyModifiedProperties();


				Material fontMaterial = new Material( defaultShader );
				path = AssetDatabase.GetAssetPath( font.material );
				path = path.Insert( path.IndexOf( ".mat" ), "(Packed)" );
				AssetDatabase.CreateAsset( fontMaterial, path );
				packedFont.material = fontMaterial;

				packedfontMapPath = AssetDatabase.GetAssetPath( fontMap );
				packedfontMapPath = packedfontMapPath.Insert( packedfontMapPath.LastIndexOf( "." ), "(Packed)" );
			}

			startPackNodeList = new List<PackNode>();
			resultPackNodeList = new List<PackNode>();
			anchors = new List<Vector2>();
			partitions = new List<Rect>();


			for (int i = 0; i < nodes.Count; i++) {
				PackNode newNode = new PackNode();
				newNode.CIIndex = i;
				if (nodes[i].ch == " ") {
					font.characterInfo[i].vert = new Rect( 0, 0, 1, 1 );
					nodes[i].rect.width = 1;
					nodes[i].rect.height = 1;
				}

				newNode.CI = font.characterInfo[i];
				newNode.StartRect = nodes[i].rect;
				newNode.ResultRect = newNode.StartRect;
				newNode.ResultRect.width += packBuffer;
				newNode.ResultRect.height += packBuffer;
				startPackNodeList.Add( newNode );
			}


			int Total = 0;
			for (int i = 0; i < startPackNodeList.Count; i++) {
				Total += (int) (startPackNodeList[i].ResultRect.width * startPackNodeList[i].ResultRect.height);
			}
			Total = (int) Mathf.Sqrt( Total );
			Total = NearestPOT( Total );
			packSizeX = packSizeY = Total;


			for (int i = 0; i < startPackNodeList.Count; i++) { //Sort rects according to toolbar choice
				switch (packSort) {
					case 0:
						startPackNodeList[i].Height = startPackNodeList[i].ResultRect.height;
						break;
					case 1:
						startPackNodeList[i].Height = startPackNodeList[i].ResultRect.width;
						startPackNodeList[i].ResultRect = new Rect( startPackNodeList[i].ResultRect.x, startPackNodeList[i].ResultRect.y, startPackNodeList[i].ResultRect.height, startPackNodeList[i].ResultRect.width );
						startPackNodeList[i].SameOrient = !startPackNodeList[i].SameOrient;
						break;
					case 2:
						if (startPackNodeList[i].ResultRect.height > startPackNodeList[i].ResultRect.width) {
							startPackNodeList[i].Height = startPackNodeList[i].ResultRect.height;
						} else {
							startPackNodeList[i].Height = startPackNodeList[i].ResultRect.width;
							startPackNodeList[i].ResultRect = new Rect( startPackNodeList[i].ResultRect.x, startPackNodeList[i].ResultRect.y, startPackNodeList[i].ResultRect.height, startPackNodeList[i].ResultRect.width );
							startPackNodeList[i].SameOrient = !startPackNodeList[i].SameOrient;
						}
						break;
					case 3:
						if (startPackNodeList[i].ResultRect.height > startPackNodeList[i].ResultRect.width) {
							startPackNodeList[i].Height = startPackNodeList[i].ResultRect.width;
							startPackNodeList[i].ResultRect = new Rect( startPackNodeList[i].ResultRect.x, startPackNodeList[i].ResultRect.y, startPackNodeList[i].ResultRect.height, startPackNodeList[i].ResultRect.width );
							startPackNodeList[i].SameOrient = !startPackNodeList[i].SameOrient;
						} else {
							startPackNodeList[i].Height = startPackNodeList[i].ResultRect.height;
						}
						break;
				}
			}

			if (packMethod != 4) {
				for (int i = startPackNodeList.Count - 1; i > 0; i--) {
					CheckAbove( i );
				}
			}

			switch (packMethod) { //Pack rects according to toolbar choice
				case 0:
					SimplePack();
					break;
				case 1:
					SwitchbackPack();
					break;
				case 2:
					PartitionPack();
					break;
				case 3:
					AnchorPack();
					break;
				case 4:
					UnityPack();
					break;
			}

			if (packMethod != 4)
				DrawResult( resultPackNodeList );
			packComplete = true;

			ViewReset();

			System.TimeSpan TimeB = System.DateTime.Now - TimeA;
			Debug.Log( "Time taken: " + string.Format( "{0:ss ffffff}", TimeB ) );
		}

		//increases int to next POT
		int NearestPOT ( int n ) {
			int i = 1;
			while (i < n) {
				i = i << 1;
			}
			return i;
		}


		//Slides pack rect down and left, if possible, during pack
		void Slide ( bool b ) {
			int y1 = (int) getPackNode.ResultRect.y, safe = 0, step = 1;
			Rect testRect = new Rect( getPackNode.ResultRect.x, y1, getPackNode.ResultRect.width, getPackNode.ResultRect.height );
			if (NodeIsFree( testRect )) {
				step = -1;
			}
			y1 += step;

			//safeLimit stops infinate loops occuring in the while loop. If you have a lot of characters, this might not be big enough.
			while (true && safe < 1000) {
				if (y1 < 0) {
					getPackNode.ResultRect.y = y1 - step;
					return;
				}
				if (y1 + getPackNode.ResultRect.height > packSizeY) {
					if (packOptionNPOT) {
						packSizeY = y1 + (int) getPackNode.ResultRect.height;
						Slide( b );
						return;
					} else {
						packSizeY *= 2;
						Slide( b );
						return;
					}
				}
				testRect = new Rect( getPackNode.ResultRect.x, y1, getPackNode.ResultRect.width, getPackNode.ResultRect.height );
				bool NoOverlap = NodeIsFree( testRect );
				if (step == 1 && NoOverlap) {
					getPackNode.ResultRect.y = y1;
					return;
				}
				if (step == -1 && !NoOverlap) {
					getPackNode.ResultRect.y = y1 - step;
					int x1 = (int) testRect.x;
					step = -1;
					while (b && true && safe < 300) {
						if (x1 < 0) {
							getPackNode.ResultRect.x = x1 - step;
							return;
						}
						testRect = new Rect( x1, getPackNode.ResultRect.y, getPackNode.ResultRect.width, getPackNode.ResultRect.height );
						if (!NodeIsFree( testRect )) {
							getPackNode.ResultRect.x = x1 - step;
							return;
						}
						x1 += step;
						safe++;
					}
					return;
				}
				y1 += step;
				safe++;
			}
			Debug.Log( "possible infinate loop, oops! Doublecheck that there is the correct number of chracters, if not, try a different packing method" );
			return;
		}


		//slides rect during an anchor pack, up or down depending on start condition
		Rect anchorslide ( Rect r ) {
			Rect testRect = new Rect( r.x, r.y, r.width, r.height );
			int leftMost = (int) r.x;
			//safeLimit stops infinate loops occuring in the while loop. If you have a lot of characters, this might not be big enough.
			int safe = 0;
			while (NodeIsFree( testRect ) && leftMost > 0 && safe < 1000) {
				leftMost = (int) testRect.x;
				testRect.x--;
				safe++;
			}
			testRect.x = r.x;
			int topMost = (int) r.y;
			while (NodeIsFree( testRect ) && topMost > 0 && safe < 1000) {
				topMost = (int) testRect.y;
				testRect.y--;
				safe++;
			}
			if ((r.x - leftMost) > (r.y - topMost)) {
				r.x = leftMost;
			} else {
				r.y = topMost;
			}
			if (safe > 999) { Debug.Log( "possible infinate loop, oops! Doublecheck that there is the correct number of chracters, if not, try a different packing method" ); }
			return r;
		}


		//during partition pack, adds rect into the free partition
		void AddPartition ( Rect r, int i ) {
			getPackNode.ResultRect = r;
			resultPackNodeList.Add( getPackNode );
			if (getPackNode.ResultRect.xMax > packMaxSizeX) { packMaxSizeX = (int) getPackNode.ResultRect.xMax; }
			if (getPackNode.ResultRect.yMax > packMaxSizeY) { packMaxSizeY = (int) getPackNode.ResultRect.yMax; }
			partitions.RemoveAt( i );
			if (r.height < r.width) {
				partitions.Add( new Rect( r.xMax, r.yMin, 0, r.height ) );
				partitions.Add( new Rect( r.xMin, r.yMax, 0, -1 ) );
			} else {
				partitions.Add( new Rect( r.xMin, r.yMax, r.width, 0 ) );
				partitions.Add( new Rect( r.xMax, r.yMin, -1, 0 ) );
			}
		}


		//pack by partition, roughly based on lightmap packing
		void PartitionPack () {
			//safeLimit stops infinate loops occuring in the while loop. If you have a lot of characters, this might not be big enough.
			int safeLimit = 1000;
			packSizeX = 2;
			packSizeY = 2;
			packMaxSizeX = 2;
			packMaxSizeY = 2;
			partitions.Add( new Rect( 0, 0, packSizeX, packSizeY ) );
			for (int i = startPackNodeList.Count; i > 0; i--) {
				GetFromHeap();
				int longest = (int) (getPackNode.ResultRect.width > getPackNode.ResultRect.height ? getPackNode.ResultRect.width : getPackNode.ResultRect.height);
				//qwer Not needed?
				//int shortest = (int) (getPackNode.ResultRect.width>getPackNode.ResultRect.height?getPackNode.ResultRect.height:getPackNode.ResultRect.width);
				bool searching = true;
				int safe = 0;
				int j = -1;
				while (searching && safe < safeLimit) {
					safe++;
					j++;
					if (j > partitions.Count - 1) {
						if (packSizeX < packSizeY) {
							if (packOptionNPOT) {
								packSizeX += longest;
							} else {
								packSizeX *= 2;
							}
						} else {
							if (packOptionNPOT) {
								packSizeY += longest;
							} else {
								packSizeY *= 2;
							}
						}
						j = 0;
					}
					Rect testRect = new Rect( partitions[j].x, partitions[j].y, getPackNode.ResultRect.width, getPackNode.ResultRect.height );
					if (NodeIsFree( testRect )) {
						AddPartition( testRect, j );
						searching = false;
					} else if (packOptionRotate && NodeIsFree( new Rect( testRect.x, testRect.y, testRect.height, testRect.width ) )) {
						getPackNode.SameOrient = !getPackNode.SameOrient;
						AddPartition( new Rect( testRect.x, testRect.y, testRect.height, testRect.width ), j );
						searching = false;
					}

				}
				if (safe > safeLimit - 1)
					Debug.Log( "possible infinate loop, oops! If you have a lot of characters, try increasing the safe limit in PartitionPack ()" );
			}
		}


		//pack by anchors, added from top left and bottom right coreners of packed rects, sorted by sortmode
		void AnchorPack () {
			packSizeX = 2;
			packSizeY = 2;
			packMaxSizeX = 2;
			packMaxSizeY = 2;
			anchors.Add( Vector2.zero );
			for (int i = startPackNodeList.Count; i > 0; i--) {
				GetFromHeap();
				bool searching = true;
				int j = 0;
				while (searching) {
					if (j > anchors.Count - 1) {
						if (packSizeX < packSizeY) {
							if (packOptionNPOT) {
								packSizeX += (int) getPackNode.ResultRect.width;
							} else {
								packSizeX *= 2;
							}
						} else {
							if (packOptionNPOT) {
								packSizeY += (int) getPackNode.ResultRect.height;
							} else {
								packSizeY *= 2;
							}
						}
						j = 0;
					}
					Rect testRect = new Rect( anchors[j].x, anchors[j].y, getPackNode.ResultRect.width, getPackNode.ResultRect.height );
					if (NodeIsFree( testRect )) {
						testRect = anchorslide( testRect );
						getPackNode.ResultRect = testRect;
						resultPackNodeList.Add( getPackNode );
						if (getPackNode.ResultRect.xMax > packMaxSizeX) { packMaxSizeX = (int) getPackNode.ResultRect.xMax; }
						if (getPackNode.ResultRect.yMax > packMaxSizeY) { packMaxSizeY = (int) getPackNode.ResultRect.yMax; }
						anchors.Add( new Vector2( testRect.xMax, testRect.yMin ) );
						anchors.Add( new Vector2( testRect.xMin, testRect.yMax ) );
						anchors.Sort( Sortanchors );
						searching = false;
					} else if (packOptionRotate && NodeIsFree( new Rect( testRect.x, testRect.y, testRect.height, testRect.width ) )) {
						getPackNode.SameOrient = !getPackNode.SameOrient;
						testRect = new Rect( testRect.x, testRect.y, testRect.height, testRect.width );
						testRect = anchorslide( testRect );
						getPackNode.ResultRect = testRect;
						resultPackNodeList.Add( getPackNode );
						if (getPackNode.ResultRect.xMax > packMaxSizeX) { packMaxSizeX = (int) getPackNode.ResultRect.xMax; }
						if (getPackNode.ResultRect.yMax > packMaxSizeY) { packMaxSizeY = (int) getPackNode.ResultRect.yMax; }
						anchors.Add( new Vector2( testRect.xMax, testRect.yMin ) );
						anchors.Add( new Vector2( testRect.xMin, testRect.yMax ) );
						anchors.Sort( Sortanchors );
						searching = false;
					}
					j++;
				}
			}
		}


		//pack left to right, then right to left
		void SwitchbackPack () {
			packMaxSizeX = 2;
			packMaxSizeY = 2;
			bool SwitchStep = true;
			for (int i = startPackNodeList.Count; i > 0; i--) {
				GetFromHeap();
				if (resultPackNodeList.Count == 0) {
					getPackNode.ResultRect.x = 0;
					getPackNode.ResultRect.y = 0;
					resultPackNodeList.Add( getPackNode );
				} else {
					if (SwitchStep) {
						getPackNode.ResultRect.x = resultPackNodeList[resultPackNodeList.Count - 1].ResultRect.x + resultPackNodeList[resultPackNodeList.Count - 1].ResultRect.width;
					} else {
						getPackNode.ResultRect.x = resultPackNodeList[resultPackNodeList.Count - 1].ResultRect.x - getPackNode.ResultRect.width;
					}
					getPackNode.ResultRect.y = resultPackNodeList[resultPackNodeList.Count - 1].ResultRect.y;
					if (getPackNode.ResultRect.x > 0 && getPackNode.ResultRect.xMax < packSizeX) {
						Slide( SwitchStep );
						resultPackNodeList.Add( getPackNode );
						if (getPackNode.ResultRect.xMax > packMaxSizeX) { packMaxSizeX = (int) getPackNode.ResultRect.xMax; }
						if (getPackNode.ResultRect.yMax > packMaxSizeY) { packMaxSizeY = (int) getPackNode.ResultRect.yMax; }
					} else {
						if (SwitchStep) {
							getPackNode.ResultRect.x = packSizeX - getPackNode.ResultRect.width;
						} else {
							getPackNode.ResultRect.x = 0;
						}
						getPackNode.ResultRect.y = resultPackNodeList[resultPackNodeList.Count - 1].ResultRect.y + resultPackNodeList[resultPackNodeList.Count - 1].ResultRect.height;
						SwitchStep = !SwitchStep;
						Slide( SwitchStep );
						resultPackNodeList.Add( getPackNode );
						if (getPackNode.ResultRect.xMax > packMaxSizeX) { packMaxSizeX = (int) getPackNode.ResultRect.xMax; }
						if (getPackNode.ResultRect.yMax > packMaxSizeY) { packMaxSizeY = (int) getPackNode.ResultRect.yMax; }
					}
				}
			}
		}


		int newLineIndex = 0;
		//basic pack from left to right, with a downward slide
		void SimplePack () {
			packMaxSizeX = 2;
			packMaxSizeY = 2;
			for (int i = startPackNodeList.Count; i > 0; i--) {
				GetFromHeap();
				if (resultPackNodeList.Count == 0) {
					newLineIndex = 0;
					getPackNode.ResultRect.x = 0;
					getPackNode.ResultRect.y = 0;
				} else {
					if (resultPackNodeList[resultPackNodeList.Count - 1].ResultRect.x + resultPackNodeList[resultPackNodeList.Count - 1].ResultRect.width + getPackNode.ResultRect.width > packSizeX) {
						getPackNode.ResultRect.x = 0;
						getPackNode.ResultRect.y = resultPackNodeList[newLineIndex].ResultRect.y + resultPackNodeList[newLineIndex].ResultRect.height;
						newLineIndex = resultPackNodeList.Count;
						Slide( true );
					} else {
						getPackNode.ResultRect.x = resultPackNodeList[resultPackNodeList.Count - 1].ResultRect.x + resultPackNodeList[resultPackNodeList.Count - 1].ResultRect.width;
						getPackNode.ResultRect.y = resultPackNodeList[resultPackNodeList.Count - 1].ResultRect.y;
						Slide( true );
					}
				}
				resultPackNodeList.Add( getPackNode );
				if (getPackNode.ResultRect.xMax > packMaxSizeX) { packMaxSizeX = (int) getPackNode.ResultRect.xMax; }
				if (getPackNode.ResultRect.yMax > packMaxSizeY) { packMaxSizeY = (int) getPackNode.ResultRect.yMax; }
			}
		}


		//get the top rect from the heap
		void GetFromHeap () {
			getPackNode = startPackNodeList[0];
			startPackNodeList[0] = startPackNodeList[startPackNodeList.Count - 1];
			startPackNodeList.RemoveAt( startPackNodeList.Count - 1 );
			CheckBelow( 0 );
		}


		//bubble up in the heap
		void CheckAbove ( int i ) {
			if (i == 0) { return; }
			int ParentIndex = (int) Mathf.Floor( (i + 1) / 2 ) - 1;
			if (startPackNodeList[i].Height > startPackNodeList[ParentIndex].Height) {
				swapPackNode = startPackNodeList[ParentIndex];
				startPackNodeList[ParentIndex] = startPackNodeList[i];
				startPackNodeList[i] = swapPackNode;
				CheckBelow( i );
			}

		}


		//bubble down in the heap
		void CheckBelow ( int i ) {
			int child2Index = (i + 1) * 2;
			int child1Index = child2Index - 1;
			int swapIndex = i;
			if (child1Index < startPackNodeList.Count) {
				if (startPackNodeList[child1Index].Height > startPackNodeList[i].Height) {
					swapIndex = child1Index;
				}
			}
			if (child2Index < startPackNodeList.Count) {
				if (startPackNodeList[child2Index].Height > startPackNodeList[(swapIndex == i) ? i : child1Index].Height) {
					swapIndex = child2Index;
				}
			}
			if (swapIndex != i) {
				swapPackNode = startPackNodeList[swapIndex];
				startPackNodeList[swapIndex] = startPackNodeList[i];
				startPackNodeList[i] = swapPackNode;
				CheckBelow( swapIndex );
			}

		}


		//Sort anchors, either by dist from the origin, or dist to the closest pack boundary
		int Sortanchors ( Vector2 a, Vector2 b ) {
			if (packOptionSort) {
				int dx = packSizeX - (int) a.x, dy = packSizeY - (int) a.y, dx2 = packSizeX - (int) b.x, dy2 = packSizeY - (int) b.y;
				return (dx2 > dy2 ? dx2 : dy2).CompareTo( dx > dy ? dx : dy );
			} else {
				return a.sqrMagnitude.CompareTo( b.sqrMagnitude );
			}
		}




		void UnityPack () {
			Texture2D[] texs = new Texture2D[nodes.Count];
			Rect[] rects = new Rect[nodes.Count];
			packedFontMap = new Texture2D( 1, 1 );
			for (int i = 0; i < nodes.Count; i++) {
				texs[i] = new Texture2D( (int) nodes[i].rect.width, (int) nodes[i].rect.height );
				texs[i].SetPixels( fontMap.GetPixels( (int) nodes[i].rect.x, (int) nodes[i].rect.y, (int) nodes[i].rect.width, (int) nodes[i].rect.height ) );
			}


			rects = packedFontMap.PackTextures( texs, packBuffer );
			for (int i = 0; i < nodes.Count; i++) {
				PackNode newNode = new PackNode( startPackNodeList[i] );
				newNode.ResultRect = rects[i];
				resultPackNodeList.Add( newNode );
			}


			byte[] bytes = packedFontMap.EncodeToPNG();
			System.IO.File.WriteAllBytes( packedfontMapPath, bytes );

			CharacterInfo[] OutCharacterInfo = new CharacterInfo[nodes.Count];
			for (int i = 0; i < resultPackNodeList.Count; i++) {
				Rect newUvRect = resultPackNodeList[i].ResultRect;
				if (resultPackNodeList[i].SameOrient) {
					if (resultPackNodeList[i].CI.flipped) {
						newUvRect.y = newUvRect.y + newUvRect.height;
						newUvRect.height = newUvRect.height * -1;
					} else {
						//The rect hasnt been rotated, and wasnt originally flipped anyway.
					}
				} else {
					if (resultPackNodeList[i].CI.flipped) {
						resultPackNodeList[i].CI.flipped = false;
					} else {
						newUvRect.y = newUvRect.y + newUvRect.height;
						newUvRect.height = newUvRect.height * -1;
						resultPackNodeList[i].CI.flipped = true;
						resultPackNodeList[i].CI.vert.width = resultPackNodeList[i].ResultRect.height;
						resultPackNodeList[i].CI.vert.height = resultPackNodeList[i].ResultRect.width * -1;
					}
				}

				resultPackNodeList[i].CI.uv = newUvRect;
				OutCharacterInfo[resultPackNodeList[i].CIIndex] = resultPackNodeList[i].CI;
			}

			packedFont.characterInfo = OutCharacterInfo;
			EditorUtility.SetDirty( packedFont );
			EditorApplication.SaveAssets();
			AssetDatabase.Refresh();

			packedFont.material.mainTexture = (Texture2D) AssetDatabase.LoadAssetAtPath( packedfontMapPath, typeof( Texture2D ) );
			Debug.Log( "Pack Complete, Result texture size: " + packedFontMap.width + "x" + packedFontMap.height + " = " + packedFontMap.width * packedFontMap.height + " Pixels" );
			Debug.Log( "Pack fits inside " + packMaxSizeX + "x" + packMaxSizeY + " = " + packMaxSizeX * packMaxSizeY + " Pixels, and contains " + resultPackNodeList.Count + " characters" );

		}



		//draws the packed map, translating from startrect to resultrect
		void DrawResult ( List<PackNode> R ) {
			if (packBuffer != 0) {
				for (int r = 0; r < R.Count; r++) {
					R[r].ResultRect.width = R[r].ResultRect.width - packBuffer;
					R[r].ResultRect.height = R[r].ResultRect.height - packBuffer;
				}
			}
			packedFontMap = new Texture2D( packOptionNPOT ? (packOptionFit ? NearestPOT( packMaxSizeX ) : packSizeX) : packSizeX, packOptionNPOT ? (packOptionFit ? NearestPOT( packMaxSizeY ) : packSizeY) : packSizeY );
			packedFontMap.filterMode = FilterMode.Point;
			for (int i = 0; i < packedFontMap.width; i++) {
				for (int j = 0; j < packedFontMap.height; j++) {
					packedFontMap.SetPixel( i, j, new Color( 0, 0, 0, 0 ) );
				}
			}
			packedFontMap.Apply();
			for (int r = 0; r < R.Count; r++) {
				for (int j = 0; j < R[r].ResultRect.height; j++) {
					for (int i = 0; i < R[r].ResultRect.width; i++) {
						Color Col;
						if (R[r].SameOrient) {
							Col = fontMap.GetPixel( (int) R[r].StartRect.x + i, (int) R[r].StartRect.y + j );
						} else {
							if (R[r].CI.flipped) {
								Col = fontMap.GetPixel( (int) (R[r].StartRect.x + R[r].StartRect.width - (R[r].StartRect.width - j)), (int) (R[r].StartRect.y + (R[r].StartRect.height - i) - 1) );
							} else {
								Col = fontMap.GetPixel( (int) (R[r].StartRect.x + R[r].StartRect.width - j - 1), (int) R[r].StartRect.y + i );
							}
						}
						//Draw in the acutal texture pixel
						packedFontMap.SetPixel( (int) R[r].ResultRect.x + i, (int) R[r].ResultRect.y + j, Col );

						//To draw a box around each character rect, uncomment the next 3 lines (helps dbug)
						//if (i == 0 || i == R[r].ResultRect.width - 1 || j == 0 || j == R[r].ResultRect.height - 1) {
						//	packedFontMap.SetPixel( (int) R[r].ResultRect.x + i, (int) R[r].ResultRect.y + j, Color.black );
						//}
					}
				}
			}
			packedFontMap.Apply();
			byte[] bytes = packedFontMap.EncodeToPNG();
			System.IO.File.WriteAllBytes( packedfontMapPath, bytes );
			SetOutFont();
			packedFont.material.mainTexture = (Texture2D) AssetDatabase.LoadAssetAtPath( packedfontMapPath, typeof( Texture2D ) );
			Debug.Log( "Pack Complete, Result texture size: " + packedFontMap.width + "x" + packedFontMap.height + " = " + packedFontMap.width * packedFontMap.height + " Pixels" );
			Debug.Log( "Pack fits inside " + packMaxSizeX + "x" + packMaxSizeY + " = " + packMaxSizeX * packMaxSizeY + " Pixels, and contains " + resultPackNodeList.Count + " characters" );
		}

		//Get, rotate if needed, and set the characterinfo from infont to outfont
		void SetOutFont () {
			CharacterInfo[] OutCharacterInfo = new CharacterInfo[nodes.Count];
			for (int i = 0; i < resultPackNodeList.Count; i++) {
				Rect newUvRect = new Rect( resultPackNodeList[i].ResultRect.x / packedFontMap.width, resultPackNodeList[i].ResultRect.y / packedFontMap.height, resultPackNodeList[i].ResultRect.width / packedFontMap.width, resultPackNodeList[i].ResultRect.height / packedFontMap.height );
				if (resultPackNodeList[i].SameOrient) {
					if (resultPackNodeList[i].CI.flipped) {
						newUvRect.y = newUvRect.y + newUvRect.height;
						newUvRect.height = newUvRect.height * -1;
					} else {
						//The rect hasnt been rotated, and wasnt originally flipped anyway.
					}
				} else {
					if (resultPackNodeList[i].CI.flipped) {
						resultPackNodeList[i].CI.flipped = false;
					} else {
						newUvRect.y = newUvRect.y + newUvRect.height;
						newUvRect.height = newUvRect.height * -1;
						resultPackNodeList[i].CI.flipped = true;
						resultPackNodeList[i].CI.vert.width = resultPackNodeList[i].ResultRect.height;
						resultPackNodeList[i].CI.vert.height = resultPackNodeList[i].ResultRect.width * -1;
					}
				}

				resultPackNodeList[i].CI.uv = newUvRect;
				OutCharacterInfo[resultPackNodeList[i].CIIndex] = resultPackNodeList[i].CI;
			}

			packedFont.characterInfo = OutCharacterInfo;
			EditorUtility.SetDirty( packedFont );
			EditorApplication.SaveAssets();
			AssetDatabase.Refresh();
		}

		//Checks given rect for overlap with all others, during pack, and if it fits into the pack area
		bool NodeIsFree ( Rect r1 ) {
			bool NoOverlap = true;
			for (int i = 0; i < resultPackNodeList.Count; i++) {
				Rect r2 = resultPackNodeList[i].ResultRect;
				if ((r1.xMin < r2.xMax) && (r1.xMax > r2.xMin) && (r1.yMin < r2.yMax) && (r1.yMax > r2.yMin)) {
					NoOverlap = false;
				}
			}

			if (r1.xMin < 0 || r1.xMax > packSizeX || r1.yMin < 0 || r1.yMax > packSizeY) {
				NoOverlap = false;
			}
			return NoOverlap;
		}


		#endregion
	}
}