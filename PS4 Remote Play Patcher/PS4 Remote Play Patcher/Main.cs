// 
// This file is licensed under the terms of the Simple Non Code License (SNCL) 2.1.0.
// The full license text can be found in the file named License.txt.
// Written originally by Alexandre Quoniou in 2018.
//

using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using BsDiff;
using ICSharpCode.SharpZipLib.Zip;
using Microsoft.Win32;

namespace Dash.RemotePlayPatcher
{
    public partial class Main : Form
    {
        public string PatchesLocation { get; } = "https://github.com/MysteryDash/Offline-PS4-Remote-Play/raw/master/Patches.zip";
        private byte[] _patch;
        private string _remotePlayPath;
        
        public Main()
        {
            InitializeComponent();
        }

        private async void Main_Load(object sender, System.EventArgs e)
        {
            var uiContext = SynchronizationContext.Current;
            
            uiContext.Post(UpdateCurrentOperation, "Searching for RemotePlay.exe...");
            _remotePlayPath = FindRemotePlay();
            if (string.IsNullOrEmpty(_remotePlayPath))
            {
                if (MessageBox.Show("The PS4 Remote Play couldn't be found. Please select the file that needs to be patched.",
                        "Not found",
                        MessageBoxButtons.OKCancel,
                        MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    Environment.Exit(0);
                }

                using (var dialog = new OpenFileDialog())
                {
                    dialog.Title = "Select the RemotePlay.exe you want to patch";
                    dialog.Filter = "RemotePlay.exe|RemotePlay.exe";

                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        Environment.Exit(0);
                    }

                    _remotePlayPath = dialog.FileName;
                }
            }

            // For some reason the FileVersion FileVersionInfo provides isn't exactly the one we're looking for.
            // Sometimes padding is added to one of the parts.
            var version = FileVersionInfo.GetVersionInfo(_remotePlayPath);
            var fileVersion = $"{version.FileMajorPart}.{version.FileMinorPart}.{version.FileBuildPart}.{version.FilePrivatePart}";

            await Task.Run(() =>
            {
                try
                {
                    new RetryPolicy()
                        .MaxAttempts(int.MaxValue)
                        .Abort.IfException<Exception>(ex => MessageBox.Show($"An error occured while downloading the latest patches.\n{ex}", "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error) == DialogResult.Cancel)
                        .Try(() =>
                        {
                            using (var cryptoProvider = new MD5CryptoServiceProvider())
                            using (var client = new WebClient())
                            {
                                uiContext.Post(UpdateCurrentOperation, "Computing RemotePlay.exe's hash...");
                                var hash = string.Join(string.Empty, cryptoProvider.ComputeHash(File.ReadAllBytes(_remotePlayPath)).Select(b => b.ToString("X2")));

                                uiContext.Post(UpdateCurrentOperation, "Downloading latest patches from GitHub...");
                                using (var zipStream = new ZipInputStream(client.OpenRead(PatchesLocation)))
                                {
                                    var zipEntry = zipStream.GetNextEntry();
                                    while (zipEntry != null)
                                    {
                                        var patchName = zipEntry.Name;

                                        if (patchName.StartsWith(fileVersion, StringComparison.InvariantCultureIgnoreCase) &&
                                            patchName.EndsWith(hash, StringComparison.InvariantCultureIgnoreCase))
                                        {
                                            using (var memoryStream = new MemoryStream())
                                            {
                                                zipStream.CopyTo(memoryStream);
                                                _patch = memoryStream.ToArray();
                                            }

                                            uiContext.Post(UpdateCurrentOperation, "Ready !");
                                            uiContext.Post(EnablePatchButton, true);
                                            return;
                                        }

                                        zipEntry = zipStream.GetNextEntry();
                                    }

                                    throw new Exception("Patch not found or RemotePlay.exe is already patched !");
                                }
                            }
                        });
                }
                catch (AggregateException)
                {
                    Environment.Exit(0);
                }
            });
        }

        private void UpdateCurrentOperation(object text)
        {
            currentOperationLabel.Text = (string)text;
        }

        private void EnablePatchButton(object enabled)
        {
            patchButton.Enabled = (bool)enabled;
        }

        private async void patchButton_Click(object sender, System.EventArgs e)
        {
            var uiContext = SynchronizationContext.Current;

            var createBackup = MessageBox.Show("Do you wish to create a backup of your original RemotePlay.exe ?", "Create backup ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;

            await Task.Run(() =>
            {
                uiContext.Post(UpdateCurrentOperation, "Patching...");
                uiContext.Post(EnablePatchButton, false);

                var originalRemotePlay = File.ReadAllBytes(_remotePlayPath);
                if (createBackup)
                {
                    File.Move(_remotePlayPath, Path.ChangeExtension(_remotePlayPath, ".exe.bak"));
                }
                File.Delete(_remotePlayPath);

                using (var originalRemotePlayStream = new MemoryStream(originalRemotePlay))
                using (var outputStream = File.OpenWrite(_remotePlayPath))
                {
                    BinaryPatchUtility.Apply(originalRemotePlayStream, () => new MemoryStream(_patch), outputStream);
                }

                uiContext.Post(UpdateCurrentOperation, "Done !");
            });
        }

        private string FindRemotePlay()
        {
            var baseKey = Environment.Is64BitOperatingSystem ? 
                @"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\" : 
                @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
            
            using (var keys = Registry.LocalMachine.OpenSubKey(baseKey))
            {
                var remotePlayKey = keys?.GetSubKeyNames()
                    .Select(name => keys.OpenSubKey(name))
                    .FirstOrDefault(key => key.GetValue("DisplayName", "").ToString().Contains("PS4") &&
                                           key.GetValue("Publisher", "").ToString().Contains("Sony"));
                var path = Path.Combine(remotePlayKey?.GetValue("InstallLocation", null)?.ToString() ?? string.Empty, "RemotePlay.exe");
                return File.Exists(path) ? path : null;
            }
        }
    }
}
