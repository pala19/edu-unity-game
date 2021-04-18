
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Altom.AltUnityDriver;
using Altom.Editor.Logging;
using NLog;
using NLog.Layouts;
using UnityEditor.SceneManagement;

namespace Altom.Editor
{
    public class AltUnityTesterEditor : UnityEditor.EditorWindow
    {
        private static readonly Logger logger = EditorLogManager.Instance.GetCurrentClassLogger();

        public static bool NeedsRepaiting = false;
        public static AltUnityEditorConfiguration EditorConfiguration;
        public static AltUnityTesterEditor Window;
        public static int SelectedTest = -1;

        //TestResult after running a test
        public static bool IsTestRunResultAvailable = false;
        public static int ReportTestPassed;
        public static int ReportTestFailed;
        public static double TimeTestRan;

        public static List<AltUnityDevice> Devices = new List<AltUnityDevice>();

        UnityEngine.Object obj;
        private static UnityEngine.Texture2D passIcon;
        private static UnityEngine.Texture2D failIcon;
        private static UnityEngine.Texture2D downArrowIcon;
        private static UnityEngine.Texture2D upArrowIcon;
        private static UnityEngine.Texture2D xIcon;
        private static UnityEngine.Texture2D reloadIcon;

        private static UnityEngine.Color greenColor = new UnityEngine.Color(0.0f, 0.5f, 0.2f, 1f);
        private static UnityEngine.Color redColor = new UnityEngine.Color(0.7f, 0.15f, 0.15f, 1f);
        private static UnityEngine.Color selectedTestColor = new UnityEngine.Color(1f, 1f, 1f, 1f);
        private static UnityEngine.Color selectedTestColorDark = new UnityEngine.Color(0.6f, 0.6f, 0.6f, 1f);
        private static UnityEngine.Color oddNumberTestColor = new UnityEngine.Color(0.75f, 0.75f, 0.75f, 1f);
        private static UnityEngine.Color evenNumberTestColor = new UnityEngine.Color(0.7f, 0.7f, 0.7f, 1f);
        private static UnityEngine.Color oddNumberTestColorDark = new UnityEngine.Color(0.23f, 0.23f, 0.23f, 1f);
        private static UnityEngine.Color evenNumberTestColorDark = new UnityEngine.Color(0.25f, 0.25f, 0.25f, 1f);
        private static UnityEngine.Texture2D selectedTestTexture;
        private static UnityEngine.Texture2D oddNumberTestTexture;
        private static UnityEngine.Texture2D evenNumberTestTexture;
        public static UnityEngine.Texture2D PortForwardingTexture;

        private static string downloadURl;
        private const string RELEASENOTESURL = "https://altom.gitlab.io/altunity/altunityinspector/pages/release-notes.html#release-notes";
        private static string version;
        private static UnityEngine.GUIStyle gUIStyleButton;
        private static UnityEngine.GUIStyle gUIStyleText;
        private static UnityEngine.GUIStyle gUIStyleHistoryChanges;

        private static long timeSinceLastClick;
        private static UnityEngine.Networking.UnityWebRequest www;
        UnityEngine.Vector2 scrollPosition;
        private UnityEngine.Vector2 scrollPositonTestResult;

        private bool foldOutScenes = true;
        private bool foldOutBuildSettings = true;
        private bool foldOutLogSettings = true;
        private bool foldOutIosSettings = true;
        private bool foldOutAltUnityServerSettings = true;

        private static bool showPopUp = false;
        UnityEngine.Rect popUpPosition;
        UnityEngine.Rect popUpBorderPosition;
        UnityEngine.Rect closeButtonPosition;
        UnityEngine.Rect downloadButtonPosition;
        UnityEngine.Rect checkVersionChangesButtonPosition;

        private static UnityEngine.Font font;
        #region UnityEditor MenuItems
        // Add menu item named "My Window" to the Window menu
        [UnityEditor.MenuItem("AltUnity Tools/AltUnityTester", false, 80)]
        public static void ShowWindow()
        {
            //Show existing window instance. If one doesn't exist, make one.
            Window = (AltUnityTesterEditor)GetWindow(typeof(AltUnityTesterEditor));
            Window.minSize = new UnityEngine.Vector2(600, 100);
            Window.titleContent = new UnityEngine.GUIContent("AltUnity Tester Editor");
            Window.Show();
        }

        [UnityEditor.MenuItem("Assets/Create/AltUnityTest", true, 80)]
        public static bool CanCreateAltUnityTest()
        {
            return (getPathForSelectedItem() + "/").Contains("/Editor/");
        }

        [UnityEditor.MenuItem("Assets/Create/AltUnityTest", false, 80)]
        public static void CreateAltUnityTest()
        {
            var templatePath = UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("DefaultTestExample")[0]);
            string folderPath = getPathForSelectedItem();
            string newFilePath = System.IO.Path.Combine(folderPath, "NewAltUnityTest.cs");
#if UNITY_2019_1_OR_NEWER
            UnityEditor.ProjectWindowUtil.CreateScriptAssetFromTemplateFile(templatePath, newFilePath);
#else
            System.Reflection.MethodInfo method = typeof(UnityEditor.ProjectWindowUtil).GetMethod("CreateScriptAsset", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
            if (method == null)
                throw new Altom.AltUnityDriver.NotFoundException("Method to create Script file was not found");
            method.Invoke((object)null, new object[2]
            {
                (object) templatePath,
                (object) newFilePath
            });
#endif
        }

        [UnityEditor.MenuItem("AltUnity Tools/CreateAltUnityTesterPackage", false, 800)]
        public static void CreateAltUnityTesterPackage()
        {
            UnityEngine.Debug.Log("AltUnityTester - Unity Package creation started...");
            var version = AltUnityRunner.VERSION.Replace('.', '_');
            string packageName = "AltUnityTester_" + version + ".unitypackage";
            string assetPathNames = "Assets/AltUnityTester";
            UnityEditor.AssetDatabase.ExportPackage(assetPathNames, packageName, UnityEditor.ExportPackageOptions.Recurse);
            UnityEngine.Debug.Log("AltUnityTester - Unity Package done.");
        }

        private void Awake()
        {
            if (EditorConfiguration == null)
            {
                InitEditorConfiguration();
            }
            SendInspectorVersionRequest();
        }
        [UnityEditor.MenuItem("AltUnity Tools/AddAltIdToEveryObject", false, 800)]
        public static void AddIdComponentToEveryObjectInTheProject()
        {
            var rootObjects = new List<UnityEngine.GameObject>();
            UnityEngine.SceneManagement.Scene scene = EditorSceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);

            // iterate root objects and do something
            for (int i = 0; i < rootObjects.Count; ++i)
            {
                addComponentToObjectAndHisChildren(rootObjects[i]);
            }
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }


        [UnityEditor.MenuItem("AltUnity Tools/RemoveAltIdFromEveryObject", false, 800)]
        public static void RemoveIdComponentFromEveryObjectInTheProject()
        {
            var scenes = altUnityGetAllScenes();
            foreach (var scene in scenes)
            {
                EditorSceneManager.OpenScene(scene);
                removeComponentFromEveryObjectInTheScene();
            }
        }

        #endregion

        #region Unity Events
        protected void OnFocus()
        {
            if (EditorConfiguration == null)
            {
                InitEditorConfiguration();
            }

            if (!UnityEditor.AssetDatabase.IsValidFolder("Assets/Resources/AltUnityTester"))
            {
                AltUnityBuilder.CreateJsonFileForInputMappingOfAxis();
            }
            if (failIcon == null)
            {
                var findIcon = UnityEditor.AssetDatabase.FindAssets("16px-indicator-fail");
                failIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(UnityEditor.AssetDatabase.GUIDToAssetPath(findIcon[0]));
            }
            if (passIcon == null)
            {
                var findIcon = UnityEditor.AssetDatabase.FindAssets("16px-indicator-pass");
                passIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(UnityEditor.AssetDatabase.GUIDToAssetPath(findIcon[0]));
            }
            if (downArrowIcon == null)
            {
                var findIcon = UnityEditor.EditorGUIUtility.isProSkin ? UnityEditor.AssetDatabase.FindAssets("downArrowIcon") : UnityEditor.AssetDatabase.FindAssets("downArrowIconDark");
                downArrowIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(UnityEditor.AssetDatabase.GUIDToAssetPath(findIcon[0]));
            }
            if (upArrowIcon == null)
            {
                var findIcon = UnityEditor.EditorGUIUtility.isProSkin ? UnityEditor.AssetDatabase.FindAssets("upArrowIcon") : UnityEditor.AssetDatabase.FindAssets("upArrowIconDark");
                upArrowIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(UnityEditor.AssetDatabase.GUIDToAssetPath(findIcon[0]));
            }

            if (xIcon == null)
            {
                var findIcon = UnityEditor.EditorGUIUtility.isProSkin ? UnityEditor.AssetDatabase.FindAssets("xIcon") : UnityEditor.AssetDatabase.FindAssets("xIconDark");
                xIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(UnityEditor.AssetDatabase.GUIDToAssetPath(findIcon[0]));
            }
            if (reloadIcon == null)
            {
                var findIcon = UnityEditor.EditorGUIUtility.isProSkin ? UnityEditor.AssetDatabase.FindAssets("reloadIcon") : UnityEditor.AssetDatabase.FindAssets("reloadIconDark");
                reloadIcon = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Texture2D>(UnityEditor.AssetDatabase.GUIDToAssetPath(findIcon[0]));
            }

            if (selectedTestTexture == null)
            {
                selectedTestTexture = MakeTexture(20, 20, UnityEditor.EditorGUIUtility.isProSkin ? selectedTestColorDark : selectedTestColor);
            }
            if (evenNumberTestTexture == null)
            {
                evenNumberTestTexture = MakeTexture(20, 20, UnityEditor.EditorGUIUtility.isProSkin ? evenNumberTestColorDark : evenNumberTestColor);
            }
            if (oddNumberTestTexture == null)
            {
                oddNumberTestTexture = MakeTexture(20, 20, UnityEditor.EditorGUIUtility.isProSkin ? oddNumberTestColorDark : oddNumberTestColor);
            }
            if (PortForwardingTexture == null)
            {
                PortForwardingTexture = MakeTexture(20, 20, greenColor);
            }

            getListOfSceneFromEditor();
            AltUnityTestRunner.SetUpListTest();
        }
        protected void OnInspectorUpdate()
        {
            Repaint();
            if (IsTestRunResultAvailable)
            {
                IsTestRunResultAvailable = !UnityEditor.EditorUtility.DisplayDialog("Test Report",
                      " Total tests:" + (ReportTestFailed + ReportTestPassed) + System.Environment.NewLine + " Tests passed:" +
                      ReportTestPassed + System.Environment.NewLine + " Tests failed:" + ReportTestFailed + System.Environment.NewLine +
                      " Duration:" + TimeTestRan + " seconds", "Ok");
                ReportTestFailed = 0;
                ReportTestPassed = 0;
                TimeTestRan = 0;
            }
        }
        protected void OnGUI()
        {

            if (NeedsRepaiting)
            {
                NeedsRepaiting = false;
                Repaint();
            }

            if (UnityEngine.Application.isPlaying && !EditorConfiguration.RanInEditor)
            {
                EditorConfiguration.RanInEditor = true;
            }

            if (!UnityEngine.Application.isPlaying && EditorConfiguration.RanInEditor)
            {
                afterExitPlayMode();

            }
            DrawGUI();
        }
        protected void DrawGUI()
        {
            var screenWidth = UnityEditor.EditorGUIUtility.currentViewWidth;
            if (showPopUp)
            {
                popUpPosition = new UnityEngine.Rect(screenWidth / 2 - 300, 0, 600, 100);
                popUpBorderPosition = new UnityEngine.Rect(screenWidth / 2 - 298, 2, 596, 96);
                closeButtonPosition = new UnityEngine.Rect(popUpPosition.xMax - 20, popUpPosition.yMin + 5, 15, 15);
                downloadButtonPosition = new UnityEngine.Rect(popUpPosition.xMax - 200, popUpPosition.yMin + 30, 180, 30);
                checkVersionChangesButtonPosition = new UnityEngine.Rect(popUpPosition.xMax - 200, popUpPosition.yMin + 60, 180, 30);

                if (UnityEngine.Event.current.type == UnityEngine.EventType.MouseDown)
                {
                    if (checkVersionChangesButtonPosition.Contains(UnityEngine.Event.current.mousePosition))
                    {
                        UnityEngine.Application.OpenURL(RELEASENOTESURL);
                        UnityEngine.GUIUtility.ExitGUI();
                    }
                    if (downloadButtonPosition.Contains(UnityEngine.Event.current.mousePosition))
                    {
                        UnityEngine.Application.OpenURL(downloadURl);
                        UnityEngine.GUIUtility.ExitGUI();
                    }
                    if (closeButtonPosition.Contains(UnityEngine.Event.current.mousePosition))
                    {
                        showPopUp = false;
                        UnityEngine.GUIUtility.ExitGUI();
                    }
                }
            }
            //----------------------Left Panel------------
            UnityEditor.EditorGUILayout.BeginHorizontal();
            var leftSide = (screenWidth / 3) * 2;
            scrollPosition = UnityEditor.EditorGUILayout.BeginScrollView(scrollPosition, false, false, UnityEngine.GUILayout.MinWidth(leftSide));
            if (EditorConfiguration.MyTests != null)
            {
                displayTestGui(EditorConfiguration.MyTests);
            }

            UnityEditor.EditorGUILayout.Separator();

            displayBuildSettings();
            UnityEditor.EditorGUILayout.Separator();

            displayLogSettings();

            UnityEditor.EditorGUILayout.Separator();

            displayAltUnityServerSettings();
            UnityEditor.EditorGUILayout.Separator();

            displayPortForwarding(leftSide);


            UnityEditor.EditorGUILayout.EndScrollView();

            //-------------------Right Panel--------------
            var rightSide = (screenWidth / 3);
            UnityEditor.EditorGUILayout.BeginVertical();

            UnityEditor.EditorGUILayout.LabelField("Platform", UnityEditor.EditorStyles.boldLabel);

            if (rightSide <= 300)
            {
                UnityEditor.EditorGUILayout.BeginVertical();
                EditorConfiguration.platform = (AltUnityPlatform)UnityEngine.GUILayout.SelectionGrid((int)EditorConfiguration.platform, System.Enum.GetNames(typeof(AltUnityPlatform)), 1, UnityEditor.EditorStyles.radioButton);

                UnityEditor.EditorGUILayout.EndVertical();
            }
            else
            {
                UnityEditor.EditorGUILayout.BeginHorizontal();
                EditorConfiguration.platform = (AltUnityPlatform)UnityEngine.GUILayout.SelectionGrid((int)EditorConfiguration.platform, System.Enum.GetNames(typeof(AltUnityPlatform)), System.Enum.GetNames(typeof(AltUnityPlatform)).Length, UnityEditor.EditorStyles.radioButton);

                UnityEditor.EditorGUILayout.EndHorizontal();
            }


            if (EditorConfiguration.platform == AltUnityPlatform.Standalone)
            {
                UnityEditor.BuildTarget[] options = new UnityEditor.BuildTarget[]
                {
                UnityEditor.BuildTarget.StandaloneWindows, UnityEditor.BuildTarget.StandaloneWindows64,UnityEditor.BuildTarget.StandaloneOSX,UnityEditor.BuildTarget.StandaloneLinux64
                };

                int selected = System.Array.IndexOf(options, EditorConfiguration.StandaloneTarget);

                selected = UnityEditor.EditorGUILayout.Popup("Build Target", selected, options.ToList().ConvertAll(x => x.ToString()).ToArray());
                EditorConfiguration.StandaloneTarget = options[selected];
                browseBuildLocation();
            }


            if (EditorConfiguration.platform == AltUnityPlatform.Android)
            {
                browseBuildLocation();
                UnityEditor.EditorGUILayout.Separator();
                UnityEditor.EditorGUILayout.LabelField("Settings", UnityEditor.EditorStyles.boldLabel);

                if (UnityEngine.GUILayout.Button("Android player settings"))
                {
#if UNITY_2018_3_OR_NEWER
                    UnityEditor.SettingsService.OpenProjectSettings("Project/Player");
#else
                    UnityEditor.EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
#endif
                    UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup = UnityEditor.BuildTargetGroup.Android;
                }
            }


#if UNITY_EDITOR_OSX
            if (EditorConfiguration.platform == AltUnityPlatform.iOS)
            {
                browseBuildLocation();
                UnityEditor.EditorGUILayout.Separator();
                UnityEditor.EditorGUILayout.LabelField("Settings", UnityEditor.EditorStyles.boldLabel);

                if (UnityEngine.GUILayout.Button("iOS player settings"))
                {
                    UnityEditor.EditorGUILayout.Separator();
                    UnityEditor.EditorGUILayout.LabelField("Settings", UnityEditor.EditorStyles.boldLabel);

                    if (UnityEngine.GUILayout.Button("iOS player settings"))
                    {
#if UNITY_2018_3_OR_NEWER
                        UnityEditor.SettingsService.OpenProjectSettings("Project/Player");
#else
                    UnityEditor.EditorApplication.ExecuteMenuItem("Edit/Project Settings/Player");
#endif
                        UnityEditor.EditorUserBuildSettings.selectedBuildTargetGroup = UnityEditor.BuildTargetGroup.iOS;
                    }
                }
            }
#endif

            UnityEditor.EditorGUILayout.Separator();
            UnityEditor.EditorGUILayout.Separator();
            UnityEditor.EditorGUILayout.Separator();


            if (AltUnityBuilder.built)
            {
                var found = false;

                UnityEngine.SceneManagement.Scene scene = EditorSceneManager.OpenScene(AltUnityBuilder.GetFirstSceneWhichWillBeBuilt());
                if (scene.path.Equals(AltUnityBuilder.GetFirstSceneWhichWillBeBuilt()))
                {
                    if (scene.GetRootGameObjects()
                        .Any(gameObject => gameObject.name.Equals("AltUnityRunnerPrefab")))
                    {
                        UnityEngine.SceneManagement.SceneManager.SetActiveScene(scene);
                        var altunityRunner = scene.GetRootGameObjects()
                            .First(a => a.name.Equals("AltUnityRunnerPrefab"));
                        destroyAltUnityRunner(altunityRunner);
                        found = true;
                    }

                    if (found == false)
                        AltUnityBuilder.built = false;
                }

            }

            UnityEditor.EditorGUILayout.LabelField("Build", UnityEditor.EditorStyles.boldLabel);
            if (EditorConfiguration.platform != AltUnityPlatform.Editor)
            {
                if (UnityEngine.GUILayout.Button("Build Only"))
                {
                    if (EditorConfiguration.platform == AltUnityPlatform.Android)
                    {
                        AltUnityBuilder.BuildAndroidFromUI(autoRun: false);
                    }
#if UNITY_EDITOR_OSX
                    else if (EditorConfiguration.platform == AltUnityPlatform.iOS)
                    {
                        AltUnityBuilder.BuildiOSFromUI(autoRun: false);
                    }
#endif
                    else if (EditorConfiguration.platform == AltUnityPlatform.Standalone)
                    {
                        AltUnityBuilder.BuildStandaloneFromUI(EditorConfiguration.StandaloneTarget, autoRun: false);
                    }
                    else
                    {
                        runInEditor();
                    }
                    UnityEngine.GUIUtility.ExitGUI();

                }
            }
            else
            {
                UnityEditor.EditorGUI.BeginDisabledGroup(true);
                UnityEngine.GUILayout.Button("Build Only");
                UnityEditor.EditorGUI.EndDisabledGroup();
            }

            UnityEditor.EditorGUILayout.Separator();
            UnityEditor.EditorGUILayout.Separator();
            UnityEditor.EditorGUILayout.Separator();

            UnityEditor.EditorGUILayout.LabelField("Run", UnityEditor.EditorStyles.boldLabel);
            if (EditorConfiguration.platform == AltUnityPlatform.Editor)
            {
                if (UnityEngine.GUILayout.Button("Play in Editor"))
                {
                    runInEditor();
                }
            }
            else
            {
                UnityEditor.EditorGUI.BeginDisabledGroup(true);
                UnityEngine.GUILayout.Button("Play in Editor");
                UnityEditor.EditorGUI.EndDisabledGroup();
            }

            if (EditorConfiguration.platform != AltUnityPlatform.Editor)
            {
                if (UnityEngine.GUILayout.Button("Build & Run"))
                {
                    if (EditorConfiguration.platform == AltUnityPlatform.Android)
                    {
                        AltUnityBuilder.BuildAndroidFromUI(autoRun: true);
                    }
#if UNITY_EDITOR_OSX
                    else if (EditorConfiguration.platform == AltUnityPlatform.iOS)
                    {
                        AltUnityBuilder.BuildiOSFromUI(autoRun: true);
                    }
#endif
                    else if (EditorConfiguration.platform == AltUnityPlatform.Standalone)
                    {
                        AltUnityBuilder.BuildStandaloneFromUI(EditorConfiguration.StandaloneTarget, autoRun: true);
                    }
                    UnityEngine.GUIUtility.ExitGUI();
                }

            }
            else
            {
                UnityEditor.EditorGUI.BeginDisabledGroup(true);
                UnityEngine.GUILayout.Button("Build & Run", UnityEngine.GUILayout.MinWidth(50));
                UnityEditor.EditorGUI.EndDisabledGroup();
            }
            UnityEditor.EditorGUILayout.Separator();
            UnityEditor.EditorGUILayout.Separator();
            UnityEditor.EditorGUILayout.Separator();
            UnityEditor.EditorGUILayout.LabelField("Run tests", UnityEditor.EditorStyles.boldLabel);

            if (UnityEngine.GUILayout.Button("Run All Tests"))
            {
                if (EditorConfiguration.platform == AltUnityPlatform.Editor)
                {
                    var testThread = new Thread(() => AltUnityTestRunner.RunTests(AltUnityTestRunner.TestRunMode.RunAllTest));
                    testThread.Start();
                }
                else
                {

                    AltUnityTestRunner.RunTests(AltUnityTestRunner.TestRunMode.RunAllTest);
                }
            }
            if (UnityEngine.GUILayout.Button("Run Selected Tests"))
            {
                if (EditorConfiguration.platform == AltUnityPlatform.Editor)
                {
                    var testThread = new Thread(() => AltUnityTestRunner.RunTests(AltUnityTestRunner.TestRunMode.RunSelectedTest));
                    testThread.Start();
                }
                else
                {

                    AltUnityTestRunner.RunTests(AltUnityTestRunner.TestRunMode.RunSelectedTest);
                }
            }
            if (UnityEngine.GUILayout.Button("Run Failed Tests"))
            {
                if (EditorConfiguration.platform == AltUnityPlatform.Editor)
                {
                    var testThread = new Thread(() => AltUnityTestRunner.RunTests(AltUnityTestRunner.TestRunMode.RunFailedTest));
                    testThread.Start();
                }
                else
                {

                    AltUnityTestRunner.RunTests(AltUnityTestRunner.TestRunMode.RunFailedTest);
                }
            }
            //Status test

            scrollPositonTestResult = UnityEditor.EditorGUILayout.BeginScrollView(scrollPositonTestResult, UnityEngine.GUI.skin.textArea, UnityEngine.GUILayout.ExpandHeight(true));
            if (SelectedTest != -1)
            {
                var gUIStyle = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label)
                {
                    wordWrap = true,
                    richText = true,
                    alignment = UnityEngine.TextAnchor.MiddleCenter
                };
                var gUIStyle2 = new UnityEngine.GUIStyle();
                UnityEditor.EditorGUILayout.LabelField("<b>" + EditorConfiguration.MyTests[SelectedTest].TestName + "</b>", gUIStyle);


                UnityEditor.EditorGUILayout.Separator();
                string textToDisplayForMessage;
                if (EditorConfiguration.MyTests[SelectedTest].Status == 0)
                {
                    textToDisplayForMessage = "No informartion about this test available.\nPlease rerun the test.";
                    UnityEditor.EditorGUILayout.LabelField(textToDisplayForMessage, gUIStyle, UnityEngine.GUILayout.MinWidth(30));
                }
                else
                {
                    gUIStyle = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label)
                    {
                        wordWrap = true,
                        richText = true
                    };

                    string status = "";
                    switch (EditorConfiguration.MyTests[SelectedTest].Status)
                    {
                        case 1:
                            status = "<color=green>Passed</color>";
                            break;
                        case -1:
                            status = "<color=red>Failed</color>";
                            break;

                    }

                    UnityEngine.GUILayout.BeginHorizontal();
                    UnityEditor.EditorGUILayout.LabelField("<b>Time</b>", gUIStyle, UnityEngine.GUILayout.MinWidth(30));
                    UnityEditor.EditorGUILayout.LabelField(EditorConfiguration.MyTests[SelectedTest].TestDuration.ToString(), gUIStyle, UnityEngine.GUILayout.MinWidth(100));
                    UnityEngine.GUILayout.EndHorizontal();

                    UnityEngine.GUILayout.BeginHorizontal();
                    UnityEditor.EditorGUILayout.LabelField("<b>Status</b>", gUIStyle, UnityEngine.GUILayout.MinWidth(30));
                    UnityEditor.EditorGUILayout.LabelField(status, gUIStyle, UnityEngine.GUILayout.MinWidth(100));
                    UnityEngine.GUILayout.EndHorizontal();
                    if (EditorConfiguration.MyTests[SelectedTest].Status == -1)
                    {
                        UnityEngine.GUILayout.BeginHorizontal();
                        UnityEditor.EditorGUILayout.LabelField("<b>Message</b>", gUIStyle, UnityEngine.GUILayout.MinWidth(30));
                        UnityEditor.EditorGUILayout.LabelField(EditorConfiguration.MyTests[SelectedTest].TestResultMessage, gUIStyle, UnityEngine.GUILayout.MinWidth(100));
                        UnityEngine.GUILayout.EndHorizontal();

                        UnityEngine.GUILayout.BeginHorizontal();
                        UnityEditor.EditorGUILayout.LabelField("<b>StackTrace</b>", gUIStyle, UnityEngine.GUILayout.MinWidth(30));
                        UnityEditor.EditorGUILayout.LabelField(EditorConfiguration.MyTests[SelectedTest].TestStackTrace, gUIStyle, UnityEngine.GUILayout.MinWidth(100));
                        UnityEngine.GUILayout.EndHorizontal();
                    }
                }
            }
            else
            {
                UnityEditor.EditorGUILayout.LabelField("No test selected");
            }
            UnityEditor.EditorGUILayout.EndScrollView();
            UnityEditor.EditorGUILayout.EndVertical();
            UnityEditor.EditorGUILayout.EndHorizontal();
            //PopUp
            if (showPopUp)
            {
                ShowAltUnityPopup();
            }
        }

        private void ShowAltUnityPopup()
        {
            if (font == null)
            {
                font = UnityEngine.Font.CreateDynamicFontFromOSFont("Arial", 16);
            }
            if (gUIStyleText == null)
            {
                gUIStyleText = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label)
                {
                    wordWrap = true,
                    richText = true,
                    alignment = UnityEngine.TextAnchor.MiddleLeft,
                    font = font
                };
            }
            if (gUIStyleButton == null)
            {
                gUIStyleButton = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.button)
                {
                    wordWrap = true,
                    richText = true,
                    alignment = UnityEngine.TextAnchor.MiddleCenter,
                    font = font
                };
                gUIStyleButton.normal.background = AltUnityTesterEditor.PortForwardingTexture;
                gUIStyleButton.normal.textColor = UnityEngine.Color.white;
            }
            if (gUIStyleHistoryChanges == null)
            {
                gUIStyleHistoryChanges = new UnityEngine.GUIStyle(UnityEngine.GUI.skin.label)
                {
                    wordWrap = true,
                    richText = true,
                    alignment = UnityEngine.TextAnchor.MiddleCenter,
                    font = font
                };
            }


            UnityEngine.GUI.DrawTexture(popUpPosition, evenNumberTestTexture);
            UnityEngine.GUI.DrawTexture(popUpBorderPosition, oddNumberTestTexture);

            UnityEngine.GUI.Button(closeButtonPosition, "<size=10>X</size>", gUIStyleHistoryChanges);
            UnityEngine.GUI.Button(downloadButtonPosition, "<b><size=16>Download now!</size></b>", gUIStyleButton);
            UnityEngine.GUI.Button(checkVersionChangesButtonPosition, "<size=13>Check version history</size>", gUIStyleHistoryChanges);
            UnityEditor.EditorGUI.LabelField(checkVersionChangesButtonPosition, "<size=13>__________________</size>", gUIStyleHistoryChanges);

            UnityEngine.Rect textPosition = new UnityEngine.Rect(popUpPosition.xMin + 20, popUpPosition.yMin + 30, 370, 30);
            UnityEditor.EditorGUI.LabelField(textPosition, System.String.Format("<b><size=16>AltUnity Inspector {0} has been released!</size></b>", version), gUIStyleText);
        }

        private static void SendInspectorVersionRequest()
        {
            www = UnityEngine.Networking.UnityWebRequest.Get("https://altom.com/altunity-inspector-versions/?id=unityeditor");
            var wwwOp = www.SendWebRequest();
            UnityEditor.EditorApplication.update += CheckInspectorVersionRequest;

        }
        public static void CheckInspectorVersionRequest()
        {
            if (!www.isDone)
                return;

            if (www.isNetworkError)
            {
                UnityEngine.Debug.Log(www.error);
            }
            else
            {
                if (www.responseCode == 200)
                {
                    string textReceived = www.downloadHandler.text;
                    System.Text.RegularExpressions.Regex regex = null;
                    if (UnityEngine.SystemInfo.operatingSystemFamily == UnityEngine.OperatingSystemFamily.Windows)
                    {
                        regex = new System.Text.RegularExpressions.Regex(@"https://altom.com/app/uploads/altunityinspector/AltUnityInspector.*\.exe");

                    }
                    else if (UnityEngine.SystemInfo.operatingSystemFamily == UnityEngine.OperatingSystemFamily.MacOSX)
                    {
                        regex = new System.Text.RegularExpressions.Regex(@"https://altom.com/app/uploads/altunityinspector/AltUnityInspector.*\.dmg");
                    }

                    System.Text.RegularExpressions.Match match = regex.Match(textReceived);
                    if (match.Success)
                    {

                        var splitedText = match.Value.Split('_');
                        var releasedVersion = splitedText[2].Substring(1);
                        if (String.IsNullOrEmpty(EditorConfiguration.LatestInspectorVersion) || !isCurrentVersionEqualOrNewer(releasedVersion, EditorConfiguration.LatestInspectorVersion))
                        {
                            EditorConfiguration.LatestInspectorVersion = releasedVersion;
                            downloadURl = match.Value;
                            version = releasedVersion;
                            showPopUp = true;
                        }
                    }
                }

                UnityEditor.EditorApplication.update -= CheckInspectorVersionRequest;
            }
        }
        private static bool isCurrentVersionEqualOrNewer(string releasedVersion, string version)
        {
            var releasedVersionSplited = releasedVersion.Split('.');
            var currentVersionSplited = version.Split('.');
            if (Int16.Parse(currentVersionSplited[0]) != Int16.Parse(releasedVersionSplited[0]))//check major number
            {
                if (Int16.Parse(currentVersionSplited[0]) < Int16.Parse(releasedVersionSplited[0]))
                    return false;
                else
                    return true;
            }
            if (Int16.Parse(currentVersionSplited[1]) != Int16.Parse(releasedVersionSplited[1]))//check minor number
            {
                if (Int16.Parse(currentVersionSplited[1]) < Int16.Parse(releasedVersionSplited[1]))
                    return false;
                else
                    return true;
            }
            if (Int16.Parse(currentVersionSplited[2]) < Int16.Parse(releasedVersionSplited[2]))//check patch number
                return false;
            return true;
        }
        #endregion

        #region public methods
        public static void InitEditorConfiguration()
        {
            if (UnityEditor.AssetDatabase.FindAssets("AltUnityTesterEditorSettings").Length == 0)
            {
                var altUnityEditorFolderPath = UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("AltUnityTesterEditor")[0]);
                altUnityEditorFolderPath = altUnityEditorFolderPath.Substring(0, altUnityEditorFolderPath.Length - 24);
                EditorConfiguration = CreateInstance<AltUnityEditorConfiguration>();
                EditorConfiguration.MyTests = null;
                UnityEditor.AssetDatabase.CreateAsset(EditorConfiguration, altUnityEditorFolderPath + "/AltUnityTesterEditorSettings.asset");
                UnityEditor.AssetDatabase.SaveAssets();
            }
            else
            {
                EditorConfiguration = UnityEditor.AssetDatabase.LoadAssetAtPath<AltUnityEditorConfiguration>(
                    UnityEditor.AssetDatabase.GUIDToAssetPath(UnityEditor.AssetDatabase.FindAssets("AltUnityTesterEditorSettings")[0]));
            }
            UnityEditor.EditorUtility.SetDirty(EditorConfiguration);
        }

        public static UnityEngine.Texture2D MakeTexture(int width, int height, UnityEngine.Color col)
        {
            UnityEngine.Color[] pix = new UnityEngine.Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            var result = new UnityEngine.Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        public static void AddAllScenes()
        {
            var scenesToBeAddedGuid = UnityEditor.AssetDatabase.FindAssets("t:SceneAsset");
            EditorConfiguration.Scenes = new System.Collections.Generic.List<AltUnityMyScenes>();
            foreach (var sceneGuid in scenesToBeAddedGuid)
            {
                var scenePath = UnityEditor.AssetDatabase.GUIDToAssetPath(sceneGuid);
                EditorConfiguration.Scenes.Add(new AltUnityMyScenes(false, scenePath, 0));

            }

            UnityEditor.EditorBuildSettings.scenes = pathFromTheSceneInCurrentList();
        }

        public static void SelectAllScenes()
        {
            foreach (var scene in EditorConfiguration.Scenes)
            {
                scene.ToBeBuilt = true;
            }
            UnityEditor.EditorBuildSettings.scenes = pathFromTheSceneInCurrentList();
        }

        #endregion
        private static void swap(int index1, int index2)
        {
            AltUnityMyScenes backUp = EditorConfiguration.Scenes[index1];
            EditorConfiguration.Scenes[index1] = EditorConfiguration.Scenes[index2];
            EditorConfiguration.Scenes[index2] = backUp;
        }
        private static void browseBuildLocation()
        {
            UnityEngine.GUILayout.BeginHorizontal();
            EditorConfiguration.BuildLocationPath = UnityEditor.EditorGUILayout.TextField("Build Location", EditorConfiguration.BuildLocationPath, UnityEngine.GUILayout.MaxWidth(300));
            UnityEngine.GUI.SetNextControlName("Browse");
            if (UnityEngine.GUILayout.Button("Browse", UnityEngine.GUILayout.MaxHeight(15)))
            {
                EditorConfiguration.BuildLocationPath = UnityEditor.EditorUtility.OpenFolderPanel("Select Build Location", "", "");
                UnityEngine.GUI.FocusControl("Browse");
            }
            UnityEngine.GUILayout.EndHorizontal();
        }

        private void getListOfSceneFromEditor()
        {
            var newSceneses = new List<AltUnityMyScenes>();
            foreach (var scene in UnityEditor.EditorBuildSettings.scenes)
            {
                newSceneses.Add(new AltUnityMyScenes(scene.enabled, scene.path, 0));
            }

            EditorConfiguration.Scenes = newSceneses;
        }

        private void displayPortForwarding(float widthColumn)
        {
            foldOutScenes = UnityEditor.EditorGUILayout.Foldout(foldOutScenes, "Port Forwarding");
            var guiStyleBolded = setTextGuiStyle();
            guiStyleBolded.fontStyle = UnityEngine.FontStyle.Bold;

            var guiStyleNormal = setTextGuiStyle();

            UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", UnityEngine.GUILayout.MaxWidth(30));
            UnityEditor.EditorGUILayout.BeginVertical();
            widthColumn -= 30;
            if (foldOutScenes)
            {
                UnityEngine.GUILayout.BeginVertical(UnityEngine.GUI.skin.textField, UnityEngine.GUILayout.MaxHeight(30));
                UnityEngine.GUILayout.BeginHorizontal();
                UnityEngine.GUILayout.Label("DeviceId", guiStyleBolded, UnityEngine.GUILayout.Width(widthColumn / 2), UnityEngine.GUILayout.ExpandWidth(true));
                UnityEngine.GUILayout.FlexibleSpace();
                UnityEngine.GUILayout.Label("Local Port", guiStyleBolded, UnityEngine.GUILayout.Width(widthColumn / 7));
                UnityEngine.GUILayout.Label("Remote Port", guiStyleBolded, UnityEngine.GUILayout.Width(widthColumn / 7));

                UnityEngine.GUILayout.BeginHorizontal();
                if (UnityEngine.GUILayout.Button(reloadIcon, UnityEngine.GUILayout.Width(widthColumn / 10)))
                {
                    refreshDeviceList();
                }
                UnityEngine.GUILayout.EndHorizontal();
                UnityEngine.GUILayout.EndHorizontal();

                if (Devices.Count != 0)
                {
                    foreach (var device in Devices)
                    {
                        if (device.Active)
                        {
                            var styleActive = new UnityEngine.GUIStyle();
                            styleActive.normal.background = PortForwardingTexture;

                            UnityEngine.GUILayout.BeginHorizontal(styleActive);
                            UnityEngine.GUILayout.Label(device.DeviceId, guiStyleNormal, UnityEngine.GUILayout.Width(widthColumn / 2), UnityEngine.GUILayout.ExpandWidth(true));
                            UnityEngine.GUILayout.Label(device.LocalPort.ToString(), guiStyleNormal, UnityEngine.GUILayout.Width(widthColumn / 7));
                            UnityEngine.GUILayout.Label(device.RemotePort.ToString(), guiStyleNormal, UnityEngine.GUILayout.Width(widthColumn / 7));
                            if (UnityEngine.GUILayout.Button("Stop", UnityEngine.GUILayout.Width(widthColumn / 10), UnityEngine.GUILayout.Height(15)))
                            {
                                if (device.Platform == "Android")
                                {
                                    AltUnityPortForwarding.RemoveForwardAndroid(device.LocalPort, device.DeviceId, EditorConfiguration.AdbPath);
                                }
#if UNITY_EDITOR_OSX
                                else
                                {
                                    AltUnityPortForwarding.KillIProxy(device.Pid);
                                }
#endif
                                device.Active = false;
                                refreshDeviceList();
                            }
                        }
                        else
                        {
                            UnityEngine.GUILayout.BeginHorizontal();
                            UnityEngine.GUILayout.Label(device.DeviceId, guiStyleNormal, UnityEngine.GUILayout.Width(widthColumn / 2), UnityEngine.GUILayout.ExpandWidth(true));
                            device.LocalPort = UnityEditor.EditorGUILayout.IntField(device.LocalPort, UnityEngine.GUILayout.Width(widthColumn / 7));
                            device.RemotePort = UnityEditor.EditorGUILayout.IntField(device.RemotePort, UnityEngine.GUILayout.Width(widthColumn / 7));
                            if (UnityEngine.GUILayout.Button("Start", UnityEngine.GUILayout.Width(widthColumn / 10), UnityEngine.GUILayout.MaxHeight(15)))
                            {
                                if (device.Platform == "Android")
                                {
                                    var response = AltUnityPortForwarding.ForwardAndroid(device.LocalPort, device.RemotePort, device.DeviceId, EditorConfiguration.AdbPath);
                                    if (!response.Equals("Ok"))
                                    {
                                        logger.Error(response);
                                    }
                                }
#if UNITY_EDITOR_OSX
                                else
                                {
                                    var response = AltUnityPortForwarding.ForwardIos(device.LocalPort, device.RemotePort, device.DeviceId, EditorConfiguration.IProxyPath);
                                    if (response.StartsWith("Ok"))
                                    {
                                        var processID = int.Parse(response.Split(' ')[1]);
                                        device.Active = true;
                                        device.Pid = processID;
                                    }
                                    else
                                    {
                                        logger.Error(response);
                                    }

                                }
#endif
                                refreshDeviceList();
                            }
                        }

                        UnityEngine.GUILayout.EndHorizontal();
                    }
                }
                else
                {
                    UnityEditor.EditorGUILayout.LabelField("No devices connected. Click \"refresh\" button to search for devices", guiStyleNormal);
                }
                UnityEngine.GUILayout.EndVertical();
            }

            UnityEditor.EditorGUILayout.EndVertical();
            UnityEditor.EditorGUILayout.EndHorizontal();
        }

        private void refreshDeviceList()
        {
            List<AltUnityDevice> adbDevices = AltUnityPortForwarding.GetDevicesAndroid(EditorConfiguration.AdbPath);
            List<AltUnityDevice> androidForwardedDevices = AltUnityPortForwarding.GetForwardedDevicesAndroid(EditorConfiguration.AdbPath);
            foreach (var adbDevice in adbDevices)
            {
                var deviceForwarded = androidForwardedDevices.FirstOrDefault(device => device.DeviceId.Equals(adbDevice.DeviceId));
                if (deviceForwarded != null)
                {
                    adbDevice.LocalPort = deviceForwarded.LocalPort;
                    adbDevice.RemotePort = deviceForwarded.RemotePort;
                    adbDevice.Active = deviceForwarded.Active;
                }
            }
#if UNITY_EDITOR_OSX
            var iOSDEvices = AltUnityPortForwarding.GetConnectediOSDevices(EditorConfiguration.XcrunPath);
            var iOSForwardedDevices = AltUnityPortForwarding.GetForwardediOSDevices();
            foreach (var iOSDEvice in iOSDEvices)
            {
                var deviceForwarded = iOSForwardedDevices.FirstOrDefault(device => device.DeviceId.Equals(iOSDEvice.DeviceId));
                if (deviceForwarded != null)
                {
                    iOSDEvice.LocalPort = deviceForwarded.LocalPort;
                    iOSDEvice.RemotePort = deviceForwarded.RemotePort;
                    iOSDEvice.Active = deviceForwarded.Active;
                    iOSDEvice.Pid = deviceForwarded.Pid;
                }
            }
#endif

            Devices = adbDevices;
#if UNITY_EDITOR_OSX
            Devices.AddRange(iOSDEvices);
#endif
        }

        private void displayAltUnityServerSettings()
        {
            foldOutAltUnityServerSettings = UnityEditor.EditorGUILayout.Foldout(foldOutAltUnityServerSettings, "AltUnityServer Settings");
            if (foldOutAltUnityServerSettings)
            {
                labelAndInputFieldHorizontalLayout("Request separator", ref EditorConfiguration.RequestSeparator);
                labelAndInputFieldHorizontalLayout("Request ending", ref EditorConfiguration.RequestEnding);
                labelAndInputFieldHorizontalLayout("Server port", ref EditorConfiguration.ServerPort);
            }
        }

        private void afterExitPlayMode()
        {
            removeAltUnityRunnerPrefab();
            AltUnityBuilder.RemoveAltUnityTesterFromScriptingDefineSymbols(UnityEditor.BuildPipeline.GetBuildTargetGroup(UnityEditor.EditorUserBuildSettings.activeBuildTarget));
            EditorConfiguration.RanInEditor = false;
        }

        private static void removeAltUnityRunnerPrefab()
        {
            var activeScene = EditorSceneManager.GetActiveScene();
            var altUnityRunners = activeScene.GetRootGameObjects()
                .Where(gameObject => gameObject.name.Equals("AltUnityRunnerPrefab")).ToList();
            if (altUnityRunners.Count != 0)
            {
                foreach (var altUnityRunner in altUnityRunners)
                {
                    DestroyImmediate(altUnityRunner);

                }
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
                EditorSceneManager.SaveOpenScenes();
            }
        }

        private void runInEditor()
        {
            AltUnityBuilder.InsertAltUnityInTheActiveScene();
            AltUnityBuilder.CreateJsonFileForInputMappingOfAxis();
            AltUnityBuilder.AddAltUnityTesterInScritpingDefineSymbolsGroup(UnityEditor.BuildPipeline.GetBuildTargetGroup(UnityEditor.EditorUserBuildSettings.activeBuildTarget));
            UnityEditor.EditorApplication.isPlaying = true;
        }

        private void displayBuildSettings()
        {
            foldOutBuildSettings = UnityEditor.EditorGUILayout.Foldout(foldOutBuildSettings, "Build Settings");
            if (foldOutBuildSettings)
            {
                var companyName = UnityEditor.PlayerSettings.companyName;
                labelAndInputFieldHorizontalLayout("Company Name", ref companyName);
                UnityEditor.PlayerSettings.companyName = companyName;

                var productName = UnityEditor.PlayerSettings.productName;
                labelAndInputFieldHorizontalLayout("Product Name", ref productName);
                UnityEditor.PlayerSettings.productName = productName;

                labelAndCheckboxHorizontalLayout("Input visualizer:", ref EditorConfiguration.InputVisualizer);
                labelAndCheckboxHorizontalLayout("Show popup", ref EditorConfiguration.ShowPopUp);
                labelAndCheckboxHorizontalLayout("Append \"Test\" to product name for AltUnityTester builds:", ref EditorConfiguration.appendToName);

                switch (EditorConfiguration.platform)
                {
                    case AltUnityPlatform.Android:
                        foldOutIosSettings = UnityEditor.EditorGUILayout.Foldout(foldOutIosSettings, "Android Settings");
                        if (foldOutIosSettings)
                        {
                            string androidBundleIdentifier = UnityEditor.PlayerSettings.GetApplicationIdentifier(UnityEditor.BuildTargetGroup.Android);
                            labelAndInputFieldHorizontalLayout("Android Bundle Identifier", ref androidBundleIdentifier);
                            if (androidBundleIdentifier != UnityEditor.PlayerSettings.GetApplicationIdentifier(UnityEditor.BuildTargetGroup.Android))
                            {
                                UnityEditor.PlayerSettings.SetApplicationIdentifier(UnityEditor.BuildTargetGroup.Android, androidBundleIdentifier);
                            }
                            labelAndInputFieldHorizontalLayout("Adb Path:", ref EditorConfiguration.AdbPath);
                        }
                        break;
                    case AltUnityPlatform.Editor:
                        break;
                    case AltUnityPlatform.Standalone:
                        break;
#if UNITY_EDITOR_OSX
                    case AltUnityPlatform.iOS:
                        foldOutIosSettings = UnityEditor.EditorGUILayout.Foldout(foldOutIosSettings, "iOS Settings");
                        if (foldOutIosSettings)
                        {
                            string iOSBundleIdentifier = UnityEditor.PlayerSettings.GetApplicationIdentifier(UnityEditor.BuildTargetGroup.iOS);
                            labelAndInputFieldHorizontalLayout("iOS Bundle Identifier", ref iOSBundleIdentifier);
                            if (iOSBundleIdentifier != UnityEditor.PlayerSettings.GetApplicationIdentifier(UnityEditor.BuildTargetGroup.iOS))
                            {
                                UnityEditor.PlayerSettings.SetApplicationIdentifier(UnityEditor.BuildTargetGroup.iOS, iOSBundleIdentifier);
                            }

                            var appleDevoleperTeamID = UnityEditor.PlayerSettings.iOS.appleDeveloperTeamID;
                            labelAndInputFieldHorizontalLayout("Signing Team Id: ", ref appleDevoleperTeamID);
                            UnityEditor.PlayerSettings.iOS.appleDeveloperTeamID = appleDevoleperTeamID;

                            var appleEnableAutomaticsSigning = UnityEditor.PlayerSettings.iOS.appleEnableAutomaticSigning;
                            labelAndCheckboxHorizontalLayout("Automatically Sign: ", ref appleEnableAutomaticsSigning);
                            UnityEditor.PlayerSettings.iOS.appleEnableAutomaticSigning = appleEnableAutomaticsSigning;

                            labelAndInputFieldHorizontalLayout("Iproxy Path: ", ref EditorConfiguration.IProxyPath);
                            labelAndInputFieldHorizontalLayout("Xcrun Path: ", ref EditorConfiguration.XcrunPath);
                        }
                        break;
#endif
                }

                displayScenes();
            }
        }

        private void displayLogSettings()
        {
            foldOutLogSettings = UnityEditor.EditorGUILayout.Foldout(foldOutLogSettings, "Log Settings");
            if (foldOutLogSettings)
            {
                labelAndInputFieldHorizontalLayout("Max Length (Optional)", ref EditorConfiguration.MaxLogLength);
                if (string.IsNullOrEmpty(EditorConfiguration.MaxLogLength))
                {
                    EditorConfiguration.MaxLogLength = "";
                }
                else
                {
                    try
                    {
                        var maxLogLength = int.Parse(EditorConfiguration.MaxLogLength);
                        if (maxLogLength < 100)
                        {
                            EditorConfiguration.MaxLogLength = "100";
                        }
                    }
                    catch
                    {
                        EditorConfiguration.MaxLogLength = "100";
                    }
                }
            }
        }

        private static void labelAndCheckboxHorizontalLayout(string label, ref bool editorConfigVariable)
        {
            UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", UnityEngine.GUILayout.MaxWidth(30));
            UnityEditor.EditorGUILayout.LabelField(label, UnityEngine.GUILayout.Width(145));
            editorConfigVariable =
                UnityEditor.EditorGUILayout.Toggle(editorConfigVariable, UnityEngine.GUILayout.MaxWidth(30));
            UnityEngine.GUILayout.FlexibleSpace();
            UnityEditor.EditorGUILayout.EndHorizontal();
        }

        private static void labelAndInputFieldHorizontalLayout(string labelText, ref string editorConfigVariable)
        {
            UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", UnityEngine.GUILayout.MaxWidth(30));
            editorConfigVariable = UnityEditor.EditorGUILayout.TextField(labelText, editorConfigVariable);
            UnityEditor.EditorGUILayout.EndHorizontal();
        }

        private static void labelAndInputFieldHorizontalLayout(string labelText, ref int editorConfigVariable)
        {
            UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", UnityEngine.GUILayout.MaxWidth(30));
            editorConfigVariable = UnityEditor.EditorGUILayout.IntField(labelText, editorConfigVariable);
            UnityEditor.EditorGUILayout.EndHorizontal();
        }

        private void displayScenes()
        {
            foldOutScenes = UnityEditor.EditorGUILayout.Foldout(foldOutScenes, "SceneManager");
            UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.LabelField("", UnityEngine.GUILayout.MaxWidth(30));
            UnityEditor.EditorGUILayout.BeginVertical();
            UnityEngine.GUIStyle guiStyle = setTextGuiStyle();
            if (foldOutScenes)
            {
                if (EditorConfiguration.Scenes.Count != 0)
                {
                    UnityEditor.EditorGUILayout.BeginHorizontal();
                    UnityEditor.EditorGUILayout.LabelField("Display scene full path:", UnityEngine.GUILayout.Width(140), UnityEngine.GUILayout.ExpandWidth(false));
                    EditorConfiguration.ScenePathDisplayed = UnityEditor.EditorGUILayout.Toggle(EditorConfiguration.ScenePathDisplayed, UnityEngine.GUILayout.ExpandWidth(false), UnityEngine.GUILayout.Width(15));
                    UnityEngine.GUILayout.FlexibleSpace();


                    UnityEditor.EditorGUILayout.EndHorizontal();
                    UnityEngine.GUILayout.BeginVertical(UnityEngine.GUI.skin.textField);
                    AltUnityMyScenes sceneToBeRemoved = null;
                    int counter = 0;
                    foreach (var scene in EditorConfiguration.Scenes)
                    {
                        UnityEngine.GUILayout.BeginHorizontal(UnityEngine.GUI.skin.textArea);

                        var valToggle = UnityEditor.EditorGUILayout.Toggle(scene.ToBeBuilt, UnityEngine.GUILayout.MaxWidth(15));
                        if (valToggle != scene.ToBeBuilt)
                        {
                            scene.ToBeBuilt = valToggle;
                            UnityEditor.EditorBuildSettings.scenes = pathFromTheSceneInCurrentList();
                        }
                        var sceneName = scene.Path;
                        if (!EditorConfiguration.ScenePathDisplayed)
                        {
                            var splitedPath = sceneName.Split('/');
                            sceneName = splitedPath[splitedPath.Length - 1];
                        }

                        UnityEditor.EditorGUILayout.LabelField(sceneName, guiStyle);
                        UnityEngine.GUILayout.FlexibleSpace();
                        string value;
                        if (scene.ToBeBuilt)
                        {
                            scene.BuildScene = counter;
                            counter++;
                            value = scene.BuildScene.ToString();
                        }
                        else
                        {
                            value = "";
                        }
                        var buttonWidth = 20;
                        UnityEditor.EditorGUILayout.LabelField(value, guiStyle, UnityEngine.GUILayout.MaxWidth(buttonWidth));


                        if (EditorConfiguration.Scenes.IndexOf(scene) != 0 && EditorConfiguration.Scenes.Count > 1)
                        {

                            if (UnityEngine.GUILayout.Button(upArrowIcon, UnityEngine.GUILayout.MaxWidth(buttonWidth)))
                            {
                                sceneMove(scene, true);
                                UnityEditor.EditorBuildSettings.scenes = pathFromTheSceneInCurrentList();
                            }
                        }
                        else
                        {
                            UnityEditor.EditorGUILayout.LabelField("", UnityEngine.GUILayout.MaxWidth(buttonWidth));
                        }

                        if (EditorConfiguration.Scenes.IndexOf(scene) != EditorConfiguration.Scenes.Count - 1 && EditorConfiguration.Scenes.Count > 1)
                        {
                            if (UnityEngine.GUILayout.Button(downArrowIcon, UnityEngine.GUILayout.MaxWidth(buttonWidth)))
                            {
                                sceneMove(scene, false);
                                UnityEditor.EditorBuildSettings.scenes = pathFromTheSceneInCurrentList();
                            }
                        }
                        else
                        {
                            UnityEditor.EditorGUILayout.LabelField("", UnityEngine.GUILayout.MaxWidth(buttonWidth));
                        }


                        if (UnityEngine.GUILayout.Button(xIcon, UnityEngine.GUILayout.MaxWidth(buttonWidth)))
                        {
                            sceneToBeRemoved = scene;
                        }

                        UnityEngine.GUILayout.EndHorizontal();

                    }


                    if (sceneToBeRemoved != null)
                    {
                        removeScene(sceneToBeRemoved);
                    }

                    UnityEngine.GUILayout.EndVertical();
                }

                UnityEngine.GUILayout.BeginVertical();
                UnityEditor.EditorGUILayout.BeginHorizontal();
                UnityEditor.EditorGUILayout.LabelField("Add scene: ", UnityEngine.GUILayout.MaxWidth(80));
                obj = UnityEditor.EditorGUILayout.ObjectField(obj, typeof(UnityEditor.SceneAsset), true);

                if (obj != null)
                {
                    var path = UnityEditor.AssetDatabase.GetAssetPath(obj);
                    if (EditorConfiguration.Scenes.All(n => n.Path != path))
                    {
                        EditorConfiguration.Scenes.Add(new AltUnityMyScenes(false, path, 0));
                        obj = new UnityEngine.Object();
                    }

                    obj = null;
                }
                UnityEditor.EditorGUILayout.EndHorizontal();

                if (UnityEditor.EditorGUIUtility.currentViewWidth / 3 * 2 > 700)//All scene button in one line
                {
                    UnityEditor.EditorGUILayout.BeginHorizontal();
                    if (UnityEngine.GUILayout.Button("Add all scenes", UnityEditor.EditorStyles.miniButtonLeft, UnityEngine.GUILayout.MinWidth(30)))
                    {
                        AddAllScenes();
                    }

                    if (UnityEngine.GUILayout.Button("Select all scenes", UnityEditor.EditorStyles.miniButtonMid, UnityEngine.GUILayout.MinWidth(30)))
                    {
                        SelectAllScenes();
                    }
                    if (UnityEngine.GUILayout.Button("Deselect all scenes", UnityEditor.EditorStyles.miniButtonMid, UnityEngine.GUILayout.MinWidth(30)))
                    {
                        deselectAllScenes();
                    }
                    if (UnityEngine.GUILayout.Button("Remove unselected scenes", UnityEditor.EditorStyles.miniButtonMid, UnityEngine.GUILayout.MinWidth(30)))
                    {
                        removeNotSelectedScenes();
                    }
                    if (UnityEngine.GUILayout.Button("Remove all scenes", UnityEditor.EditorStyles.miniButtonRight, UnityEngine.GUILayout.MinWidth(30)))
                    {
                        EditorConfiguration.Scenes = new System.Collections.Generic.List<AltUnityMyScenes>();
                        UnityEditor.EditorBuildSettings.scenes = pathFromTheSceneInCurrentList();
                    }
                    UnityEditor.EditorGUILayout.EndHorizontal();
                }
                else
                {
                    UnityEditor.EditorGUILayout.BeginHorizontal();
                    if (UnityEngine.GUILayout.Button("Add all scenes", UnityEditor.EditorStyles.miniButtonLeft, UnityEngine.GUILayout.MinWidth(30)))
                    {
                        AddAllScenes();
                    }

                    if (UnityEngine.GUILayout.Button("Select all scenes", UnityEditor.EditorStyles.miniButtonMid, UnityEngine.GUILayout.MinWidth(30)))
                    {
                        SelectAllScenes();
                    }
                    UnityEditor.EditorGUILayout.EndHorizontal();
                    UnityEditor.EditorGUILayout.BeginHorizontal();

                    if (UnityEngine.GUILayout.Button("Deselect all scenes", UnityEditor.EditorStyles.miniButtonMid, UnityEngine.GUILayout.MinWidth(30)))
                    {
                        deselectAllScenes();
                    }
                    if (UnityEngine.GUILayout.Button("Remove unselected scenes", UnityEditor.EditorStyles.miniButtonMid, UnityEngine.GUILayout.MinWidth(30)))
                    {
                        removeNotSelectedScenes();
                    }
                    if (UnityEngine.GUILayout.Button("Remove all scenes", UnityEditor.EditorStyles.miniButtonRight, UnityEngine.GUILayout.MinWidth(30)))
                    {
                        EditorConfiguration.Scenes = new System.Collections.Generic.List<AltUnityMyScenes>();
                        UnityEditor.EditorBuildSettings.scenes = pathFromTheSceneInCurrentList();
                    }
                    UnityEditor.EditorGUILayout.EndHorizontal();
                }

                UnityEditor.EditorGUILayout.EndVertical();
            }

            UnityEditor.EditorGUILayout.EndVertical();
            UnityEditor.EditorGUILayout.EndHorizontal();
        }

        private static UnityEngine.GUIStyle setTextGuiStyle()
        {
            var guiStyle = new UnityEngine.GUIStyle
            {
                alignment = UnityEngine.TextAnchor.MiddleLeft,
                stretchHeight = true
            };
            guiStyle.normal.textColor = UnityEditor.EditorGUIUtility.isProSkin ? UnityEngine.Color.white : UnityEngine.Color.black;
            guiStyle.wordWrap = true;
            return guiStyle;
        }

        private void removeNotSelectedScenes()
        {
            var copyMySceneses = new List<AltUnityMyScenes>();
            foreach (var scene in EditorConfiguration.Scenes)
            {
                if (scene.ToBeBuilt)
                {
                    copyMySceneses.Add(scene);
                }
            }

            EditorConfiguration.Scenes = copyMySceneses;
            UnityEditor.EditorBuildSettings.scenes = pathFromTheSceneInCurrentList();
        }

        private void deselectAllScenes()
        {
            foreach (var scene in EditorConfiguration.Scenes)
            {
                scene.ToBeBuilt = false;
            }
            UnityEditor.EditorBuildSettings.scenes = pathFromTheSceneInCurrentList();

        }


        private void displayTestGui(System.Collections.Generic.List<AltUnityMyTest> tests)
        {
            UnityEditor.EditorGUILayout.LabelField("Tests list", UnityEditor.EditorStyles.boldLabel);
            UnityEditor.EditorGUILayout.BeginHorizontal();
            UnityEditor.EditorGUILayout.EndHorizontal();
            UnityEditor.EditorGUILayout.BeginVertical(UnityEngine.GUI.skin.textArea);

            int foldOutCounter = 0;
            int testCounter = 0;
            var parentNames = new List<string>();
            foreach (var test in tests)
            {
                if (test.TestCaseCount == 0)
                {
                    continue;
                }
                if (foldOutCounter > 0 && test.Type == typeof(NUnit.Framework.Internal.TestMethod))
                {
                    foldOutCounter--;
                    continue;
                }
                if (foldOutCounter > 0)
                {
                    continue;
                }
                testCounter++;
                var idx = parentNames.IndexOf(test.ParentName) + 1;
                parentNames.RemoveRange(idx, parentNames.Count - idx);

                if (tests.IndexOf(test) == SelectedTest)
                {
                    var selectedTestStyle = new UnityEngine.GUIStyle();
                    selectedTestStyle.normal.background = selectedTestTexture;
                    UnityEditor.EditorGUILayout.BeginHorizontal(selectedTestStyle);
                }
                else
                {
                    if (testCounter % 2 == 0)
                    {
                        var evenNumberStyle = new UnityEngine.GUIStyle();
                        evenNumberStyle.normal.background = evenNumberTestTexture;
                        UnityEditor.EditorGUILayout.BeginHorizontal(evenNumberStyle);
                    }
                    else
                    {
                        var oddNumberStyle = new UnityEngine.GUIStyle();
                        oddNumberStyle.normal.background = oddNumberTestTexture;
                        UnityEditor.EditorGUILayout.BeginHorizontal(oddNumberStyle);
                    }
                }
                UnityEditor.EditorGUILayout.LabelField(" ", UnityEngine.GUILayout.Width(30 * parentNames.Count));
                var gUIStyle = new UnityEngine.GUIStyle
                {
                    alignment = UnityEngine.TextAnchor.MiddleLeft
                };
                var valueChanged = UnityEditor.EditorGUILayout.Toggle(test.Selected, UnityEngine.GUILayout.Width(15));
                if (valueChanged != test.Selected)
                {
                    test.Selected = valueChanged;
                    changeSelectionChildsAndParent(test);
                }

                var testName = test.TestName;

                if (test.ParentName == "")
                {
                    var splitedPath = testName.Split('/');
                    testName = splitedPath[splitedPath.Length - 1];
                }
                else
                {
                    var splitedPath = testName.Split('.');
                    testName = splitedPath[splitedPath.Length - 1];
                }


                if (test.Status == 0)
                {
                    var guiStyle = new UnityEngine.GUIStyle { normal = { textColor = UnityEditor.EditorGUIUtility.isProSkin ? UnityEngine.Color.white : UnityEngine.Color.black } };
                    selectTest(tests, test, testName, guiStyle);
                }
                else
                {
                    UnityEngine.Color color = redColor;
                    UnityEngine.Texture2D icon = failIcon;
                    if (test.Status == 1)
                    {
                        color = greenColor;
                        icon = passIcon;
                    }
                    UnityEngine.GUILayout.Label(icon, gUIStyle, UnityEngine.GUILayout.Width(20));
                    var guiStyle = new UnityEngine.GUIStyle { normal = { textColor = color } };
                    selectTest(tests, test, testName, guiStyle);
                }
                UnityEngine.GUILayout.FlexibleSpace();
                if (test.Type == typeof(NUnit.Framework.Internal.TestMethod))
                {
                    test.FoldOut = true;
                }
                else
                {
                    test.FoldOut = UnityEditor.EditorGUILayout.Foldout(test.FoldOut, "");
                    if (!test.FoldOut)
                    {
                        foldOutCounter = test.TestCaseCount;
                    }
                    parentNames.Add(test.TestName);
                }
                UnityEditor.EditorGUILayout.EndHorizontal();

            }
            UnityEditor.EditorGUILayout.EndVertical();
        }

        private static void selectTest(System.Collections.Generic.List<AltUnityMyTest> tests, AltUnityMyTest test, string testName, UnityEngine.GUIStyle guiStyle)
        {
            if (!test.IsSuite)
            {
                if (UnityEngine.GUILayout.Button(testName, guiStyle))
                {
                    if (SelectedTest == tests.IndexOf(test))
                    {
                        var actualTime = System.DateTime.Now.Ticks;
                        if (actualTime - timeSinceLastClick < 5000000)
                        {
#if UNITY_2019_1_OR_NEWER
                            UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(test.path, 1, 0);
#else
                        UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(test.path, 1);
#endif
                        }
                    }
                    else
                    {
                        SelectedTest = tests.IndexOf(test);

                    }
                    timeSinceLastClick = System.DateTime.Now.Ticks;
                }
            }
            else
            {
                UnityEngine.GUILayout.Label(testName, guiStyle);
            }
        }

        private void changeSelectionChildsAndParent(AltUnityMyTest test)
        {
            if (test.Type.ToString().Equals("NUnit.Framework.Internal.TestAssembly"))
            {
                var index = EditorConfiguration.MyTests.IndexOf(test);
                for (int i = index + 1; i < EditorConfiguration.MyTests.Count; i++)
                {
                    if (EditorConfiguration.MyTests[i].Type.ToString().Equals("NUnit.Framework.Internal.TestAssembly"))
                    {
                        break;
                    }
                    else
                    {
                        EditorConfiguration.MyTests[i].Selected = test.Selected;
                    }
                }
            }
            else
            {
                if (test.IsSuite)
                {
                    var index = EditorConfiguration.MyTests.IndexOf(test);
                    for (int i = index + 1; i <= index + test.TestCaseCount; i++)
                    {
                        EditorConfiguration.MyTests[i].Selected = test.Selected;
                    }
                }
                if (test.Selected == false)
                {
                    while (test.ParentName != null)
                    {
                        test = EditorConfiguration.MyTests.FirstOrDefault(a => a.TestName.Equals(test.ParentName));
                        if (test != null)
                            test.Selected = false;
                        else
                            return;
                    }
                }

            }
        }

        private static void sceneMove(AltUnityMyScenes scene, bool up)
        {
            int index = EditorConfiguration.Scenes.IndexOf(scene);
            if (up)
            {
                swap(index, index - 1);
            }
            else
            {
                swap(index, index + 1);
            }
        }

        private static UnityEditor.EditorBuildSettingsScene[] pathFromTheSceneInCurrentList()
        {
            var listofPath = new List<UnityEditor.EditorBuildSettingsScene>();
            foreach (var scene in EditorConfiguration.Scenes)
            {
                listofPath.Add(new UnityEditor.EditorBuildSettingsScene(scene.Path, scene.ToBeBuilt));
            }

            return listofPath.ToArray();
        }

        private void removeScene(AltUnityMyScenes scene)
        {
            EditorConfiguration.Scenes.Remove(scene);
            UnityEditor.EditorBuildSettings.scenes = pathFromTheSceneInCurrentList();
        }

        private static string getPathForSelectedItem()
        {
            string path = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);
            if (System.IO.Path.GetExtension(path) != "") //checks if current item is a folder or a file
            {
                path = path.Replace(System.IO.Path.GetFileName(UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject)), "");
            }
            return path;
        }

        private static void destroyAltUnityRunner(UnityEngine.Object altUnityRunner)
        {

            DestroyImmediate(altUnityRunner);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            EditorSceneManager.SaveOpenScenes();
            EditorSceneManager.OpenScene(AltUnityBuilder.PreviousScenePath);
        }

        private static void addComponentToObjectAndHisChildren(UnityEngine.GameObject gameObject)
        {
            var component = gameObject.GetComponent<AltUnityId>();
            if (component == null)
            {
                gameObject.AddComponent(typeof(AltUnityId));

            }
            int childCount = gameObject.transform.childCount;
            for (int j = 0; j < childCount; j++)
            {
                addComponentToObjectAndHisChildren(gameObject.transform.GetChild(j).gameObject);
            }
        }

        private static void removeComponentFromEveryObjectInTheScene()
        {
            var rootObjects = new List<UnityEngine.GameObject>();
            UnityEngine.SceneManagement.Scene scene = EditorSceneManager.GetActiveScene();
            scene.GetRootGameObjects(rootObjects);

            // iterate root objects and do something
            for (int i = 0; i < rootObjects.Count; ++i)
            {
                removeComponentFromObjectAndHisChildren(rootObjects[i]);
            }
            EditorSceneManager.MarkSceneDirty(scene);
            EditorSceneManager.SaveScene(scene);
        }

        private static void removeComponentFromObjectAndHisChildren(UnityEngine.GameObject gameObject)
        {
            var component = gameObject.GetComponent<AltUnityId>();
            if (component != null)
            {
                DestroyImmediate(component);
            }
            int childCount = gameObject.transform.childCount;
            for (int j = 0; j < childCount; j++)
            {
                removeComponentFromObjectAndHisChildren(gameObject.transform.GetChild(j).gameObject);
            }
        }

        private static string[] altUnityGetAllScenes()
        {
            string[] temp = UnityEditor.AssetDatabase.GetAllAssetPaths();
            var result = new List<string>();
            foreach (string s in temp)
            {
                if (s.EndsWith(".unity")) result.Add(s);
            }
            return result.ToArray();
        }
    }
}
