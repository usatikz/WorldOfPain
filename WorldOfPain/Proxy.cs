using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace WorldOfPain
{
    class Proxy : IUnit
    {
        HeavyUnit heavyUnit; // замещаемый объект
        int healthCache;
        int attackCache;
        int defenceCache;
        int costCache;
        string nameCache;
        
        public int Health {
            set { heavyUnit.Health = value; healthCache = value; }
            get { return healthCache; }
        }

        public int Attack {
            set { heavyUnit.Attack = value; attackCache = value; }
            get { return attackCache; }
        }
        public int Defence {
            set { heavyUnit.Defence = value; defenceCache = value; }
            get { return defenceCache; }
        }
        public int Cost {
            set { heavyUnit.Cost = value; costCache = value; }
            get { return costCache; }
        }
        public string Name { set { heavyUnit.Name = value; nameCache = value; }
            get { return nameCache; }
        }
        public event EventHandler Handler;
        public int UnitId { private set; get;}
        public Proxy(HeavyUnit unit)
        {
            heavyUnit = unit;
            healthCache = unit.Health;
            attackCache = unit.Attack;
            defenceCache = unit.Defence;
            costCache = unit.Cost;
            nameCache = unit.Name;
            UnitId = unit.UnitId;
        }
        public IUnit Fight(IUnit unit)
        {
            var text = $"Battle. {GetInfo()} vs {unit.GetInfo()}. ";
            IUnit dead = heavyUnit.Fight(unit);
            if (dead == null)
                text += $"After {unit.GetInfo()} .";
            else
                text += "Enemy died. ";
            LogProxy(text);
            return dead;
        }
        public void AddEvent(EventHandler eventHandler)
        {
            Handler += eventHandler;
            heavyUnit.AddEvent(eventHandler);
        }
        public void DeleteEvent(EventHandler eventHandler)
        {
            Handler -= eventHandler;
            heavyUnit.DeleteEvent(eventHandler);
        }
        public void NotifyObservers()
        {
            Handler?.Invoke(this, EventArgs.Empty);
        }
        public string GetInfo()
        {
            return $"{Name} ::::: attack {Attack}, defence {Defence}, health  {Health}";
        }
        public void LogProxy(string text)//Логирование 
        {
            using (StreamWriter sw = new StreamWriter("HeavyUnitProxyLog.log", true))
            {
                sw.WriteLine(text);
            }
        }
        public IUnit Copy()
        {
            return new Proxy((HeavyUnit)heavyUnit.Copy());
        }
    }
}



