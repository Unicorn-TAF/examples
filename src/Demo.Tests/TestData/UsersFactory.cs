using Demo.Tests.BO;
using System;

namespace Demo.Tests.TestData
{
    internal class UsersFactory
    {
        /// <summary>
        /// Gets user by its name.
        /// </summary>
        /// <param name="user"><see cref="Users"/> enum value</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static User GetUser(Users user)
        {
            switch (user)
            {
                case Users.NoEmail: return new User("Mrs", "Jane", "Bloggs", "", "jbPwd1");
                case Users.IncorrectEmail: return new User("Mrs", "Jane", "Bloggs", "yahoomail.com", "jbPwd1");
                case Users.JDoe: return new User("Mr", "John", "Doe", "jdoe@mail.com", "jdoePwd1");
                case Users.NoPassword: return new User("Mr", "Alan", "Smithee", "asmith@mailbox.com", "");
                case Users.NoTitle: return new User("", "Rudolf", "Lingens", "rlingens@yahoomail.com", "123456");
                case Users.NoGivenName: return new User("Mr", "", "Lingens", "rlingens@yahoomail.com", "123456");
                default: throw new ArgumentException($"User '{user}' is not valid.");
            }
        }

        /// <summary>
        /// Gets default user (JDoe).
        /// </summary>
        /// <returns></returns>
        public static User GetDefaultUser() =>
            GetUser(Users.JDoe);
    }
}
