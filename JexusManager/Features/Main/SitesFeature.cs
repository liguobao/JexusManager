﻿// Copyright (c) Lex Li. All rights reserved.
// 
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

/*
 * Created by SharpDevelop.
 * User: lextm
 * Time: 11:06 AM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */

namespace JexusManager.Features.Main
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using System.Windows.Forms;

    using JexusManager.Dialogs;
    using JexusManager.Main.Properties;
    using JexusManager.Services;

    using Microsoft.Web.Administration;
    using Microsoft.Web.Management.Client;
    using Microsoft.Web.Management.Client.Win32;

    using Binding = Microsoft.Web.Administration.Binding;
    using Module = Microsoft.Web.Management.Client.Module;

    /// <summary>
    /// Description of DefaultDocumentFeature.
    /// </summary>
    internal class SitesFeature
    {
        private sealed class FeatureTaskList : DefaultTaskList
        {
            private readonly SitesFeature _owner;

            public FeatureTaskList(SitesFeature owner)
            {
                _owner = owner;
            }

            public override ICollection GetTaskItems()
            {
                var result = new ArrayList();
                result.Add(
                    new MethodTaskItem("Add", "Add Website...", string.Empty, string.Empty, Resources.site_new_16)
                        .SetUsage());
                result.Add(new MethodTaskItem("Set", "Set Website Defaults", string.Empty).SetUsage());
                if (_owner.SelectedItem != null)
                {
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    result.Add(new TextTaskItem("Edit Site", string.Empty, true));
                    result.Add(new MethodTaskItem("Bindings", "Bindings...", string.Empty).SetUsage());
                    result.Add(
                        new MethodTaskItem("Basic", "Basic Settings...", string.Empty, string.Empty,
                            Resources.basic_settings_16).SetUsage());
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    result.Add(
                        new MethodTaskItem("Explore", "Explore", string.Empty, string.Empty, Resources.explore_16)
                            .SetUsage());
                    result.Add(new MethodTaskItem("Permissions", "Edit Permissions...", string.Empty).SetUsage());
                    result.Add(RemoveTaskItem);
                    result.Add(new MethodTaskItem("Rename", "Rename", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem("Applications", "View Applications", string.Empty).SetUsage());
                    result.Add(
                        new MethodTaskItem("VirtualDirectories", "View Virtual Directories", string.Empty).SetUsage());
                    result.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    var manageGroup = new GroupTaskItem(string.Empty, "Manage Website", string.Empty, true);
                    result.Add(manageGroup);
                    manageGroup.Items.Add(
                        new MethodTaskItem("Restart", "Restart", string.Empty, string.Empty, Resources.restart_16)
                            .SetUsage(!_owner.IsBusy));
                    manageGroup.Items.Add(
                        new MethodTaskItem("Start", "Start", string.Empty, string.Empty, Resources.start_16).SetUsage(
                            !_owner.IsBusy && _owner.SelectedItem.State != ObjectState.Started));
                    manageGroup.Items.Add(new MethodTaskItem("Stop", "Stop", string.Empty, string.Empty,
                        Resources.stop_16)
                        .SetUsage(
                            !_owner.IsBusy && _owner.SelectedItem.State == ObjectState.Started));
                    manageGroup.Items.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    manageGroup.Items.Add(new TextTaskItem("Browse Website", string.Empty, true));
                    foreach (Binding binding in _owner.SelectedItem.Bindings)
                    {
                        var uri = binding.ToUri();
                        manageGroup.Items.Add(
                            new MethodTaskItem("Browse", string.Format("Browse {0}", uri), string.Empty, string.Empty,
                                Resources.browse_16, uri).SetUsage());
                    }

                    manageGroup.Items.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    manageGroup.Items.Add(
                        new MethodTaskItem("Advanced", "Advanced Settings...", string.Empty).SetUsage());
                    manageGroup.Items.Add(new MethodTaskItem(string.Empty, "-", string.Empty).SetUsage());
                    manageGroup.Items.Add(new TextTaskItem("Configure", string.Empty, true));
                    manageGroup.Items.Add(
                        new MethodTaskItem("Tracing", "Failed Request Tracing...", string.Empty).SetUsage());
                    manageGroup.Items.Add(new MethodTaskItem("Limits", "Limits...", string.Empty).SetUsage());
                }

                return result.ToArray(typeof(TaskItem)) as TaskItem[];
            }

            [Obfuscation(Exclude = true)]
            public void Add()
            {
                _owner.Add();
            }

            [Obfuscation(Exclude = true)]
            public void Set()
            {
                _owner.Set();
            }

            [Obfuscation(Exclude = true)]
            public override void Remove()
            {
                _owner.Remove();
            }

            [Obfuscation(Exclude = true)]
            public void Rename()
            {
                _owner.Rename();
            }

            [Obfuscation(Exclude = true)]
            public void Explore()
            {
                _owner.Explore();
            }

            [Obfuscation(Exclude = true)]
            public void Permissions()
            {
                _owner.Permissions();
            }

            [Obfuscation(Exclude = true)]
            public void Bindings()
            {
                _owner.Bindings();
            }

            [Obfuscation(Exclude = true)]
            public void Basic()
            {
                _owner.Basic();
            }

            [Obfuscation(Exclude = true)]
            public void Applications()
            {
                _owner.Applications();
            }

            [Obfuscation(Exclude = true)]
            public void VirtualDirectories()
            {
                _owner.VirtualDirectories();
            }

            [Obfuscation(Exclude = true)]
            public void Restart()
            {
                _owner.Restart();
            }

            [Obfuscation(Exclude = true)]
            public void Start()
            {
                _owner.Start();
            }

            [Obfuscation(Exclude = true)]
            public void Stop()
            {
                _owner.Stop();
            }

            [Obfuscation(Exclude = true)]
            public void Browse(object uri)
            {
                _owner.Browse(uri);
            }

            [Obfuscation(Exclude = true)]
            public void Advanced()
            {
                _owner.Advanced();
            }

            [Obfuscation(Exclude = true)]
            public void Tracing()
            {
                _owner.Tracing();
            }

            [Obfuscation(Exclude = true)]
            public void Limits()
            {
                _owner.Limits();
            }
        }

        public SitesFeature(Module module)
        {
            Module = module;
        }

        protected static readonly Version FxVersion10 = new Version("1.0");
        protected static readonly Version FxVersion11 = new Version("1.1");
        protected static readonly Version FxVersion20 = new Version("2.0");
        protected static readonly Version FxVersionNotRequired = new Version();
        private FeatureTaskList _taskList;

        protected void DisplayErrorMessage(Exception ex, ResourceManager resourceManager)
        {
            var service = (IManagementUIService)GetService(typeof(IManagementUIService));
            service.ShowError(ex, resourceManager.GetString("General"), "", false);
        }

        protected object GetService(Type type)
        {
            return (Module as IServiceProvider).GetService(type);
        }

        public TaskList GetTaskList()
        {
            return _taskList ?? (_taskList = new FeatureTaskList(this));
        }

        public void Load()
        {
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            Items = service.Server.Sites;
            OnSitesSettingsSaved();
        }

        public SiteCollection Items { get; set; }
        public Site SelectedItem
        {
            get;
            set;
        }

        public List<Site> SelectedItems { get; set; }

        protected void OnSitesSettingsSaved()
        {
            SitesSettingsUpdated?.Invoke();
        }

        public virtual bool ShowHelp()
        {
            Process.Start("http://go.microsoft.com/fwlink/?LinkId=210531");
            return false;
        }

        private void Add()
        {
            var dialog = new NewSiteDialog(Module, Items);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            Items.Add(dialog.NewSite);
            dialog.NewSite.Applications[0].Save();
            SelectedItem = dialog.NewSite;
            SelectedItem.Server.CommitChanges();
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            ((MainForm)service.Form).AddSiteNode(dialog.NewSite);
        }

        private void Set()
        {
        }

        internal void Remove()
        {
            // Change : modify remove single site to remove multiple all selected sites
            if (SelectedItems == null)
            {
                return;
            }

            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            var result = dialog.ShowMessage("Are you sure that you want to remove the selected site?", "Confirm Remove",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                return;
            }

            var index = Items.IndexOf(SelectedItem);
            foreach (var selectedItem in SelectedItems)
            {
                Items.Remove(selectedItem);
            }
            SelectedItem.Server.CommitChanges();
            var service = (IConfigurationService)GetService(typeof(IConfigurationService));
            ((MainForm)service.Form).RemoveSiteNode(SelectedItem);
            if (Items.Count == 0)
            {
                SelectedItem = null;
            }
            else
            {
                SelectedItem = index > Items.Count - 1 ? Items[Items.Count - 1] : Items[index];
            }

            OnSitesSettingsSaved();
        }

        private void Rename()
        {
            // TODO: use service to find node and trigger rename.
        }

        private void Limits()
        {
        }

        private void Tracing()
        {
        }

        private void Advanced()
        {
        }

        private void Browse(object uri)
        {
            // IMPORTANT: help users launch IIS Express instance.
            var site = SelectedItem;
            if (site.Server.Mode == WorkingMode.IisExpress && site.State != ObjectState.Started)
            {
                var message = (IManagementUIService)GetService(typeof(IManagementUIService));
                var result = message.ShowMessage(
                    "This website is not yet running. Do you want to start it now?",
                    "Question",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);
                if (result != DialogResult.Yes)
                {
                    return;
                }

                Start();
            }

            Process.Start(uri.ToString());
        }

        private async void Stop()
        {
            if (SelectedItem == null)
            {
                return;
            }

            if (SelectedItem.Bindings.RequireElevation() && !JexusManager.NativeMethods.IsProcessElevated)
            {
                var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
                dialog.ShowMessage("This site cannot be stopped. Please run Jexus Manager as administrator.", Name);
                return;
            }

            IsBusy = true;
            OnSitesSettingsSaved();
            await SelectedItem.StopAsync();
            IsBusy = false;
            OnSitesSettingsSaved();
        }

        private async void Start()
        {
            if (SelectedItem == null)
            {
                return;
            }

            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (SelectedItem.Bindings.RequireElevation() && !JexusManager.NativeMethods.IsProcessElevated)
            {
                dialog.ShowMessage("This site cannot be started. Please run Jexus Manager as administrator.", Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            IsBusy = true;
            OnSitesSettingsSaved();
            try
            {
                await SelectedItem.StartAsync();
            }
            catch (Exception ex)
            {
                dialog.ShowMessage(ex.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            IsBusy = false;
            OnSitesSettingsSaved();
        }

        private async void Restart()
        {
            if (SelectedItem == null)
            {
                return;
            }

            var dialog = (IManagementUIService)GetService(typeof(IManagementUIService));
            if (SelectedItem.Bindings.RequireElevation() && !JexusManager.NativeMethods.IsProcessElevated)
            {
                dialog.ShowMessage("This site cannot be restarted. Please run Jexus Manager as administrator.", Name);
                return;
            }

            IsBusy = true;
            OnSitesSettingsSaved();
            try
            {
                await SelectedItem.RestartAsync();
            }
            catch (Exception ex)
            {
                dialog.ShowMessage(ex.Message, Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            IsBusy = false;
            OnSitesSettingsSaved();
        }

        private void VirtualDirectories()
        {
        }

        private void Applications()
        {
        }

        private void Basic()
        {
            if (SelectedItem == null)
            {
                return;
            }

            var dialog = new EditSiteDialog(Module, SelectedItem.Applications[0]);
            if (dialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            SelectedItem.Applications[0].Save();
            OnSitesSettingsSaved();
        }

        private void Permissions()
        {
            NativeMethods.ShowFileProperties(SelectedItem.Applications[0].VirtualDirectories[0].PhysicalPath.ExpandIisExpressEnvironmentVariables());
        }

        private void Explore()
        {
            Process.Start(SelectedItem.Applications[0].VirtualDirectories[0].PhysicalPath.ExpandIisExpressEnvironmentVariables());
        }

        private void Bindings()
        {
            if (SelectedItem == null)
            {
                return;
            }

            var dialog = new BindingsDialog(Module, SelectedItem);
            dialog.ShowDialog();
            OnSitesSettingsSaved();
        }

        public bool IsBusy { get; set; }

        public SitesSettingsSavedEventHandler SitesSettingsUpdated { get; set; }
        public string Description { get; }

        public virtual bool IsFeatureEnabled
        {
            get { return true; }
        }

        public virtual Version MinimumFrameworkVersion
        {
            get { return FxVersionNotRequired; }
        }

        public Module Module { get; }
        public string Name { get; }
    }

    public delegate void SitesSettingsSavedEventHandler();
}
