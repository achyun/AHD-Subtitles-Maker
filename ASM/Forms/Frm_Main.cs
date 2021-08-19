// This file is part of AHD Subtitles Maker.
// A program can make and edit subtitle.
// 
// Copyright © Alaa Ibrahim Hadid 2009 - 2021
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.
// 
// Author email: mailto:alaahadidfreeware@gmail.com
//
using AHD.Forms;
using AHD.SM.ASMP;
using AHD.SM.Controls;
using AHD.SM.Formats;
using AHD.SM.Forms;
using SmartHotKey;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace AHD.SM
{
    public partial class Frm_Main : Form
    {
        public Frm_Main(string[] Args)
        {
            InitializeComponent();
            AddTimelineControl();
            AddMediaPlayerControl();
            AddSubtitleEditorControl();
            AddSubtitlesDataEditorControl();
            AddErrorsCheckerControl();
            AddMultipleSubtitleTracksViewer();
            AddPreparedTextControl();
            LoadSettings();
            //create new default project any way
            Program.ProjectManager.Project = new Project("Untitled");
            Program.ProjectManager.FilePath = "";
            Program.ProjectManager.ProjectSaved += new EventHandler(ProjectManager_ProjectSaved);
            Program.ProjectManager.ProjectDidntSave += new EventHandler<ErrorEventArgs>(ProjectManager_ProjectDidntSave);
            Program.ProjectManager.ProjectLoaded += new EventHandler(ProjectManager_ProjectLoaded);
            Program.ProjectManager.ProjectDidntLoad += new EventHandler<ErrorEventArgs>(ProjectManager_ProjectDidntLoad);
            mediaPlayer1.ChangeMediaRequest += new EventHandler(mediaPlayer1_ChangeMediaRequest);
            ApplyProject();
            Save = false;
            LaunchCommandLines(Args);

            WaveReader.ProgressStarted += WaveReader_ProgressStarted;
            WaveReader.ProgressFinished += WaveReader_ProgressFinished;
            WaveReader.Progress += WaveReader_Progress;

            // DISABLE CHECK FOR UPDATE
            checkForUpdatesToolStripMenuItem.Enabled = false;
        }
        private const string translation_temp_track = "TRANLATION_BACKUP";
        private ActiveMode _activeMode = ActiveMode.None;
        private ActiveMode activeMode
        {
            get { return _activeMode; }
            set
            {
                _activeMode = value;
                if (value == ActiveMode.Properties ||
                    value == ActiveMode.ProjectDescription ||
                    value == ActiveMode.PreparedText ||
                    isRenaming)
                {
                    //     if (Program.Settings.ShortcutAddSubtitle != "")
                    hotKey1.RemoveKey(Program.Settings.ShortcutAddSubtitle);
                    hotKey1.RemoveKey(Program.Settings.ShortcutFWD);
                    hotKey1.RemoveKey(Program.Settings.ShortcutREW);
                }
                else
                {
                    if (isEditingSubtitle)
                        return;
                    if (Program.Settings.ShortcutAddSubtitle != "")
                        hotKey1.AddHotKey(Program.Settings.ShortcutAddSubtitle);
                    if (Program.Settings.ShortcutFWD != "")
                        hotKey1.AddHotKey(Program.Settings.ShortcutFWD);
                    if (Program.Settings.ShortcutREW != "")
                        hotKey1.AddHotKey(Program.Settings.ShortcutREW);
                }
            }
        }
        private SubtitlesTrack selectedTrack = null;
        private Subtitle viewedSubtitle = null;
        private List<Subtitle> copiedSubtitles = new List<Subtitle>();
        private int sIndex = -1;//current viewed subtitle index
        private int pIndex = -1;//set to -1 to refresh view
        private bool IsAddingSubtitle = false;//set when the user hold the add button
        private Subtitle addSubtitle = null;//The subtitle to add, created when user hold quick add button, 
        //destroyed when user release that button
        private int SearchIndex = 0;
        private Point downPoint;
        private bool canDragDrop = false;
        private bool needToSaveSettings = true;
        private int currentHistory = 0;
        // others...
        private bool haltOSMessage = false;// set to true to disable the message of download subtitles from OS for this movie...
        private int autoSaveTimerSeconds = 0;
        private Frm_AutoSaving frm_autoSaving;
        Frm_Translate frm_translate = null;
        private bool isUpdatingCheck;
        private bool isRenaming;
        private bool isChangingSettings = false;
        private bool isEditingSubtitle = false;
        /*Controls*/
        private TimeLine timeLine1;
        private MediaPlayer mediaPlayer1;
        private SubtitleEditor subtitleEditor1;
        private SubtitlesDataEditor subtitlesDataEditor1;
        private ErrorsChecker errorsChecker1;
        private MultipleSubtitleTracksViewer multipleSubtitleTrackViewer;
        private PreparedTextEditor preparedTextEditor;

        private void LaunchCommandLines(string[] Args)
        {
            if (Args != null)
            {
                if (Args.Length > 0)
                {
                    for (int i = 0; i < Args.Length; i++)
                    {
                        switch (Args[i])
                        {
                            case "/imp"://import subtitles format
                                i++;
                                ImportFormat(Args[i]);
                                break;
                            case "/ly"://load layout
                                i++;
                                LoadLayout(Args[i]);
                                break;
                            case "/med"://change media
                                i++;
                                ChangeMedia(Args[i]);
                                break;
                            default:
                                if (File.Exists(Args[i]))
                                {
                                    LoadProject(Args[i]);
                                }
                                break;
                        }
                    }
                }
            }
        }
        public void ShowQuickStart()
        {
            Frm_QuickStart qq = new Frm_QuickStart(Program.Settings.RecentProject, Program.Settings.ShowQuickStart);
            qq.ShowDialog(this);
            Program.Settings.ShowQuickStart = qq.ShowAtStartUp;
            switch (qq.QuickStartResult)
            {
                case QuickStartResult.NewProject: NewProject(); break;
                case QuickStartResult.OpenProject: openToolStripMenuItem_Click(this, null); break;
                case QuickStartResult.OpenRecentProject: LoadProject(qq.OpenRecentPath); break;
                case QuickStartResult.GettingStarted:
                    Help.ShowHelp(this, Program.StartUpPath + "\\Help.chm",
HelpNavigator.KeywordIndex, "Workflow"); break;
                case QuickStartResult.Import://import subtitles format file
                    {
                        OpenFileDialog op = new OpenFileDialog();
                        op.Title = Program.ResourceManager.GetString("Title_ImportSubtitlesFormat");
                        op.Filter = SubtitleFormats.GetFilter();
                        if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            ImportFormat(op.FileName);
                        }
                        break;
                    }
            }
            Program.Settings.Save();
        }
        public void ShowTipOfTheDay()
        {
            Frm_TipOfTheDay tii = new Frm_TipOfTheDay(Program.Settings.ShowTipOfDay,
                Program.Settings.TipOfDayIndex, Program.StartUpPath + "\\" + Program.CultureInfo.Name + "\\Help.chm");
            tii.ShowDialog(this);
            Program.Settings.ShowTipOfDay = tii.DisplayAtStartUp;
            Program.Settings.TipOfDayIndex = tii.TipIndex;
            Program.Settings.Save();
        }
        private void ImportFormat(string filePath)
        {
            Frm_Import import = new Frm_Import(filePath, Program.Settings.ImportDeepSearch);
            if (import.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                //prepare format
                SubtitlesFormat format = import.SubtitlesFormat;
                Program.Settings.FavoriteFormat = format.Name;
                format.LoadStarted += new EventHandler(format_LoadStarted);
                format.LoadFinished += new EventHandler(format_LoadFinished);
                format.Progress += new EventHandler<ProgressArgs>(format_Progress);
                //load
                format.Load(filePath, import.SelectedEncoding);
                //get track(s)
                SubtitlesTrack[] tracks = format.SubtitleTracks.ToArray();
                if (!format.IsMultiTrack)
                    tracks = new SubtitlesTrack[1] { format.SubtitleTrack };
                //add the track(s) into the project
                for (int i = 0; i < tracks.Length; i++)
                {
                    int j = 0;
                    string name = "ImportedTrack" + (i + 1).ToString() + "_" + j;
                    while (Program.ProjectManager.Project.IsSubtitlesTrackExist(name))
                    {
                        j++;
                        name = "ImportedTrack" + (i + 1).ToString() + "_" + j;
                    }
                    tracks[i].Name = name;
                    if (!format.IsMultiTrack)
                    {
                        tracks[i].RightToLeft = import.RightToLeftSubtitlesFormat;
                        foreach (Subtitle sub in tracks[i].Subtitles)
                            sub.Text.RighttoLeft = import.RightToLeftSubtitlesFormat;
                    }
                    Program.ProjectManager.Project.SubtitleTracks.Add(tracks[i]);
                }
                format.Dispose();
                //refresh
                RefreshSubtitleTracks();
                //select
                foreach (TreeNode_SubtitlesTrack node in treeView_subtitleTracks.Nodes)
                {
                    if (node.SubtitlesTrack.Name == tracks[0].Name)
                    {
                        treeView_subtitleTracks.SelectedNode = node;
                        break;
                    }
                }
                if (Program.Settings.AutoCheckForErrors)
                {
                    if (!Program.Settings.CurrentLayout.Tab_Errors.Visible)
                    {
                        Program.Settings.CurrentLayout.Tab_Errors.Visible = errorsToolStripMenuItem1.Checked = true;
                        SetTabParent(Program.Settings.CurrentLayout.Tab_Errors, tabPage_errors);
                        CheckForCollapse();
                    }
                    ((TabControl)tabPage_errors.Parent).SelectedTab = tabPage_errors;
                    errorsChecker1.CheckForErrors();
                }
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_ImportSubtitlesFormat"));
            }
        }
        private void ApplyTrackToControls()
        {
            errorsChecker1.SubtitlesTrack = subtitleEditor1.SubtitlesTrack = timeLine1.SubtitlesTrack =
          subtitlesDataEditor1.SubtitlesTrack = selectedTrack;
        }
        public bool CheckShortcut(Keys key)
        {
            for (int i = 0; i < menuStrip1.Items.Count; i++)
            {
                if (menuStrip1.Items[i] is ToolStripMenuItem)
                {
                    ToolStripMenuItem itemL1 = (ToolStripMenuItem)menuStrip1.Items[i];
                    // Check
                    if (itemL1.ShortcutKeys == key)
                    {
                        MessageDialog.ShowErrorMessage(Program.ResourceManager.GetString("Message_ThisShortcutKeyAlreadyTakenBy")
                            + " '" + itemL1.Text + "'", Program.ResourceManager.GetString("MessageCaption_ChangeKey"));
                        return true;
                    }
                    // Second level check
                    for (int j = 0; j < itemL1.DropDownItems.Count; j++)
                    {
                        if (itemL1.DropDownItems[j] is ToolStripMenuItem)
                        {
                            ToolStripMenuItem itemL2 = (ToolStripMenuItem)itemL1.DropDownItems[j];
                            // Check
                            if (itemL2.ShortcutKeys == key)
                            {
                                MessageDialog.ShowErrorMessage(Program.ResourceManager.GetString("Message_ThisShortcutKeyAlreadyTakenBy")
                                    + " '" + itemL2.Text + "'", Program.ResourceManager.GetString("MessageCaption_ChangeKey"));
                                return true;
                            }
                            // Third level check
                            for (int k = 0; k < itemL2.DropDownItems.Count; k++)
                            {
                                if (itemL2.DropDownItems[k] is ToolStripMenuItem)
                                {
                                    ToolStripMenuItem itemL3 = (ToolStripMenuItem)itemL2.DropDownItems[k];
                                    // Check
                                    if (itemL3.ShortcutKeys == key)
                                    {
                                        MessageDialog.ShowErrorMessage(Program.ResourceManager.GetString("Message_ThisShortcutKeyAlreadyTakenBy")
                                            + " '" + itemL3.Text + "'", Program.ResourceManager.GetString("MessageCaption_ChangeKey"));
                                        return true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return false;
        }

        #region Controls initialize
        private void AddTimelineControl()
        {
            timeLine1 = new TimeLine();
            tabPage_TimeLine.Controls.Add(timeLine1);
            timeLine1.Dock = DockStyle.Fill;
            this.timeLine1.ContextMenuStrip = contextMenuStrip_subtitlesData;
            this.timeLine1.BackColor = System.Drawing.Color.White;
            this.timeLine1.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.timeLine1.MarkColor = System.Drawing.Color.Red;
            this.timeLine1.MediaDuration = 1000D;
            this.timeLine1.MediaHeaderRectangleColor = System.Drawing.Color.MediumSeaGreen;
            this.timeLine1.MediaRectangleColor = System.Drawing.Color.MediumAquamarine;
            this.timeLine1.MediaText = "";
            this.timeLine1.MegnaticAccuracy = 3;
            this.timeLine1.Name = "timeLine1";
            this.timeLine1.PreviewMode = false;
            this.timeLine1.SelectionRectangleColor = System.Drawing.Color.Navy;
            this.timeLine1.SmartToolAccuracy = 7;
            this.timeLine1.SplitersColor = System.Drawing.Color.Gray;
            this.timeLine1.SubtitleHeaderRectangleColor = System.Drawing.Color.RoyalBlue;
            this.timeLine1.SubtitleRectangleColor = System.Drawing.Color.DodgerBlue;
            this.timeLine1.SubtitleSelectedColor = System.Drawing.Color.Purple;
            this.timeLine1.SubtitlesTrack = null;
            this.timeLine1.SubtitleStringColor = System.Drawing.Color.White;
            this.timeLine1.TickColor = System.Drawing.Color.White;
            this.timeLine1.TickPanelToolTipColor = System.Drawing.Color.Blue;
            this.timeLine1.TimeLineColor = Color.White;
            this.timeLine1.TimeMarks = null;
            this.timeLine1.ToolTipRectangleColor = System.Drawing.Color.RoyalBlue;
            this.timeLine1.ToolTipTextColor = System.Drawing.Color.White;
            this.timeLine1.TimeChangeRequest += new System.EventHandler<AHD.SM.Controls.TimeChangeArgs>(this.timeLine1_TimeChangeRequest);
            this.timeLine1.SubtitlesSelected += new System.EventHandler(this.timeLine1_SubtitlesSelected);
            this.timeLine1.SubtitleDoubleClick += new System.EventHandler(this.timeLine1_SubtitleDoubleClick);
            this.timeLine1.SubtitlePropertiesChanged += new System.EventHandler(this.timeLine1_SubtitlePropertiesChanged);
            this.timeLine1.JumpIntoTimeRequest += new System.EventHandler(this.timeLine1_JumpIntoTimeRequest);
            this.timeLine1.AddMarkRequest += new System.EventHandler(this.timeLine1_AddMarkRequest);
            this.timeLine1.RemoveMarkRequest += new System.EventHandler(this.timeLine1_RemoveMarkRequest);
            this.timeLine1.JumpIntoMarkRequest += new System.EventHandler(this.timeLine1_JumpIntoMarkRequest);
            this.timeLine1.SelectMarkRequest += new System.EventHandler<AHD.SM.ASMP.MarkEditArgs>(this.timeLine1_SelectMarkRequest);
            this.timeLine1.MarkEdit += new System.EventHandler<AHD.SM.ASMP.MarkEditArgs>(this.timeLine1_MarkEdit);
            this.timeLine1.SelectedTrackChanged += TimeLine1_SelectedTrackChanged;
            this.timeLine1.Enter += timeLine1_Enter;
            this.timeLine1.MarkSelected += TimeLine1_MarkSelected;
            this.timeLine1.ChangeMediaRequest += changeMediaToolStripMenuItem_Click;
            this.timeLine1.ImportSubtitlesFormatRequest += importToolStripMenuItem_Click;
        }
        private void AddMediaPlayerControl()
        {
            mediaPlayer1 = new MediaPlayer();
            tabPage_media.Controls.Add(mediaPlayer1);
            mediaPlayer1.Dock = DockStyle.Fill;
            this.mediaPlayer1.AddButtonEnabled = true;
            this.mediaPlayer1.AdvanceTime = 100D;
            this.mediaPlayer1.CurrentPosition = 0D;
            this.mediaPlayer1.EditorBackColor = System.Drawing.Color.Black;
            this.mediaPlayer1.EndSetButtonEnabled = true;
            this.mediaPlayer1.Mute = false;
            this.mediaPlayer1.Name = "mediaPlayer1";
            this.mediaPlayer1.ShowEditorStatusStrip = false;
            this.mediaPlayer1.Volume = 100;
            this.mediaPlayer1.SaveRequest += new System.EventHandler(this.mediaPlayer1_SaveRequest);
            this.mediaPlayer1.AddButtonHoldPress += new System.EventHandler(this.mediaPlayer1_AddButtonHoldPress);
            this.mediaPlayer1.AddButtonHoldRelease += new System.EventHandler(this.mediaPlayer1_AddButtonHoldRelease);
            this.mediaPlayer1.EndSetButtonPressed += new System.EventHandler(this.mediaPlayer1_EndSetButtonPressed);
            this.mediaPlayer1.MuteToggle += new System.EventHandler(this.mediaPlayer1_MuteToggle);
            this.mediaPlayer1.TimeSlide += new System.EventHandler(this.mediaPlayer1_TimeSlide);
            this.mediaPlayer1.SubtitleTextEditStarted += MediaPlayer1_SubtitleTextEditStarted;
            this.mediaPlayer1.Enter += mediaPlayer1_Enter;
            this.mediaPlayer1.DragEnter += tabPage_media_DragEnter;
            this.mediaPlayer1.DragDrop += tabPage_media_DragDrop;
            this.mediaPlayer1.EditStylesRequest += SubtitleEditor1_EditStylesRequest;
            this.mediaPlayer1.EditSubtitleRequest += MediaPlayer1_EditSubtitleRequest;
            this.mediaPlayer1.EditCustomStyleRequest += MediaPlayer1_EditCustomStyleRequest;
        }
        private void AddSubtitleEditorControl()
        {
            subtitleEditor1 = new SubtitleEditor();
            tabPage_Properties.Controls.Add(subtitleEditor1);
            subtitleEditor1.Dock = DockStyle.Fill;
            this.subtitleEditor1.AskWhenChangingRTLToApplyToAll = false;
            this.subtitleEditor1.DirectApplyToItem = true;
            this.subtitleEditor1.EditorBackColor = System.Drawing.Color.Black;
            this.subtitleEditor1.Name = "subtitleEditor1";
            this.subtitleEditor1.SelectedItems = null;
            this.subtitleEditor1.ShowStatusStrip = true;
            this.subtitleEditor1.SubtitlesTrack = null;
            this.subtitleEditor1.SaveRequest += new System.EventHandler(this.subtitleEditor1_SaveRequest);
            this.subtitleEditor1.Enter += subtitleEditor1_Enter;
            this.subtitleEditor1.EditStylesRequest += SubtitleEditor1_EditStylesRequest;
        }
        private void AddSubtitlesDataEditorControl()
        {
            subtitlesDataEditor1 = new SubtitlesDataEditor();
            tabPage_subitltesData.Controls.Add(subtitlesDataEditor1);
            subtitlesDataEditor1.Dock = DockStyle.Fill;
            this.subtitlesDataEditor1.AlwayesEnsureViewed = true;
            this.subtitlesDataEditor1.ContextMenuStrip = this.contextMenuStrip_subtitlesData;
            this.subtitlesDataEditor1.Name = "subtitlesDataEditor1";
            this.subtitlesDataEditor1.SubtitlesTrack = null;
            this.subtitlesDataEditor1.SubtitlesSelected += new System.EventHandler(this.subtitlesDataEditor1_SubtitlesSelected);
            this.subtitlesDataEditor1.TimeChangeRequest += new System.EventHandler<AHD.SM.Controls.TimeChangeArgs>(this.subtitlesDataEditor1_TimeChangeRequest);
            this.subtitlesDataEditor1.SubtitleEditRequest += new System.EventHandler(this.subtitlesDataEditor1_SubtitleEditRequest);
            this.subtitlesDataEditor1.SubtitlePropertiesChanged += SubtitlesDataEditor1_SubtitlePropertiesChanged;
            this.subtitlesDataEditor1.Enter += new System.EventHandler(this.subtitlesDataEditor1_Enter);
        }
        private void AddErrorsCheckerControl()
        {
            this.errorsChecker1 = new ErrorsChecker();
            this.tabPage_errors.Controls.Add(errorsChecker1);
            this.errorsChecker1.Dock = DockStyle.Fill;
            this.errorsChecker1.Name = "errorsChecker1";
            this.errorsChecker1.SubtitlesTrack = null;
            this.errorsChecker1.Enter += errorsChecker1_Enter;
            this.errorsChecker1.CheckProgress += new System.EventHandler<AHD.SM.ASMP.ProgressArgs>(this.errorsChecker1_CheckProgress);
            this.errorsChecker1.CheckFinished += new System.EventHandler(this.errorsChecker1_CheckFinished);
            this.errorsChecker1.CheckStarted += new System.EventHandler(this.errorsChecker1_CheckStarted);
            this.errorsChecker1.AutoFixProgress += new System.EventHandler<AHD.SM.ASMP.ProgressArgs>(this.errorsChecker1_AutoFixProgress);
            this.errorsChecker1.AutoFixFinished += new System.EventHandler(this.errorsChecker1_AutoFixFinished);
            this.errorsChecker1.AutoFixStarted += new System.EventHandler(this.errorsChecker1_AutoFixStarted);
            this.errorsChecker1.SelectSubtitlesRequest += new System.EventHandler<AHD.SM.ASMP.SubtitlesSelectArgs>(this.errorsChecker1_SelectSubtitlesRequest);
        }
        private void AddMultipleSubtitleTracksViewer()
        {
            this.multipleSubtitleTrackViewer = new MultipleSubtitleTracksViewer();
            this.tabPage_multipleSubtitleTrackViewer.Controls.Add(multipleSubtitleTrackViewer);
            this.multipleSubtitleTrackViewer.Dock = DockStyle.Fill;
            this.multipleSubtitleTrackViewer.Name = "multipleSubtitleTrackViewer";
            this.multipleSubtitleTrackViewer.Enter += multipleSubtitleTrackViewer_Enter;
            this.multipleSubtitleTrackViewer.UpdateTrackChecks += multipleSubtitleTrackViewer_UpdateTrackChecks;
        }
        private void AddPreparedTextControl()
        {
            this.preparedTextEditor = new PreparedTextEditor();
            this.tabPage_preparedText.Controls.Add(preparedTextEditor);
            this.preparedTextEditor.Dock = DockStyle.Fill;
            this.preparedTextEditor.Name = "preparedTextEditor";
            this.preparedTextEditor.Enter += preparedTextEditor_Enter;
            this.preparedTextEditor.SaveRequest += preparedTextEditor_SaveRequest;
            this.preparedTextEditor.TextChanged += preparedTextEditor_TextChanged;
            this.preparedTextEditor.BringToFront();
            this.preparedTextEditor.EditStylesRequest += SubtitleEditor1_EditStylesRequest;
        }
        #endregion
        #region Project New/Save/Open
        void NewProject()
        {
            if (!AskToSave())
                return;
            if (!Program.Settings.ShowNewProjectWizard)
            {
                Program.ProjectManager.Project = new Project(Program.ResourceManager.GetString("Untitled"));
                Program.ProjectManager.FilePath = "";
                Program.ProjectManager.ProjectSaved += new EventHandler(ProjectManager_ProjectSaved);
                Program.ProjectManager.ProjectDidntSave += new EventHandler<ErrorEventArgs>(ProjectManager_ProjectDidntSave);
                Program.ProjectManager.ProjectLoaded += new EventHandler(ProjectManager_ProjectLoaded);
                Program.ProjectManager.ProjectDidntLoad += new EventHandler<ErrorEventArgs>(ProjectManager_ProjectDidntLoad);
                if (Program.Settings.AutoCreateSubtitlesTrackAfterCreatingNewProject)
                {
                    int i = 1;
                    string name = Program.ResourceManager.GetString("SubtitlesTrack") + i;
                    while (Program.ProjectManager.Project.IsSubtitlesTrackExist(name))
                    {
                        i++;
                        name = Program.ResourceManager.GetString("SubtitlesTrack") + i;
                    }
                    SubtitlesTrack newTrack = new SubtitlesTrack(name);
                    //template
                    if (Program.Settings.NewSubtitlesTrackTemplate)
                    {
                        //add subtitle at begaining
                        Subtitle sub = new Subtitle(1, 3, SubtitleText.FromString("Subtitles made by AHD Subtitles Maker\nE-mail: alahadid@hotmail.com"));
                        newTrack.Subtitles.Add(sub);
                        //add at the end
                        if (mediaPlayer1.Duration > 0)
                        {
                            sub = new Subtitle(mediaPlayer1.Duration - 4, mediaPlayer1.Duration - 2,
                                SubtitleText.FromString("Subtitles created by AHD Subtitles Maker\nE-mail: alahadid@hotmail.com"));
                            newTrack.Subtitles.Add(sub);
                        }
                        else//no media, add after 10 second from the first one
                        {
                            sub = new Subtitle(11, 13,
                               SubtitleText.FromString("Subtitles created by AHD Subtitles Maker\nE-mail: alahadid@hotmail.com"));
                            newTrack.Subtitles.Add(sub);
                        }
                    }
                    Program.ProjectManager.Project.SubtitleTracks.Add(newTrack);
                    RefreshSubtitleTracks();
                }
                ApplyProject();
                Save = false;
            }
            else
            {
                Frm_NewProject newDialog = new Frm_NewProject();
                if (newDialog.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    Program.ProjectManager.Project = new Project(newDialog.ProjectName);
                    //media
                    if (File.Exists(newDialog.ProjectMediaFile))
                        Program.ProjectManager.Project.MediaPath = newDialog.ProjectMediaFile;
                    //Import
                    if (File.Exists(newDialog.ProjectImport))
                    {
                        ImportFormat(newDialog.ProjectImport);
                    }
                    //Prepared Text
                    if (File.Exists(newDialog.ProjectPreparedText))
                    {
                        try
                        {
                            RichTextBox r = new RichTextBox();
                            if (preparedTextEditor.BackColor == Color.Black)
                                r.SelectionColor = Color.White;
                            r.SelectionAlignment = HorizontalAlignment.Center;
                            r.SelectedText = File.ReadAllText(newDialog.ProjectPreparedText);
                            Program.ProjectManager.Project.PreparedTextRTF = r.Rtf;
                            Program.ProjectManager.Project.UsePreparedText = true;
                        }
                        catch (Exception ex)
                        {
                            MessageDialog.ShowErrorMessage(ex.Message + "\n\n" + ex.ToString(), "AHD Subtitles Maker");
                        }
                    }
                    Program.ProjectManager.FilePath = "";
                    Program.ProjectManager.ProjectSaved += new EventHandler(ProjectManager_ProjectSaved);
                    Program.ProjectManager.ProjectDidntSave += new EventHandler<ErrorEventArgs>(ProjectManager_ProjectDidntSave);
                    Program.ProjectManager.ProjectLoaded += new EventHandler(ProjectManager_ProjectLoaded);
                    Program.ProjectManager.ProjectDidntLoad += new EventHandler<ErrorEventArgs>(ProjectManager_ProjectDidntLoad);
                    if (Program.Settings.AutoCreateSubtitlesTrackAfterCreatingNewProject &&
                        Program.ProjectManager.Project.SubtitleTracks.Count == 0)
                    {
                        int i = 1;
                        string name = Program.ResourceManager.GetString("SubtitlesTrack") + i;
                        while (Program.ProjectManager.Project.IsSubtitlesTrackExist(name))
                        {
                            i++;
                            name = Program.ResourceManager.GetString("SubtitlesTrack") + i;
                        }
                        SubtitlesTrack newTrack = new SubtitlesTrack(name);
                        //template
                        if (Program.Settings.NewSubtitlesTrackTemplate)
                        {
                            //add subtitle at begaining
                            Subtitle sub = new Subtitle(1, 3, SubtitleText.FromString("Subtitles created by AHD Subtitles Maker\nE-mail: alahadid@hotmail.com"));
                            newTrack.Subtitles.Add(sub);
                            //add at the end
                            if (mediaPlayer1.Duration > 0)
                            {
                                sub = new Subtitle(mediaPlayer1.Duration - 4, mediaPlayer1.Duration - 2,
                                    SubtitleText.FromString("Subtitles created by AHD Subtitles Maker\nE-mail: alahadid@hotmail.com"));
                                newTrack.Subtitles.Add(sub);
                            }
                            else//no media, add after 10 second from the first one
                            {
                                sub = new Subtitle(11, 13,
                                   SubtitleText.FromString("Subtitles created by AHD Subtitles Maker\nE-mail: alahadid@hotmail.com"));
                                newTrack.Subtitles.Add(sub);
                            }
                        }
                        Program.ProjectManager.Project.SubtitleTracks.Add(newTrack);
                        RefreshSubtitleTracks();
                    }
                    ApplyProject();
                    Save = false;
                }
            }
        }

        void ProjectManager_ProjectDidntLoad(object sender, ErrorEventArgs e)
        {
            SetStatus(Program.ResourceManager.GetString("Status_ProjectDidntLoad"), StatusType.Error);
            MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_ErrorWhenLoadingProject") +
                ":\n" + e.GetException().Message, Program.ResourceManager.GetString("MessageCaption_LoadError"),
                MessageDialogButtons.Ok, MessageDialogIcon.Error);
        }
        void ProjectManager_ProjectLoaded(object sender, EventArgs e)
        {
            AddRecent(Program.ProjectManager.FilePath);
            RefreshRecents();
            SetStatus(Program.ResourceManager.GetString("Status_ProjectLoadedSuccessfuly"), StatusType.Good);
        }
        void ProjectManager_ProjectDidntSave(object sender, ErrorEventArgs e)
        {
            SetStatus(Program.ResourceManager.GetString("Status_ProjectDidntSave"), StatusType.Error);
            MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_ErrorWhenSavingProject") + ":\n" + e.GetException().Message,
                Program.ResourceManager.GetString("MessageCaption_SaveError"),
                MessageDialogButtons.Ok, MessageDialogIcon.Error);
        }
        void ProjectManager_ProjectSaved(object sender, EventArgs e)
        {
            AddRecent(Program.ProjectManager.FilePath);
            RefreshRecents();
            SetStatus(Program.ResourceManager.GetString("Status_ProjectSavedSuccessfuly"), StatusType.Good);
        }

        bool SaveProject()
        {
            if (File.Exists(Program.ProjectManager.FilePath))
            {
                if (Program.ProjectManager.Save())
                {
                    Save = false;
                    currentHistory = listBox_history.SelectedIndex;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                SaveFileDialog sav = new SaveFileDialog();
                sav.Title = Program.ResourceManager.GetString("Title_SaveProject");
                sav.Filter = Filters.ASMP;
                if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    if (Program.ProjectManager.Save(sav.FileName))
                    {
                        Save = false;
                        currentHistory = listBox_history.SelectedIndex;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
        void ApplyProject()
        {
            //Clear all
            ClearAll();
            ClearHistory();
            AddHistory(Program.ResourceManager.GetString("History_ProjectOpened"));
            //subtitle tracks
            // If we have a backup translation track, rename it !
            foreach (SubtitlesTrack trk in Program.ProjectManager.Project.SubtitleTracks)
            {
                if (trk.Name == translation_temp_track)
                {
                    trk.Name = "translation_backup_retrieved";
                    break;
                }
            }
            RefreshSubtitleTracks();
            //Log
            richTextBox_Log.Text = Program.ProjectManager.Project.Log;
            //Description
            richTextBox_description.Text = Program.ProjectManager.Project.Description;
            //Media
            ChangeMedia(Program.ProjectManager.Project.MediaPath);
            // Prepared Text
            toolStripButton33.Checked = Program.ProjectManager.Project.UsePreparedText;
            toolStripButton34.Checked = Program.ProjectManager.Project.CutPreparedTextAfterAdd;
            toolStripButton_prepared_word_wrap.Checked = Program.ProjectManager.Project.WordWrapPreparedText;
            preparedTextEditor.RefreshText(Program.ProjectManager.Project.PreparedTextRTF);
            preparedTextEditor.WordWrap = Program.ProjectManager.Project.WordWrapPreparedText;
            //marks
            timeLine1.TimeMarks = Program.ProjectManager.Project.TimeMarks;
            RefreshMarks();
            // Styles
            if (Program.ProjectManager.Project.Styles == null)
                Program.ProjectManager.Project.RebuildStyles();
            subtitleEditor1.RefreshStyles(Program.ProjectManager.Project.Styles.ToArray());
            preparedTextEditor.RefreshStyles(Program.ProjectManager.Project.Styles.ToArray());
            mediaPlayer1.RefreshStyles(Program.ProjectManager.Project.Styles.ToArray());
            // Reset auto save timer
            autoSaveTimerSeconds = Program.Settings.AutoSavePeriodMinutes * 60;
        }
        void LoadProject(string FilePath)
        {
            if (!AskToSave())
                return;
            if (Program.ProjectManager.Load(FilePath))
            {
                ApplyProject();
                Save = false;
                if (Program.Settings.AutoSelectFirstTrack)
                {
                    if (treeView_subtitleTracks.Nodes.Count > 0)
                        treeView_subtitleTracks.SelectedNode = treeView_subtitleTracks.Nodes[0];
                }
            }
        }
        void ChangeMedia(string FilePath)
        {
            ChangeMedia(FilePath, true);
        }
        void ChangeMedia(string FilePath, bool warn)
        {
            if (mediaPlayer1 == null)
                return;
            mediaPlayer1.LoadMedia(FilePath);
            WaveReader.ClearBuffer();
            timeLine1.MediaText = FilePath;
            Program.ProjectManager.Project.MediaPath = FilePath;
            Timer_Player.Start();
            iD3TagsSynchronizedLyricsToolStripMenuItem.Enabled =
                previewWithAHDSynchronisedLyricsViewToolStripMenuItem.Enabled =
                Path.GetExtension(FilePath).ToLower() == ".mp3";

            if (warn && File.Exists(FilePath))
            {
                if (Path.GetExtension(FilePath).ToLower() == ".mp3")
                {
                    if (Program.Settings.WarnMeWhenID3Detected)
                    {
                        MessageDialogResult result = MessageDialog.ShowMessage(this, Program.ResourceManager.GetString("Message_ID3Detected"),
                            Program.ResourceManager.GetString("MessageCaption_ID3Detected"),
                            MessageDialogButtons.OkNo | MessageDialogButtons.Checked,
                            MessageDialogIcon.Question, Program.Settings.WarnMeWhenID3Detected, Program.ResourceManager.GetString("Button_Yes"),
                             Program.ResourceManager.GetString("Button_No"),
                            "", Program.ResourceManager.GetString("MessageCheckBox_AlwaysWarnMeWhenID3Detected"));
                        Program.Settings.WarnMeWhenID3Detected = ((result & MessageDialogResult.Checked) == MessageDialogResult.Checked);
                        if ((result & MessageDialogResult.Ok) == MessageDialogResult.Ok)
                        {
                            iD3TagsSynchronizedLyricsToolStripMenuItem_Click(this, null);
                        }
                    }
                }
                if (Program.Settings.TimelineAutoGenerateWaveform)
                {
                    timeLine1.ShowWaveform(true);
                }
            }
        }
        void RefreshMarks()
        {
            ComboBox_marks.Items.Clear();
            foreach (TimeMark mark in Program.ProjectManager.Project.TimeMarks)
            {
                ComboBox_marks.Items.Add(mark.Name + " (" + mark.Time.ToString("F3") + ")");
            }
        }
        void SelectMarkAtTime(double time)
        {
            int index = -1;
            foreach (TimeMark mark in Program.ProjectManager.Project.TimeMarks)
            {
                if (time >= mark.Time)
                    index++;
            }
            ComboBox_marks.SelectedIndex = index;
        }
        #endregion
        #region Save/ Load settings
        void LoadSettings()
        {
            //languages
            for (int i = 0; i < Program.SupportedLanguages.Length / 3; i++)
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Text = Program.SupportedLanguages[i, 2];
                item.Checked = Program.SupportedLanguages[i, 0] == Program.Settings.Language;
                languageToolStripMenuItem.DropDownItems.Add(item);
            }
            try
            {
                this.Size = Program.Settings.Frm_Main_Size;
                this.Location = Program.Settings.Frm_Main_Location;
                if (Program.Settings.CurrentLayout == null)
                    Program.Settings.CurrentLayout = new LayoutPresent();
                ApplyLayout(Program.Settings.CurrentLayout);

                RefreshRecents();
                RefreshFindRecents();
                ApplySettings();
                needToSaveSettings = true;
            }
            catch (Exception ex)
            {
                MessageDialogResult result = MessageDialog.ShowMessage(this, Program.ResourceManager.GetString("Message_SettingsLoadError") + "\n" +
                       Program.ResourceManager.GetString("Title_SystemMessage") + "\n" + ex.Message + "\n" + ex.ToString(),
                       Program.ResourceManager.GetString("MessageCaption_Settings"), MessageDialogButtons.OkNoCancel,
                       MessageDialogIcon.Error, false, Program.ResourceManager.GetString("Button_ResetSettings"),
                       Program.ResourceManager.GetString("Button_RetryLoading"), Program.ResourceManager.GetString("Button_Abort"), "");
                if (result == MessageDialogResult.Ok)
                {
                    Program.Settings.Reset();
                    LoadSettings();
                }
                if (result == MessageDialogResult.No)
                {
                    LoadSettings();
                }
                if (result == MessageDialogResult.Cancel)
                {
                    needToSaveSettings = false;
                    Application.Exit();
                    return;
                }
            }
        }
        void SaveSettings()
        {
            Program.Settings.Frm_Main_Size = this.Size;
            Program.Settings.Frm_Main_Location = this.Location;
            Program.Settings.CurrentLayout.SplitterDistance1 = splitContainer1.SplitterDistance;
            Program.Settings.CurrentLayout.SplitterDistance2 = splitContainer2.SplitterDistance;
            Program.Settings.CurrentLayout.SplitterDistance3 = splitContainer3.SplitterDistance;
            Program.Settings.CurrentLayout.SplitterDistance4 = splitContainer4.SplitterDistance;
            Program.Settings.CurrentLayout.SplitterDistance5 = splitContainer5.SplitterDistance;
            Program.Settings.AskWhenChangingRTL = subtitleEditor1.AskWhenChangingRTLToApplyToAll;
            // Media player settings
            Program.Settings.MediaPlayerCustomColor = mediaPlayer1.CustomStyle.Color;
            Program.Settings.MediaPlayerCustomFont = mediaPlayer1.CustomStyle.Font;
            Program.Settings.MediaPlayerEnableSubtitlePreview = mediaPlayer1.EnableSubtitleView;
            Program.Settings.MediaPlayerUseSubtitleFormatting = mediaPlayer1.IsUsingSubtitleFormatting;

            Program.Settings.Save();
            //controls
            subtitlesDataEditor1.SaveSettings();
        }
        void ApplySettings()
        {
            //player
            //load media player
            MediaPlayerManager.LoadMediaPlayer(Program.Settings.MediaPlayerCurrent);
            //media control
            mediaPlayer1.AdvanceTime = Program.Settings.AdvanceTime;
            nextFrameToolStripMenuItem.Text = "+ " + Program.Settings.AdvanceTime.ToString("F3") + " sec";
            previousFrameToolStripMenuItem.Text = "- " + Program.Settings.AdvanceTime.ToString("F3") + " sec";
            mediaPlayer1.EditorBackColor = Program.Settings.SubtitleTextEditorBackColor;
            mediaPlayer1.ShowEditorStatusStrip = Program.Settings.SubtitleTextEditorShowStatusStrip;
            Timer_Player.Interval = Program.Settings.PlayerTimer;

            mediaPlayer1.ApplySettings(Program.Settings.MediaPlayerEnableSubtitlePreview, Program.Settings.MediaPlayerUseSubtitleFormatting,
                new ASMPFontStyle("Subtitle Preview", Program.Settings.MediaPlayerCustomFont, Program.Settings.MediaPlayerCustomColor));

            //timeline
            timeLine1.MegnaticAccuracy = Program.Settings.MegnaticAccuracy;
            timeLine1.SmartToolAccuracy = Program.Settings.SmartToolAccuracy;
            timeLine1.WaveFormQuality = Program.Settings.WaveFormQuality;
            //formats
            LoadFormats();
            //others
            subtitleEditor1.AskWhenChangingRTLToApplyToAll = Program.Settings.AskWhenChangingRTL;
            subtitleEditor1.EditorBackColor = Program.Settings.SubtitleTextEditorBackColor;
            subtitleEditor1.ShowStatusStrip = Program.Settings.SubtitleTextEditorShowStatusStrip;
            // id3 tag
            Program.LoadID3V2Settings();
            // auto save timer
            if (Program.Settings.AutoSaveEnabled)
            {
                autoSaveTimerSeconds = Program.Settings.AutoSavePeriodMinutes * 60;
                timer_autosave.Start();
            }
            else
            {
                timer_autosave.Stop();
            }
            // Hot keys
            hotKey1.RemoveAllKeys();
            if (Program.Settings.ShortcutAddSubtitle != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutAddSubtitle);
            if (Program.Settings.ShortcutPreviousSubtitle != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutPreviousSubtitle);
            if (Program.Settings.ShortcutNextSubtitle != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutNextSubtitle);
            if (Program.Settings.ShortcutJumpIntoSelectedSubtitleTime != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutJumpIntoSelectedSubtitleTime);
            if (Program.Settings.ShortcutFWD != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutFWD);
            if (Program.Settings.ShortcutREW != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutREW);

            if (Program.Settings.ShortcutStretchToNext != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutStretchToNext);
            if (Program.Settings.ShortcutStretchToPrevious != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutStretchToPrevious);

            // Finally, force reloading media file in media
            if (Program.ProjectManager.Project != null)
                ChangeMedia(Program.ProjectManager.Project.MediaPath, false);
        }
        void AddRecent(string fileName)
        {
            if (Program.Settings.RecentProject == null)
                Program.Settings.RecentProject = new System.Collections.Specialized.StringCollection();
            //remove the same one
            for (int i = 0; i < Program.Settings.RecentProject.Count; i++)
            {
                if (fileName == Program.Settings.RecentProject[i])
                {
                    Program.Settings.RecentProject.RemoveAt(i);
                    i = -1;
                }
            }
            //Insert
            Program.Settings.RecentProject.Insert(0, fileName);
            //Limit to 9
            if (Program.Settings.RecentProject.Count > 9)
            {
                Program.Settings.RecentProject.RemoveAt(6);
            }
        }
        void RefreshRecents()
        {
            if (Program.Settings.RecentProject == null)
                Program.Settings.RecentProject = new System.Collections.Specialized.StringCollection();
            recentToolStripMenuItem.DropDownItems.Clear();
            foreach (string Recc in Program.Settings.RecentProject)
            {
                ToolStripMenuItem IT = new ToolStripMenuItem();
                IT.Text = Path.GetFileNameWithoutExtension(Recc);
                IT.ToolTipText = Recc;
                recentToolStripMenuItem.DropDownItems.Add(IT);
            }
        }
        void AddFindRecent(string searchWord)
        {
            if (searchWord == "")
                return;
            if (!Program.Settings.RecentFindItems.Contains(searchWord))
                Program.Settings.RecentFindItems.Add(searchWord);
            //limit to 20 items
            if (Program.Settings.RecentFindItems.Count > 20)
                Program.Settings.RecentFindItems.RemoveAt(0);
            RefreshFindRecents();
        }
        void RefreshFindRecents()
        {
            ComboBox_search.Items.Clear();
            if (Program.Settings.RecentFindItems == null)
                Program.Settings.RecentFindItems = new System.Collections.Specialized.StringCollection();
            foreach (string item in Program.Settings.RecentFindItems)
                ComboBox_search.Items.Add(item);
            ComboBox_search.SelectedIndex = ComboBox_search.Items.Count - 1;
        }
        void LoadFormats()
        {
            if (Program.Settings.EnabledFormats == null)
                Program.Settings.EnabledFormats = new BooleansCollection();
            if (Program.Settings.EnabledFormats.Count != SubtitleFormats.Formats.Length)
            {
                Program.Settings.EnabledFormats = new BooleansCollection();
                foreach (SubtitlesFormat format in SubtitleFormats.Formats)
                {
                    Program.Settings.EnabledFormats.Add(true);
                }
            }
            for (int i = 0; i < Program.Settings.EnabledFormats.Count; i++)
            {
                SubtitleFormats.Formats[i].Enabled = Program.Settings.EnabledFormats[i];
            }
        }
        #endregion
        #region Layout Present

        void ApplyLayout(LayoutPresent layout)
        {
            if (layout == null)
                return;
            //menu items
            mediaToolStripMenuItem.Checked = layout.Tab_Media.Visible;
            subtitlesDataToolStripMenuItem.Checked = layout.Tab_SubtitlesData.Visible;
            subtitleTracksToolStripMenuItem.Checked = layout.Tab_SubtitleTracks.Visible;
            projectDescriptionToolStripMenuItem.Checked = layout.Tab_ProjectDescription.Visible;
            historyToolStripMenuItem.Checked = layout.Tab_History.Visible;
            logToolStripMenuItem.Checked = layout.Tab_Log.Visible;
            errorsToolStripMenuItem1.Checked = layout.Tab_Errors.Visible;
            timelineToolStripMenuItem.Checked = layout.Tab_TimeLine.Visible;
            propertiesToolStripMenuItem2.Checked = layout.Tab_Properties.Visible;
            multipleSubtitleTracksViewerToolStripMenuItem.Checked = layout.Tab_MultipleSubtitleTracksViewer.Visible;
            preparedTextToolStripMenuItem.Checked = layout.Tab_PreparedText.Visible;
            //tabs
            SetTabParent(layout.Tab_Errors, tabPage_errors);
            SetTabParent(layout.Tab_History, tabPage_history);
            SetTabParent(layout.Tab_Log, tabPage_log);
            SetTabParent(layout.Tab_Media, tabPage_media);
            SetTabParent(layout.Tab_ProjectDescription, tabPage_ProjectDescription);
            SetTabParent(layout.Tab_Properties, tabPage_Properties);
            SetTabParent(layout.Tab_SubtitlesData, tabPage_subitltesData);
            SetTabParent(layout.Tab_SubtitleTracks, tabPage_subtitleTracks);
            SetTabParent(layout.Tab_TimeLine, tabPage_TimeLine);
            SetTabParent(layout.Tab_MultipleSubtitleTracksViewer, tabPage_multipleSubtitleTrackViewer);
            SetTabParent(layout.Tab_PreparedText, tabPage_preparedText);
            //toolbars
            toolStrip_edit.Visible = editToolStripMenuItem1.Checked = layout.Bar_Edit.Visible;
            SetBarParent(layout.Bar_Edit, toolStrip_edit);
            toolStrip_main.Visible = mainToolStripMenuItem.Checked = layout.Bar_Main.Visible;
            SetBarParent(layout.Bar_Main, toolStrip_main);
            toolStrip_marks.Visible = marksToolStripMenuItem1.Checked = layout.Bar_Marks.Visible;
            SetBarParent(layout.Bar_Marks, toolStrip_marks);
            try
            {
                //containers
                splitContainer1.SplitterDistance = layout.SplitterDistance1;
                splitContainer2.SplitterDistance = layout.SplitterDistance2;
                splitContainer3.SplitterDistance = layout.SplitterDistance3;
                splitContainer4.SplitterDistance = layout.SplitterDistance4;
                splitContainer5.SplitterDistance = layout.SplitterDistance5;
            }
            catch (Exception ex)
            {

            }
            CheckForCollapse();
        }

        void SaveLayout(string FilePath, LayoutPresent layout)
        {
            try
            {
                Stream str = new FileStream(FilePath, FileMode.Create, FileAccess.Write);
                XmlSerializer ser = new XmlSerializer(typeof(LayoutPresent));
                ser.Serialize(str, layout);
                str.Close();
            }
            catch
            { }
        }
        LayoutPresent LoadLayout(string FilePath)
        {
            try
            {
                LayoutPresent layout;
                Stream str = new FileStream(FilePath, FileMode.Open, FileAccess.Read);
                XmlSerializer ser = new XmlSerializer(typeof(LayoutPresent));
                layout = (LayoutPresent)ser.Deserialize(str);
                str.Close();
                return layout;
            }
            catch
            { }
            return null;
        }
        void SetTabParent(TabProperties properties, TabPage page)
        {
            if (!properties.Visible) { page.Parent = null; }
            else
            {
                switch (properties.Parent)
                {
                    case TabParent.Down: page.Parent = tabControl_down_middle; break;
                    case TabParent.DownLeft: page.Parent = tabControl_down_left; break;
                    case TabParent.DownRight: page.Parent = tabControl_down_Right; break;
                    case TabParent.Top: page.Parent = tabControl_top_middle; break;
                    case TabParent.TopLeft: page.Parent = tabControl_top_left; break;
                    case TabParent.TopRight: page.Parent = tabControl_top_right; break;
                    case TabParent.None:
                    default: page.Parent = null; break;
                }
            }
        }
        void SetBarParent(ToolbarProperties properties, ToolStrip toolstrip)
        {
            switch (properties.Parent)
            {
                case ToolbarParent.Top: toolstrip.Parent = toolStripContainer1.TopToolStripPanel; break;
                case ToolbarParent.Right: toolstrip.Parent = toolStripContainer1.RightToolStripPanel; break;
                case ToolbarParent.Left: toolstrip.Parent = toolStripContainer1.LeftToolStripPanel; break;
                case ToolbarParent.Bottom: toolstrip.Parent = toolStripContainer1.BottomToolStripPanel; break;
            }
            toolstrip.Location = properties.Location;
        }

        void CheckForCollapse()
        {
            //clear view tab
            if (tabControl_down_left.TabPages.Count > 0)
                if (tabControl_down_left.TabPages[0].Text == "")
                    tabControl_down_left.TabPages.RemoveAt(0);
            if (tabControl_down_middle.TabPages.Count > 0)
                if (tabControl_down_middle.TabPages[0].Text == "")
                    tabControl_down_middle.TabPages.RemoveAt(0);
            if (tabControl_down_Right.TabPages.Count > 0)
                if (tabControl_down_Right.TabPages[0].Text == "")
                    tabControl_down_Right.TabPages.RemoveAt(0);
            if (tabControl_top_left.TabPages.Count > 0)
                if (tabControl_top_left.TabPages[0].Text == "")
                    tabControl_top_left.TabPages.RemoveAt(0);
            if (tabControl_top_middle.TabPages.Count > 0)
                if (tabControl_top_middle.TabPages[0].Text == "")
                    tabControl_top_middle.TabPages.RemoveAt(0);
            if (tabControl_top_right.TabPages.Count > 0)
                if (tabControl_top_right.TabPages[0].Text == "")
                    tabControl_top_right.TabPages.RemoveAt(0);

            //Right Area
            if (tabControl_top_right.TabCount == 0)
                splitContainer2.Panel1Collapsed = true;
            if (tabControl_down_Right.TabCount == 0)
                splitContainer2.Panel2Collapsed = true;
            if ((tabControl_top_right.TabCount == 0) & (tabControl_down_Right.TabCount == 0))
                splitContainer1.Panel2Collapsed = true;
            //Left area
            //TOP
            if (tabControl_top_middle.TabCount == 0)
                splitContainer4.Panel2Collapsed = true;
            if (tabControl_top_left.TabCount == 0)
                splitContainer4.Panel1Collapsed = true;
            if ((tabControl_top_middle.TabCount == 0) & (tabControl_top_left.TabCount == 0))
                splitContainer3.Panel1Collapsed = true;
            //DOWN
            if (tabControl_down_middle.TabCount == 0)
                splitContainer5.Panel2Collapsed = true;
            if (tabControl_down_left.TabCount == 0)
                splitContainer5.Panel1Collapsed = true;
            if ((tabControl_down_middle.TabCount == 0) & (tabControl_down_left.TabCount == 0))
                splitContainer3.Panel2Collapsed = true;
            //
            if ((tabControl_top_middle.TabCount == 0) & (tabControl_top_left.TabCount == 0) &
                (tabControl_down_middle.TabCount == 0) & (tabControl_down_left.TabCount == 0))
                splitContainer1.Panel1Collapsed = true;
        }
        void CheckDragAndDrop()
        {
            if (tabControl_down_left.TabCount == 0)
                tabControl_down_left.TabPages.Add("");
            if (tabControl_down_middle.TabCount == 0)
                tabControl_down_middle.TabPages.Add("");
            if (tabControl_down_Right.TabCount == 0)
                tabControl_down_Right.TabPages.Add("");
            if (tabControl_top_left.TabCount == 0)
                tabControl_top_left.TabPages.Add("");
            if (tabControl_top_middle.TabCount == 0)
                tabControl_top_middle.TabPages.Add("");
            if (tabControl_top_right.TabCount == 0)
                tabControl_top_right.TabPages.Add("");
        }
        void ExpandTabControls()
        {
            splitContainer1.Panel1Collapsed = false;
            splitContainer1.Panel2Collapsed = false;
            splitContainer2.Panel1Collapsed = false;
            splitContainer2.Panel2Collapsed = false;
            splitContainer3.Panel1Collapsed = false;
            splitContainer3.Panel2Collapsed = false;
            splitContainer4.Panel1Collapsed = false;
            splitContainer4.Panel2Collapsed = false;
            splitContainer5.Panel1Collapsed = false;
            splitContainer5.Panel2Collapsed = false;
        }
        void SaveTabParent(TabPage page, TabParent newParent)
        {
            if (page == tabPage_errors)
                Program.Settings.CurrentLayout.Tab_Errors.Parent = newParent;
            else if (page == tabPage_history)
                Program.Settings.CurrentLayout.Tab_History.Parent = newParent;
            else if (page == tabPage_log)
                Program.Settings.CurrentLayout.Tab_Log.Parent = newParent;
            else if (page == tabPage_media)
                Program.Settings.CurrentLayout.Tab_Media.Parent = newParent;
            else if (page == tabPage_ProjectDescription)
                Program.Settings.CurrentLayout.Tab_ProjectDescription.Parent = newParent;
            else if (page == tabPage_Properties)
                Program.Settings.CurrentLayout.Tab_Properties.Parent = newParent;
            else if (page == tabPage_subitltesData)
                Program.Settings.CurrentLayout.Tab_SubtitlesData.Parent = newParent;
            else if (page == tabPage_subtitleTracks)
                Program.Settings.CurrentLayout.Tab_SubtitleTracks.Parent = newParent;
            else if (page == tabPage_TimeLine)
                Program.Settings.CurrentLayout.Tab_TimeLine.Parent = newParent;
            else if (page == tabPage_multipleSubtitleTrackViewer)
                Program.Settings.CurrentLayout.Tab_MultipleSubtitleTracksViewer.Parent = newParent;
            else if (page == tabPage_preparedText)
                Program.Settings.CurrentLayout.Tab_PreparedText.Parent = newParent;
        }
        ToolbarParent GetBarParent(ToolStrip bar)
        {
            if (bar.Parent == toolStripContainer1.TopToolStripPanel)
                return ToolbarParent.Top;
            if (bar.Parent == toolStripContainer1.BottomToolStripPanel)
                return ToolbarParent.Bottom;
            if (bar.Parent == toolStripContainer1.LeftToolStripPanel)
                return ToolbarParent.Left;
            if (bar.Parent == toolStripContainer1.RightToolStripPanel)
                return ToolbarParent.Right;
            return ToolbarParent.Top;
        }
        #endregion
        #region History and log
        void AddHistory(string title)
        {
            //remove uneeded events
            int oldIndex = listBox_history.SelectedIndex;
            for (int i = oldIndex + 1; i < listBox_history.Items.Count; i++)
            {
                listBox_history.Items.RemoveAt(i);
                i = oldIndex;
            }
            //add event
            listBox_history.Items.Add(title);
            listBox_history.SelectedIndex = listBox_history.Items.Count - 1;
            //Save the event into project file
            ProjectManager historyManager = new ProjectManager();
            historyManager.Project = Program.ProjectManager.Project;
            //historyManager.Save(Path.GetTempPath() + "\\ASM\\HIST" + listBox_history.SelectedIndex);
            historyManager.Save(Program.TempPath + "\\HIST" + listBox_history.SelectedIndex);
            //log
            WriteLog(DateTime.Now.ToShortDateString() + " " +
                DateTime.Now.ToShortTimeString() + ": " + title);
        }
        void WriteLog(string text)
        {
            richTextBox_Log.SelectionStart = richTextBox_Log.Text.Length;
            richTextBox_Log.SelectedText = text + "\n";
            richTextBox_Log.ScrollToCaret();
        }
        //open history at history listbox index
        void OpenHistory()
        {
            if (listBox_history.SelectedIndex == -1)
                return;
            //load the project as history
            string path = Program.ProjectManager.FilePath;
            string oldMediaPath = Program.ProjectManager.Project.MediaPath;
            //Program.ProjectManager.Load(Path.GetTempPath() + "\\ASM\\HIST" + listBox_history.SelectedIndex, false);
            Program.ProjectManager.Load(Program.TempPath + "\\HIST" + listBox_history.SelectedIndex, false);
            Program.ProjectManager.FilePath = path;
            //apply
            //save selection
            int selectedNodeIndex = 0;
            int selectedSubtitle = -1;
            if (treeView_subtitleTracks.SelectedNode != null)
                selectedNodeIndex = treeView_subtitleTracks.SelectedNode.Index;
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 1)
                selectedSubtitle = subtitlesDataEditor1.SelectedSubtitleItems[0].Index;
            //clear
            ClearAll(false);
            //refresh
            RefreshSubtitleTracks();
            //reselect track
            try { treeView_subtitleTracks.SelectedNode = treeView_subtitleTracks.Nodes[selectedNodeIndex]; }
            catch { }
            //reselect subtitle
            if (selectedSubtitle >= 0 & subtitlesDataEditor1.SubtitlesCount > selectedSubtitle)
                subtitlesDataEditor1.SelectItem(selectedSubtitle);
            //others
            richTextBox_description.Text = Program.ProjectManager.Project.Description;
            if (Program.ProjectManager.Project.MediaPath != oldMediaPath)
            {
                mediaPlayer1.ClearMedia();
                ChangeMedia(Program.ProjectManager.Project.MediaPath, false);
            }
            timeLine1.TimeMarks = Program.ProjectManager.Project.TimeMarks;
            RefreshMarks();
            // prepared text
            preparedTextEditor.RefreshText(Program.ProjectManager.Project.PreparedTextRTF);

            if (listBox_history.SelectedIndex != currentHistory)
                Save = true;
            else
                Save = false;
        }
        void ClearHistory()
        {
            if (!Directory.Exists(Program.TempPath))
            { Directory.CreateDirectory(Program.TempPath); return; }
            for (int i = 0; i < Directory.GetFiles(Program.TempPath).Length; i++)
            {
                File.Delete(Directory.GetFiles(Program.TempPath)[0]);
                i = -1;
            }
            listBox_history.Items.Clear();
        }
        void EnableUndoRedo()
        {
            if (listBox_history.SelectedIndex == -1)
                undoToolStripMenuItem.Enabled = redoToolStripMenuItem.Enabled = false;

            undoToolStripMenuItem.Enabled = listBox_history.SelectedIndex > 0;

            redoToolStripMenuItem.Enabled = listBox_history.SelectedIndex < listBox_history.Items.Count - 1;
        }
        #endregion
        #region SAVE
        bool save;
        bool Save
        {
            get { return save; }
            set
            {
                save = value;
                if (value)
                    this.Text = Program.ProjectManager.Project.Name + "* - AHD Subtitles Maker";
                else
                    this.Text = Program.ProjectManager.Project.Name + " - AHD Subtitles Maker";
            }
        }
        bool AskToSave()
        {
            if (save)
            {
                MessageDialogResult result = MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_DoYouWantToSaveProjectFirst")
                    , Program.ResourceManager.GetString("MessageCaption_SaveProject"),
                    MessageDialogButtons.OkNoCancel, MessageDialogIcon.Save, false, Program.ResourceManager.GetString("Button_Save"),
                   Program.ResourceManager.GetString("Button_DontSave"), Program.ResourceManager.GetString("Button_Cancel"));
                switch (result)
                {
                    case MessageDialogResult.Ok:
                        if (SaveProject())
                            return true;
                        return false;
                    case MessageDialogResult.No: return true;
                    case MessageDialogResult.Cancel: return false;
                }
            }
            return true;
        }
        #endregion
        #region Status
        enum StatusType { Error, Good, Warning, None };
        void SetStatus(string status, StatusType statusType)
        {
            StatusLabel.Text = status;
            switch (statusType)
            {
                case StatusType.None: StatusLabel.ForeColor = Color.Black; break;
                case StatusType.Good: StatusLabel.ForeColor = Color.Green; break;
                case StatusType.Error: StatusLabel.ForeColor = Color.Red; break;
                case StatusType.Warning: StatusLabel.ForeColor = Color.Yellow; break;
            }
        }
        #endregion

        #region Refreshs and clears
        /*Clear everything in tabs*/
        void ClearAll()
        { ClearAll(true); }
        void ClearAll(bool ClearMedia)
        {
            treeView_subtitleTracks.Nodes.Clear();
            if (ClearMedia)
            {
                mediaPlayer1.ClearMedia();
                timeLine1.MediaText = "";
                timeLine1.MediaDuration = 0;
                subtitlesDataEditor1.MediaDuration = 0;
            }
            mediaPlayer1.HideSubtitle();
            subtitlesDataEditor1.Clear();
            subtitleEditor1.Clear();
            ComboBox_marks.Items.Clear();
            if (timeLine1.TimeMarks != null)
                timeLine1.TimeMarks.Clear();
            errorsChecker1.SubtitlesTrack = timeLine1.SubtitlesTrack = subtitleEditor1.SubtitlesTrack =
                subtitlesDataEditor1.SubtitlesTrack = null;
        }
        void ClearEditors()
        {
            subtitlesDataEditor1.Clear();
            subtitleEditor1.Clear();
            timeLine1.SubtitlesTrack = errorsChecker1.SubtitlesTrack =
                subtitleEditor1.SubtitlesTrack = subtitlesDataEditor1.SubtitlesTrack = null;
        }
        void RefreshSubtitleTracks()
        {
            selectedTrack = null;
            treeView_subtitleTracks.Nodes.Clear();
            foreach (SubtitlesTrack track in Program.ProjectManager.Project.SubtitleTracks)
            {
                TreeNode_SubtitlesTrack node = new TreeNode_SubtitlesTrack();
                node.SubtitlesTrack = track;
                treeView_subtitleTracks.Nodes.Add(node);
            }
            multipleSubtitleTrackViewer.RefreshTracks(Program.ProjectManager.Project.SubtitleTracks.ToArray());
            timeLine1.RefreshSubtitleTracksList(Program.ProjectManager.Project.SubtitleTracks.ToArray());
        }
        #endregion
        #region Add items
        private void AddSubtitlesTracks(object sender, EventArgs e)
        {
            int i = 1;
            string name = Program.ResourceManager.GetString("SubtitlesTrack") + i;
            while (Program.ProjectManager.Project.IsSubtitlesTrackExist(name))
            {
                i++;
                name = Program.ResourceManager.GetString("SubtitlesTrack") + i;
            }
            SubtitlesTrack newTrack = new SubtitlesTrack(name);
            //template
            if (Program.Settings.NewSubtitlesTrackTemplate)
            {
                //add subtitle at begaining
                Subtitle sub = new Subtitle(1, 3, SubtitleText.FromString("Subtitles made by AHD Subtitles Maker\nE-mail: alahadid@hotmail.com"));
                newTrack.Subtitles.Add(sub);
                //add at the end
                if (mediaPlayer1.Duration > 0)
                {
                    sub = new Subtitle(mediaPlayer1.Duration - 4, mediaPlayer1.Duration - 2,
                        SubtitleText.FromString("Subtitles made by AHD Subtitles Maker\nE-mail: alahadid@hotmail.com"));
                    newTrack.Subtitles.Add(sub);
                }
                else//no media, add after 10 second from the first one
                {
                    sub = new Subtitle(11, 13,
                       SubtitleText.FromString("Subtitles made by AHD Subtitles Maker\nE-mail: alahadid@hotmail.com"));
                    newTrack.Subtitles.Add(sub);
                }
            }
            Program.ProjectManager.Project.SubtitleTracks.Add(newTrack);
            RefreshSubtitleTracks();
            treeView_subtitleTracks.Nodes[treeView_subtitleTracks.Nodes.Count - 1].BeginEdit();
            Save = true;
            AddHistory(Program.ResourceManager.GetString("History_SubtitlesTrackAdded") + " '" + name + "'");
        }
        private void AddSubtitle(object sender, EventArgs e)
        {
            if (selectedTrack == null || treeView_subtitleTracks.SelectedNode == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                    Program.ResourceManager.GetString("MessageCaption_AddSubtitle"));
                return;
            }
            // Create the subtitle text
            SubtitleText subtext = Program.Settings.AddNewSubtitleTextWhenAddNewSubtitle ?
               SubtitleText.FromString(Program.ResourceManager.GetString("Title_NewSubtitle"),
               Program.Settings.NewSubtitleFont, Program.Settings.NewSubtitleColor)
               :
               SubtitleText.FromString(" ", Program.Settings.NewSubtitleFont, Program.Settings.NewSubtitleColor);
            // Create subtitle
            Subtitle sub = new Subtitle(mediaPlayer1.CurrentPosition, mediaPlayer1.CurrentPosition +
                Program.Settings.NewSubtitleDuration, subtext);
            ListViewItem_Subtitle item = new ListViewItem_Subtitle();
            sub.Text.RighttoLeft = selectedTrack.RightToLeft;
            item.Subtitle = sub;
            Frm_SubtitleEdit subtitleEdit = new Frm_SubtitleEdit(item, selectedTrack,
                Program.Settings.SubtitleTextEditorBackColor
                , Program.Settings.AllowErrorsInEditControl, Program.ProjectManager.Project.UsePreparedText,
                Program.ProjectManager.Project.PreparedTextRTF,
                Program.ProjectManager.Project.CutPreparedTextAfterAdd,
                Program.ProjectManager.Project.WordWrapPreparedText);
            subtitleEdit.EditStylesRequest += SubtitleEditor1_EditStylesRequest;
            subtitleEdit.RefreshStyles(Program.ProjectManager.Project.Styles.ToArray());
            subtitleEdit.SelectAll();
            if (subtitleEdit.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (Program.ProjectManager.Project.UsePreparedText && Program.ProjectManager.Project.CutPreparedTextAfterAdd)
                {
                    Program.ProjectManager.Project.PreparedTextRTF = subtitleEdit.PreparedTextRTFAfterChange;
                    preparedTextEditor.RefreshText(Program.ProjectManager.Project.PreparedTextRTF);
                }
                selectedTrack.Subtitles.Add(item.Subtitle);
                selectedTrack.Subtitles.Sort(new SubtitleComparer());
                subtitlesDataEditor1.RefreshSubtitles();
                timeLine1.UpdateSubtitlesReview(); Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SubtitleAddedAt") + " " +
                    TimeFormatConvertor.To_TimeSpan_Milli(sub.StartTime));
                ApplyTrackToControls();
            }
            pIndex = -1;
        }
        private void InsertSubtitle(object sender, EventArgs e)
        {
            if (selectedTrack == null || treeView_subtitleTracks.SelectedNode == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                   Program.ResourceManager.GetString("MessageCaption_InsertSubtitle"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length != 1)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitleFirst"),
                         Program.ResourceManager.GetString("MessageCaption_InsertSubtitle"));
                return;
            }
            Subtitle sub = new Subtitle(subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.EndTime,
                subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.EndTime + Program.Settings.NewSubtitleDuration, new SubtitleText());
            ListViewItem_Subtitle item = new ListViewItem_Subtitle();
            sub.Text.RighttoLeft = selectedTrack.RightToLeft;
            item.Subtitle = sub;
            Frm_SubtitleEdit subtitleEdit = new Frm_SubtitleEdit(item, selectedTrack,
                Program.Settings.SubtitleTextEditorBackColor, Program.Settings.AllowErrorsInEditControl,
                Program.ProjectManager.Project.UsePreparedText,
                Program.ProjectManager.Project.PreparedTextRTF,
                Program.ProjectManager.Project.CutPreparedTextAfterAdd,
                Program.ProjectManager.Project.WordWrapPreparedText);
            subtitleEdit.EditStylesRequest += SubtitleEditor1_EditStylesRequest;
            subtitleEdit.RefreshStyles(Program.ProjectManager.Project.Styles.ToArray());
            if (subtitleEdit.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (Program.ProjectManager.Project.UsePreparedText && Program.ProjectManager.Project.CutPreparedTextAfterAdd)
                {
                    Program.ProjectManager.Project.PreparedTextRTF = subtitleEdit.PreparedTextRTFAfterChange;
                    preparedTextEditor.RefreshText(Program.ProjectManager.Project.PreparedTextRTF);
                }
                selectedTrack.Subtitles.Add(item.Subtitle);
                selectedTrack.Subtitles.Sort(new SubtitleComparer());
                subtitlesDataEditor1.RefreshSubtitles();
                timeLine1.UpdateSubtitlesReview(); Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SubtitleInsertedAt") +
                    " " + TimeFormatConvertor.To_TimeSpan_Milli(sub.StartTime));
                ApplyTrackToControls();
            }
            pIndex = -1;
        }
        #endregion
        #region Edit and delete

        private void Delete()
        {
            switch (activeMode)
            {
                case ActiveMode.None: return;
                case ActiveMode.Properties:
                    {
                        this.subtitleEditor1.DeleteSelected();
                        break;
                    }
                case ActiveMode.SubtitlesTrack:
                    {
                        if (treeView_subtitleTracks.SelectedNode == null)
                        {
                            SetStatus(Program.ResourceManager.GetString("Message_CantDeleteNoSubtitlesTrackSelected"), StatusType.Error);
                            return;
                        }

                        if (MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_AreYouSure"),
                           Program.ResourceManager.GetString("MessageCaption_DeleteSelectedSubtitlesTrack"),
                            MessageDialogButtons.OkNo, MessageDialogIcon.Question, false, Program.ResourceManager.GetString("Button_Yes"),
                           Program.ResourceManager.GetString("Button_No")) == MessageDialogResult.Ok)
                        {
                            Program.ProjectManager.Project.SubtitleTracks.Remove(((TreeNode_SubtitlesTrack)treeView_subtitleTracks.SelectedNode).SubtitlesTrack);
                            ClearEditors();
                            RefreshSubtitleTracks();
                            Save = true;
                            AddHistory(Program.ResourceManager.GetString("History_SubtitlesTrackRemove."));
                        }

                        ApplyTrackToControls();
                        EnableDisableEditButtons();

                        break;
                    }
                case ActiveMode.Subtitles:
                    {
                        if (subtitlesDataEditor1.SelectedSubtitleItems.Length > 0)
                        {
                            if (Program.Settings.AskBeforeDeletingSubtitle)
                            {
                                MessageDialogResult result = MessageDialog.ShowMessage(this, Program.ResourceManager.GetString("Message_AreYouSureYouWantToDeleteSelected") + " " +
                                    subtitlesDataEditor1.SelectedSubtitleItems.Length + " " +
                                    Program.ResourceManager.GetString("Message_Subtitles") + " ?",
                                    Program.ResourceManager.GetString("MessageCaption_DeleteSubtitles"),
                                    MessageDialogButtons.OkNo | MessageDialogButtons.Checked, MessageDialogIcon.Question,
                                    Program.Settings.AskBeforeDeletingSubtitle, Program.ResourceManager.GetString("Button_Yes"),
                                    Program.ResourceManager.GetString("Button_No"), "",
                                    Program.ResourceManager.GetString("CheckBox_AskEveryTimeBeforeDeletingSubtitle"));

                                if ((result & MessageDialogResult.Ok) == MessageDialogResult.Ok)
                                {
                                    foreach (ListViewItem_Subtitle item in subtitlesDataEditor1.SelectedSubtitleItems)
                                    {
                                        selectedTrack.Subtitles.Remove(item.Subtitle);
                                    }
                                    Program.Settings.AskBeforeDeletingSubtitle = (result & MessageDialogResult.Checked)
                                        == MessageDialogResult.Checked;
                                    subtitlesDataEditor1.RefreshSubtitles();
                                    timeLine1.UpdateSubtitlesReview();
                                    timeLine1.Invalidate();
                                    subtitleEditor1.Clear();
                                    Save = true;
                                    AddHistory(Program.ResourceManager.GetString("History_SubtitlesDelete"));
                                }
                            }
                            else
                            {
                                // Just delete them !
                                foreach (ListViewItem_Subtitle item in subtitlesDataEditor1.SelectedSubtitleItems)
                                {
                                    selectedTrack.Subtitles.Remove(item.Subtitle);
                                }
                                subtitlesDataEditor1.RefreshSubtitles();
                                timeLine1.UpdateSubtitlesReview(); 
                                timeLine1.Invalidate();
                                subtitleEditor1.Clear();
                                Save = true;
                                AddHistory(Program.ResourceManager.GetString("History_SubtitlesDelete"));
                            }
                        }
                        else
                            SetStatus(Program.ResourceManager.GetString("Status_CantDeleteNoSubtitleSelected"), StatusType.Error);

                        ApplyTrackToControls();
                        EnableDisableEditButtons();

                        break;
                    }
            }

        }
        private void Cut(object sender, EventArgs e)
        {
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length > 0)
            {
                Copy(sender, e);
                //delete
                foreach (ListViewItem_Subtitle item in subtitlesDataEditor1.SelectedSubtitleItems)
                {
                    selectedTrack.Subtitles.Remove(item.Subtitle);
                }
                subtitlesDataEditor1.RefreshSubtitles();
                timeLine1.UpdateSubtitlesReview();
                Save = true;
                AddHistory(copiedSubtitles.Count + " " + Program.ResourceManager.GetString("History_SubtitlesCut"));
            }
            else
            {
                SetStatus(Program.ResourceManager.GetString("Status_CantCopyOrCutNoSubtitleSelected"), StatusType.Error);
            }
            EnableDisableEditButtons();
            ApplyTrackToControls();
        }
        private void Copy(object sender, EventArgs e)
        {
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length > 0)
            {
                copiedSubtitles = new List<Subtitle>();
                foreach (ListViewItem_Subtitle item in subtitlesDataEditor1.SelectedSubtitleItems)
                {
                    copiedSubtitles.Add(item.Subtitle.Clone());
                }
                SetStatus(copiedSubtitles.Count + " " + Program.ResourceManager.GetString("Status_SubtitlesCopied"), StatusType.None);
            }
            else
            {
                SetStatus(Program.ResourceManager.GetString("Status_CantCopyOrCutNoSubtitleSelected"), StatusType.Error);
            }
            EnableDisableEditButtons();
        }
        private void Paste(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                SetStatus(Program.ResourceManager.GetString("Status_CantPasteNoSubtitesTrackSelected"), StatusType.Error);
                return;
            }
            int SubIndex = -1;
            int Pastedd = 0;
            int Removed = 0;
            double mooo = 0;
            bool ShouldRemove = false;
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length > 0)
            {
                MessageDialogResult result = MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_WhereToPasteSubtitles"),
                        Program.ResourceManager.GetString("MessageCaption_PasteSubtitles"),
                        MessageDialogButtons.OkNoCancel, MessageDialogIcon.Question, false, Program.ResourceManager.GetString("Button_Insert"),
                      Program.ResourceManager.GetString("Button_OnTime"), Program.ResourceManager.GetString("Button_Cancel")
                          );
                if (result == MessageDialogResult.Cancel)
                { return; }
                if (result == MessageDialogResult.Ok)
                {
                    ListViewItem_Subtitle suuu = subtitlesDataEditor1.SelectedSubtitleItems[subtitlesDataEditor1.SelectedSubtitleItems.Length - 1];
                    mooo = suuu.Subtitle.EndTime + 0.001;
                }
                else
                {
                    mooo = mediaPlayer1.CurrentPosition;
                }
                ShouldRemove = true;
            }
            else
            {
                mooo = mediaPlayer1.CurrentPosition;
                ShouldRemove = false;
            }
            double Moved = mooo - copiedSubtitles[0].StartTime;
            foreach (Subtitle sub in copiedSubtitles)
            {
                sub.StartTime += Moved;
                sub.EndTime += Moved;
                #region Remove subtitles in our way
                if (ShouldRemove)
                {
                    for (int i = 0; i < selectedTrack.Subtitles.Count; i++)
                    {
                        //copied subtitle is in another
                        if (sub.StartTime >= selectedTrack.Subtitles[i].StartTime & sub.EndTime <= selectedTrack.Subtitles[i].EndTime)
                        {
                            selectedTrack.Subtitles.RemoveAt(i);
                            i = -1;
                            Removed++;
                        }
                        else
                        {
                            //copied subtitle is over another
                            if (sub.StartTime <= selectedTrack.Subtitles[i].StartTime & sub.EndTime >= selectedTrack.Subtitles[i].EndTime)
                            {
                                selectedTrack.Subtitles.RemoveAt(i);
                                i = -1;
                                Removed++;
                            }
                            else
                            {
                                //copied subtitle start time is in another
                                if (sub.StartTime <= selectedTrack.Subtitles[i].EndTime & sub.StartTime >= selectedTrack.Subtitles[i].StartTime)
                                {
                                    selectedTrack.Subtitles.RemoveAt(i);
                                    i = -1;
                                    Removed++;
                                }
                                else
                                {
                                    //copied subtitle end time is in another
                                    if (sub.EndTime >= selectedTrack.Subtitles[i].StartTime & sub.EndTime <= selectedTrack.Subtitles[i].EndTime)
                                    {
                                        selectedTrack.Subtitles.RemoveAt(i);
                                        i = -1;
                                        Removed++;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
                //Set index
                SubIndex = -1;
                for (int i = 0; i < selectedTrack.Subtitles.Count; i++)
                {
                    if (sub.StartTime >= selectedTrack.Subtitles[i].StartTime)
                    { SubIndex = i; }
                    else
                    { break; }
                }
                SubIndex++;
                //Add the subtitle
                Subtitle newSubtitle = sub.Clone();

                selectedTrack.Subtitles.Insert(SubIndex, newSubtitle);
                Pastedd++;
            }
            subtitlesDataEditor1.RefreshSubtitles();
            timeLine1.UpdateSubtitlesReview(); timeLine1.Invalidate();
            Save = true;
            AddHistory(Pastedd.ToString() + " " + Program.ResourceManager.GetString("History_SubtitlesPasted") + ", " + Removed.ToString()
                + " " + Program.ResourceManager.GetString("History_SubtitlesReplaced"));
            StatusLabel.Text = Pastedd.ToString() + " " + Program.ResourceManager.GetString("History_SubtitlesPasted") +
                ", " + Removed.ToString() + " " + Program.ResourceManager.GetString("History_SubtitlesReplaced");
            EnableDisableEditButtons();
            ApplyTrackToControls();
        }
        private void SubtitleProperties(object sender, EventArgs e)
        {
            //edit ...
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 1)
            {
                mediaPlayer1.Pause();
                Frm_SubtitleEdit subtitleEdit = new Frm_SubtitleEdit(subtitlesDataEditor1.SelectedSubtitleItems[0],
                   selectedTrack, Program.Settings.SubtitleTextEditorBackColor, Program.Settings.AllowErrorsInEditControl,
                   Program.ProjectManager.Project.UsePreparedText,
               Program.ProjectManager.Project.PreparedTextRTF,
               Program.ProjectManager.Project.CutPreparedTextAfterAdd,
               Program.ProjectManager.Project.WordWrapPreparedText);
                subtitleEdit.RefreshStyles(Program.ProjectManager.Project.Styles.ToArray());
                subtitleEdit.EditStylesRequest += SubtitleEditor1_EditStylesRequest;

                if (Program.Settings.EditMoreThanOneSubtitleUsingEditorWindow)
                {
                    subtitleEdit.TopMost = true;
                    subtitleEdit.OkButtonPressed += new EventHandler(subtitleEdit_OkButtonPressed);
                    subtitleEdit.Show();
                }
                else
                {
                    if (subtitleEdit.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        Save = true;
                        AddHistory(Program.ResourceManager.GetString("History_SubtitlePropertiesChanged"));
                        ApplyTrackToControls();
                    }
                }
                pIndex = -1;
            }
        }

        void subtitleEdit_OkButtonPressed(object sender, EventArgs e)
        {
            Save = true;
            AddHistory(Program.ResourceManager.GetString("History_SubtitlePropertiesChanged"));
            ApplyTrackToControls();
            if (Program.ProjectManager.Project.UsePreparedText && Program.ProjectManager.Project.CutPreparedTextAfterAdd)
            {
                Program.ProjectManager.Project.PreparedTextRTF = ((Frm_SubtitleEdit)sender).PreparedTextRTFAfterChange;
                preparedTextEditor.RefreshText(Program.ProjectManager.Project.PreparedTextRTF);
            }
        }
        //call this to auto enable disable edit buttons (copy, cut and delete)
        void EnableDisableEditButtons()
        {
            copyToolStripMenuItem.Enabled = cutToolStripMenuItem.Enabled = subtitlesDataEditor1.SelectedSubtitleItems.Length > 0;
            pasteToolStripMenuItem.Enabled = copiedSubtitles.Count > 0;
        }
        #endregion
        #region Timers
        private void Timer_Player_Tick(object sender, EventArgs e)
        {
            //update player
            mediaPlayer1.UpdateTimer();
            if (timeLine1.MediaDuration != MediaPlayerManager.Duration)
            {
                timeLine1.MediaDuration = MediaPlayerManager.Duration;
            }
            if (subtitlesDataEditor1.MediaDuration != mediaPlayer1.Duration)
            {
                subtitlesDataEditor1.MediaDuration = mediaPlayer1.Duration;
            }
            //if (mediaPlayer1.IsPlaying)
            {
                timeLine1.UpdateTime(mediaPlayer1.CurrentPosition);
                timeLine1.IsPlaying = mediaPlayer1.IsPlaying;
            }
            //view subtitle
            if (mediaPlayer1.EnableSubtitleView)
                ViewSubtitle();
            multipleSubtitleTrackViewer.UpdateTimer(mediaPlayer1.CurrentPosition);
            //Show add subtitle info
            if (IsAddingSubtitle)
            {
                addSubtitle.EndTime = mediaPlayer1.CurrentPosition;
                string info = Program.ResourceManager.GetString("Status_AddingNewSubtitle") + ":\n" +
                Program.ResourceManager.GetString("Status_Start") + ": " + TimeFormatConvertor.To_TimeSpan_Milli(addSubtitle.StartTime) + "\n"
              + Program.ResourceManager.GetString("Status_End") + ": " + TimeFormatConvertor.To_TimeSpan_Milli(addSubtitle.EndTime) + "\n"
              + Program.ResourceManager.GetString("Status_Duration") + ": " + addSubtitle.Duration.ToString("F3") + "\n";
                mediaPlayer1.SetInfoTextOfAddButton(info);
            }
        }
        private void timer_autosave_Tick(object sender, EventArgs e)
        {
            if (autoSaveTimerSeconds > 0)
                autoSaveTimerSeconds--;
            else if (autoSaveTimerSeconds == 0)
            {
                autoSaveTimerSeconds--;
                // Show save form
                if (frm_translate != null) // The user is translating
                {
                    if (frm_translate.ISGOOGLETRANSLATE) { autoSaveTimerSeconds++; timer_autosave.Start(); return; }
                    frm_autoSaving = new Frm_AutoSaving();
                    //frm_autoSaving.ShowDialog(this);
                    frm_autoSaving.Show();
                }
                else if (IsAddingSubtitle)
                {
                    // Waite for user to finish adding subtitle ...
                    autoSaveTimerSeconds++; timer_autosave.Start(); return;
                }
                else if (save && File.Exists(Program.ProjectManager.FilePath))
                {
                    frm_autoSaving = new Frm_AutoSaving();
                    //frm_autoSaving.ShowDialog(this);
                    frm_autoSaving.Show();
                }
                else
                {
                    // Just reload ...
                    autoSaveTimerSeconds = Program.Settings.AutoSavePeriodMinutes * 60;
                }
            }
            else
            {
                // Hold timer
                timer_autosave.Stop();
                // If the user is translating, create a backup translation subtitles track
                if (frm_translate != null)
                {
                    // Waite for google translate to finish
                    if (frm_translate.ISGOOGLETRANSLATE)
                    {
                        timer_autosave.Start();
                        return;
                    }
                    if (!Program.ProjectManager.Project.IsSubtitlesTrackExist(translation_temp_track))
                    {
                        SubtitlesTrack track = new SubtitlesTrack(translation_temp_track);
                        foreach (Subtitle sub in frm_translate.TranslationSubtitlesTrack.Subtitles)
                        {
                            track.Subtitles.Add(sub.Clone());
                        }
                        Program.ProjectManager.Project.SubtitleTracks.Add(track);
                        Save = true;
                    }
                    else
                    {
                        // Update the track ...
                        foreach (SubtitlesTrack trk in Program.ProjectManager.Project.SubtitleTracks)
                        {
                            if (trk.Name == translation_temp_track)
                            {
                                trk.Subtitles.Clear();
                                foreach (Subtitle sub in frm_translate.TranslationSubtitlesTrack.Subtitles)
                                {
                                    trk.Subtitles.Add(sub.Clone());
                                }
                                Save = true;
                                break;
                            }
                        }
                    }
                }
                else if (IsAddingSubtitle)
                {
                    // Waite for user to finish adding subtitle ...
                    timer_autosave.Start(); return;
                }
                // Save
                SaveProject();
                // Reload
                autoSaveTimerSeconds = Program.Settings.AutoSavePeriodMinutes * 60;
                // Close save form
                frm_autoSaving.Close();
                // Release timer
                timer_autosave.Start();
            }
        }
        #endregion
        #region Subtitle View and add enablation
        void ViewSubtitle()
        {
            sIndex = FindSubtitleIndex(mediaPlayer1.CurrentPosition);
            if (sIndex >= 0)
            {
                if (sIndex != pIndex)
                {
                    viewedSubtitle = selectedTrack.Subtitles[sIndex];
                    mediaPlayer1.ShowSubtitle(selectedTrack.Subtitles[sIndex].Text);
                    subtitlesDataEditor1.SetViewedSubtitle(sIndex, true);
                }
                pIndex = sIndex;
                //enable buttons
                mediaPlayer1.SubEditButtonEnabled = selectedTrack != null;

                if (selectedTrack != null & mediaPlayer1.IsPlaying)
                {
                    mediaPlayer1.AddButtonEnabled = false;
                    mediaPlayer1.EndSetButtonEnabled = true;
                }
                else
                {
                    mediaPlayer1.AddButtonEnabled = false;
                    mediaPlayer1.EndSetButtonEnabled = false;
                }
            }
            else
            {
                viewedSubtitle = null;
                subtitlesDataEditor1.SetViewedSubtitle(pIndex, false);
                mediaPlayer1.HideSubtitle();
                mediaPlayer1.SubEditButtonEnabled = mediaPlayer1.EndSetButtonEnabled = false;
                //enable buttons
                mediaPlayer1.AddButtonEnabled = (selectedTrack != null & mediaPlayer1.IsPlaying);
            }
        }
        int FindSubtitleIndex(double time)
        {
            if (selectedTrack != null)
            {
                int i = 0;
                foreach (Subtitle sub in selectedTrack.Subtitles)
                {
                    if (sub.StartTime <= time & sub.EndTime >= time)
                        return i;

                    i++;
                }
            }
            return -1;
        }
        #endregion

        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!AskToSave())
            {
                e.Cancel = true;
                return;
            }
            if (WaveReader.IsGenerating)
                WaveReader.AbortGenerateProcess();


            this.WindowState = FormWindowState.Normal;
            ClearHistory();
            if (needToSaveSettings)
                SaveSettings();
            ControlsBase.Settings.Save();
            needToSaveSettings = false;
        }
        private void Frm_Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "Layout (*.ly)|*.ly";
            sav.Title = Program.ResourceManager.GetString("Title_SaveLayout");
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                SaveLayout(sav.FileName, Program.Settings.CurrentLayout);
            }
        }
        private void loadAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Layout (*.ly)|*.ly";
            op.Title = Program.ResourceManager.GetString("Title_LoadLayout");
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ApplyLayout(Program.Settings.CurrentLayout = LoadLayout(op.FileName));
            }
        }
        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ApplyLayout(new LayoutPresent());
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewProject();
        }
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject();
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_OpenProject");
            op.Filter = Filters.ASMP;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                LoadProject(op.FileName);
        }
        private void saveAsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = Program.ResourceManager.GetString("Title_SaveProject");
            sav.Filter = Filters.ASMP;
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (Program.ProjectManager.Save(sav.FileName))
                    Save = false;
            }
        }
        private void richTextBox_description_TextChanged(object sender, EventArgs e)
        {
            Program.ProjectManager.Project.Description = richTextBox_description.Text;
        }
        private void richTextBox_Log_TextChanged(object sender, EventArgs e)
        {
            Program.ProjectManager.Project.Log = richTextBox_Log.Text;
        }
        private void clearLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_AreYouSure"),
                Program.ResourceManager.GetString("MessageCaption_ClearLog"),
                MessageDialogButtons.OkNo, MessageDialogIcon.Question) == MessageDialogResult.Ok)
            {
                richTextBox_Log.Text = "";
                Save = true;
            }
        }
        private void saveLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sav = new SaveFileDialog();
            sav.Title = Program.ResourceManager.GetString("Title_SaveLog");
            sav.Filter = "Text file (*.txt)|*txt";
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                File.WriteAllLines(sav.FileName, richTextBox_Log.Lines);
            }
        }
        /*Subtitle Tracks drag and drop*/
        private void treeView_subtitleTracks_ItemDrag(object sender, ItemDragEventArgs e)
        {
            DoDragDrop(e.Item, DragDropEffects.Move);
        }
        private void treeView_subtitleTracks_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TreeNode_SubtitlesTrack)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void treeView_subtitleTracks_DragOver(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)
            {
                // Retrieve the client coordinates of the mouse position.

                Point targetPoint = treeView_subtitleTracks.PointToClient(new Point(e.X, e.Y));

                // Select the node at the mouse position.

                treeView_subtitleTracks.SelectedNode = treeView_subtitleTracks.GetNodeAt(targetPoint);
            }
        }
        private void treeView_subtitleTracks_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.Move)//move track
            {
                // Retrieve the client coordinates of the drop location.

                Point targetPoint = treeView_subtitleTracks.PointToClient(new Point(e.X, e.Y));

                // Retrieve the node at the drop location.

                TreeNode targetNode = treeView_subtitleTracks.GetNodeAt(targetPoint);

                // Retrieve the node that was dragged.

                TreeNode_SubtitlesTrack draggedNode = (TreeNode_SubtitlesTrack)e.Data.GetData(typeof(TreeNode_SubtitlesTrack));
                if (draggedNode == null)
                    return;
                if (targetNode == null)
                    return;
                // Make new track node instead of drag the same one
                TreeNode_SubtitlesTrack NewTrackNode = new TreeNode_SubtitlesTrack();
                NewTrackNode.SubtitlesTrack = draggedNode.SubtitlesTrack;

                //delete
                Program.ProjectManager.Project.SubtitleTracks.Remove(draggedNode.SubtitlesTrack);
                //insert
                Program.ProjectManager.Project.SubtitleTracks.Insert(targetNode.Index, NewTrackNode.SubtitlesTrack);
                RefreshSubtitleTracks();

                treeView_subtitleTracks.SelectedNode = treeView_subtitleTracks.GetNodeAt(targetPoint);
                Save = true;
            }
            else if (e.Effect == DragDropEffects.Link)//import subtitles format
            {
                string[] dragedFiles = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (dragedFiles != null)
                    if (dragedFiles.Length > 0)
                        ImportFormat(dragedFiles[0]);
            }
        }
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Delete();
        }

        //after select track
        private void treeView_subtitleTracks_MouseClick(object sender, MouseEventArgs e)
        {
            activeMode = ActiveMode.SubtitlesTrack; pIndex = -1;
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if (treeView_subtitleTracks.GetNodeAt(e.Location) != null)
                {
                    treeView_subtitleTracks.SelectedNode = treeView_subtitleTracks.GetNodeAt(e.Location);
                    selectedTrack = errorsChecker1.SubtitlesTrack = subtitleEditor1.SubtitlesTrack = timeLine1.SubtitlesTrack =
                        subtitlesDataEditor1.SubtitlesTrack = ((TreeNode_SubtitlesTrack)treeView_subtitleTracks.SelectedNode).SubtitlesTrack;
                    toolStripButton_selectedTrackRTL.Checked = selectedTrack.RightToLeft;
                    toolStrip_subtitleTracks.Enabled = true;
                    subtitleTracksStatus.Text = selectedTrack.Subtitles.Count + " sub.";
                }
                else
                {
                    treeView_subtitleTracks.SelectedNode = treeView_subtitleTracks.GetNodeAt(e.Location);
                    selectedTrack = errorsChecker1.SubtitlesTrack = subtitleEditor1.SubtitlesTrack = timeLine1.SubtitlesTrack = subtitlesDataEditor1.SubtitlesTrack = null;
                    toolStripButton_selectedTrackRTL.Checked = false;
                    toolStrip_subtitleTracks.Enabled = false;
                    mediaPlayer1.HideViewer();
                    subtitleTracksStatus.Text = Program.ResourceManager.GetString("Status_SelectTrack");
                }
            }
        }
        private void treeView_subtitleTracks_AfterSelect(object sender, TreeViewEventArgs e)
        {
            activeMode = ActiveMode.SubtitlesTrack; pIndex = -1;
            if (treeView_subtitleTracks.SelectedNode != null)
            {
                selectedTrack = errorsChecker1.SubtitlesTrack = subtitleEditor1.SubtitlesTrack = timeLine1.SubtitlesTrack =
                    subtitlesDataEditor1.SubtitlesTrack = ((TreeNode_SubtitlesTrack)treeView_subtitleTracks.SelectedNode).SubtitlesTrack;
                toolStripButton_selectedTrackRTL.Checked = selectedTrack.RightToLeft;
                toolStrip_subtitleTracks.Enabled = true;
                subtitleTracksStatus.Text = selectedTrack.Subtitles.Count + " sub.";
            }
            else
            {
                selectedTrack = errorsChecker1.SubtitlesTrack = subtitleEditor1.SubtitlesTrack = timeLine1.SubtitlesTrack = subtitlesDataEditor1.SubtitlesTrack = null;
                toolStripButton_selectedTrackRTL.Checked = false;
                toolStrip_subtitleTracks.Enabled = false;
                subtitleTracksStatus.Text = Program.ResourceManager.GetString("Status_SelectTrack");
                mediaPlayer1.HideViewer();
            }
        }
        private void renameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (treeView_subtitleTracks.SelectedNode == null)
                return;
            treeView_subtitleTracks.SelectedNode.BeginEdit();
        }
        private void duplicateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (treeView_subtitleTracks.SelectedNode == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_SelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_Error"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                return;
            }
            SubtitlesTrack desiredSubtitlesTrack = ((TreeNode_SubtitlesTrack)treeView_subtitleTracks.SelectedNode).SubtitlesTrack;
            SubtitlesTrack newTrack = new SubtitlesTrack();
            //Name
            int i = 0;
            while (Program.ProjectManager.Project.IsSubtitlesTrackExist(desiredSubtitlesTrack.Name + "_Duplication" + (i + 1).ToString()))
            {
                i++;
            }
            newTrack.Name = desiredSubtitlesTrack.Name + "_Duplication" + (i + 1).ToString();
            //subtitles
            foreach (Subtitle sub in desiredSubtitlesTrack.Subtitles)
            {
                Subtitle newSubtitle = new Subtitle();
                newSubtitle.EndTime = sub.EndTime;
                newSubtitle.StartTime = sub.StartTime;
                newSubtitle.Text = SubtitleTextWrapper.Clone(sub.Text);
                newTrack.Subtitles.Add(newSubtitle);
            }
            //add track
            Program.ProjectManager.Project.SubtitleTracks.Add(newTrack);
            RefreshSubtitleTracks();
            Save = true;
            AddHistory(Program.ResourceManager.GetString("History_SubtitlesTrackDuplicate") + " '" + desiredSubtitlesTrack.Name + "'");
        }
        private void changeMediaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_ChangeMedia");
            op.Filter = Filters.Media;
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (op.FileName != Program.ProjectManager.Project.MediaPath)// change only when media is not the same
                {
                    ChangeMedia(op.FileName);
                    if (MediaPlayerManager.Duration <= 0)
                    {
                        MessageDialog.ShowErrorMessage(Program.ResourceManager.GetString("Message_MediaNotLoaded"), Program.ResourceManager.GetString("MessageCaption_ChangeMedia"));
                    }

                    Save = true;
                    AddHistory(Program.ResourceManager.GetString("History_MediaChanged"));
                }
            }
        }
        private void selectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subtitlesDataEditor1.SelectAll();
        }
        private void selectNoneToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subtitlesDataEditor1.SelectNone();
        }
        private void selectSameTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subtitlesDataEditor1.SelectWithSameText();
        }
        private void subtitlesDataEditor1_SubtitlesSelected(object sender, EventArgs e)
        {
            subtitleEditor1.SelectedItems = subtitlesDataEditor1.SelectedSubtitleItems;

            List<Subtitle> selectedSubtitles = new List<Subtitle>();
            foreach (ListViewItem_Subtitle item in subtitleEditor1.SelectedItems)
                selectedSubtitles.Add(item.Subtitle);
            timeLine1.SelectedSubtitles = selectedSubtitles;

            EnableDisableEditButtons();
        }
        private void subtitleEditor1_SaveRequest(object sender, EventArgs e)
        {
            AddHistory(Program.ResourceManager.GetString("History_SubtitlePropertiesChanged"));
            Save = true;
        }
        private void listBox_history_SelectedIndexChanged(object sender, EventArgs e)
        {
            EnableUndoRedo();
        }
        private void undoToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            toolStripButton7.Enabled = undoToolStripMenuItem.Enabled;
        }
        private void redoToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            toolStripButton8.Enabled = redoToolStripMenuItem.Enabled;
        }
        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox_history.SelectedIndex - 1 >= 0)
            {
                listBox_history.SelectedIndex--;
                OpenHistory();
            }
        }
        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listBox_history.SelectedIndex + 1 < listBox_history.Items.Count)
            {
                listBox_history.SelectedIndex++;
                OpenHistory();
            }
        }
        private void listBox_history_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                OpenHistory();
            }
        }

        private void mediaPlayer1_SaveRequest(object sender, EventArgs e)
        {
            try
            {
                subtitlesDataEditor1.SubtitleItems[sIndex].RefreshText();
                subtitlesDataEditor1.SubtitleItems[sIndex].ChangeTimingView();
            }
            catch { }
            if (Program.Settings.ShortcutAddSubtitle != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutAddSubtitle);
            if (Program.Settings.ShortcutFWD != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutFWD);
            if (Program.Settings.ShortcutREW != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutREW);
            AddHistory(Program.ResourceManager.GetString("History_SubtitlePropertiesChanged"));
            Save = true; isEditingSubtitle = false;
        }
        private void subtitlesDataEditor1_TimeChangeRequest(object sender, Controls.TimeChangeArgs e)
        {
            if (Program.Settings.MediaSeekOnSelect & !mediaPlayer1.IsPlaying)
                mediaPlayer1.CurrentPosition = e.NewTime;
        }
        private void subtitlesDataEditor1_SubtitleEditRequest(object sender, EventArgs e)
        {
            //this rised only if one subtitle selected via mouse double click
            if (mediaPlayer1.IsPlaying)
            {
                if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 1)
                {
                    mediaPlayer1.CurrentPosition = subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.StartTime;
                    timeLine1.ScrollToCurrentTime();
                }
            }
            else
            {
                SubtitleProperties(this, e);
            }
        }
        private void mediaPlayer1_AddButtonHoldPress(object sender, EventArgs e)
        {
            if (mediaPlayer1.IsPlaying)
            {
                addSubtitle = new Subtitle(mediaPlayer1.CurrentPosition, mediaPlayer1.CurrentPosition + 0.01,
                    SubtitleText.FromString(Program.ResourceManager.GetString("Title_NewSubtitle"),
                    Program.Settings.NewSubtitleFont, Program.Settings.NewSubtitleColor));
                IsAddingSubtitle = true;
            }
        }
        private void mediaPlayer1_AddButtonHoldRelease(object sender, EventArgs e)
        {
            if (IsAddingSubtitle)
            {
                IsAddingSubtitle = false;
                if (Program.Settings.PauseAfterAddUsingQuickMode)
                    mediaPlayer1.Pause();
                mediaPlayer1.SetInfoTextOfAddButton("");
                // Create the subtitle text
                SubtitleText subtext = Program.Settings.AddNewSubtitleTextWhenAddNewSubtitle ?
                   SubtitleText.FromString(Program.ResourceManager.GetString("Title_NewSubtitle"),
                   Program.Settings.NewSubtitleFont, Program.Settings.NewSubtitleColor)
                   :
                   SubtitleText.FromString(" ", Program.Settings.NewSubtitleFont, Program.Settings.NewSubtitleColor);

                Subtitle sub = new Subtitle(addSubtitle.StartTime, addSubtitle.EndTime, subtext);
                ListViewItem_Subtitle item = new ListViewItem_Subtitle();
                sub.Text.RighttoLeft = selectedTrack.RightToLeft;
                item.Subtitle = sub;
                if (Program.Settings.EditSubtitleAfterAddUsingQuickMode)
                {
                    Frm_SubtitleEdit subtitleEdit = new Frm_SubtitleEdit(item, selectedTrack,
                        Program.Settings.SubtitleTextEditorBackColor, Program.Settings.AllowErrorsInEditControl,
                        Program.ProjectManager.Project.UsePreparedText,
                        Program.ProjectManager.Project.PreparedTextRTF,
                        Program.ProjectManager.Project.CutPreparedTextAfterAdd,
                Program.ProjectManager.Project.WordWrapPreparedText);
                    subtitleEdit.RefreshStyles(Program.ProjectManager.Project.Styles.ToArray());
                    subtitleEdit.EditStylesRequest += SubtitleEditor1_EditStylesRequest;
                    subtitleEdit.SelectAll();
                    if (subtitleEdit.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        if (Program.ProjectManager.Project.UsePreparedText && Program.ProjectManager.Project.CutPreparedTextAfterAdd)
                        {
                            Program.ProjectManager.Project.PreparedTextRTF = subtitleEdit.PreparedTextRTFAfterChange;
                            preparedTextEditor.RefreshText(Program.ProjectManager.Project.PreparedTextRTF);
                        }
                        selectedTrack.Subtitles.Add(item.Subtitle);
                        selectedTrack.Subtitles.Sort(new SubtitleComparer());
                        subtitlesDataEditor1.RefreshSubtitles();
                        timeLine1.UpdateSubtitlesReview();
                        Save = true;
                        AddHistory(Program.ResourceManager.GetString("History_SubtitleAddedAt") + " " +
                            TimeFormatConvertor.To_TimeSpan_Milli(sub.StartTime));
                        //seek
                        if (Program.Settings.SeekMediaToNewSubtitlesStartTimeAfterAdd)
                            mediaPlayer1.CurrentPosition = sub.StartTime;
                    }
                }
                else
                {
                    //add without edit
                    selectedTrack.Subtitles.Add(item.Subtitle);
                    selectedTrack.Subtitles.Sort(new SubtitleComparer());
                    subtitlesDataEditor1.RefreshSubtitles();
                    timeLine1.UpdateSubtitlesReview();
                    Save = true;
                    AddHistory(Program.ResourceManager.GetString("History_SubtitleAddedAt") + " " +
                        TimeFormatConvertor.To_TimeSpan_Milli(sub.StartTime));
                    //seek
                    if (Program.Settings.SeekMediaToNewSubtitlesStartTimeAfterAdd)
                        mediaPlayer1.CurrentPosition = sub.StartTime;
                }
                addSubtitle = null;
                pIndex = -1;
            }
        }
        private void renameProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EnterNameForm dial = new EnterNameForm(Program.ResourceManager.GetString("Title_EnterProjectName"),
                Program.ProjectManager.Project.Name, true, false);
            if (dial.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Program.ProjectManager.Project.Name = dial.EnteredName;
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_ProjectNameChange"));
            }
        }
        private void mediaPlayer1_EndSetButtonPressed(object sender, EventArgs e)
        {
            if (viewedSubtitle != null)
            {
                viewedSubtitle.EndTime = mediaPlayer1.CurrentPosition;
                subtitlesDataEditor1.SubtitleItems[sIndex].RefreshText();
                subtitlesDataEditor1.SubtitleItems[sIndex].ChangeTimingView();

                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SetCurrentTimeAsEndTimeForSubtitleAt") + " " +
                    TimeFormatConvertor.To_TimeSpan_Milli(viewedSubtitle.StartTime));
            }
        }
        private void cutToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            cutToolStripMenuItem1.Enabled = toolStripButton4.Enabled = cutToolStripMenuItem.Enabled;
        }
        private void copyToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            copyToolStripMenuItem1.Enabled = toolStripButton5.Enabled = copyToolStripMenuItem.Enabled;
        }
        private void pasteToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            pasteToolStripMenuItem1.Enabled = toolStripButton6.Enabled = pasteToolStripMenuItem.Enabled;
        }
        private void subtitlesDataEditor1_Enter(object sender, EventArgs e)
        {
            activeMode = ActiveMode.Subtitles;
        }
        private void recentToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int index = 0;
            for (int i = 0; i < recentToolStripMenuItem.DropDownItems.Count; i++)
            {
                if (e.ClickedItem == recentToolStripMenuItem.DropDownItems[i])
                { index = i; break; }
            }
            if (File.Exists(Program.Settings.RecentProject[index]))
            {
                LoadProject(Program.Settings.RecentProject[index]);
            }
            else
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_ThisProjectIsMissing"),
                    Program.ResourceManager.GetString("MessageCaption_CantFindProject"));
                RefreshRecents();
            }
        }
        private void timeLine1_TimeChangeRequest(object sender, Controls.TimeChangeArgs e)
        {
            mediaPlayer1.CurrentPosition = e.NewTime;
            //refresh view
            pIndex = -1;
            activeMode = ActiveMode.None;
        }
        private void timeLine1_SubtitlesSelected(object sender, EventArgs e)
        {
            activeMode = ActiveMode.Subtitles;
            subtitlesDataEditor1.SelectNone();
            foreach (Subtitle sub in timeLine1.SelectedSubtitles)
                subtitlesDataEditor1.SelectItem(sub);

            subtitleEditor1.SelectedItems = subtitlesDataEditor1.SelectedSubtitleItems;

            EnableDisableEditButtons();
        }
        private void timeLine1_SubtitleDoubleClick(object sender, EventArgs e)
        {
            //this rised only if one subtitle selected via mouse double click
            if (mediaPlayer1.IsPlaying)
            {
                if (timeLine1.SelectedSubtitles.Count == 1)
                    mediaPlayer1.CurrentPosition = timeLine1.SelectedSubtitles[0].StartTime;
            }
            else
            {
                SubtitleProperties(this, e);
            }
        }
        private void timeLine1_SubtitlePropertiesChanged(object sender, EventArgs e)
        {
            //refresh controls
            selectedTrack.CheckSubtitlesOrder();
            subtitlesDataEditor1.RefreshSubtitles();
            //set save and history
            AddHistory(Program.ResourceManager.GetString("History_SubtitlePropertiesChanged"));
            Save = true;

            activeMode = ActiveMode.None;
        }
        private void playPauseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer1.TogglePlayPause();
        }
        private void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer1.Stop();
        }
        private void upToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                mediaPlayer1.Volume += 2;
            }
            catch { }
        }
        private void downToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                mediaPlayer1.Volume -= 2;
            }
            catch { }
        }
        private void muteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer1.ToggleMute();
        }
        private void mediaPlayer1_MuteToggle(object sender, EventArgs e)
        {
            muteToolStripMenuItem.Checked = mediaPlayer1.Mute;
        }
        private void jumpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_JumpTime frm = new Frm_JumpTime();
            frm.StartPosition = FormStartPosition.Manual;
            frm.Time = mediaPlayer1.CurrentPosition;
            frm.Location = Cursor.Position;
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                mediaPlayer1.CurrentPosition = frm.Time;
        }
        private void nextFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer1.NextFrame();
        }
        private void previousFrameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer1.PreviousFrame();
        }
        private void translateToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                 Program.ResourceManager.GetString("MessageCaption_Translate"));
                return;
            }

            frm_translate = new Frm_Translate(selectedTrack);
            if (frm_translate.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (frm_translate.IsCreateNew)
                {
                    //just add the translated track
                    string newName = selectedTrack.Name + "_Translation";
                    int i = 0;
                    while (Program.ProjectManager.Project.IsSubtitlesTrackExist(newName))
                    {
                        newName = selectedTrack.Name + "_Translation" + i.ToString();
                        i++;
                    }
                    frm_translate.TranslationSubtitlesTrack.Name = newName;
                    Program.ProjectManager.Project.SubtitleTracks.Add(frm_translate.TranslationSubtitlesTrack);

                }
                else//replace original, add the translated subtitles to the same track
                {
                    selectedTrack.Subtitles.Clear();
                    selectedTrack.Subtitles = frm_translate.TranslationSubtitlesTrack.Subtitles;
                }
                frm_translate = null;
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SubtitlesTrackTranslated"));
            }
            // Delete the temp track if found ...
            if (Program.ProjectManager.Project.IsSubtitlesTrackExist(translation_temp_track))
            {
                for (int i = 0; i < Program.ProjectManager.Project.SubtitleTracks.Count; i++)
                {
                    if (Program.ProjectManager.Project.SubtitleTracks[i].Name == translation_temp_track)
                    {
                        Program.ProjectManager.Project.SubtitleTracks.RemoveAt(i);
                        break;
                    }
                }
            }
            frm_translate = null;
            RefreshSubtitleTracks();
            subtitlesDataEditor1.RefreshSubtitles();
            timeLine1.UpdateSubtitlesReview();
            timeLine1.Refresh();
        }
        private void synchronizeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_Synchronise"));
                return;
            }
            Frm_Synchronize frm = new Frm_Synchronize();
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (frm.TargetFramerate == frm.BaseFramerate)
                    return;
                if (frm.TargetFramerate == 0)
                {
                    MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheTargetFramerateMustBeNotZero"),
                          Program.ResourceManager.GetString("MessageCaption_Synchronise"));
                    return;
                }
                if (frm.BaseFramerate == 0)
                {
                    MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheBaseFramerateMustBeNotZero"),
                        Program.ResourceManager.GetString("MessageCaption_Synchronise"));
                    return;
                }
                //synch
                ProgressBar1.Visible = true;
                ProgressBar1.Maximum = 100;
                SetStatus(Program.ResourceManager.GetString("Status_Synchronising"), StatusType.None);
                int i = 0;
                foreach (Subtitle sub in selectedTrack.Subtitles)
                {
                    sub.StartTime = (sub.StartTime * frm.BaseFramerate) / frm.TargetFramerate;
                    sub.EndTime = (sub.EndTime * frm.BaseFramerate) / frm.TargetFramerate;
                    ProgressBar1.Value = (i * 100) / selectedTrack.Subtitles.Count;
                    i++;
                }
                ProgressBar1.Visible = false;
                SetStatus(Program.ResourceManager.GetString("Status_Done"), StatusType.Good);
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SubtitlesTrackSynchronised"));
                subtitlesDataEditor1.RefreshSubtitles();
                timeLine1.UpdateSubtitlesReview();
            }
        }
        private void exportGTTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_ExportGT"));
                return;
            }
            SaveFileDialog sav = new SaveFileDialog();
            sav.Filter = "GT-Text (*txt)|*.txt";
            if (sav.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                List<string> LINES = new List<string>();
                foreach (Subtitle sub in selectedTrack.Subtitles)
                {
                    for (int i = 0; i < sub.Text.TextLines.Count; i++)
                        LINES.Add(sub.Text.TextLines[i].ToString());
                    LINES.Add("==========");
                }
                File.WriteAllLines(sav.FileName, LINES.ToArray(), Encoding.Unicode);
            }
        }
        private void importGTTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_ImportGT"));
                return;
            }
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "GT-Text (*txt)|*.txt";
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                string[] LINES = File.ReadAllLines(op.FileName);
                string text = "";
                int index = 0;
                try
                {
                    foreach (string line in LINES)
                    {
                        if (line != "==========")
                            text += line + "\n";
                        else
                        {
                            text = text.Substring(0, text.Length - 1);
                            selectedTrack.Subtitles[index].Text = SubtitleText.FromString(text, Program.Settings.NewSubtitleFont, Program.Settings.NewSubtitleColor);
                            index++;
                            text = "";
                        }
                    }
                }
                catch
                {
                    MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_FaildToImportGTText"),
                   Program.ResourceManager.GetString("MessageCaption_FaildToImportGTText"), MessageDialogButtons.Ok, MessageDialogIcon.Error);
                }
                subtitlesDataEditor1.RefreshSubtitles();
                timeLine1.UpdateSubtitlesReview();
                Save = true;
                AddHistory("GT-Text import.");
            }
        }
        private void shiftTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                   Program.ResourceManager.GetString("MessageCaption_ShiftTime"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length <= 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectSubtitleToShift"),
                     Program.ResourceManager.GetString("MessageCaption_ShiftTime"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length != 1 && subtitlesDataEditor1.SelectedSubtitleItems.Length != selectedTrack.Subtitles.Count)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectSubtitleToShift"),
                     Program.ResourceManager.GetString("MessageCaption_ShiftTime"));
                return;
            }
            double Pre = 0;
            double nxt = 0;
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 1)//Shift selected
            {
                if (subtitlesDataEditor1.SelectedSubtitleItems[0].Index == 0)
                {
                    ListViewItem_Subtitle suuu = subtitlesDataEditor1.SelectedSubtitleItems[0];
                    Pre = 0;
                    if (subtitlesDataEditor1.SubtitleItems.Length > 1)
                    {
                        ListViewItem_Subtitle suuu1 = subtitlesDataEditor1.SubtitleItems[suuu.Index + 1];
                        nxt = suuu1.Subtitle.StartTime - 0.001;
                    }
                    else
                    {
                        if (mediaPlayer1.Duration > 0)
                        {
                            nxt = mediaPlayer1.Duration - 0.001;
                        }
                        else
                        {
                            MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheMediaDurationMustBeLargerThanZero"),
                            Program.ResourceManager.GetString("MessageCaption_ShiftTime"), MessageDialogButtons.Ok, MessageDialogIcon.Error); return;
                        }
                    }
                    Frm_Shift shh = new Frm_Shift(suuu.Subtitle.StartTime, suuu.Subtitle.EndTime, suuu.Subtitle.Duration, Pre, nxt, Program.Settings.ShiftTime);
                    shh.ShowDialog(this);
                    if (shh.OK)
                    {
                        double moved = shh.Shifted;
                        shh.Close();
                        suuu.Subtitle.StartTime += moved;
                        suuu.Subtitle.EndTime += moved;
                        selectedTrack.Subtitles[suuu.Index] = suuu.Subtitle;
                        Save = true;
                        subtitlesDataEditor1.RefreshSubtitles(); timeLine1.UpdateSubtitlesReview();
                        AddHistory(Program.ResourceManager.GetString("History_SubtitleShift"));
                    }
                }
                else
                {
                    ListViewItem_Subtitle suuu = subtitlesDataEditor1.SelectedSubtitleItems[0];
                    ListViewItem_Subtitle suuuPrev = subtitlesDataEditor1.SubtitleItems[suuu.Index - 1];
                    Pre = suuuPrev.Subtitle.EndTime + 0.001;
                    if (suuu.Index != subtitlesDataEditor1.SubtitleItems.Length - 1)
                    {
                        ListViewItem_Subtitle suuuNxt = subtitlesDataEditor1.SubtitleItems[suuu.Index + 1];
                        nxt = suuuNxt.Subtitle.StartTime - 0.001;
                    }
                    else
                    {
                        if (mediaPlayer1.Duration > 0)
                        {
                            nxt = mediaPlayer1.Duration - 0.001;
                        }
                        else
                        {
                            MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheMediaDurationMustBeLargerThanZero"),
                              Program.ResourceManager.GetString("MessageCaption_ShiftTime"), MessageDialogButtons.Ok, MessageDialogIcon.Error); return;
                        }
                    }
                    Frm_Shift shh = new Frm_Shift(suuu.Subtitle.StartTime, suuu.Subtitle.EndTime, suuu.Subtitle.Duration, Pre, nxt, Program.Settings.ShiftTime);
                    shh.ShowDialog(this);
                    if (shh.OK)
                    {
                        double moved = shh.Shifted;
                        shh.Close();
                        suuu.Subtitle.StartTime += moved;
                        suuu.Subtitle.EndTime += moved;
                        selectedTrack.Subtitles[suuu.Index] = suuu.Subtitle;
                        Save = true;
                        subtitlesDataEditor1.RefreshSubtitles(); timeLine1.UpdateSubtitlesReview();
                        AddHistory(Program.ResourceManager.GetString("History_SubtitleShift"));
                    }
                }
            }
            else//Shift all
            {
                if (mediaPlayer1.Duration == 0)
                {
                    MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheMediaDurationMustBeLargerThanZero"),
                                Program.ResourceManager.GetString("MessageCaption_ShiftTime"), MessageDialogButtons.Ok, MessageDialogIcon.Error); return;
                }

                if (IsAddingSubtitle)
                    return;
                double Pre1 = 0;
                double nxt1 = mediaPlayer1.Duration - 0.001;
                ListViewItem_Subtitle startsub = subtitlesDataEditor1.SubtitleItems[0];
                ListViewItem_Subtitle endsub = subtitlesDataEditor1.SubtitleItems[subtitlesDataEditor1.SubtitleItems.Length - 1];
                Frm_Shift shh = new Frm_Shift(startsub.Subtitle.StartTime,
                    endsub.Subtitle.EndTime, (double)(endsub.Subtitle.EndTime - startsub.Subtitle.StartTime), Pre1, nxt1, Program.Settings.ShiftTime);
                shh.ShowDialog(this);
                if (shh.OK)
                {
                    double moved = shh.Shifted;
                    for (int i = 0; i < subtitlesDataEditor1.SubtitleItems.Length; i++)
                    {
                        ListViewItem_Subtitle sub = subtitlesDataEditor1.SubtitleItems[i];
                        sub.Subtitle.StartTime += moved;
                        sub.Subtitle.EndTime += moved;
                    }
                    Save = true;
                    subtitlesDataEditor1.RefreshSubtitles(); timeLine1.UpdateSubtitlesReview();
                    AddHistory(Program.ResourceManager.GetString("History_SubtitlesTrackShift"));
                }
            }
        }
        private void fillSpaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_FillSpace"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length != 1)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectOneSubtitleToFillSpaceFor"),
                    Program.ResourceManager.GetString("MessageCaption_FillSpace"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems[0].Index == 0)
            {
                ListViewItem_Subtitle suuu = subtitlesDataEditor1.SelectedSubtitleItems[0];
                suuu.Subtitle.StartTime = 0;
                if (subtitlesDataEditor1.SubtitleItems.Length > 1)
                {
                    ListViewItem_Subtitle suuu1 = subtitlesDataEditor1.SubtitleItems[suuu.Index + 1];
                    suuu.Subtitle.EndTime = suuu1.Subtitle.StartTime - 0.001;
                }
                else
                {
                    if (mediaPlayer1.Duration > 0)
                    {
                        suuu.Subtitle.EndTime = mediaPlayer1.Duration - 0.001;
                    }
                    else
                    {
                        MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheMediaDurationMustBeLargerThanZero"),
                        Program.ResourceManager.GetString("MessageCaption_FillSpace"), MessageDialogButtons.Ok, MessageDialogIcon.Error); return;
                    }
                }
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SubtitleFillSpace"));
                subtitlesDataEditor1.RefreshSubtitles(); timeLine1.UpdateSubtitlesReview();
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems[0].Index == subtitlesDataEditor1.SubtitleItems.Length - 1)
            {
                ListViewItem_Subtitle suuu = subtitlesDataEditor1.SelectedSubtitleItems[0];
                if (subtitlesDataEditor1.SubtitleItems.Length > 1)
                {
                    ListViewItem_Subtitle suuuPre = subtitlesDataEditor1.SubtitleItems[suuu.Index - 1];
                    suuu.Subtitle.StartTime = suuuPre.Subtitle.EndTime + 0.001;
                }
                else
                {
                    suuu.Subtitle.StartTime = 0;
                }
                if (mediaPlayer1.Duration > 0)
                {
                    suuu.Subtitle.EndTime = mediaPlayer1.Duration - 0.001;
                }
                else
                {
                    MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheMediaDurationMustBeLargerThanZero"),
                    Program.ResourceManager.GetString("MessageCaption_FillSpace"), MessageDialogButtons.Ok, MessageDialogIcon.Error); return;
                }

                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SubtitleFillSpace"));
                subtitlesDataEditor1.RefreshSubtitles();
                return;
            }
            else
            {
                ListViewItem_Subtitle suuu = subtitlesDataEditor1.SelectedSubtitleItems[0];
                ListViewItem_Subtitle suuuPre = subtitlesDataEditor1.SubtitleItems[suuu.Index - 1];
                ListViewItem_Subtitle suuunxt = subtitlesDataEditor1.SubtitleItems[suuu.Index + 1];
                suuu.Subtitle.StartTime = suuuPre.Subtitle.EndTime + 0.001;
                suuu.Subtitle.EndTime = suuunxt.Subtitle.StartTime - 0.001;
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SubtitleFillSpace"));
                subtitlesDataEditor1.RefreshSubtitles(); timeLine1.UpdateSubtitlesReview();
            }
        }
        private void stretchToNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                    Program.ResourceManager.GetString("MessageCaption_StretchToNext"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length != 1)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectOneSubtitleToStretch"),
                    Program.ResourceManager.GetString("MessageCaption_StretchToNext"));
                return;
            }
            int item_index = subtitlesDataEditor1.SelectedSubtitleItems[0].Index;
            int item_next_index = item_index + 1;
            if (item_next_index < subtitlesDataEditor1.SubtitleItems.Length)
            {
                // the next item is existed.
                double time = subtitlesDataEditor1.SubtitleItems[item_next_index].Subtitle.StartTime - 0.001;
                if (time > subtitlesDataEditor1.SubtitleItems[item_index].Subtitle.StartTime)
                    subtitlesDataEditor1.SubtitleItems[item_index].Subtitle.EndTime = time;

            }
            else
            {
                // There is no next subtitle. Stretch to media end ?
                double time = mediaPlayer1.Duration - 0.001;
                if (time > subtitlesDataEditor1.SubtitleItems[item_index].Subtitle.StartTime)
                    subtitlesDataEditor1.SubtitleItems[item_index].Subtitle.EndTime = time;
            }
            Save = true;
            AddHistory(Program.ResourceManager.GetString("History_SubtitleStretchToNext"));
            subtitlesDataEditor1.RefreshSubtitles();
        }
        private void stretchToPreviousToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_StretchToPrevious"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length != 1)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectOneSubtitleToStretch"),
                   Program.ResourceManager.GetString("MessageCaption_StretchToPrevious"));
                return;
            }


            int index = subtitlesDataEditor1.SelectedSubtitleItems[0].Index - 1;
            if (index < 0)
            {
                // No subtitle located before this one, Stretch to 0
                if (subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.EndTime > 0)
                    subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.StartTime = 0;
            }
            else
            {
                // Stretch to previous subtitle
                double time = subtitlesDataEditor1.SubtitleItems[index].Subtitle.EndTime + 0.001;

                if (time < subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.EndTime)
                    subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.StartTime = time;
            }

            Save = true;
            AddHistory(Program.ResourceManager.GetString("History_SubtitleStretchToPrevious"));
            subtitlesDataEditor1.RefreshSubtitles();
            timeLine1.UpdateSubtitlesReview();
        }
        private void splitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                   Program.ResourceManager.GetString("MessageCaption_Split"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length != 1)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectOneSubtitleToSplit"),
                    Program.ResourceManager.GetString("MessageCaption_Split"));
                return;
            }
            Frm_Split frm = new Frm_Split(subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Subtitle newSubtitle = new Subtitle(frm.SplitTime, subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.EndTime,
               SubtitleTextWrapper.Clone(subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.Text));
                //update original subtitle
                subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.EndTime = frm.SplitTime - 0.001;
                //Insert new one
                int index = subtitlesDataEditor1.SelectedSubtitleItems[0].Index + 1;
                selectedTrack.Subtitles.Add(newSubtitle);
                selectedTrack.Subtitles.Sort(new SubtitleComparer());
                subtitlesDataEditor1.RefreshSubtitles();
                timeLine1.UpdateSubtitlesReview();
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SubtitleSplited"));
            }
        }
        private void timeLine1_JumpIntoTimeRequest(object sender, EventArgs e)
        {
            jumpToolStripMenuItem_Click(sender, e);
        }

        private void addMarkToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_AddTimeMark frm = new Frm_AddTimeMark(mediaPlayer1.CurrentPosition);
            frm.StartPosition = FormStartPosition.Manual;
            frm.Location = Cursor.Position;
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                Program.ProjectManager.Project.TimeMarks.Add(new TimeMark(frm.MarkName, frm.MarkTime));
                Program.ProjectManager.Project.TimeMarks.Sort(new TimeMarkComparer());
                RefreshMarks();
                SelectMarkAtTime(mediaPlayer1.CurrentPosition);
                timeLine1.RefreshMarks(Program.ProjectManager.Project.TimeMarks.ToArray(), ComboBox_marks.SelectedIndex);
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_MarkAddedAt") + " " + frm.MarkTime);
            }
        }
        private void deleteSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ComboBox_marks.SelectedIndex >= 0)
            {
                if (MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_AreYouSure"),
                   Program.ResourceManager.GetString("MessageCaption_RemoveMark"), MessageDialogButtons.OkNo, MessageDialogIcon.Question, false, "&Yes", "&No") == MessageDialogResult.Ok)
                {
                    Program.ProjectManager.Project.TimeMarks.RemoveAt(ComboBox_marks.SelectedIndex);
                    Program.ProjectManager.Project.TimeMarks.Sort(new TimeMarkComparer());
                    RefreshMarks();
                    SelectMarkAtTime(mediaPlayer1.CurrentPosition);
                    timeLine1.RefreshMarks(Program.ProjectManager.Project.TimeMarks.ToArray(), ComboBox_marks.SelectedIndex);
                    Save = true;
                    AddHistory(Program.ResourceManager.GetString("History_MarkRemoved"));
                }
            }
        }
        private void jumpIntoSelectedTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (ComboBox_marks.SelectedIndex >= 0)
            {
                mediaPlayer1.CurrentPosition = Program.ProjectManager.Project.TimeMarks[ComboBox_marks.SelectedIndex].Time;
            }
        }
        private void ComboBox_marks_DropDownClosed(object sender, EventArgs e)
        {
            timeLine1.SelectMark(ComboBox_marks.SelectedIndex);
            jumpIntoSelectedTimeToolStripMenuItem_Click(sender, e);
        }
        private void timeLine1_JumpIntoMarkRequest(object sender, EventArgs e)
        {
            jumpIntoSelectedTimeToolStripMenuItem_Click(sender, e);
        }
        private void timeLine1_AddMarkRequest(object sender, EventArgs e)
        {
            addMarkToolStripMenuItem_Click(sender, e);
        }
        private void timeLine1_RemoveMarkRequest(object sender, EventArgs e)
        {
            deleteSelectedToolStripMenuItem_Click(sender, e);
        }
        private void timeLine1_MarkEdit(object sender, MarkEditArgs e)
        {
            Save = true;
            AddHistory(Program.ResourceManager.GetString("History_MarkEdit"));
            RefreshMarks();
            SelectMarkAtTime(Program.ProjectManager.Project.TimeMarks[e.MarkIndex].Time);
        }
        private void timeLine1_SelectMarkRequest(object sender, MarkEditArgs e)
        {
            SelectMarkAtTime(Program.ProjectManager.Project.TimeMarks[e.MarkIndex].Time);
        }
        /*Find And Replace*/
        private void findToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFindRecent(ComboBox_search.Text);
            //if we already have a find window, ignore this call
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.GetType() == typeof(Frm_FindAndReplace))
                {
                    ((Frm_FindAndReplace)frm).FindWord = ComboBox_search.Text;
                    ((Frm_FindAndReplace)frm).IsFindAndReplace = false;
                    frm.Select();
                    return;
                }
            }
            Frm_FindAndReplace findAndReplaceForm = new Frm_FindAndReplace(false, ComboBox_search.Text);
            findAndReplaceForm.FindNextRequest += new EventHandler<FindAndReplaceArgs>(findAndReplaceForm_FindNextRequest);
            findAndReplaceForm.Show();
        }
        private void findAndReplaceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddFindRecent(ComboBox_search.Text);
            //if we already have a find window, ignore this call
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.GetType() == typeof(Frm_FindAndReplace))
                {
                    ((Frm_FindAndReplace)frm).FindWord = ComboBox_search.Text;
                    ((Frm_FindAndReplace)frm).IsFindAndReplace = true;
                    frm.Select();
                    return;
                }
            }
            Frm_FindAndReplace findAndReplaceForm = new Frm_FindAndReplace(true, ComboBox_search.Text);
            findAndReplaceForm.FindNextRequest += new EventHandler<FindAndReplaceArgs>(findAndReplaceForm_FindNextRequest);
            findAndReplaceForm.Show();
        }
        private void findAndReplaceForm_FindNextRequest(object sender, FindAndReplaceArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                   Program.ResourceManager.GetString("MessageCaption_FindSubtitle"));
                return;
            }
            bool stopSearch = false;
            bool found = false;
            if (e.All)
                subtitlesDataEditor1.SelectNone();
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length > 0)
            {
                SearchIndex = subtitlesDataEditor1.SelectedSubtitleItems[0].Index + 1;
                if (SearchIndex == selectedTrack.Subtitles.Count)
                    SearchIndex = 0;
            }
            for (int i = SearchIndex; i < selectedTrack.Subtitles.Count; i++)
            {
                if (stopSearch)
                    break;
                if (selectedTrack.Subtitles[i].Text.ToString().Length >= e.FindWhat.Length)
                {
                    if (!e.MatchWholeWord)
                    {
                        for (int SearchWordIndex = 0; SearchWordIndex <
                                (selectedTrack.Subtitles[i].Text.ToString().Length - e.FindWhat.Length) + 1; SearchWordIndex++)
                        {
                            string Ser = selectedTrack.Subtitles[i].Text.ToString().Substring(SearchWordIndex, e.FindWhat.Length);
                            if (!e.MatchCase)
                            {
                                if (Ser.ToLower() == e.FindWhat.ToLower())
                                {
                                    //this is it
                                    if (!e.All)
                                        subtitlesDataEditor1.SelectNone();
                                    subtitlesDataEditor1.SelectItem(i);
                                    subtitlesDataEditor1.Select();
                                    found = true;
                                    if (!e.All)
                                        stopSearch = true;
                                    //replace ?
                                    if (e.ReplaceWith != "")
                                    {
                                        //search for the word in the subtile text lines
                                        foreach (SubtitleLine line in subtitlesDataEditor1.SubtitleItems[i].Subtitle.Text.TextLines)
                                        {
                                            if (line.ToString().Length >= e.FindWhat.Length)
                                            {
                                                for (int c = 0; c <= line.Chars.Count - e.FindWhat.Length; c++)
                                                {
                                                    Ser = line.ToString().Substring(c, e.FindWhat.Length);
                                                    if (Ser.ToLower() == e.FindWhat.ToLower())
                                                    {
                                                        //we found the index of the first char of the find word
                                                        line.Chars.RemoveRange(c, e.FindWhat.Length);
                                                        //Add the replace word
                                                        for (int o = 0; o < e.ReplaceWith.Length; o++)
                                                        {
                                                            line.Chars.Insert(c, new SubtitleChar(e.ReplaceWith.ToCharArray()[o],
                                                                Program.Settings.NewSubtitleFont, Program.Settings.NewSubtitleColor));
                                                            c++;
                                                        }
                                                        //break !!
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    subtitlesDataEditor1.SubtitleItems[i].RefreshText();
                                    subtitlesDataEditor1.SubtitleItems[i].ChangeTimingView();
                                    break;
                                }
                            }
                            else
                            {
                                if (Ser == e.FindWhat)
                                {
                                    //this is it
                                    if (!e.All)
                                        subtitlesDataEditor1.SelectNone();
                                    subtitlesDataEditor1.SelectItem(i);
                                    subtitlesDataEditor1.Select();
                                    found = true;
                                    if (!e.All)
                                        stopSearch = true;
                                    if (e.ReplaceWith != "")
                                    {
                                        //search for the word in the subtile text lines
                                        foreach (SubtitleLine line in subtitlesDataEditor1.SubtitleItems[i].Subtitle.Text.TextLines)
                                        {
                                            if (line.ToString().Length >= e.FindWhat.Length)
                                            {
                                                for (int c = 0; c <= line.Chars.Count - e.FindWhat.Length; c++)
                                                {
                                                    Ser = line.ToString().Substring(c, e.FindWhat.Length);
                                                    if (Ser == e.FindWhat)
                                                    {
                                                        //we found the index of the first char of the find word
                                                        line.Chars.RemoveRange(c, e.FindWhat.Length);
                                                        //Add the replace word
                                                        for (int o = 0; o < e.ReplaceWith.Length; o++)
                                                        {
                                                            line.Chars.Insert(c, new SubtitleChar(e.ReplaceWith.ToCharArray()[o],
                                                                Program.Settings.NewSubtitleFont, Program.Settings.NewSubtitleColor));
                                                            c++;
                                                        }
                                                        //break !!
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    subtitlesDataEditor1.SubtitleItems[i].RefreshText();
                                    subtitlesDataEditor1.SubtitleItems[i].ChangeTimingView();
                                    break;
                                }
                            }
                        }
                    }
                    else//match word search
                    {
                        string[] wordToFind = e.FindWhat.Split(new char[] { ' ' });
                        string[] subtitleTexts = selectedTrack.Subtitles[i].Text.ToString().Split(new char[] { ' ' });
                        int matched = 0;
                        for (int j = 0; j < subtitleTexts.Length; j++)
                        {
                            for (int h = 0; h < wordToFind.Length; h++)
                            {
                                if (!e.MatchCase)
                                {
                                    if (subtitleTexts[j].ToLower() == wordToFind[h].ToLower())
                                    {
                                        matched++;
                                        wordToFind[h] = "";
                                        break;
                                    }
                                }
                                else
                                {
                                    if (subtitleTexts[j] == wordToFind[h])
                                    {
                                        wordToFind[h] = "";
                                        matched++;
                                        break;
                                    }
                                }
                            }
                        }
                        if (matched >= wordToFind.Length)
                        {
                            //this is it
                            if (!e.All)
                                subtitlesDataEditor1.SelectNone();
                            subtitlesDataEditor1.SelectItem(i);
                            subtitlesDataEditor1.Select();
                            found = true;
                            if (!e.All)
                                stopSearch = true;
                            if (e.ReplaceWith != "")
                            {
                                //search for the word in the subtile text lines
                                foreach (SubtitleLine line in subtitlesDataEditor1.SubtitleItems[i].Subtitle.Text.TextLines)
                                {
                                    if (line.ToString().Length >= e.FindWhat.Length)
                                    {
                                        for (int c = 0; c <= line.Chars.Count - e.FindWhat.Length; c++)
                                        {
                                            string Ser = line.ToString().Substring(c, e.FindWhat.Length);
                                            if (e.MatchCase)
                                            {
                                                if (Ser == e.FindWhat)
                                                {
                                                    //we found the index of the first char of the find word
                                                    line.Chars.RemoveRange(c, e.FindWhat.Length);
                                                    //Add the replace word
                                                    for (int o = 0; o < e.ReplaceWith.Length; o++)
                                                    {
                                                        line.Chars.Insert(c, new SubtitleChar(e.ReplaceWith.ToCharArray()[o],
                                                            Program.Settings.NewSubtitleFont, Program.Settings.NewSubtitleColor));
                                                        c++;
                                                    }
                                                    //break !!
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                if (Ser.ToLower() == e.FindWhat.ToLower())
                                                {
                                                    //we found the index of the first char of the find word
                                                    line.Chars.RemoveRange(c, e.FindWhat.Length);
                                                    //Add the replace word
                                                    for (int o = 0; o < e.ReplaceWith.Length; o++)
                                                    {
                                                        line.Chars.Insert(c, new SubtitleChar(e.ReplaceWith.ToCharArray()[o],
                                                            Program.Settings.NewSubtitleFont, Program.Settings.NewSubtitleColor));
                                                        c++;
                                                    }
                                                    //break !!
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            subtitlesDataEditor1.SubtitleItems[i].RefreshText();
                            subtitlesDataEditor1.SubtitleItems[i].ChangeTimingView();
                            break;
                        }
                    }
                }
            }
            //Reset index if nothing found
            if (!found)
            {
                SearchIndex = 0;
                subtitlesDataEditor1.SelectNone();
            }
            //Status
            if (e.All)
            {
                if (e.ReplaceWith != "")
                {
                    SetStatus(Program.ResourceManager.GetString("Status_ReplaceAllDone") + ", " +
                        subtitlesDataEditor1.SelectedSubtitleItems.Length + " " +
                        Program.ResourceManager.GetString("Status_subtitlesFound"), StatusType.None);
                    Save = true;
                    AddHistory(Program.ResourceManager.GetString("Status_ReplaceAllDone"));
                }
                else
                {
                    SetStatus(Program.ResourceManager.GetString("Status_FindAllDone") + ", " + subtitlesDataEditor1.SelectedSubtitleItems.Length + " " +
                        Program.ResourceManager.GetString("Status_subtitlesFound"), StatusType.None);
                }
            }
            else
            {
                if (e.ReplaceWith != "")
                {
                    SetStatus(Program.ResourceManager.GetString("Status_ReplaceDone"), StatusType.None);
                    Save = true;
                    AddHistory(Program.ResourceManager.GetString("Status_ReplaceDone"));
                }
                else
                {
                    SetStatus(Program.ResourceManager.GetString("Status_FindDone"), StatusType.None);
                }
            }
        }
        private void findNextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //we should have a find window to request a next find
            foreach (Form frm in Application.OpenForms)
            {
                if (frm.GetType() == typeof(Frm_FindAndReplace))
                {
                    ((Frm_FindAndReplace)frm).FindNext();
                    return;
                }
            }
        }
        private void clonetimingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                   Program.ResourceManager.GetString("MessageCaption_Clone"));
                return;
            }
            if (Program.ProjectManager.Project.SubtitleTracks.Count < 2)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheProjectMustHave2TracksOrMore"),
                    Program.ResourceManager.GetString("MessageCaption_Clone"));
                return;
            }
            Frm_ChooseSubtitlesTrack frm = new Frm_ChooseSubtitlesTrack(selectedTrack, Program.ProjectManager.Project);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (frm.SelectedSubtitlesTrack.Subtitles.Count != selectedTrack.Subtitles.Count)
                {
                    MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_SelectedTracksHaveNotTheSameCountOfSubtitles"),
                          Program.ResourceManager.GetString("MessageCaption_Clone"));
                    return;
                }
                //clone
                ProgressBar1.Maximum = 100;
                ProgressBar1.Visible = true;
                SetStatus(Program.ResourceManager.GetString("Status_CloningTimings"), StatusType.None);
                for (int i = 0; i < selectedTrack.Subtitles.Count; i++)
                {
                    frm.SelectedSubtitlesTrack.Subtitles[i].StartTime = selectedTrack.Subtitles[i].StartTime;
                    frm.SelectedSubtitlesTrack.Subtitles[i].EndTime = selectedTrack.Subtitles[i].EndTime;
                    ProgressBar1.Value = (i * 100) / selectedTrack.Subtitles.Count;
                }
                ProgressBar1.Visible = false;
                SetStatus(Program.ResourceManager.GetString("Status_Done"), StatusType.Good);
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SubtitlesTrackTimingsClone"));
            }
        }
        private void cloneTextsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
               Program.ResourceManager.GetString("MessageCaption_Clone"));
                return;
            }
            if (Program.ProjectManager.Project.SubtitleTracks.Count < 2)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheProjectMustHave2TracksOrMore"),
                    Program.ResourceManager.GetString("MessageCaption_Clone"));
                return;
            }
            Frm_ChooseSubtitlesTrack frm = new Frm_ChooseSubtitlesTrack(selectedTrack, Program.ProjectManager.Project);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (frm.SelectedSubtitlesTrack.Subtitles.Count != selectedTrack.Subtitles.Count)
                {
                    MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_SelectedTracksHaveNotTheSameCountOfSubtitles"),
                              Program.ResourceManager.GetString("MessageCaption_Clone"));
                    return;
                }
                //clone
                ProgressBar1.Maximum = 100;
                ProgressBar1.Visible = true;
                SetStatus(Program.ResourceManager.GetString("Status_CloningTexts"), StatusType.None);
                for (int i = 0; i < selectedTrack.Subtitles.Count; i++)
                {
                    frm.SelectedSubtitlesTrack.Subtitles[i].Text = SubtitleTextWrapper.Clone(selectedTrack.Subtitles[i].Text);
                    ProgressBar1.Value = (i * 100) / selectedTrack.Subtitles.Count;
                }
                ProgressBar1.Visible = false;
                SetStatus(Program.ResourceManager.GetString("Status_Done"), StatusType.Good);
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SubtitlesTrackTextsClone"));
            }
        }
        private void errorsChecker1_CheckProgress(object sender, ProgressArgs e)
        {
            ProgressBar1.Value = e.PrecentageCompleted;
            SetStatus(e.Status, StatusType.None);
        }
        private void errorsChecker1_CheckFinished(object sender, EventArgs e)
        {
            ProgressBar1.Visible = false;
        }
        private void errorsChecker1_CheckStarted(object sender, EventArgs e)
        {
            ProgressBar1.Visible = true;
        }
        private void errorsChecker1_SelectSubtitlesRequest(object sender, SubtitlesSelectArgs e)
        {
            subtitlesDataEditor1.SelectNone();
            foreach (int index in e.Indices)
                subtitlesDataEditor1.SelectItem(index);
        }
        private void checkForErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Program.Settings.CurrentLayout.Tab_Errors.Visible)
            {
                Program.Settings.CurrentLayout.Tab_Errors.Visible = errorsToolStripMenuItem1.Checked = true;
                SetTabParent(Program.Settings.CurrentLayout.Tab_Errors, tabPage_errors);
                CheckForCollapse();
            }
            ((TabControl)tabPage_errors.Parent).SelectedTab = tabPage_errors;
            errorsChecker1.CheckForErrors();
        }
        private void selectAllToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            errorsChecker1.SelectAllItems();
        }
        private void jumpIntoSelectedErrorSubtitlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            errorsChecker1.SelectErrorSubtitles();
        }
        private void iD3TagsSynchronizedLyricsToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            toolStripButton_id3.Enabled = iD3TagsSynchronizedLyricsToolStripMenuItem.Enabled;
        }
        private void iD3TagsSynchronizedLyricsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            mediaPlayer1.ClearMedia();

            Frm_ID3v2 frm = new Frm_ID3v2(Program.ProjectManager.Project);

            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                RefreshSubtitleTracks();
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_ID3Managed"));
            }
            //reload media
            ChangeMedia(Program.ProjectManager.Project.MediaPath, false);
        }

        private void mediaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpandTabControls();
            Program.Settings.CurrentLayout.Tab_Media.Visible = mediaToolStripMenuItem.Checked;
            SetTabParent(Program.Settings.CurrentLayout.Tab_Media, tabPage_media);
            CheckForCollapse();
        }
        private void subtitlesDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpandTabControls();
            Program.Settings.CurrentLayout.Tab_SubtitlesData.Visible = subtitlesDataToolStripMenuItem.Checked;
            SetTabParent(Program.Settings.CurrentLayout.Tab_SubtitlesData, tabPage_subitltesData);
            CheckForCollapse();
        }
        private void subtitleTracksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpandTabControls();
            Program.Settings.CurrentLayout.Tab_SubtitleTracks.Visible = subtitleTracksToolStripMenuItem.Checked;
            SetTabParent(Program.Settings.CurrentLayout.Tab_SubtitleTracks, tabPage_subtitleTracks);
            CheckForCollapse();
        }
        private void projectDescriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpandTabControls();
            Program.Settings.CurrentLayout.Tab_ProjectDescription.Visible = projectDescriptionToolStripMenuItem.Checked;
            SetTabParent(Program.Settings.CurrentLayout.Tab_ProjectDescription, tabPage_ProjectDescription);
            CheckForCollapse();
        }
        private void historyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpandTabControls();
            Program.Settings.CurrentLayout.Tab_History.Visible = historyToolStripMenuItem.Checked;
            SetTabParent(Program.Settings.CurrentLayout.Tab_History, tabPage_history);
            CheckForCollapse();
        }
        private void logToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpandTabControls();
            Program.Settings.CurrentLayout.Tab_Log.Visible = logToolStripMenuItem.Checked;
            SetTabParent(Program.Settings.CurrentLayout.Tab_Log, tabPage_log);
            CheckForCollapse();
        }
        private void errorsToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ExpandTabControls();
            Program.Settings.CurrentLayout.Tab_Errors.Visible = errorsToolStripMenuItem1.Checked;
            SetTabParent(Program.Settings.CurrentLayout.Tab_Errors, tabPage_errors);
            CheckForCollapse();
        }
        private void timelineToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpandTabControls();
            Program.Settings.CurrentLayout.Tab_TimeLine.Visible = timelineToolStripMenuItem.Checked;
            SetTabParent(Program.Settings.CurrentLayout.Tab_TimeLine, tabPage_TimeLine);
            CheckForCollapse();
        }
        private void propertiesToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            ExpandTabControls();
            Program.Settings.CurrentLayout.Tab_Properties.Visible = propertiesToolStripMenuItem2.Checked;
            SetTabParent(Program.Settings.CurrentLayout.Tab_Properties, tabPage_Properties);
            CheckForCollapse();
        }
        /*Tabs drag and drop*/
        private void tabControl_top_left_MouseUp(object sender, MouseEventArgs e)
        {
            canDragDrop = false;
        }
        private void tabControl_top_left_MouseDown(object sender, MouseEventArgs e)
        {
            downPoint = e.Location;
            canDragDrop = true;
        }
        private void tabControl_top_left_MouseMove(object sender, MouseEventArgs e)
        {
            if (canDragDrop & ((e.X - downPoint.X) > 10 | (e.Y - downPoint.Y) > 10))
            {
                ExpandTabControls();
                CheckDragAndDrop();
                DoDragDrop(tabControl_top_left.SelectedTab, DragDropEffects.Move);
                CheckForCollapse();
            }
        }
        private void tabControl_top_left_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TabPage)))
            {
                ((TabPage)e.Data.GetData(typeof(TabPage))).Parent = tabControl_top_left;
                SaveTabParent(((TabPage)e.Data.GetData(typeof(TabPage))), TabParent.TopLeft);
                canDragDrop = false;
            }
        }
        private void tabControl_top_middle_MouseMove(object sender, MouseEventArgs e)
        {
            if (canDragDrop & ((e.X - downPoint.X) > 10 | (e.Y - downPoint.Y) > 10))
            {
                ExpandTabControls();
                CheckDragAndDrop();
                DoDragDrop(tabControl_top_middle.SelectedTab, DragDropEffects.Move);
                CheckForCollapse();
            }
        }
        private void tabControl_top_middle_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TabPage)))
            {
                ((TabPage)e.Data.GetData(typeof(TabPage))).Parent = tabControl_top_middle;
                SaveTabParent(((TabPage)e.Data.GetData(typeof(TabPage))), TabParent.Top);
                canDragDrop = false;
            }
        }
        private void tabControl_top_right_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TabPage)))
            {
                ((TabPage)e.Data.GetData(typeof(TabPage))).Parent = tabControl_top_right;
                SaveTabParent(((TabPage)e.Data.GetData(typeof(TabPage))), TabParent.TopRight);
                canDragDrop = false;
            }
        }
        private void tabControl_top_right_MouseMove(object sender, MouseEventArgs e)
        {
            if (canDragDrop & ((e.X - downPoint.X) > 10 | (e.Y - downPoint.Y) > 10))
            {
                ExpandTabControls();
                CheckDragAndDrop();
                DoDragDrop(tabControl_top_right.SelectedTab, DragDropEffects.Move);
                CheckForCollapse();
            }
        }
        private void tabControl_down_left_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TabPage)))
            {
                ((TabPage)e.Data.GetData(typeof(TabPage))).Parent = tabControl_down_left;
                SaveTabParent(((TabPage)e.Data.GetData(typeof(TabPage))), TabParent.DownLeft);
                canDragDrop = false;
            }
        }
        private void tabControl_down_left_MouseMove(object sender, MouseEventArgs e)
        {
            if (canDragDrop & ((e.X - downPoint.X) > 10 | (e.Y - downPoint.Y) > 10))
            {
                ExpandTabControls();
                CheckDragAndDrop();
                DoDragDrop(tabControl_down_left.SelectedTab, DragDropEffects.Move);
                CheckForCollapse();
            }
        }
        private void tabControl_down_middle_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TabPage)))
            {
                ((TabPage)e.Data.GetData(typeof(TabPage))).Parent = tabControl_down_middle;
                SaveTabParent(((TabPage)e.Data.GetData(typeof(TabPage))), TabParent.Down);
                canDragDrop = false;
            }
        }
        private void tabControl_down_middle_MouseMove(object sender, MouseEventArgs e)
        {
            if (canDragDrop & ((e.X - downPoint.X) > 10 | (e.Y - downPoint.Y) > 10))
            {
                ExpandTabControls();
                CheckDragAndDrop();
                DoDragDrop(tabControl_down_middle.SelectedTab, DragDropEffects.Move);
                CheckForCollapse();
            }
        }
        private void tabControl_down_Right_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TabPage)))
            {
                ((TabPage)e.Data.GetData(typeof(TabPage))).Parent = tabControl_down_Right;
                SaveTabParent(((TabPage)e.Data.GetData(typeof(TabPage))), TabParent.DownRight);
                canDragDrop = false;
            }
        }
        private void tabControl_down_Right_MouseMove(object sender, MouseEventArgs e)
        {
            if (canDragDrop & ((e.X - downPoint.X) > 10 | (e.Y - downPoint.Y) > 10))
            {
                ExpandTabControls();
                CheckDragAndDrop();
                DoDragDrop(tabControl_down_Right.SelectedTab, DragDropEffects.Move);
                CheckForCollapse();
            }
        }
        private void tabControl_top_left_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        private void mainToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Program.Settings.CurrentLayout.Bar_Main.Visible = toolStrip_main.Visible = mainToolStripMenuItem.Checked;
        }
        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Program.Settings.CurrentLayout.Bar_Edit.Visible = toolStrip_edit.Visible = editToolStripMenuItem1.Checked;
        }
        private void marksToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Program.Settings.CurrentLayout.Bar_Marks.Visible = toolStrip_marks.Visible = marksToolStripMenuItem1.Checked;
        }

        private void toolStrip_main_ParentChanged(object sender, EventArgs e)
        {
            Program.Settings.CurrentLayout.Bar_Main.Parent = GetBarParent(toolStrip_main);
        }
        private void toolStrip_marks_ParentChanged(object sender, EventArgs e)
        {
            Program.Settings.CurrentLayout.Bar_Marks.Parent = GetBarParent(toolStrip_marks);
        }
        private void toolStrip_edit_ParentChanged(object sender, EventArgs e)
        {
            Program.Settings.CurrentLayout.Bar_Edit.Parent = GetBarParent(toolStrip_edit);
        }
        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Program.ProjectManager.Project.SubtitleTracks.Count == 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_NoSubtitlesTrackToExport"),
                  Program.ResourceManager.GetString("MessageCaption_CantExport"),
                    MessageDialogButtons.Ok, MessageDialogIcon.Error);
                return;
            }
            mediaPlayer1.Pause();
            Frm_Export export = new Frm_Export(Program.ProjectManager.Project, Program.Settings.FavoriteFormat);
            export.ShowDialog(this);
            Program.Settings.FavoriteFormat = export.FavoriteFormat;
        }
        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Title = Program.ResourceManager.GetString("Title_ImportSubtitlesFormat");
            op.Filter = SubtitleFormats.GetFilter();
            if (op.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                ImportFormat(op.FileName);
            }
        }
        private void format_Progress(object sender, ProgressArgs e)
        {
            ProgressBar1.Value = e.PrecentageCompleted;
            SetStatus(e.Status, StatusType.None);
        }
        private void format_LoadFinished(object sender, EventArgs e)
        {
            ProgressBar1.Visible = false;
        }
        private void format_LoadStarted(object sender, EventArgs e)
        {
            ProgressBar1.Visible = true;
        }

        private void treeView_subtitleTracks_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            isRenaming = false;
            if (treeView_subtitleTracks.SelectedNode == null)
                return;
            if (e.Label == "" || e.Label == null)
            {
                e.CancelEdit = true;
                return;
            }
            if (Program.ProjectManager.Project.IsSubtitlesTrackExist(e.Label))
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_NameAlreadyTaken"),
                   Program.ResourceManager.GetString("MessageCaption_CantRename"));
                e.CancelEdit = true;
                return;
            }
            ((TreeNode_SubtitlesTrack)treeView_subtitleTracks.SelectedNode).SubtitlesTrack.Name = e.Label;
            multipleSubtitleTrackViewer.UpdateTrackControls();
            Save = true;
            AddHistory(Program.ResourceManager.GetString("History_SubtitlesTrackRenamed"));
        }
        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isChangingSettings = true;
            mediaPlayer1.Pause();
            Frm_Settings frm = new Frm_Settings();
            frm.ShowDialog(this);
            ApplySettings();
            isChangingSettings = false;
        }
        private void aboutAHDSubtitlesMakerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Frm_AboutASM about = new Frm_AboutASM(Program.Version, Program.Settings.Language);
            about.ShowDialog(this);
        }
        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/alaahadid/AHD-Subtitles-Maker");
        }
        private void tipOfTheDayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowTipOfTheDay();
        }
        private void quickStartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowQuickStart();
        }
        private void autoFixErrorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            errorsChecker1.AutoFixErrors();
        }
        private void errorsChecker1_AutoFixFinished(object sender, EventArgs e)
        {
            ProgressBar1.Visible = false;
            Save = true;

            //refresh controls
            subtitlesDataEditor1.RefreshSubtitles();
            timeLine1.UpdateSubtitlesReview(); timeLine1.Refresh();

            AddHistory(Program.ResourceManager.GetString("History_ErrorsAutoFix"));
        }
        private void errorsChecker1_AutoFixProgress(object sender, ProgressArgs e)
        {
            ProgressBar1.Value = e.PrecentageCompleted;
            SetStatus(e.Status, StatusType.None);
        }
        private void errorsChecker1_AutoFixStarted(object sender, EventArgs e)
        {
            ProgressBar1.Visible = true;
        }
        private void setRightToLeftToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
                return;
            foreach (Subtitle sub in selectedTrack.Subtitles)
            {
                sub.Text.RighttoLeft = true;
            }
            Save = true;
            AddHistory(Program.ResourceManager.GetString("History_RTLValueChanged"));
            SetStatus(Program.ResourceManager.GetString("Status_RightToLeftSet"), StatusType.None);
        }
        private void setLeftToRightToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
                return;
            foreach (Subtitle sub in selectedTrack.Subtitles)
            {
                sub.Text.RighttoLeft = false;
            }
            Save = true;
            AddHistory(Program.ResourceManager.GetString("History_RTLValueChanged"));
            SetStatus(Program.ResourceManager.GetString("Status_LeftToRightSet"), StatusType.None);
        }
        private void toolStripButton_selectedTrackRTL_Click(object sender, EventArgs e)
        {
            if (selectedTrack != null)
            {
                selectedTrack.RightToLeft = !selectedTrack.RightToLeft;
                toolStripButton_selectedTrackRTL.Checked = selectedTrack.RightToLeft;
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_DefaultRTLChanged"));
            }
        }
        private void toolStripButton29_Click(object sender, EventArgs e)
        {
            activeMode = ActiveMode.SubtitlesTrack;
            Delete();
        }
        private void defaultRTLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            toolStripButton_selectedTrackRTL_Click(sender, e);
        }
        private void toolStripButton_selectedTrackRTL_CheckedChanged(object sender, EventArgs e)
        {
            defaultRTLToolStripMenuItem.Checked = toolStripButton_selectedTrackRTL.Checked;
        }
        private void mediaPlayer1_TimeSlide(object sender, EventArgs e)
        {
            pIndex = -1;
            if (Program.Settings.ScrollTimelineToMediaOnMediaSlide)
                timeLine1.ScrollToCurrentTime();
        }
        private void viewHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string helpPath = Program.StartUpPath + "\\" + Program.CultureInfo.Name + "\\Help.chm";
            if (File.Exists(helpPath))
                Help.ShowHelp(this, helpPath, HelpNavigator.TableOfContents);
            else
                Help.ShowHelp(this, Program.StartUpPath + "\\en-US\\Help.chm", HelpNavigator.TableOfContents);
        }
        private void languageToolStripMenuItem_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            int i = 0;
            int index = 0;
            foreach (ToolStripMenuItem item in languageToolStripMenuItem.DropDownItems)
            {
                if (item.Text == e.ClickedItem.Text)
                {
                    item.Checked = true;
                    index = i;
                }
                else
                    item.Checked = false;
                i++;
            }
            Program.Language = Program.SupportedLanguages[index, 0];
            Program.Settings.Language = Program.SupportedLanguages[index, 0];

            MessageBox.Show(Program.ResourceManager.GetString("Message_YouMustRestartTheProgramToApplyLanguage"),
                Program.ResourceManager.GetString("MessageCaption_ApplyLanguage"));
        }
        private void setFontOfSelectedSubtitlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_StretchToPrevious"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectAtLeastOneSubtitle"),
                   Program.ResourceManager.GetString("MessageCaption_NosubtitleSelected"));
                return;
            }
            FontDialog fontDial = new FontDialog();
            if (fontDial.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                foreach (ListViewItem_Subtitle subItem in subtitlesDataEditor1.SelectedSubtitleItems)
                {
                    subItem.Subtitle.Text.SetFont(fontDial.Font);
                }
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_FontSetForSelected"));
            }
        }
        private void setColorForSelectedSubtitlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_StretchToPrevious"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectAtLeastOneSubtitle"),
                   Program.ResourceManager.GetString("MessageCaption_NosubtitleSelected"));
                return;
            }

            ColorDialog colorDial = new ColorDialog();
            if (colorDial.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                foreach (ListViewItem_Subtitle subItem in subtitlesDataEditor1.SelectedSubtitleItems)
                {
                    subItem.Subtitle.Text.SetColor(colorDial.Color);
                }
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_ColorSetForSelected"));
            }
        }
        private void setrightToLeftForSelectedSubtitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_StretchToPrevious"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectAtLeastOneSubtitle"),
                   Program.ResourceManager.GetString("MessageCaption_NosubtitleSelected"));
                return;
            }
            foreach (ListViewItem_Subtitle subItem in subtitlesDataEditor1.SelectedSubtitleItems)
            {
                subItem.Subtitle.Text.RighttoLeft = true;
            }
            Save = true;
            AddHistory(Program.ResourceManager.GetString("History_SetRightToLeftForSelectedSubtitle"));
        }
        private void setleftToRightForSelectedSubtitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_StretchToPrevious"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectAtLeastOneSubtitle"),
                   Program.ResourceManager.GetString("MessageCaption_NosubtitleSelected"));
                return;
            }
            foreach (ListViewItem_Subtitle subItem in subtitlesDataEditor1.SelectedSubtitleItems)
            {
                subItem.Subtitle.Text.RighttoLeft = false;
            }
            Save = true;
            AddHistory(Program.ResourceManager.GetString("History_SetLeftToRightForSelectedSubtitle"));
        }
        private void setpositionForSelectedSubtitlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_StretchToPrevious"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectAtLeastOneSubtitle"),
                   Program.ResourceManager.GetString("MessageCaption_NosubtitleSelected"));
                return;
            }

            SubtitleText text = new SubtitleText();
            AHD.SM.Controls.Frm_SubtitlePosition frm = new AHD.SM.Controls.Frm_SubtitlePosition(text);
            frm.Location = new Point(Cursor.Position.X, Cursor.Position.Y);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                foreach (ListViewItem_Subtitle subItem in subtitlesDataEditor1.SelectedSubtitleItems)
                {
                    subItem.Subtitle.Text.Position = text.Position;
                    subItem.Subtitle.Text.IsCustomPosition = text.IsCustomPosition;
                    subItem.Subtitle.Text.CustomPosition = text.CustomPosition;
                }
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_PostitionSetforSelectedSubtitles"));
            }
        }
        private void setAlignmentForSelectedSubtitleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_StretchToPrevious"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectAtLeastOneSubtitle"),
                   Program.ResourceManager.GetString("MessageCaption_NosubtitleSelected"));
                return;
            }

            Frm_SetAlignment frm = new Frm_SetAlignment();
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                foreach (ListViewItem_Subtitle subItem in subtitlesDataEditor1.SelectedSubtitleItems)
                {
                    foreach (SubtitleLine line in subItem.Subtitle.Text.TextLines)
                        line.Alignement = frm.LineAlignement;
                }
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SetAlignmentForSelectedSubtitles"));
            }
        }
        private void mediaPlayer1_ChangeMediaRequest(object sender, EventArgs e)
        {
            MessageDialogResult result = MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_NoMediaDoYouWantToChangeMedia"),
                Program.ResourceManager.GetString("MessageCaption_NoMedia"),
                MessageDialogButtons.OkNo, MessageDialogIcon.Question, false, Program.ResourceManager.GetString("Button_Yes"),
                Program.ResourceManager.GetString("Button_No"));

            if (result == MessageDialogResult.Ok)
            {
                changeMediaToolStripMenuItem_Click(this, e);
            }
        }
        private void spellCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_SpellCheck"));
                return;
            }
            int index = 0;
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 1)
                index = subtitlesDataEditor1.SelectedSubtitleItems[0].Index;

            Frm_SpellCheck frm = new Frm_SpellCheck(selectedTrack, index, (activeMode == ActiveMode.Subtitles));
            try
            {
                if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    ApplyTrackToControls();
                    // subtitlesDataEditor1.RefreshSubtitles();
                    Save = true;
                    AddHistory(Program.ResourceManager.GetString("History_SpellCheck"));
                }
            }
            catch { }
        }
        private void mergeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
               Program.ResourceManager.GetString("MessageCaption_Merge"));
                return;
            }
            if (Program.ProjectManager.Project.SubtitleTracks.Count < 2)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheProjectMustHave2TracksOrMore"),
                    Program.ResourceManager.GetString("MessageCaption_Merge"));
                return;
            }
            Frm_ChooseSubtitlesTrack frm = new Frm_ChooseSubtitlesTrack(selectedTrack, Program.ProjectManager.Project);
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (frm.SelectedSubtitlesTrack.Subtitles.Count != selectedTrack.Subtitles.Count)
                {
                    MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_SelectedTracksHaveNotTheSameCountOfSubtitles"),
                              Program.ResourceManager.GetString("MessageCaption_Merge"));
                    return;
                }
                Frm_MergeTwoTracks options = new Frm_MergeTwoTracks();
                if (options.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                {
                    bool isAbove = options.SubtitlesAreAbove;
                    //clone
                    ProgressBar1.Maximum = 100;
                    ProgressBar1.Visible = true;
                    SetStatus(Program.ResourceManager.GetString("Status_Merging"), StatusType.None);
                    for (int i = 0; i < selectedTrack.Subtitles.Count; i++)
                    {
                        SubtitleText target = SubtitleTextWrapper.Clone(frm.SelectedSubtitlesTrack.Subtitles[i].Text);
                        if (isAbove)
                        {
                            int index = 0;
                            foreach (SubtitleLine line in target.TextLines)
                            {
                                selectedTrack.Subtitles[i].Text.TextLines.Insert(index, line);
                                index++;
                            }
                        }
                        else
                        {
                            foreach (SubtitleLine line in target.TextLines)
                            {
                                selectedTrack.Subtitles[i].Text.TextLines.Add(line);
                            }
                        }
                        ProgressBar1.Value = (i * 100) / selectedTrack.Subtitles.Count;
                    }
                    if (options.DeleteTheTarget)
                    {
                        Program.ProjectManager.Project.SubtitleTracks.Remove(frm.SelectedSubtitlesTrack);
                        RefreshSubtitleTracks();
                        if (treeView_subtitleTracks.Nodes.Count > 0)
                            treeView_subtitleTracks.SelectedNode = treeView_subtitleTracks.Nodes[0];

                    }
                    ProgressBar1.Visible = false;
                    SetStatus(Program.ResourceManager.GetString("Status_Done"), StatusType.Good);
                    Save = true;
                    AddHistory(Program.ResourceManager.GetString("History_SubtitleTracksMerge"));
                }
            }
        }
        private void mergeToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_Merge"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length < 2)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectMoreThanOneSubtitle")
                    , Program.ResourceManager.GetString("MessageCaption_Merge"));
                return;
            }
            mediaPlayer1.Pause();
            // make sure the subtitles are in one que
            int index = subtitlesDataEditor1.SelectedSubtitleItems[0].Index;
            int lastIndex = subtitlesDataEditor1.SelectedSubtitleItems[subtitlesDataEditor1.SelectedSubtitleItems.Length - 1].Index;
            for (int i = index; i <= lastIndex; i++)
            {
                if (!subtitlesDataEditor1.SubtitleItems[i].Selected)
                {
                    MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_TheSubtitlesMustBeInOneQueue")
                        , Program.ResourceManager.GetString("MessageCaption_Merge"));
                    return;
                }
            }
            // let's do the merge
            SubtitleText newText = new SubtitleText();
            foreach (ListViewItem_Subtitle item in subtitlesDataEditor1.SelectedSubtitleItems)
            {
                foreach (SubtitleLine line in item.Subtitle.Text.TextLines)
                    newText.TextLines.Add(line);
            }
            Subtitle newSubtitle = new Subtitle(subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.StartTime,
                subtitlesDataEditor1.SelectedSubtitleItems[subtitlesDataEditor1.SelectedSubtitleItems.Length - 1].Subtitle.EndTime,
                newText);
            newSubtitle.Text.CustomPosition = subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.Text.CustomPosition;
            newSubtitle.Text.IsCustomPosition = subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.Text.IsCustomPosition;
            newSubtitle.Text.Position = subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.Text.Position;
            newSubtitle.Text.RighttoLeft = subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.Text.RighttoLeft;
            // delete all selected subtitles from the track
            foreach (ListViewItem_Subtitle item in subtitlesDataEditor1.SelectedSubtitleItems)
            {
                selectedTrack.Subtitles.Remove(item.Subtitle);
            }
            // add this subtitle to the track
            selectedTrack.Subtitles.Add(newSubtitle);
            selectedTrack.Subtitles.Sort(new SubtitleComparer());
            subtitlesDataEditor1.RefreshSubtitles();
            timeLine1.UpdateSubtitlesReview();
            Save = true;
            AddHistory(Program.ResourceManager.GetString("History_SubtitlesMergeAt") + " " +
                TimeFormatConvertor.To_TimeSpan_Milli(newSubtitle.StartTime));

            // show the properties of this new subtitle
            ListViewItem_Subtitle newItem = new ListViewItem_Subtitle();
            newItem.Subtitle = newSubtitle;
            Frm_SubtitleEdit frm = new Frm_SubtitleEdit(newItem, selectedTrack,
                Program.Settings.SubtitleTextEditorBackColor, Program.Settings.AllowErrorsInEditControl,
                        Program.ProjectManager.Project.UsePreparedText,
                        Program.ProjectManager.Project.PreparedTextRTF,
                        Program.ProjectManager.Project.CutPreparedTextAfterAdd,
                Program.ProjectManager.Project.WordWrapPreparedText);
            frm.RefreshStyles(Program.ProjectManager.Project.Styles.ToArray());
            frm.EditStylesRequest += SubtitleEditor1_EditStylesRequest;
            frm.SelectAll();
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                if (Program.ProjectManager.Project.UsePreparedText && Program.ProjectManager.Project.CutPreparedTextAfterAdd)
                {
                    Program.ProjectManager.Project.PreparedTextRTF = frm.PreparedTextRTFAfterChange;
                    preparedTextEditor.RefreshText(Program.ProjectManager.Project.PreparedTextRTF);
                }
            }
        }
        private void mediaPlayer1_Enter(object sender, EventArgs e)
        {
            activeMode = ActiveMode.None;
        }
        private void errorsChecker1_Enter(object sender, EventArgs e)
        {
            activeMode = ActiveMode.None;
        }
        private void subtitleEditor1_Enter(object sender, EventArgs e)
        {
            activeMode = ActiveMode.Properties;
        }
        private void multipleSubtitleTrackViewer_Enter(object sender, EventArgs e)
        {
            activeMode = ActiveMode.None;
        }
        private void multipleSubtitleTracksViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpandTabControls();
            Program.Settings.CurrentLayout.Tab_MultipleSubtitleTracksViewer.Visible = multipleSubtitleTracksViewerToolStripMenuItem.Checked;
            SetTabParent(Program.Settings.CurrentLayout.Tab_MultipleSubtitleTracksViewer, tabPage_multipleSubtitleTrackViewer);
            CheckForCollapse();
        }
        private void treeView_subtitleTracks_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (isUpdatingCheck) return;
            // Update check values
            for (int i = 0; i < Program.ProjectManager.Project.SubtitleTracks.Count; i++)
            {
                Program.ProjectManager.Project.SubtitleTracks[i].Preview = treeView_subtitleTracks.Nodes[i].Checked;
            }
            multipleSubtitleTrackViewer.UpdateTrackControls();
        }
        private void multipleSubtitleTrackViewer_UpdateTrackChecks(object sender, EventArgs e)
        {
            isUpdatingCheck = true;
            for (int i = 0; i < Program.ProjectManager.Project.SubtitleTracks.Count; i++)
            {
                treeView_subtitleTracks.Nodes[i].Checked = Program.ProjectManager.Project.SubtitleTracks[i].Preview;
            }
            isUpdatingCheck = false;
        }
        private void richTextBox_description_Enter(object sender, EventArgs e)
        {
            this.activeMode = ActiveMode.ProjectDescription;
        }
        private void treeView_subtitleTracks_BeforeLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            isRenaming = true;
        }
        private void tabPage_media_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(typeof(TabPage)))
            {
                e.Effect = DragDropEffects.Move;
            }
            else if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }
        private void tabPage_media_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                if (files == null) return;
                if (files.Length > 0)
                    ChangeMedia(files[0], true);
            }
        }
        private void tabPage_media_DragOver(object sender, DragEventArgs e)
        {

        }
        // show/hide
        private void preparedTextToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExpandTabControls();
            Program.Settings.CurrentLayout.Tab_PreparedText.Visible = preparedTextToolStripMenuItem.Checked;
            SetTabParent(Program.Settings.CurrentLayout.Tab_PreparedText, tabPage_preparedText);
            CheckForCollapse();
        }
        private void preparedTextEditor_Enter(object sender, EventArgs e)
        {
            activeMode = ActiveMode.PreparedText;
        }
        private void timeLine1_Enter(object sender, EventArgs e)
        {
            activeMode = ActiveMode.None;
        }
        private void preparedTextEditor_SaveRequest(object sender, EventArgs e)
        {
            Program.ProjectManager.Project.PreparedTextRTF = preparedTextEditor.TextRTF;
            AddHistory(Program.ResourceManager.GetString("History_PreparedTextChanged"));
            Save = true;
        }
        private void preparedTextEditor_TextChanged(object sender, EventArgs e)
        {
            Program.ProjectManager.Project.PreparedTextRTF = preparedTextEditor.TextRTF;
        }
        private void toolStripButton33_Click(object sender, EventArgs e)
        {
            toolStripButton33.Checked = !toolStripButton33.Checked;
            Program.ProjectManager.Project.UsePreparedText = toolStripButton33.Checked;
        }
        private void toolStripButton34_Click(object sender, EventArgs e)
        {
            toolStripButton34.Checked = !toolStripButton34.Checked;
            Program.ProjectManager.Project.CutPreparedTextAfterAdd = toolStripButton34.Checked;
        }
        private void hotKey1_HotKeyPressed(object sender, HotKeyEventArgs e)
        {
            if (activeMode == ActiveMode.Properties ||
                 activeMode == ActiveMode.ProjectDescription ||
                 activeMode == ActiveMode.PreparedText ||
                 isRenaming || isChangingSettings || isEditingSubtitle)
                return;

            if (e.HotKey == Program.Settings.ShortcutAddSubtitle)
            {
                if (File.Exists(Program.ProjectManager.Project.MediaPath))
                {
                    if (!IsAddingSubtitle && mediaPlayer1.AddButtonEnabled && mediaPlayer1.IsPlaying)
                        mediaPlayer1_AddButtonHoldPress(this, null);
                    else
                    {
                        if (IsAddingSubtitle)
                            mediaPlayer1_AddButtonHoldRelease(this, null);
                        else
                            mediaPlayer1.Play();
                    }
                }
            }
            if (e.HotKey == Program.Settings.ShortcutPreviousSubtitle)
            {
                subtitlesDataEditor1.SelectPreviousSubtitle();
            }
            if (e.HotKey == Program.Settings.ShortcutNextSubtitle)
            {
                subtitlesDataEditor1.SelectNextSubtitle();
            }
            if (e.HotKey == Program.Settings.ShortcutJumpIntoSelectedSubtitleTime)
            {
                if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 1)
                    mediaPlayer1.CurrentPosition = subtitlesDataEditor1.SelectedSubtitleItems[0].Subtitle.StartTime;
            }
            if (e.HotKey == Program.Settings.ShortcutFWD)
            {
                mediaPlayer1.NextFrame();
            }
            if (e.HotKey == Program.Settings.ShortcutREW)
            {
                mediaPlayer1.PreviousFrame();
            }
            if (e.HotKey == Program.Settings.ShortcutStretchToNext)
                stretchToNextToolStripMenuItem_Click(this, null);
            if (e.HotKey == Program.Settings.ShortcutStretchToPrevious)
                stretchToPreviousToolStripMenuItem_Click(this, null);
        }
        private void Frm_Main_Activated(object sender, EventArgs e)
        {
            // Hot keys
            hotKey1.RemoveAllKeys();
            if (Program.Settings.ShortcutAddSubtitle != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutAddSubtitle);
            if (Program.Settings.ShortcutPreviousSubtitle != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutPreviousSubtitle);
            if (Program.Settings.ShortcutNextSubtitle != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutNextSubtitle);
            if (Program.Settings.ShortcutJumpIntoSelectedSubtitleTime != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutJumpIntoSelectedSubtitleTime);
            if (Program.Settings.ShortcutFWD != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutFWD);
            if (Program.Settings.ShortcutREW != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutREW);
            if (Program.Settings.ShortcutStretchToNext != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutStretchToNext);
            if (Program.Settings.ShortcutStretchToPrevious != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutStretchToPrevious);
        }
        private void Frm_Main_Deactivate(object sender, EventArgs e)
        {
            // Disable the hotkeys manager so that user can use keyboard out side ASM
            hotKey1.RemoveAllKeys();
        }
        private void ComboBox_search_Enter(object sender, EventArgs e)
        {
            // Disable the hotkeys manager so that user can use keyboard to enter search word
            hotKey1.RemoveAllKeys();
        }
        private void ComboBox_search_Leave(object sender, EventArgs e)
        {
            // Return Hot keys
            hotKey1.RemoveAllKeys();
            if (Program.Settings.ShortcutAddSubtitle != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutAddSubtitle);
            if (Program.Settings.ShortcutPreviousSubtitle != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutPreviousSubtitle);
            if (Program.Settings.ShortcutNextSubtitle != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutNextSubtitle);
            if (Program.Settings.ShortcutJumpIntoSelectedSubtitleTime != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutJumpIntoSelectedSubtitleTime);
            if (Program.Settings.ShortcutFWD != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutFWD);
            if (Program.Settings.ShortcutREW != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutREW);
            if (Program.Settings.ShortcutStretchToNext != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutStretchToNext);
            if (Program.Settings.ShortcutStretchToPrevious != "")
                hotKey1.AddHotKey(Program.Settings.ShortcutStretchToPrevious);
        }
        private void toolStripButton_prepared_word_wrap_Click(object sender, EventArgs e)
        {
            toolStripButton_prepared_word_wrap.Checked = !toolStripButton_prepared_word_wrap.Checked;
            Program.ProjectManager.Project.WordWrapPreparedText = toolStripButton_prepared_word_wrap.Checked;
            preparedTextEditor.WordWrap = toolStripButton_prepared_word_wrap.Checked;
        }
        private void CheckForUpdates(object sender, EventArgs e)
        {
            ProcessStartInfo str = new ProcessStartInfo();
            str.Arguments = "/checknow /lang " + "\"" + Program.Settings.Language + "\"";
            str.FileName = Program.StartUpPath + "\\Updater.exe";
            Process.Start(str);
        }
        private void previewWithAHDSynchronisedLyricsViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!File.Exists(Program.ProjectManager.Project.MediaPath) || !MediaPlayerManager.Initialized)
            {
                MessageDialog.ShowErrorMessage(Program.ResourceManager.GetString("Message_NoMediaLoaded"),
                    Program.ResourceManager.GetString("Messagecaption_PreviewwithAHDSynchronisedLyricsViewer"));
                return;
            }
            mediaPlayer1.Pause();
            ProcessStartInfo str = new ProcessStartInfo();
            str.Arguments = "\"" + Program.ProjectManager.Project.MediaPath + "\"" + " /lang " + "\"" + Program.Settings.Language + "\"";
            str.FileName = Program.StartUpPath + "\\aslv.exe";
            Process.Start(str);
        }
        private void previewWithAHDSynchronisedLyricsViewToolStripMenuItem_EnabledChanged(object sender, EventArgs e)
        {
            toolStripButton_preview.Enabled = previewWithAHDSynchronisedLyricsViewToolStripMenuItem.Enabled;
        }
        private void MediaPlayer1_SubtitleTextEditStarted(object sender, EventArgs e)
        {
            isEditingSubtitle = true;
            // Remove all hotkeys so user can edit !
            hotKey1.RemoveKey(Program.Settings.ShortcutAddSubtitle);
            hotKey1.RemoveKey(Program.Settings.ShortcutFWD);
            hotKey1.RemoveKey(Program.Settings.ShortcutREW);
        }
        private void TimeLine1_MarkSelected(object sender, MarkSelectionArgs e)
        {
            if (e.MarkIndex >= 0 && e.MarkIndex < ComboBox_marks.Items.Count)
                ComboBox_marks.SelectedIndex = e.MarkIndex;
        }
        private void selectAfterSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subtitlesDataEditor1.SelectAllAfterSelected();
        }
        private void selectAllBeforeSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            subtitlesDataEditor1.SelectAllBeforeSelected();
        }
        private void TimeLine1_SelectedTrackChanged(object sender, EventArgs e)
        {
            foreach (TreeNode_SubtitlesTrack trk in treeView_subtitleTracks.Nodes)
            {
                if (trk.SubtitlesTrack.Name == timeLine1.SelectedTrackInComboboxName)
                {
                    treeView_subtitleTracks.SelectedNode = trk;
                    break;
                }
            }
        }
        private void SubtitleEditor1_EditStylesRequest(object sender, EventArgs e)
        {
            Frm_EditStyles frm = new Frm_EditStyles(Program.ProjectManager.Project.Styles.ToArray());
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                Program.ProjectManager.Project.Styles = frm.EnteredStyles;
                subtitleEditor1.RefreshStyles(Program.ProjectManager.Project.Styles.ToArray());
                preparedTextEditor.RefreshStyles(Program.ProjectManager.Project.Styles.ToArray());
                mediaPlayer1.RefreshStyles(Program.ProjectManager.Project.Styles.ToArray());
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_StylesEdited"));
            }
        }
        private void setStyleForSelectedSubtitlesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_StretchToPrevious"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectAtLeastOneSubtitle"),
                   Program.ResourceManager.GetString("MessageCaption_NosubtitleSelected"));
                return;
            }
            Frm_SelectStyle frm = new Frm_SelectStyle(Program.ProjectManager.Project.Styles.ToArray());
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                foreach (ListViewItem_Subtitle subItem in subtitlesDataEditor1.SelectedSubtitleItems)
                {
                    subItem.Subtitle.Text.SetFont(frm.SelectedStyle.Font);
                    subItem.Subtitle.Text.SetColor(frm.SelectedStyle.Color);
                }
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_FontSetForSelected"));
            }
        }
        private void MediaPlayer1_EditSubtitleRequest(object sender, EventArgs e)
        {
            if (sIndex >= 0)
            {
                Frm_SubtitleEdit subtitleEdit = new Frm_SubtitleEdit(subtitlesDataEditor1.SubtitleItems[sIndex],
                  selectedTrack, Program.Settings.SubtitleTextEditorBackColor, Program.Settings.AllowErrorsInEditControl,
                  Program.ProjectManager.Project.UsePreparedText,
              Program.ProjectManager.Project.PreparedTextRTF,
              Program.ProjectManager.Project.CutPreparedTextAfterAdd,
              Program.ProjectManager.Project.WordWrapPreparedText);
                subtitleEdit.RefreshStyles(Program.ProjectManager.Project.Styles.ToArray());
                subtitleEdit.EditStylesRequest += SubtitleEditor1_EditStylesRequest;

                if (Program.Settings.EditMoreThanOneSubtitleUsingEditorWindow)
                {
                    subtitleEdit.TopMost = true;
                    subtitleEdit.OkButtonPressed += new EventHandler(subtitleEdit_OkButtonPressed);
                    subtitleEdit.Show();
                }
                else
                {
                    if (subtitleEdit.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                    {
                        Save = true;
                        AddHistory(Program.ResourceManager.GetString("History_SubtitlePropertiesChanged"));
                        ApplyTrackToControls();
                    }
                }
                pIndex = -1;
            }
        }
        private void WaveReader_Progress(object sender, ProgressArgs e)
        {
            if (!this.InvokeRequired)
            {
                SetStatus(e.PrecentageCompleted, e.Status);
            }
            else
            {
                Invoke(new SetStatusDelegate(SetStatus), e.PrecentageCompleted, e.Status);
            }
        }
        private delegate void SetStatusDelegate(int p, string s);
        private void SetStatus(int p, string s)
        {
            ProgressBar1.Value = p;
            StatusLabel.Text = s;
        }
        private void ShowStatus()
        {
            ProgressBar1.Visible = true;
            StatusLabel.Visible = true;
        }
        private void HideStatus()
        {
            ProgressBar1.Visible = false;
            StatusLabel.Visible = false;
        }
        private void WaveReader_ProgressFinished(object sender, EventArgs e)
        {
            if (!this.InvokeRequired)
            {
                ProgressBar1.Visible = true;
                StatusLabel.Visible = true;
            }
            else
            {
                Invoke(new Action(HideStatus));
            }
        }
        private void WaveReader_ProgressStarted(object sender, EventArgs e)
        {
            if (!this.InvokeRequired)
            {
                ProgressBar1.Visible = true;
                StatusLabel.Visible = true;
            }
            else
            {
                Invoke(new Action(ShowStatus));
            }
        }
        private void MediaPlayer1_EditCustomStyleRequest(object sender, EventArgs e)
        {
            isChangingSettings = true;
            mediaPlayer1.Pause();
            Frm_Settings frm = new Frm_Settings("media-player");
            frm.ShowDialog(this);
            ApplySettings();
            isChangingSettings = false;
        }
        private void SubtitlesDataEditor1_SubtitlePropertiesChanged(object sender, EventArgs e)
        {
            //refresh controls
            selectedTrack.CheckSubtitlesOrder();
            subtitlesDataEditor1.RefreshSubtitles();
            //set save and history
            AddHistory(Program.ResourceManager.GetString("History_SubtitlePropertiesChanged"));
            Save = true;

            activeMode = ActiveMode.None;
        }
        private void splitCaptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_SpellCheck"));
                return;
            }

            MediaPlayerManager.Pause();

            Frm_SplitCaptions frm = new Frm_SplitCaptions(selectedTrack);
            if (frm.ShowDialog(this) == DialogResult.OK)
            {
                //clone
                ProgressBar1.Maximum = 100;
                ProgressBar1.Visible = true;
                SetStatus(Program.ResourceManager.GetString("Status_SplittingCaptions"), StatusType.None);
                SubtitlesTrack track_captions = null;
                SubtitlesTrack track_texts = null;
                if (!frm.IsOneTrack)
                {
                    // Create both tracks
                    string name1 = selectedTrack.Name + "_" + "Captions";
                    string name2 = selectedTrack.Name + "_" + "Texts";
                    int i = 1;
                    while (Program.ProjectManager.Project.IsSubtitlesTrackExist(name1))
                    {
                        name1 = selectedTrack.Name + "_" + "Captions" + i;
                        i++;
                    }
                    i = 1;
                    while (Program.ProjectManager.Project.IsSubtitlesTrackExist(name2))
                    {
                        name2 = selectedTrack.Name + "_" + "Texts" + i;
                        i++;
                    }
                    track_captions = new SubtitlesTrack(name1);
                    track_texts = new SubtitlesTrack(name2);
                    Program.ProjectManager.Project.SubtitleTracks.Add(track_captions);
                    Program.ProjectManager.Project.SubtitleTracks.Add(track_texts);
                }
                else
                {
                    if (frm.OneTrackCaptionsOnly)
                    {
                        // Only captions
                        string name1 = selectedTrack.Name + "_" + "Captions";
                        int i = 1;
                        while (Program.ProjectManager.Project.IsSubtitlesTrackExist(name1))
                        {
                            name1 = selectedTrack.Name + "_" + "Captions" + i;
                            i++;
                        }
                        track_captions = new SubtitlesTrack(name1);
                        Program.ProjectManager.Project.SubtitleTracks.Add(track_captions);
                    }
                    else
                    {
                        string name2 = selectedTrack.Name + "_" + "Texts";
                        int i = 1;
                        while (Program.ProjectManager.Project.IsSubtitlesTrackExist(name2))
                        {
                            name2 = selectedTrack.Name + "_" + "Texts" + i;
                            i++;
                        }
                        track_texts = new SubtitlesTrack(name2);
                        Program.ProjectManager.Project.SubtitleTracks.Add(track_texts);
                    }
                }
                for (int i = 0; i < selectedTrack.Subtitles.Count; i++)
                {
                    SubtitleText CaptionsOnly = TextFormatter.GetCaptionsOnly(selectedTrack.Subtitles[i].Text,
                        frm.IsCaptionSurrounds, frm.CaptionsSurroundingFirstChar, frm.CaptionsSurroundingLastChar);
                    SubtitleText TextsOnly = TextFormatter.GetTextOnly(selectedTrack.Subtitles[i].Text,
                        frm.IsCaptionSurrounds, frm.CaptionsSurroundingFirstChar, frm.CaptionsSurroundingLastChar);
                    if (!frm.IsOneTrack)
                    {
                        if (CaptionsOnly.ToString() != "")
                        {
                            Subtitle sub_caption = new Subtitle(selectedTrack.Subtitles[i].StartTime,
                            selectedTrack.Subtitles[i].EndTime, CaptionsOnly);
                            track_captions.Subtitles.Add(sub_caption);
                        }
                        if (TextsOnly.ToString() != "")
                        {
                            Subtitle sub_texts = new Subtitle(selectedTrack.Subtitles[i].StartTime,
                            selectedTrack.Subtitles[i].EndTime, TextsOnly);
                            track_texts.Subtitles.Add(sub_texts);
                        }
                    }
                    else
                    {
                        if (frm.OneTrackCaptionsOnly)
                        {
                            if (CaptionsOnly.ToString() != "")
                            {
                                Subtitle sub_caption = new Subtitle(selectedTrack.Subtitles[i].StartTime,
                                selectedTrack.Subtitles[i].EndTime, CaptionsOnly);
                                track_captions.Subtitles.Add(sub_caption);
                            }
                        }
                        else
                        {
                            if (TextsOnly.ToString() != "")
                            {
                                Subtitle sub_texts = new Subtitle(selectedTrack.Subtitles[i].StartTime,
                                selectedTrack.Subtitles[i].EndTime, TextsOnly);
                                track_texts.Subtitles.Add(sub_texts);
                            }
                        }
                    }

                    ProgressBar1.Value = (i * 100) / selectedTrack.Subtitles.Count;
                }
                // Finish
                ProgressBar1.Visible = false;
                RefreshSubtitleTracks();
                SetStatus(Program.ResourceManager.GetString("Status_Done"), StatusType.Good);
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_SubtitlesTrackCaptionsSplit"));
            }
        }
        private void applyLinebreakToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (selectedTrack == null)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_PleaseSelectSubtitlesTrackFirst"),
                  Program.ResourceManager.GetString("MessageCaption_StretchToPrevious"));
                return;
            }
            if (subtitlesDataEditor1.SelectedSubtitleItems.Length == 0)
            {
                MessageDialog.ShowMessage(Program.ResourceManager.GetString("Message_YouMustSelectAtLeastOneSubtitle"),
                   Program.ResourceManager.GetString("MessageCaption_NosubtitleSelected"));
                return;
            }

            Frm_LineBreakRule frm = new Frm_LineBreakRule();
            if (frm.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
            {
                SubtitleTextEditor edi = new SubtitleTextEditor();
                ProgressBar1.Visible = true;
                ProgressBar1.Minimum = 0;
                ProgressBar1.Maximum = 100;
                int x = 0;
                int max = subtitlesDataEditor1.SelectedSubtitleItems.Length;
                foreach (ListViewItem_Subtitle subItem in subtitlesDataEditor1.SelectedSubtitleItems)
                {
                    SubtitleText text = subItem.Subtitle.Text;
                    // capital letter
                    if (frm.RuleCapialLatter)
                    {
                        for (int line = 0; line < text.TextLines.Count; line++)
                        {
                            for (int chr = 0; chr < text.TextLines[line].Chars.Count; chr++)
                            {
                                char currentChar = text.TextLines[line].Chars[chr].TheChar;
                                if (Char.IsUpper(currentChar))
                                {
                                    // this is it !!
                                    // insert a line-break here !
                                    // we need to check the previous line first
                                    bool can_do_it = true;
                                    int bef = chr - 1;
                                    if (bef >= 0)
                                    {
                                        if (text.TextLines[line].Chars[bef].TheChar == '\n')
                                        {
                                            can_do_it = false;
                                        }
                                    }
                                    else
                                    {
                                        // Already at the first of the line !
                                        can_do_it = false;
                                    }
                                    if (can_do_it)
                                    {
                                        // insert it !!
                                        text.TextLines[line].Chars.Insert(chr, new SubtitleChar('\n', text.TextLines[line].Chars[chr].Font, text.TextLines[line].Chars[chr].Color));
                                        chr += 2;
                                    }
                                }
                            }
                        }
                    }
                    // char-limit
                    if (frm.RuleCharLimit)
                    {
                        int val = frm.RuleCharLimitValue;

                        for (int line = 0; line < text.TextLines.Count; line++)
                        {
                            int line_count = text.TextLines[line].Chars.Count;

                            if (line_count > val)
                            {
                                int segmants = line_count / val;
                                int ind = val;
                                for (int i = 0; i < segmants; i++)
                                {
                                    if (ind < text.TextLines[line].Chars.Count)
                                    {
                                        // We can only insert break when the index fall into a space
                                        if (text.TextLines[line].Chars[ind].TheChar != ' ')
                                        {
                                            while (ind < text.TextLines[line].Chars.Count)
                                            {
                                                ind++;
                                                if (ind >= text.TextLines[line].Chars.Count)
                                                    break;
                                                if (text.TextLines[line].Chars[ind].TheChar == ' ')
                                                    break;
                                            }
                                        }
                                        if (ind < text.TextLines[line].Chars.Count)
                                            text.TextLines[line].Chars.Insert(ind, new SubtitleChar('\n', text.TextLines[line].Chars[ind].Font, text.TextLines[line].Chars[ind].Color));
                                        ind += 1 + val;
                                    }
                                }
                            }
                        }
                    }
                    // punctuation
                    if (frm.RulePuncuation)
                    {
                        string[] punctuations = frm.RulePuncationText.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                        List<string> check_list = new List<string>(punctuations);
                        for (int line = 0; line < text.TextLines.Count; line++)
                        {
                            for (int chr = 0; chr < text.TextLines[line].Chars.Count; chr++)
                            {
                                string currentChar = text.TextLines[line].Chars[chr].TheChar.ToString();
                                if (check_list.Contains(currentChar))
                                {
                                    // this is it !!
                                    // insert a line-break here !
                                    // we need to check the next line first
                                    bool can_do_it = true;
                                    int af = chr + 1;
                                    if (af < text.TextLines[line].Chars.Count)
                                    {
                                        if (text.TextLines[line].Chars[af].TheChar == '\n')
                                        {
                                            can_do_it = false;
                                        }
                                    }
                                    else
                                    {
                                        // Already at the first of the line !
                                        can_do_it = false;
                                    }
                                    if (can_do_it)
                                    {
                                        if (frm.RulePuncuationAfter)
                                        {
                                            // insert it !!
                                            text.TextLines[line].Chars.Insert(af, new SubtitleChar('\n', text.TextLines[line].Chars[chr].Font, text.TextLines[line].Chars[chr].Color));
                                            chr += 2;
                                        }
                                        else
                                        {
                                            text.TextLines[line].Chars.Insert(chr, new SubtitleChar('\n', text.TextLines[line].Chars[chr].Font, text.TextLines[line].Chars[chr].Color));
                                            chr++;
                                        }
                                    }
                                }
                            }
                        }
                    }


                    edi.SubtitleText = text;
                    edi.SaveTextToSubtitle();

                    // Do a fix, remove space from the start of each line
                    for (int line = 0; line < edi.SubtitleText.TextLines.Count; line++)
                    {
                        while (edi.SubtitleText.TextLines[line].Chars.Count > 0)
                        {
                            if (edi.SubtitleText.TextLines[line].Chars[0].TheChar == ' ')
                            {
                                edi.SubtitleText.TextLines[line].Chars.RemoveAt(0);
                            }
                            else
                                break;
                        }
                    }

                    subItem.Subtitle.Text = edi.SubtitleText;

                    ProgressBar1.Value = (x * 100) / max;
                    statusStrip1.Refresh();
                }
                Save = true;
                AddHistory(Program.ResourceManager.GetString("History_LineBreakApplied"));
                ProgressBar1.Visible = false;
            }
        }
        private void wikiOnlineHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/alaahadid/AHD-Subtitles-Maker/wiki");
        }
    }
    public enum ActiveMode
    { SubtitlesTrack, Subtitles, Properties, ProjectDescription, PreparedText, None }
}
