// 
// This file is licensed under the terms of the Simple Non Code License (SNCL) 2.3.0.
// The full license text can be found in the file named License.txt.
// Written originally by Alexandre Quoniou in 2019.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace Dash.RemotePlayPatcher
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void patchButton_Click(object sender, EventArgs e)
        {
            var remotePlayLocation = FindRemotePlay();
            if (string.IsNullOrEmpty(remotePlayLocation))
            {
                if (MessageBox.Show("The PS4 Remote Play couldn't be found. Please select the file that needs to be patched.",
                                    "Not found",
                                    MessageBoxButtons.OKCancel,
                                    MessageBoxIcon.Exclamation) == DialogResult.Cancel)
                {
                    return;
                }

                using (var dialog = new OpenFileDialog())
                {
                    dialog.Title = "Select the RemotePlay.exe you want to patch";
                    dialog.Filter = "RemotePlay.exe|RemotePlay.exe";

                    if (dialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    remotePlayLocation = dialog.FileName;
                }
            }

            if (MessageBox.Show("Do you wish to create a backup of your original file?", "Create backup?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                File.Copy(remotePlayLocation, Path.ChangeExtension(remotePlayLocation, ".exe.bak"), true);
            }
            
            var module = ModuleDefinition.ReadModule(remotePlayLocation, new ReaderParameters() { ReadWrite = true });
            var main = module.EntryPoint;
            var il = main.Body.GetILProcessor();

            var checkUpdateVariable = main
                .Body
                .Variables
                .FirstOrDefault(v => v.VariableType.Name.Equals("CheckUpdate", StringComparison.InvariantCulture));
            if (checkUpdateVariable == null)
            {
                MessageBox.Show("CheckUpdate couldn't be found in the program's entry point, please ensure that you are trying to patch the RemotePlay.",
                                "CheckUpdate not found.", 
                                MessageBoxButtons.OK, 
                                MessageBoxIcon.Error);
                return;
            }

            var callvirts = main
                .Body
                .Instructions
                .SelectWithPrevious((prev, curr) => new { prev, curr })
                .Where(instrs => instrs.prev.OpCode == OpCodes.Ldloc_S &&
                                 instrs.prev.Operand is VariableDefinition def &&
                                 def.Index == checkUpdateVariable.Index &&
                                 instrs.curr.OpCode == OpCodes.Callvirt)
                .ToList();

            var showDialogCall = callvirts.FirstOrDefault(instrs => instrs.curr.Operand is MethodReference def && def.Name == "ShowDialog");
            if (showDialogCall == null)
            {
                MessageBox.Show("The ShowDialog expected call could not be found. You might have already patched the RemotePlay.",
                                "Probably already patched.",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                return;
            }

            il.Remove(showDialogCall.prev);
            il.Remove(showDialogCall.curr.Next);
            il.Remove(showDialogCall.curr);
            module.Write();

            MessageBox.Show("The RemotePlay was patched successfully!", "Success!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Close();
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

    public static class LinqExtensions
    {
        public static IEnumerable<TResult> SelectWithPrevious<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TSource, TResult> projection)
        {
            using (var iterator = source.GetEnumerator())
            {
                if (!iterator.MoveNext())
                {
                    yield break;
                }
                var previous = iterator.Current;
                while (iterator.MoveNext())
                {
                    yield return projection(previous, iterator.Current);
                    previous = iterator.Current;
                }
            }
        }
    }
}
