using Cafe.CLI.Menus;
using CafeConsoleCLI.Composition;
using Microsoft.Extensions.DependencyInjection;

var sp = Composition.Build();
sp.GetRequiredService<Menu>().Run();