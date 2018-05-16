using Sockets.Chat.Model.Servers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Sockets.Chat.Model
{
    public class MessageHandlersService
    {
        #region Members

        private object mTarget;
        private Dictionary<MessageCode, List<MethodInfo>> mMessageHandlers
           = new Dictionary<MessageCode, List<MethodInfo>>();

        #endregion

        public MessageHandlersService(object target)
        {
            mTarget = target;
            InitializeMessageHandlers(target);
        }

        public void Invoke(ChatMessage message)
        {
            foreach (var method in mMessageHandlers[message.Code])
                method.Invoke(mTarget, new object[] { message });
        }

        private void InitializeMessageHandlers(object target)
        {
            var messageHandlers = target.GetType().GetMethods(BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(method => method.GetCustomAttribute<MessageHandlerAttribute>() != null)
                .Select(method => new KeyValuePair<MessageCode, MethodInfo>(
                    method.GetCustomAttribute<MessageHandlerAttribute>().MessageCode, method));

            foreach (var messageCode in (MessageCode[])Enum.GetValues(typeof(MessageCode)))
                mMessageHandlers.Add(messageCode, new List<MethodInfo>());

            foreach (var messageHandler in messageHandlers)
                mMessageHandlers[messageHandler.Key].Add(messageHandler.Value);
        }
    }
}
