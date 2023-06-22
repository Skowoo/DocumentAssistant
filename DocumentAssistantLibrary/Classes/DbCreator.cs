using DocumentAssistantLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DocumentAssistantLibrary;

namespace DocumentAssistantLibrary.Classes
{
    public static class DbCreator
    {
        public static bool CreateNewDb()
        {
            try
            {
                using (var db = new MainContext())
                {
                    db.Database.EnsureCreated();

                    var salt = PassGenerator.GenerateSalt();
                    var adminRole = new Role
                    {
                        RoleName = "Admin"
                    };
                    db.Roles.Add(adminRole);
                    db.SaveChanges();

                    var adminUser = new User
                    {
                        FirstName = "Admin",
                        LastName = "Admin",
                        Login = "Admin",
                        Password = PassGenerator.ComputeHash("Admin", salt),
                        Salt = salt,
                        IsActive = true,
                        RoleID = 1
                    };
                    db.Users.Add(adminUser);
                    db.SaveChanges();

                    db.Roles.Add(new Role { RoleName = "Kierownik" });
                    db.Roles.Add(new Role { RoleName = "Koordynator" });
                    db.Roles.Add(new Role { RoleName = "Użytkownik" });
                    db.Roles.Add(new Role { RoleName = "Obserwator" });

                    db.Languages.Add(new Language { LanguageName = "Polski" });
                    db.Languages.Add(new Language { LanguageName = "Angielski" });
                    db.Languages.Add(new Language { LanguageName = "Japoński" });
                    db.Languages.Add(new Language { LanguageName = "Wietnamski" });
                    db.Languages.Add(new Language { LanguageName = "Chiński" });

                    db.SaveChanges();

                    return true;
                }
            }
            catch { return false; }            
        }
    }
}
