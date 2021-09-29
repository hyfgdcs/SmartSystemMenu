﻿using System;
using System.Windows.Forms;
using System.IO;
using System.Linq;
using SmartSystemMenu.Settings;
using SmartSystemMenu.Controls;

namespace SmartSystemMenu.Forms
{
    public partial class SettingsForm : Form
    {
        private SmartSystemMenuSettings _settings;
        private CloserSettings _closerSettings;

        public event EventHandler<SmartSystemMenuSettingsEventArgs> OkClick;

        public SettingsForm(SmartSystemMenuSettings settings)
        {
            InitializeComponent();

            try
            {
                _settings = settings;
                _closerSettings = (CloserSettings)settings.Closer.Clone();
                InitializeControls(settings);
            }
            catch
            {
                tabMain.Enabled = false;
                btnApply.Enabled = false;
                MessageBox.Show("Failed to read the settings.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InitializeControls(SmartSystemMenuSettings settings)
        {
            tabpGeneral.Text = settings.LanguageSettings.GetValue("tab_settings_general");
            tabpMenuStart.Text = settings.LanguageSettings.GetValue("tab_settings_menu_start");
            tabpMenuSize.Text = settings.LanguageSettings.GetValue("tab_settings_menu_size");
            tabpMenu.Text = settings.LanguageSettings.GetValue("tab_settings_menu");
            grpbLanguage.Text = settings.LanguageSettings.GetValue("grpb_language");
            grpbProcessExclusions.Text = settings.LanguageSettings.GetValue("grpb_process_exclusions");
            grpbStartProgram.Text = settings.LanguageSettings.GetValue("grpb_start_program");
            grpbWindowSize.Text = settings.LanguageSettings.GetValue("grpb_window_size");
            grpbCloser.Text = settings.LanguageSettings.GetValue("grpb_closer");
            grpbSizer.Text = settings.LanguageSettings.GetValue("grpb_sizer");
            clmProcessExclusionName.HeaderText = settings.LanguageSettings.GetValue("clm_process_exclusion_name");
            clmProcessExclusionEdit.ToolTipText = settings.LanguageSettings.GetValue("clm_process_exclusion_edit");
            clmProcessExcusionDelete.ToolTipText = settings.LanguageSettings.GetValue("clm_process_exclusion_delete");
            clmStartProgramTitle.HeaderText = settings.LanguageSettings.GetValue("clm_start_program_title");
            clmStartProgramPath.HeaderText = settings.LanguageSettings.GetValue("clm_start_program_path");
            clmStartProgramArguments.HeaderText = settings.LanguageSettings.GetValue("clm_start_program_arguments");
            clmStartProgramEdit.ToolTipText = settings.LanguageSettings.GetValue("clm_start_program_edit");
            clmStartProgramDelete.ToolTipText = settings.LanguageSettings.GetValue("clm_start_program_delete");
            clmWindowSizeTitle.HeaderText = settings.LanguageSettings.GetValue("clm_window_size_title");
            clmWindowSizeLeft.HeaderText = settings.LanguageSettings.GetValue("clm_window_size_left");
            clmWindowSizeTop.HeaderText = settings.LanguageSettings.GetValue("clm_window_size_top");
            clmWindowSizeWidth.HeaderText = settings.LanguageSettings.GetValue("clm_window_size_width");
            clmWindowSizeHeight.HeaderText = settings.LanguageSettings.GetValue("clm_window_size_height");
            clmWindowSizeEdit.ToolTipText = settings.LanguageSettings.GetValue("clm_window_size_edit");
            clmWindowSizeDelete.ToolTipText = settings.LanguageSettings.GetValue("clm_window_size_delete");
            clmnMenuItemName.HeaderText = settings.LanguageSettings.GetValue("clm_hotkeys_name");
            clmnHotkeys.HeaderText = settings.LanguageSettings.GetValue("clm_hotkeys_keys");
            toolTipAddProcessName.SetToolTip(btnProcessExclusionDown, settings.LanguageSettings.GetValue("btn_process_exclusion_down"));
            toolTipAddProcessName.SetToolTip(btnProcessExclusionUp, settings.LanguageSettings.GetValue("btn_process_exclusion_up"));
            toolTipAddProcessName.SetToolTip(btnAddProcessExclusion, settings.LanguageSettings.GetValue("btn_add_process_exclusion"));
            toolTipAddProcessName.SetToolTip(btnAddStartProgram, settings.LanguageSettings.GetValue("btn_add_start_program"));
            toolTipAddProcessName.SetToolTip(btnStartProgramDown, settings.LanguageSettings.GetValue("btn_start_program_down"));
            toolTipAddProcessName.SetToolTip(btnStartProgramUp, settings.LanguageSettings.GetValue("btn_start_program_up"));
            toolTipAddProcessName.SetToolTip(btnAddWindowSize, settings.LanguageSettings.GetValue("btn_add_window_size"));
            toolTipAddProcessName.SetToolTip(btnWindowSizeDown, settings.LanguageSettings.GetValue("btn_window_size_down"));
            toolTipAddProcessName.SetToolTip(btnWindowSizeUp, settings.LanguageSettings.GetValue("btn_window_size_up"));
            btnCloser.Text = settings.LanguageSettings.GetValue("closer_button_name");
            btnApply.Text = settings.LanguageSettings.GetValue("settings_btn_apply");
            btnCancel.Text = settings.LanguageSettings.GetValue("settings_btn_cancel");
            Text = settings.LanguageSettings.GetValue("settings_form");

            foreach (var processExclusion in settings.ProcessExclusions)
            {
                var index = gvProcessExclusions.Rows.Add();
                var row = gvProcessExclusions.Rows[index];
                row.Cells[0].Value = processExclusion;
                row.Cells[1].ToolTipText = settings.LanguageSettings.GetValue("clm_process_exclusion_edit");
                row.Cells[2].ToolTipText = settings.LanguageSettings.GetValue("clm_process_exclusion_delete");
            }

            foreach (var item in settings.MenuItems.WindowSizeItems)
            {
                var index = gvWindowSize.Rows.Add();
                var row = gvWindowSize.Rows[index];
                row.Tag = (WindowSizeMenuItem)item.Clone();
                row.Cells[0].Value = item.Title;
                row.Cells[1].Value = item.Left.HasValue ? item.Left.ToString() : string.Empty;
                row.Cells[2].Value = item.Top.HasValue ? item.Top.ToString() : string.Empty;
                row.Cells[3].Value = item.Width.ToString();
                row.Cells[4].Value = item.Height.ToString();
                row.Cells[5].Value = item.ToString();
                row.Cells[6].ToolTipText = settings.LanguageSettings.GetValue("clm_window_size_edit");
                row.Cells[7].ToolTipText = settings.LanguageSettings.GetValue("clm_window_size_delete");
            }

            foreach (var item in settings.MenuItems.StartProgramItems)
            {
                var index = gvStartProgram.Rows.Add();
                var row = gvStartProgram.Rows[index];
                row.Cells[0].Value = item.Title;
                row.Cells[1].Value = item.FileName;
                row.Cells[2].Value = item.Arguments;
                row.Cells[3].ToolTipText = settings.LanguageSettings.GetValue("clm_start_program_edit");
                row.Cells[4].ToolTipText = settings.LanguageSettings.GetValue("clm_start_program_delete");
            }

            cmbLanguage.DisplayMember = "Text";
            cmbLanguage.ValueMember = "Value";

            var languageItems = new[] {
                new { Text = "", Value = "" },
                new { Text = "English", Value = "en" },
                new { Text = "Deutsch", Value = "de" },
                new { Text = "Português", Value = "pt" },
                new { Text = "Русский", Value = "ru" },
                new { Text = "Српски", Value = "sr" },
                new { Text = "简体中文", Value = "zh_cn" },
                new { Text = "繁體中文", Value = "zh_tw"},
                new { Text = "日本語", Value = "ja" },
                new { Text = "한국어", Value = "ko" }
            };

            cmbLanguage.DataSource = languageItems;
            cmbLanguage.SelectedValue = settings.LanguageName;

            cmbSizer.Items.Add(settings.LanguageSettings.GetValue("sizer_window_with_margins"));
            cmbSizer.Items.Add(settings.LanguageSettings.GetValue("sizer_window_without_margins"));
            cmbSizer.Items.Add(settings.LanguageSettings.GetValue("sizer_window_client_area"));
            cmbSizer.SelectedIndex = (int)settings.Sizer;

            FillGridViewRowHotkey(gvHotkeys, settings, "information");
            FillGridViewRowHotkey(gvHotkeys, settings, "roll_up");
            FillGridViewRowHotkey(gvHotkeys, settings, "aero_glass");
            FillGridViewRowHotkey(gvHotkeys, settings, "always_on_top");
            FillGridViewRowHotkey(gvHotkeys, settings, "send_to_bottom");
            FillGridViewRowHotkey(gvHotkeys, settings, "save_screenshot");
            FillGridViewRowHotkey(gvHotkeys, settings, "open_file_in_explorer");
            FillGridViewRowHotkey(gvHotkeys, settings, "drag_by_mouse");
            FillGridViewGroupHotkey(gvHotkeys, settings, "size");
            FillGridViewRowHotkey(gvHotkeys, settings, "size_default", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "size_custom", null, true);
            FillGridViewGroupHotkey(gvHotkeys, settings, "move_to");
            FillGridViewGroupHotkey(gvHotkeys, settings, "alignment");
            FillGridViewRowHotkey(gvHotkeys, settings, "align_top_left", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "align_top_center", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "align_top_right", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "align_middle_left", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "align_middle_center", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "align_middle_right", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "align_bottom_left", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "align_bottom_center", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "align_bottom_right", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "align_default", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "align_custom", null, true);
            FillGridViewGroupHotkey(gvHotkeys, settings, "transparency");
            FillGridViewRowHotkey(gvHotkeys, settings, "trans_opaque", "0%" + settings.LanguageSettings.GetValue("trans_opaque"), true);
            FillGridViewRowHotkey(gvHotkeys, settings, "10%", "10%", true);
            FillGridViewRowHotkey(gvHotkeys, settings, "20%", "20%", true);
            FillGridViewRowHotkey(gvHotkeys, settings, "30%", "30%", true);
            FillGridViewRowHotkey(gvHotkeys, settings, "40%", "40%", true);
            FillGridViewRowHotkey(gvHotkeys, settings, "50%", "50%", true);
            FillGridViewRowHotkey(gvHotkeys, settings, "60%", "60%", true);
            FillGridViewRowHotkey(gvHotkeys, settings, "70%", "70%", true);
            FillGridViewRowHotkey(gvHotkeys, settings, "80%", "80%", true);
            FillGridViewRowHotkey(gvHotkeys, settings, "90%", "90%", true);
            FillGridViewRowHotkey(gvHotkeys, settings, "trans_invisible", "100%" + settings.LanguageSettings.GetValue("trans_invisible"), true);
            FillGridViewRowHotkey(gvHotkeys, settings, "trans_default", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "trans_custom", null, true);
            FillGridViewGroupHotkey(gvHotkeys, settings, "priority");
            FillGridViewRowHotkey(gvHotkeys, settings, "priority_real_time", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "priority_high", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "priority_above_normal", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "priority_normal", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "priority_below_normal", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "priority_idle", null, true);
            FillGridViewGroupHotkey(gvHotkeys, settings, "clipboard");
            FillGridViewRowHotkey(gvHotkeys, settings, "copy_text_to_clipboard", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "clear_clipboard", null, true);
            FillGridViewGroupHotkey(gvHotkeys, settings, "system_tray");
            FillGridViewRowHotkey(gvHotkeys, settings, "minimize_to_systemtray", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "minimize_always_to_systemtray", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "suspend_to_systemtray", null, true);
            FillGridViewGroupHotkey(gvHotkeys, settings, "other_windows");
            FillGridViewRowHotkey(gvHotkeys, settings, "minimize_other_windows", null, true);
            FillGridViewRowHotkey(gvHotkeys, settings, "close_other_windows", null, true);
            FillGridViewGroupHotkey(gvHotkeys, settings, "start_program");
        }

        private void GridViewProcessExclusionsCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;

            if (grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 1)
                {
                    var cell = grid.Rows[e.RowIndex].Cells[0];
                    var dialog = new ProcessExclusionForm(cell.Value.ToString(), _settings.LanguageSettings);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        cell.Value = dialog.ProcessName;
                    }
                }

                if (e.ColumnIndex == 2)
                {
                    grid.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void GridViewStartProgramCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;

            if (grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 3)
                {
                    var cellTitle = grid.Rows[e.RowIndex].Cells[0];
                    var cellFileName = grid.Rows[e.RowIndex].Cells[1];
                    var cellArguments = grid.Rows[e.RowIndex].Cells[2];

                    var dialog = new StartProgramForm(cellTitle.Value.ToString(), cellFileName.Value.ToString(), cellArguments.Value.ToString(), _settings.LanguageSettings);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        cellTitle.Value = dialog.Title;
                        cellFileName.Value = dialog.FileName;
                        cellArguments.Value = dialog.Arguments;
                    }
                }

                if (e.ColumnIndex == 4)
                {
                    grid.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void GridViewWindowSizeCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;

            if (grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                if (e.ColumnIndex == 6 && grid.Rows[e.RowIndex].Tag is WindowSizeMenuItem menuItem)
                {
                    var dialog = new SettingsSizeForm(_settings.LanguageSettings, menuItem);
                    if (dialog.ShowDialog(this) == DialogResult.OK)
                    {
                        var row = grid.Rows[e.RowIndex];
                        row.Cells[0].Value = dialog.MenuItem.Title;
                        row.Cells[1].Value = dialog.MenuItem.Left.HasValue ? dialog.MenuItem.Left.ToString() : string.Empty;
                        row.Cells[2].Value = dialog.MenuItem.Top.HasValue ? dialog.MenuItem.Top.ToString() : string.Empty;
                        row.Cells[3].Value = dialog.MenuItem.Width.ToString();
                        row.Cells[4].Value = dialog.MenuItem.Height.ToString();
                        row.Cells[5].Value = dialog.MenuItem.ToString();

                        menuItem.Title = dialog.MenuItem.Title;
                        menuItem.Left = dialog.MenuItem.Left;
                        menuItem.Top = dialog.MenuItem.Top;
                        menuItem.Width = dialog.MenuItem.Width;
                        menuItem.Height = dialog.MenuItem.Height;
                        menuItem.Key1 = dialog.MenuItem.Key1;
                        menuItem.Key2 = dialog.MenuItem.Key2;
                        menuItem.Key3 = dialog.MenuItem.Key3;
                    }
                }

                if (e.ColumnIndex == 7)
                {
                    grid.Rows.RemoveAt(e.RowIndex);
                }
            }
        }

        private void GridViewProcessExclusionsCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (e.ColumnIndex == 0 && e.RowIndex >= 0)
            {
                var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                var dialog = new ProcessExclusionForm(cell.Value.ToString(), _settings.LanguageSettings);
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    cell.Value = dialog.ProcessName;
                }
            }
        }

        private void GridViewHotkeysCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if (grid.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
            {
                var row = grid.Rows[e.RowIndex];
                if (!row.ReadOnly)
                {
                    ShowHotkeysForm(row);
                }
            }

            if (grid.Columns[e.ColumnIndex] is DataGridViewCheckBoxColumn && e.RowIndex >= 0)
            {
                var row = grid.Rows[e.RowIndex];
                var cell = (DataGridViewCheckBoxCell)row.Cells[e.ColumnIndex];
                cell.Value = !(bool)cell.Value;
                var menuItem = (Settings.MenuItem)row.Tag;
                menuItem.Show = (bool)cell.Value;
            }
        }

        private void GridViewHotkeysCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if ((e.ColumnIndex == 0 || e.ColumnIndex == 1) && e.RowIndex >= 0)
            {
                var row = grid.Rows[e.RowIndex];
                if (!row.ReadOnly)
                {
                    ShowHotkeysForm(row);
                }
            }
        }

        private void GridViewStartProgramCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if ((e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2) && e.RowIndex >= 0)
            {
                var row = grid.Rows[e.RowIndex];
                var cellTitle = row.Cells[0];
                var cellFileName = row.Cells[1];
                var cellArguments = row.Cells[2];
                var dialog = new StartProgramForm(cellTitle.Value.ToString(), cellFileName.Value.ToString(), cellArguments.Value.ToString(), _settings.LanguageSettings);
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    cellTitle.Value = dialog.Title;
                    cellFileName.Value = dialog.FileName;
                    cellArguments.Value = dialog.Arguments;
                }
            }
        }

        private void GridViewWindowSizeCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (DataGridView)sender;
            if ((e.ColumnIndex == 0 || e.ColumnIndex == 1 || e.ColumnIndex == 2 || e.ColumnIndex == 3 || e.ColumnIndex == 4 || e.ColumnIndex == 5) && e.RowIndex >= 0 && grid.Rows[e.RowIndex].Tag is WindowSizeMenuItem menuItem)
            {
                var dialog = new SettingsSizeForm(_settings.LanguageSettings, menuItem);
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    var row = grid.Rows[e.RowIndex];
                    row.Cells[0].Value = dialog.MenuItem.Title;
                    row.Cells[1].Value = dialog.MenuItem.Left.HasValue ? dialog.MenuItem.Left.ToString() : string.Empty;
                    row.Cells[2].Value = dialog.MenuItem.Top.HasValue ? dialog.MenuItem.Top.ToString() : string.Empty;
                    row.Cells[3].Value = dialog.MenuItem.Width.ToString();
                    row.Cells[4].Value = dialog.MenuItem.Height.ToString();
                    row.Cells[5].Value = dialog.MenuItem.ToString();

                    menuItem.Title = dialog.MenuItem.Title;
                    menuItem.Left = dialog.MenuItem.Left;
                    menuItem.Top = dialog.MenuItem.Top;
                    menuItem.Width = dialog.MenuItem.Width;
                    menuItem.Height = dialog.MenuItem.Height;
                    menuItem.Key1 = dialog.MenuItem.Key1;
                    menuItem.Key2 = dialog.MenuItem.Key2;
                    menuItem.Key3 = dialog.MenuItem.Key3;
                }
            }
        }

        private void ButtonAddProcessExclusionClick(object sender, EventArgs e)
        {
            var dialog = new ProcessExclusionForm("", _settings.LanguageSettings);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                var index = gvProcessExclusions.Rows.Add();
                var row = gvProcessExclusions.Rows[index];
                row.Cells[0].Value = dialog.ProcessName;
                row.Cells[1].ToolTipText = _settings.LanguageSettings.GetValue("clm_process_exclusion_edit");
                row.Cells[2].ToolTipText = _settings.LanguageSettings.GetValue("clm_process_exclusion_delete");
            }
        }

        private void ButtonAddStartProgramClick(object sender, EventArgs e)
        {
            var dialog = new StartProgramForm("", "", "", _settings.LanguageSettings);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                var index = gvStartProgram.Rows.Add();
                var row = gvStartProgram.Rows[index];
                row.Cells[0].Value = dialog.Title;
                row.Cells[1].Value = dialog.FileName;
                row.Cells[2].Value = dialog.Arguments;
                row.Cells[3].ToolTipText = _settings.LanguageSettings.GetValue("clm_start_program_edit");
                row.Cells[4].ToolTipText = _settings.LanguageSettings.GetValue("clm_start_program_delete");
            }
        }

        private void ButtonAddWindowSizeClick(object sender, EventArgs e)
        {
            var dialog = new SettingsSizeForm(_settings.LanguageSettings, new WindowSizeMenuItem { Width = 1, Height = 1 });
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                var index = gvWindowSize.Rows.Add();
                var row = gvWindowSize.Rows[index];
                row.Cells[0].Value = dialog.MenuItem.Title;
                row.Cells[1].Value = dialog.MenuItem.Left.HasValue ? dialog.MenuItem.Left.ToString() : string.Empty;
                row.Cells[2].Value = dialog.MenuItem.Top.HasValue ? dialog.MenuItem.Top.ToString() : string.Empty;
                row.Cells[3].Value = dialog.MenuItem.Width.ToString();
                row.Cells[4].Value = dialog.MenuItem.Height.ToString();
                row.Cells[5].Value = dialog.MenuItem.ToString();
                row.Cells[6].ToolTipText = _settings.LanguageSettings.GetValue("clm_window_size_edit");
                row.Cells[7].ToolTipText = _settings.LanguageSettings.GetValue("clm_window_size_delete");
                row.Tag = dialog.MenuItem;
            }
        }

        private void ButtonArrowUpClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var grid = button.Name == "btnProcessExclusionUp" ? gvProcessExclusions : button.Name == "btnWindowSizeUp" ? gvWindowSize : gvStartProgram;
            if (grid.SelectedRows.Count > 0)
            {
                var index = grid.SelectedRows[0].Index;
                var newIndex = index > 0 ? index - 1 : 0;
                var selectedRow = grid.SelectedRows[0];
                grid.Rows.RemoveAt(index);
                grid.Rows.Insert(newIndex, selectedRow);
                grid.Rows[newIndex].Selected = true;
            }
        }

        private void ButtonArrowDownClick(object sender, EventArgs e)
        {
            var button = (Button)sender;
            var grid = button.Name == "btnProcessExclusionDown" ? gvProcessExclusions : button.Name == "btnWindowSizeDown" ? gvWindowSize : gvStartProgram;
            if (grid.SelectedRows.Count > 0)
            {
                var index = grid.SelectedRows[0].Index;
                var newIndex = index < grid.Rows.Count - 1 ? index + 1 : grid.Rows.Count - 1;
                var selectedRow = grid.SelectedRows[0];
                grid.Rows.RemoveAt(index);
                grid.Rows.Insert(newIndex, selectedRow);
                grid.Rows[newIndex].Selected = true;
            }
        }

        private void ButtonWindowCloserClick(object sender, EventArgs e)
        {
            var dialog = new SettingsCloserForm(_settings.LanguageSettings, _closerSettings.Key1, _closerSettings.Key2, _closerSettings.MouseButton, _closerSettings.Type);
            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                _closerSettings.Key1 = dialog.Key1;
                _closerSettings.Key2 = dialog.Key2;
                _closerSettings.MouseButton = dialog.MouseButton;
                _closerSettings.Type = dialog.CloserType;
            }
        }

        private void ButtonApplyClick(object sender, EventArgs e)
        {
            var settings = new SmartSystemMenuSettings();

            foreach (DataGridViewRow row in gvProcessExclusions.Rows)
            {
                settings.ProcessExclusions.Add(row.Cells[0].Value.ToString());
            }

            foreach (DataGridViewRow row in gvWindowSize.Rows)
            {
                if (row.Tag is WindowSizeMenuItem item)
                {
                    settings.MenuItems.WindowSizeItems.Add(new WindowSizeMenuItem 
                    { 
                        Title = item.Title,
                        Left = item.Left,
                        Top = item.Top,
                        Width = item.Width, 
                        Height = item.Height,
                        Key1 = item.Key1,
                        Key2 = item.Key2,
                        Key3 = item.Key3
                    });
                }
            }


            foreach (DataGridViewRow row in gvStartProgram.Rows)
            {
                settings.MenuItems.StartProgramItems.Add(new StartProgramMenuItem { Title = row.Cells[0].Value.ToString(), FileName = row.Cells[1].Value.ToString(), Arguments = row.Cells[2].Value.ToString() });
            }

            foreach (DataGridViewRow row in gvHotkeys.Rows.OfType<DataGridViewRow>().Where(x => x.Tag is Settings.MenuItem))
            {
                var menuItem = (Settings.MenuItem)row.Tag;
                settings.MenuItems.Items.Add(menuItem);
            }

            settings.Closer.Key1 = _closerSettings.Key1;
            settings.Closer.Key2 = _closerSettings.Key2;
            settings.Closer.MouseButton = _closerSettings.MouseButton;
            settings.Closer.Type = _closerSettings.Type;
            settings.Sizer = (WindowSizerType)cmbSizer.SelectedIndex;
            settings.LanguageName = cmbLanguage.SelectedValue == null ? "" : cmbLanguage.SelectedValue.ToString();

            if (!settings.Equals(_settings))
            {
                MessageBox.Show(_settings.LanguageSettings.GetValue("message_box_attention_content"), _settings.LanguageSettings.GetValue("message_box_attention_title"), MessageBoxButtons.OK);

                try
                {
                    settings.LanguageSettings = _settings.LanguageSettings;

                    var settingsFileName = Path.Combine(AssemblyUtils.AssemblyDirectory, "SmartSystemMenu.xml");
                    SmartSystemMenuSettings.Save(settingsFileName, settings);
                    if (OkClick != null)
                    {
                        OkClick.Invoke(this, new SmartSystemMenuSettingsEventArgs(settings));
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to save the settings." + Environment.NewLine + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Close();
        }

        private void ButtonCancelClick(object sender, EventArgs e)
        {
            Close();
        }

        private void KeyDownClick(object sender, KeyEventArgs e)
        {
            if (e.KeyValue == 13)
            {
                ButtonApplyClick(sender, e);
            }

            if (e.KeyValue == 27)
            {
                Close();
            }
        }

        private void ShowHotkeysForm(DataGridViewRow row)
        {
            var menuItem = (Settings.MenuItem)row.Tag;
            var form = new HotkeysForm(_settings.LanguageSettings, menuItem);
            var result = form.ShowDialog(this);
            if (result == DialogResult.OK)
            {
                menuItem.Key1 = form.MenuItem.Key1;
                menuItem.Key2 = form.MenuItem.Key2;
                menuItem.Key3 = form.MenuItem.Key3;
                row.Cells[1].Value = menuItem.ToString();
                row.Tag = menuItem;
            }
        }

        private void FillGridViewRowHotkey(DataGridView gridView, SmartSystemMenuSettings settings, string itemName, string title = null, bool isPadding = false)
        {
            var index = gridView.Rows.Add();
            var row = gridView.Rows[index];
            var menuItem = settings.MenuItems.Items.FirstOrDefault(x => x.Name == itemName);
            title = title != null ? title : settings.LanguageSettings.GetValue(itemName);
            row.Tag = (Settings.MenuItem)menuItem.Clone();
            row.Cells[0].Value = title;
            row.Cells[1].Value = menuItem == null ? "" : menuItem.ToString();
            ((DataGridViewCheckBoxCell)row.Cells[2]).Value = menuItem.Show;
            ((DataGridViewCheckBoxCell)row.Cells[2]).ToolTipText = settings.LanguageSettings.GetValue("clm_hotkeys_show_tooltip");
            if (isPadding)
            {
                var padding = row.Cells[0].Style.Padding;
                row.Cells[0].Style.Padding = new Padding(20, padding.Top, padding.Right, padding.Bottom);
            }
        }

        private void FillGridViewGroupHotkey(DataGridView gridView, SmartSystemMenuSettings settings, string itemName)
        {
            var index = gridView.Rows.Add();
            var row = gridView.Rows[index];
            var menuItem = settings.MenuItems.Items.FirstOrDefault(x => x.Name == itemName);
            row.Tag = (Settings.MenuItem)menuItem.Clone();
            row.Cells[0].Value = settings.LanguageSettings.GetValue(itemName);
            row.ReadOnly = true;
            ((DataGridViewCheckBoxCell)row.Cells[2]).Value = menuItem.Show;
            ((DataGridViewCheckBoxCell)row.Cells[2]).ToolTipText = settings.LanguageSettings.GetValue("clm_hotkeys_show_tooltip");
            ((DataGridViewDisableButtonCell)row.Cells[3]).Enabled = false;
        }
    }

    public class SmartSystemMenuSettingsEventArgs : EventArgs
    {
        public SmartSystemMenuSettings Settings { get; private set; }

        public SmartSystemMenuSettingsEventArgs(SmartSystemMenuSettings settings)
        {
            Settings = settings;
        }
    }
}