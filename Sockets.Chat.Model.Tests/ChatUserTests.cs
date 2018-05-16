using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sockets.Chat.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sockets.Chat.Model.Tests
{
    [TestClass()]
    public class ChatUserTests
    {
        [TestMethod()]
        public void ChatUserTest()
        {
            ChatUser user = new ChatUser(1, "name");
        }

        [TestMethod()]
        public void ToStringTest()
        {
            ChatUser user = new ChatUser(1, "name");

            Assert.AreEqual("1#name", user.ToString());
        }

        [TestMethod()]
        public void ParseTest()
        {
            ChatUser user = ChatUser.Parse("1#name");

            Assert.AreEqual(user.Id, 1);
            Assert.AreEqual(user.Name, "name");
        }
    }
}