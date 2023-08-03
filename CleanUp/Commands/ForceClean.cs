using System;
using CommandSystem;
using Exiled.API.Features;
using System.Collections.Generic;

namespace CleanUp.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    [CommandHandler(typeof(GameConsoleCommandHandler))]
    class ForceClean : ICommand
    {
        public string Command => "cleanUp";
        public string[] Aliases => new string[] { "CuP" };
        public string Description => "Limpieza de items del piso";
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            try
            {
                response = EventHandler.Clean();
                return true;
            }
            catch (Exception)
            {
                response = "Hubo un error (Comando)";
                return false;
            }
        }
    }
}
