using DocumentAssistantLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace DocumentAssistantLibrary.Classes
{
    /// <summary>
    /// Class holds methods which create new Database for aplication
    /// </summary>
    public static class DbCreator
    {
        /// <summary>
        /// Methods creates new database for application
        /// </summary>
        /// <returns><b>true</b> when new database was created, otherwise <b>false</b> </returns>
        public static bool CreateNewDb()
        {
            try
            {
                using (var context = new MainContext())
                {
                    context.Database.Migrate();

                    var salt = PassGenerator.GenerateSalt();
                    var adminRole = new Role
                    {
                        RoleName = "Admin"
                    };
                    context.Roles.Add(adminRole);
                    context.SaveChanges();

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
                    context.Users.Add(adminUser);
                    context.SaveChanges();

                    context.Roles.Add(new Role { RoleName = "Kierownik" });
                    context.Roles.Add(new Role { RoleName = "Koordynator" });
                    context.Roles.Add(new Role { RoleName = "Użytkownik" });
                    context.Roles.Add(new Role { RoleName = "Obserwator" });

                    context.Languages.Add(new Language { LanguageName = "Polski" });
                    context.Languages.Add(new Language { LanguageName = "Angielski" });
                    context.Languages.Add(new Language { LanguageName = "Japoński" });
                    context.Languages.Add(new Language { LanguageName = "Wietnamski" });
                    context.Languages.Add(new Language { LanguageName = "Chiński" });

                    context.SaveChanges();

                    return true;
                }
            }
            catch { return false; }
        }
    }
}
