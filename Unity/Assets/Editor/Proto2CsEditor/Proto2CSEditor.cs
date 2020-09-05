using System.Diagnostics;
using ETModel;
using UnityEditor;
using System.IO;
using System.Text;
using System.Collections.Generic;

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
            var arr = AssetDatabase.FindAssets("t:ETMessageDefineObject");
            foreach (var item in arr)
            {
                var path = AssetDatabase.GUIDToAssetPath(item);
                var obj = AssetDatabase.LoadAssetAtPath<ETMessageDefineObject>(path);
                obj.GenerateMessage();
            }
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
			sb.AppendLine("\t\tpublic static HandlersHelperBase Instance { get; set; }");

			var messages = ETMessageDefineObject.ParseMessages("HotfixMessage");
			ParseMessages(sb, messages, 1);
			sb.AppendLine("\t}");

			ParseMessages(sb, messages, 2);

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

		private static void ParseMessages(StringBuilder sb, List<MessageClass> messages, int tag)
        {
			foreach (MessageClass messageClass in messages)
			{
				if (messageClass.MessageType == ETMessageType.IMessage
					|| messageClass.MessageType == ETMessageType.IRequest
					|| messageClass.MessageType == ETMessageType.IActorMessage
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
						responseClass = responseClass.Replace("Request", "Response");
						var result = messages.Find(x => x.ClassName == responseClass);
						if (result == null)
						{
							//Log.Error($"{responseClass} is null");
							responseClass = "MessageResponse";
						}
					}
					var attr = "ActorMessageHandler";
					var baseHandler = "AMActorLocationRpcHandler";
					if (messageClass.MessageType == ETMessageType.IMessage)
					{
						attr = "MessageHandler";
						baseHandler = "AMHandler";
					}
					if (messageClass.MessageType == ETMessageType.IRequest)
					{
						attr = "MessageHandler";
						baseHandler = "AMRpcHandler";
					}
					if (messageClass.MessageType == ETMessageType.IActorMessage)
					{
						attr = "ActorMessageHandler";
						baseHandler = "AMActorHandler";
					}
					if (messageClass.MessageType == ETMessageType.IActorLocationMessage)
					{
						attr = "ActorMessageHandler";
						baseHandler = "AMActorLocationHandler";
					}
					if (messageClass.MessageType == ETMessageType.IActorLocationRequest)
					{
						attr = "ActorMessageHandler";
						baseHandler = "AMActorLocationRpcHandler";
					}
					if (tag !=1)
                    {
						sb.AppendLine($"\t[{attr}]");
					}

					if (baseHandler.Contains("Actor"))
					{
						if (baseHandler.Contains("Rpc"))
						{
							if (baseHandler.Contains("Location"))
                            {
								if (tag ==1 )
									sb.AppendLine($"\t\tpublic virtual async ETTask {messageClass.ClassName}Handler(Unit unit, {messageClass.ClassName} request, {responseClass} response, Action reply){{}}");
								else
									sb.AppendLine($"\tpublic class {messageClass.ClassName}Handler : {baseHandler}<Unit, {messageClass.ClassName}, {responseClass}>");
							}
							else
                            {
								if (tag == 1)
									sb.AppendLine($"\t\tpublic virtual async ETTask {messageClass.ClassName}Handler(Scene scene, {messageClass.ClassName} request, {responseClass} response, Action reply){{}}");
								else
									sb.AppendLine($"\tpublic class {messageClass.ClassName}Handler : {baseHandler}<Scene, {messageClass.ClassName}, {responseClass}>");
							}
						}
						else
						{
							if (baseHandler.Contains("Location"))
                            {
								if (tag == 1)
									sb.AppendLine($"\t\tpublic virtual async ETTask {messageClass.ClassName}Handler(Unit unit, {messageClass.ClassName} message){{}}");
								else
									sb.AppendLine($"\tpublic class {messageClass.ClassName}Handler : {baseHandler}<Unit, {messageClass.ClassName}>");
							}
                            else
                            {
								if (tag == 1)
									sb.AppendLine($"\t\tpublic virtual async ETTask {messageClass.ClassName}Handler(Scene scene, {messageClass.ClassName} message){{}}");
								else
									sb.AppendLine($"\tpublic class {messageClass.ClassName}Handler : {baseHandler}<Scene, {messageClass.ClassName}>");
							}
						}

						if (tag != 1)
                        {
							sb.AppendLine("\t{");
							if (baseHandler.Contains("Rpc"))
							{
								if (baseHandler.Contains("Location"))
								{
									sb.AppendLine($"\t\t protected override async ETTask Run(Unit unit, {messageClass.ClassName} request, {responseClass} response, Action reply){{");
									sb.AppendLine($"\t\tawait HandlersHelperBase.Instance.{messageClass.ClassName}Handler(unit, request, response, reply);");
									sb.AppendLine("\t}");
								}
								else
								{
									sb.AppendLine($"\t\t protected override async ETTask Run(Scene scene, {messageClass.ClassName} request, {responseClass} response, Action reply){{");
									sb.AppendLine($"\t\tawait HandlersHelperBase.Instance.{messageClass.ClassName}Handler(scene, request, response, reply);");
									sb.AppendLine("\t}");
								}
							}
							else
							{
								if (baseHandler.Contains("Location"))
								{
									sb.AppendLine($"\t\t protected override async ETTask Run(Unit unit, {messageClass.ClassName} request){{");
									sb.AppendLine($"\t\tawait HandlersHelperBase.Instance.{messageClass.ClassName}Handler(unit, request);");
									sb.AppendLine("\t}");
								}
								else
								{
									sb.AppendLine($"\t\t protected override async ETTask Run(Scene scene, {messageClass.ClassName} request){{");
									sb.AppendLine($"\t\tawait HandlersHelperBase.Instance.{messageClass.ClassName}Handler(scene, request);");
									sb.AppendLine("\t}");
								}
							}
						}
					}
					else
					{
						if (baseHandler.Contains("Rpc"))
						{
							if (tag == 1)
								sb.AppendLine($"\t\tpublic virtual async ETTask {messageClass.ClassName}Handler(Session session, {messageClass.ClassName} request, {responseClass} response, Action reply){{}}");
							else
								sb.AppendLine($"\tpublic class {messageClass.ClassName}Handler : {baseHandler}<{messageClass.ClassName}, {responseClass}>");
						}
						else
						{
							if (tag == 1)
								sb.AppendLine($"\t\tpublic virtual async ETTask {messageClass.ClassName}Handler(Session session, {messageClass.ClassName} request){{}}");
							else
								sb.AppendLine($"\tpublic class {messageClass.ClassName}Handler : {baseHandler}<{messageClass.ClassName}>");
						}
						if (tag != 1)
                        {
							sb.AppendLine("\t{");
							if (baseHandler.Contains("Rpc"))
							{
								sb.AppendLine($"\t\t protected override async ETTask Run(Session session, {messageClass.ClassName} request, {responseClass} response, Action reply){{");
								sb.AppendLine($"\t\tawait HandlersHelperBase.Instance.{messageClass.ClassName}Handler(session, request, response, reply);");
								sb.AppendLine("\t}");
							}
							else
							{
								sb.AppendLine($"\t\t protected override async ETTask Run(Session session, {messageClass.ClassName} request){{");
								sb.AppendLine($"\t\tawait HandlersHelperBase.Instance.{messageClass.ClassName}Handler(session, request);");
								sb.AppendLine("\t}");
							}
						}
					}
					if (tag != 1)
                    {
						sb.AppendLine("\t}");
					}
				}
			}
		}
	}
}
