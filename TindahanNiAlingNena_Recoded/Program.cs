using TindahanNiAlingNena_Recoded.Services;
using TindahanNiAlingNena_Recoded.UI;

//ApplicationConfiguration.Initialize();

Game game = new Game();
GameScreen screen = new GameScreen();

// player clicks -> game logic
screen.DenominationClicked += game.AddChangePiece;
screen.GiveChangeClicked += game.GiveChange;
screen.ClearClicked += game.ClearChange;
screen.AskForMoreClicked += game.AskForMore;

// game changes -> screen display
game.StateChanged += screen.ShowState;
game.MessageChanged += screen.ShowMessage;

game.Start();
Application.Run(screen);