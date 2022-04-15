using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using Microsoft.Xna.Framework.Content.Pipeline.Tasks;

namespace TModLoaderHelper
{
	// Token: 0x02000002 RID: 2
	public class BuildEngine : IBuildEngine
	{
		// Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
		public BuildEngine()
		{
			this.errors = new List<string>();
			this.log = true;
		}

		// Token: 0x06000002 RID: 2 RVA: 0x00002078 File Offset: 0x00000278
		public BuildEngine(string logFile)
		{
			this.logFile = logFile;
			this.log = true;
			try
			{
				this.logWriter = new StreamWriter(logFile, true);
			}
			catch
			{
				this.log = false;
			}
		}

		// Token: 0x06000003 RID: 3 RVA: 0x000020D4 File Offset: 0x000002D4
		public void Begin()
		{
			bool flag = this.log;
			if (flag)
			{
				this.errors = new List<string>();
			}
		}

		// Token: 0x06000004 RID: 4 RVA: 0x000020FC File Offset: 0x000002FC
		private void Log(string message)
		{
			bool flag = this.log;
			if (flag)
			{
				try
				{
					this.logWriter.WriteLine(message);
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000005 RID: 5 RVA: 0x0000213C File Offset: 0x0000033C
		public void End()
		{
			bool flag = this.log;
			if (flag)
			{
				try
				{
					this.logWriter.Flush();
					this.logWriter.Close();
				}
				catch
				{
				}
			}
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002188 File Offset: 0x00000388
		public List<string> GetErrors()
		{
			return this.errors;
		}

		// Token: 0x06000007 RID: 7 RVA: 0x000021A0 File Offset: 0x000003A0
		public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
		{
			return true;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000008 RID: 8 RVA: 0x000021B4 File Offset: 0x000003B4
		public int ColumnNumberOfTaskNode
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000009 RID: 9 RVA: 0x000021C8 File Offset: 0x000003C8
		public bool ContinueOnError
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000003 RID: 3
		// (get) Token: 0x0600000A RID: 10 RVA: 0x000021DC File Offset: 0x000003DC
		public int LineNumberOfTaskNode
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021F0 File Offset: 0x000003F0
		public void LogCustomEvent(CustomBuildEventArgs e)
		{
			bool flag = this.log;
			if (flag)
			{
				this.Log("Custom Event at " + DateTime.Now.ToString() + ": " + e.Message);
			}
		}

		// Token: 0x0600000C RID: 12 RVA: 0x00002234 File Offset: 0x00000434
		public void LogErrorEvent(BuildErrorEventArgs e)
		{
			bool flag = this.log;
			if (flag)
			{
				this.Log("Error at " + DateTime.Now.ToString() + ": " + e.Message);
			}
			this.errors.Add(DateTime.Now.ToString() + ": " + e.Message);
		}

		// Token: 0x0600000D RID: 13 RVA: 0x000022A0 File Offset: 0x000004A0
		public void LogMessageEvent(BuildMessageEventArgs e)
		{
			bool flag = this.log;
			if (flag)
			{
				this.Log("Message at " + DateTime.Now.ToString() + ": " + e.Message);
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x000022E4 File Offset: 0x000004E4
		public void LogWarningEvent(BuildWarningEventArgs e)
		{
			bool flag = this.log;
			if (flag)
			{
				this.Log("Warning at " + DateTime.Now.ToString() + ": " + e.Message);
			}
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x0600000F RID: 15 RVA: 0x00002328 File Offset: 0x00000528
		public string ProjectFileOfTaskNode
		{
			get
			{
				return string.Empty;
			}
		}

		// Token: 0x04000001 RID: 1
		private string logFile = "";

		// Token: 0x04000002 RID: 2
		private StreamWriter logWriter;

		// Token: 0x04000003 RID: 3
		private List<string> errors;

		// Token: 0x04000004 RID: 4
		public bool log;
	}

	public class XNBBuilder : BuildContent
	{
		// Token: 0x06000012 RID: 18 RVA: 0x00002404 File Offset: 0x00000604
		public XNBBuilder()
		{
			base.TargetPlatform = "Windows";
			base.TargetProfile = "Reach";
			base.CompressContent = false;
			base.BuildConfiguration = "Debug";
			this.BuildAudioAsSongs = false;
			this.BuildAudioAsSoundEffects = false;
			this.buildEngine = new BuildEngine();
		}

		// Token: 0x06000013 RID: 19 RVA: 0x0000245E File Offset: 0x0000065E
		public XNBBuilder(bool CompressContent)
		{
			base.CompressContent = CompressContent;
			this.BuildAudioAsSongs = false;
			this.BuildAudioAsSoundEffects = false;
			this.buildEngine = new BuildEngine();
		}

		// Token: 0x06000014 RID: 20 RVA: 0x0000248C File Offset: 0x0000068C
		public XNBBuilder(string targetPlatform, string targetProfile, bool CompressContent)
		{
			base.TargetPlatform = targetPlatform;
			base.TargetProfile = targetProfile;
			base.CompressContent = CompressContent;
			base.BuildConfiguration = "Debug";
			this.BuildAudioAsSongs = false;
			this.BuildAudioAsSoundEffects = false;
			this.buildEngine = new BuildEngine();
		}

		// Token: 0x06000015 RID: 21 RVA: 0x000024E0 File Offset: 0x000006E0
		public List<string> GetErrors()
		{
			return this.buildEngine.GetErrors();
		}

		// Token: 0x06000016 RID: 22 RVA: 0x00002500 File Offset: 0x00000700
		public string[] PackageContent(string[] fileNames, string outputDirectory, bool shouldLog, string rootDirectory, out bool buildStatus)
		{
			string[] array = null;
			buildStatus = false;
			try
			{
				bool flag = !shouldLog;
				if (flag)
				{
					this.buildEngine.log = false;
				}
				else
				{
					this.buildEngine = new BuildEngine("logfile.txt");
				}
				base.OutputDirectory = outputDirectory;
				base.RootDirectory = rootDirectory;
				List<TaskItem> list = new List<TaskItem>();
				int i = 0;
				while (i < fileNames.Length)
				{
					string value = "." + fileNames[i].Split(new char[]
					{
						'.'
					}).Last<string>();
					Dictionary<string, object> dictionary = new Dictionary<string, object>();
					bool flag2 = ".bmp.dds.dib.hdr.jpg.pfm.png.ppm.tga".Contains(value);
					if (flag2)
					{
						dictionary.Add("Importer", "TextureImporter");
						dictionary.Add("Processor", "TextureProcessor");
						goto IL_3B6;
					}
					bool flag3 = ".fbx".Contains(value);
					if (flag3)
					{
						dictionary.Add("Importer", "FbxImporter");
						dictionary.Add("Processor", "ModelProcessor");
						goto IL_3B6;
					}
					bool flag4 = ".fx".Contains(value);
					if (flag4)
					{
						dictionary.Add("Importer", "EffectImporter");
						dictionary.Add("Processor", "EffectProcessor");
						goto IL_3B6;
					}
					bool flag5 = ".spritefont".Contains(value);
					if (flag5)
					{
						dictionary.Add("Importer", "FontDescriptionImporter");
						dictionary.Add("Processor", "FontDescriptionProcessor");
						goto IL_3B6;
					}
					bool flag6 = ".x".Contains(value);
					if (flag6)
					{
						dictionary.Add("Importer", "XImporter");
						dictionary.Add("Processor", "ModelProcessor");
						goto IL_3B6;
					}
					bool flag7 = ".xml".Contains(value);
					if (flag7)
					{
						dictionary.Add("Importer", "XmlImporter");
						dictionary.Add("Processor", "PassThroughProcessor");
						goto IL_3B6;
					}
					bool flag8 = ".mp3".Contains(value);
					if (flag8)
					{
						dictionary.Add("Importer", "Mp3Importer");
						bool buildAudioAsSoundEffects = this.BuildAudioAsSoundEffects;
						if (buildAudioAsSoundEffects)
						{
							dictionary.Add("Processor", "SoundEffectProcessor");
						}
						else
						{
							bool buildAudioAsSongs = this.BuildAudioAsSongs;
							if (buildAudioAsSongs)
							{
								dictionary.Add("Processor", "SongProcessor");
							}
							else
							{
								dictionary.Add("Processor", "SoundEffectProcessor");
							}
						}
						goto IL_3B6;
					}
					bool flag9 = ".wma".Contains(value);
					if (flag9)
					{
						dictionary.Add("Importer", "WmaImporter");
						bool buildAudioAsSoundEffects2 = this.BuildAudioAsSoundEffects;
						if (buildAudioAsSoundEffects2)
						{
							dictionary.Add("Processor", "SoundEffectProcessor");
						}
						else
						{
							bool buildAudioAsSongs2 = this.BuildAudioAsSongs;
							if (buildAudioAsSongs2)
							{
								dictionary.Add("Processor", "SongProcessor");
							}
							else
							{
								dictionary.Add("Processor", "SoundEffectProcessor");
							}
						}
						goto IL_3B6;
					}
					bool flag10 = ".wav".Contains(value);
					if (flag10)
					{
						dictionary.Add("Importer", "WavImporter");
						bool buildAudioAsSoundEffects3 = this.BuildAudioAsSoundEffects;
						if (buildAudioAsSoundEffects3)
						{
							dictionary.Add("Processor", "SoundEffectProcessor");
						}
						else
						{
							bool buildAudioAsSongs3 = this.BuildAudioAsSongs;
							if (buildAudioAsSongs3)
							{
								dictionary.Add("Processor", "SongProcessor");
							}
							else
							{
								dictionary.Add("Processor", "SoundEffectProcessor");
							}
						}
						goto IL_3B6;
					}
					bool flag11 = ".wmv".Contains(value);
					if (flag11)
					{
						dictionary.Add("Importer", "WmvImporter");
						dictionary.Add("Processor", "VideoProcessor");
						goto IL_3B6;
					}
					Console.WriteLine("自动忽略" + fileNames[i] + "，不合法的文件");
				IL_3F3:
					i++;
					continue;
				IL_3B6:
					Console.WriteLine("正在编译" + fileNames[i]);
					dictionary.Add("Name", Path.GetFileNameWithoutExtension(fileNames[i]));
					list.Add(new TaskItem(fileNames[i], dictionary));
					goto IL_3F3;
				}
				ITaskItem[] array2 = list.ToArray();
				base.SourceAssets = array2;
				this.buildEngine.Begin();
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
				array2 = new TaskItem[]
				{
					new TaskItem(baseDirectory + "Microsoft.Xna.Framework.dll"),
					new TaskItem(baseDirectory + "Microsoft.Xna.Framework.Content.Pipeline.dll"),
					new TaskItem(baseDirectory + "Microsoft.Xna.Framework.Content.Pipeline.AudioImporters.dll"),
					new TaskItem(baseDirectory + "Microsoft.Xna.Framework.Content.Pipeline.EffectImporter.dll"),
					new TaskItem(baseDirectory + "Microsoft.Xna.Framework.Content.Pipeline.FBXImporter.dll"),
					new TaskItem(baseDirectory + "Microsoft.Xna.Framework.Content.Pipeline.TextureImporter.dll"),
					new TaskItem(baseDirectory + "Microsoft.Xna.Framework.Content.Pipeline.VideoImporters.dll"),
					new TaskItem(baseDirectory + "Microsoft.Xna.Framework.Content.Pipeline.XImporter.dll")
				};
				base.PipelineAssemblies = array2;
				base.BuildEngine = this.buildEngine;
				base.IntermediateDirectory = Directory.GetCurrentDirectory();
				buildStatus = this.Execute();
				bool flag12 = base.OutputContentFiles != null;
				if (flag12)
				{
					array = new string[base.OutputContentFiles.Length];
					for (int j = 0; j < array.Length; j++)
					{
						array[j] = base.OutputContentFiles[j].ToString();
					}
				}
			}
			catch
			{
			}
			finally
			{
				this.buildEngine.End();
			}
			return array;
		}

		// Token: 0x04000005 RID: 5
		public BuildEngine buildEngine;

		// Token: 0x04000006 RID: 6
		public bool BuildAudioAsSoundEffects;

		// Token: 0x04000007 RID: 7
		public bool BuildAudioAsSongs;
	}
}
