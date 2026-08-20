using NineTwoNineTerminal;
using NineTwoNineTerminal.Application;

var controller = new Controller();

var showLink = true;
var simplifiedView = false;
foreach (var arg in args)
{
    switch (arg)
    {
        case "--simple":
            simplifiedView = true;
            break;
            
        case "--no-link":
            showLink = false;
            break;
    }
}

await controller.Run(showLink, simplifiedView);

return 0;
