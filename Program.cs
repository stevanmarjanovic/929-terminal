using DailyLearning;

var controller = new Controller();

bool showLink = true;
bool simplifiedView = false;
foreach (var arg in args)
{
    switch (arg)
    {
        case "update":
            await controller.UpdateFromLocalJsonAsync();
            return 0;

        case "initialize":
            await controller.Initialize();
            return 0;
        
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
