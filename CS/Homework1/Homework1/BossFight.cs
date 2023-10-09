using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Homework1
{
    internal class BossFight
    {
        static void Main(string[] args)
        {
            const string HookMenu = "1";
            const string GoatMenu = "2";
            const string HealMenu = "3";
            const string SlapMenu = "4";

            float enemyMaxHealth = 200f;
            float enemyHealth = enemyMaxHealth;
            int enemyMinDamage = 10;
            int enemyMaxDamage = 20;
            int enemyHealHP = 10;
            int enemyHealCount = 5;
            int enemyCurrentHealCount = 0;
            int enemyCurrentDamage = 0;

            float playerMaxHealth = 150f;
            float playerHealth = playerMaxHealth;
            int playerMinDamage = 10;
            int playerMaxDamage = 30;
            int playerHookCount = 0;
            float playerGoatCritChance = 0.5f;
            float playerGoatPowerMul = 2f;
            int playeBaseAddDamage = 5;
            int playerHealHP = 25;
            bool isPlayerHealed = false;
            float playerSlapAntiCritChance = 0.75f;
            float playerSlapPowerMul = 0.5f;
            int playerCurrentDamage = 0;
            Random random = new Random();

            Console.WriteLine("Добро пожаловать в ад. Сдохни, или умри.");
            Console.WriteLine($"Каждые {enemyHealCount} ударов босс востанавливает {enemyHealHP} HP.");
            Console.WriteLine("Если вы неверно выбрали удар (ошибка, или он был недоступен) - пропускаете ход.");

            while (playerHealth > 0 && enemyHealth > 0)
            {
                Console.WriteLine($"У вас {playerHealth} HP, у босса {enemyHealth} HP. \nВыбирайте удар (номер)");
                Console.WriteLine($"{HookMenu}. Хук с правой." +
                    $"\n- Базовый урон {playerMinDamage}-{playerMaxDamage} HP.");
                Console.WriteLine($"{GoatMenu}. Сделать козу. " +
                    $"\n- С вероятностью {playerGoatCritChance} наносит критический урон (Х{playerGoatPowerMul})." +
                    $"\n- Увеличивает базовый урон на {playeBaseAddDamage} HP." +
                    $"\nУсловие: доступно после двух ударов \"Хук с правой\". ");
                Console.WriteLine($"{HealMenu}. Передышка." +
                    $"\n- Позволяет восстановить {playerHealHP} HP." +
                    $"\nУсловие: После этого приёма доступен только удар \"Пощёчина\".");
                Console.WriteLine($"{SlapMenu}. Пощёчина." +
                    $"\n- С вероятностью {playerSlapAntiCritChance} наносит уменьшенный урон (Х{playerSlapPowerMul}).");

                switch (Console.ReadLine())
                {
                    case HookMenu:
                        if (isPlayerHealed)
                        {
                            isPlayerHealed = false;
                            break;
                        }

                        playerHookCount++;
                        playerCurrentDamage = random.Next(playerMinDamage, playerMaxDamage);
                        enemyHealth -= playerCurrentDamage;
                        break;

                    case GoatMenu:
                        if (isPlayerHealed || playerHookCount < 2)
                        {
                            isPlayerHealed = false;
                            break;
                        }

                        if (random.NextDouble() < playerGoatCritChance)
                        {
                            playerCurrentDamage = Convert.ToInt32(random.Next(playerMinDamage, playerMaxDamage) * playerGoatPowerMul);
                        }
                        else
                        {
                            playerCurrentDamage = random.Next(playerMinDamage, playerMaxDamage);
                        }

                        playerHookCount = 0;
                        enemyHealth -= playerCurrentDamage;
                        playerMinDamage += playeBaseAddDamage;
                        playerMaxDamage += playeBaseAddDamage;
                        break;

                    case HealMenu:
                        if (isPlayerHealed)
                        {
                            isPlayerHealed = false;
                            break;
                        }

                        isPlayerHealed = true;
                        playerHealth += playerHealHP;

                        if(playerHealth > playerMaxHealth)
                        {
                            playerHealth = playerMaxHealth;
                        }

                        Console.WriteLine($"Вы восстановили {playerHealHP} HP");
                        break;

                    case SlapMenu:
                        isPlayerHealed = false;

                        if (random.NextDouble() < playerSlapAntiCritChance)
                        {
                            playerCurrentDamage = Convert.ToInt32(random.Next(playerMinDamage, playerMaxDamage) * playerSlapPowerMul);
                        }
                        else
                        {
                            playerCurrentDamage = random.Next(playerMinDamage, playerMaxDamage);
                        }

                        enemyHealth -= playerCurrentDamage;
                        break;

                    default:
                        isPlayerHealed = false;
                        Console.WriteLine("Ошибочка вышла, пропускаете ход.");
                        break;
                }

                enemyCurrentDamage = random.Next(enemyMinDamage, enemyMaxDamage);
                playerHealth -= enemyCurrentDamage;
                enemyCurrentHealCount++;

                if (enemyCurrentHealCount >= enemyHealCount)
                {
                    enemyHealth += enemyHealHP;

                    if(enemyHealth > enemyMaxHealth)
                    {
                        enemyHealth = enemyMaxHealth;
                    }

                    Console.WriteLine($"Босс востановил {enemyHealHP} HP.");
                    enemyCurrentHealCount = 0;
                }

                Console.WriteLine($"Вы нанесли урон в {playerCurrentDamage} HP " +
                    $", а босс нанес Вам урон в {enemyCurrentDamage} HP");
                playerCurrentDamage = 0;
                enemyCurrentDamage = 0;
            }

            Console.WriteLine("Битва закончена.");

            if (playerHealth > 0)
            {
                Console.WriteLine("Вы выиграли, босс повержен... ?");
            }
            else if (enemyHealth > 0)
            {
                Console.WriteLine("Враг победил, вы раздавлены.");
            }
            else
            {
                Console.WriteLine("Странно, но похоже все умерли. Ничья, получается?");
            }
        }

    }
}