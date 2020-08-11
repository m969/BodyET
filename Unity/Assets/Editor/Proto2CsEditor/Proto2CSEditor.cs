using System.Diagnostics;
using ETModel;
using UnityEditor;
using System.IO;
using System.Text;

namespace ETEditor
{
	internal class OpcodeInfo
	{
		public string Name;
		public int Opcode;
	}

	public class Proto2CSEditor: EditorWindow
	{
		[MenuItem("Tools/Proto2CS")]
		public static void AllProto2CS()
		{
			Process process = ProcessHelper.Run("dotnet", "Proto2CS.dll", "../Proto/", true);
			Log.Info(process.StandardOutput.ReadToEnd());
			AssetDatabase.Refresh();
			GenerateHandlersHelperBase();
		}

		private static void GenerateHandlersHelperBase()
        {
			StringBuilder sb = new StringBuilder();
			sb.AppendLine($"using System;");
			sb.AppendLine($"using ETModel;");
			sb.AppendLine($"namespace ETHotfix");
			sb.AppendLine("{");
			sb.AppendLine($"\tpublic class HandlersHelperBase");
			sb.AppendLine("\t{");

			var messages = MapCallHelperObject.ParseMessages();
			foreach (MessageClass messageClass in messages)
			{
				if (messageClass.MessageType == ETMessageType.IMessage
					|| messageClass.MessageType == ETMessageType.IRequest
					|| messageClass.MessageType == ETMessageType.IActorLocationMessage
					|| messageClass.MessageType == ETMessageType.IActorLocationRequest
                    )
                {
					var responseClass = $"MessageResponse";
					if (messageClass.ClassName.Contains("_"))
					{
						var arr = messageClass.ClassName.Split('_');
						var rev = Reverse1(arr[0]);
						responseClass = $"{rev}_{arr[1]}";
						var result = messages.Find(x => x.ClassName == responseClass);
						if (result == null)
                        {
							responseClass = "MessageResponse";
						}
					}
					sb.AppendLine($"\t\t public virtual async ETTask {messageClass.ClassName}Handler(Scene scene, {messageClass.ClassName} request, {responseClass} response, Action reply){{}}");
				}
			}
			sb.AppendLine("\t}");

			//foreach (MessageClass messageClass in messages)
			//{
			//	if (messageClass.MessageType == ETMessageType.IMessage
			//		|| messageClass.MessageType == ETMessageType.IRequest
			//		|| messageClass.MessageType == ETMessageType.IActorLocationMessage
			//		|| messageClass.MessageType == ETMessageType.IActorLocationRequest
			//		)
			//	{
			//		var responseClass = $"MessageResponse";
			//		if (messageClass.ClassName.Contains("_"))
			//		{
			//			var arr = messageClass.ClassName.Split('_');
			//			var rev = Reverse1(arr[0]);
			//			responseClass = $"{rev}_{arr[1]}";
			//			var result = messages.Find(x => x.ClassName == responseClass);
			//			if (result == null)
			//			{
			//				responseClass = "MessageResponse";
			//			}
			//		}
			//		sb.AppendLine($"\tpublic class {messageClass.ClassName}Handler : AMRpcHandler<{messageClass.ClassName}, {responseClass}>");
			//		sb.AppendLine("\t{");
			//		sb.AppendLine($"\t\t public async ETTask {messageClass.ClassName}Handler(Scene scene, {messageClass.ClassName} request, {responseClass} response, Action reply){{}}");
			//		sb.AppendLine("\t}");

			//	}
			//}

			sb.AppendLine("}");

			string csPath = Path.Combine("../Server/Hotfix/Module/Demo/", "HandlersHelperBase.generate.cs");
			File.WriteAllText(csPath, sb.ToString());
		}

		static string Reverse1(string original)
		{
			char[] arr = original.ToCharArray();
			System.Array.Reverse(arr);
			return new string(arr);
		}
	}
}
