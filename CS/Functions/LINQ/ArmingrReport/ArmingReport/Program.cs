 internal class Program
 {
     private static void Main()
     {
         Application application = new Application();
         application.Run();
     }
 }

 internal class Application
 {
     private readonly List<Soldier> _soldiers;

     public Application()
     {
         _soldiers = SoldiersGenerator();
     }

     public void Run()
     {
         PrintSoldiersInfo(_soldiers, "Полные данные обо всех солдатах");
         
         var soldiersInfo = _soldiers
             .Select(soldier => new {Name = soldier.Name, soldier.Rank});

         Console.WriteLine("Сокращённый список с именами и званиями:");
         
         foreach (var soldierInfo in soldiersInfo)
         {
             Console.WriteLine($"Имя - {soldierInfo.Name}, звание - {soldierInfo.Rank}");
         }
     }

     private List<Soldier> SoldiersGenerator()
     {
         List<Soldier> soldiers = new List<Soldier>();
        
         soldiers.Add(new Soldier("Джони","Винтовка","Рядовой",13));
         soldiers.Add(new Soldier("Алексей", "Автомат", "Сержант", 25));
         soldiers.Add(new Soldier("Михаил", "Пистолет", "Лейтенант", 31));
         soldiers.Add(new Soldier("Иван", "Пулемёт", "Капитан", 28));
         soldiers.Add(new Soldier("Андрей", "Снайперская винтовка", "Ефрейтор", 19));
         soldiers.Add(new Soldier("Дмитрий", "Гранатомёт", "Майор", 37));
         
         return soldiers;
     }

     private void PrintSoldiersInfo(List<Soldier> soldiers, string description)
     {
         Console.WriteLine(description);

         foreach (var soldier in soldiers)
         {
             Console.WriteLine($"Имя - {soldier.Name}, звание - {soldier.Rank}, " +
                               $"оружие - {soldier.Weapon}, срок службы {soldier.LifeTime}");
         }
     }
 }

 internal class Soldier
 {
     public Soldier(string name, string weapon, string rank, int lifeTime)
     {
         Name = name;
         Weapon = weapon;
         Rank = rank;
         LifeTime = lifeTime;
     }

     public string Name { get; private set; }
     public string Weapon { get; private set; }
     public string Rank { get; private set; }
     public int LifeTime { get; private set; }
 }