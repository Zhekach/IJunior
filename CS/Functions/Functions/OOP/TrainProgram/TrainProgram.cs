using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Functions.OOP.TrainProgram
{
    internal class TrainProgram
    {
    }

    class Train
    {
        private int _passengersCount = 0;
        private List<TrainCar> _cars = new List<TrainCar>();
        private bool _isLeft = false;
        public Route Route { get; private set; }

    }

    class TrainCar
    {
        private Dictionary<string, int> _capacity = new Dictionary<string, int>() 
        {
            {"Сидячий", 107},
            {"Общий", 72},
            {"Плацкарт", 54},
            {"Купе", 36},
            {"СВ", 20},
            {"Люкс", 8},
        };
    }

    struct Route
    {
        public string Departure {  get; private set; }
        public string Arrival {  get; private set; }

        public Route(string departure, string arrival)
        {
            Departure = departure;
            Arrival = arrival;
        }

        public void Print()
        {
            Console.WriteLine($"Место отправдения - {Departure}, есто прибытия - {Arrival}.");
        }
    }
}