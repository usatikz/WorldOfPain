using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WorldOfPain
{
    public class Menu
    {
        Army firstArmy, secondArmy;
        Battlefield battlefield;
        CommandInvoker invoker;
        bool subscribed;
        string logPath = "WorldOfPain.log";
        public void ShowMenu()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("1. Create Armies");
            Console.WriteLine("2. Show Armies");
            Console.WriteLine("3. Make a step");
            Console.WriteLine("4. Play  to the end");
            Console.WriteLine("5. unDO last step");
            Console.WriteLine("6. reDO last step");
            Console.WriteLine("7. Subscribe  to arena beep");
            Console.WriteLine("8. UnSubscribe");
            Console.WriteLine("9. Strategy type");
            var respon = Console.ReadLine();
            Console.Clear();
            switch (respon)
            {

                case "1":
                    Console.WriteLine("Enter the amount for which your first army will be purchased:");
                    int money;

                    while (!Int32.TryParse(Console.ReadLine(), out money))
                        Console.WriteLine("Error. Enter the integer.");

                    var factoryArmy = new FactoryArmy();
                    firstArmy = factoryArmy.CreateArmy(money, "1");

                    Console.WriteLine("Enter the amount for which your second army will be purchased:");
                    while (!Int32.TryParse(Console.ReadLine(), out money))
                        Console.WriteLine("Error. Enter the integer.");

                    secondArmy = factoryArmy.CreateArmy(money, "2");

                    battlefield = new Battlefield(firstArmy, secondArmy);
                    invoker = new CommandInvoker(battlefield);

                    Write(String.Format("Armies created.\n{0}", battlefield.GetArmyInfo()));
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "2":
                    if (battlefield != null)
                        Console.WriteLine(battlefield.GetArmyInfo());
                    else
                        Console.WriteLine("Armies not created");
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "3":
                    if (invoker != null && battlefield != null)
                    {
                        invoker.Move();
                        Write(battlefield.MoveInfo);
                    }
                    else
                        Console.WriteLine("Armies not created");
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "4":
                    if (invoker != null && battlefield != null)
                    {
                        invoker.PlayToTheEnd();
                        Write(battlefield.GameInfo);
                         Console.ReadLine();
                         ShowMenu();
                    }
                    else
                        Console.WriteLine("Armies not created");
                    Console.WriteLine();
                    break;
                case "5":
                    if (invoker != null && invoker.Undo())
                        Write("Last move canceled. \n");
                    else { Write("There are no moves. \n"); }
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "6":
                    if (invoker != null && invoker.Redo())
                        Write("Last move repeated.\n");
                    else
                        Write("There are no canceled moves.\n");
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "7":
                    if (invoker != null && battlefield != null)
                    {
                        if (subscribed)
                            Console.WriteLine("Subscription has already been issued. ");
                        else
                        {
                            battlefield.Subscribe();
                            Write("Subscribed.\n");
                            subscribed = true;
                        }
                    }
                    else
                        Console.WriteLine("Armies not created");
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "8":
                    if (!subscribed)
                        Console.WriteLine("Subscription has not already been issued. ");
                    else
                    {
                        battlefield.UnSubscribe();
                        Write("Unsubscribed.\n");
                        subscribed = false;
                    }
                    Console.ReadLine();
                    ShowMenu();
                    break;
                case "9":
                    if (battlefield != null)
                    {
                        Console.WriteLine("Input a number of strategy: 1. 1x1  2. 3x3 3.NxN");
                        string strategy = Console.ReadLine();
                        switch (strategy)
                        {
                            case "1":
                                battlefield.Strategy = new OneToOneStrategy();
                                Write("Strategy 1х1 is established\n");
                                break;
                            case "2":
                                battlefield.Strategy = new ThreeToThreeStrategy();
                                Write("Strategy 3х3 is established\n");
                                break;
                            case "3":
                                battlefield.Strategy = new NToNStrategy(firstArmy, secondArmy);
                                Write("Strategy is established\n");
                                break;
                            default:
                                Console.WriteLine("Error. There is no such item. Plaese try again.");
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Armies not created");
                    }
                    Console.ReadLine();
                    ShowMenu();
                    break;
                default:
                    Console.WriteLine("Error. There is no such item. Please try again.");
                    Console.ReadLine();
                    ShowMenu();
                    break;
            }
        }
        //public void Write(string text)
        //{
        //    Console.WriteLine(text);
        //    using (StreamWriter sw = new StreamWriter("Game.log", true))
        //    {
        //        sw.WriteLine(text);
        //    }
        //}
        //public void ClearLog()
        //{
        //    File.WriteAllText("Game.log", "");
        //    File.WriteAllText("HeavyUnitProxyLog.log", "");
        //}
        public void Write(string text)
        {
            Console.WriteLine(text);
            using (StreamWriter sw = new StreamWriter(logPath, true))
            {
                sw.WriteLine(text);
            }
        }

        public void ClearLog()
        {

            File.WriteAllText(logPath, "");
        }
    }
}

