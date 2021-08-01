using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfPain
{
    interface IPublisher
    {
        void AddEvent(EventHandler eventHandler);
        void DeleteEvent(EventHandler eventHandler);
        void NotifyObservers();
    }
    interface IUnit : IPublisher
    {
        int Health { set; get; }
        int Attack { set; get; }
        int Defence { set; get; }
        int Cost { set; get; }
        string Name { set; get; }
        IUnit Fight(IUnit unit);
        IUnit Copy();
        string GetInfo();
    }

    interface ISpecialAction
    {
        IUnit DoSpecialAction(IUnit unit);
        int Strength { set; get; }
        int Range { set; get; }
    }

    interface ICurable
    {
        void Heal(int health);
    }

    interface ICloneable
    {
        IUnit Clone();
    }

    abstract class Unit : IUnit
    {
        static int counter = 0;
        public int Health { set; get; }
        public int Attack { set; get; }
        public int Defence { set; get; }
        public int Cost { set; get; }
        public string Name { set; get; }
        public int UnitId { private set; get; }
        public event EventHandler Handler;
        public Unit()
        {
            UnitId = counter++;

        }
        public virtual IUnit Fight(IUnit unit)
        {
            if (Attack > unit.Defence + unit.Health)
                return unit;
            if (unit.Defence > Attack)
            {
                unit.Defence = unit.Defence - Attack;
                return null;
            }
            unit.Health = unit.Health - (Attack - unit.Defence);
            unit.Defence = 0;
            return null;
        }
        public override int GetHashCode()
        {
            return this.UnitId;
        }
        public override bool Equals(object obj)
        {
            return Equals(obj as Unit);
        }
        public bool Equals(Unit other)
        {
            if (other == null) return false;
            return (this.UnitId.Equals(other.UnitId));
        }
        public virtual void AddEvent(EventHandler eventHandler)
        {
            Handler += eventHandler;
        }
        public virtual void DeleteEvent(EventHandler eventHandler)
        {
            Handler -= eventHandler;
        }
        public virtual void NotifyObservers()
        {
            Handler?.Invoke(this, EventArgs.Empty);
        }
        public virtual IUnit Copy()
        {
            return null;
        }
        public virtual string GetInfo()
        {
            return $"{Name} :::::: attack {Attack}, defence {Defence}, health  {Health}";
        }
    }

    class Rand
    {
        private static readonly Random random = new Random();
        private static readonly object syncLock = new object();
        public static int Get(int min, int max)
        {
            lock (syncLock)
            {
                return random.Next(min, max);
            }
        }
    }

    class LightUnit : Unit, IUnit, ICurable, ICloneable, ISpecialAction
    {
        public int Strength { set; get; }
        public int Range { set; get; }

        public LightUnit() : base()
        {
            Health = SettingsUnit.LightUnit.Health;
            Attack = Rand.Get(SettingsUnit.LightUnit.rangeAttackForce.Item1, SettingsUnit.LightUnit.rangeAttackForce.Item2);
            Defence = Rand.Get(SettingsUnit.LightUnit.rangeDefence.Item1, SettingsUnit.LightUnit.rangeDefence.Item2);
            Cost = SettingsUnit.LightUnit.Cost;
            Name = SettingsUnit.LightUnit.Name;
            Range = SettingsUnit.LightUnit.range;
        }

        public LightUnit(LightUnit unit) : base()
        {
            Health = unit.Health;
            Attack = unit.Attack;
            Defence = unit.Defence;
            Cost = unit.Cost;
            Name = unit.Name;
            Range = unit.Range;
        }

        public void Heal(int health)
        {
            Health += health;
            if (Health > 100)
                Health = 100;
        }

        public IUnit Clone()
        {
            return new LightUnit(this);
        }
        public override IUnit Copy()
        {
            return new LightUnit(this);
        }

        public IUnit DoSpecialAction(IUnit unit)
        {
            HeavyUnit heavyunit = unit as HeavyUnit;
            if (heavyunit == null)
                return null;

            switch (Rand.Get(0, 4))
            {
                case 0:
                    return new HorseDecorator(unit);
                case 1:
                    return new ShieldDecorator(unit);
                case 2:
                    return new PikeDecorator(unit);
                case 3:
                    return new HelmetDecorator(unit);
                default:
                    break;
            }
            return null;
        }
    }

    class HeavyUnit : Unit, IUnit
    {
        public HeavyUnit() : base()
        {
            Health = SettingsUnit.HeavyUnit.Health;
            Attack = Rand.Get(SettingsUnit.HeavyUnit.rangeAttack.Item1, SettingsUnit.HeavyUnit.rangeAttack.Item2);
            Defence = Rand.Get(SettingsUnit.HeavyUnit.rangeDefence.Item1, SettingsUnit.HeavyUnit.rangeDefence.Item2);
            Cost = SettingsUnit.HeavyUnit.Cost;
            Name = SettingsUnit.HeavyUnit.Name;
        }

        public HeavyUnit(HeavyUnit unit) : base()
        {
            Health = unit.Health;
            Attack = unit.Attack;
            Defence = unit.Defence;
            Cost = unit.Cost;
            Name = unit.Name;
        }
        public override IUnit Copy()
        {
            return new HeavyUnit(this);
        }
    }

    class BowmanUnit : Unit, IUnit, ICurable, ICloneable, ISpecialAction
    {
        public int Strength { set; get; }
        public int Range { set; get; }

        public BowmanUnit() : base()
        {
            Health = SettingsUnit.Bowman.Health;
            Attack = Rand.Get(SettingsUnit.Bowman.rangeAttack.Item1, SettingsUnit.Bowman.rangeAttack.Item2);
            Defence = Rand.Get(SettingsUnit.Bowman.rangeDefence.Item1, SettingsUnit.Bowman.rangeDefence.Item2);
            Range = SettingsUnit.Bowman.range;
            Strength = Rand.Get(SettingsUnit.Bowman.rangeStrength.Item1, SettingsUnit.Bowman.rangeStrength.Item2);
            Cost = SettingsUnit.Bowman.Cost;
            Name = SettingsUnit.Bowman.Name;
        }

        public BowmanUnit(BowmanUnit unit) : base()
        {
            Health = unit.Health;
            Attack = unit.Attack;
            Defence = unit.Defence;
            Range = unit.Range;
            Strength = unit.Strength;
            Cost = unit.Cost;
            Name = unit.Name;
        }

        public void Heal(int health)
        {
            Health += health;
            if (Health > 100)
                Health = 100;
        }

        public IUnit Clone()
        {
            return new BowmanUnit(this);
        }
        public override IUnit Copy()
        {
            return new BowmanUnit(this);
        }
        public IUnit DoSpecialAction(IUnit unit)
        {
            if (Strength > unit.Defence + unit.Health)
                return this;
            unit.Defence -= Strength;
            if (unit.Defence < 0)
            {
                unit.Health += unit.Defence;
                unit.Defence = 0;
            }
            return unit;
        }
        public override string GetInfo()
        {
            return base.GetInfo() + $", strength {Strength}";
        }
    }

    class HealerUnit : Unit, IUnit, ICurable, ISpecialAction
    {
        public int Strength { set; get; }
        public int Range { set; get; }

        public HealerUnit() : base()
        {
            Health = SettingsUnit.HealerUnit.Health;
            Attack = Rand.Get(SettingsUnit.HealerUnit.rangeAttack.Item1, SettingsUnit.HealerUnit.rangeAttack.Item2);
            Defence = Rand.Get(SettingsUnit.HealerUnit.rangeDefence.Item1, SettingsUnit.HealerUnit.rangeDefence.Item2);
            Range = SettingsUnit.HealerUnit.range;
            Strength = Rand.Get(SettingsUnit.HealerUnit.rangeStrength.Item1, SettingsUnit.HealerUnit.rangeStrength.Item2);
            Cost = SettingsUnit.HealerUnit.Cost;
            Name = SettingsUnit.HealerUnit.Name;
        }

        public HealerUnit(HealerUnit unit) : base()
        {
            Health = unit.Health;
            Attack = unit.Attack;
            Defence = unit.Defence;
            Range = unit.Range;
            Strength = unit.Strength;
            Cost = unit.Cost;
            Name = unit.Name;
        }

        public IUnit DoSpecialAction(IUnit unit)
        {
            if (unit is ICurable)
            {
                ((ICurable)unit).Heal(Strength);
                return unit;
            }
            return null;
        }

        public void Heal(int health)
        {
            Health += health;
            if (Health > 100)
                Health = 100;
        }
        public override IUnit Copy()
        {
            return new HealerUnit(this);
        }
        public override string GetInfo()
        {
            return base.GetInfo() + $", strength {Strength}";
        }
    }

    class WizardUnit : Unit, IUnit, ICurable, ISpecialAction
    {
        public int Strength { set; get; }
        public int Range { set; get; }

        public WizardUnit() : base()
        {
            Health = SettingsUnit.Wizard.Health;
            Attack = Rand.Get(SettingsUnit.Wizard.rangeAttack.Item1, SettingsUnit.Wizard.rangeAttack.Item2);
            Defence = Rand.Get(SettingsUnit.Wizard.rangeDefence.Item1, SettingsUnit.Wizard.rangeDefence.Item2);
            
            Range = SettingsUnit.Wizard.range;
            Cost = SettingsUnit.Wizard.Cost;
            Name = SettingsUnit.Wizard.Name;
        }

        public WizardUnit(WizardUnit unit) : base()
        {
            Health = unit.Health;
            Attack = unit.Attack;
            Defence = unit.Defence;
            Range = unit.Range;
            Cost = unit.Cost;
            Name = unit.Name;
        }

        public IUnit DoSpecialAction(IUnit unit)
        {
            if (unit is ICloneable)
            {
                int rand = Rand.Get(0, 100);
                if (rand < 50)
                    return ((ICloneable)unit).Clone();
            }
            return null;
        }
        public override IUnit Copy()
        {
            return new WizardUnit(this);
        }
        public void Heal(int health)
        {
            Health += health;
            if (Health > 100)
                Health = 100;
        }
        public override string GetInfo()
        {
            return base.GetInfo();
        }
    }
}

