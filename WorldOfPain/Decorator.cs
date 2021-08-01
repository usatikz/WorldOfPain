using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfPain
{
    abstract class HeavyUnitDecorator : HeavyUnit
    {
        protected IUnit Unit;
        public HeavyUnitDecorator(IUnit unit)
        {
            Unit = unit;
        }
        public override void AddEvent(EventHandler eventHandler)
        {
            Unit.AddEvent(eventHandler);
        }
        public override void DeleteEvent(EventHandler eventHandler)
        {
            Unit.DeleteEvent(eventHandler);
        }
        public override IUnit Fight(IUnit unit)
        {
            return Unit.Fight(unit);
        }
        public override void NotifyObservers()
        {
            Unit.NotifyObservers();
        }
        public override string GetInfo()
        {
            return Unit.GetInfo();
        }
    }

    class HorseDecorator : HeavyUnitDecorator
    {
        public HorseDecorator(IUnit unit) : base(unit)
        {
            Unit.Attack = Unit.Attack + 20;
            Unit.Name = Unit.Name + " on horse";
        }
        public override IUnit Copy()
        {
            return new HorseDecorator(Unit.Copy());
        }
    }

    class ShieldDecorator : HeavyUnitDecorator
    {
        public ShieldDecorator(IUnit unit)
            : base(unit)
        {
            Unit.Defence = Unit.Defence + 30;
            Unit.Name = Unit.Name + " with shield";
        }
        public override IUnit Copy()
        {
            return new ShieldDecorator(Unit.Copy());
        }
    }

    class PikeDecorator : HeavyUnitDecorator
    {
        public PikeDecorator(IUnit unit)
            : base(unit)
        {
            Unit.Attack += 20;
            Unit.Name += " with pike";
        }
        public override IUnit Copy()
        {
            return new PikeDecorator(Unit.Copy());
        }
    }

    class HelmetDecorator : HeavyUnitDecorator
    {
        public HelmetDecorator(IUnit unit)
            : base(unit)
        {
            Unit.Defence += 10;
            Unit.Name += " with helmet";
        }
        public override IUnit Copy()
        {
            return new HelmetDecorator(Unit.Copy());
        }
    }
}

