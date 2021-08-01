using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfPain
{
    
    class FactoryArmy
    {
        public Army CreateArmy(int money, string name)
        {
            var unitFactory = new FactoryUnit();
            var army = new Army(name);

            while (money >= SettingsUnit.MinCost)
            {
                var unit = unitFactory.GetUnit();
                if (money - unit.Cost >= 0)
                {
                    army.Push(unit);
                    money -= unit.Cost;
                }
            }
            return army;
        }

    }
    class FactoryUnit
    {
        public IUnit GetUnit()
        {
            int probability = Rand.Get(0, 100);

            if (probability < 46)
                return new LightUnit();
            else
            if (probability < 59)
                return new Proxy(new HeavyUnit());
            else
            if (probability < 73)
                return new HealerUnit();
            else
            if (probability < 87)
                return new BowmanUnit();
            else
                return new WizardUnit();
        }
    }
}

