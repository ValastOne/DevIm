using LoginInDevim.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginInDevim
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Login:");
            //string login = Console.ReadLine();
            Console.WriteLine("Password:");
            string password = Console.ReadLine();
            Crypter.Crypter crypter = new Crypter.Crypter();
            password = crypter.Encrypt(password);
            //Console.WriteLine(password);
            //Console.ReadLine();
            Entities.Entities entities = new Entities.Entities();
            var Users = entities.Users;
            Users user = new Users();
            Console.WriteLine("Добавление пользователя");
            user.surname = "Савченко";
            user.name = "Андрей";
            user.role = 3;
            user.login = "Admin";
            user.password = password;
            Users.Add(user);
            var list_users = Users.ToList();
            foreach (var cur_user in list_users)
            {
                Console.WriteLine(crypter.Decrypt(cur_user.password));
            }
            Console.ReadLine();
            entities.SaveChanges();
            Console.WriteLine("Успешное добавление!");
            //password = crypter.Decrypt(password);
            //Console.WriteLine(password);
            Console.ReadLine();
        }
    }
}
