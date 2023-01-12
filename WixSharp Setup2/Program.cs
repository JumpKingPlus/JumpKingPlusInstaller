using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using WixSharp;
using WixSharp.Forms;
using File = System.IO.File;

// NuGet console: Install-Package WixSharp
// NuGet Manager UI: browse tab

namespace WixSharp_Setup
{
    class Program
    {
        static void MoveFile(string source, string destination, string destFolder)
        {
            try
            {
                if (!Directory.Exists(destFolder))
                {
                    CreateFolder(destFolder);
                }
                File.Move(source, destination);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{destination}\n{ex.Message}", "Error moving file");
            }
        }

        static void CreateFolder(string dir)
        {
            try
            {
                if (!Directory.Exists(dir))
                {
                    Directory.CreateDirectory(dir);
                }
                else
                {
                    Directory.Delete(dir, true);
                    System.Threading.Thread.Sleep(500);
                    Directory.CreateDirectory(dir);
                    System.Threading.Thread.Sleep(500);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{dir}\n{ex.Message}", "Error creating folder");
            }
        }

        static void Main()
        {
            var project = new ManagedProject("JumpKingPlus",
                             new Dir(@"%ProgramFiles%\Steam\steamapps\common\Jump King",
                                new WixSharp.File(@"Include\babe.xml"),                             /* ---------------------------- */
                                new WixSharp.File(@"Include\discordLocations.xml"),
                                new WixSharp.File(@"Include\DiscordRPC.dll"),
                                new WixSharp.File(@"Include\JumpKing.exe"),
                                new WixSharp.File(@"Include\JumpKing.exe.Config"),
                                new WixSharp.File(@"Include\JumpKingPlus.dll"),
                                new WixSharp.File(@"Include\Newtonsoft.Json.dll"),
                                new WixSharp.File(@"Include\JK_Plus_Logo.xnb"),
                                new WixSharp.File(@"Include\merchant_nbp.xml"),
                                new WixSharp.File(@"Include\merchant_quotes.xml"),
                                new WixSharp.File(@"Include\owl_mold_man.xml"),
                                new WixSharp.File(@"Include\owl_shroom_guy.xml"),
                                new WixSharp.File(@"Include\snake.xml"),
                                new WixSharp.File(@"Include\link.xnb")
                            ));                                                                     /* ---------------------------- */

            project.GUID = new Guid("1C3139B9-5520-49DC-9219-1F0B92570528");
            project.Platform = Platform.x86;
            project.Name = "JumpKingPlus";
            project.Version = new Version("1.8.2");
            project.Description = "An enhanced version of Jump King.";
            project.ControlPanelInfo.ProductIcon = "Assets\\favicon.ico";
            project.ControlPanelInfo.Manufacturer = "Phoenixx19";
            project.ControlPanelInfo.NoModify = true;
            project.BackgroundImage = "Assets\\Background.bmp";
            project.BannerImage = "Assets\\Banner.bmp";
            project.LicenceFile = "Assets\\LICENSE.rtf";

            //project.ManagedUI = ManagedUI.Empty;    //no standard UI dialogs
            project.ManagedUI = ManagedUI.Default;  //all standard UI dialogs

            //custom set of standard UI dialogs
            project.ManagedUI = new ManagedUI();

            project.ManagedUI.InstallDialogs.Add(Dialogs.Welcome)
                                            .Add(Dialogs.Licence)
                                            .Add(Dialogs.InstallDir)
                                            .Add(Dialogs.Progress)
                                            .Add(Dialogs.Exit);

            project.ManagedUI.ModifyDialogs.Add(Dialogs.MaintenanceType)
                                           .Add(Dialogs.Progress)
                                           .Add(Dialogs.Exit);

            project.Load += Msi_Load;
            project.BeforeInstall += Msi_BeforeInstall;
            project.AfterInstall += Msi_AfterInstall;

            //project.SourceBaseDir = "<input dir path>";
            //project.OutDir = "<output dir path>";

            project.InstallScope = InstallScope.perMachine;
            project.InstallPrivileges = InstallPrivileges.elevated;

            project.OutFileName = "JKPlusInstaller-v"+project.Version+"-x86";

            project.BuildMsi();
        }

        static void Msi_Load(SetupEventArgs e)
        {
        }

        static void Msi_BeforeInstall(SetupEventArgs e)
        {
            if (e.IsInstalling)
            {
                if (File.Exists(e.InstallDir + "JumpKingRPC.dll"))
                {
                    MessageBox.Show("JumpKingRPC already installed. Please uninstall JumpKingRPC first.", "Error");
                    Environment.Exit(1);
                }
                if (File.Exists(e.InstallDir + "JumpKingPlus.dll"))
                {
                    MessageBox.Show("JumpKingPlus already installed. Please uninstall JumpKingPlus first.", "Error");
                    Environment.Exit(1);
                }


                /* -------------------------- */

                /// <summary>
                /// PIRACY CHECK:
                /// 
                /// THE FILES THAT ARE GETTING CHECK BY THE "JUMPKINGPLUS PIRACY CHECK"
                /// HAVE BEEN REPLACED WITH "[REDACTED]" TO PREVENT ILLEGAL JUMPKINGPLUS
                /// INSTALLATIONS.
                /// 
                /// TLDR; CHECKS STEAM DYNAMIC LINK LIBRARIES IN THE FOLDER
                /// </summary>
                
                /* -------------------------- */


                if (File.Exists("[REDACTED]") && File.Exists("[REDACTED]") && Directory.Exists(e.InstallDir + "Content"))
                {
                    Directory.CreateDirectory(e.InstallDir + "Backup");
                    System.Threading.Thread.Sleep(500);

                    MoveFile(e.InstallDir + "JumpKing.exe", e.InstallDir + "Backup\\JumpKing.exe", e.InstallDir + "Backup");
                    MoveFile(e.InstallDir + "JumpKing.exe.config", e.InstallDir + "Backup\\JumpKing.exe.config", e.InstallDir + "Backup");

                    CreateFolder(e.InstallDir + "Content\\ControllerBinds");

                    MoveFile(e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\merchant_nbp.xml", e.InstallDir + "Backup\\merchant_nbp.xml", e.InstallDir + "Backup");
                    MoveFile(e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\merchant_quotes.xml", e.InstallDir + "Backup\\merchant_quotes.xml", e.InstallDir + "Backup");
                    MoveFile(e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\owl_mold_man.xml", e.InstallDir + "Backup\\owl_mold_man.xml", e.InstallDir + "Backup");
                    MoveFile(e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\owl_shroom_guy.xml", e.InstallDir + "Backup\\owl_shroom_guy.xml", e.InstallDir + "Backup");
                    MoveFile(e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\snake.xml", e.InstallDir + "Backup\\snake.xml", e.InstallDir + "Backup");

                    System.Threading.Thread.Sleep(500);
                } else
                {
                    MessageBox.Show("You have chosen the wrong folder."+Environment.NewLine+"Please try again and select the Jump King folder.", "Piracy check");
                    Environment.Exit(1);
                }
            } 
            else
            {
                MoveFile(e.InstallDir + "Content\\JK_Plus_Logo.xnb", e.InstallDir + "JK_Plus_Logo.xnb", e.InstallDir);
                MoveFile(e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\merchant_nbp.xml", e.InstallDir + "merchant_nbp.xml", e.InstallDir);
                MoveFile(e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\merchant_quotes.xml", e.InstallDir + "merchant_quotes.xml", e.InstallDir);
                MoveFile(e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\owl_mold_man.xml", e.InstallDir + "owl_mold_man.xml", e.InstallDir);
                MoveFile(e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\owl_shroom_guy.xml", e.InstallDir + "owl_shroom_guy.xml", e.InstallDir);
                MoveFile(e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\snake.xml", e.InstallDir + "snake.xml", e.InstallDir);
                MoveFile(e.InstallDir + "Content\\settings\\babe.xml", e.InstallDir + "babe.xml", e.InstallDir);
                MoveFile(e.InstallDir + "Content\\settings\\discordLocations.xml", e.InstallDir + "discordLocations.xml", e.InstallDir);
                MoveFile(e.InstallDir + "Content\\gui\\link.xnb", e.InstallDir + "link.xnb", e.InstallDir);

                CreateFolder(e.InstallDir + "Content\\mods");
                CreateFolder(e.InstallDir + "Content\\wardrobe");
            }
        }

        static void Msi_AfterInstall(SetupEventArgs e)
        {
            if (e.IsUninstalling)
            {
                if (Directory.Exists(e.InstallDir + "Backup"))
                {
                    MoveFile(e.InstallDir + "Backup\\JumpKing.exe", e.InstallDir + "JumpKing.exe", e.InstallDir);
                    MoveFile(e.InstallDir + "Backup\\JumpKing.exe.config", e.InstallDir + "JumpKing.exe.config", e.InstallDir);
                    MoveFile(e.InstallDir + "Backup\\merchant_nbp.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\merchant_nbp.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant");
                    MoveFile(e.InstallDir + "Backup\\merchant_quotes.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\merchant_quotes.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant");
                    MoveFile(e.InstallDir + "Backup\\owl_mold_man.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\owl_mold_man.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant");
                    MoveFile(e.InstallDir + "Backup\\owl_shroom_guy.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\owl_shroom_guy.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant");
                    MoveFile(e.InstallDir + "Backup\\snake.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\snake.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant");
                    
                    Directory.Delete(e.InstallDir + "Backup", true);
                }
            } 
            else
            {
                MoveFile(e.InstallDir + "JK_Plus_Logo.xnb", e.InstallDir + "Content\\JK_Plus_Logo.xnb", e.InstallDir + "Content");

                CreateFolder(e.InstallDir + "Content\\ControllerBinds");

                MoveFile(e.InstallDir + "merchant_nbp.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\merchant_nbp.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant");
                MoveFile(e.InstallDir + "merchant_quotes.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\merchant_quotes.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant");
                MoveFile(e.InstallDir + "owl_mold_man.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\owl_mold_man.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant");
                MoveFile(e.InstallDir + "owl_shroom_guy.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\owl_shroom_guy.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant");
                MoveFile(e.InstallDir + "snake.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant\\snake.xml", e.InstallDir + "Content\\props\\textures\\old_man\\merchant");

                MoveFile(e.InstallDir + "babe.xml", e.InstallDir + "Content\\settings\\babe.xml", e.InstallDir + "Content\\settings");
                MoveFile(e.InstallDir + "discordLocations.xml", e.InstallDir + "Content\\settings\\discordLocations.xml", e.InstallDir + "Content\\settings");

                MoveFile(e.InstallDir + "link.xnb", e.InstallDir + "Content\\gui\\link.xnb", e.InstallDir + "Content\\gui");

                if (!Directory.Exists(e.InstallDir + "Content\\mods"))
                {
                    Directory.CreateDirectory(e.InstallDir + "Content\\mods");
                }
                if (!Directory.Exists(e.InstallDir + "Content\\wardrobe"))
                {
                    Directory.CreateDirectory(e.InstallDir + "Content\\wardrobe");
                }
            }
        }
    }
}