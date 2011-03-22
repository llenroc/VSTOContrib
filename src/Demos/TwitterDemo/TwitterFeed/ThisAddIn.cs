﻿using System;
using System.Windows;
using Microsoft.Office.Core;
using Office.Contrib;
using Office.Contrib.RibbonFactory;
using Office.Contrib.RibbonFactory.Interfaces;
using Office.Outlook.Contrib.RibbonFactory;
using TwitterFeedCore;

namespace TwitterFeed
{
    public partial class ThisAddIn
    {
        private AddinBootstrapper _core;

        private static void ThisAddInStartup(object sender, EventArgs e)
        {
            if (System.Windows.Application.Current == null)
                new Application {ShutdownMode = ShutdownMode.OnExplicitShutdown};

            //Check for updates
            new VstoClickOnceUpdater()
                .CheckForUpdateAsync(
                    r =>
                        {
                            if (r.Updated)
                            {
                                MessageBox.Show("Twitter feed add-in updated");
                            }
                        });
        }

        protected override IRibbonExtensibility CreateRibbonExtensibilityObject()
        {
            return new OutlookRibbonFactory(typeof(AddinBootstrapper).Assembly);
        }

        private void ThisAddInShutdown(object sender, EventArgs e)
        {
            _core.Dispose();
            System.Windows.Application.Current.Shutdown();
        }

        private void InternalStartup()
        {
            _core = new AddinBootstrapper();
            OutlookRibbonFactory.SetApplication(Application);
            RibbonFactory.Current.InitialiseFactory(
                t => (IRibbonViewModel)_core.Resolve(t),
                CustomTaskPanes);

            Startup += ThisAddInStartup;
            Shutdown += ThisAddInShutdown;
        }
    }
}
