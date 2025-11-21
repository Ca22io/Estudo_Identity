using Microsoft.AspNetCore.Identity;
using App.Models;
using App.Data; // Adicione este using

public static class IdentitySeeder
{
    public static async Task SeedAdminAsync(IServiceScope scope)
    {
        var serviceProvider = scope.ServiceProvider;
        
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<UsuarioModel>>();

        // ------------------------------------------
        // 1. CRIAR OS ROLES (Papéis)
        // ------------------------------------------
        string[] roleNames = ["Admin", "User"];
        foreach (var roleName in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                var result = await roleManager.CreateAsync(new IdentityRole<int>(roleName));

                if (!result.Succeeded)
                {
                    Console.WriteLine($"ERRO ao criar Role {roleName}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
                }
            }
        }
        
        const string adminEmail = "admin@seuprojeto.com";
        const string adminPassword = "Admin123*"; 
        var adminUser = await userManager.FindByEmailAsync(adminEmail);

        if (adminUser == null)
        {
            adminUser = new UsuarioModel 
            { 
                UserName = adminEmail,
                Nome = "Administrador", 
                Email = adminEmail,
                EmailConfirmed = true
            };

            var createResult = await userManager.CreateAsync(adminUser, adminPassword);
            
            if (!createResult.Succeeded)
            {
                Console.WriteLine($"ERRO ao criar usuário Admin: {string.Join(", ", createResult.Errors.Select(e => e.Description))}");
            }
        }

        // ------------------------------------------
        // 3. ATRIBUIR O PAPEL DE ADMINISTRADOR
        // ------------------------------------------
        if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
        {
            var roleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
        }

    }
}