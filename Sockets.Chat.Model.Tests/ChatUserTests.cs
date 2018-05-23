using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sockets.Chat.Model;
using Sockets.Chat.Model.Data;
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

            Assert.AreEqual("name#1", user.ToString());
        }

        [TestMethod()]
        public void ParseTest()
        {
            ChatUser user = ChatUser.Parse("name#1");

            Assert.AreEqual(user.Id, 1);
            Assert.AreEqual(user.Name, "name");
        }

        [TestMethod()]
        public void EqualsTest()
        {
            ChatUser user = ChatUser.Parse("name#1");
            ChatUser user2 = user;
            ChatUser user3 = ChatUser.Parse("name#1");

            Assert.IsTrue(user.Equals(user2));
            Assert.IsTrue(user.Equals(user3));

            Assert.IsTrue(user == user2);
            Assert.IsTrue(user != user3);

            ChatUser[] users = new ChatUser[] { user, user2, user3 };

            var t = users.Where(u => u.Equals(user));

            Assert.AreEqual(3, t.Count());
        }
    }
}