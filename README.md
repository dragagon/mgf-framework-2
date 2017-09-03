# mgf-framework-2
Repository for the MGF Framework v2.0 for the CJR Gaming videos

SETUP

After downloading the repository, open the solution in Visual Studio 2017.
Right click the solution file and "Restore NuGet Packages"

If you are still using Photon 3, unload MGF-Photon4, remove the reference to MGF-Photon4 from the Servers project, and add MGF-Photon.
If you are using Photon 4, unload the MGF-Photon Project

Right click the MGF project you are using, add references to your location for Photon's ExitGames.Logging.Log4New, ExitGamesLibs, log4net, Photon.SocketServer and PhotonHostRuntimes
Do the same for Servers.

Right click the Servers project and go to properties
Go to the build tab and set the output folder to a new subfolder in your Photon deploy directory and build the solution.
