using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace ProjectPlanner
{
    sealed class Menu
    {
        private const string root = "Tools/Project Planner/";
        private const string help = root + "Help/";
        private const string import = root + "Import | Export/";

        [MenuItem(root + "Board Window %&b", false, 0)]
        private static void OpenBoardWindow()
        {
            BoardWindow.Init();
        }
        [MenuItem(root + "Task Window %&t", false, 1)]
        private static void OpenTaskWindow()
        {
            TaskWindow.Init();
        }
        [MenuItem(root + "Tree Window", false, 2)]
        private static void OpenTreeViewWindow()
        {
            TreeWindow.Init();
        }
        [MenuItem(root + "Code Analyzer", false, 3)]
        private static void OpenCodeAnalyzerWindow()
        {
            CodeAnalyzerWindow.Init();
        }
        [MenuItem(root + "Quick Task %&q", false, 4)]
        private static void OpenQuickTaskWindow()
        {
            QuickTaskWindow.Init();
        }

        [MenuItem(root + "Settings", false, 98)]
        public static void OpenSettings()
        {
            PreferencesWindow.Init();
        }

        [MenuItem(import + "Demo Content", false, 100)]
        public static void DemoContent()
        {
            DemoImporter.Init();
        }
        [MenuItem(import + "GitKraken Glo Boards", false, 101)]
        public static void GitKrakenGloBoards()
        {
            GitKrakenWindow.Init();
        }
        [MenuItem(import + "FTP Server", false, 102)]
        public static void FTP()
        {
            FTPWindow.Init();
        }

        [MenuItem(help + "Welcome", false, 101)]
        private static void OpenWelcomeWindow()
        {
            WelcomeWindow.Init();
        }
        [MenuItem(help + "Manual", false, 103)]
        public static void Manual()
        {
            Application.OpenURL(FileManager.ManualPath);
        }
        [MenuItem(help + "Release Notes", false, 104)]
        public static void OpenReleaseNotes()
        {
            ReleaseNotesWindow.Init();
        }
        [MenuItem(help + "Report Bug", false, 115)]
        private static void ReportIssue()
        {
            Application.OpenURL(Info.ReportBugURL);
        }
        [MenuItem(help + "Contact Me", false, 116)]
        private static void ContactMe()
        {
            Application.OpenURL(Info.ContactURL);
        }
        
        [MenuItem(root + "About", false, 103)]
        public static void OpenAbout()
        {
            AboutWindow.Init();
        }
    }
}
