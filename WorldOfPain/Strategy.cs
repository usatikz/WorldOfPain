using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorldOfPain
{
    interface IStrategy
    {
        int rowSize { get; set; }
        List<IUnit> GetOutsideTargets(Army inside, Army outside, ISpecialAction unit);
        List<IUnit> GetInsideTargets(Army inside, ISpecialAction unit);
        string GetInfo(Army army);
    }
    class OneToOneStrategy : IStrategy
    {
        public int rowSize { get; set; } = 1;

        public string GetInfo(Army army)
        {
            var info = String.Format("Army {0}:", army.Name);
            for (int i = 1; i <= army.Count(); i++)
                info += String.Format("\n{0}. {1}", i, army[i - 1].GetInfo());
            return info;
        }
        public List<IUnit> GetOutsideTargets(Army first, Army second, ISpecialAction unit)
        {
            var targets = new List<IUnit>();
            int indexUnit = first.IndexOf((IUnit)unit);

            if (indexUnit >= first.Count() - unit.Range)
            {
                int targetsCount = unit.Range - (first.Count() - 1 - indexUnit);
                for (int i = 0; i < ((targetsCount > second.Count()) ? second.Count() : targetsCount); i++)
                    targets.Add(second[second.Count() - 1 - i]);
            }
            return targets;
        }

        public List<IUnit> GetInsideTargets(Army inside, ISpecialAction unit)
        {
            var targets = new List<IUnit>();
            int index = inside.IndexOf((IUnit)unit);
            int from = (index - unit.Range > 0) ? index - unit.Range : 0;
            int to = ((index + unit.Range + 1) > inside.Count()) ? inside.Count() : index + unit.Range + 1;
            for (int i = from; i < to; i++)
                targets.Add(inside[i]);
            return targets;
        }
    }
    class ThreeToThreeStrategy : XStrategy
    {

        public ThreeToThreeStrategy()
        {
            rowSize = 3;
        }


    }

    class NToNStrategy : XStrategy
    {
        public NToNStrategy(Army first, Army second)
        {
            rowSize = Math.Min(first.Count(), second.Count());
        }
    }

        class XStrategy : IStrategy
    {
        public int rowSize { get; set; }

        public string GetInfo(Army army)
        {
            var info = string.Format("Army {0}:", army.Name);
            for (int j = 0; j < rowSize; j++)
            {
                info += string.Format("\nRow {0}:", j + 1);
                for (int line = 1, i = army.Count() - j - 1; i >= 0; line++, i -= rowSize)
                    info += string.Format("\nLine {0}. {1}", line, army[i].GetInfo());
            }
            return info;
        }
        public List<IUnit> GetOutsideTargets(Army inside, Army outside, ISpecialAction unit)
        {
            var targets = new List<IUnit>();
            int row = (inside.Count() - inside.IndexOf((IUnit)unit) - 1) % rowSize;
            int line = (inside.Count() - inside.IndexOf((IUnit)unit) - 1) / rowSize;
            if (line >= unit.Range)
                return targets;
            for (int i = outside.Count() - 1 - row, targetsCount = unit.Range - line; i >= 0 && targetsCount > 0; i -= rowSize, targetsCount--)
                targets.Add(outside[i]);
            return targets;
        }

        public List<IUnit> GetInsideTargets(Army inside, ISpecialAction unit)
        {
            var targets = new List<IUnit>();
            int index = inside.IndexOf((IUnit)unit);

            int row = (inside.Count() - index - 1) % rowSize;
            int line = (inside.Count() - index - 1) / rowSize;

            for (int i = 0; i < inside.Count(); i++)
            {
                int r = (inside.Count() - i - 1) % rowSize;
                int l = (inside.Count() - i - 1) / rowSize;
                if (Math.Sqrt((r - row) * (r - row) + (l - line) * (l - line)) <= unit.Range)
                    targets.Add(inside[i]);
            }
            return targets;
        }
    }

}
