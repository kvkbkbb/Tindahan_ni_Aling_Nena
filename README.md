# Tindahan ni Aling Nena

An endless cashier simulation set in a Filipino sari-sari store. The player adds up the items, works out the change, and hands back the exact amount using bills and coins from the cash drawer.

Built with C# and Windows Forms for IT111L.

> This README covers the **game logic (`Core/`)**. The UI (`UI/`) is handled separately and talks to the logic only through `GameSession`.

---

## Game rules (as implemented in Core)

- Endless play, no time limit.
- Each customer has an order. The player gets the payment, puts it in the drawer, and gives change.
- **Exact change** counts as a successful sale: streak +1, profit += order total.
- **Wrong change** (too much or too little): streak resets to 0, profit -= (amount off + ₱20 fine). The sale does not count.
- **Kulang po**: used when the customer paid less than the total.
- Best streak is saved as the high score.

### Open rules (the group must decide)

1. Does a correct *Kulang po* earn profit, or is the customer simply dismissed? Does it extend the streak?
2. Does a wrong *Kulang po* (on a full payment) get the ₱20 fine, or only a streak reset?

Whatever is decided goes in `GameSession.CallKulangPo()`.

---

## Folder structure

```
Tindahan_ni_Aling_Nena/
├── Program.cs
├── App.config
├── Core/                  <- game logic, NO System.Windows.Forms allowed
│   ├── Money/
│   │   ├── Denomination.cs
│   │   ├── CashDrawer.cs
│   │   └── ChangeChecker.cs
│   ├── Orders/
│   │   ├── Product.cs
│   │   ├── ProductCatalog.cs
│   │   ├── Order.cs
│   │   └── OrderGenerator.cs
│   ├── Customers/
│   │   ├── Customer.cs
│   │   └── PaymentGenerator.cs
│   └── Game/
│       ├── GameSession.cs
│       ├── SaleResult.cs
│       ├── ScoreManager.cs
│       └── HighScoreStore.cs
├── UI/                    <- handled separately
└── Assets/
```

---

## Core reference

### Core/Money/

**`Denomination.cs`** (record / class)
Represents one kind of bill or coin and holds the master list of all of them. Agree on this list first, because the drawer, payment generator, and UI art all depend on it.

```csharp
string Name
decimal Value
bool IsBill
static IReadOnlyList<Denomination> All
```

**`CashDrawer.cs`** (regular class)
Tracks the stock of each denomination.

```csharp
void Add(Denomination d)         // payment dropped in, or restock
bool Remove(Denomination d)      // false if out of stock
int Count(Denomination d)        // stock of one denomination
decimal Total { get; }           // value of everything inside
void Reset()                     // restore starting stock
```

**`ChangeChecker.cs`** (static class)
The referee for change. It compares total value only, not which pieces were used.

```csharp
static decimal ExpectedChange(Customer c)                                // paid - total
static decimal Difference(Customer c, IEnumerable<Denomination> given)   // given - expected
static bool IsExact(Customer c, IEnumerable<Denomination> given)
```

### Core/Orders/

**`Product.cs`** (record / class): one sari-sari item.

```csharp
string Name
decimal Price
string ImageKey                  // lets the UI find the picture
```

**`ProductCatalog.cs`** (static class)
The only place prices are defined.

```csharp
static IReadOnlyList<Product> All
static Product GetRandom(Random rng)
```

**`Order.cs`** (regular class): the items one customer is buying.

```csharp
List<Product> Items { get; }
decimal Total { get; }           // computed from Items, never stored
void AddItem(Product p)
```

**`OrderGenerator.cs`** (regular class): builds random orders and controls difficulty.

```csharp
Order Generate(int streak)       // harder orders as the streak grows
```

### Core/Customers/

**`Customer.cs`** (regular class)

```csharp
Order Order { get; }
decimal AmountPaid { get; }
List<Denomination> PaymentPieces { get; }
bool IsUnderpaying { get; }      // AmountPaid < Order.Total
```

**`PaymentGenerator.cs`** (regular class)
Chooses what the customer pays with and occasionally triggers the underpay event. It must never create a payment the drawer cannot make change for, or the game becomes unwinnable.

```csharp
Customer Generate(Order order, CashDrawer drawer)
```

### Core/Game/

**`GameSession.cs`** (regular class)
The coordinator for one play-through and the **only class the UI talks to**. It creates the `ScoreManager`, `CashDrawer`, and generators itself. "Play again" is `new GameSession()`.

```csharp
event Action<Customer> CustomerArrived
event Action<SaleResult> SaleCompleted
event Action GameOver

Customer CurrentCustomer { get; }
int Streak { get; }
int BestStreak { get; }
decimal Profit { get; }
bool IsPaused { get; }

void Start()                     // begins the first customer
void AcceptPayment()             // payment dragged into the drawer
void GiveChange(IEnumerable<Denomination> given)
void CallKulangPo()
void Pause()
void Resume()
```

**`SaleResult.cs`** (record / class): describes what happened on one sale.

```csharp
bool IsCorrect
decimal Difference               // how far off the change was
decimal ProfitChange
int NewStreak
```

**`ScoreManager.cs`** (regular class)

```csharp
int Streak { get; }
int BestStreak { get; }
decimal Profit { get; }
event Action BestStreakBeaten

void RecordCorrect(decimal orderTotal)   // streak++, profit += total
void RecordWrong(decimal amountOff)      // streak = 0, profit -= amountOff + 20
void Reset()
```

**`HighScoreStore.cs`** (regular class)
Saves the best streak to a plain text file in `Environment.SpecialFolder.ApplicationData`, **not** next to the source files, so it never gets committed.

```csharp
int Load()                       // saved best streak, or 0
void Save(int bestStreak)
```

---

## How one customer flows through Core

1. `GameSession` asks `OrderGenerator` for an `Order`, then `PaymentGenerator` for a `Customer`.
2. `CustomerArrived` fires.
3. The payment goes into the drawer (`AcceptPayment()` calls `CashDrawer.Add`).
4. The player gives change, and the UI calls `GameSession.GiveChange(pieces)`.
5. `ChangeChecker` checks the amount, and `ScoreManager` updates streak and profit.
6. `SaleCompleted` fires with a `SaleResult`.
7. The next customer arrives.

For *Kulang po*, `GameSession.CallKulangPo()` checks `Customer.IsUnderpaying`.

### Dependencies (one direction only)

```
Denomination  <- CashDrawer, ChangeChecker, PaymentGenerator
Product <- ProductCatalog <- OrderGenerator -> Order
Order + PaymentGenerator -> Customer
Customer + CashDrawer + ChangeChecker + ScoreManager + HighScoreStore -> GameSession
```

Nothing in `Money/` or `Orders/` knows about `GameSession`. This is what lets everyone work in parallel.

---

## Coding rules for Core

- **Use `decimal` for all money**, never `double`. Floating-point errors will make change checks fail randomly.
- **No `using System.Windows.Forms`** anywhere in `Core/`.
- `static` is only for pure functions and fixed data (`ChangeChecker`, `ProductCatalog`). Anything with changing state is a regular class.
- **No singletons.** `GameSession` owns its own `ScoreManager`, `CashDrawer`, and generators.
- Drive the game with events, not a `while` loop, so the UI thread never freezes.
- Naming: PascalCase for types and methods, `_camelCase` for private fields, one class per file, filename matches class name.
- **Framework note:** `record` types need C# 9 and a modern .NET target. If the project stays on .NET Framework, use plain classes instead.

---

## Team and ownership

| Area | Files |
|---|---|
| Money & Drawer | `Denomination`, `CashDrawer`, `ChangeChecker` |
| Orders & Customers | `Product`, `ProductCatalog`, `Order`, `OrderGenerator`, `Customer`, `PaymentGenerator` |
| Game Flow & Score | `GameSession`, `SaleResult`, `ScoreManager`, `HighScoreStore`, `Program.cs` |
| UI | `UI/` folder (handled separately) |

Fill in names next to each area.

---

## Git workflow

- Work on your own branch: `feature/your-feature`.
- Pull from `main` before starting each day.
- Merge back often, and don't let a branch live longer than a few days.
- Only edit files in your own area.
- Pull before adding a file, and commit the `.csproj` quickly, since it changes whenever a file is added.
- Open `.sln`, not the `.csproj` or individual files.

---

## Day one checklist

- [ ] One person creates the project and pushes it, with `.gitignore` (Visual Studio template) and this README
- [ ] Push empty stub files for every class above, with the public members listed
- [ ] Agree on the denomination list
- [ ] Decide the two open *Kulang po* rules
- [ ] Everyone clones, opens the `.sln`, and confirms it builds


This README.md and the organization of this project was assisted with the help of Claude.
